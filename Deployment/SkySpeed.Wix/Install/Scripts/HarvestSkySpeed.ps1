#==============================================================================================================#
#	Subject: Filter all files needed by Sky Speed and harvest it to wxs								   #
#	Notes: binDirectory is a location where product build output files can be found. It can be defined using:  #
#		   - build pipeline variable 'BinOutput' (custom, need to be defined)								   #
#		   - build pipeline built-in variable Pipeline_Workspace (default folder for 'download artifacts)      #
#		   - otherwise script relative path will be used.													   #
#==============================================================================================================#

param
(
	$configuration = "Release"
)

$SkySpeedHarvest = "Features\SkySpeedHarvest.wxs"

if ($PSBoundParameters.ContainsKey('Disable'))
{
	Write-Verbose "Script disabled; no actions will be taken on the files."
}

#set ProjectDir path
$ProjectDir = Resolve-Path "$PSScriptRoot\.."

if (-not (Test-Path $ProjectDir))
{
	Write-Host “##vso[task.logissue type=error;] Path does not exist: $ProjectDir"
	exit 1
}

#find bin files folder
if($null -ne $Env:BinOutPut )
{
	$binDirectory = $Env:BinOutPut
}
if ($env:Pipeline_Workspace)
{
	$binDirectory =  Join-Path $env:Pipeline_Workspace -ChildPath "PublishSkySpeed"
}
else
{
	$binDirectory =  Resolve-Path "$projectDir\..\..\..\Sources\Client\bin"
}

Write-Host "BinDirectory is $binDirectory"

# Copy the required files to Harvest folder
try
{
	$dirForHarvest =  Join-Path "$ProjectDir" -ChildPath "HarvestSkySpeed"
	if(Test-Path $dirForHarvest)
	{
	   Remove-Item $dirForHarvest -force -recurse
	}

	$exclude = @(
		'*.pdb',
		'*.config',
		'*.ini'
	)
    New-Item $dirForHarvest -Type Directory
	Copy-Item -Path $binDirectory\* -Destination $dirForHarvest -Exclude $exclude -force -Recurse
}
catch
{
	Write-Error "Copying the file from: $binDirectory to $dirForHarvest - failed"
	exit 1
}

#harvest
try
{
	Write-Host "Start to harvest from $dirForHarvest"
	$SkySpeedHarvest = Join-Path "$ProjectDir" -ChildPath "$SkySpeedHarvest"
	Set-ItemProperty (Get-ChildItem $SkySpeedHarvest) IsReadOnly $false

	&"$Env:WIX\bin\heat.exe" dir "$dirForHarvest" -cg CG_SkySpeedHarvest -gg -sfrag -srd -sreg -var var.SourcesBinDirSkySpeed -dr SkySpeed -t "$PSScriptRoot\FilterSkySpeedHarvest.xslt" -out "$SkySpeedHarvest" -platform "x86"
}
catch
{
	Write-Error "WixHarvestFolder: $dirForHarvest - failed to harvest!"
	exit 1
}