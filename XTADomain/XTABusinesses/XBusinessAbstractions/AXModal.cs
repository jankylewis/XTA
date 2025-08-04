using Microsoft.Playwright;
using XTACore.XTAUtils;
using XTADomain.XTASharedActions;

namespace XTADomain.XTABusinesses.XBusinessAbstractions;

public abstract class AXModal
{
    protected IPage p_xPage;
    
    protected readonly XTAWebUISharedActions pr_xtaWebUISharedActions = XSingletonFactory.s_DaVinci<XTAWebUISharedActions>();
    protected readonly XTAWebUISharedVerifiers pr_xtaWebUISharedVerifiers = XSingletonFactory.s_DaVinci<XTAWebUISharedVerifiers>();
    protected readonly XTAWebUIWaitStrategies pr_xtaWebUIWaitStrategies = XSingletonFactory.s_DaVinci<XTAWebUIWaitStrategies>();
}