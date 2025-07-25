using Microsoft.Playwright;
using XTADomain.XTABusinesses.XCoreExperience;
using XTADomain.XTABusinesses.XOnboardingExperience;
using XTADomain.XTABusinesses.XOnboardingExperience.XModals;

namespace XTAClient.XTATests.XOnboardingExperience;

[Parallelizable(ParallelScope.Children)]
[TestFixture]
internal class XLogInTests : AXTATestFoundation
{
    [Test]
    public async Task NavigateToXLogInPage_InputCorrectCredentials_BeingLandedOnHomePage()
    {
        await p_xPage.GotoAsync(ps_xAppConfModel.BaseXURL, new PageGotoOptions()
        {
            WaitUntil = WaitUntilState.Load,
            Timeout = 30000
        });

        await new XLogInPage(p_xPage).ClickOnSignInBtnAsync();

        XSignInToXModal xSignInToXModal = new(p_xPage);
        XEnterYourPasswordModal xEnterYourPasswordModal = new(p_xPage);
        
        await xSignInToXModal.InputUsernameAsync(ps_xAppConfModel.XUsername);
        await xSignInToXModal.ClickOnNextBtnAsync();

        await xEnterYourPasswordModal.InputPasswordAsync(ps_xAppConfModel.XPassword);
        await xEnterYourPasswordModal.ClickOnLogInBtnAsync();

        await new XHomePage(p_xPage).ClickOnProfileNavAsync();

        XUserProfilePage xUserProfilePage = new(p_xPage);

        await xUserProfilePage.VerifyDisplayNameShownCorrectlyAsync(ps_xAppConfModel.XDisplayName);
        await xUserProfilePage.VerifyUsernameShownCorrectlyAsync("@" + ps_xAppConfModel.XUsername);;
    }
    
    [Test]
    public async Task NavigateToXLogInPage_ClickOnSignInButton_VerifySignInToXModalDisplayed()
    {
        await p_xPage.GotoAsync(ps_xAppConfModel.BaseXURL, new PageGotoOptions
        {
            WaitUntil = WaitUntilState.Load,
            Timeout = 30000
        });

        await new XLogInPage(p_xPage).ClickOnSignInBtnAsync();

        await new XSignInToXModal(p_xPage).VerifyIfSignInToXModalDisplayedAsync();
    }
}