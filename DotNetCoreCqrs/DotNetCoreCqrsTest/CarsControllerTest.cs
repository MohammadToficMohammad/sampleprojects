using DotNetCoreCqrs.Controllers;
using DotNetCoreCqrs.CQRS;
using DotNetCoreCqrs.Models.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using System;
using System.Threading.Tasks;


namespace DotNetCoreCqrsTest
{
    public class CarsControllerTest
    {

        public Mock<ICqrsAggregate> cqrsCommandsHandler;
        public Mock<ICqrsProjection> cqrsQueriesHandler;
        public CarsController carsController;

        [SetUp]
        public void Setup()
        {
            cqrsCommandsHandler = new Mock<ICqrsAggregate>();
            cqrsQueriesHandler = new Mock<ICqrsProjection>();
           
        }

        [Test]
        public void GetCarTest()
        {
            var car = new Car();
            car.Color = "YELLOW";
            car.Model = CARMODEL.BMW;
            cqrsQueriesHandler.Setup(x => x.HandleAsync<Car>(It.IsAny<CqrsQuery>())).Returns(Task.FromResult(car));
            carsController = new CarsController(cqrsCommandsHandler.Object, cqrsQueriesHandler.Object);
            var result=(OkObjectResult)carsController.GetCar(1).Result;
            Assert.IsTrue(result.StatusCode.Equals(StatusCodes.Status200OK));
            Assert.IsTrue(((Car)result.Value).Color.Equals("YELLOW"));
        }
    }
}