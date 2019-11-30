[CmdletBinding(SupportsShouldProcess=$true)]

param(
    [Parameter(Mandatory=$true,Position=0)]
    [string] $version,
    [Parameter(Mandatory=$true,Position=1)]
    [string] $key,
    [Parameter(Mandatory=$false,Position=2)]
    [switch] $release
)

$binFolder = "Debug";
if($release) {
    $binFolder = "Release";
}


if($PSCmdlet.ShouldProcess("./Monads.Standard/bin/$binFolder/Darthruneis.Monads.$version.nupkg", "Nuget-Push"))  {
    (dotnet nuget push "./Monads.Standard/bin/$binFolder/Darthruneis.Monads.$version.nupkg" -k $key -s https://api.nuget.org/v3/index.json)
}
else {
    Write-Information ("Would have pushed " + "./Monads.Standard/bin/$binFolder/Darthruneis.Monads.$version.nupkg" + "to https://api.nuget.org/v3/index.json using key $key")
}