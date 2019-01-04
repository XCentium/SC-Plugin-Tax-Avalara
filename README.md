
Sitecore Commerce Engine AVALARA Tax plugin (Sitecore Commerce 9)
======================================

This plugin allows the user to integrate AVALARA as a tax calculation and reporting application for your Sitecore Experience Commerce site. 
- It is very easy to integrate or extend to fit your needs.
- If will fetch tax rates from AVALARA based on the tax code you assign to the product or variant in your cart.



Features:
===========
- You get to add your configuration settings in Sitecore's launch Pad
- You can test connection and turn AvaTax calculation on or off using Launch Pad interface
- You can add Tax code on the product or Variant level.
- You can set Entity Use Code or Excemption Number on a customer level.
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
	
	2a. under Sitecore.Commerce.Engine project, open the file named SitecoreServicesConfigurationExtensions.cs, look for a line with ```.Add<CalculateCartTaxBlock>()``` after that line, insert a line break and add ```.Add<UpdateCartTaxBlock>().After<CalculateCartTaxBlock>()```
		

3. For the frontend interface, go to Sitecore package installer and install the package Avalara Tax Package-9.0.0.zip [See figure 1.0 on the Avalara Setup Reference.docx](../Avalara Setup Reference.docx)

4. Login to Sitecore content editor, on the ribbon, click on Commerce and click Update Data Templates

5. Go to Sitecore Launch Pad, you will see a huge Avalara Panel button (See figure 2.0 on the Avalara Setup Reference.docx). Click on it and it will take you to your configuration panel (See figure 3.0 on the Avalara Setup Reference.docx).

6. Add all credentials ans shipping address to the form on the panel and click save. 

7. Click the "Test Connection button" If your configuration is all correct, you will get a response of success otherwise you will be notified.

8. You are ready to start using it.

9. Using the Biz Tool under Launch Pad, Navigate to a Sellable Item or its variant and you will see a view with a form field that allows you to add your Avalara tax code (See figure 4.0 and 5.0 on the Avalara Setup Reference.docx).

10. Go ahead add tax code to all your products and variants.

11. Where a product or variant does not have a tax code, a taxcode of "P000000" is assumed

12. You can also navigate to a customer using BizToool and set Entity Use Code or Excemption Number for that customer incase the customer is an institution excempted from being taxed (See figure 6.0 on the Avalara Setup Reference.docx).

Note:
=====
- Remember to register with AVALARA and get your API connection details. Also activate your registration and set your AVALARA Nexus choices.

- If you have any questions, comment or need us to help install, extend or adapt to your needs, do not hesitate to reachout to us at XCentium ask about Sitecore Commerce Engine AVALARA Plugin.






