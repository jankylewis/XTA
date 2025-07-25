namespace XTAPlaywright.XTestCircle;

public interface IXTestAdapter
{
    string XTestMetaKey { get; set; }
    string XTestCorrelationID { get; set; }
    
    IXTestAdapter ProduceXTestAdapter(string in_testMetaKey, string in_browserType);
}