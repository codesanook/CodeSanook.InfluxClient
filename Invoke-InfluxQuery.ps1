param(
    [Parameter(Mandatory=$true)] [string] $query
)

$libraryName = "CodeSanook.Influx"
#Import DLL
$outputDir = Join-Path $PSScriptRoot -ChildPath "codesanook.influx/bin/debug/"
$assemblyPath = Join-Path -Path $outputDir -ChildPath "$libraryName.dll"

#LoadFrom() look for the dependent DLLs in the same directory
$assembly = [Reflection.Assembly]::LoadFrom($assemblyPath)   
$assembly

$configFile = Join-Path $PSScriptRoot -ChildPath "config.yaml"
$influxClient = New-Object -TypeName CodeSanook.Influx.InfluxClient -ArgumentList $configFile
$result = $influxClient.Query($query);

#transform
$columns = $result[0].columns
$columns
$points = $result[0].points

$data = $points | ForEach-Object {
    $item = @{}
    for ($index = 0; $index -lt $columns.Length; $index++){
         $item.Add($columns[$index], $_[$index])
    }
    $item
}


$outputFile = Join-Path -Path $PSScriptRoot -ChildPath "output.txt"
Remove-Item -Path $outputFile -Force -ErrorAction Ignore
New-Item -ItemType File -Path $outputFile | Out-Null

$output = $data | ForEach-Object {
    $row = $_
   "( '$($row.time)', '$($row.machine_name)' )"
} 
$output -join ",`n" | Set-Content -Path $outputFile

