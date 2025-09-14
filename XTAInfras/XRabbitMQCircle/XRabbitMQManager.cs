using RabbitMQ.Client;
using XTACore.XCoreUtils;
using XTAInfras.XConfFactories.XConfModels;

namespace XTAInfras.XRabbitMQCircle;

public class XRabbitMQManager
{
    #region Introduce constructors

    public XRabbitMQManager()
    {
        mr_xAccountCredFactory = XSingletonFactory.s_DaVinci<XAccountCredFactory>();
        mr_xRabbitMQConnFactory = XSingletonFactory.s_DaVinci<XRabbitMQConnectionFactory>();
    }

    #endregion Introduce constructors

    #region Introduce class vars

    private const string m_ACCOUNT_QUEUE = "xta_account_queue";

    private readonly XAccountCredFactory mr_xAccountCredFactory;
    private readonly XRabbitMQConnectionFactory mr_xRabbitMQConnFactory;

    #endregion Introduce class vars

    #region Introduce RabbitMQ operations

    // CI Implementation
    public string ResolveXRabbitMQServerIP() => "";
    
    // CI Implementation
    public int ResolveXRabbitMQServerPort() => -1;
    
    public async Task PubAllXAccountCredModelsAsync(
        IList<XAccountCredModel> in_xAccountCredModels, IConnectionFactory? in_xConnFactory = default)
    {
        await mr_xRabbitMQConnFactory.GenConnectionAsync(in_xConnFactory);
        
        IChannel xRabbitMQChann = await mr_xRabbitMQConnFactory.GenChannelAsync(in_xConnFactory);
        
        await xRabbitMQChann.QueueDeclareAsync(m_ACCOUNT_QUEUE, durable: true, exclusive: false, autoDelete: false);

        await mr_xAccountCredFactory.PopulateXAccountCredModelsAsync(
            in_xAccountCredModels: in_xAccountCredModels, in_xChannel: xRabbitMQChann, in_xRoutingKey: m_ACCOUNT_QUEUE);
    }

    public async Task<(XAccountCredModel? out_xAccountModel, ulong out_xDeliveryTag, IChannel out_xRabbitMQChann)> GetAnIdleXAccountCredModel(
        IConnectionFactory? in_xConnFactory = default)
    {
        IChannel xRabbitMQChann = await mr_xRabbitMQConnFactory.GenChannelAsync(in_xConnFactory: in_xConnFactory);
        
        (XAccountCredModel? xAccountModel, ulong xDeliveryTag) 
            = await mr_xAccountCredFactory.BasicGetXAccountCredModelAsync(in_xChannel: xRabbitMQChann, in_xRoutingKey: m_ACCOUNT_QUEUE);;
        
        return (out_xAccountModel: xAccountModel, out_xDeliveryTag: xDeliveryTag, out_xRabbitMQChann: xRabbitMQChann);
    }

    public async Task AckMsgAsync(ulong in_deliveryTag, IChannel in_xRabbitMQChann)
        => await in_xRabbitMQChann.BasicAckAsync(deliveryTag: in_deliveryTag, multiple: false);

    public async Task RepublishXAccountToQueueAsync(XAccountCredModel in_xAccountCredModel, IChannel in_xRabbitMQChann)
        => await mr_xAccountCredFactory.BasicPubXAccountCredModelAsync(in_xAccountCredModel, in_xRabbitMQChann, m_ACCOUNT_QUEUE);

    public async Task PurgeXAccountCredModelQueueAsync()
    {
        await using IChannel xRabbitMQChann = await mr_xRabbitMQConnFactory.GenChannelAsync();
        await xRabbitMQChann.QueuePurgeAsync(m_ACCOUNT_QUEUE);
    }
    
    public async Task DeleteXAccountCredModelQueueAsync()
    {
        await using IChannel xRabbitMQChann = await mr_xRabbitMQConnFactory.GenChannelAsync();
        await xRabbitMQChann.QueueDeleteAsync(m_ACCOUNT_QUEUE);
    }
    
    public async Task DisposeXRabbitMQConnectionAsync() => await mr_xRabbitMQConnFactory.DisposeAsync();
    
    #endregion Introduce RabbitMQ operations
}