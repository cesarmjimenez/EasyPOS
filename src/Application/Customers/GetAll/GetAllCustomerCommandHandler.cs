using Application.Customers.Common;
using Domain.Customers;
using ErrorOr;
using MediatR;

namespace Application.Customers.GetAll
{
    internal sealed class GetAllCustomerCommandHandler(
           ICustomerRepository customerRepository
           ) : IRequestHandler<GetAllCustomerCommand, ErrorOr<IReadOnlyList<CustomerResponse>>>
    {
        public async Task<ErrorOr<IReadOnlyList<CustomerResponse>>> Handle(GetAllCustomerCommand request, CancellationToken cancellationToken)
        {
            IReadOnlyList<Customer> customers = await customerRepository.GetAll();

            return customers.Select(customer => new CustomerResponse(
                customer.Id.Value,
                customer.FullName,
                customer.Email,
                customer.PhoneNumber.Value,
                new AddressResponse(
                    customer.Address.Country,
                    customer.Address.Line1,
                    customer.Address.Line2,
                    customer.Address.City,
                    customer.Address.State,
                    customer.Address.ZipCode
                    ),
                customer.Active
                )).ToList();
        }
    }
}
