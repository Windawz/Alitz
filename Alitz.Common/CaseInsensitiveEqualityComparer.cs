using System;
using System.Collections.Generic;

namespace Alitz;
public class CaseInsensitiveEqualityComparer : EqualityComparer<string> {
    private CaseInsensitiveEqualityComparer(StringComparison comparison) {
        _comparison = comparison;
    }

    private readonly StringComparison _comparison;

    public new static CaseInsensitiveEqualityComparer Default { get; } =
        new(StringComparison.CurrentCultureIgnoreCase);
    public static CaseInsensitiveEqualityComparer Invariant { get; } =
        new(StringComparison.InvariantCultureIgnoreCase);
    public static CaseInsensitiveEqualityComparer Ordinal { get; } =
        new(StringComparison.OrdinalIgnoreCase);

    public override bool Equals(string? x, string? y) {
        return string.Equals(x, y, _comparison);
    }

    public override int GetHashCode(string obj) {
        throw new NotSupportedException();
    }
}
