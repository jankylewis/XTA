using Microsoft.Playwright;
using XTAPlaywright.XConfFactories.XConfModels;
using XTAPlaywright.XPlwCircle.XPlwAdapter;
using XTAPlaywright.XPlwCircle.XPlwCable.XPlwCableFactories;
using XTAPlaywright.XPlwCircle.XPlwCable.XPlwCableModels;
using XTAPlaywright.XTestCircle;

namespace XTAPlaywright.XPlwCircle;

public class XPlwEngineer
{
    public XPlwEngineer(XPlwConfModel in_xPlwConfModel)
        => m_XPlwConfModel = in_xPlwConfModel;
    
    private readonly XPlwConfModel m_XPlwConfModel;

    public async Task<XPlwSingleCoreCableModel> GenXPlwSingleCoreCableModelAsync()
    {
        XPlwSingleCoreCableFactory xPlwSingleCoreCableFactory = new(m_XPlwConfModel);

        await xPlwSingleCoreCableFactory.GenPlaywrightAsync();
        await xPlwSingleCoreCableFactory.GenBrowserAsync();

        return xPlwSingleCoreCableFactory.TakeXPlwSingleCoreCableModel();
    }

    public async Task<XPlwMultiCoreCableModel> GenXPlwMultiCoreCableModelAsync(IBrowser in_xBrowser)
    {
        XPlwMultiCoreCableFactory xPlwMultiCoreCableFactory = new(m_XPlwConfModel);

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
}