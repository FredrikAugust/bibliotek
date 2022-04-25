using AutoMapper;
using Domain.Entities;
using Domain.ValueObjects;
using MediatR;
using Serilog;

namespace Application.Books.Queries.GetAllBooks;

public class GetAllBooksQuery  : IRequest<IEnumerable<BookDto>>
{
}

public class GetAllBooksQueryHandler : IRequestHandler<GetAllBooksQuery, IEnumerable<BookDto>>
{
    private readonly IMapper _mapper;
    private readonly ILogger _logger;
    
    public GetAllBooksQueryHandler(IMapper mapper, ILogger logger)
    {
        _mapper = mapper;
        _logger = logger;
    }
    
    public Task<IEnumerable<BookDto>> Handle(GetAllBooksQuery request, CancellationToken cancellationToken)
    {
        var books = new List<Book>
        {
            new()
            {
                Id = Guid.NewGuid(),
                Isbn = new Isbn("123"),
                Name = "How to be an Elf"
            },
            new()
            {
                Id = Guid.NewGuid(),
                Isbn = new Isbn("456"),
                Name = "How to stop being an Elf"
            }
        };
        
        _logger.Debug("Found {Count} books", books.Count);       

        return Task.FromResult(_mapper.Map<IList<Book>, IEnumerable<BookDto>>(books));
    }
}