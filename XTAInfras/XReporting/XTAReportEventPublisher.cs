using System.Text;
using System.Text.Json;
using RabbitMQ.Client;
using XTAInfras.XInfrasUtils;
using XTAReportingEngine.Events;

namespace XTAInfras.XReporting;

public sealed class XTAReportEventPublisher : IAsyncDisposable
{
    private const string m_EXCHANGE = "xta.test.events.topic";
    private readonly ConnectionFactory mr_connFactory;
    private IConnection? m_connection;
    private IChannel? m_channel;

    public XTAReportEventPublisher()
    {
        mr_connFactory = new ConnectionFactory
        {
            HostName = Environment.GetEnvironmentVariable("XTA_RMQ_HOST") ?? XNetworkingServices.LOCALHOST_ADDRESS,
            AutomaticRecoveryEnabled = true
        };
    }

    public async Task PublishRunSessionStartedAsync(
        string runSessionID,
        string? branch = null,
        string? commit = null,
        CancellationToken ct = default)
    {
        await m_EnsureConnectedAsync(ct);
        
        var runMode = Environment.GetEnvironmentVariable("XTA_RUN_MODE") ?? "Local Run";
        
        var evt = new RunSessionStartedEvent
        {
            EventType = XTAEventTypes.RunSessionStarted,
            RunSessionID = runSessionID,
            TimestampUTC = DateTime.UtcNow,
            RunMode = runMode,
            Machine = Environment.MachineName,
            Branch = branch,
            Commit = commit
        };
        await m_PublishAsync(evt, routingKey: $"run.{runSessionID}", ct);
    }

    public async Task PublishRunSessionCompletedAsync(string runSessionID, CancellationToken ct = default)
    {
        await m_EnsureConnectedAsync(ct);
        
        var evt = new RunSessionCompletedEvent
        {
            EventType = XTAEventTypes.RunSessionCompleted,
            RunSessionID = runSessionID,
            TimestampUTC = DateTime.UtcNow
        };
        await m_PublishAsync(evt, routingKey: $"run.{runSessionID}", ct);
    }

    private async Task m_EnsureConnectedAsync(CancellationToken ct)
    {
        if (m_connection is { IsOpen: true } && m_channel is { IsOpen: true }) 
            return;
        
        m_connection = await mr_connFactory.CreateConnectionAsync(cancellationToken: ct);
        
        m_channel = await m_connection.CreateChannelAsync(cancellationToken: ct);
        
        await m_channel.ExchangeDeclareAsync(m_EXCHANGE, ExchangeType.Topic, durable: true, cancellationToken: ct);
    }

    private async Task m_PublishAsync(object payload, string routingKey, CancellationToken ct)
    {
        var body = Encoding.UTF8.GetBytes(JsonSerializer.Serialize(payload));
        var props = new BasicProperties() { Persistent = true, ContentType = "application/json" };
        
        await m_channel!.BasicPublishAsync(
            m_EXCHANGE, routingKey, mandatory: false, basicProperties: props, body: body.AsMemory(), ct);
    }

    public async ValueTask DisposeAsync()
    {
        try
        {
            if (m_channel is not null)
                await m_channel.CloseAsync();
        }
        finally
        {
            m_channel = null;
            if (m_connection is not null)
                await m_connection.CloseAsync();
            m_connection = null;
        }
    }
}


