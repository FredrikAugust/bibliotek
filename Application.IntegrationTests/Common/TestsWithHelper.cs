using Application.Common.Interfaces;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace Application.IntegrationTests.Common;

public abstract class TestsWithHelper
{
    protected readonly TestHelper Helper;
    
    protected readonly IServiceScope Scope;

    protected readonly ISender Mediator;
    
    protected readonly IApplicationDbContext ApplicationDbContext;

    protected TestsWithHelper()
    {
        Helper = new TestHelper();
        Scope = Helper.ServiceScopeFactory.CreateScope();
        Mediator = Scope.ServiceProvider.GetRequiredService<ISender>();
        ApplicationDbContext = Scope.ServiceProvider.GetRequiredService<IApplicationDbContext>();
    }
}