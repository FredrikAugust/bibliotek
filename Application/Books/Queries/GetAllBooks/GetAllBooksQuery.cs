using Application.Common.Interfaces;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Serilog;

namespace Application.Books.Queries.GetAllBooks;

public class GetAllBooksQuery : IRequest<GetAllBooksVm>
{
}

public class GetAllBooksQueryHandler : IRequestHandler<GetAllBooksQuery, GetAllBooksVm>
{
    private readonly IMapper _mapper;
    private readonly ILogger _logger;
    private readonly IApplicationContext _applicationContext;

    public GetAllBooksQueryHandler(IMapper mapper, ILogger logger, IApplicationContext applicationContext)
    {
        _mapper = mapper;
        _logger = logger;
        _applicationContext = applicationContext;
    }

    public async Task<GetAllBooksVm> Handle(GetAllBooksQuery request, CancellationToken cancellationToken)
    {
        var books = await _applicationContext.Books.AsNoTracking().ProjectTo<BookDto>(_mapper.ConfigurationProvider)
            .ToListAsync(cancellationToken: cancellationToken);

        _logger.Debug("Found {Count} books", books.Count);

        return new GetAllBooksVm
        {
            Books = books
        };
    }
}