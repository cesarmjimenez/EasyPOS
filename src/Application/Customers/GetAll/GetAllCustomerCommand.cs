using Application.Customers.Common;
using ErrorOr;
using MediatR;

namespace Application.Customers.GetAll
{
    public record GetAllCustomerCommand() : IRequest<ErrorOr<IReadOnlyList<CustomerResponse>>>;
}
