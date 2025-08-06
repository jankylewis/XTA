using RabbitMQ.Client;
using XTACore.XTAUtils;
using XTAInfras.XConfFactories.XConfModels;

namespace XTAInfras.XRabbitMQCircle;

public class XRabbitMQManager : IAsyncDisposable
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
    
    public async Task PushAllXAccountCredsAsync(
        IList<XAccountCredModel> in_xAccountCredModels, IConnectionFactory? in_xConnFactory = default)
    {
        await m_EstablishRabbitMQConnection(in_xConnFactory);
        
        await mr_xRabbitMQConnFactory.xRabbitMQChann
            .QueueDeclareAsync(m_ACCOUNT_QUEUE, durable: true, exclusive: false, autoDelete: false);
        
        await mr_xAccountCredFactory.PopulateXAccountCredsAsync(
            in_xAccountCredModels, mr_xRabbitMQConnFactory.xRabbitMQChann, m_ACCOUNT_QUEUE
            );
    }

    public async Task<XAccountCredModel?> GetIdleXAccountCreds()
        => await mr_xAccountCredFactory.BasicGetAccountCredAsync(mr_xRabbitMQConnFactory.xRabbitMQChann, m_ACCOUNT_QUEUE);
    
    public async Task DeleteAllExistingXRabbitMQMessagesAsync() {}
    
    private async Task m_EstablishRabbitMQConnection(IConnectionFactory? in_xConnFactory = default) 
        => await mr_xRabbitMQConnFactory.GenChannelAsync(in_xConnFactory);
    
    #endregion Introduce RabbitMQ operations
    
    #region Introduce RabbitMQ disposal

    public async ValueTask DisposeAsync()
    {
        await mr_xRabbitMQConnFactory.xRabbitMQChann.CloseAsync();
        await mr_xRabbitMQConnFactory.xRabbitMQConn.CloseAsync();
        await mr_xRabbitMQConnFactory.xRabbitMQChann.DisposeAsync();
        await mr_xRabbitMQConnFactory.xRabbitMQConn.DisposeAsync();
    }

    #endregion Introduce RabbitMQ disposal
}