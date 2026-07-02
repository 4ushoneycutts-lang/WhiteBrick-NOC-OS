$OutDir = ".\data"
New-Item -ItemType Directory -Force -Path $OutDir | Out-Null

$timestamp = Get-Date -Format "yyyy-MM-dd HH:mm:ss"

$system = @{
    timestamp = $timestamp
    computerName = $env:COMPUTERNAME
    userName = $env:USERNAME
    uptime = ((Get-Date) - (Get-CimInstance Win32_OperatingSystem).LastBootUpTime).ToString()
    cpuLoad = (Get-CimInstance Win32_Processor).LoadPercentage
    memory = Get-CimInstance Win32_OperatingSystem | Select-Object TotalVisibleMemorySize,FreePhysicalMemory
}

$drives = Get-CimInstance Win32_LogicalDisk | Select-Object DeviceID,VolumeName,DriveType,Size,FreeSpace

$networkAdapters = Get-NetAdapter | Select-Object Name,Status,LinkSpeed,MacAddress

$ipConfig = Get-NetIPConfiguration | Select-Object InterfaceAlias,IPv4Address,IPv6Address,DNSServer

$pingTargets = @("8.8.8.8","1.1.1.1","google.com")
$pings = foreach ($target in $pingTargets) {
    $result = Test-Connection $target -Count 4 -ErrorAction SilentlyContinue
    [PSCustomObject]@{
        target = $target
        success = [bool]$result
        avgMs = if ($result) { [math]::Round(($result | Measure-Object ResponseTime -Average).Average,2) } else { $null }
    }
}

$data = @{
    system = $system
    drives = $drives
    networkAdapters = $networkAdapters
    ipConfig = $ipConfig
    pings = $pings
}

$data | ConvertTo-Json -Depth 6 | Out-File "$OutDir\noc_status.json" -Encoding utf8

Write-Host "NOC probe complete."
Write-Host "Output: $OutDir\noc_status.json"