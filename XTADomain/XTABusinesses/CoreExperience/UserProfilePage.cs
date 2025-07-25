using Microsoft.Playwright;
using XTADomain.XTABusinesses.Abstractions;
using XTADomain.XTAPageObjects.CoreExperience;

namespace XTADomain.XTABusinesses.CoreExperience;

public class UserProfilePage : AXPage
{
    #region Introduce constructors

    public UserProfilePage(IPage in_page) => prot_page = in_page;

    #endregion Introduce constructors

    #region Introduce class vars

    private readonly UserProfilePOs m_ro_USER_PROFILE_POS = new();

    #endregion Introduce class vars

    #region Introduce actions

    public async Task VerifyDisplayNameShownCorrectly(String in_expectedDisplayName)
    {
        await prot_ro_XTA_WEBUI_WAIT_STRATEGIES
            .WaitForElementToBeVisible(prot_page, m_ro_USER_PROFILE_POS.LBL_USER_DISPLAY_NAME(in_expectedDisplayName));
        
        await prot_ro_XTA_WEBUI_SHARED_VERIFIERS
            .VerifyIfElementIsVisibleWithoutWaits(prot_page, m_ro_USER_PROFILE_POS.LBL_USER_DISPLAY_NAME(in_expectedDisplayName));
    }

    public async Task VerifyUsernameShownCorrectly(String in_expectedUsername) =>
        await prot_ro_XTA_WEBUI_SHARED_VERIFIERS
            .VerifyIfElementIsVisibleWithoutWaits(prot_page, m_ro_USER_PROFILE_POS.LBL_USERNAME(in_expectedUsername));
    
    #endregion Introduce actions
}