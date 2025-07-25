using Microsoft.Playwright;
using XTADomain.XTABusinesses.CoreExperience;
using XTADomain.XTABusinesses.OnboardingExperience;
using XTADomain.XTABusinesses.OnboardingExperience.Modals;

namespace XTAClient._XTATests;

[Parallelizable(ParallelScope.Children)]
[TestFixture]
internal class _LogInTests : AXTATestFoundation
{
    [Test]
    public async Task _v()
    {
        await prot_xPage.GotoAsync(prot_xAppConfModel.BaseXURL, new PageGotoOptions()
        {
            WaitUntil = WaitUntilState.Load,
            Timeout = 30000
        });

        await new LogInPage(prot_xPage).ClickOnSignInButton();

        SignInToXModal signInToXModal = new(prot_xPage);
        EnterYourPasswordModal enterYourPasswordModal = new(prot_xPage);
        
        await signInToXModal.InputUsername(prot_xAppConfModel.XUsername);
        await signInToXModal.ClickOnNextButton();

        await enterYourPasswordModal.InputPassword(prot_xAppConfModel.XPassword);
        await enterYourPasswordModal.ClickOnLogInButton();

        await new HomePage(prot_xPage).ClickOnProfileNav();

        UserProfilePage userProfilePage = new(prot_xPage);

        await userProfilePage.VerifyDisplayNameShownCorrectly(prot_xAppConfModel.XDisplayName);
        await userProfilePage.VerifyUsernameShownCorrectly("@" + prot_xAppConfModel.XUsername);;
    }
    
    [Test]
    public async Task _v2()
    {
        await prot_xPage.GotoAsync(prot_xAppConfModel.BaseXURL, new PageGotoOptions
        {
            WaitUntil = WaitUntilState.Load,
            Timeout = 30000
        });

        await new LogInPage(prot_xPage).ClickOnSignInButton();

        await new SignInToXModal(prot_xPage).VerifyIfSignInToXModalDisplayed();
    }
}