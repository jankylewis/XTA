using XTADomain.XTAPageObjects.Abstractions;
using XTADomain.XTAPageObjects.POUtils;

namespace XTADomain.XTAPageObjects.CoreExperience;

internal class UserProfilePOs : APOs
{
    internal UserProfilePOs() {}
    
    internal String LBL_USER_DISPLAY_NAME(String in_displayName) =>
        m_ro_PO_SHARED_UTILS.BuildSelector($"//div[@data-testid='UserName']//span[text()='{in_displayName}']", ELocatingMechanism.CSS);
    
    internal String LBL_USERNAME(String in_username) =>
        m_ro_PO_SHARED_UTILS.BuildSelector($"//div[@data-testid='UserName']//span[text()='{in_username}']", ELocatingMechanism.XPATH);
}