using Daylog.Shared.Core.Enums;

namespace Daylog.Shared.Core;

[Obsolete(message: "Use DocumentationProviderSwitch static class methods instead for better performance and simplicity.", error: true)]
public sealed class DocumentationProviderSwitcher<TReturn> : IDisposable
{
    private Func<TReturn> _swagger = null!;
    public required Func<TReturn> Swagger
    {
        private get => _swagger;
        init => _swagger = value ?? throw new ArgumentNullException(nameof(Swagger), "Swagger function cannot be null.");
    }

    private Func<TReturn> _scalar = null!;
    public required Func<TReturn> Scalar
    {
        private get => _scalar;
        init => _scalar = value ?? throw new ArgumentNullException(nameof(Scalar), "Scalar function cannot be null.");
    }

    private bool _disposed = false;

    /// <summary>
    /// Executes the function corresponding to the specified documentation provider.
    /// </summary>
    /// <param name="provider">The documentation provider.</param>
    /// <returns>The result of the executed function.</returns>
    /// <exception cref="NotSupportedException">Thrown when the specified documentation provider is not supported.</exception>
    /// <remarks>
    /// This method uses a switch expression to determine which function to execute based on the provided documentation provider.
    /// It throws a NotSupportedException for unsupported providers.
    /// </remarks>
    /// <example>
    /// <code>
    /// using var switcher = new DocumentationProviderSwitcher&lt;string&gt;
    /// {
    ///     Swagger = () =&gt; "Swagger selected",
    ///     Scalar = () =&gt; "Scalar selected"
    /// };
    /// var result = switcher.Execute(DocumentationProviderEnum.Swagger);
    /// // result will be "Swagger selected"
    /// </code>
    /// </example>
    /// <seealso cref="DocumentationProviderEnum"/>
    /// <seealso cref="NotSupportedException"/>
    /// <seealso cref="Func{TResult}"/>
    public TReturn Execute(DocumentationProviderEnum provider)
    {
        if (_disposed)
        {
            throw new ObjectDisposedException(nameof(DocumentationProviderSwitcher<>), "Cannot execute method on a disposed object.");
        }

        return provider switch
        {
            DocumentationProviderEnum.Swagger => Swagger(),
            DocumentationProviderEnum.Scalar => Scalar(),
            _ => throw new NotSupportedException($"The documentation provider '{provider}' is not supported."),
        };
    }

    public void Dispose()
    {
        _swagger = null!;
        _scalar = null!;

        _disposed = true;
        GC.SuppressFinalize(this);
    }
}

public static class DocumentationProviderSwitch
{
    public static TReturn For<TReturn>(
        DocumentationProviderEnum documentationProvider,
        Func<TReturn> swagger,
        Func<TReturn> scalar
        )
    {
        ArgumentNullException.ThrowIfNull(swagger, nameof(swagger));
        ArgumentNullException.ThrowIfNull(scalar, nameof(scalar));

        return documentationProvider switch
        {
            DocumentationProviderEnum.Swagger => swagger(),
            DocumentationProviderEnum.Scalar => scalar(),
            _ => throw new NotSupportedException($"The documentation provider '{documentationProvider}' is not supported.")
        };
    }
}
