$BHOME=$PSScriptRoot
function Remove-IfExists([string] $path) {
    if (Test-Path $path) {
        Remove-Item -Recurse -Force $path
    }
}
Remove-IfExists $BHOME/publish
New-Item -Path $BHOME/publish/win64 -ItemType "Directory"

$timestamp = Get-Date -Format o | ForEach-Object { $_ -replace ":", "-" }
$logname = "build_$timestamp.log"
dotnet publish -c Release -r win-x64 -o publish/win64 --version-suffix "win64" --self-contained --nologo --verbosity detailed -p:PublishSingleFile=true -p:PublishReadyToRun=true | Tee-Object "$BHOME/$logname"
