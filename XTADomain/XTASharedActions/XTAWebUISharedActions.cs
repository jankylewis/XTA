using Microsoft.Playwright;

namespace XTADomain.XTASharedActions;

public class XTAWebUISharedActions
{
    public XTAWebUISharedActions() {}
    
    internal async Task ClickAsync(IPage in_xPage, string in_selector) 
        => await in_xPage.ClickAsync(in_selector);
    
    internal async Task ClickAsync(ILocator in_selector) 
        => await in_selector.ClickAsync();

    internal async Task FillTextAsync(IPage in_xPage, string in_selector, string in_text)
        => await in_xPage.Locator(in_selector).FillAsync(in_text);

    internal async Task<string> GetTextContentAsync(IPage in_xPage, string in_selector)
        => await in_xPage.Locator(in_selector).TextContentAsync() ?? throw new NullReferenceException("TextContent got null. Please have a check!        ");

    internal async Task<List<ILocator>> GetListOfLocatorsAsync(IPage in_xPage, string in_selector)
    {
        IReadOnlyList<ILocator> selectors = await in_xPage.Locator(in_selector).AllAsync();
        return selectors.ToList();
    }
    
    internal async Task ReloadPageAsync(IPage in_xPage, PageReloadOptions? in_pageReloadOptions = default)
        => await in_xPage.ReloadAsync(in_pageReloadOptions);
}