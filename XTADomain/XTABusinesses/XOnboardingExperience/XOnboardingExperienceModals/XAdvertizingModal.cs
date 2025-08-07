using Microsoft.Playwright;
using XTACore.XCoreUtils;
using XTADomain.XTABusinesses.XBusinessAbstractions;
using XTADomain.XTAPageObjects.XOnboardingExperience.XOnboardingExperienceModals;

namespace XTADomain.XTABusinesses.XOnboardingExperience.XOnboardingExperienceModals;

public class XAdvertizingModal(IPage in_xPage) 
    : AXModal<XAdvertizingMOs>(in_xPage, XSingletonFactory.s_DaVinci<XAdvertizingMOs>()) {}