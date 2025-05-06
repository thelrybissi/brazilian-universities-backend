using Xunit;
using Moq;
using backend.Controllers;
using backend.Models;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http;
using System.Threading.Tasks;
using System.Collections.Generic;
using brazilian_universities_backend.Services;

public class UniversitiesControllerTests
{
    [Fact]
    public async Task Get_ReturnsOk_WithPaginatedResult()
    {
        // Arrange
        var mockUniversities = new List<University>
        {
            new University { Name = "UFU", Country = "Brazil" },
            new University { Name = "USP", Country = "Brazil" }
        };

        var paginatedResult = new PaginatedResult<University>(mockUniversities, mockUniversities.Count, 1, 10);

        var serviceMock = new Mock<IUniversityService>();
        serviceMock.Setup(s => s.GetUniversitiesAsync(1, 10)).ReturnsAsync(paginatedResult);

        var controller = new UniversitiesController(serviceMock.Object);

        // Act
        var result = await controller.Get(1, 10);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var returnedResult = Assert.IsType<PaginatedResult<University>>(okResult.Value);
        Assert.Equal(2, returnedResult.TotalCount);
        Assert.Equal(1, returnedResult.Page);
        Assert.Equal(10, returnedResult.PageSize);
        Assert.Equal(2, returnedResult.Items.Count);
    }

    [Fact]
    public async Task Get_ReturnsNotFound_WhenNoUniversities()
    {
        // Arrange
        var paginatedResult = new PaginatedResult<University>(new List<University>(), 0, 1, 10);

        var serviceMock = new Mock<IUniversityService>();
        serviceMock.Setup(s => s.GetUniversitiesAsync(1, 10)).ReturnsAsync(paginatedResult);

        var controller = new UniversitiesController(serviceMock.Object);

        // Act
        var result = await controller.Get(1, 10);

        // Assert
        var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
        Assert.Equal("Nenhuma universidade encontrada.", notFoundResult.Value);
    }

    [Fact]
    public async Task Get_Returns503_WhenServiceThrowsException()
    {
        // Arrange
        var serviceMock = new Mock<IUniversityService>();
        serviceMock.Setup(s => s.GetUniversitiesAsync(1, 10)).ThrowsAsync(new HttpRequestException("Erro"));

        var controller = new UniversitiesController(serviceMock.Object);

        // Act
        var result = await controller.Get(1, 10);

        // Assert
        var statusCodeResult = Assert.IsType<ObjectResult>(result);
        Assert.Equal(503, statusCodeResult.StatusCode);
        Assert.Equal("Erro ao acessar o serviço de universidades.", statusCodeResult.Value);
    }

    [Fact]
    public async Task Get_ReturnsBadRequest_WhenPaginationIsInvalid()
    {
        // Arrange
        var serviceMock = new Mock<IUniversityService>();
        var controller = new UniversitiesController(serviceMock.Object);

        // Act
        var result = await controller.Get(0, -1); // valores inválidos

        // Assert
        var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
        Assert.Equal("Parâmetros de paginação inválidos.", badRequestResult.Value);
    }
}
