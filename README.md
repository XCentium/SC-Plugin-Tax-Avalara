
Sitecore Commerce Engine AVALARA Tax plugin (Sitecore Commerce 9)
======================================

This plugin allows the user to integrate AVALARA as a tax calculation Method for the ecommerce site. 
- It is very easy to integrate or extend to fit your needs.
- If will fetch tax rates from AVALARA based on the item type and price and of the items in your cart or cartline.



Features:
===========
- You get to add your Configuration settings in Sitecore's launch Pad
- You can test connection and turn AvaTax calculation on or off using Launch Pad interface
- You can add Tax code on the product or Variant level.
- You can set tax code on a customer level.
- You can opt in or out of tax reporting.


Grouping
========
This is a Tax plugin using AVALARA Api

Sponsor
=======
This plugin was sponsored and created by XCentium.

How to Install
==============

1. Copy the Sitecore.Commerce.Plugin.Avalara project folder to your Sitecore Commerce Engine Solution and add it as a project 

2. Add it as a dependency to your Sitecore Commerce Engine Project and deploy your commerce project as usual.

3. For the frontend interface, go to Sitecore package installer and install the package Avalara Tax Package-9.0.0.zip

4. Login to Sitecore content editor, on the ribbon, click on Commerce and click Update Data Templates

5. Go to Sitecore Launch Pad, you will see a huge Avalara Panel button. Click on it and it will take you to your configuration panel.

6. Add all credentials ans shipping address to the form on the panel and click save. 

7. Click the "Test Connection button" If your configuration is all correct, you will get a response of success otherwise you will be notified.

8. You are ready to start using it.

9. Using the Biz Tool under Launch Pad, Navigate to a Sellable Item or its variant and you will see a view with a form field that allows you to add your Avalara tax code.

10. Go ahead add tax code to all your products and variants.

11. Where a product or variant does not have a tax code, a taxcode of "P000000" is assumed

Note:
=====
- Remember to register with AVALARA and get your API connection details. Also activate your registration and set your AVALARA Nexus choices.

- If you have any questions, comment or need us to help install, extend or adapt to your needs, do not hesitate to reachout to us at XCentium ask about Sitecore Commerce Engine AVALARA Plugin.






