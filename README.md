# Data Tier For The Greenhouse Monitoring System
## Steps to test :
- Clone this repository in a directory of your choice.
- Make sure you have Sql Server  installed.
- For reference, the IndoorClimateData.sql script can be found in the Fake Data folder.
- Set up SQL Server to work with SQL Server Authentication :
  - Go into SSMS and connect to your local server.
  - Right click your server and choose properties.
  - Under select a page, choose Security.
  - Under server authentication, choose 'SQL Server and Windows Authentication mode'
  - Click OK to save your changes.
  - Go into the security folder in your server.
  - Right click the Logins folder.
  - Choose SQL Server authentication in the window that pops up.
  - Choose a name for the user in the top of the page.
  - Enter a password and confirm (use '12345' for simplicity's sake)
  - Click Ok to save your changes.
  - Close SSMS.
  - Go into SQL Server Configuration Manager.
  - Right Click  SQLServer(MSSQLSERVER)
  - Click restart.
  - Now open SSMS again and when the 'Connect to Server' windows pops up, change Authentication to SQL Server Authentication.
  - Input your previously created credentials.
  - This means that authentication works now.
- Generate database through EFC:
  - Open this cloned repository in an IDE of your choice (e.g Rider, Visual Studio).
  - Go to EFCData and open the GreenhouseContext class.
  - Replace the properties in the connection string (of the optionsBuilder.UseSqlServer) with your Server Name, the user name and the password you created.
  - Go to the terminal and type in these commands:
     cd EFCData
     dotnet ef migrations add InitialMigration
     dotnet ef update database
- Populate the database with fake data (Optional), else insert values manually into the rows in SSMS:
  - Create a new database named GreenhouseTest.
  - Run the IndoorClimateData.sql script on this db.
  - Run the 'InitialFakeData.sql' script (make sure that the FROM statement accurately describes the name of the table that the fake data is in).
- Run the Web API and use the endpoints from Swagger in your app to test.
