using Formri.ApiService.Controllers;
using Formri.Domain.Models.ContactForm;
using Formri.Domain.Services.User;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace Formri.Tests.Api.Controllers
{
    public class ContactControllerTests
    {
        private readonly Mock<IUserService> _mockUserService;
        private readonly ContactController _controller;

        public ContactControllerTests()
        {
            _mockUserService = new Mock<IUserService>();
            _controller = new ContactController(_mockUserService.Object);
        }

        [Fact]
        public async Task SubmitForm_ReturnsOk_WhenSubmissionIsSuccessful()
        {
            // Arrange
            var model = new ContactFormModel
            {
                FirstName = "John",
                LastName = "Doe",
                Email = "john.doe@example.com"
            };

            _mockUserService.Setup(service => service.AddContactFormUser(model, It.IsAny<CancellationToken>()))
                .ReturnsAsync(true);

            // Act
            var result = await _controller.SubmitForm(model);

            // Assert
            var actionResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal("Form submitted successfully.", actionResult.Value);
        }

        [Fact]
        public async Task SubmitForm_ReturnsBadRequest_WhenModelStateIsInvalid()
        {
            // Arrange
            var model = new ContactFormModel
            {
                FirstName = "",
                LastName = "Doe",
                Email = "john.doe@example.com"
            };
            _controller.ModelState.AddModelError("FirstName", "The FirstName field is required.");

            // Act
            var result = await _controller.SubmitForm(model);

            // Assert
            var actionResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("All model parameters must have values!", actionResult.Value);
        }

        [Fact]
        public async Task SubmitForm_ReturnsBadRequest_WhenSubmissionFails()
        {
            // Arrange
            var model = new ContactFormModel
            {
                FirstName = "John",
                LastName = "Doe",
                Email = "john.doe@example.com"
            };

            _mockUserService.Setup(service => service.AddContactFormUser(model, It.IsAny<CancellationToken>()))
                .ReturnsAsync(false);

            // Act
            var result = await _controller.SubmitForm(model);

            // Assert
            var actionResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("An error happened while trying to add new user. A minute must pass until the same user can be resubmitted", actionResult.Value);
        }
    }
}
