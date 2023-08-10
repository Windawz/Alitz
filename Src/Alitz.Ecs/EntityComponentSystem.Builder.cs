using System;
using System.Collections.Generic;

using Alitz.Systems;

namespace Alitz;
public partial class EntityComponentSystem
{
    public class Builder
    {
        internal Builder() { }

        private readonly List<SystemFactory> _factories = new();

        public Builder AddSystem<TSystem>() where TSystem : class, ISystem, new()
        {
            _factories.Add(SystemFactory.Create(() => new TSystem()));
            return this;
        }

        public Builder AddSystem<TSystem>(Func<TSystem> factory) where TSystem : class, ISystem
        {
            _factories.Add(SystemFactory.Create(factory));
            return this;
        }

        public EntityComponentSystem Build()
        {
            var schedule = new Schedule(_factories);
            return new EntityComponentSystem(schedule);
        }
    }
}
