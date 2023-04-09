using SwaggerSkills.Web;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.OpenApi;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.Build.Framework;

namespace SwaggerSkills.Web.Entities
{
    public class Customer
    {
        /// <summary>
        /// Customer Id
        /// </summary>
        public Guid Id { get; set; }
        /// <summary>
        /// Customer Name
        /// </summary>
        [Required]
        public string Name { get; set; }
        /// <summary>
        /// Customer Surname
        /// </summary>
        [Required]
        public string Surname { get; set; }
        /// <summary>
        /// Customer BirthDate
        /// </summary>
        public DateTime BirthDate { get; set; }
        /// <summary>
        /// Customer Age
        /// </summary>
        public int Age { get; set; }
    }


    public static class CustomerEndpoints
    {
        public static void MapCustomerEndpoints(this IEndpointRouteBuilder routes)
        {
            var group = routes.MapGroup("/api/Customer").WithTags(nameof(Customer));

            group.MapGet("/", async (SwaggerContext db) =>
            {
                return await db.Customers.ToListAsync();
            })
            .WithName("GetAllCustomers")
            .WithOpenApi();

            group.MapGet("/{id}", async Task<Results<Ok<Customer>, NotFound>> (Guid id, SwaggerContext db) =>
            {
                return await db.Customers.AsNoTracking()
                    .FirstOrDefaultAsync(model => model.Id == id)
                    is Customer model
                        ? TypedResults.Ok(model)
                        : TypedResults.NotFound();
            })
            .WithName("GetCustomerById")
            .WithOpenApi();

            group.MapPut("/{id}", async Task<Results<Ok, NotFound>> (Guid id, Customer customer, SwaggerContext db) =>
            {
                var affected = await db.Customers
                    .Where(model => model.Id == id)
                    .ExecuteUpdateAsync(setters => setters
                      .SetProperty(m => m.Id, customer.Id)
                      .SetProperty(m => m.Name, customer.Name)
                      .SetProperty(m => m.Surname, customer.Surname)
                      .SetProperty(m => m.BirthDate, customer.BirthDate)
                      .SetProperty(m => m.Age, customer.Age)
                    );

                return affected == 1 ? TypedResults.Ok() : TypedResults.NotFound();
            })
            .WithName("UpdateCustomer")
            .WithOpenApi();

            group.MapPost("/", async (Customer customer, SwaggerContext db) =>
            {
                db.Customers.Add(customer);
                await db.SaveChangesAsync();
                return TypedResults.Created($"/api/Customer/{customer.Id}", customer);
            })
            .WithName("CreateCustomer")
            .WithOpenApi();

            group.MapDelete("/{id}", async Task<Results<Ok, NotFound>> (Guid id, SwaggerContext db) =>
            {
                var affected = await db.Customers
                    .Where(model => model.Id == id)
                    .ExecuteDeleteAsync();

                return affected == 1 ? TypedResults.Ok() : TypedResults.NotFound();
            })
            .WithName("DeleteCustomer")
            .WithOpenApi();
        }
    }
}
