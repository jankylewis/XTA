using XTADomain.XTAPageObjects.XPOAbstractions;
using XTADomain.XTAPageObjects.XPOUtils;

namespace XTADomain.XTAPageObjects.XCoreExperience.XHomExperience;

public class XHomePOs : AXPOs
{
    public XHomePOs() {}

    internal String BTN_ACCOUNT_MENU 
        => pr_xPOSharedUtils.BuildSelector("button[data-testid='SideNav_AccountSwitcher_Button']", ELocatingMechanism.CSS);

    internal String LBL_TWEET_TEXT
        => pr_xPOSharedUtils.BuildSelector("div[data-testid='tweetText']", ELocatingMechanism.CSS);

    internal String BTN_TWEET_MENU
        => pr_xPOSharedUtils.BuildSelector("button[data-testid='caret']", ELocatingMechanism.CSS);
    
    internal String BTN_DELETE_TWEET
        => pr_xPOSharedUtils.BuildSelector("//div[span[text()='Delete']]", ELocatingMechanism.XPATH);
}

