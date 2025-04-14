# Script to populate database with sample task data
# Usage: .\populate-database.ps1 -JsonFile "sample-tasks.json" -ApiUrl "https://localhost:7138/api/v1/TaskItem"

param(
    [Parameter(Mandatory=$true)]
    [string]$JsonFile,
    
    [Parameter(Mandatory=$true)]
    [string]$ApiUrl,
    
    [switch]$IgnoreSslErrors
)

# Display script banner
Write-Host "====================================================" -ForegroundColor Cyan
Write-Host "  Database Population Tool - Task Items Importer" -ForegroundColor Cyan
Write-Host "====================================================" -ForegroundColor Cyan
Write-Host ""

# Function to check if the file exists
function Test-FileExists {
    param([string]$FilePath)
    
    if (-not (Test-Path -Path $FilePath)) {
        Write-Host "Error: File '$FilePath' not found." -ForegroundColor Red
        exit 1
    }
}

# Check if JSON file exists
Test-FileExists -FilePath $JsonFile

# Read JSON file content
try {
    Write-Host "Reading task data from $JsonFile..." -ForegroundColor Yellow
    $tasksJson = Get-Content -Path $JsonFile -Raw
    $tasks = $tasksJson | ConvertFrom-Json
    Write-Host "Found $(($tasks | Measure-Object).Count) tasks to import." -ForegroundColor Green
} catch {
    Write-Host "Error reading JSON file: $_" -ForegroundColor Red
    exit 1
}

# Ignore SSL certificate errors if specified
if ($IgnoreSslErrors) {
    Write-Host "Warning: SSL certificate validation disabled." -ForegroundColor Yellow
    
    # For PowerShell 6+ (Core)
    if ($PSVersionTable.PSVersion.Major -ge 6) {
        $PSDefaultParameterValues['Invoke-RestMethod:SkipCertificateCheck'] = $true
    } else {
        # For Windows PowerShell 5.1 and below
        Add-Type @"
            using System.Net;
            using System.Security.Cryptography.X509Certificates;
            public class TrustAllCertsPolicy : ICertificatePolicy {
                public bool CheckValidationResult(
                    ServicePoint srvPoint, X509Certificate certificate,
                    WebRequest request, int certificateProblem) {
                    return true;
                }
            }
"@
        [System.Net.ServicePointManager]::CertificatePolicy = New-Object TrustAllCertsPolicy
    }
    
    # Also set TLS 1.2
    [Net.ServicePointManager]::SecurityProtocol = [Net.SecurityProtocolType]::Tls12
}

# Function to send HTTP POST request
function Invoke-PostRequest {
    param(
        [string]$Uri,
        [string]$Body
    )
    
    $headers = @{
        "Content-Type" = "application/json"
        "Accept" = "application/json"
    }
    
    try {
        if ($PSVersionTable.PSVersion.Major -ge 6) {
            # PowerShell 6+ (Core)
            $response = Invoke-RestMethod -Uri $Uri -Method Post -Body $Body -ContentType "application/json" -Headers $headers -ErrorAction Stop
        } else {
            # Windows PowerShell 5.1 and below
            $response = Invoke-RestMethod -Uri $Uri -Method Post -Body $Body -ContentType "application/json" -Headers $headers -ErrorAction Stop
        }
        return $response
    } catch {
        $statusCode = $_.Exception.Response.StatusCode.value__
        $errorMsg = $_.Exception.Message
        
        if ($_.Exception.Response) {
            try {
                $reader = New-Object System.IO.StreamReader($_.Exception.Response.GetResponseStream())
                $errorMsg = $reader.ReadToEnd()
                $reader.Close()
            } catch {}
        }
        
        throw "HTTP Error $($statusCode): $($errorMsg)"
    }
}

# Process each task
$successCount = 0
$failCount = 0

Write-Host ""
Write-Host "Starting import process..." -ForegroundColor Cyan
Write-Host "--------------------------------------------------" -ForegroundColor Cyan

foreach ($task in $tasks) {
    # Convert task to JSON
    $taskJson = $task | ConvertTo-Json -Compress
    
    Write-Host "Importing task: $($task.name) for userId: $($task.userId)..." -ForegroundColor Yellow
    
    try {
        # Send POST request
        $response = Invoke-PostRequest -Uri $ApiUrl -Body $taskJson
        Write-Host "  Success!" -ForegroundColor Green
        $successCount++
    } catch {
        Write-Host "  ✗ Failed: $_" -ForegroundColor Red
        $failCount++
    }
}

Write-Host "--------------------------------------------------" -ForegroundColor Cyan
Write-Host "Import summary:" -ForegroundColor Cyan
Write-Host "  Total tasks: $(($tasks | Measure-Object).Count)" -ForegroundColor White
Write-Host "  Successfully imported: $successCount" -ForegroundColor Green
Write-Host "  Failed to import: $failCount" -ForegroundColor $(if ($failCount -gt 0) { "Red" } else { "Green" })
Write-Host "--------------------------------------------------" -ForegroundColor Cyan

if ($failCount -eq 0) {
    Write-Host "All tasks were imported successfully!" -ForegroundColor Green
} else {
    Write-Host "Some tasks failed to import. Check the errors above." -ForegroundColor Yellow
}