﻿using Application.Customers.Create;
using Domain.Customers;
using Domain.Primitives;
using ErrorOr;
using FluentAssertions;
using Domain.DomainErrors;

namespace Application.Customers.UnitTests.Create;

public class CreateCustomerCommandHandlerUnitTest
{
    private readonly Mock<ICustomerRepository> _mockCustomerRepository;
    private readonly Mock<IUnitOfWork> _mockUnitOfWork;
    private readonly CreateCustomerCommandHandler _handler;

    public CreateCustomerCommandHandlerUnitTest()
    {
        _mockCustomerRepository = new Mock<ICustomerRepository>();
        _mockUnitOfWork = new Mock<IUnitOfWork>();

         
        _handler = new CreateCustomerCommandHandler(_mockUnitOfWork.Object, _mockCustomerRepository.Object);
    }

    [Fact]
    public async Task HandleCreateCustomer_WhenPhoneNumberHasBadFormat_ShouldReturnValidationError()
    {
        //Arrange
        // Se configura los parametros de entrada de nuestra prueba unitaria
        CreateCustomerCommand command = new CreateCustomerCommand("Mario", "Lopez", "al@g.com", "98",
            "", "", "", "", "", "");
        //Act
        // Se ejecuta el metodo a probar en la prueba unitaria
        var result = await _handler.Handle(command, default);
        //Assert
        // Se verifica los datos de retorno de la prueba unitaria
        result.IsError.Should().BeTrue();
        result.FirstError.Type.Should().Be(ErrorType.Validation);
        result.FirstError.Code.Should().Be(Errors.Customer.PhoneNumberWithBadFormat.Code);
        result.FirstError.Description.Should().Be(Errors.Customer.PhoneNumberWithBadFormat.Description);
    }
}
