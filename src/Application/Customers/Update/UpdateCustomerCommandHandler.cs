using Domain.Customers;
using Domain.Primitives;
using Domain.ValueObjects;
using ErrorOr;
using MediatR;

namespace Application.Customers.Update
{
    internal sealed class UpdateCustomerCommandHandler(
        ICustomerRepository customerRepository,
        IUnitOfWork unitOfWork
        ) : IRequestHandler<UpdateCustomerCommand, ErrorOr<Unit>>
    {
        public async Task<ErrorOr<Unit>> Handle(UpdateCustomerCommand request, CancellationToken cancellationToken)
        {
            if (!await customerRepository.ExistsAsync(new CustomerId(request.Id)))
            {
                return Error.NotFound("Customer.NotFound", "The customer with the provide id was not found.");
            }

            if (PhoneNumber.Created(request.PhoneNumber) is not PhoneNumber phoneNumber)
            {
                return Error.Validation("Customer.PhoneNumber", "Phone Number has not a valid format");
            }

            if (Address.Create(request.Country, request.Line1, request.Line2,
                request.City, request.State, request.ZipCode) is not Address address)
            {
                return Error.Validation("Customer.Address", "Address is not valid");
            }

            Customer customer = Customer.UpdateCustomer(request.Id,
                request.Name,
                request.LastName,
                request.Email,
                phoneNumber,
                address,
                request.Active);

            customerRepository.Update(customer);

            await unitOfWork.SaveChangesAsync(cancellationToken);

            return Unit.Value;

        }
    }
}
