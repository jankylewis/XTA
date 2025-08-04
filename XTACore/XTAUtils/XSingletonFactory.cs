using System.Collections.Concurrent;

namespace XTACore.XTAUtils;

public static class XSingletonFactory
{
    #region Introduce class vars
    
    private static readonly ConcurrentDictionary<Type, Lazy<object>> msr_xSingletonServices = new();
    
    #endregion Introduce class vars

    #region Introduce global singleton services
    
    public static void s_Register<XService>() where XService : new() 
        => msr_xSingletonServices.TryAdd(typeof(XService), new Lazy<object>(() => new XService(), LazyThreadSafetyMode.ExecutionAndPublication));
    
    public static void s_Register<XService>(XService in_xInstance) 
        => msr_xSingletonServices.TryAdd(typeof(XService), new Lazy<object>(() => in_xInstance!, LazyThreadSafetyMode.ExecutionAndPublication));
    
    public static XService s_Retrieve<XService>()
    {
        if (msr_xSingletonServices.TryGetValue(typeof(XService), out var a_lazyInitExec))
            return (XService) a_lazyInitExec.Value;

        throw new InvalidOperationException($"Type {typeof(XService).FullName} has not been registered in the X Singleton Pool.      ");
    }
    
    public static XService s_DaVinci<XService>() where XService : new()
    {
        Lazy<object> lazyInitExec = msr_xSingletonServices.GetOrAdd(
            typeof(XService), _ => new Lazy<object>(() => new XService(), LazyThreadSafetyMode.ExecutionAndPublication));

        return (XService)lazyInitExec.Value;
    }

    #endregion Introduce global singleton services

    #region Introduce global service disposal
    
    public static void s_DisposeAll()
    {
        foreach (Lazy<object> l_lazyInit in msr_xSingletonServices.Values)
        {
            if (!l_lazyInit.IsValueCreated) 
                continue;

            if (l_lazyInit.Value is IDisposable a_disposableLazyInit)
            {
                try { a_disposableLazyInit.Dispose(); }
                catch {}
            }
        }

        msr_xSingletonServices.Clear();
    }
    
    #endregion Introduce global service disposal
}