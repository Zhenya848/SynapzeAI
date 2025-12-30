using System.Text.Json;
using Core;
using CSharpFunctionalExtensions;
using UserService.Domain.Shared;

namespace UserService.Application.Providers;

public interface IMessageProvider
{
    public Task<UnitResult<Error>> SendCode(ChatInfoDto chatInfo, string code);
    public Result<ChatInfoDto, Error> GetChatInfo(JsonElement json);
}