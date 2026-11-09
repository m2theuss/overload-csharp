using Microsoft.Extensions.DependencyInjection;
using overload.Tests.TestUtils;

namespace overload.Fixtures;
public class TestFixture : IAsyncDisposable
{
    public ServiceProvider Provider{get;}
    public TestFixture(){
        var service = new ServiceCollection();
        service.AddScoped<IContextHandler, ContextHandler>();
        Provider = service.BuildServiceProvider();
    }
    public ValueTask DisposeAsync()
    {
        return Provider.DisposeAsync();
    }
}