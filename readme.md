Follow steps: https://github.com/Azure/aad-pod-identity

1: az identity create -g aks-ot-rg -n testrobert -o json
{
  "clientId": "8ee62c99-90f1-47ee-99e3-cb36d06637f0",
  "clientSecretUrl": "https://control-westeurope.identity.azure.net/subscriptions/7b520150-0df3-4adb-83b7-5646c7c62fef/resourcegroups/aks-ot-rg/providers/Microsoft.ManagedIdentity/userAssignedIdentities/testrobert/credentials?tid=d8f00f92-64fd-40fe-9f0b-e09e6b0875cc&oid=3d259623-491e-4f5d-a6f5-73a527ebdea3&aid=8ee62c99-90f1-47ee-99e3-cb36d06637f0",
  "id": "/subscriptions/7b520150-0df3-4adb-83b7-5646c7c62fef/resourcegroups/aks-ot-rg/providers/Microsoft.ManagedIdentity/userAssignedIdentities/testrobert",
  "location": "westeurope",
  "name": "testrobert",
  "principalId": "3d259623-491e-4f5d-a6f5-73a527ebdea3",
  "resourceGroup": "aks-ot-rg",
  "tags": {},
  "tenantId": "d8f00f92-64fd-40fe-9f0b-e09e6b0875cc",
  "type": "Microsoft.ManagedIdentity/userAssignedIdentities"
}

2. Toekennen aan resource
Reader-role in KeyVault: https://aahnl-keyvault-ot-kv.vault.azure.net/



