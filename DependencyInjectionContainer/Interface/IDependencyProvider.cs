namespace DependencyInjectionContainer
{
    interface IDependencyProvider
    {
        TDependency Resolve<TDependency>();
    }
}
