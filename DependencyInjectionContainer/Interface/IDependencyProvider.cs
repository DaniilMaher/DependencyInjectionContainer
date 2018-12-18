namespace DependencyInjectionContainer
{
    interface IDependencyProvider
    {
        TDependency Resolve<TDependency>() where TDependency : class;
    }
}
