using Microsoft.Playwright;
using XTACore.XCoreUtils;
using XTADomain.XTAPageObjects.XOffboardingExperience.XOffboardingExperienceModals;
using XTADomain.XTASharedActions;

namespace XTADomain.XTABusinesses.XOffboardingExperience.XOffboardingExperienceModals;

public class XLogOutOfXModal(IPage in_xPage)
{
    private readonly XLogOutOfXMOs mr_xLogOutOfXMOs = XSingletonFactory.s_DaVinci<XLogOutOfXMOs>();

    public async Task ClickOnLogOutBtnAsync()
        => await XSingletonFactory.s_DaVinci<XTAWebUISharedActions>().ClickAsync(in_xPage, mr_xLogOutOfXMOs.BTN_LOGOUT);
}