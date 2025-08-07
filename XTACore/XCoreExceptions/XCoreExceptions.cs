namespace XTACore.XCoreExceptions;

public abstract class XCoreExceptions : Exception
{
    protected XCoreExceptions() {}
    protected XCoreExceptions(string in_message) : base(in_message) {}
    protected XCoreExceptions(string in_message, Exception in_innerException) : base(in_message) {}
}

public class XFailedToStartWindowsServiceException : XCoreExceptions
{
    public XFailedToStartWindowsServiceException() {}
    public XFailedToStartWindowsServiceException(string in_message) : base(in_message) {}
    public XFailedToStartWindowsServiceException(string in_message, Exception in_innerException) : base(in_message) {}
}

public class XFailedToStopWindowsServiceException : XCoreExceptions
{
    public XFailedToStopWindowsServiceException() {}
    public XFailedToStopWindowsServiceException(string in_message) : base(in_message) {}
    public XFailedToStopWindowsServiceException(string in_message, Exception in_innerException) : base(in_message) {}
}