using Microsoft.AspNetCore.Mvc;
using Wolverine.Http;

namespace Feed.API.UserEndpoints;

public static class UserEndpoints
{
    public const string CreateEndpoint = "/api/user";

    [WolverinePost(CreateEndpoint)]
    public static async Task<IResult> CreateUserAsync(
        [FromBody] Guid userId)
    {
        await Task.CompletedTask;

        return TypedResults.Ok();
    }

    public const string ListEndpoint = "/api/user";

    [WolverineGet(ListEndpoint)]
    public static async Task<IResult> ListUsersAsync(
        [FromRoute] Guid userId)
    {
        await Task.CompletedTask;

        return TypedResults.Ok();
    }

    public const string GetEndpoint = "/api/user";

    [WolverineGet(GetEndpoint + "{id:guid}")]
    public static async Task<IResult> GetUserAsync(
        [FromRoute] Guid userId)
    {
        await Task.CompletedTask;

        return TypedResults.Ok();
    }

    public const string PutEndpoint = "/api/user";

    [WolverinePut(PutEndpoint + "{id:guid}")]
    public static async Task<IResult> UpdateUserAsync(
        [FromRoute] Guid userId)
    {
        await Task.CompletedTask;

        return TypedResults.Ok();
    }


    public const string DeleteEndpoint = "/api/user";

    [WolverineDelete(DeleteEndpoint + "{id:guid}")]
    public static async Task<IResult> DeleteUserAsync(
        [FromRoute] Guid userId)
    {
        await Task.CompletedTask;

        return TypedResults.Ok();
    }
}