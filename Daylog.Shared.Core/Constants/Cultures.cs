using System.Globalization;

namespace Daylog.Shared.Core.Constants;

public static class Cultures
{
    public const string English = "en";
    public const string EnglishUnitedStates = "en-US";
    public const string Portuguese = "pt";
    public const string PortugueseBrazil = "pt-BR";

    public static readonly CultureInfo EnglishCultureInfo = CultureInfo.GetCultureInfo(English);
    public static readonly CultureInfo EnglishUnitedStatesCultureInfo = CultureInfo.GetCultureInfo(EnglishUnitedStates);
    public static readonly CultureInfo PortugueseCultureInfo = CultureInfo.GetCultureInfo(Portuguese);
    public static readonly CultureInfo PortugueseBrazilCultureInfo = CultureInfo.GetCultureInfo(PortugueseBrazil);

    public const string DefaultCulture = English;
    public static readonly CultureInfo DefaultCultureInfo = EnglishCultureInfo;

    public static readonly string[] SupportedCultures = [
        English,
        EnglishUnitedStates,
        Portuguese,
        PortugueseBrazil,
    ];

    public static readonly CultureInfo[] SupportedCultureInfos = SupportedCultures
        .Select(CultureInfo.GetCultureInfo)
        .ToArray();
}
