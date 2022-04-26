using Application.Common.Interfaces;
using AutoMapper;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Serilog;

namespace Application.Rentals.Commands.CreateRental;

public class CreateRentalCommand : IRequest<CreateRentalVm?>
{
    public Guid BookId { get; set; }
}

public class CreateRentalCommandHandler : IRequestHandler<CreateRentalCommand, CreateRentalVm?>
{
    private readonly IMapper _mapper;
    private readonly ILogger _logger;
    private readonly IApplicationDbContext _applicationDbContext;
    private readonly ICurrentUserService _currentUserService;

    public CreateRentalCommandHandler(IMapper mapper, ILogger logger, IApplicationDbContext applicationDbContext, ICurrentUserService currentUserService)
    {
        _mapper = mapper;
        _logger = logger;
        _applicationDbContext = applicationDbContext;
        _currentUserService = currentUserService;
    }

    public async Task<CreateRentalVm?> Handle(CreateRentalCommand request, CancellationToken cancellationToken)
    {
        var book = await _applicationDbContext.Books.AsNoTracking().FirstOrDefaultAsync(book => book.Id == request.BookId, cancellationToken: cancellationToken);

        if (book == null || _currentUserService.UserId == null)
            return null;

        var rental = new Rental
        {
            BookId = request.BookId,
            Start = DateTime.Now,
            End = null,
            UserId = _currentUserService.UserId
        };

        _applicationDbContext.Rentals.Add(rental);

        await _applicationDbContext.SaveAsync(cancellationToken);
        
        _logger.Information("Book {} is now being lent to {@Rental}", book.Id, rental.UserId);

        return _mapper.Map<CreateRentalVm>(rental);
    }
}
