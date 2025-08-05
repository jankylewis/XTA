using Microsoft.Playwright;
using XTACore.XTAUtils;
using XTADomain.XTABusinesses.XBusinessAbstractions;
using XTADomain.XTAPageObjects.XOnboardingExperience.XOnboardingExperienceModals;

namespace XTADomain.XTABusinesses.XOnboardingExperience.XOnboardingExperienceModals;

public class XAdvertizingModal(IPage in_xAdvertizingModal) 
    : AXModal<XAdvertizingMOs>(in_xAdvertizingModal, XSingletonFactory.s_DaVinci<XAdvertizingMOs>()) {}