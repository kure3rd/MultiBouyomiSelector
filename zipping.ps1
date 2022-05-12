$cwd = Get-Location
Write-Output $cwd
Write-Output $cwd

$outputDir = (Join-Path $cwd "\Output")
Write-Output $outputDir
if (Test-Path $outputDir) {}
else {New-Item $outputDir -ItemType Directory}

$targetPath = (Join-Path $cwd "bin\Debug\net48\*")
Write-Output $targetPath

$outFileName = "MultiBouyomiSelector.zip"
$outFilePath = (Join-Path $outputDir $outFileName)

Compress-Archive -Path $targetPath -DestinationPath $outFilePath -Force

#Write-Output $outputDir
#Write-Output $outFilePath