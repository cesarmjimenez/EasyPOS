using Domain.Customers;
using Domain.Primitives;
using Domain.ValueObjects;
using ErrorOr;
using MediatR;
using Domain.DomainErrors;

namespace Application.Customers.Create
{
    public sealed class CreateCustomerCommandHandler : IRequestHandler<CreateCustomerCommand, ErrorOr<Unit>>
    {
        private readonly ICustomerRepository _customerRepository;
        private readonly IUnitOfWork _unitOfWork;

        public CreateCustomerCommandHandler(IUnitOfWork unitOfWork, ICustomerRepository customerRepository)
        {
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
            _customerRepository = customerRepository ?? throw new ArgumentNullException(nameof(customerRepository));
        }

        public async Task<ErrorOr<Unit>> Handle(CreateCustomerCommand command, CancellationToken cancellationToken)
        {

            if (PhoneNumber.Created(command.PhoneNumber) is not PhoneNumber phoneNumber)
            {
                return Errors.Customer.PhoneNumberWithBadFormat;
            }

            if (Address.Create(command.Country, command.Line1, command.Line2, command.City,
                command.State, command.ZipCode) is not Address address)
            {
                return Errors.Customer.AddressWithBadFormat;
            }

            var customer = new Customer(
                new CustomerId(Guid.NewGuid()),
                command.Name,
                command.LastName,
                command.Email,
                phoneNumber,
                address,
                true);

            _customerRepository.Add(customer);

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return Unit.Value;

        }

    }
}
