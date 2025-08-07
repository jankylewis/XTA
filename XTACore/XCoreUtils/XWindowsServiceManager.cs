using System.ServiceProcess;
using XTACore.XCoreExceptions;

namespace XTACore.XCoreUtils;

public class XWindowsServiceManager
{
    public XWindowsServiceManager(string in_xServiceName) 
        => ms_xServiceController = new ServiceController(in_xServiceName);
    
    private readonly ServiceController ms_xServiceController;
    private readonly TimeSpan ms_timeout = TimeSpan.FromSeconds(60);
    
    public async Task EnsureServiceIsRunningAsync()
    {
        if (ms_xServiceController.Status != ServiceControllerStatus.Running)
            await m_StartServiceAsync();
    }

    private async Task m_StartServiceAsync()
    {
        try
        {
            if (ms_xServiceController.Status is ServiceControllerStatus.Stopped or ServiceControllerStatus.Paused)
            {
                ms_xServiceController.Start();
                
                await Task.Run(() => ms_xServiceController.WaitForStatus(ServiceControllerStatus.Running, ms_timeout));
            }
        }
        catch (Exception a_ex)
        {
            throw new XFailedToStartWindowsServiceException(
                $"An error occurred while trying to start the service '{ms_xServiceController.ServiceName}'.    ", a_ex);
        }
    }

    public async Task StopServiceAsync()
    {
        try
        {
            if (ms_xServiceController.Status is ServiceControllerStatus.Running or ServiceControllerStatus.Paused)
            {
                ms_xServiceController.Stop();
         
                await Task.Run(() => ms_xServiceController.WaitForStatus(ServiceControllerStatus.Stopped, ms_timeout));
            }
        }
        catch (Exception a_ex)
        {
            throw new XFailedToStopWindowsServiceException(
                $"An error occurred while trying to stop the service '{ms_xServiceController.ServiceName}'.     ", a_ex);
        }
    }
}

