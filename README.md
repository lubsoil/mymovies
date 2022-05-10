# My Movies
created by Emil Andrzejak

## How to launch project in dev mode
1. Download the project.
2. Open the solution (MyMoviesAPI.sln from MyMoviesAPI directory) in Visual Studio.
3. In appsetting.json change database settings in "MovieContext" section.
4. If the table has not been created, it should be created together with the migration assistance.
    * Open the NuGet Manager Console
    * Enter "Add-Migration InitialCreate" to create migration files
    * Enter "Update-Database" to create Table.
5. Launch MyMovies with MyMovies configuration
6. Launch vue project with command "npm run serve" in "webclient" directory.
