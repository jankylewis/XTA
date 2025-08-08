using XTAReportingEngine.Events;

namespace XTAReportingEngine.Publishing;

public interface IXTAReportEventPublisher
{
    Task PublishAsync(XTAEvent evt, CancellationToken ct = default);
}


