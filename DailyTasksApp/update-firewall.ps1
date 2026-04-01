# file: update-firewall.ps1
# author: Yug Patel
# last modified: 31 March 2026
# a script to update the firewall with the local machine's ip address to allow access to database
$currentIp = (Invoke-RestMethod -Uri "https://api.ipify.org")
$resourceGroup = "DailyTasksApp_group"
$serverName = "dailytasksappdb-server"
$ruleName = "Yug-Lenovo"

Write-Host "Our current Ip: $currentIp"

az sql server firewall-rule create `
    --resource-group $resourceGroup `
    --server $serverName `
    --name $ruleName `
    --start-ip-address $currentIp `
    --end-ip-address $currentIp
   
Write-Host "Firewall updated successfully"