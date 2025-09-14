using XTACore.XCoreUtils;
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

    internal String BTN_ONTO_SHEET_DIALOG(String in_buttonText)
        => pr_xPOSharedUtils.BuildSelector($"//div[@data-testid='sheetDialog'][//span[text()= '{in_buttonText}']]", ELocatingMechanism.XPATH);

    #endregion Introduce shared elements
}