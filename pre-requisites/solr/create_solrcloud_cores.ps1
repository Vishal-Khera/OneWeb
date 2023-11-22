$solrUrl = "https://ss749062-6wx8683i-eastus-azure.searchstax.com";

# dev
# https://ss749062-6wx8683i-eastus-azure.searchstax.com
# staging
# https://ss749062-6wx8683i-eastus-azure.searchstax.com
# prod
# https://ss618435-iwjhmy0f-eastus-azure.searchstax.com

$coreAdmin="${$solrUrl}/solr/admin/collections"
$configName="oneweb_configset"
$numShards=1
$replicationFactor=1

$apiUser = "sc"
$apiUserPassword = "RephYrM9D1n"

#Create our credentials object
$pwd = ConvertTo-SecureString $apiUserPassword -AsPlainText -Force;
$Cred = New-Object System.Management.Automation.PSCredential ($apiUser, $pwd);
$coreName = "oneweb_ow"

$url = "${coreAdmin}?action=CREATE&name=${coreName}&numShards=${numShards}&replicationFactor=${replicationFactor}&collection.configName=${configName}"	
Invoke-WebRequest $url -UseBasicParsing -Credential $Cred -Method Post | Out-Null