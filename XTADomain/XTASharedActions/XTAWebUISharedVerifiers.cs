using FluentAssertions;
using Microsoft.Playwright;

namespace XTADomain.XTASharedActions;

public class XTAWebUISharedVerifiers
{
    internal XTAWebUISharedVerifiers() {}
    
    internal async Task VerifyIfTextContentMatchedAsync(IPage in_page, string in_selector, string in_expectedText) 
        => (await in_page.TextContentAsync(in_selector)).Should().Be(in_expectedText);

    internal async Task VerifyIfElementIsVisibleWithoutWaitsAsync(IPage in_page, string in_selector) 
        => (await in_page.IsVisibleAsync(in_selector)).Should().Be(true);

    internal async Task VerifyIfElementIsVisibleWithWaitsAsync(IPage in_page, string in_selector) 
        => await Expect(in_page.Locator(in_selector)).ToBeVisibleAsync();
}