using System;

namespace DependencyInjectionContainer
{
    public interface IDependencyConfiguration
    {
        void Register(Type dependency, Type implementation);
        void Register<TDependency, TImplementation>() where TDependency : class where TImplementation : TDependency;
    }
}
