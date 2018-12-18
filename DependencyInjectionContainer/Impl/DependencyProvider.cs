using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
namespace DependencyInjectionContainer
{
    public class DependencyProvider : IDependencyProvider
    {
        private static object obj = new object();
        private DependencyConfiguration configuration;

        public DependencyProvider(DependencyConfiguration configuration)
        {
            if (IsValidConfiguration(configuration))
            {
                this.configuration = configuration;
            }
            else
            {
                throw new Exception("Configuration is invalid");
            }
        }

        public TDependency Resolve<TDependency>() where TDependency : class
        {
            Type type = typeof(TDependency);
            
            if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(IEnumerable<>))
            {
                return ResolveAll<TDependency>();
            }
            Dependency dependency = configuration.FindDependency(type);
            if (dependency == null && type.IsGenericType)
            {
                dependency = configuration.FindDependency(type.GetGenericTypeDefinition());
                if (dependency != null)
                {
                    return (TDependency)ResolveGeneric(type, dependency);
                }
            }
            if (dependency != null)
            {
                return (TDependency)Resolve(dependency);
            }
            throw new Exception($"Dependency from type {typeof(TDependency)} is not registered");
        }

        private bool IsValidConfiguration(DependencyConfiguration configuration)
        {
            return configuration != null;
        }

        private object ResolveGeneric(Type requiredType, Dependency dependency)
        {
            Type tImplementation = dependency.TImplementation.MakeGenericType(requiredType.GenericTypeArguments);
            dependency.Instance = Create(tImplementation);
            return dependency.Instance;
        }

        private object Resolve(Dependency dependency)
        {
            if (dependency.IsSingleton && dependency.Instance != null)
            {
                return dependency.Instance;
            }
            Type tImplementation = dependency.TImplementation;
            if (tImplementation.IsGenericTypeDefinition)
            {
                tImplementation = tImplementation.MakeGenericType(dependency.TDependency.GenericTypeArguments);
            }
            dependency.Instance = Create(tImplementation);
            return dependency.Instance;

        }

        private object Create(Type tImplementation)
        {
            ConstructorInfo constructor = tImplementation.GetConstructors()
                                .OrderByDescending(x => x.GetParameters().Length)
                                .FirstOrDefault();
            object[] parameters = ResolveConstructorParameters(constructor);
            return Activator.CreateInstance(tImplementation, parameters);
        }

        private object[] ResolveConstructorParameters(ConstructorInfo constructor)
        {
            List<object> parametersValues = new List<object>();

            foreach (var parameter in constructor.GetParameters())
            {
                Type parameterType = parameter.ParameterType;
                var dependency = configuration.FindDependency(parameterType);
                if (dependency == null)
                {
                    throw new Exception($"Dependency from type {parameterType} is not registered");
                }
                parametersValues.Add(Resolve(dependency));
            }
            return parametersValues.ToArray(); ;
        }

        private TDependency ResolveAll<TDependency>()
        {
            Type nestedType = typeof(TDependency).GenericTypeArguments.FirstOrDefault();
            var dependencies = configuration.FindAllDependencies(nestedType);
            if (dependencies.Count == 0)
            {
                throw new Exception($"Dependencies from type {nestedType} are not registered");
            }
            var result = (IList)Activator.CreateInstance(typeof(List<>).MakeGenericType(nestedType));
            foreach (Dependency dependency in dependencies)
            {
                result.Add(Resolve(dependency));
            }
            return (TDependency)result;
        }
    }
}
