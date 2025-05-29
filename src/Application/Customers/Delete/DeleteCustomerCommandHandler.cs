using Domain.Customers;
using Domain.Primitives;
using ErrorOr;
using MediatR;

namespace Application.Customers.Delete
{
    internal sealed class DeleteCustomerCommandHandler(
        ICustomerRepository customerRepository,
        IUnitOfWork unitOfWork
        ) : IRequestHandler<DeleteCustomerCommand, ErrorOr<Unit>>
    {
        public async Task<ErrorOr<Unit>> Handle(
            DeleteCustomerCommand command,
            CancellationToken cancellationToken
            )
        {
            if(await customerRepository.GetByIdAsync(new CustomerId(command.Id)) is not Customer customer)
            {
                return Error.NotFound("Customer.NotFound", "The customer with the provide Id was not found");
            }

            customerRepository.Delete(customer);

            await unitOfWork.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}
