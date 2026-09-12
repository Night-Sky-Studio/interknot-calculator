using System.Runtime.CompilerServices;

namespace InterknotCalculator.Test;

internal static class VerifyModuleInitializer {
    [ModuleInitializer]
    public static void Initialize() {
        DerivePathInfo((sourceFile, projectDir, type, method) => new PathInfo(
            directory: Path.Combine(Path.GetDirectoryName(sourceFile)!, "Snapshots"),
            typeName: type.Name,
            methodName: method.Name));
        
        VerifierSettings.AddExtraSettings(s => {
            s.Converters.Add(new DamageDoubleConverter());
        });
    }
}

internal class DamageDoubleConverter : WriteOnlyJsonConverter<double> {
    public override void Write(VerifyJsonWriter writer, double value) {
        writer.WriteValue(Math.Round(value, 2));
    }
}