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
    
    internal async Task<(XAccountCredModel? out_xAccountModel, ulong out_xDeliveryTag)> BasicGetXAccountCredModelAsync(
        IChannel in_xChannel, string in_xRoutingKey)
    {
        BasicGetResult? results = await in_xChannel.BasicGetAsync(queue: in_xRoutingKey, autoAck: false);
        
        if (results == null)
            return (out_xAccountModel: null, out_xDeliveryTag: 0);
        
        byte[] body = results.Body.ToArray();
        
        string message = Encoding.UTF8.GetString(body);
        
        XAccountCredModel? xAccountCredModel = JsonConvert.DeserializeObject<XAccountCredModel>(message);
        
        return (out_xAccountModel: xAccountCredModel, out_xDeliveryTag: results.DeliveryTag);
    }   
    
    internal async Task PopulateXAccountCredModelsAsync(
        IList<XAccountCredModel> in_xAccountCredModels, IChannel in_xChannel, string in_xRoutingKey) 
    {
        foreach (XAccountCredModel l_xAccountCredModel in in_xAccountCredModels)
            await BasicPubXAccountCredModelAsync(l_xAccountCredModel, in_xChannel, in_xRoutingKey);
    }
    
    internal async Task BasicPubXAccountCredModelAsync(
        XAccountCredModel in_xAccountCredModel, IChannel in_xChannel, string in_xRoutingKey)
    {
        string message = JsonConvert.SerializeObject(in_xAccountCredModel);
        ReadOnlyMemory<byte> body = Encoding.UTF8.GetBytes(message);

        await in_xChannel.BasicPublishAsync(exchange: "", routingKey: in_xRoutingKey, body: body);
    }
    
    #endregion Introduce XAccountCreds operations
}