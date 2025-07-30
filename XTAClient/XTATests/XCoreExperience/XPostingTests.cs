using XTAClient.XTATests.XTATestFoundation;
using XTAPlaywright.XTestCircle;

namespace XTAClient.XTATests.XCoreExperience;

[Parallelizable(ParallelScope.Children)]
[TestFixture]
internal class XPostingTests : AXTATestFoundation
{
    [Test]
    [Order(XTestEchelon.ALPHA)]
    [Category(XTestSet.XUI_STANDARD_MODE)]
    public async Task XUITest_NavigateToXHomePage_CreateATextBasedPost_VerifyPostSuccessfullyCreated()
    {
        
    }
}