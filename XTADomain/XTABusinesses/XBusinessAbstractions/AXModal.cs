using Microsoft.Playwright;
using XTADomain.XTASharedActions;

namespace XTADomain.XTABusinesses.XBusinessAbstractions;

public abstract class AXModal
{
    protected IPage p_xPage;
    
    protected readonly XTAWebUISharedActions pr_xtaWebUISharedActions = new();
    protected readonly XTAWebUISharedVerifiers pr_xtaWebUISharedVerifiers = new();
    protected readonly XTAWebUIWaitStrategies pr_xtaWebUIWaitStrategies = new();
}