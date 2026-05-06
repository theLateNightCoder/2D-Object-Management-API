using individueelProject.Controllers;
using individueelProject.Repository.Environment2DRepo;
using individueelProject.Repository.Models;
using individueelProject.Services;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace AppTest
{
    [TestClass]
    public sealed class Test1
    {
        private Mock<IEnivronmentRepository> _repository;
        private Mock<IAuthenticationService> _service;
        private EnvironmentController _controller;

        [TestInitialize]
        public void Setup()
        {
            _repository  = new Mock<IEnivronmentRepository>();
            _service  = new Mock<IAuthenticationService>();
            _controller = new EnvironmentController(_repository.Object, _service.Object);
        }


        //Test if the enviroment name is short
        [TestMethod]
        public async Task Create_NameIsTooShort_ReturnsBadRequest()
        {
            var environment = new Environment2DDTO
            {
                Name = "",
                MaxHeight = 10,
                MaxLength = 10
            };

            var result = await _controller.Create(environment);

            Assert.IsInstanceOfType(result, typeof(BadRequestObjectResult));
        }


        //Test if the enviroment name is too long

        [TestMethod]
        public async Task Create_NameIsTooLong_ReturnsBadRequest()
        {
            var environment = new Environment2DDTO
            {
                Name =  "aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa",
                MaxHeight = 10,
                MaxLength = 10
            };

            var result = await _controller.Create(environment);

            Assert.IsInstanceOfType(result, typeof(BadRequestObjectResult));
        }

        //Test if the enviroment name is alread exist

        [TestMethod]
        public async Task Create_NameAlreadyExists_ReturnsConflict()
        {
            var environment = new Environment2DDTO
            {
                Name = "Environment",
                MaxHeight = 10,
                MaxLength = 10
            };


            //Get user id
            _service.Setup(s => s.GetCurrentAuthenticatedUserId()).Returns("user123");

            //User does  has an Environments with the same name
            _repository.Setup(r => r.GetByUserAndNameAsync("user123", "Environment")).ReturnsAsync(new Environment2D());

            var result = await _controller.Create(environment);

            Assert.IsInstanceOfType(result, typeof(ConflictObjectResult));
        }

        //Test if the user has max enviroments

        [TestMethod]
        public async Task Create_MaxEnvironments_ReturnsBadRequest()
        {
            var environment = new Environment2DDTO
            {
                Name = "New",
                MaxHeight = 10,
                MaxLength = 10
            };


            //Get user id
            _service.Setup(s => s.GetCurrentAuthenticatedUserId()).Returns("user123");

            //User does not has any Environments with the same name
            _repository.Setup(r => r.GetByUserAndNameAsync("user123", "New")).ReturnsAsync((Environment2D)null);

            //User has more than 5 enviroments
            _repository.Setup(r => r.CountByUserAsync("user123")).ReturnsAsync(5);

            var result = await _controller.Create(environment);

            Assert.IsInstanceOfType(result, typeof(BadRequestObjectResult));
        }


        //Test if the enviroment can be created if the validation was good

        [TestMethod]
        public async Task Create_ValidEnvironment_ReturnsOk()
        {
            var environment = new Environment2DDTO
            {
                Name = "Environment",
                MaxHeight = 10,
                MaxLength = 10
            };


            //Get user id
            _service.Setup(s => s.GetCurrentAuthenticatedUserId()).Returns("user123");

            //User does not has any Environments with the same name
            _repository.Setup(r => r.GetByUserAndNameAsync("user123", "Environment")).ReturnsAsync((Environment2D)null);

            //User has less than 5 enviroments
            _repository.Setup(r => r.CountByUserAsync("user123")).ReturnsAsync(2);

            //Adding the Environment to the database was good 
            _repository.Setup(r => r.AddAsync(It.IsAny<Environment2D>())).ReturnsAsync(1);

            var result = await _controller.Create(environment);

            Assert.IsInstanceOfType(result, typeof(OkResult));
        }

    }
     
}
