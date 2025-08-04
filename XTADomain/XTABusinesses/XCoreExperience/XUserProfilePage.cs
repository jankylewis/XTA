using Microsoft.Playwright;
using XTACore.XTAUtils;
using XTADomain.XTABusinesses.XBusinessAbstractions;
using XTADomain.XTAPageObjects.XCoreExperience;

namespace XTADomain.XTABusinesses.XCoreExperience;

public class XUserProfilePage(IPage in_xPage) 
    : AXPage<XUserProfilePOs>(in_xPage, XSingletonFactory.s_DaVinci<XUserProfilePOs>())
{
    #region Introduce class vars

    private readonly XUserProfilePOs mr_xUserProfilePOs = XSingletonFactory.s_Retrieve<XUserProfilePOs>();

    #endregion Introduce class vars

    #region Introduce verifications

    public async Task VerifyDisplayNameShownCorrectlyAsync(String in_expectedXDisplayName)
    {
        await pr_xtaWebUIWaitStrategies
            .WaitForElementToBeVisibleAsync(pr_xPage, mr_xUserProfilePOs.LBL_USER_DISPLAY_NAME(in_expectedXDisplayName));
        
        await pr_xtaWebUISharedVerifiers
            .VerifyIfElementIsVisibleWithoutWaitsAsync(pr_xPage, mr_xUserProfilePOs.LBL_USER_DISPLAY_NAME(in_expectedXDisplayName));
    }

    public async Task VerifyUsernameShownCorrectlyAsync(String in_expectedXUsername) 
        => await pr_xtaWebUISharedVerifiers
            .VerifyIfElementIsVisibleWithoutWaitsAsync(pr_xPage, mr_xUserProfilePOs.LBL_USERNAME(in_expectedXUsername));

    public async Task VerifyAnXPostSuccessfullyCreated()
    {
        
    }
    
    #endregion Introduce verifications
    
    
}