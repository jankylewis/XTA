using System.Security.Principal;
namespace XTACore.XCoreUtils.XOSUtils;

public class XWindowsUtils
{
    public XWindowsUtils() {}
    
    public bool WhetherRunningAsAdmin()
    {
        if (!OperatingSystem.IsWindows())
            throw new PlatformNotSupportedException("This function is only supported on Windows unfortunately.    ");

        using WindowsIdentity? windowsIdentity = WindowsIdentity.GetCurrent();
        
        if (windowsIdentity is null)
            return false;

        WindowsPrincipal windowsPrincipal = new WindowsPrincipal(windowsIdentity);
        
        return windowsPrincipal.IsInRole(WindowsBuiltInRole.Administrator);
    }
}