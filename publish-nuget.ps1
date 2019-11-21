[CmdletBinding(SupportsShouldProcess=$true)]

param(
    [Parameter(Mandatory=$true,Position=0)]
    [string] $version,
    [Parameter(Mandatory=$true,Position=1)]
    [string] $key
)

if($PSCmdlet.ShouldProcess("./Monads.Standard/bin/Release/Darthruneis.Monads.$version.nupkg", "Nuget-Push"))  {
    (dotnet nuget push "./Monads.Standard/bin/Release/Darthruneis.Monads.$version.nupkg" -k $key -s https://api.nuget.org/v3/index.json)
}
else {
    Write-Information ("Would have pushed " + "./Monads.Standard/bin/Release/Darthruneis.Monads.$version.nupkg" + "to https://api.nuget.org/v3/index.json using key $key")
}