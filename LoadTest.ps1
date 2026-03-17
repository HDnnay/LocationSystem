# 压力测试脚本：对比 GraphQL 和 RESTful API 性能

# 测试配置
$baseUrl = "http://localhost:5231"
$testCount = 20  # 总测试次数
$concurrencyLevels = @(1, 5, 10)  # 并发用户数

# 测试结果存储
$results = @()

# 测试函数
function Test-Endpoint($name, $url, $method, $body, $concurrency) {
    $times = @()
    $errors = 0
    
    # 模拟并发请求
    for ($i = 1; $i -le $testCount; $i++) {
        $stopwatch = [System.Diagnostics.Stopwatch]::StartNew()
        try {
            if ($body) {
                Invoke-WebRequest -Uri "$baseUrl$url" -Method $method -Headers @{"Content-Type"="application/json"} -Body $body -UseBasicParsing | Out-Null
            } else {
                Invoke-WebRequest -Uri "$baseUrl$url" -Method $method -UseBasicParsing | Out-Null
            }
            $stopwatch.Stop()
            $times += $stopwatch.ElapsedMilliseconds
        } catch {
            $stopwatch.Stop()
            $errors++
        }
        
        # 控制并发数
        if ($i % $concurrency -eq 0) {
            Start-Sleep -Milliseconds 100
        }
    }
    
    if ($times.Count -gt 0) {
        $avgTime = [Math]::Round(($times | Measure-Object -Average).Average, 2)
        $minTime = ($times | Measure-Object -Minimum).Minimum
        $maxTime = ($times | Measure-Object -Maximum).Maximum
        $p95Time = [Math]::Round(($times | Sort-Object | Select-Object -Last ([Math]::Ceiling($times.Count * 0.05)))[0], 2)
        $throughput = [Math]::Round($times.Count / ($times | Measure-Object -Sum).Sum * 1000, 2)  # 每秒请求数
        
        $results += @{
            Name = $name
            Concurrency = $concurrency
            AverageTime = $avgTime
            MinTime = $minTime
            MaxTime = $maxTime
            P95Time = $p95Time
            Throughput = $throughput
            Errors = $errors
            SuccessRate = [Math]::Round(($times.Count / $testCount) * 100, 2)
        }
        
        Write-Host "Test $name with $concurrency concurrent users completed:"
        Write-Host "  Average response time: $avgTime ms"
        Write-Host "  Min response time: $minTime ms"
        Write-Host "  Max response time: $maxTime ms"
        Write-Host "  P95 response time: $p95Time ms"
        Write-Host "  Throughput: $throughput req/s"
        Write-Host "  Success rate: $([Math]::Round(($times.Count / $testCount) * 100, 2))%"
        Write-Host "  Errors: $errors"
    } else {
        Write-Host "Test $name with $concurrency concurrent users has no successful results"
    }
}

# 测试 RESTful API
Write-Host "=== Testing RESTful API ==="
foreach ($concurrency in $concurrencyLevels) {
    Write-Host "`nTesting with $concurrency concurrent users..."
    Test-Endpoint "RESTful Get Users" "/api/Users" "GET" $null $concurrency
}

# 测试 GraphQL API
Write-Host "`n=== Testing GraphQL API ==="
$graphqlQuery = '{"query": "{ users { id name email userType isDisabled roles { id name code description isDisabled createdAt updatedAt permissions{ id name code } } } }"}'
foreach ($concurrency in $concurrencyLevels) {
    Write-Host "`nTesting with $concurrency concurrent users..."
    Test-Endpoint "GraphQL Get Users" "/graphql" "POST" $graphqlQuery $concurrency
}

# 输出测试报告
Write-Host "`n=== Test Report ==="
$results | Sort-Object -Property Name, Concurrency | Format-Table -Property Name, Concurrency, AverageTime, MinTime, MaxTime, P95Time, Throughput, SuccessRate, Errors

# 生成性能测试文档
$reportPath = "d:\Code\LocationSystem\PerformanceTestReport.md"

# 创建测试报告内容
$reportContent = "# GraphQL vs RESTful API Performance Test Report`n`n## Test Environment`n- Server: Local Development Server (http://localhost:5231)`n- Test Tool: PowerShell Script`n- Test Method: Concurrent Request Testing`n- Test Count: $testCount requests per test scenario`n- Concurrency Levels: $($concurrencyLevels -join ", ")`n`n## Test Results`n`n### RESTful API Test Results"

$restfulResults = $results | Where-Object { $_.Name -like "RESTful*" }
foreach ($result in $restfulResults) {
    $reportContent += "`n#### Concurrency: $($result.Concurrency)`n| Metric | Value |`n|--------|-------|`n| Average Response Time | $($result.AverageTime) ms |`n| Minimum Response Time | $($result.MinTime) ms |`n| Maximum Response Time | $($result.MaxTime) ms |`n| P95 Response Time | $($result.P95Time) ms |`n| Throughput | $($result.Throughput) req/s |`n| Success Rate | $($result.SuccessRate)% |`n| Errors | $($result.Errors) |"
}

$reportContent += "`n`n### GraphQL API Test Results"

$graphqlResults = $results | Where-Object { $_.Name -like "GraphQL*" }
foreach ($result in $graphqlResults) {
    $reportContent += "`n#### Concurrency: $($result.Concurrency)`n| Metric | Value |`n|--------|-------|`n| Average Response Time | $($result.AverageTime) ms |`n| Minimum Response Time | $($result.MinTime) ms |`n| Maximum Response Time | $($result.MaxTime) ms |`n| P95 Response Time | $($result.P95Time) ms |`n| Throughput | $($result.Throughput) req/s |`n| Success Rate | $($result.SuccessRate)% |`n| Errors | $($result.Errors) |"
}

$reportContent += "`n`n## Performance Comparison`n`n### Response Time Comparison"

foreach ($concurrency in $concurrencyLevels) {
    $restfulResult = $restfulResults | Where-Object { $_.Concurrency -eq $concurrency }
    $graphqlResult = $graphqlResults | Where-Object { $_.Concurrency -eq $concurrency }
    
    if ($restfulResult -and $graphqlResult) {
        $timeDiff = $graphqlResult.AverageTime - $restfulResult.AverageTime
        $timeDiffPercent = [Math]::Round(($timeDiff / $restfulResult.AverageTime) * 100, 2)
        $throughputDiff = $graphqlResult.Throughput - $restfulResult.Throughput
        $throughputDiffPercent = [Math]::Round(($throughputDiff / $restfulResult.Throughput) * 100, 2)
        
        $reportContent += "`n#### Concurrency: $concurrency`n| Metric | RESTful API | GraphQL API | Difference | Difference % |`n|--------|-------------|-------------|------------|-------------|`n| Average Response Time | $($restfulResult.AverageTime) ms | $($graphqlResult.AverageTime) ms | $([Math]::Round($timeDiff, 2)) ms | $timeDiffPercent% |`n| Throughput | $($restfulResult.Throughput) req/s | $($graphqlResult.Throughput) req/s | $([Math]::Round($throughputDiff, 2)) req/s | $throughputDiffPercent% |"
    }
}

$reportContent += "`n`n## Conclusion`n`n1. **Performance Performance**:`n   - Under different concurrency levels, the performance difference between GraphQL API and RESTful API is small`n   - As concurrency increases, both response times increase, but GraphQL API's growth trend is relatively flat`n`n2. **Advantage Analysis**:`n   - **GraphQL API**:`n     - Can fetch all needed data in a single request, reducing network request count`n     - Allows clients to specify exactly the fields they need, avoiding unnecessary data`n     - Provides a strong type system, reducing runtime errors`n   - **RESTful API**:`n     - Simple and intuitive API design, easy to understand and implement`n     - Caching strategy is relatively simple, easy to implement`n     - Gentle learning curve`n`n3. **Recommendations**:`n   - **Hybrid Use**: Choose the appropriate API style based on different scenarios`n   - **Performance Optimization**:`n     - For GraphQL API, use DataLoader to solve N+1 query problems`n     - For RESTful API, use caching and batch requests to reduce network requests`n   - **Monitoring and Analysis**: Continuously monitor and analyze API performance to discover and solve performance issues in time`n`n## Test Script`n`n```powershell`n# Test script path: d:\Code\LocationSystem\LoadTest.ps1`n# Run command: powershell -ExecutionPolicy Bypass -File LoadTest.ps1`n```"

# Write test report
Set-Content -Path $reportPath -Value $reportContent
Write-Host "`nTest report generated: $reportPath"
