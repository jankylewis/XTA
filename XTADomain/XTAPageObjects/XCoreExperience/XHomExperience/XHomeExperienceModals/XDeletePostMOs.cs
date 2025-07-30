using XTACore.XTAUtils;
using XTADomain.XTAPageObjects.XPOUtils;

namespace XTADomain.XTAPageObjects.XCoreExperience.XHomExperience.XHomeExperienceModals;

internal class XDeletePostMOs
{
    public XDeletePostMOs() {}
    
    private readonly XPOSharedUtils mr_xPOSharedUtils = XSingletonFactory.s_DaVinciResolve<XPOSharedUtils>();

    internal String BTN_DELETE_POST
        => mr_xPOSharedUtils.BuildSelector("//div[span[text()='Delete']]", ELocatingMechanism.XPATH);
}