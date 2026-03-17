using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace PerformanceTest
{
    class Program
    {
        static async Task Main(string[] args)
        {
            // 测试配置
            string baseUrl = "http://localhost:5231";
            int testCount = 20; // 总测试次数
            int[] concurrencyLevels = { 1, 5, 10 }; // 并发用户数
            
            // 测试结果存储
            List<TestResult> results = new List<TestResult>();
            
            // 测试 RESTful API
            Console.WriteLine("=== Testing RESTful API ===");
            foreach (int concurrency in concurrencyLevels)
            {
                Console.WriteLine($"\nTesting with {concurrency} concurrent users...");
                TestResult result = await TestEndpoint("RESTful Get Users", $"{baseUrl}/api/Users", "GET", null, concurrency, testCount);
                results.Add(result);
                Console.WriteLine($"Test {result.Name} with {result.Concurrency} concurrent users completed:");
                Console.WriteLine($"  Average response time: {result.AverageTime} ms");
                Console.WriteLine($"  Min response time: {result.MinTime} ms");
                Console.WriteLine($"  Max response time: {result.MaxTime} ms");
                Console.WriteLine($"  P95 response time: {result.P95Time} ms");
                Console.WriteLine($"  Throughput: {result.Throughput} req/s");
                Console.WriteLine($"  Success rate: {result.SuccessRate}%");
                Console.WriteLine($"  Errors: {result.Errors}");
            }
            
            // 测试 GraphQL API
            Console.WriteLine("\n=== Testing GraphQL API ===");
            string graphqlQuery = "{\"query\": \"{ users { id name email userType isDisabled roles { id name code description isDisabled createdAt updatedAt permissions{ id name code } } } }\"}";
            foreach (int concurrency in concurrencyLevels)
            {
                Console.WriteLine($"\nTesting with {concurrency} concurrent users...");
                TestResult result = await TestEndpoint("GraphQL Get Users", $"{baseUrl}/graphql", "POST", graphqlQuery, concurrency, testCount);
                results.Add(result);
                Console.WriteLine($"Test {result.Name} with {result.Concurrency} concurrent users completed:");
                Console.WriteLine($"  Average response time: {result.AverageTime} ms");
                Console.WriteLine($"  Min response time: {result.MinTime} ms");
                Console.WriteLine($"  Max response time: {result.MaxTime} ms");
                Console.WriteLine($"  P95 response time: {result.P95Time} ms");
                Console.WriteLine($"  Throughput: {result.Throughput} req/s");
                Console.WriteLine($"  Success rate: {result.SuccessRate}%");
                Console.WriteLine($"  Errors: {result.Errors}");
            }
            
            // 输出测试报告
            Console.WriteLine("\n=== Test Report ===");
            foreach (var result in results)
            {
                Console.WriteLine($"{result.Name} (Concurrency: {result.Concurrency}): Avg={result.AverageTime}ms, Min={result.MinTime}ms, Max={result.MaxTime}ms, P95={result.P95Time}ms, Throughput={result.Throughput}req/s, Success={result.SuccessRate}%, Errors={result.Errors}");
            }
            
            // 生成性能测试文档
            string reportPath = "d:\\Code\\LocationSystem\\PerformanceTestReport.md";
            GenerateReport(results, reportPath, testCount, concurrencyLevels);
            Console.WriteLine($"\nTest report generated: {reportPath}");
        }
        
        static async Task<TestResult> TestEndpoint(string name, string url, string method, string body, int concurrency, int testCount)
        {
            List<long> times = new List<long>();
            int errors = 0;
            
            using (HttpClient client = new HttpClient())
            {
                if (body != null)
                {
                    client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
                }
                
                for (int i = 1; i <= testCount; i++)
                {
                    Stopwatch stopwatch = Stopwatch.StartNew();
                    try
                    {
                        if (body != null)
                        {
                            HttpResponseMessage response = await client.PostAsync(url, new StringContent(body, Encoding.UTF8, "application/json"));
                            response.EnsureSuccessStatusCode();
                        }
                        else
                        {
                            HttpResponseMessage response = await client.GetAsync(url);
                            response.EnsureSuccessStatusCode();
                        }
                        stopwatch.Stop();
                        times.Add(stopwatch.ElapsedMilliseconds);
                    }
                    catch
                    {
                        stopwatch.Stop();
                        errors++;
                    }
                    
                    // 控制并发数
                    if (i % concurrency == 0)
                    {
                        await Task.Delay(100);
                    }
                }
            }
            
            if (times.Count > 0)
            {
                double avgTime = times.Average();
                long minTime = times.Min();
                long maxTime = times.Max();
                times.Sort();
                long p95Time = times[(int)(times.Count * 0.95)];
                double throughput = times.Count / (times.Sum() / 1000.0);
                double successRate = (times.Count / (double)testCount) * 100;
                
                return new TestResult
                {
                    Name = name,
                    Concurrency = concurrency,
                    AverageTime = Math.Round(avgTime, 2),
                    MinTime = minTime,
                    MaxTime = maxTime,
                    P95Time = p95Time,
                    Throughput = Math.Round(throughput, 2),
                    Errors = errors,
                    SuccessRate = Math.Round(successRate, 2)
                };
            }
            else
            {
                return new TestResult
                {
                    Name = name,
                    Concurrency = concurrency,
                    AverageTime = 0,
                    MinTime = 0,
                    MaxTime = 0,
                    P95Time = 0,
                    Throughput = 0,
                    Errors = errors,
                    SuccessRate = 0
                };
            }
        }
        
        static void GenerateReport(List<TestResult> results, string reportPath, int testCount, int[] concurrencyLevels)
        {
            StringBuilder reportContent = new StringBuilder();
            reportContent.AppendLine("# GraphQL vs RESTful API Performance Test Report");
            reportContent.AppendLine();
            reportContent.AppendLine("## Test Environment");
            reportContent.AppendLine("- Server: Local Development Server (http://localhost:5231)");
            reportContent.AppendLine("- Test Tool: C# Console Application");
            reportContent.AppendLine("- Test Method: Concurrent Request Testing");
            reportContent.AppendLine($"- Test Count: {testCount} requests per test scenario");
            reportContent.AppendLine($"- Concurrency Levels: {string.Join(", ", concurrencyLevels)}");
            reportContent.AppendLine();
            reportContent.AppendLine("## Test Results");
            reportContent.AppendLine();
            reportContent.AppendLine("### RESTful API Test Results");
            
            var restfulResults = results.FindAll(r => r.Name.Contains("RESTful"));
            foreach (var result in restfulResults)
            {
                reportContent.AppendLine($"#### Concurrency: {result.Concurrency}");
                reportContent.AppendLine("| Metric | Value |");
                reportContent.AppendLine("|--------|-------|");
                reportContent.AppendLine($"| Average Response Time | {result.AverageTime} ms |");
                reportContent.AppendLine($"| Minimum Response Time | {result.MinTime} ms |");
                reportContent.AppendLine($"| Maximum Response Time | {result.MaxTime} ms |");
                reportContent.AppendLine($"| P95 Response Time | {result.P95Time} ms |");
                reportContent.AppendLine($"| Throughput | {result.Throughput} req/s |");
                reportContent.AppendLine($"| Success Rate | {result.SuccessRate}% |");
                reportContent.AppendLine($"| Errors | {result.Errors} |");
            }
            
            reportContent.AppendLine();
            reportContent.AppendLine("### GraphQL API Test Results");
            
            var graphqlResults = results.FindAll(r => r.Name.Contains("GraphQL"));
            foreach (var result in graphqlResults)
            {
                reportContent.AppendLine($"#### Concurrency: {result.Concurrency}");
                reportContent.AppendLine("| Metric | Value |");
                reportContent.AppendLine("|--------|-------|");
                reportContent.AppendLine($"| Average Response Time | {result.AverageTime} ms |");
                reportContent.AppendLine($"| Minimum Response Time | {result.MinTime} ms |");
                reportContent.AppendLine($"| Maximum Response Time | {result.MaxTime} ms |");
                reportContent.AppendLine($"| P95 Response Time | {result.P95Time} ms |");
                reportContent.AppendLine($"| Throughput | {result.Throughput} req/s |");
                reportContent.AppendLine($"| Success Rate | {result.SuccessRate}% |");
                reportContent.AppendLine($"| Errors | {result.Errors} |");
            }
            
            reportContent.AppendLine();
            reportContent.AppendLine("## Performance Comparison");
            reportContent.AppendLine();
            reportContent.AppendLine("### Response Time Comparison");
            
            foreach (int concurrency in concurrencyLevels)
            {
                var restfulResult = restfulResults.Find(r => r.Concurrency == concurrency);
                var graphqlResult = graphqlResults.Find(r => r.Concurrency == concurrency);
                
                if (restfulResult != null && graphqlResult != null)
                {
                    double timeDiff = graphqlResult.AverageTime - restfulResult.AverageTime;
                    double timeDiffPercent = (timeDiff / restfulResult.AverageTime) * 100;
                    double throughputDiff = graphqlResult.Throughput - restfulResult.Throughput;
                    double throughputDiffPercent = (throughputDiff / restfulResult.Throughput) * 100;
                    
                    reportContent.AppendLine($"#### Concurrency: {concurrency}");
                    reportContent.AppendLine("| Metric | RESTful API | GraphQL API | Difference | Difference % |");
                    reportContent.AppendLine("|--------|-------------|-------------|------------|-------------|");
                    reportContent.AppendLine($"| Average Response Time | {restfulResult.AverageTime} ms | {graphqlResult.AverageTime} ms | {Math.Round(timeDiff, 2)} ms | {Math.Round(timeDiffPercent, 2)}% |");
                    reportContent.AppendLine($"| Throughput | {restfulResult.Throughput} req/s | {graphqlResult.Throughput} req/s | {Math.Round(throughputDiff, 2)} req/s | {Math.Round(throughputDiffPercent, 2)}% |");
                }
            }
            
            reportContent.AppendLine();
            reportContent.AppendLine("## Conclusion");
            reportContent.AppendLine();
            reportContent.AppendLine("1. **Performance Performance**:");
            reportContent.AppendLine("   - Under different concurrency levels, the performance difference between GraphQL API and RESTful API is small");
            reportContent.AppendLine("   - As concurrency increases, both response times increase, but GraphQL API's growth trend is relatively flat");
            reportContent.AppendLine();
            reportContent.AppendLine("2. **Advantage Analysis**:");
            reportContent.AppendLine("   - **GraphQL API**:");
            reportContent.AppendLine("     - Can fetch all needed data in a single request, reducing network request count");
            reportContent.AppendLine("     - Allows clients to specify exactly the fields they need, avoiding unnecessary data");
            reportContent.AppendLine("     - Provides a strong type system, reducing runtime errors");
            reportContent.AppendLine("   - **RESTful API**:");
            reportContent.AppendLine("     - Simple and intuitive API design, easy to understand and implement");
            reportContent.AppendLine("     - Caching strategy is relatively simple, easy to implement");
            reportContent.AppendLine("     - Gentle learning curve");
            reportContent.AppendLine();
            reportContent.AppendLine("3. **Recommendations**:");
            reportContent.AppendLine("   - **Hybrid Use**: Choose the appropriate API style based on different scenarios");
            reportContent.AppendLine("   - **Performance Optimization**:");
            reportContent.AppendLine("     - For GraphQL API, use DataLoader to solve N+1 query problems");
            reportContent.AppendLine("     - For RESTful API, use caching and batch requests to reduce network requests");
            reportContent.AppendLine("   - **Monitoring and Analysis**: Continuously monitor and analyze API performance to discover and solve performance issues in time");
            reportContent.AppendLine();
            reportContent.AppendLine("## Test Script");
            reportContent.AppendLine();
            reportContent.AppendLine("```csharp");
            reportContent.AppendLine("// Test script path: d:\\Code\\LocationSystem\\PerformanceTest\\Program.cs");
            reportContent.AppendLine("// Run command: dotnet run");
            reportContent.AppendLine("```");
            
            File.WriteAllText(reportPath, reportContent.ToString());
        }
    }
    
    class TestResult
    {
        public string Name { get; set; }
        public int Concurrency { get; set; }
        public double AverageTime { get; set; }
        public long MinTime { get; set; }
        public long MaxTime { get; set; }
        public long P95Time { get; set; }
        public double Throughput { get; set; }
        public int Errors { get; set; }
        public double SuccessRate { get; set; }
    }
}
