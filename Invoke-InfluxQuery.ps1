
$libraryName = "CodeSanook.Influx"
#Import DLL
$outputDir = Join-Path $PSScriptRoot -ChildPath "codesanook.influx/bin/debug/"
$assemblyPath = Join-Path -Path $outputDir -ChildPath "$libraryName.dll"

#LoadFrom() look for the dependent DLLs in the same directory
$assembly = [Reflection.Assembly]::LoadFrom($assemblyPath)   
$assembly

$configFile = Join-Path $PSScriptRoot -ChildPath "config.yaml"
$influxClient = New-Object -TypeName CodeSanook.Influx.InfluxClient -ArgumentList "$configFile"
