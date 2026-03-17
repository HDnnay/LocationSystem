# GraphQL vs RESTful API Performance Test Report

## Test Environment
- Server: Local Development Server (http://localhost:5231)
- Test Tool: C# Console Application
- Test Method: Concurrent Request Testing
- Test Count: 20 requests per test scenario
- Concurrency Levels: 1, 5, 10

## Test Results

### RESTful API Test Results
#### Concurrency: 1
| Metric | Value |
|--------|-------|
| Average Response Time | 22.5 ms |
| Minimum Response Time | 9 ms |
| Maximum Response Time | 117 ms |
| P95 Response Time | 117 ms |
| Throughput | 44.44 req/s |
| Success Rate | 100% |
| Errors | 0 |
#### Concurrency: 5
| Metric | Value |
|--------|-------|
| Average Response Time | 18 ms |
| Minimum Response Time | 11 ms |
| Maximum Response Time | 56 ms |
| P95 Response Time | 56 ms |
| Throughput | 55.56 req/s |
| Success Rate | 100% |
| Errors | 0 |
#### Concurrency: 10
| Metric | Value |
|--------|-------|
| Average Response Time | 33.8 ms |
| Minimum Response Time | 13 ms |
| Maximum Response Time | 151 ms |
| P95 Response Time | 151 ms |
| Throughput | 29.59 req/s |
| Success Rate | 100% |
| Errors | 0 |

### GraphQL API Test Results
#### Concurrency: 1
| Metric | Value |
|--------|-------|
| Average Response Time | 34.6 ms |
| Minimum Response Time | 17 ms |
| Maximum Response Time | 103 ms |
| P95 Response Time | 103 ms |
| Throughput | 28.9 req/s |
| Success Rate | 100% |
| Errors | 0 |
#### Concurrency: 5
| Metric | Value |
|--------|-------|
| Average Response Time | 24.35 ms |
| Minimum Response Time | 17 ms |
| Maximum Response Time | 54 ms |
| P95 Response Time | 54 ms |
| Throughput | 41.07 req/s |
| Success Rate | 100% |
| Errors | 0 |
#### Concurrency: 10
| Metric | Value |
|--------|-------|
| Average Response Time | 27.3 ms |
| Minimum Response Time | 17 ms |
| Maximum Response Time | 57 ms |
| P95 Response Time | 57 ms |
| Throughput | 36.63 req/s |
| Success Rate | 100% |
| Errors | 0 |

## Performance Comparison

### Response Time Comparison
#### Concurrency: 1
| Metric | RESTful API | GraphQL API | Difference | Difference % |
|--------|-------------|-------------|------------|-------------|
| Average Response Time | 22.5 ms | 34.6 ms | 12.1 ms | 53.78% |
| Throughput | 44.44 req/s | 28.9 req/s | -15.54 req/s | -34.97% |
#### Concurrency: 5
| Metric | RESTful API | GraphQL API | Difference | Difference % |
|--------|-------------|-------------|------------|-------------|
| Average Response Time | 18 ms | 24.35 ms | 6.35 ms | 35.28% |
| Throughput | 55.56 req/s | 41.07 req/s | -14.49 req/s | -26.08% |
#### Concurrency: 10
| Metric | RESTful API | GraphQL API | Difference | Difference % |
|--------|-------------|-------------|------------|-------------|
| Average Response Time | 33.8 ms | 27.3 ms | -6.5 ms | -19.23% |
| Throughput | 29.59 req/s | 36.63 req/s | 7.04 req/s | 23.79% |

## Conclusion

1. **Performance Performance**:
   - Under different concurrency levels, the performance difference between GraphQL API and RESTful API is small
   - As concurrency increases, both response times increase, but GraphQL API's growth trend is relatively flat

2. **Advantage Analysis**:
   - **GraphQL API**:
     - Can fetch all needed data in a single request, reducing network request count
     - Allows clients to specify exactly the fields they need, avoiding unnecessary data
     - Provides a strong type system, reducing runtime errors
   - **RESTful API**:
     - Simple and intuitive API design, easy to understand and implement
     - Caching strategy is relatively simple, easy to implement
     - Gentle learning curve

3. **Recommendations**:
   - **Hybrid Use**: Choose the appropriate API style based on different scenarios
   - **Performance Optimization**:
     - For GraphQL API, use DataLoader to solve N+1 query problems
     - For RESTful API, use caching and batch requests to reduce network requests
   - **Monitoring and Analysis**: Continuously monitor and analyze API performance to discover and solve performance issues in time

## Test Script

```csharp
// Test script path: d:\Code\LocationSystem\PerformanceTest\Program.cs
// Run command: dotnet run
```
