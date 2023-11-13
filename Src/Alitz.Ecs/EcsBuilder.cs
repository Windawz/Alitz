using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;



namespace Alitz.Ecs;
public class EcsBuilder
{
    private readonly List<Type> _systemTypes = new();
    private readonly Dictionary<Type, Func<ISystem>> _factories = new();
    private bool _doValidateFactories = true;

    public EcsBuilder SetFactoryValidation(bool doValidateFactories)
    {
        _doValidateFactories = doValidateFactories;
        return this;
    }

    public EcsBuilder AddSystem(Type systemType, Func<ISystem>? factory = null)
    {
        if (factory is not null)
        {
            if (_doValidateFactories)
            {
                var system = factory();
                if (system.GetType() != systemType)
                {
                    throw new FactoryReturnTypeMismatchException(systemType, factory, system);
                }
            }
        }
        else
        {
            var parameterlessConstructor = systemType.GetConstructor(
                bindingAttr: BindingFlags.Public | BindingFlags.Instance,
                types: Array.Empty<Type>()
            );

            if (parameterlessConstructor is not null)
            {
                factory = Expression.Lambda<Func<ISystem>>(
                    Expression.New(parameterlessConstructor)
                )
                .Compile();
            }
            else
            {
                throw new FactoryResolutionException(systemType, Array.Empty<Type>());
            }
        }

        _systemTypes.Add(systemType);
        _factories[systemType] = factory;

        return this;
    }

    public EntityComponentSystem Build()
    {
        return new EntityComponentSystem(
            new SystemSchedule(_systemTypes)
                .Select(systemType => _factories[systemType]())
                .ToArray()
        );
    }
}
