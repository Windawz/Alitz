using System;

namespace Alitz;
public readonly struct Variant<T1, T2> where T1 : notnull where T2 : notnull {
    public Variant(T1 value) {
        _value1 = value;
    }

    public Variant(T2 value) {
        _value2 = value;
    }

    private readonly T1? _value1 = default;
    private readonly T2? _value2 = default;

    public readonly object Value =>
        _value1 is not null ? _value1 :
        _value2 is not null ? _value2 : throw new InvalidOperationException("Variant is empty");

    public readonly T1 Value1 =>
        Get<T1>();

    public readonly T2 Value2 =>
        Get<T2>();

    public readonly bool IsEmpty =>
        _value1 is null && _value2 is null;

    public static explicit operator T1(Variant<T1, T2> variant) =>
        variant.Get<T1>();

    public static explicit operator T2(Variant<T1, T2> variant) =>
        variant.Get<T2>();

    public static implicit operator Variant<T1, T2>(T1 value) =>
        new(value);

    public static implicit operator Variant<T1, T2>(T2 value) =>
        new(value);

    public readonly T Get<T>() {
        if (TryGet<T>(out var output)) {
            return output!;
        }
        throw new InvalidOperationException(
            $"No value of type {typeof(T)} is being stored in {nameof(Variant<T1, T2>)}"
        );
    }

    public readonly bool TryGet<T>(out T output) {
        if (_value1 is T value1) {
            output = value1;
            return true;
        }
        if (_value2 is T value2) {
            output = value2;
            return true;
        }
        output = default!;
        return false;
    }

    public readonly bool Is<T>() =>
        _value1 is T && _value2 is null || _value1 is null && _value2 is T;
}
