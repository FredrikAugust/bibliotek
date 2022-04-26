using Application.Common.Interfaces;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Serilog;

namespace Application.Rentals.Queries.Active;

public class ActiveRentalsQuery : IRequest<IEnumerable<ActiveRentalDto>>
{
}

public class ActiveRentalsQueryHandler : IRequestHandler<ActiveRentalsQuery, IEnumerable<ActiveRentalDto>>
{
    private readonly IMapper _mapper;
    private readonly ILogger _logger;
    private readonly IApplicationDbContext _applicationDbContext;
    private readonly ICurrentUserService _currentUserService;

    public ActiveRentalsQueryHandler(IMapper mapper, ILogger logger, IApplicationDbContext applicationDbContext, ICurrentUserService currentUserService)
    {
        _mapper = mapper;
        _logger = logger;
        _applicationDbContext = applicationDbContext;
        _currentUserService = currentUserService;
    }

    public async Task<IEnumerable<ActiveRentalDto>> Handle(ActiveRentalsQuery request, CancellationToken cancellationToken)
    {
        return await _applicationDbContext.Rentals.AsNoTracking().Where(rental => rental.UserId == _currentUserService.UserId)
            .Include(rental => rental.Book).ProjectTo<ActiveRentalDto>(_mapper.ConfigurationProvider).ToListAsync(cancellationToken);
    }
}