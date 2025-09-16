using Core;
using CSharpFunctionalExtensions;
using Microsoft.EntityFrameworkCore;
using TestsService.Application.Abstractions;
using TestsService.Application.Repositories;
using TestsService.Domain.Shared;
using TestsService.Domain.Shared.ValueObjects.Dtos.ForQuery;

namespace TestsService.Application.Tests.Commands.GetTest;

public class GetTestHandler : ICommandHandler<Guid, Result<TestDto, ErrorList>>
{
    private readonly IReadDbContext _readDbContext;

    public GetTestHandler(IReadDbContext readDbContext)
    {
        _readDbContext = readDbContext;
    }
    
    public async Task<Result<TestDto, ErrorList>> Handle(
        Guid testId, 
        CancellationToken cancellationToken = default)
    {
        var testResult = await _readDbContext.Tests
            .Include(t => t.Tasks.OrderBy(sn => sn.SerialNumber))
            .FirstOrDefaultAsync(i => i.Id == testId, cancellationToken);
        
        if (testResult == null)
            return (ErrorList)Errors.General.NotFound(testId);
        
        return testResult;
    }
}