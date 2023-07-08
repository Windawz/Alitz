using System;
using System.Collections.Generic;
using System.Linq;

using Alitz.Systems;

namespace Alitz;
public partial class EntityComponentSystem
{
    public class Builder
    {
        private readonly HashSet<Type> _systemTypes = new();
        private EntityComponentSystemOptions _options = new();

        public Builder UseSystem<TSystem>() where TSystem : ISystem
        {
            _systemTypes.Add(typeof(TSystem));
            return this;
        }

        public Builder UseOptions(EntityComponentSystemOptions options)
        {
            _options = options;
            return this;
        }

        public EntityComponentSystem Build()
        {
            var schedule = new Schedule(_systemTypes.ToArray());
            return new EntityComponentSystem(_options, schedule);
        }
    }
}
