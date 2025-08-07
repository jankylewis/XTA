using XTACore.XCoreUtils;
using XTADomain.XTAPageObjects.XPOUtils;

namespace XTADomain.XTAPageObjects.XCoreExperience.XHomExperience.XHomeExperienceModals;

internal class XDeletePostMOs
{
    public XDeletePostMOs() {}
    
    private readonly XPOSharedUtils mr_xPOSharedUtils = XSingletonFactory.s_DaVinci<XPOSharedUtils>();

    internal String BTN_DELETE_POST
        => mr_xPOSharedUtils.BuildSelector("//span[text()='Delete']", ELocatingMechanism.XPATH);
}