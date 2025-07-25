using Microsoft.Playwright;
using XTADomain.XTASharedActions;

namespace XTADomain.XTABusinesses.Abstractions;

public abstract class AXPage
{
    #region Introduce vars
    
    protected IPage prot_page;
    protected readonly XTAWebUISharedActions prot_ro_XTA_WEBUI_SHARED_ACTIONS = new();
    protected readonly XTAWebUISharedVerifiers prot_ro_XTA_WEBUI_SHARED_VERIFIERS = new();
    protected readonly XTAWebUIWaitStrategies prot_ro_XTA_WEBUI_WAIT_STRATEGIES = new();
    
    #endregion Introduce vars

    #region Introduce actions

    protected async Task ClickOnSharedNav(String in_expectedNav) => 
        await prot_ro_XTA_WEBUI_SHARED_ACTIONS.ClickAsync(prot_page, in_expectedNav);

    #endregion Introduce actions
}