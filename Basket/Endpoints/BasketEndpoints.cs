using Basket.Models;
using Basket.Services;

namespace Basket.Endpoints;

public static class BasketEndpoints
{
    public static void MapBasketEndpoints(this WebApplication app)
    {
        var group = app.MapGroup("/basket");

        group.MapGet("/{userName}", async (string userName, BasketService service) =>
        {
            var basket = await service.GetBasket(userName);
            return basket is null ? Results.NotFound() : Results.Ok(basket);
        })
            .WithName("GetBasket")
            .Produces<ShoppingCart>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status404NotFound);


        group.MapPost("/", async (ShoppingCart shoppingCart, BasketService basketService) =>
        {
            await basketService.UpdateBasket(shoppingCart);
            return Results.Created($"/basket/{shoppingCart.UserName}", shoppingCart);
        })
            .WithName("UpdateBasket")
            .Produces<ShoppingCart>(StatusCodes.Status201Created);

        group.MapDelete("/{userName}", async (string userName, BasketService service) =>
        {
            await service.DeleteBasket(userName);
            return Results.NoContent();
        })
            .WithName("DeleteBasket")
            .Produces(StatusCodes.Status204NoContent);
    }
}
