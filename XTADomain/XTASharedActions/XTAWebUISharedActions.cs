using Microsoft.Playwright;

namespace XTADomain.XTASharedActions;

public class XTAWebUISharedActions
{
    internal XTAWebUISharedActions() {}
    
    internal async Task ClickAsync(IPage in_page, string in_selector) 
        => await in_page.ClickAsync(in_selector);

    internal async Task FillTextAsync(IPage in_page, string in_selector, string in_text)
        => await in_page.Locator(in_selector).FillAsync(in_text);
}