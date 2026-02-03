using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

[TestClass]
public class RateLimitTest
{
    private static HttpClient _client;

    [ClassInitialize]
    public static void Setup(TestContext context)
    {
        // 启动你的API（假设运行在 http://localhost:5000）
        _client = new HttpClient();
        _client.BaseAddress = new System.Uri("http://localhost:5231");
    }

    [TestMethod]
    public async Task TestRateLimit()
    {
        // 测试：连续发11个请求，第11个应该返回429

        // 1. 前10个请求应该成功
        for (int i = 1; i <= 10; i++)
        {
            var response = await _client.GetAsync("/api/renthouse/test");
            Console.WriteLine($"请求{i}: {(int)response.StatusCode}");
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
            await Task.Delay(50); // 等50ms
        }

        // 2. 第11个请求应该被限流
        var rateLimitedResponse = await _client.GetAsync("/api/renthouse/test");
        Console.WriteLine($"请求11: {(int)rateLimitedResponse.StatusCode}");
        Assert.AreEqual(HttpStatusCode.TooManyRequests, rateLimitedResponse.StatusCode);
    }

    [TestMethod]
    public async Task TestRateLimitReset()
    {
        // 触发限流
        for (int i = 0; i < 11; i++)
        {
            await _client.GetAsync("/api/renthouse/test");
            await Task.Delay(50);
        }

        // 等11秒，限流应该重置
        await Task.Delay(11000);

        // 再发请求应该成功
        var response = await _client.GetAsync("/api/renthouse/test");
        Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
    }
}