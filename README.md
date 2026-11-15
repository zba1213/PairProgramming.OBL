# DR musiksamling – backend

Første brugerhistorie: Som bruger vil jeg se alle musikplader.

## Projekter
- Dette repository: ASP.NET Core REST API og MSTest repository-tests.
- `PairProgramming.OBL.Frontend`: HTML, CSS, Vue og Axios.
- `PairProgramming.OBL.UITests`: separat C#-solution med MSTest og Selenium.

## Kør lokalt
Installer .NET 8 SDK. Åbn `MusicRecords.sln` i Visual Studio, eller kør:

```sh
dotnet test MusicRecords.sln
dotnet run --project src/MusicRecords.Api --launch-profile http
```

API: http://localhost:5000/api/musicrecords
Swagger: http://localhost:5000/swagger

GET returnerer HTTP 200 og en JSON-liste (også når listen er tom).
Data ligger foreløbig i en List med tre eksempelplader; der er ingen database endnu.
Varighed måles i sekunder og udgivelsesår er et heltal.
CORS tillader localhost og 127.0.0.1 på port 5500. Ved hosting sættes
`Cors__AllowedOrigins__0` til frontendens præcise origin.

## Postman
Importer `postman/MusicRecords.postman_collection.json`. Variablen `baseUrl`
er som standard `http://localhost:5000`. Kør collection og gem testresultater til afleveringen.

## Arbejdsplan
1. Vis alle plader (denne feature branch).
2. Søg og filtrér.
3. Login/logout med JWT og roller, samt Vue-komponenter.
4. Tilføj/slet og skift til database.
5. Redigér.

Færdiggør backend, frontend og tests for hver historie før næste.
Brug feature branch og pull request i de relevante repositories.
Tracks og yderligere kunstneroplysninger er ekstraopgaver.

## Hosting og aflevering
Backend kan foreløbig køre lokalt som opgaven tillader. Azure-deployment kræver
valg af App Service og tilknyttede deployment-rettigheder; CI-workflowet kører tests,
men deployer endnu ikke. Frontend skal senere uploades via FTP.
Aflever én tekstfil pr. par med de tre offentlige GitHub-links og Postman-tests
(screenshots eller URL og scripts).
