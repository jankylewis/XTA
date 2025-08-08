using RabbitMQ.Client;
using XTAInfras.XInfrasExceptions;
using XTAInfras.XInfrasUtils;

namespace XTAInfras.XRabbitMQCircle;

internal class XRabbitMQConnectionFactory : IAsyncDisposable
{
    #region Introduce constructors

    public XRabbitMQConnectionFactory() {}
    
    #endregion Introduce constructors

    private IConnection? m_xRabbitMQConn;
    
    #region Introduce RabbitMQ operations

    internal async Task<IConnection> GenConnectionAsync(IConnectionFactory? in_xConnFactory = default)
    {
        IConnectionFactory xConnFacTory = in_xConnFactory ?? new ConnectionFactory { HostName = XNetworkingServices.LOOPBACK_ADDRESS };
        return m_xRabbitMQConn = await xConnFacTory.CreateConnectionAsync();
    }

    internal async Task<IChannel> GenChannelAsync(IConnectionFactory? in_xConnFactory = default)
        => await m_xRabbitMQConn.CreateChannelAsync();
    
    #endregion Introduce RabbitMQ operations

    #region Introduce RabbitMQ disposal

    public async ValueTask DisposeAsync()
    {
        if (m_xRabbitMQConn is not null)
        {
            await m_xRabbitMQConn.CloseAsync();
            m_xRabbitMQConn = null;
            
            return;
        }

        throw new XRabbitMQConnectionMetNullException("The XRabbitMQConnection is null, please check that out.     ");
    }

    #endregion Introduce RabbitMQ disposal
}