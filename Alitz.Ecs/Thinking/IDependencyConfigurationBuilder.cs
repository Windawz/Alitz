using System;

namespace Alitz.Thinking;
public interface IDependencyConfigurationBuilder
{
    IDependencyConfigurationBuilder DependsOn<TThinker>() where TThinker : Thinker;
    IDependencyConfigurationBuilder DependsOn(Type thinkerType);
}
