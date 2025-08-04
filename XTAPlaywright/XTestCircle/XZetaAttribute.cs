using System.Reflection;
using NUnit.Framework;
using NUnit.Framework.Interfaces;
using XTAPlaywright.XExceptions;

namespace XTAPlaywright.XTestCircle;

[AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
public class XZetaAttribute : Attribute, ITestAction
{
    public XZetaAttribute(params string[] in_xZetas)
        => mr_xTestZetas = in_xZetas ?? throw new ArgumentNullException(nameof(in_xZetas));
    
    private readonly string[] mr_xTestZetas;
    
    public ActionTargets Targets => ActionTargets.Test;

    public void BeforeTest(ITest in_xTest)
    {
        object? xTestInstance = in_xTest.Fixture;
        
        Type? xTestClass = xTestInstance?.GetType();

        foreach (string l_xZeta in mr_xTestZetas)
            _ExecuteXZeta(xTestInstance, xTestClass!, l_xZeta);
    }

    public void AfterTest(ITest in_xTest) {}

    private void _ExecuteXZeta(
        object in_xTestInstance, Type in_xTestClass, string in_xTestMethodName)
    {
        try
        {
            MethodInfo? methodInfo = in_xTestClass.GetMethod(
                in_xTestMethodName, BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance
            );

            if (methodInfo is null)
                throw new XZetaNotQualifiedException(
                    $"Zeta method: {in_xTestMethodName} not found in class: <{in_xTestClass.Name}>. " +
                    $"Please ensure the method exists and is accessible.      "
                );

            object? xZetaResults = methodInfo.Invoke(in_xTestInstance, null);

            if (xZetaResults is Task task)
                task.GetAwaiter().GetResult();
        }
        catch (TargetInvocationException a_targetInvocationEx)
        {
            throw new XZetaNotQualifiedException(
                $"Zeta method: {in_xTestMethodName} failed during execution: " +
                $"\n\nOriginal exception: {a_targetInvocationEx}\n\n" + 
                $"\n\nInner exception: {a_targetInvocationEx?.InnerException?.Message}\n\n",
                a_targetInvocationEx.InnerException
            );
        }
        catch (Exception a_ex)
        {
            throw new XZetaNotQualifiedException(
                $"Zeta method: {in_xTestMethodName} failed to execute Zeta method <{in_xTestMethodName}> " +
                $"with failed message: {a_ex?.Message}",
                a_ex
            );
        }
    }
}