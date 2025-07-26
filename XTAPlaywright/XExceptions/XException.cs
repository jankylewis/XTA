namespace XTAPlaywright.XExceptions;

public abstract class XException : Exception
{
    protected XException() {}
    protected XException(string in_message) : base(in_message) {}
    protected XException(string in_message, Exception in_innerException) : base(in_message) {}
}

public class XPageNotInitializedException : XException
{
    public XPageNotInitializedException() {}
    public XPageNotInitializedException(string in_message) : base(in_message) {}
    public XPageNotInitializedException(string in_message, Exception in_innerException) : base(in_message) {}
}

public class XBrowserContextNotInitializedException : XException
{
    public XBrowserContextNotInitializedException() {}
    public XBrowserContextNotInitializedException(string in_message) : base(in_message) {}
    public XBrowserContextNotInitializedException(string in_message, Exception in_innerException) : base(in_message) {}
}

public class XTestMethodKeyGotEmptyException : XException
{
    public XTestMethodKeyGotEmptyException() {}
    public XTestMethodKeyGotEmptyException(string in_message) : base(in_message) {}
    public XTestMethodKeyGotEmptyException(string in_message, Exception in_innerException) : base(in_message) {}
}

public class XBrowserExecutionNotSupported : XException
{
    public XBrowserExecutionNotSupported() {}
    public XBrowserExecutionNotSupported(string in_message) : base(in_message) {}
    public XBrowserExecutionNotSupported(string in_message, Exception in_innerException) : base(in_message) {}   
}