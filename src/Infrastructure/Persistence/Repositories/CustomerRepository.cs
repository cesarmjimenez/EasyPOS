using Domain.Customers;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Repositories
{
    public class CustomerRepository(
        ApplicationDbContext context
        ) : ICustomerRepository
    {

        public void Add(Customer customer) =>  context.Customers.Add(customer);

        public void Delete(Customer customer) => context.Customers.Remove(customer);

        public void Update(Customer customer) => context.Customers.Update(customer);

        public async Task<bool> ExistsAsync(CustomerId id) => await context.Customers.AnyAsync(customer => customer.Id == id);

        public async Task<Customer?> GetByIdAsync(CustomerId id) => await context.Customers.SingleOrDefaultAsync(c => c.Id == id);
        public async Task<List<Customer>> GetAll() => await context.Customers.ToListAsync();


    }
}
