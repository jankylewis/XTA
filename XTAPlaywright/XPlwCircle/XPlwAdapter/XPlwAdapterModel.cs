using System.Collections.Concurrent;
using Microsoft.Playwright;

namespace XTAPlaywright.XPlwCircle.XPlwAdapter;

public class XPlwAdapterModel
{
    public ConcurrentDictionary<string, IBrowserContext> XBrowserContexts { get; set; }
    public ConcurrentDictionary<string, IPage> XPages { get; set; }
}