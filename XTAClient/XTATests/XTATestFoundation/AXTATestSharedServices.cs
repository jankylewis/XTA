using XTACore.XTAUtils;
using XTADomain.XTABusinesses.XOnboardingExperience;
using XTADomain.XTABusinesses.XOnboardingExperience.XOnboardingExperienceModals;
using XTADomain.XTASharedActions;

namespace XTAClient.XTATests.XTATestFoundation;

internal abstract partial class AXTATestFoundation
{
    protected static XTANavigationKit? ps_xtaNavigationKit = default;

    protected async Task p_LogInToXAsync()
    {
        XSignInToXModal xSignInToXModal = new(p_xPage);
        XEnterYourPasswordModal xEnterYourPasswordModal = new(p_xPage);
        XUnusualLogInModal xUnusualLogInModal = new(p_xPage);
        
        await XSingletonFactory
            .s_DaVinci<XTANavigationKit>()
            .NavigateToURLAsync(p_xPage, ps_xAppConfModel.BaseXURL);
        
        await new XLogInPage(p_xPage).ClickOnSignInBtnAsync();
        await xSignInToXModal.InputUsernameAsync(ps_xAppConfModel.XUsername);
        await xSignInToXModal.ClickOnNextBtnAsync();

        if (await xUnusualLogInModal.VerifyWhetherXUnusualLogInModalPresented())
        {
            await xUnusualLogInModal.InputEmailAsync(ps_xAppConfModel.XEmail);
            await xUnusualLogInModal.ClickOnNextBtnAsync();
        }
            
        await xEnterYourPasswordModal.InputPasswordAsync(ps_xAppConfModel.XPassword);
        await xEnterYourPasswordModal.ClickOnLogInBtnAsync();
    }
}