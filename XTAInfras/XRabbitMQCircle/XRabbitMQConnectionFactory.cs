using RabbitMQ.Client;

namespace XTAInfras.XRabbitMQCircle;

internal class XRabbitMQConnectionFactory
{
    #region Introduce constructors

    public XRabbitMQConnectionFactory() {}
    
    internal IConnection xRabbitMQConn;
    internal IChannel xRabbitMQChann;

    #endregion Introduce constructors

    #region Introduce RabbitMQ operations
    
    internal async Task<IChannel> GenChannelAsync(IConnectionFactory? in_xConnFactory = default)
    {
        IConnectionFactory xConnFactory = m_GenConnectionFactoryAsync(in_xConnFactory);
        
        xRabbitMQConn = await m_GenConnectionAsync(xConnFactory);
        
        return xRabbitMQChann = await m_GenChannelAsync(xRabbitMQConn);
    }

    #region Introduce private services

    private IConnectionFactory m_GenConnectionFactoryAsync(IConnectionFactory? in_xConnFactory = default)
        => in_xConnFactory ?? new ConnectionFactory()
        {
            HostName = "localhost"
        };
    
    private async Task<IConnection> m_GenConnectionAsync(IConnectionFactory? in_xConnFactory = default) 
        => await in_xConnFactory!.CreateConnectionAsync();

    private async Task<IChannel> m_GenChannelAsync(IConnection in_xConn)
        => await in_xConn.CreateChannelAsync();

    #endregion Introduce private services

    #endregion Introduce RabbitMQ operations
}