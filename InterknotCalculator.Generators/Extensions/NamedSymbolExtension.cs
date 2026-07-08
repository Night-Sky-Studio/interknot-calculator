using Microsoft.CodeAnalysis;

namespace InterknotCalculator.Generators.Extensions;

public static class NamedSymbolExtension {
    public static bool InheritsFrom(this INamedTypeSymbol type, string baseFqn) {
        for (var t = type.BaseType; t is not null; t = t.BaseType)
            if (t.ToDisplayString() == baseFqn) return true;
        return false;
    }
}