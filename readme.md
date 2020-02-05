Follow steps: https://github.com/Azure/aad-pod-identity

1: az identity create -g MC_aks-ot-rg_aahnl-kubernetes-ot_westeurope -n testrobert -o json
{
  "clientId": "88ebdf2a-ab1e-4c53-bd96-6653a8cd1423",
  "clientSecretUrl": "https://control-westeurope.identity.azure.net/subscriptions/7b520150-0df3-4adb-83b7-5646c7c62fef/resourcegroups/MC_aks-ot-rg_aahnl-kubernetes-ot_westeurope/providers/Microsoft.ManagedIdentity/userAssignedIdentities/testrobert/credentials?tid=d8f00f92-64fd-40fe-9f0b-e09e6b0875cc&oid=409efa37-b0ca-431c-b55e-555debfedfa4&aid=88ebdf2a-ab1e-4c53-bd96-6653a8cd1423",
  "id": "/subscriptions/7b520150-0df3-4adb-83b7-5646c7c62fef/resourcegroups/MC_aks-ot-rg_aahnl-kubernetes-ot_westeurope/providers/Microsoft.ManagedIdentity/userAssignedIdentities/testrobert",
  "location": "westeurope",
  "name": "testrobert",
  "principalId": "409efa37-b0ca-431c-b55e-555debfedfa4",
  "resourceGroup": "MC_aks-ot-rg_aahnl-kubernetes-ot_westeurope",
  "tags": {},
  "tenantId": "d8f00f92-64fd-40fe-9f0b-e09e6b0875cc",
  "type": "Microsoft.ManagedIdentity/userAssignedIdentities"
}

2. Toekennen aan resource
Reader-role in KeyVault: https://aahnl-keyvault-ot-kv.vault.azure.net/



