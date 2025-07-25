using Microsoft.Playwright;
using XTADomain.XTABusinesses.XBusinessAbstractions;
using XTADomain.XTAPageObjects.XCoreExperience;

namespace XTADomain.XTABusinesses.XCoreExperience;

public class XUserProfilePage : AXPage
{
    #region Introduce constructors

    public XUserProfilePage(IPage in_xPage) => p_xPage = in_xPage;

    #endregion Introduce constructors

    #region Introduce class vars

    private readonly XUserProfilePOs mr_xUserProfilePOs = new();

    #endregion Introduce class vars

    #region Introduce actions

    public async Task VerifyDisplayNameShownCorrectlyAsync(String in_expectedXDisplayName)
    {
        await pr_xtaWebUIWaitStrategies
            .WaitForElementToBeVisibleAsync(p_xPage, mr_xUserProfilePOs.LBL_USER_DISPLAY_NAME(in_expectedXDisplayName));
        
        await pr_xtaWebUISharedVerifiers
            .VerifyIfElementIsVisibleWithoutWaitsAsync(p_xPage, mr_xUserProfilePOs.LBL_USER_DISPLAY_NAME(in_expectedXDisplayName));
    }

    public async Task VerifyUsernameShownCorrectlyAsync(String in_expectedXUsername) 
        => await pr_xtaWebUISharedVerifiers
            .VerifyIfElementIsVisibleWithoutWaitsAsync(p_xPage, mr_xUserProfilePOs.LBL_USERNAME(in_expectedXUsername));
    
    #endregion Introduce actions
}