using Microsoft.Playwright;
using XTACore.XTAUtils;
using XTADomain.XTAPageObjects.XCoreExperience.XHomExperience.XHomeExperienceModals;
using XTADomain.XTASharedActions;

namespace XTADomain.XTABusinesses.XCoreExperience.XHomeExperience.XHomeExperienceModals;

public class XDeletePostModal(IPage in_xPage)
{
    #region Introduce class vars

    private readonly XDeletePostMOs mr_xDeletePostMOs = XSingletonFactory.s_DaVinci<XDeletePostMOs>();

    #endregion Introduce class vars

    #region Introduce actions

    public async Task ClickOnDeletePostBtnAsync()
        => await XSingletonFactory.s_DaVinci<XTAWebUISharedActions>().ClickAsync(in_xPage, mr_xDeletePostMOs.BTN_DELETE_POST);

    #endregion Introduce actions
}