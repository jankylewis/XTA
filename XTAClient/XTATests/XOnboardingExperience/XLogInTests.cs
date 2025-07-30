
#region Import statements

using Microsoft.Playwright;
using XTAClient.XTATests.XTATestFoundation;
using XTACore.XTAUtils;
using XTADomain.XTABusinesses.XCoreExperience;
using XTADomain.XTABusinesses.XCoreExperience.XHomeExperience;
using XTADomain.XTABusinesses.XOnboardingExperience;
using XTADomain.XTABusinesses.XOnboardingExperience.XOnboardingExperienceModals;
using XTADomain.XTAModels;
using XTADomain.XTASharedActions;
using XTAPlaywright.XExceptions;
using XTAPlaywright.XTestCircle;

#endregion Import statements

namespace XTAClient.XTATests.XOnboardingExperience;

#region XLogInTests > Class level

[Parallelizable(ParallelScope.Children)]
[TestFixture]
internal class XLogInTests : AXTATestFoundation
{
    #region Introduce X Log In tests
    
    [Test]
    [Order(XTestEchelon.ALPHA)]
    [Category(XTestSet.XUI_STANDARD_MODE)]
    public async Task XUITest_NavigateToXLogInPage_InputCorrectCredentials_VerifySuccessfullyLogInAndBeLandedOnHomePage()
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
    [Order(XTestEchelon.ALPHA)]
    [Category(XTestSet.XUI_STANDARD_MODE)]
    public async Task XUITest_NavigateToXLogInPage_ClickOnSignInButton_VerifySignInToXModalDisplayed()
    {
        await new XLogInPage(p_xPage).ClickOnSignInBtnAsync();
        await new XSignInToXModal(p_xPage).VerifyIfSignInToXModalDisplayedAsync();
    }
    
    [Test]
    [Order(XTestEchelon.HYPER)]
    [Category(XTestSet.XUI_LOCAL_MODE)]
    [XPrerequisites(nameof(_AsObservedToNotBeSupported))]
    public async Task XUITest_NavigateToXLogInPage_SignInWithApple_VerifyXAppleOAuthModalPresented()
    {
        XLogInPage xLogInPage = new(p_xPage);

        IPage? xAppleOAuthPopup = default;

        await XSingletonFactory.s_DaVinciResolve<XRetryUtils>()
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
    [XPrerequisites(nameof(_AsObservedToNotBeSupported))]
    public async Task XUITest_NavigateToXLogInPage_SignInWithGoogle_VerifyXAppleOAuthModalPresented()
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

    private void _AsObservedToNotBeSupported()
    {
        if (!ps_xPlaywrightConfModel.Headed)
            throw new XTestNotSupportedUponHeadlessModeException(
                $"Test {p_xTestMetaKey} cannot be executed upon headless mode.       ");
    }

    #endregion Private services
    
    #region Introduce NUnit SetUp phase

    [OneTimeSetUp]
    public static void s_XMetaSetUp() 
        => XSingletonFactory.s_Register<XTANavigationKit>();

    [SetUp]
    public async Task XMegaSetUp()
        => await XSingletonFactory.s_Retrieve<XTANavigationKit>()
            .NavigateToURLAsync(p_xPage, ps_xAppConfModel.BaseXURL);
    
    #endregion Introduce NUnit SetUp phase
}

#endregion XLogInTests > Class level