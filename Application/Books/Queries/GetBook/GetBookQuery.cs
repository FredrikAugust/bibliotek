using Application.Common.Interfaces;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Serilog;

namespace Application.Books.Queries.GetBook;

public class GetBookQuery : IRequest<BookWithRentalVm?>
{
    public Guid BookId { get; set; }
}

public class GetBookQueryHandler : IRequestHandler<GetBookQuery, BookWithRentalVm?>
{
    private readonly IApplicationDbContext _applicationDbContext;
    private readonly ILogger _logger;
    private readonly IMapper _mapper;
    
    public GetBookQueryHandler(IApplicationDbContext applicationDbContext, ILogger logger, IMapper mapper)
    {
        _applicationDbContext = applicationDbContext;
        _logger = logger;
        _mapper = mapper;
    }
    
    public async Task<BookWithRentalVm?> Handle(GetBookQuery request, CancellationToken cancellationToken)
    {
        var book = await _applicationDbContext.Books
            .AsNoTracking()
            .FirstOrDefaultAsync(book => book.Id == request.BookId, cancellationToken: cancellationToken);

        if (book == null)
        {
            _logger.Information("Requested non-existent book with ID {@BookId}", request.BookId);
            return null;
        }

        var rentals = await _applicationDbContext.Rentals.Where(rental => rental.BookId == book.Id)
            .ProjectTo<BriefRentalDto>(_mapper.ConfigurationProvider).ToListAsync(cancellationToken: cancellationToken);
        
        _logger.Debug("Found book {@Book} with active rentals {@Rentals}", book, rentals.Where(dto => dto.Active));

        return new BookWithRentalVm
        {
            Book = _mapper.Map<BookDto>(book),
            Rentals = rentals
        };
    }
}