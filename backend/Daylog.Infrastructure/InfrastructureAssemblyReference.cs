using System.Reflection;

namespace Daylog.Infrastructure;

public sealed class InfrastructureAssemblyReference
{
    public static Assembly Assembly => typeof(InfrastructureAssemblyReference).Assembly;
}
