using System.Linq.Expressions;
using System.Reflection;

namespace Daylog.Domain;

public abstract record EntityId<TEntityId, TValue>(TValue Value)
    where TEntityId : EntityId<TEntityId, TValue>
    where TValue : struct
{
    public override string? ToString() => Value.ToString();

    protected static TEntityId Create(TValue value)
        => _factory(value);
        //=> (TEntityId) Activator.CreateInstance(typeof(TEntityId), value)!;

    private static readonly Func<TValue, TEntityId> _factory = CreateFactory();

    private static Func<TValue, TEntityId> CreateFactory()
    {
        var constructor = typeof(TEntityId)
            .GetConstructor(BindingFlags.Public | BindingFlags.Instance,
                binder: null,
                [typeof(TValue)],
                modifiers: null)
            ?? throw new InvalidOperationException($"The type {typeof(TEntityId).Name} must have a public constructor with parameter ({typeof(TValue).Name}).");

        var parameter = Expression.Parameter(typeof(TValue), "value");

        var newExpression = Expression.New(constructor, parameter);

        return Expression.Lambda<Func<TValue, TEntityId>>(newExpression, parameter).Compile();
    }

    public static implicit operator TValue(EntityId<TEntityId, TValue> entityId)
        => entityId.Value;
}

public abstract record GuidEntityId<TEntityId>(Guid Value)
    : EntityId<TEntityId, Guid>(Value)
    where TEntityId : GuidEntityId<TEntityId>
{
    public static TEntityId New()
        => Create(Guid.CreateVersion7());

    public static TEntityId Existing(Guid value)
        => Create(value);
}

public abstract record NumberEntityId<TEntityId>(long Value)
    : EntityId<TEntityId, long>(Value)
    where TEntityId : NumberEntityId<TEntityId>
{
    public static TEntityId New()
        => Create(0L);

    public static TEntityId Existing(long value)
        => Create(value);
}
