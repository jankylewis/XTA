using XTACore.XTAUtils;
using XTADomain.XTAPageObjects.XPOUtils;

namespace XTADomain.XTAPageObjects.XPOAbstractions;

public abstract class AXMOs
{
    #region Introduce vars

    protected readonly XPOSharedUtils pr_xPOSharedUtils = XSingletonFactory.s_DaVinci<XPOSharedUtils>();
    
    #endregion Introduce vars

    #region Introduce shared elements

    internal String BTN_CLOSE_MODAL
        => pr_xPOSharedUtils.BuildSelector("//button[@data-testid='app-bar-close']", ELocatingMechanism.XPATH);
    
    internal String LBL_HEADING 
        => pr_xPOSharedUtils.BuildSelector("modal-header", ELocatingMechanism.ID);

    #endregion Introduce shared elements
}