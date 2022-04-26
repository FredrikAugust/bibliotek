using Application.Common.Interfaces;
using AutoMapper;
using AutoMapper.QueryableExtensions;
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
    private readonly IApplicationDbContext _applicationDbContext;

    public GetAllBooksQueryHandler(IMapper mapper, ILogger logger, IApplicationDbContext applicationDbContext)
    {
        _mapper = mapper;
        _logger = logger;
        _applicationDbContext = applicationDbContext;
    }

    public async Task<GetAllBooksVm> Handle(GetAllBooksQuery request, CancellationToken cancellationToken)
    {
        var books = await _applicationDbContext.Books.AsNoTracking().ProjectTo<BriefBookDto>(_mapper.ConfigurationProvider)
            .ToListAsync(cancellationToken: cancellationToken);

        _logger.Debug("Found {Count} books", books.Count);

        return new GetAllBooksVm
        {
            Books = books
        };
    }
}