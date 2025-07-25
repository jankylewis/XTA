using Microsoft.Playwright;

namespace XTADomain.XTASharedActions;

public class XTANavigationKit
{
    public XTANavigationKit() {}

    public async Task NavigateToURL(IPage in_xPage, string in_xURL, PageGotoOptions? in_xPageGoToOptions = default)
        => await in_xPage.GotoAsync(in_xURL, in_xPageGoToOptions);
}