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
    private readonly IApplicationContext _applicationContext;
    private readonly ICurrentUserService _currentUserService;

    public DeliverCommandHandler(IMapper mapper, ILogger logger, IApplicationContext applicationContext, ICurrentUserService currentUserService)
    {
        _mapper = mapper;
        _logger = logger;
        _applicationContext = applicationContext;
        _currentUserService = currentUserService;
    }

    public async Task<bool> Handle(DeliverCommand request, CancellationToken cancellationToken)
    {
        var rental = await _applicationContext.Rentals.FindAsync(new object?[] { request.RentalId }, cancellationToken: cancellationToken);
        
        if (rental == null || rental.UserId != _currentUserService.UserId)
            return false;

        if (rental.End != null)
        {
            _logger.Debug("Attempted to hand in already delivered book {}. Rental id: {}", rental.BookId, rental.Id);
            return false;
        }
        
        rental.End = DateTime.Now;

        await _applicationContext.SaveAsync(cancellationToken);
        
        _logger.Information("Book {} was handed in", rental.BookId);

        return true;
    }
}