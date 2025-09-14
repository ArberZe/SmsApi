using Microsoft.AspNetCore.Mvc;
using SmsApi.Entities;
using SmsApi.Services;

namespace SmsApi.Endpoints;

public static class MessageEndpoints
{
    public static RouteGroupBuilder MapMessageEndpoints(this IEndpointRouteBuilder routes)
    {
        var group = routes.MapGroup("/api/v1/messages");

        group.MapPost("/", async (SendMessageDto dto, IMessageService service) =>
        {
            if (dto is null)
                return Results.BadRequest(new { error = "Request body is required." });

            var result = await service.SendMessageAsync(dto);
            return Results.Created($"/api/v1/messages/{result.Id}", result);
        });

        group.MapGet("/{id:int}", async (int id, IMessageService service) =>
        {
            var result = await service.GetMessageStatusAsync(id);
            return result is null ? Results.NotFound() : Results.Ok(result);
        });

        return group;
    }
}