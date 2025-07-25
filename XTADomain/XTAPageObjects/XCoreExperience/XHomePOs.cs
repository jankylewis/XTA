using XTADomain.XTAPageObjects.XPOAbstractions;
using XTADomain.XTAPageObjects.XPOUtils;

namespace XTADomain.XTAPageObjects.XCoreExperience;

internal class XHomePOs : AXPOs
{
    internal XHomePOs() {}

    internal String BTN_ACCOUNT_MENU 
        => p_xPOSharedUtils.BuildSelector("button[data-testid='SideNav_AccountSwitcher_Button']", ELocatingMechanism.CSS);
}

