using XTADomain.XTAPageObjects.XPOAbstractions;
using XTADomain.XTAPageObjects.XPOUtils;

namespace XTADomain.XTAPageObjects.XCoreExperience;

public class XUserProfilePOs : AXPOs
{
    public XUserProfilePOs() {}
    
    internal String LBL_USER_DISPLAY_NAME(String in_xDisplayName) 
        => pr_xPOSharedUtils.BuildSelector($"//div[@data-testid='UserName']//span[text()='{in_xDisplayName}']", ELocatingMechanism.CSS);
    
    internal String LBL_USERNAME(String in_xUsername) 
        => pr_xPOSharedUtils.BuildSelector($"//div[@data-testid='UserName']//span[text()='{in_xUsername}']", ELocatingMechanism.XPATH);

    internal String LBL_NUMBER_OF_TWEETS 
        => pr_xPOSharedUtils.BuildSelector("//div[@data-testid='primaryColumn']//div[preceding-sibling::h2]", ELocatingMechanism.XPATH);

    internal String LBl_TWEET_CONTENT
        => pr_xPOSharedUtils.BuildSelector("article[data-testid='tweet'] div[data-testid='tweetText']", ELocatingMechanism.CSS);
    
    internal String BTN_TWEET_MENU
        => pr_xPOSharedUtils.BuildSelector("article[data-testid='tweet'] button[data-testid='caret']", ELocatingMechanism.CSS);
    
    internal String BTN_DELETE_TWEET
        => pr_xPOSharedUtils.BuildSelector("//div[@data-testid='Dropdown']//span[text()='Delete']", ELocatingMechanism.XPATH);
    
    internal String IMG_USER_AVATAR
        => pr_xPOSharedUtils.BuildSelector("//a[contains(@href,'photo')]", ELocatingMechanism.XPATH);
}