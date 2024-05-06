using NUnit.Framework;
using System.Threading.Tasks;
using ConsoleApp1;
using Moq;
using Moq.Protected;
using System.Net;
using Newtonsoft.Json;



public class JokeServiceTests
{
    private JokeService _jokeService;
    private Mock<HttpMessageHandler> _mockHttpMessageHandler;
    private HttpClient _httpClient;

    [SetUp]
    public void Setup()
    {
            _mockHttpMessageHandler = new Mock<HttpMessageHandler>(MockBehavior.Strict);
            _httpClient = new HttpClient(_mockHttpMessageHandler.Object);
            _jokeService = new JokeService(_httpClient);
    }


    [Test]
    public async Task GetRandomJokes_SuccessfulResponse_ReturnsJokes()
    {
        string category = "category";
        int numberOfJokes = 1;

        _mockHttpMessageHandler.Protected()
            .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
            .ReturnsAsync(new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent(JsonConvert.SerializeObject(new JokeResponse { Value = "Joke" }))
            });

        string[] jokes = await _jokeService.GetRandomJokes(category, numberOfJokes);
        Assert.AreEqual(numberOfJokes, jokes.Length);
    }


    [Test]
    public async Task GetRandomJokes_Without_Category_Returns_Jokes()
    {
        string category = null;
        int numberOfJokes = 1;

        _mockHttpMessageHandler.Protected()
            .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
            .ReturnsAsync(new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent(JsonConvert.SerializeObject(new JokeResponse { Value = "Joke" }))
            });

        string[] jokes = await _jokeService.GetRandomJokes(category, numberOfJokes);
        Assert.AreEqual(numberOfJokes, jokes.Length);
    }

    [Test]
    public async Task GetRandomJokes_ReturnsMaximumUniqueJokes()
    {

        string category = "money";
        int numberOfJokes = 9;

        _mockHttpMessageHandler.Protected()
            .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
            .ReturnsAsync(new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent(JsonConvert.SerializeObject(new JokeResponse { Value = "Joke" }))
            });

        string[] jokes = await _jokeService.GetRandomJokes(category, numberOfJokes);
        Assert.AreEqual(1, jokes.Length);
    }

    [Test]
    public async Task GetCategories_SuccessfulResponse_ReturnsCategories()
    {

        _mockHttpMessageHandler.Protected()
            .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
            .ReturnsAsync(new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent("[\"category1\",\"category2\"]")
            });


        string[] categories = await _jokeService.GetCategories();
        Assert.AreEqual(new string[] { "category1", "category2" }, categories);
    }

    [Test]
    public async Task GetCategories_FailedHttpResponse_ReturnsEmptyArray()
    {
        _mockHttpMessageHandler.Protected()
            .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
            .ReturnsAsync(new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.BadRequest
            });

        string[] categories = await _jokeService.GetCategories();
        Assert.IsEmpty(categories);
    }


    [Test]
    public async Task GetCategories_FailedHttpRequest_ReturnsEmptyArray()
    {

        _mockHttpMessageHandler.Protected()
            .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
            .ThrowsAsync(new HttpRequestException("Failed to fetch categories"));

        string[] categories = await _jokeService.GetCategories();
        Assert.IsEmpty(categories);
    }

    [Test]
    public async Task GetCategories_UnexpectedError_ReturnsEmptyArray()
    {
        _mockHttpMessageHandler.Protected()
            .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
            .ThrowsAsync(new Exception("Unexpected error"));

        string[] categories = await _jokeService.GetCategories();
        Assert.IsEmpty(categories);
    }

}
