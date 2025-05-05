using Xunit;
using Moq;
using Moq.Protected;
using backend.Controllers;
using backend.Models;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Text.Json;
using brazilian_universities_backend.Controllers.MockController;
using brazilian_universities_backend.Services;

public class UniversitiesControllerTests
{
    [Fact]
    public async Task Get_ReturnsOk_WithListOfUniversities()
    {
        // Arrange
        var mockUniversities = new List<University>
        {
            new University { Name = "UFU", Country = "Brazil" },
            new University { Name = "USP", Country = "Brazil" }
        };

        var serviceMock = new Mock<IUniversityService>();
        serviceMock.Setup(s => s.GetUniversitiesAsync()).ReturnsAsync(mockUniversities);

        var controller = new UniversitiesController(serviceMock.Object);

        // Act
        var result = await controller.Get();

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var universities = Assert.IsAssignableFrom<List<University>>(okResult.Value);
        Assert.Equal(2, universities.Count);
    }

    [Fact]
    public async Task Get_ReturnsNotFound_WhenListIsEmpty()
    {
        // Arrange
        var serviceMock = new Mock<IUniversityService>();
        serviceMock.Setup(s => s.GetUniversitiesAsync()).ReturnsAsync(new List<University>());

        var controller = new UniversitiesController(serviceMock.Object);

        // Act
        var result = await controller.Get();

        // Assert
        var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
        Assert.Equal("Nenhuma universidade encontrada.", notFoundResult.Value);
    }

    [Fact]
    public async Task Get_Returns503_WhenExceptionIsThrown()
    {
        // Arrange
        var serviceMock = new Mock<IUniversityService>();
        serviceMock.Setup(s => s.GetUniversitiesAsync()).ThrowsAsync(new HttpRequestException("Erro"));

        var controller = new UniversitiesController(serviceMock.Object);

        // Act
        var result = await controller.Get();

        // Assert
        var statusCodeResult = Assert.IsType<ObjectResult>(result);
        Assert.Equal(503, statusCodeResult.StatusCode);
        Assert.Equal("Erro ao acessar o serviço de universidades.", statusCodeResult.Value);
    }
}