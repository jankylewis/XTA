namespace XTAInfras.XConfFactories.XConfModels;

public class XPlwConfModel
{
    public string BrowserType { get; set; }
    public bool Headed { get; set; }
    public int SlowMo { get; set; }
    public int Timeout { get; set; }
    public ViewportSize ViewportSize { get; set; }
}

public class ViewportSize
{
    public int Width { get; set; }
    public int Height { get; set; }
}