# Performance test script: Compare GraphQL and RESTful API performance

# Test configuration
$baseUrl = "http://localhost:5231"
$testCount = 10  # Number of times to run each test

# Test results storage
$results = @()

# Test function
function Test-Endpoint($name, $url, $method, $body) {
    $times = @()
    for ($i = 1; $i -le $testCount; $i++) {
        $stopwatch = [System.Diagnostics.Stopwatch]::StartNew()
        try {
            if ($body) {
                Invoke-WebRequest -Uri "$baseUrl$url" -Method $method -Headers @{"Content-Type"="application/json"} -Body $body -UseBasicParsing | Out-Null
            } else {
                Invoke-WebRequest -Uri "$baseUrl$url" -Method $method -UseBasicParsing | Out-Null
            }
        } catch {
            Write-Host "Test " $name " failed: " $_.Exception.Message
            continue
        }
        $stopwatch.Stop()
        $time = $stopwatch.ElapsedMilliseconds
        $times += $time
        Write-Host "Test " $name " iteration " $i ": " $time " ms"
    }
    
    if ($times.Count -gt 0) {
        $avgTime = [Math]::Round(($times | Measure-Object -Average).Average, 2)
        $minTime = ($times | Measure-Object -Minimum).Minimum
        $maxTime = ($times | Measure-Object -Maximum).Maximum
        
        $results += @{
            Name = $name
            AverageTime = $avgTime
            MinTime = $minTime
            MaxTime = $maxTime
        }
        
        Write-Host "Test " $name " completed, average response time: " $avgTime " ms, min: " $minTime " ms, max: " $maxTime " ms"
    } else {
        Write-Host "Test " $name " has no successful results"
    }
}

# Test RESTful API
Write-Host "=== Testing RESTful API ==="
Test-Endpoint "RESTful Get Users" "/api/Users" "GET" $null
Test-Endpoint "RESTful Get User Detail" "/api/Users/37dbf1b248c447ca96c51ea9803501b8" "GET" $null
Test-Endpoint "RESTful Get Roles" "/api/Roles" "GET" $null
Test-Endpoint "RESTful Get Permissions" "/api/Permissions" "GET" $null

# Test GraphQL API
Write-Host "`n=== Testing GraphQL API ==="
$graphqlQuery = '{"query": "{ users { id name email userType isDisabled roles { id name code description isDisabled createdAt updatedAt permissions{ id name code } } } }"}'
Test-Endpoint "GraphQL Get Users" "/graphql" "POST" $graphqlQuery

$graphqlUserQuery = '{"query": "{ user(id: \"37dbf1b248c447ca96c51ea9803501b8\") { id name email userType isDisabled roles { id name code description isDisabled createdAt updatedAt permissions{ id name code } } } }"}'
Test-Endpoint "GraphQL Get User Detail" "/graphql" "POST" $graphqlUserQuery

$graphqlRolesQuery = '{"query": "{ roles { id name code description isDisabled createdAt updatedAt permissions{ id name code } } }"}'
Test-Endpoint "GraphQL Get Roles" "/graphql" "POST" $graphqlRolesQuery

$graphqlPermissionsQuery = '{"query": "{ permissions { id name code parentId childPermissions { id name code } } }"}'
Test-Endpoint "GraphQL Get Permissions" "/graphql" "POST" $graphqlPermissionsQuery

# Output test report
Write-Host "`n=== Test Report ==="
$results | Sort-Object -Property AverageTime | Format-Table -Property Name, AverageTime, MinTime, MaxTime

# Analyze results
Write-Host "`n=== Analysis ==="
$restfulTests = $results | Where-Object { $_.Name -like "RESTful*" }
$graphqlTests = $results | Where-Object { $_.Name -like "GraphQL*" }

$restfulAvg = [Math]::Round(($restfulTests | Measure-Object -Property AverageTime -Average).Average, 2)
$graphqlAvg = [Math]::Round(($graphqlTests | Measure-Object -Property AverageTime -Average).Average, 2)

Write-Host "RESTful API average response time: " $restfulAvg " ms"
Write-Host "GraphQL API average response time: " $graphqlAvg " ms"

if ($graphqlAvg -lt $restfulAvg) {
    $improvement = [Math]::Round((1 - $graphqlAvg / $restfulAvg) * 100, 2)
    Write-Host "GraphQL API is " $improvement "% faster than RESTful API"
} else {
    $degradation = [Math]::Round(($graphqlAvg / $restfulAvg - 1) * 100, 2)
    Write-Host "GraphQL API is " $degradation "% slower than RESTful API"
}
