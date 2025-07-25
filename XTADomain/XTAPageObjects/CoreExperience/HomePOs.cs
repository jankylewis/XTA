using XTADomain.XTAPageObjects.Abstractions;
using XTADomain.XTAPageObjects.POUtils;

namespace XTADomain.XTAPageObjects.CoreExperience;

internal class HomePOs : APOs
{
    internal HomePOs() {}

    internal String BTN_ACCOUNT_MENU =>
        m_ro_PO_SHARED_UTILS.BuildSelector("button[data-testid='SideNav_AccountSwitcher_Button']", ELocatingMechanism.CSS);
}

