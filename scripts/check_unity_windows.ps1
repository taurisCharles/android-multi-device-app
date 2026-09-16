$unityPaths = @(
    "$env:ProgramFiles\Unity\Hub\Editor",
    "${env:ProgramFiles(x86)}\Unity\Hub\Editor"
)

Write-Host "Checking for Unity Editor installs..."

$found = $false
foreach ($path in $unityPaths) {
    if (Test-Path $path) {
        Get-ChildItem $path -Directory | ForEach-Object {
            $editor = Join-Path $_.FullName "Editor\Unity.exe"
            if (Test-Path $editor) {
                $found = $true
                Write-Host "Found Unity:" $editor
            }
        }
    }
}

if (-not $found) {
    Write-Host "Unity Editor not found. Install Unity Hub, then add Unity 2D with Android Build Support."
    exit 1
}
