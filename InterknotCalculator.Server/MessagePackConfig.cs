using MessagePack;
using MessagePack.Resolvers;

namespace InterknotCalculator.Server;

public static class MessagePackConfig {
    public static readonly MessagePackSerializerOptions Options =
        MessagePackSerializerOptions.Standard.WithResolver(
            CompositeResolver.Create(
                InterknotResolver.Instance,
                StandardResolverAllowPrivate.Instance
            ));
}