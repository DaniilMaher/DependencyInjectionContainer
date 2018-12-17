using System;

namespace DependencyInjectionContainer
{
    public class Dependency
    {
        public Type TDependency
        {
            get;
            private set;
        }

        public Type TImplementation
        {
            get;
            private set;
        }

        public bool IsSingleton
        {
            get;
            private set;
        }

        public object Instance
        {
            get;
            set;
        }

        public Dependency(Type tDependenct, Type tImplementation, bool isSingleton)
        {
            TDependency = tDependenct;
            TImplementation = tImplementation;
            IsSingleton = isSingleton;
            Instance = null;
        }
    }
}
