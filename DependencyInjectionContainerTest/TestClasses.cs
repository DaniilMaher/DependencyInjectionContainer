namespace DependencyInjectionContainerTest
{
    internal interface IFoo { }


    internal interface IBar { }

    internal abstract class AFoo : IFoo { }

    internal abstract class ABar : IBar { }

    internal class BarFromABar : ABar
    {

    }

    internal class BarFromIBar : IBar
    {
        AFoo foo;

        public BarFromIBar(AFoo foo)
        {
            this.foo = foo;
        }
    }

    internal class Foo : AFoo
    {
        public ABar Bar
        {
            get;
        }

        public Foo(ABar bar)
        {
            Bar = bar;
        }
    }

    internal class Nea { }

    internal class GenFoo<T>
    {
        T val;
    }

    internal class GenBar<T> where T : class
    {
        public GenBar(T t) { }
    }

}
