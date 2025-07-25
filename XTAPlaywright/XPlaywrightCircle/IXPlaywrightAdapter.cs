using System.Collections.Concurrent;
using Microsoft.Playwright;

namespace XTAPlaywright.XPlaywrightCircle;

internal interface IXPlaywrightAdapter
{
    internal IPlaywright? XPlaywright { get; set; }
    internal IBrowser? XBrowser { get; set; }
    internal IBrowserContext? XBrowserContext { get; set; }
    internal IPage? XPage { get; set; }
    
    internal ConcurrentDictionary<string, IBrowserContext> XBrowserContexts { get; }
    internal ConcurrentDictionary<string, IPage> XPages { get; }
}

