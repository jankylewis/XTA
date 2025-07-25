using Microsoft.Playwright;
using XTAClient.XTATests.Abstractions;
using XTADomain.XTABusinesses.CoreExperience;
using XTADomain.XTABusinesses.OnboardingExperience;
using XTADomain.XTABusinesses.OnboardingExperience.Modals;
using XTADomain.XTASharedActions;
using XTAPlaywright.XConstHouse;

namespace XTAClient.XTATests.OnboardingExperience;

[Parallelizable(ParallelScope.Children)]
[TestFixture]
internal class LogInTests : AXTATestFoundation
{
    private readonly XTANavigationKit m_XTA_NAVIGATION_KIT = new();
    
    [Test]
    public async Task NavigateToXLogInPage_InputCorrectCredentials_BeingLandedOnHomePage()
    {
        await prot_xPage.GotoAsync(prot_xAppConfModel.BaseXURL, new PageGotoOptions
        {
            WaitUntil = WaitUntilState.Load,
            Timeout = XConsts.NAVIGATION_TIMEOUT_MS
        });
        
        // await m_XTA_NAVIGATION_KIT.NavigateToURL(prot_xPage, prot_xAppConfModel.BaseXURL, new PageGotoOptions
        // {
        //     WaitUntil = WaitUntilState.Load,
        //     Timeout = XConsts.NAVIGATION_TIMEOUT_MS
        // });

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
        await userProfilePage.VerifyUsernameShownCorrectly("@" + prot_xAppConfModel.XUsername);
    }
    
    [Test]
    public async Task NavigateToXLogInPage_ClickOnSignInButton_VerifySignInToXModalDisplayed()
    { 
        await prot_xPage.GotoAsync(prot_xAppConfModel.BaseXURL, new PageGotoOptions
        {
            WaitUntil = WaitUntilState.Load,
            Timeout = XConsts.NAVIGATION_TIMEOUT_MS
        });
        
        // await m_XTA_NAVIGATION_KIT.NavigateToURL(prot_xPage, prot_xAppConfModel.BaseXURL, new PageGotoOptions
        // {
        //     WaitUntil = WaitUntilState.Load,
        //     Timeout = XConsts.NAVIGATION_TIMEOUT_MS
        // });

        await new LogInPage(prot_xPage).ClickOnSignInButton();

        await new SignInToXModal(prot_xPage).VerifyIfSignInToXModalDisplayed();
    }

    [OneTimeTearDown]
    public static async Task LogInMetaTearDown()
    {
        foreach (var k in m_xPlaywrightAdapter.XBrowserContexts.Keys)
        {
            Console.WriteLine("META TEAR DOWN: BC KEY: " + k);    
        }
        
        foreach (var v in m_xPlaywrightAdapter.XBrowserContexts.Values)
        {
            Console.WriteLine("META TEAR DOWN: BC VALUE: " + v);    
        }
        
        // Console.WriteLine("META TEAR DOWN: BC VALUES: " + m_xPlaywrightAdapter.XBrowserContexts.Values);
    }
}