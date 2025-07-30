using Microsoft.Playwright;
using XTACore.XTAUtils;
using XTADomain.XTASharedActions;

namespace XTADomain.XTABusinesses.XBusinessAbstractions;

public abstract class AXPage
{
    #region Introduce vars
    
    protected IPage p_xPage;
    
    protected readonly XTAWebUISharedActions pr_xtaWebUISharedActions = XSingletonFactory.s_DaVinciResolve<XTAWebUISharedActions>();
    protected readonly XTAWebUISharedVerifiers pr_xtaWebUISharedVerifiers = XSingletonFactory.s_DaVinciResolve<XTAWebUISharedVerifiers>();
    protected readonly XTAWebUIWaitStrategies pr_xtaWebUIWaitStrategies = XSingletonFactory.s_DaVinciResolve<XTAWebUIWaitStrategies>();
    
    #endregion Introduce vars

    #region Introduce actions

    protected async Task p_ClickOnSharedNavAsync(String in_expectedNav) 
        => await pr_xtaWebUISharedActions.ClickAsync(p_xPage, in_expectedNav);

    // protected async Task p_ClickOnPostBtnAsync() 
    //     => await pr_xtaWebUISharedActions.ClickAsync(p_xPage)
    
    #endregion Introduce actions
}