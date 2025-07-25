using Microsoft.Playwright;
using System.Diagnostics;
using System.Reflection;
using XTADomain.XTABusinesses.CoreExperience;
using XTADomain.XTABusinesses.OnboardingExperience;
using XTADomain.XTABusinesses.OnboardingExperience.Modals;

namespace XTAClient.XTATests.OnboardingExperience;

[Parallelizable(ParallelScope.Children)]
[TestFixture]
public class _LogInTests
{
    private const String m_X_BASE_URL = "https://x.com/";
    private const String m_X_DISPLAY_NAME = "Janky Lewis";
    private const String m_X_USERNAME = "_jankylewis_se";
    private const String m_X_PASSWORD = "twicemassproduction1!";

    private bool m_headless = false;
    
    [Test]
    public async Task NavigateToXLogInPage_InputCorrectCredentials_BeingLandedOnHomePage()
    {
        IPlaywright plw = null;
        IBrowser br = null;
        IBrowserContext brc = null;
        IPage p = null;

        plw = await Playwright.CreateAsync();
        
        br = await plw.Chromium.LaunchAsync(new BrowserTypeLaunchOptions
        {
            Headless = m_headless,
            Args = new[] { 
                "--no-sandbox", 
                "--disable-dev-shm-usage",
                "--disable-web-security"
            }
        });

        brc = await br.NewContextAsync(new BrowserNewContextOptions
        {
            ViewportSize = new ViewportSize { Width = 1280, Height = 720 }
        });

        p = await brc.NewPageAsync();

        await p.GotoAsync(m_X_BASE_URL, new PageGotoOptions
        {
            WaitUntil = WaitUntilState.Load,
            Timeout = 30000
        });

        await new LogInPage(p).ClickOnSignInButton();

        SignInToXModal signInToXModal = new(p);
        EnterYourPasswordModal enterYourPasswordModal = new(p);
        
        await signInToXModal.InputUsername(m_X_USERNAME);
        await signInToXModal.ClickOnNextButton();

        await enterYourPasswordModal.InputPassword(m_X_PASSWORD);
        await enterYourPasswordModal.ClickOnLogInButton();

        await new HomePage(p).ClickOnProfileNav();

        UserProfilePage userProfilePage = new(p);

        await userProfilePage.VerifyDisplayNameShownCorrectly(m_X_DISPLAY_NAME);
        await userProfilePage.VerifyUsernameShownCorrectly("@" + m_X_USERNAME);

        await p.CloseAsync();
        await brc.CloseAsync();
        await br.CloseAsync();
        plw.Dispose();

        // Thread.Sleep(3000);
    }
    
    [Test]
    public async Task NavigateToXLogInPage_ClickOnSignInButton_VerifySignInToXModalDisplayed()
    {
        IPlaywright plw = null;
        IBrowser br = null;
        IBrowserContext brc = null;
        IPage p = null;

        plw = await Playwright.CreateAsync();
        
        br = await plw.Chromium.LaunchAsync(new BrowserTypeLaunchOptions
        {
            Headless = m_headless,
            Args = new[] { 
                "--no-sandbox", 
                "--disable-dev-shm-usage",
                "--disable-web-security"
            }
        });

        brc = await br.NewContextAsync(new BrowserNewContextOptions
        {
            ViewportSize = new ViewportSize { Width = 1280, Height = 720 }
        });

        p = await brc.NewPageAsync();

        await p.GotoAsync(m_X_BASE_URL, new PageGotoOptions
        {
            WaitUntil = WaitUntilState.Load,
            Timeout = 30000
        });

        await new LogInPage(p).ClickOnSignInButton();

        await new SignInToXModal(p).VerifyIfSignInToXModalDisplayed();
        
        await p.CloseAsync();
        await brc.CloseAsync();
        await br.CloseAsync();
        plw.Dispose();
    }
    
    [Test]
    public void DiagnosticTest_CheckEnvironment()
    {
        Console.WriteLine($"Current Thread ID: {Thread.CurrentThread.ManagedThreadId}");
        Console.WriteLine($"Max Worker Threads: {ThreadPool.ThreadCount}");
        Console.WriteLine($"Test Assembly: {Assembly.GetExecutingAssembly().GetName().Name}");
        Console.WriteLine($"Test executed at: {DateTime.Now:yyyy-MM-dd HH:mm:ss.fff}");
        Console.WriteLine($"Process ID: {Environment.ProcessId}");
        Console.WriteLine($"Environment.ProcessorCount: {Environment.ProcessorCount}");
        Console.WriteLine($"TaskScheduler.Current: {TaskScheduler.Current.GetType().Name}");
        
        // Check if we're running with NUnit's parallel execution
        var context = TestContext.CurrentContext;
        Console.WriteLine($"Test Worker: {context.WorkerId}");
        Console.WriteLine($"Test Name: {context.Test.Name}");
        
        // Show running processes related to testing
        Console.WriteLine("\n=== Current Test-Related Processes ===");
        var currentProcess = Process.GetCurrentProcess();
        Console.WriteLine($"Current Process: {currentProcess.ProcessName} (PID: {currentProcess.Id})");
        
        // Look for related processes
        var processes = Process.GetProcesses()
            .Where(p => p.ProcessName.ToLower().Contains("test") || 
                       p.ProcessName.ToLower().Contains("chrome") ||
                       p.ProcessName.ToLower().Contains("dotnet") ||
                       p.ProcessName.ToLower().Contains("playwright"))
            .Take(10) // Limit output
            .ToList();
            
        foreach (var proc in processes)
        {
            try
            {
                Console.WriteLine($"  {proc.ProcessName} (PID: {proc.Id})");
            }
            catch
            {
                // Process might have exited
            }
        }
        
        Assert.Pass("Diagnostic test completed");
    }
}