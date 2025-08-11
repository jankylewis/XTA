using Microsoft.Playwright;
using XTAInfras.XInfrasConstHouse;

namespace XTADomain.XTASharedActions;

public class XTANavigationKit
{
    public XTANavigationKit() {}

    public async Task NavigateToURLAsync(
        IPage in_xPage, string in_xURL, PageGotoOptions? in_xPageGoToOptions = default)
            => await in_xPage.GotoAsync(in_xURL, in_xPageGoToOptions ?? new PageGotoOptions
            { 
                WaitUntil = WaitUntilState.Load,
                Timeout = XTimedoutConsts.NAVIGATION_TIMEOUT_MS
            });
}