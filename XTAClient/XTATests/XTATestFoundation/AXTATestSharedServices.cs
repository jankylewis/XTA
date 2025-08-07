using RabbitMQ.Client;
using XTACore.XCoreUtils;
using XTADomain.XTABusinesses.XOnboardingExperience;
using XTADomain.XTABusinesses.XOnboardingExperience.XOnboardingExperienceModals;
using XTADomain.XTASharedActions;
using XTAInfras.XConfFactories.XConfModels;

namespace XTAClient.XTATests.XTATestFoundation;

internal abstract partial class AXTATestFoundation
{
    protected static XTANavigationKit? ps_xtaNavigationKit = default;

    protected async Task p_LogInToXAsync()
    {
        XSignInToXModal xSignInToXModal = new(p_xPage);
        XEnterYourPasswordModal xEnterYourPasswordModal = new(p_xPage);
        XUnusualLogInModal xUnusualLogInModal = new(p_xPage);

        (XAccountCredModel, ulong, IChannel) creds = await p_TakeIdleXAccountCredAsync();
        
        await XSingletonFactory
            .s_DaVinci<XTANavigationKit>()
            .NavigateToURLAsync(p_xPage, ps_xAppConfModel.BaseXURL);
        
        await new XLogInPage(p_xPage).ClickOnSignInBtnAsync();
        await xSignInToXModal.InputUsernameAsync(creds.Item1.XUsername);
        await xSignInToXModal.ClickOnNextBtnAsync();

        if (await xUnusualLogInModal.VerifyWhetherXUnusualLogInModalPresented())
        {
            await xUnusualLogInModal.InputEmailAsync(creds.Item1.XEmail);
            await xUnusualLogInModal.ClickOnNextBtnAsync();
        }
            
        await xEnterYourPasswordModal.InputPasswordAsync(creds.Item1.XPassword);
        await xEnterYourPasswordModal.ClickOnLogInBtnAsync();
    }
}