# AddressAPI

Before running the application:
Navigate to Pacake Manager Console -> Run the following command to execute the migration script

- add-migration AnyName
- update-database

The application can be run :
1) directly from Visual Studio 
      or
2) from dot net CLI. Execute 'dotnet run' command in command prompt from the directory that has the .csproj.

Proud Of
- Maintained layer abstraction between the Web application and the Repositories so that they can change more independently by following Service- Repository pattern with DI (Dependency Abstraction)
- Google GeoCoding api is in pay-as-you go. Hence researched and used OpenCage Geocoder instead. 
- Including Custom Validations.
- Have written generic piece of code using lambda expression and reflection which will automatically handle any properties that are added to the Address model in the future without requiring any changes to the filtering logic.
- Ensured that I fetch filtered data from database itself instead of loading all the database records in memory and then performing filtering.

More things I could have done
- Run application in docker container using docker-compose.
- Write Unit Test cases 
- Some more error handling
- Custom Validation for the Services as well
- Custom Logging





