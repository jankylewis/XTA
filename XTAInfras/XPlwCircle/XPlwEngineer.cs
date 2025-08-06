using Microsoft.Playwright;
using XTAInfras.XConfFactories.XConfModels;
using XTAInfras.XPlwCircle.XPlwAdapter;
using XTAInfras.XPlwCircle.XPlwCable.XPlwCableFactories;
using XTAInfras.XPlwCircle.XPlwCable.XPlwCableModels;
using XTAInfras.XTestCircle;

namespace XTAInfras.XPlwCircle;

public class XPlwEngineer(XPlwConfModel in_xPlwConfModel)
{
    public async Task<XPlwSingleCoreCableModel> GenXPlwSingleCoreCableModelAsync()
    {
        XPlwSingleCoreCableFactory xPlwSingleCoreCableFactory = new(in_xPlwConfModel);

        await xPlwSingleCoreCableFactory.GenPlaywrightAsync();
        await xPlwSingleCoreCableFactory.GenBrowserAsync();

        return xPlwSingleCoreCableFactory.TakeXPlwSingleCoreCableModel();
    }

    public async Task<XPlwMultiCoreCableModel> GenXPlwMultiCoreCableModelAsync(IBrowser in_xBrowser)
    {
        XPlwMultiCoreCableFactory xPlwMultiCoreCableFactory = new(in_xPlwConfModel);

        await xPlwMultiCoreCableFactory.GenBrowserContextAsync(in_xBrowser);
        await xPlwMultiCoreCableFactory.GenPageAsync();

        return xPlwMultiCoreCableFactory.TakeXPlwMultiCoreCableModel();
    }

    public XPlwAdapterModel PlugXMultiCoreCableIntoXAdapter(
        IXTestAdapter in_xTestAdapter,  XPlwMultiCoreCableModel in_xPlwMultiCoreCableModel, XPlwAdapterModel in_xPlwAdapterModel)
    {
        in_xPlwAdapterModel.XBrowserContexts[in_xTestAdapter.XTestMetaKey] = in_xPlwMultiCoreCableModel.XBrowserContext;
        in_xPlwAdapterModel.XBrowserContexts[in_xTestAdapter.XTestCorrelationID] = in_xPlwMultiCoreCableModel.XBrowserContext;

        in_xPlwAdapterModel.XPages[in_xTestAdapter.XTestMetaKey] = in_xPlwMultiCoreCableModel.XPage;
        in_xPlwAdapterModel.XPages[in_xTestAdapter.XTestCorrelationID] = in_xPlwMultiCoreCableModel.XPage;
        
        return in_xPlwAdapterModel;
    }
    
    public async Task PowerDownPlwPowerSourceAsync(
        XPlwSingleCoreCableModel in_xPlwSingleCoreCableModel, 
        XPlwAdapterModel in_xPlwAdapterModel
        ) 
            => await new XPlwPowerSource(in_xPlwSingleCoreCableModel, in_xPlwAdapterModel).DisposeAsync();

    public async Task<XPlwAdapterModel> UnplugMultiCoreCableFromXAdapterAsync(
        XPlwMultiCoreCableModel in_xPlwMultiCoreCableModel, XPlwAdapterModel in_xPlwAdapterModel, IXTestAdapter in_xTestAdapter)
    {
        await in_xPlwMultiCoreCableModel.XPage.CloseAsync();
        await in_xPlwMultiCoreCableModel.XBrowserContext.CloseAsync();

        in_xPlwAdapterModel.XPages.TryRemove(in_xTestAdapter.XTestMetaKey, out _);
        in_xPlwAdapterModel.XPages.TryRemove(in_xTestAdapter.XTestCorrelationID, out _);

        in_xPlwAdapterModel.XBrowserContexts.TryRemove(in_xTestAdapter.XTestMetaKey, out _);
        in_xPlwAdapterModel.XBrowserContexts.TryRemove(in_xTestAdapter.XTestCorrelationID, out _);
        
        return in_xPlwAdapterModel;
    }
}