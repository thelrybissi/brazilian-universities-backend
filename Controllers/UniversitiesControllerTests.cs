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
        
        var json = JsonSerializer.Serialize(mockUniversities);


        var fakeResponse = new HttpResponseMessage(HttpStatusCode.OK)
        {
            Content = new StringContent(json)
        };

        var fakeHandler = new FakeHttpMessageHandler(fakeResponse);
        var httpClient = new HttpClient(fakeHandler);

        var httpClientFactory = new Mock<IHttpClientFactory>();
        httpClientFactory.Setup(f => f.CreateClient(It.IsAny<string>())).Returns(httpClient);

        var controller = new UniversitiesController(httpClientFactory.Object);

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
        var json = JsonSerializer.Serialize(new List<University>());

        var fakeResponse = new HttpResponseMessage(HttpStatusCode.OK)
        {
            Content = new StringContent(json)
        };

        var fakeHandler = new FakeHttpMessageHandler(fakeResponse);
        var httpClient = new HttpClient(fakeHandler);

        var httpClientFactory = new Mock<IHttpClientFactory>();
        httpClientFactory.Setup(f => f.CreateClient(It.IsAny<string>())).Returns(httpClient);

        var controller = new UniversitiesController(httpClientFactory.Object);

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
        var exceptionHandler = new ExceptionThrowingHttpMessageHandler(new HttpRequestException("Erro"));
        var httpClient = new HttpClient(exceptionHandler);

        var httpClientFactoryMock = new Mock<IHttpClientFactory>();
        httpClientFactoryMock.Setup(x => x.CreateClient(It.IsAny<string>())).Returns(httpClient);

        var controller = new UniversitiesController(httpClientFactoryMock.Object);

        // Act
        var result = await controller.Get();

        // Assert
        var statusCodeResult = Assert.IsType<ObjectResult>(result);
        Assert.Equal(503, statusCodeResult.StatusCode);
    }
}