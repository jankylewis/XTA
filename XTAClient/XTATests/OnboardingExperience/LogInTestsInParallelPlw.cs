using Microsoft.Playwright;
using XTADomain.XTABusinesses.CoreExperience;
using XTADomain.XTABusinesses.OnboardingExperience;
using XTADomain.XTABusinesses.OnboardingExperience.Modals;
using System.Collections.Concurrent;  // ← Add this

namespace XTAClient.XTATests.OnboardingExperience;

[Parallelizable(ParallelScope.Children)]
[TestFixture]
public class LogInTestsInParallelPlw
{
    private const String m_X_BASE_URL = "https://x.com/";
    private const String m_X_DISPLAY_NAME = "Janky Lewis";
    private const String m_X_USERNAME = "_jankylewis_se";
    private const String m_X_PASSWORD = "twicemassproduction1!";
    private const bool m_HEADLESS = !true;

    // Shared across ALL tests in this class
    private static IPlaywright? s_playwright;
    private static IBrowser? s_browser;
    
    // 🔧 TEST-METHOD-SAFE: Per-test-method instances (survives ALL async thread switches!)
    private static readonly ConcurrentDictionary<string, IBrowserContext> s_browserContexts = new();
    private static readonly ConcurrentDictionary<string, IPage> s_pages = new();
    
    private static string TestMethodKey => TestContext.CurrentContext.Test.MethodName ?? 
        throw new InvalidOperationException("No test context available");
    
    private IPage m_page => s_pages.TryGetValue(TestMethodKey, out var page) ? page : 
        throw new InvalidOperationException($"Page not initialized for test method '{TestMethodKey}'");
    private IBrowserContext m_browserContext => s_browserContexts.TryGetValue(TestMethodKey, out var context) ? context : 
        throw new InvalidOperationException($"Browser context not initialized for test method '{TestMethodKey}'");

    // Remove these - they were causing interference
    // private IBrowserContext? _browserContext;  ❌ Remove
    // private IPage? _page;                       ❌ Remove

    public static object m_lockObj;
    private static readonly SemaphoreSlim s_setupSemaphore = new(1, 1);
    
    [OneTimeSetUp]
    public static async Task ClassSetUp()
    {
        Console.WriteLine("🚀 OneTimeSetUp STARTING...");
        
        try
        {
            Console.WriteLine("🎭 Creating Playwright...");
            // Create browser ONCE for entire test class
            s_playwright = await Playwright.CreateAsync();
            Console.WriteLine($"✅ Playwright created: {s_playwright != null}");
            
            Console.WriteLine($"🌐 Launching browser (headless: {m_HEADLESS})...");
            s_browser = await s_playwright.Chromium.LaunchAsync(new BrowserTypeLaunchOptions
            {
                Headless = m_HEADLESS,
                //Args = new[] { 
                //    "--no-sandbox", 
                //    "--disable-dev-shm-usage",
                //    "--disable-web-security"
                //}
            });
            Console.WriteLine($"✅ Browser launched: {s_browser != null}");
            Console.WriteLine("🚀 OneTimeSetUp COMPLETED successfully");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"❌ OneTimeSetUp failed: {ex.Message}");
            Console.WriteLine($"❌ StackTrace: {ex.StackTrace}");
            throw;
        }
    }

    [SetUp]
    public async Task TestSetUp()
    {
        var testMethodKey = TestMethodKey;
        Console.WriteLine($"🔧 SetUp STARTING for test method: {testMethodKey}");
        Console.WriteLine($"   📍 Thread: {Thread.CurrentThread.ManagedThreadId}, Task: {Task.CurrentId}");
        
        // Check if browser is available
        if (s_browser == null)
        {
            Console.WriteLine("❌ Browser is null in SetUp!");
            throw new InvalidOperationException("Browser is null! OneTimeSetUp may have failed.");
        }
        
        try
        {
            Console.WriteLine("🌐 Creating browser context...");
            // Create NEW context and page for THIS SPECIFIC TEST METHOD
            var browserContext = await s_browser.NewContextAsync(new BrowserNewContextOptions
            {
                ViewportSize = new ViewportSize { Width = 1280, Height = 720 }
            });

            Console.WriteLine("📄 Creating page...");
            var page = await browserContext.NewPageAsync();
            
            Console.WriteLine($"📋 Final thread: {Thread.CurrentThread.ManagedThreadId}, task: {Task.CurrentId}");
            
            // Store in ConcurrentDictionary (per-test-method resources)
            s_browserContexts[testMethodKey] = browserContext;
            s_pages[testMethodKey] = page;
            
            Console.WriteLine($"✅ SetUp COMPLETED for test method: {testMethodKey}");
            Console.WriteLine($"   📄 Page created: {page != null}");
            Console.WriteLine($"   🔑 Stored with key: {testMethodKey}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"❌ SetUp failed: {ex.Message}");
            Console.WriteLine($"❌ StackTrace: {ex.StackTrace}");
            throw;
        }
    }

    [Test]
    public async Task NavigateToXLogInPage_InputCorrectCredentials_BeingLandedOnHomePage()
    {
        var testMethodKey = TestMethodKey;
        Console.WriteLine($"🧪 TEST STARTING: {testMethodKey}");
        Console.WriteLine($"   📍 Thread: {Thread.CurrentThread.ManagedThreadId}, Task: {Task.CurrentId}");
        Console.WriteLine($"   📄 Page available: {s_pages.ContainsKey(testMethodKey)}");
        Console.WriteLine($"   🌐 Context available: {s_browserContexts.ContainsKey(testMethodKey)}");
        Console.WriteLine($"   📋 All test keys: [{string.Join(", ", s_pages.Keys)}]");
        
        // This will work even after async thread switches! 🎯
        
        
        // Use the per-thread page and context
        await m_page.GotoAsync(m_X_BASE_URL, new PageGotoOptions
        {
            WaitUntil = WaitUntilState.Load,
            Timeout = 30000
        });

        await new LogInPage(m_page).ClickOnSignInButton();

        SignInToXModal signInToXModal = new(m_page);
        EnterYourPasswordModal enterYourPasswordModal = new(m_page);
        
        await signInToXModal.InputUsername(m_X_USERNAME);
        await signInToXModal.ClickOnNextButton();

        await enterYourPasswordModal.InputPassword(m_X_PASSWORD);
        await enterYourPasswordModal.ClickOnLogInButton();

        await new HomePage(m_page).ClickOnProfileNav();

        UserProfilePage userProfilePage = new(m_page);

        await userProfilePage.VerifyDisplayNameShownCorrectly(m_X_DISPLAY_NAME);
        await userProfilePage.VerifyUsernameShownCorrectly("@" + m_X_USERNAME);
    }
    
    [Test]
    public async Task NavigateToXLogInPage_ClickOnSignInButton_VerifySignInToXModalDisplayed()
    {
        // Get THIS THREAD's page and context
        // int threadId = Thread.CurrentThread.ManagedThreadId;

        
        // Use the per-thread page and context
        await m_page.GotoAsync(m_X_BASE_URL, new PageGotoOptions
        {
            WaitUntil = WaitUntilState.Load,
            Timeout = 30000
        });

        await new LogInPage(m_page).ClickOnSignInButton();

        await new SignInToXModal(m_page).VerifyIfSignInToXModalDisplayed();
    }

    [TearDown]
    public async Task TestTearDown()
    {
        var testMethodKey = TestMethodKey;
        Console.WriteLine($"🧹 TearDown for test method: {testMethodKey}");
        
        // Clean up THIS TEST METHOD's resources
        if (s_pages.TryRemove(testMethodKey, out var page))
        {
            await page.CloseAsync();
            Console.WriteLine($"   📄 Page closed for {testMethodKey}");
        }
        
        if (s_browserContexts.TryRemove(testMethodKey, out var browserContext))
        {
            await browserContext.CloseAsync();
            Console.WriteLine($"   🌐 Context closed for {testMethodKey}");
        }
    }

    [OneTimeTearDown]
    public static async Task ClassTearDown()
    {
        Console.WriteLine("🧹 OneTimeTearDown starting...");
        
        // Clean up any remaining test method resources
        foreach (var page in s_pages.Values)
        {
            try { await page.CloseAsync(); } catch { }
        }
        s_pages.Clear();
        
        foreach (var context in s_browserContexts.Values)
        {
            try { await context.CloseAsync(); } catch { }
        }
        s_browserContexts.Clear();
        
        // Clean up shared resources
        if (s_browser != null)
        {
            await s_browser.CloseAsync();
        }
        
        s_playwright?.Dispose();
        
        Console.WriteLine("🧹 OneTimeTearDown completed");
    }
}