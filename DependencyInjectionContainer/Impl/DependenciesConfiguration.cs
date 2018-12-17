using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DependencyInjectionContainer
{
    class DependencyConfiguration : IDependencyConfiguration
    {
        public bool IsSingletonDependency
        {
            get;
            private set;
        }

        public void Register(Type dependency, Type implementation)
        {
            throw new NotImplementedException();
        }

        public void Register<TDependency, TImplementation>() where TImplementation : class, TDependency
        {
            throw new NotImplementedException();
        }
    }
}
