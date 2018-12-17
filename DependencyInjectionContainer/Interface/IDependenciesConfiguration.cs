using System;

namespace DependencyInjectionContainer
{
    interface IDependencyConfiguration
    {
        void Register(Type dependency, Type implementation);
        void Register<TDependency, TImplementation>() where TImplementation : class, TDependency;
    }
}
