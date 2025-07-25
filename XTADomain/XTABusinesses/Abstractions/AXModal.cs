using Microsoft.Playwright;
using XTADomain.XTASharedActions;

namespace XTADomain.XTABusinesses.Abstractions;

public abstract class AXModal
{
    protected IPage prot_page;
    protected readonly XTAWebUISharedActions prot_ro_XTA_WEBUI_SHARED_ACTIONS = new();
    protected readonly XTAWebUISharedVerifiers prot_ro_XTA_WEBUI_SHARED_VERIFIERS = new();
    protected readonly XTAWebUIWaitStrategies prot_ro_XTA_WEBUI_WAIT_STRATEGIES = new();
}