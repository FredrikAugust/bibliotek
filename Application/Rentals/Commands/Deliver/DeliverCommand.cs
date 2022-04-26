using Application.Common.Interfaces;
using AutoMapper;
using MediatR;
using Serilog;

namespace Application.Rentals.Commands.Deliver;

public class DeliverCommand : IRequest<bool>
{
    public Guid RentalId { get; set; }
}

public class DeliverCommandHandler : IRequestHandler<DeliverCommand, bool>
{
    private readonly IMapper _mapper;
    private readonly ILogger _logger;
    private readonly IApplicationDbContext _applicationDbContext;
    private readonly ICurrentUserService _currentUserService;

    public DeliverCommandHandler(IMapper mapper, ILogger logger, IApplicationDbContext applicationDbContext, ICurrentUserService currentUserService)
    {
        _mapper = mapper;
        _logger = logger;
        _applicationDbContext = applicationDbContext;
        _currentUserService = currentUserService;
    }

    public async Task<bool> Handle(DeliverCommand request, CancellationToken cancellationToken)
    {
        var rental = await _applicationDbContext.Rentals.FindAsync(new object?[] { request.RentalId }, cancellationToken: cancellationToken);
        
        if (rental == null || rental.UserId != _currentUserService.UserId)
            return false;

        if (rental.End != null)
        {
            _logger.Debug("Attempted to hand in already delivered book {@BookId}. Rental id: {@RentalId}", rental.BookId, rental.Id);
            return false;
        }
        
        rental.End = DateTime.Now;

        await _applicationDbContext.SaveAsync(cancellationToken);
        
        _logger.Information("Book {@BookId} was handed in", rental.BookId);

        return true;
    }
}