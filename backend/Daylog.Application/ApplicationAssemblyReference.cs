using System.Reflection;

namespace Daylog.Application;

public sealed class ApplicationAssemblyReference
{
    public static Assembly Assembly => typeof(ApplicationAssemblyReference).Assembly;
}
