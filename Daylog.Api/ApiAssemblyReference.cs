using System.Reflection;

namespace Daylog.Api;

public sealed class ApiAssemblyReference
{
    public static Assembly Assembly => typeof(ApiAssemblyReference).Assembly;
}
