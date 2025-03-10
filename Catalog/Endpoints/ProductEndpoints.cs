namespace Catalog.Endpoints;

public static class ProductEndpoints
{
    public static void MapProductEndpoints(this IEndpointRouteBuilder endpointRouteBuilder)
    {
        var group = endpointRouteBuilder.MapGroup("/products");

        //GET /products
        group.MapGet("/", async (ProductService productService) =>
        {
            var products = await productService.GetProductsAsync();
            return Results.Ok(products);
        })
          .WithName("GetAllProducts")
          .Produces<List<Product>>(StatusCodes.Status200OK);


        //GET /products/{id}    
        group.MapGet("/{id}", async (int id, ProductService productService) =>
        {
            var product = await productService.GetProductByIdAsync(id);
            if (product is null)
            {
                return Results.NotFound();
            }
            return Results.Ok(product);
        })
          .WithName("GetProductById")
          .Produces<Product>(StatusCodes.Status200OK)
          .Produces(StatusCodes.Status404NotFound);


        //PUT /products/{id}
        group.MapPut("/{id}", async (int id, Product inputProduct, ProductService productService) =>
        {
            var updatedProduct = await productService.GetProductByIdAsync(id);
            if (updatedProduct is null)
            {
                return Results.NotFound();
            }
            await productService.UpdateProduct(updatedProduct, inputProduct);
            return Results.NoContent();
        })
          .WithName("UpdateProduct")
          .Produces(StatusCodes.Status204NoContent)
          .Produces(StatusCodes.Status404NotFound);

        //DELETE /products/{id}
        group.MapDelete("/{id}", async (int id, ProductService productService) =>
        {
            var deletedProduct = await productService.GetProductByIdAsync(id);
            if (deletedProduct is null)
            {
                return Results.NotFound();
            }
            await productService.DeleteProductAsync(deletedProduct);
            return Results.NoContent();
        })
          .WithName("DeleteProduct")
          .Produces(StatusCodes.Status204NoContent)
          .Produces(StatusCodes.Status404NotFound);


        //POST /products
        group.MapPost("/", async (Product product, ProductService productService) =>
        {
            await productService.CreateProductAsync(product);
            return Results.Created($"/products/{product.Id}", product);
        })
          .WithName("CreateProduct")
          .Produces<Product>(StatusCodes.Status201Created);

    }

}
