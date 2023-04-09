using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SwaggerSkills.Web;
using SwaggerSkills.Web.Entities;

namespace SwaggerSkills.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomersController : ControllerBase
    {
        private readonly SwaggerContext _context;

        public CustomersController(SwaggerContext context)
        {
            _context = context;
        }

        /// <summary>
        /// This endpoint returns all customers by list
        /// </summary>
        /// remarks tag= sample code
        /// <remarks>
        /// sample url=https://localhost:7236/api/customers
        /// </remarks>
        /// <returns></returns>
        // Return json doc
        [Produces("application/json")]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Customer>>> GetCustomers()
        {
            if (_context.Customers == null)
            {
                return NotFound();
            }
            return await _context.Customers.ToListAsync();
        }

        /// <summary>
        /// This endpoint returns customer by id
        /// </summary>
        /// <param name="id">Customer Id</param>
        /// <returns></returns>
        /// <response code="404">Customer not found at database</response>
        /// <response code="200">Found Customer</response>
        [Produces("application/json")]
        [HttpGet("{id}")]
        public async Task<ActionResult<Customer>> GetCustomer(Guid id)
        {
            if (_context.Customers == null)
            {
                return NotFound();
            }
            var customer = await _context.Customers.FindAsync(id);

            if (customer == null)
            {
                return NotFound();
            }

            return customer;
        }

        // PUT: api/Customers/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCustomer(Guid id, Customer customer)
        {
            if (id != customer.Id)
            {
                return BadRequest();
            }

            _context.Entry(customer).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CustomerExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Customers
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        /// <summary>
        /// This endpoint add to customer
        /// </summary>
        /// <remarks>
        /// Customer Json Sample: {
        ///"id": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
        ///"name": "Alper",
        ///"surname": "Karagöz",
        ///"birthDate": "1981-05-26T19:20:59.483Z",
        ///"age": 41
        ///}
        /// </remarks>
        /// <param name="customer">Json Customer object</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult<Customer>> PostCustomer(Customer customer)
        {
            if (_context.Customers == null)
            {
                return Problem("Entity set 'SwaggerContext.Customers'  is null.");
            }
            _context.Customers.Add(customer);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetCustomer", new { id = customer.Id }, customer);
        }

        // DELETE: api/Customers/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCustomer(Guid id)
        {
            if (_context.Customers == null)
            {
                return NotFound();
            }
            var customer = await _context.Customers.FindAsync(id);
            if (customer == null)
            {
                return NotFound();
            }

            _context.Customers.Remove(customer);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool CustomerExists(Guid id)
        {
            return (_context.Customers?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
