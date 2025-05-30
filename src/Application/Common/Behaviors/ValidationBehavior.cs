﻿using ErrorOr;
using FluentValidation;
using FluentValidation.Results;
using MediatR;

namespace Application.Common.Behaviors
{
    public class ValidationBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
           where TRequest : IRequest<TResponse>
           where TResponse : IErrorOr
    {
        private readonly IValidator<TRequest>? _validator;   

        public ValidationBehavior(IValidator<TRequest>? validator = null)   
        {
            _validator = validator;
        }

        public async Task<TResponse> Handle(
            TRequest request,
            RequestHandlerDelegate<TResponse> next,
            CancellationToken cancellationToken)
        {
            if (_validator is null)
            {
                return await next();
            }

            var validatorResult = await _validator.ValidateAsync(request, cancellationToken);   

            if (validatorResult.IsValid)
            {
                return await next();
            }

            var errors = validatorResult.Errors
                          .ConvertAll(ValidationFailure => Error.Validation(
                              ValidationFailure.PropertyName,
                              ValidationFailure.ErrorMessage 
                              ));

            return (dynamic)errors;
        }
    }
}
