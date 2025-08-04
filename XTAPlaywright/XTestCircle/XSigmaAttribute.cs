using System.Reflection;
using NUnit.Framework;
using NUnit.Framework.Interfaces;
using XTAPlaywright.XExceptions;

namespace XTAPlaywright.XTestCircle;

[AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
public class XSigmaAttribute : Attribute, ITestAction
{
    public XSigmaAttribute(params string[] in_xSigmas)
        => mr_xSigmas = in_xSigmas ?? throw new ArgumentNullException(nameof(in_xSigmas));

    private readonly string[] mr_xSigmas;

    public ActionTargets Targets => ActionTargets.Test;

    public void BeforeTest(ITest in_xTest) {}

    public void AfterTest(ITest in_xTest)
    {
        object? testInstance = in_xTest.Fixture;
        Type? testClass = testInstance?.GetType();

        foreach (string l_xSigma in mr_xSigmas)
            _ExecuteXSigma(testInstance, testClass!, l_xSigma);
    }

    private static void _ExecuteXSigma(object? in_xTestInstance, Type in_xTestClass, string in_xSigma)
    {
        try
        {
            MethodInfo? methodInfo = in_xTestClass.GetMethod(
                in_xSigma,
                BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance);

            if (methodInfo is null)
                throw new XSigmaNotQualifiedException(
                    $"Sigma method: {in_xSigma} not found in <{in_xTestClass.Name}>.");

            object? result = methodInfo.Invoke(in_xTestInstance, null);
            if (result is Task t)
                t.GetAwaiter().GetResult();
        }
        catch (TargetInvocationException tie)
        {
            throw new XZetaNotQualifiedException(
                $"Sigma method: {in_xSigma} threw an exception.\nOriginal: {tie}",
                tie.InnerException ?? tie);
        }
        catch (Exception ex)
        {
            throw new XZetaNotQualifiedException(
                $"Failed to execute Sigma method <{in_xSigma}>: {ex.Message}", ex);
        }
    }
}
