using XTACore.XCoreUtils;
using XTADomain.XTAPageObjects.XPOUtils;

namespace XTADomain.XTAPageObjects.XOffboardingExperience.XOffboardingExperienceModals;

public class XLogOutOfXMOs
{
    public XLogOutOfXMOs() {}
    
    private readonly XPOSharedUtils mr_xPOSharedUtils = XSingletonFactory.s_DaVinci<XPOSharedUtils>();

    internal String BTN_LOGOUT
        => mr_xPOSharedUtils.BuildSelector("button[data-testid='confirmationSheetConfirm']", ELocatingMechanism.CSS);
}