using System.Text;
using Newtonsoft.Json;
using RabbitMQ.Client;
using XTAInfras.XConfFactories.XConfModels;

namespace XTAInfras.XRabbitMQCircle;

internal class XAccountCredFactory
{
    #region Introduce constructors
    
    public XAccountCredFactory() {}

    #endregion Introduce constructors

    #region Introduce XAccountCreds operations
    
    internal async Task<XAccountCredModel?> BasicGetAccountCredAsync(
        IChannel in_xChannel, string in_xRoutingKey)
    {
        BasicGetResult? results = await in_xChannel.BasicGetAsync(in_xRoutingKey, true);
        
        if (results == null)
            return null;
        
        byte[] body = results.Body.ToArray();
        
        string message = Encoding.UTF8.GetString(body);
        
        return JsonConvert.DeserializeObject<XAccountCredModel>(message);
    }
    
    internal async Task PopulateXAccountCredsAsync(
        IList<XAccountCredModel> in_xAccountCredModels, IChannel in_xChannel, string in_xRoutingKey) 
    {
        foreach (XAccountCredModel l_xAccountCredModel in in_xAccountCredModels)
            await m_BasicPublishAccountCredAsync(l_xAccountCredModel, in_xChannel, in_xRoutingKey);
    }
    
    private async Task m_BasicPublishAccountCredAsync(
        XAccountCredModel in_xAccountCredModel, IChannel in_xChannel, string in_xRoutingKey)
    {
        string message = JsonConvert.SerializeObject(in_xAccountCredModel);
        ReadOnlyMemory<byte> body = Encoding.UTF8.GetBytes(message);

        await in_xChannel.BasicPublishAsync("", in_xRoutingKey, body);
    }
    
    #endregion Introduce XAccountCreds operations
}