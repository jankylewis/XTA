using Microsoft.Playwright;
using XTACore.XCoreUtils;
using XTADomain.XTAPageObjects.XPOAbstractions;
using XTADomain.XTASharedActions;

namespace XTADomain.XTABusinesses.XBusinessAbstractions;

public abstract class AXModal<XMOs> where XMOs : AXMOs
{
    #region Introduce constructors

    protected AXModal(IPage in_xPage, XMOs in_xPOs)
    {
        pr_xPage = in_xPage;
        pr_xPOs = in_xPOs;
    }

    #endregion Introduce constructors

    #region Introduce shared vars

    protected readonly IPage pr_xPage;
    protected readonly XMOs pr_xPOs;
    
    protected readonly XTAWebUISharedActions pr_xtaWebUISharedActions = XSingletonFactory.s_DaVinci<XTAWebUISharedActions>();
    protected readonly XTAWebUISharedVerifiers pr_xtaWebUISharedVerifiers = XSingletonFactory.s_DaVinci<XTAWebUISharedVerifiers>();
    protected readonly XTAWebUIWaitStrategies pr_xtaWebUIWaitStrategies = XSingletonFactory.s_DaVinci<XTAWebUIWaitStrategies>();    

    #endregion Introduce shared vars
}