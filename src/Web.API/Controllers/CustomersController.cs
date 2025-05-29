using Application.Customers.Create;
using Application.Customers.Delete;
using Application.Customers.GetAll;
using Application.Customers.GetById;
using Application.Customers.Update;
using Domain.Customers;
using ErrorOr;
using MediatR;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Web.API.Controllers
{
    [Route("customers")]
    public class CustomersController : ApiController
    {
        private readonly ISender _mediator;

        public CustomersController(ISender mediator)
        {
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var customerResult = await _mediator.Send(new GetAllCustomerCommand());

            return customerResult.Match(
                customers => Ok(customers),
                errors => Problem(errors)
                );
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var customerResult = await _mediator.Send(new GetCustomerByIdQuery(id));

            return customerResult.Match(
                customers => Ok(customers),
                errors => Problem(errors)
                );
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateCustomerCommand command)
        {
            var createCustomerResult = await _mediator.Send(command);

            return createCustomerResult.Match(
                customer => Ok(),
                errors => Problem(errors)
                );
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] UpdateCustomerCommand command)
        {
            if (command.Id != id)
            {
                List<Error> errors = new()
                {
                    Error.Validation("Customer.UpdateInvalid", "The request id does not match with the url id")
                };
                return Problem(errors);
            }

            var updateResult = await _mediator.Send(command);

            return updateResult.Match(
                customerId => NoContent(),
                errors => Problem(errors)
                );
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var customerResult = await _mediator.Send(new DeleteCustomerCommand(id));

            return customerResult.Match(
                customerId => NoContent(),
                errors => Problem(errors)
                );
        }
    }
}
