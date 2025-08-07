namespace XTAInfras.XInfrasExceptions;

public abstract class XInfrasExceptions : Exception
{
    protected XInfrasExceptions() {}
    protected XInfrasExceptions(string in_message) : base(in_message) {}
    protected XInfrasExceptions(string in_message, Exception in_innerException) : base(in_message) {}
}

public class XPageNotInitializedException : XInfrasExceptions
{
    public XPageNotInitializedException() {}
    public XPageNotInitializedException(string in_message) : base(in_message) {}
    public XPageNotInitializedException(string in_message, Exception in_innerException) : base(in_message) {}
}

public class XBrowserContextNotInitializedException : XInfrasExceptions
{
    public XBrowserContextNotInitializedException() {}
    public XBrowserContextNotInitializedException(string in_message) : base(in_message) {}
    public XBrowserContextNotInitializedException(string in_message, Exception in_innerException) : base(in_message) {}
}

public class XTestMethodKeyGotEmptyException : XInfrasExceptions
{
    public XTestMethodKeyGotEmptyException() {}
    public XTestMethodKeyGotEmptyException(string in_message) : base(in_message) {}
    public XTestMethodKeyGotEmptyException(string in_message, Exception in_innerException) : base(in_message) {}
}

public class XBrowserExecutionNotSupported : XInfrasExceptions
{
    public XBrowserExecutionNotSupported() {}
    public XBrowserExecutionNotSupported(string in_message) : base(in_message) {}
    public XBrowserExecutionNotSupported(string in_message, Exception in_innerException) : base(in_message) {}
}

public class XLocatingMechanismNotSupported : XInfrasExceptions
{
    public XLocatingMechanismNotSupported() {}
    public XLocatingMechanismNotSupported(string in_message) : base(in_message) {}
    public XLocatingMechanismNotSupported(string in_message, Exception in_innerException) : base(in_message) {}
}

public class XZetaNotQualifiedException : XInfrasExceptions
{
    public XZetaNotQualifiedException() {}
    public XZetaNotQualifiedException(string in_message) : base(in_message) {}
    public XZetaNotQualifiedException(string in_message, Exception in_innerException) : base(in_message) {}
}

public class XSigmaNotQualifiedException : XInfrasExceptions
{
    public XSigmaNotQualifiedException() {}
    public XSigmaNotQualifiedException(string in_message) : base(in_message) {}
    public XSigmaNotQualifiedException(string in_message, Exception in_innerException) : base(in_message) {}
}

public class XTestNotSupportedUponHeadlessModeException : XInfrasExceptions
{
    public XTestNotSupportedUponHeadlessModeException() {}
    public XTestNotSupportedUponHeadlessModeException(string in_message) : base(in_message) {}
    public XTestNotSupportedUponHeadlessModeException(string in_message, Exception in_innerException) : base(in_message) {}   
}

public class XTestBusinessFlowException : XInfrasExceptions
{
    public XTestBusinessFlowException() {}
    public XTestBusinessFlowException(string in_message) : base(in_message) {}
    public XTestBusinessFlowException(string in_message, Exception in_innerException) : base(in_message) {}
}