# NationalCookies

![enter image description here](https://www.pluralsight.com/content/dam/pluralsight/newsroom/brand-assets/logos/pluralsight-logo-vrt-color-2.png)  

Hi! 

Welcome to the GitHub repository of the nationalcookies application.
This app is the demo app for the Pluralsight course [Building Cloud-Native Solutions for Azure with Visual Studio](https://www.pluralsight.com/courses/building-cloud-native-solutions-azure-visual-studio).

You can download a copy of the code and follow along in the course.

You can see the end result running live here: [https://www.nationalcookies.nl/](https://www.nationalcookies.nl/)

The solution consists of:

 - NationalCookies.MVC
	 - This is the website for the Nationalcookies cookie store
	 - Technology: ASP.NET Core MVC 2.1
 - NationalCookies.Data
	 - This is a class library that contains classes to connect to the Cosmos DB database and work with cookies and orders
	 - Technology: 
	 	- Class library (.NET Standard 2.0)

There are three branches in this repo:
 - main
 	- The branch that contains the MVC and Data projects. This branch is used in the module **Building Azure Cosmos DB Apps with Visual Studio**
 - withazurefunction
 	- This branch contains an additional project with an Azure Function in it. This branch is used in the module **Creating Azure Functions in Visual Studio**
 - withcontainers
 	- This branch contains changes to support running the application in containers. It also contains an additional Web API project. This branch is used in the module **Creating Container-based Apps in Visual Studio**
	

### How to get this code working 
The website needs a database to get information about cookies and read and write order information. The database that we are using is an Azure Cosmos DB. This can be one that you run with the local emulator (https://aka.ms/cosmosdb-emulator) or an Azure Cosmos DB that you run in Azure. 

Once you have a Cosmos DB running, fill in the Cosmos DB connection details in the appsettings.json file of the NationalCookies.MVC project.


That's it! Now, you can run the NationalCookies.MVC project and follow along with the course. 

Thanks for watching and let me know if you have any questions!
