namespace DependencyInjectionContainer
{
    public interface IDependencyProvider
    {
        TDependency Resolve<TDependency>() where TDependency : class;
    }
}
