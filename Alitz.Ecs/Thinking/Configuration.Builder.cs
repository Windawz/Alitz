using System;
using System.Collections.Generic;

namespace Alitz.Thinking;
internal readonly partial struct Configuration
{
    internal class Builder : IDependencyConfigurationBuilder
    {
        private readonly List<Type> _dependencies = new();

        /// <inheritdoc />
        IDependencyConfigurationBuilder IDependencyConfigurationBuilder.DependsOn<TThinker>() =>
            DependsOn<TThinker>();

        /// <inheritdoc />
        IDependencyConfigurationBuilder IDependencyConfigurationBuilder.DependsOn(Type thinkerType) =>
            DependsOn(thinkerType);

        public Builder DependsOn<TThinker>() =>
            DependsOnImpl(typeof(TThinker));

        public Builder DependsOn(Type thinkerType)
        {
            if (!thinkerType.IsAssignableTo(typeof(Thinker)))
            {
                throw new ArgumentException("Type is not a thinker", nameof(thinkerType));
            }
            return DependsOnImpl(thinkerType);
        }

        public Configuration Build() =>
            new(_dependencies);

        private Builder DependsOnImpl(Type thinkerType)
        {
            _dependencies.Add(thinkerType);
            return this;
        }
    }
}
