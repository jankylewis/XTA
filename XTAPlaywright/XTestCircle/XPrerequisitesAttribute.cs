using System.Reflection;
using NUnit.Framework;
using NUnit.Framework.Interfaces;
using XTAPlaywright.XExceptions;

namespace XTAPlaywright.XTestCircle;

[AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
public class XPrerequisitesAttribute : Attribute, ITestAction
{
    public XPrerequisitesAttribute(params string[] in_prerequisites)
        => mr_xTestPrerequisites = in_prerequisites ?? throw new ArgumentNullException(nameof(in_prerequisites));
    
    private readonly string[] mr_xTestPrerequisites;
    
    public ActionTargets Targets => ActionTargets.Test;

    public void BeforeTest(ITest in_xTest)
    {
        object? xTestInstance = in_xTest.Fixture;
        
        Type? xTestClass = xTestInstance?.GetType();

        foreach (string l_xTestPrerequisite in mr_xTestPrerequisites)
            _ExecutePrerequisiteMethod(xTestInstance, xTestClass!, l_xTestPrerequisite);;
    }

    public void AfterTest(ITest in_xTest) {}

    private void _ExecutePrerequisiteMethod(
        object in_xTestInstance, Type in_xTestClass, string in_xTestMethodName)
    {
        try
        {
            MethodInfo? methodInfo = in_xTestClass.GetMethod(
                in_xTestMethodName, BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance
            );

            if (methodInfo is null)
                throw new XPreqrequisitesNotQualifiedException(
                    $"Prerequisite method: {in_xTestMethodName} not found in class: <{in_xTestClass.Name}>. " +
                    $"Please ensure the method exists and is accessible.      "
                );

            object? xPrerequisiteResults = methodInfo.Invoke(in_xTestInstance, null);

            if (xPrerequisiteResults is Task task)
                task.GetAwaiter().GetResult();
        }
        catch (TargetInvocationException a_targetInvocationEx)
        {
            throw new XPreqrequisitesNotQualifiedException(
                $"Prerequisite method: {in_xTestMethodName} failed during execution: " +
                $"\n\nOriginal exception: {a_targetInvocationEx}\n\n" + 
                $"\n\nInner exception: {a_targetInvocationEx?.InnerException?.Message}\n\n",
                a_targetInvocationEx.InnerException
            );
        }
        catch (Exception a_ex)
        {
            throw new XPreqrequisitesNotQualifiedException(
                $"Prerequisite method: {in_xTestMethodName} failed to execute prerequisite method <{in_xTestMethodName}> " +
                $"with failed message: {a_ex?.Message}",
                a_ex
            );
        }
    }
}