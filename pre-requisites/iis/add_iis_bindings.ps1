# if (-NOT(Get-Module -ListAvailable -Name WebAdministration)) {
# 	Install-Module -Name WebAdministration
# } 
# Import-Module -Name WebAdministration -RequiredVersion 1.1.0.0
# Import-Module WebAdministration 

$iisSite = "oneweb.sc" # Must change with your iis site name
$certificate;

function CreateSSLCertificate() {
	$password = "secret"  # put your password on string1	
	$storeLocation = "Cert:\LocalMachine\My"
	$certificate = New-SelfSignedCertificate -DnsName "*.local.ametekweb.com" -CertStoreLocation $storeLocation
	$certificatePath = ("cert:\localmachine\my\" + $certificate.Thumbprint)
	$securedString = ConvertTo-SecureString -String $password -Force -AsPlainText
	Export-PfxCertificate -FilePath "C:\local.ametekweb.com.pfx" -Cert $certificatePath -Password $securedString
	Import-PfxCertificate -FilePath "C:\local.ametekweb.com.pfx" -CertStoreLocation "Cert:\LocalMachine\Root" -Password $securedString
	$global:certificate = $certificate
}
function SetRedirect() {	
	$rulename = $iisSite + ' http to https'	
	$inbound = '(.*)'
	$outbound = 'https://{HTTP_HOST}{REQUEST_URI}'
	$site = 'IIS:\Sites\' + $iisSite
	$root = 'system.webServer/rewrite/rules'
	$filter = "{0}/rule[@name='{1}']" -f $root, $rulename

	$ruleExists = Get-WebConfigurationProperty -PSPath $site -filter $root -Name Collection[name="${$rulename}"]
	if (-NOT($ruleExists)) {		
		Add-WebConfigurationProperty -PSPath $site -filter $root -name '.' -value @{name = $rulename; patterSyntax = 'Regular Expressions'; stopProcessing = 'True' }
		Set-WebConfigurationProperty -PSPath $site -filter "$filter/match" -name 'url' -value $inbound		
		Set-WebConfigurationProperty -PSPath $site -filter "$filter/conditions" -name '.' -value @{input = '{HTTPS}'; matchType = '0'; pattern = '^OFF$'; ignoreCase = 'True'; negate = 'False' }		
		Set-WebConfigurationProperty -PSPath $site -filter "$filter/action" -name 'type' -value 'Redirect'
		Set-WebConfigurationProperty -PSPath $site -filter "$filter/action" -name 'url' -value $outbound
	}
}

function UpdateHostFile($siteName) {	
	$file = "C:\Windows\System32\drivers\etc\hosts"
	$hostfile = Get-Content $file
	if (-NOT($hostfile.Contains($siteName))) {
		$hostfile += "127.0.0.1   ${siteName}"
		Set-Content -Path $file -Value $hostfile -Force
	}	
}

function UpdateBinding ($siteUrl) {
	$binding = (Get-WebBinding -Name $iisSite | where-object { $_.protocol -eq "https" -and $_.bindingInformation.Contains($siteUrl) })
	if ($null -ne $binding) {
		Remove-WebBinding -Name $iisSite -Port 443 -Protocol "https" -HostHeader $siteUrl
	}

	New-WebBinding -Name $iisSite -IPAddress "*" -Port 443 -HostHeader $siteUrl -Protocol "https" -SslFlags 1
	$binding = (Get-WebBinding -Name $iisSite -Port 443 -Protocol "https" -HostHeader $siteUrl)
	if ($binding) {
		$binding.AddSslCertificate($global:certificate.GetCertHashString(), "my");
	}

	$binding = (Get-WebBinding -Name $iisSite | where-object { $_.protocol -eq "http" -and $_.bindingInformation.Contains($siteUrl) })
	if ($null -ne $binding) {
		Remove-WebBinding -Name $iisSite -Port 80 -Protocol "http" -HostHeader $siteUrl
	}
	New-WebBinding -Name $iisSite -IPAddress "*" -Port 80 -HostHeader $siteUrl -Protocol "http"
}


CreateSSLCertificate;
SetRedirect;

foreach ($website in Get-Content ..\site_list.txt) {	
	$website = $website.Trim()
	Write-Host "Adding binding for ${website}"
	$siteName = "${website}-ow.local.ametekweb.com";
	UpdateHostFile($siteName);
	UpdateBinding($siteName);
}