using System;
using DependencyInjectionContainer;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;

namespace DependencyInjectionContainerTest
{
    [TestClass]
    public class DIContainerTests
    {
        [TestMethod]
        public void DependencyCreationTest()
        {
            DependencyConfiguration configuration = new DependencyConfiguration();
            configuration.Register<IBar, BarFromABar>(false);
            DependencyProvider provider = new DependencyProvider(configuration);
            IBar bar = provider.Resolve<IBar>();
            Assert.IsNotNull(bar);
            Assert.AreEqual(bar.GetType(), typeof(BarFromABar));
        }

        [TestMethod]
        public void SingletonDependencyTest()
        {
            DependencyConfiguration configuration = new DependencyConfiguration();
            configuration.Register<IBar, BarFromABar>(true);
            DependencyProvider provider = new DependencyProvider(configuration);
            IBar bar1 = provider.Resolve<IBar>();
            IBar bar2 = provider.Resolve<IBar>();
            Assert.AreEqual(bar1, bar2);
        }

        [TestMethod]
        public void NotSingletonDependencyTest()
        {
            DependencyConfiguration conf = new DependencyConfiguration();
            conf.Register<IBar, BarFromABar>(false);    
            DependencyProvider provider = new DependencyProvider(conf);    
            IBar bar1 = provider.Resolve<IBar>();
            IBar bar2 = provider.Resolve<IBar>();    
            Assert.AreNotEqual(bar1, bar2);
        }
    
        [TestMethod]
        public void NotRegisteredDependencyTest()
        {
            DependencyConfiguration configuration = new DependencyConfiguration();
            configuration.Register<IFoo, Foo>(false);    
            DependencyProvider provider = new DependencyProvider(configuration);
                try
            {
                IBar bar = provider.Resolve<IBar>();
                Assert.Fail("Cannot create instance of not registered type");
            }
            catch (Exception e)
            {
                Assert.IsNotNull(e, e.Message);
            }
        }

        [TestMethod]
        public void NotRegisteredInnerDependencyTest()
        {
            DependencyConfiguration configuration = new DependencyConfiguration();
            configuration.Register<IFoo, Foo>(false);
            configuration.Register<IBar, BarFromIBar>(false);
            DependencyProvider provider = new DependencyProvider(configuration);
            try
            {
                IBar bar = provider.Resolve<IBar>();
                Assert.Fail("Cannot be created");
            }
            catch (Exception e)
            {
                Assert.IsNotNull(e, e.Message);
            }
        }

        [TestMethod]
        public void AbstractClassRegisterTest()
        {
            DependencyConfiguration configuration = new DependencyConfiguration();
            try
            {
                configuration.Register<ABar, ABar>(false);
                Assert.Fail("registered class is abstract");
            }
            catch (Exception e)
            {
                Assert.IsNotNull(e, e.Message);
            }
        }

        [TestMethod]
        public void InterfaceRegisterTest()
        {
            try
            {
                DependencyConfiguration configuration = new DependencyConfiguration();
                configuration.Register<IBar, IBar>(false);
                Assert.Fail("Cannot create instance of interface");
            }
            catch (Exception e)
            {
                Assert.IsNotNull(e, e.Message);
            }
        }
    
        [TestMethod]
        public void ResolveEnumerableTest()
        {
            int expected = 2;
            DependencyConfiguration configuration = new DependencyConfiguration();
            configuration.Register<IBar, BarFromIBar>(false);
            configuration.Register<IBar, BarFromABar>(false);
            configuration.Register<AFoo, Foo>(false);
            configuration.Register<ABar, BarFromABar>(false);
            DependencyProvider provider = new DependencyProvider(configuration);
            var bars = provider.Resolve<IEnumerable<IBar>>();
            Assert.AreEqual(expected, bars.Count());
        }
    
        [TestMethod]
        public void ResolveOpenGenTypeTest()
        {
            DependencyConfiguration configuration = new DependencyConfiguration();
            configuration.Register<IBar, BarFromABar>(false);
            configuration.Register<IFoo, Foo>(false);
            configuration.Register(typeof(GenFoo<>), typeof(GenFoo<>), false);    
            DependencyProvider provider = new DependencyProvider(configuration);    
            var genFoo = provider.Resolve<GenFoo<IFoo>>();
            Assert.IsNotNull(genFoo);
        }
    
        [TestMethod]
        public void ResolveGenTypeTest()
        {
            DependencyConfiguration configuration = new DependencyConfiguration();
            configuration.Register<IBar, BarFromABar>(false);
            configuration.Register(typeof(GenBar<IBar>), typeof(GenBar<IBar>), false);
            DependencyProvider provider = new DependencyProvider(configuration);    
            GenBar<IBar> ogen = provider.Resolve<GenBar<IBar>>();
            Assert.IsNotNull(ogen);
        }
    }
}
