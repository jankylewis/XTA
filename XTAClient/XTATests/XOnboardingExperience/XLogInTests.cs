
#region Import statements

using XTAClient.XTATests.XTATestFoundation;
using XTADomain.XTABusinesses.XCoreExperience;
using XTADomain.XTABusinesses.XOnboardingExperience;
using XTADomain.XTABusinesses.XOnboardingExperience.XModals;
using XTADomain.XTAModels;

#endregion Import statements

namespace XTAClient.XTATests.XOnboardingExperience;

#region XLogInTests > Class level

[Parallelizable(ParallelScope.Children)]
[TestFixture]
internal class XLogInTests : AXTATestFoundation
{
    #region Introduce X Log In tests
    
    [Test]
    public async Task NavigateToXLogInPage_InputCorrectCredentials_BeingLandedOnHomePage()
    {
        XAccountModel xAccountModel = new()
        {
            XDisplayName = ps_xAppConfModel.XDisplayName,
            XUsername = ps_xAppConfModel.XUsername,
            XPassword = ps_xAppConfModel.XPassword,
        };
        
        XSignInToXModal xSignInToXModal = new(p_xPage);
        XEnterYourPasswordModal xEnterYourPasswordModal = new(p_xPage);   
        XUserProfilePage xUserProfilePage = new(p_xPage);
        
        await new XLogInPage(p_xPage).ClickOnSignInBtnAsync();
        
        await xSignInToXModal.InputUsernameAsync(xAccountModel.XUsername);
        await xSignInToXModal.ClickOnNextBtnAsync();

        await xEnterYourPasswordModal.InputPasswordAsync(xAccountModel.XPassword);
        await xEnterYourPasswordModal.ClickOnLogInBtnAsync();
        
        await new XHomePage(p_xPage).ClickOnProfileNavAsync();
        
        await xUserProfilePage.VerifyDisplayNameShownCorrectlyAsync(xAccountModel.XDisplayName);
        await xUserProfilePage.VerifyUsernameShownCorrectlyAsync("@" + xAccountModel.XUsername);
    }
    
    [Test]
    public async Task NavigateToXLogInPage_ClickOnSignInButton_VerifySignInToXModalDisplayed()
    {
        await new XLogInPage(p_xPage).ClickOnSignInBtnAsync();

        await new XSignInToXModal(p_xPage).VerifyIfSignInToXModalDisplayedAsync();
    }
    
    #endregion Introduce X Log In tests

    #region Introduce NUnit SetUp phase
    
    [OneTimeSetUp]
    public async Task XMetaSetUp() 
        => ps_xtaNavigationKit ??= new();

    [SetUp]
    public async Task XMegaSetUp()
        => await ps_xtaNavigationKit.NavigateToURLAsync(p_xPage, ps_xAppConfModel.BaseXURL);
    
    #endregion Introduce NUnit SetUp phase
}

#endregion XLogInTests > Class level