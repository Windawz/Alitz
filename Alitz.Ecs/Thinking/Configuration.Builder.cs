using System;
using System.Collections.Generic;

namespace Alitz.Thinking;
internal partial class Configuration
{
    internal class Builder : IDependencyConfigurationBuilder
    {
        private Builder(Type thinkerType)
        {
            _thinkerType = thinkerType;
        }

        private readonly List<Type> _dependencies = new();

        private readonly Type _thinkerType;

        /// <inheritdoc />
        IDependencyConfigurationBuilder IDependencyConfigurationBuilder.DependsOn<TThinker>() =>
            DependsOn<TThinker>();

        /// <inheritdoc />
        IDependencyConfigurationBuilder IDependencyConfigurationBuilder.DependsOn(Type thinkerType) =>
            DependsOn(thinkerType);

        public static Builder Create<TThinker>() where TThinker : Thinker =>
            new(typeof(TThinker));

        public static Builder Create(Type thinkerType) =>
            new(ValidateThinkerType(thinkerType));

        public Builder DependsOn<TThinker>() =>
            DependsOnImpl(typeof(TThinker));

        public Builder DependsOn(Type thinkerType) =>
            DependsOnImpl(ValidateThinkerType(thinkerType));

        public Configuration Build() =>
            new(_thinkerType, _dependencies);

        private Builder DependsOnImpl(Type thinkerType)
        {
            _dependencies.Add(thinkerType);
            return this;
        }

        private static Type ValidateThinkerType(Type thinkerType, string? paramName = null)
        {
            if (!thinkerType.IsAssignableTo(typeof(Thinker)))
            {
                throw new ArgumentException("Type is not a thinker", paramName ?? nameof(thinkerType));
            }
            return thinkerType;
        }
    }
}
