
using XTAInfras.XExceptions;

namespace XTADomain.XTAPageObjects.XPOUtils;

public class XPOSharedUtils
{
    public XPOSharedUtils(){}
    
    internal String BuildSelector(string in_selector, ELocatingMechanism in_locatingMechanism)
        => in_locatingMechanism switch
        {
            ELocatingMechanism.CSS => in_selector,
            ELocatingMechanism.XPATH => $"xpath={in_selector}",
            ELocatingMechanism.TEXT => $"text={in_selector}",
            ELocatingMechanism.ID => $"#{in_selector}",
            
            _ => throw new XLocatingMechanismNotSupported($"Unknown locating mechanism: {in_locatingMechanism.ToString()}        ")
        };
}