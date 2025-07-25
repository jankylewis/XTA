using XTADomain.XTAPageObjects.XPOAbstractions;
using XTADomain.XTAPageObjects.XPOUtils;

namespace XTADomain.XTAPageObjects.XCoreExperience;

internal class XUserProfilePOs : AXPOs
{
    internal XUserProfilePOs() {}
    
    internal String LBL_USER_DISPLAY_NAME(String in_xDisplayName) 
        => p_xPOSharedUtils.BuildSelector($"//div[@data-testid='UserName']//span[text()='{in_xDisplayName}']", ELocatingMechanism.CSS);
    
    internal String LBL_USERNAME(String in_xUsername) 
        => p_xPOSharedUtils.BuildSelector($"//div[@data-testid='UserName']//span[text()='{in_xUsername}']", ELocatingMechanism.XPATH);
}