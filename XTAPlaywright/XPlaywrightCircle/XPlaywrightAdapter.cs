using System.Collections.Concurrent;
using Microsoft.Playwright;
using XTAPlaywright.XTestCircle;

namespace XTAPlaywright.XPlaywrightCircle;

public class XPlaywrightAdapter : IXPlaywrightAdapter
{
    public XPlaywrightAdapter() {}
    
    public IPlaywright? XPlaywright { get; set; }
    public IBrowser? XBrowser { get; set; }
    public IBrowserContext? XBrowserContext { get; set; }
    public IPage? XPage { get; set; }

    public ConcurrentDictionary<string, IBrowserContext>? XBrowserContexts { get; set; }
    public ConcurrentDictionary<string, IPage>? XPages { get; set; }

    internal XPlaywrightAdapter InitPlaywrightAdapter(
        XPlaywrightAdapter in_xPlaywrightAdapter, IXTestAdapter in_xTestAdapter)
    {
        in_xPlaywrightAdapter.XBrowserContexts = _InitXBrowserContexts(in_xTestAdapter, in_xPlaywrightAdapter.XBrowserContext);
        in_xPlaywrightAdapter.XPages = _InitXPages(in_xTestAdapter, in_xPlaywrightAdapter.XPage);
        
        return in_xPlaywrightAdapter;
    }
    
    private ConcurrentDictionary<string, IBrowserContext> _InitXBrowserContexts(
        IXTestAdapter in_xTestAdapter, IBrowserContext in_xBrowserContext)
    {
        XBrowserContexts[in_xTestAdapter.XTestMetaKey] = in_xBrowserContext;
        XBrowserContexts[in_xTestAdapter.XTestCorrelationID] = in_xBrowserContext;
        
        return XBrowserContexts;
    }
    
    private ConcurrentDictionary<string, IPage> _InitXPages(IXTestAdapter in_xTestAdapter, IPage in_xPage)
    {
        XPages[in_xTestAdapter.XTestMetaKey] = in_xPage;
        XPages[in_xTestAdapter.XTestCorrelationID] = in_xPage;
        
        return XPages;
    }
}