
#region Import statements

using Microsoft.Playwright;
using XTAClient.XTATests.XTATestFoundation;
using XTACore.XCoreUtils;
using XTADomain.XTABusinesses.XCoreExperience;
using XTADomain.XTABusinesses.XCoreExperience.XHomeExperience;
using XTADomain.XTABusinesses.XOnboardingExperience;
using XTADomain.XTABusinesses.XOnboardingExperience.XOnboardingExperienceModals;
using XTADomain.XTAModels;
using XTADomain.XTASharedActions;
using XTAInfras.XInfrasExceptions;
using XTAInfras.XTestCircle;

#endregion Import statements

namespace XTAClient.XTATests.XOnboardingExperience;

#region XLogInTests > Class level

[Parallelizable(ParallelScope.Children)]
[FixtureLifeCycle(LifeCycle.SingleInstance)]
[TestFixture]
internal class XLogInTests : AXTATestFoundation
{
    #region Introduce X Log In tests
    
    [Test]
    [Order(XTestEchelon.ALPHA)]
    [Category(XTestSet.XUI_STANDARD_MODE)]
    public async Task XUITest_NavigateToXLogInPage_InputCorrectCredentials_VerifySuccessfullyLogInAndBeLandedOnHomePageAsync()
    {
        XAccountModel xAccountModel = new()
        {
            XDisplayName = psr_checkedOutXAccountCredCluster[p_xTestMetaKey].out_xAccountCredModel.XDisplayName,
            XUsername = psr_checkedOutXAccountCredCluster[p_xTestMetaKey].out_xAccountCredModel.XUsername,
            XPassword = psr_checkedOutXAccountCredCluster[p_xTestMetaKey].out_xAccountCredModel.XPassword
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
    [Order(XTestEchelon.ALPHA)]
    [Category(XTestSet.XUI_STANDARD_MODE)]
    public async Task XUITest_NavigateToXLogInPage_ClickOnSignInButton_VerifySignInToXModalDisplayedAsync()
    {
        await new XLogInPage(p_xPage).ClickOnSignInBtnAsync();
        await new XSignInToXModal(p_xPage).VerifyIfSignInToXModalDisplayedAsync();
    }
    
    [Test]
    [Order(XTestEchelon.HYPER)]
    [Category(XTestSet.XUI_LOCAL_MODE)]
    [XZeta(nameof(m_xZeta_AsObservedToNotBeSupported))]
    public async Task XUITest_NavigateToXLogInPage_SignInWithApple_VerifyXAppleOAuthModalPresentedAsync()
    {
        XLogInPage xLogInPage = new(p_xPage);

        IPage? xAppleOAuthPopup = default;

        await XSingletonFactory.s_DaVinci<XRetryUtils>()
            .RetryAsync(async () =>
            {
                Task<IPage> xOAuthPopupListener = xLogInPage.GenXOAuthPopupListener();

                await xLogInPage.ClickOnSignInWithAppleBtnAsync();

                xAppleOAuthPopup = await xOAuthPopupListener;
            },
            a_ex => a_ex is TimeoutException 
                    || a_ex is PlaywrightException,
            3,
            200
        );
        
        await new XAppleOAuthModal(xAppleOAuthPopup!)
            .VerifyXAppleOAuthModalPresentedAsync();
    }
    
    [Test]
    [Order(XTestEchelon.HYPER)]
    [Category(XTestSet.XUI_LOCAL_MODE)]
    [XZeta(nameof(m_xZeta_AsObservedToNotBeSupported))]
    public async Task XUITest_NavigateToXLogInPage_SignInWithGoogle_VerifyXAppleOAuthModalPresentedAsync()
    {
        XLogInPage xLogInPage = new(p_xPage);

        IPage? xGoogleOAuthPopup = default;
        
        Task<IPage> xGoogleOAuthPopupListener = xLogInPage.GenXOAuthPopupListener();
        
        await xLogInPage.ClickOnSignInWithGoogleBtnAsync();

        xGoogleOAuthPopup = await xGoogleOAuthPopupListener;

        await new XGoogleOAuthModal(xGoogleOAuthPopup!)
            .VerifyXOAuthGoogleModalPresentedAsync();
    }
    
    #endregion Introduce X Log In tests

    #region Private services

    private void m_xZeta_AsObservedToNotBeSupported()
    {
        if (!ps_xPlaywrightConfModel.Headed)
            throw new XTestNotSupportedUponHeadlessModeException(
                $"Test {p_xTestMetaKey} cannot be executed upon headless mode.       ");
    }

    #endregion Private services
    
    #region Introduce NUnit SetUp phase

    [OneTimeSetUp]
    public static void s_XMetaSetUp() 
        => XSingletonFactory.s_DaVinci<XTANavigationKit>();

    [SetUp]
    public async Task XMegaSetUpAsync()
        => await XSingletonFactory.s_Retrieve<XTANavigationKit>()
            .NavigateToURLAsync(p_xPage, ps_xAppConfModel.BaseXURL);
    
    #endregion Introduce NUnit SetUp phase
}

#endregion XLogInTests > Class level