using Application.Books.Queries.GetAllBooks;
using Application.Common.Interfaces;
using AutoMapper;
using Domain.Entities;
using Domain.ValueObjects;
using MediatR;
using Serilog;

namespace Application.Books.Commands.CreateBook;

public class CreateBookCommand : IRequest<BriefBookDto?>
{
    public string Name { get; set; }
    
    public string IsbnRaw { get; set; }
}

public class CreateBookCommandHandler : IRequestHandler<CreateBookCommand, BriefBookDto?>
{
    private readonly IMapper _mapper;
    private readonly ILogger _logger;
    private readonly IApplicationDbContext _applicationDbContext;

    public CreateBookCommandHandler(IMapper mapper, ILogger logger, IApplicationDbContext applicationDbContext)
    {
        _mapper = mapper;
        _logger = logger;
        _applicationDbContext = applicationDbContext;
    }

    public async Task<BriefBookDto?> Handle(CreateBookCommand request, CancellationToken cancellationToken)
    {
        _logger.Debug("Attempting to create book from {@Request}", request);
        
        var book = new Book
        {
            Isbn = new Isbn(request.IsbnRaw),
            Name = request.Name
        };

        var result = _applicationDbContext.Books.Add(book);

        await _applicationDbContext.SaveAsync(cancellationToken);
        
        _logger.Information("Created new book {}", book.Id);

        return _mapper.Map<BriefBookDto>(result.Entity);
    }
}