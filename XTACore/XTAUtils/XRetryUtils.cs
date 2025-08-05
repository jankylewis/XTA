using System.Runtime.ExceptionServices;

namespace XTACore.XTAUtils;

public class XRetryUtils
{
    public XRetryUtils() {}

    public async Task RetryAsync(
        Func<Task> in_xActionFunc,
        Func<bool> in_conditionToRetry,
        int in_attempts = 2,
        int in_delayMs = 0
        )
    {
        if (in_attempts < 1)
            throw new ArgumentOutOfRangeException(nameof(in_attempts), "Number of attempts must be ≥ 1      ");

        for (int l_tryNo = 1; l_tryNo <= in_attempts; l_tryNo++)
        {
            await in_xActionFunc().ConfigureAwait(false);

            // When the condition tells us no retry is needed, finish immediately
            if (!in_conditionToRetry())
                return;

            // If we have more retries left, wait before the next iteration (if a delay is requested)
            if (l_tryNo < in_attempts && in_delayMs > 0)
                await Task.Delay(in_delayMs).ConfigureAwait(false);
        }

        // If we reach this point the condition is still true after all attempts
        throw new InvalidOperationException($"Retry condition was still met after {in_attempts} retry attempts. Please have investigations on it!   ");
    }
    
    public async Task RetryAsync(
        Func<Task> in_xActionFunc,
        Func<Exception, bool>? in_isRetryable = null,
        int in_attempts = 2,
        int in_delayMs = 0
        )
    {
        if (in_attempts < 1)
            throw new ArgumentOutOfRangeException(nameof(in_attempts), "number of attempts must be ≥ 1      ");
        
        Exception? last = null;

        for (int l_tryNo = 1; l_tryNo <= in_attempts; l_tryNo++)
        {
            try
            {
                await in_xActionFunc().ConfigureAwait(false);
                return;             // Task goes successful
            }
            catch (Exception a_ex) when (l_tryNo < in_attempts && (in_isRetryable?.Invoke(a_ex) ?? true))
            {
                last = a_ex;
                
                if (in_delayMs > 0)
                    await Task.Delay(in_delayMs);
            }
        }
        
        // Re-throw the final exception if Task goes failed
        ExceptionDispatchInfo.Capture(last!).Throw();
    }
}