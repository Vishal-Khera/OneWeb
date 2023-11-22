$solrUrl = "https://solr75:8975" # Must change with your solr url
$solrPath = "D:\solr\solr-7.5.0" # Must change with your solr server path

Copy-item -Force -Recurse -Verbose ".\configsets" -Destination "${solrPath}\server\solr"

$coreAdmin = "$solrUrl/solr/admin/cores" 
$coreBaseDir = "$solrPath\server\solr" 
function CreateCore ($coreName) {
	$corePingUrl = "${coreAdmin}?action=STATUS&core=${coreName}";
	$corePingResponse = Invoke-WebRequest $corePingUrl -UseBasicParsing -Method Post
	if ($corePingResponse.Content.Contains('instanceDir')) {
		Write-Host "Unloading Core for ${corename}"
		$coreUnloadUrl = "${coreAdmin}?action=UNLOAD&deleteInstanceDir=true&core=${coreName}"
		$coreUnloadResponse = Invoke-WebRequest $coreUnloadUrl -UseBasicParsing -Method Post
		Write-Host $coreUnloadResponse		
	}
	$instanceDir = join-path $coreBaseDir $coreName	
	$coreCreateUrl = "${coreAdmin}?action=CREATE&name=${coreName}&instanceDir=${instanceDir}&configSet=oneweb_configset"
	$coreCreateResponse = Invoke-WebRequest $coreCreateUrl -UseBasicParsing -Method Post
	Write-Host $coreCreateResponse
}

foreach ($line in Get-Content ..\site_list.txt) {
	
	Write-Host "Creating Cores for ${line}"	
	$masterCore = $line.Trim() + "_ow_cms"
	$webCore = $line.Trim() + "_ow";

	CreateCore($masterCore);
	CreateCore($webCore);
}