Follow steps: https://github.com/Azure/aad-pod-identity

1. az identity create -g MC_aks-ot-rg_aahnl-kubernetes-ot_westeurope -n testrobert -o json 
   (=resourcegroep van AKS, dan heeft AKS service principal al genoeg rechten. Andere resourcegroep, dan stap 6 uitvoeren: https://github.com/Azure/aad-pod-identity#6-set-permissions-for-mic)
2. Toekennen aan resource (Reader/Contributor volstaat meestal)
3. Sommige resources vereisen meer. Bv KeyVault: access policy toevoegen voor identity
