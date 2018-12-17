using System;
using System.Collections.Generic;

namespace DependencyInjectionContainer
{
    class DependencyConfiguration
    {
        private List<Dependency> dependencies;

        public DependencyConfiguration()
        {
            dependencies = new List<Dependency>();
        }

        public void Register(Type tDependency, Type tImplementation, bool isSingleton)
        {
            if (tImplementation.IsInterface)
            {
                throw new Exception($"Can't register dependency {nameof(tDependency)} -> {nameof(tImplementation)}, {nameof(tImplementation)} is interface");
            }
            if (tImplementation.IsAbstract)
            {
                throw new Exception($"Can't register dependency {nameof(tDependency)} -> {nameof(tImplementation)}, {nameof(tImplementation)} is abstract");
            }
            if (!tDependency.IsAssignableFrom(tImplementation)) 
            {
                throw new Exception($"Can't register dependency {nameof(tDependency)} -> {nameof(tImplementation)}, {nameof(tDependency)} shoud be assignable from {nameof(tImplementation)}");
            }

            Dependency dependency = dependencies.Find(d => (d.TDependency == tDependency && d.TImplementation == tImplementation));
            if (dependency == null)
            {
                dependency = new Dependency(tDependency, tImplementation, isSingleton);
                dependencies.Add(dependency);
            }
            else
            {
                throw new Exception($"Can't register dependency {nameof(tDependency)} -> {nameof(tImplementation)}, dependency already registered");
            }
        }

        public void Register<TDependency, TImplementation>(bool isSingleton) where TImplementation : class, TDependency
        {
            Register(typeof(TDependency), typeof(TImplementation), isSingleton);
        }
    }
}
