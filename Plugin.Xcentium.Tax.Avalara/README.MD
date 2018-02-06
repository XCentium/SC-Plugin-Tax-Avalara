
Sitecore Commerce Engine AVALARA Tax plugin (Sitecore Commerce 9)
======================================

This plugin allows the user to integrate AVALARA as a tax calculation Method for the ecommerce site. 
- It is very easy to integrate or extend to fit your needs.
- If will fetch tax rates from AVALARA based on the item type and price and of the items in your cart or cartline.


Grouping
========
This is a Tax plugin using AVALARA Api

Sponsor
=======
This plugin was sponsored and created by XCentium.

How to Install
==============

1. Copy it to your Sitecore Commerce Engine Solution and add it as a project 

2. Add it as a dependency to your Sitecore Commerce Engine Project

3. Add it as a dependency to your Adventure works or Habitat or Custom Site plugin' project

4. Create a json file named "Plugin.Xcentium.Tax.Avalara.PolicySet-1.0.0.json" under your "Sitecore.Commerce.Engine"'s Environment folder. Add the json content below, set your AVALARA API key to your Sitecore Commerce Engine's environment json files

```
{
  "$type": "Sitecore.Commerce.Core.PolicySet, Sitecore.Commerce.Core",
  "Id": "Entity-PolicySet-BraintreePolicySet",
  "Version": 1,
  "IsPersisted": false,
  "Name": "AvalaraPolicySet",
  "Policies": {
    "$type": "System.Collections.Generic.List`1[[Sitecore.Commerce.Core.Policy, Sitecore.Commerce.Core]], mscorlib",
    "$values": [
    {
        "$type": "Plugin.Xcentium.Tax.Avalara.AvalaraPolicy, Plugin.Xcentium.Tax.Avalara",
        "CompanyCode": "XXXXXXXXXXX",
        "AccountId": "XXXXXXXXXXX",
        "UserName": "XXXXXXXXXXX",
        "Password": "XXXXXXXXXXX",
        "TestUrl": "https://sandbox-rest.avatax.com/api/v2/transactions/create",
        "ProductionUrl": "https://rest.avatax.com/api/v2/transactions/create",
        "InProductionMode": false,
        "ShipFromAddressLine1": "615 N Nash Street",
        "ShipFromAddressLine2": "303",
        "ShipFromAddressLine3": "",
        "ShipFromCity": "El Segundo",
        "ShipFromStateOrProvinceCode": "CA",
        "ShipFromPostalCode": "90245",
        "ShipFromCountryCode": "US",
        "PolicyId": "AvalaraPolicy",
        "Models": {
        "$type": "System.Collections.Generic.List`1[[Sitecore.Commerce.Core.Model, Sitecore.Commerce.Core]], mscorlib",
        "$values": [
        ]
        }
    }
    ]
  }
}



```

5. Replace XXXXXXXXXXX above with the API connection details you got when you registered with AVALARA,  add your ship from address.


6. You are ready to start using it. 

Note:
=====
- Remember to register with AVALARA and get your API connection details. Also activate your registration and set your AVALARA Nexus choices.

- If you have any questions, comment or need us to help install, extend or adapt to your needs, do not hesitate to reachout to us at XCentium aske about Sitecore Commerce Engine AVALARA Plugin.






