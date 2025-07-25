using Microsoft.Playwright;
using XTADomain.XTASharedActions;

namespace XTADomain.XTABusinesses.XBusinessAbstractions;

public abstract class AXPage
{
    #region Introduce vars
    
    protected IPage p_xPage;
    
    protected readonly XTAWebUISharedActions pr_xtaWebUISharedActions = new();
    protected readonly XTAWebUISharedVerifiers pr_xtaWebUISharedVerifiers = new();
    protected readonly XTAWebUIWaitStrategies pr_xtaWebUIWaitStrategies = new();
    
    #endregion Introduce vars

    #region Introduce actions

    protected async Task p_ClickOnSharedNavAsync(String in_expectedNav) 
        => await pr_xtaWebUISharedActions.ClickAsync(p_xPage, in_expectedNav);

    #endregion Introduce actions
}