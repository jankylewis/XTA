using XTADomain.XTAPageObjects.XPOAbstractions;
using XTADomain.XTAPageObjects.XPOUtils;

namespace XTADomain.XTAPageObjects.XCoreExperience;

public class XUserProfilePOs : AXPOs
{
    public XUserProfilePOs() {}
    
    internal String LBL_USER_DISPLAY_NAME(String in_xDisplayName) 
        => p_xPOSharedUtils.BuildSelector($"//div[@data-testid='UserName']//span[text()='{in_xDisplayName}']", ELocatingMechanism.CSS);
    
    internal String LBL_USERNAME(String in_xUsername) 
        => p_xPOSharedUtils.BuildSelector($"//div[@data-testid='UserName']//span[text()='{in_xUsername}']", ELocatingMechanism.XPATH);

    internal String LBL_NUMBER_OF_TWEETS 
        => p_xPOSharedUtils.BuildSelector("//div[@data-testid='primaryColumn']//div[preceding-sibling::h2]", ELocatingMechanism.XPATH);

    internal String LBl_TWEET_CONTEXT
        => p_xPOSharedUtils.BuildSelector("article[data-testid='tweet'] div[data-testid='tweetText']", ELocatingMechanism.XPATH);
    
    internal String BTN_TWEET_MENU
        => p_xPOSharedUtils.BuildSelector("article[data-testid='tweet'] button[data-testid='caret']", ELocatingMechanism.CSS);
    
    internal String BTN_DELETE_TWEET
        => p_xPOSharedUtils.BuildSelector("//div[@data-testid='Dropdown']//span[text()='Delete']", ELocatingMechanism.XPATH);
}