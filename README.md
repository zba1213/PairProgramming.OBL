# DR musiksamling – backend

REST API til at vise, søge i og administrere DR's musiksamling.

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
Data ligger i tabellen `MusicRecords`. Migreringen opretter tabellen og indlæser
de tre eksempelplader første gang.
Varighed måles i sekunder og udgivelsesår er et heltal.
CORS tillader localhost og 127.0.0.1 på port 5500. Ved hosting sættes
`Cors__AllowedOrigins__0` til frontendens præcise origin.

## Database og ændringer

Lokalt bruger API'et filen `musicrecords.db` i arbejdsmappen. Brug
`ConnectionStrings__LocalMusicRecords` til at vælge en separat SQLite-fil til test.
Sæt `ConnectionStrings__MusicRecords` som hemmelighed i hostingmiljøet for at bruge
en eksisterende Azure SQL- eller SQL Server-database. Ved første opstart
skal databasekontoen kunne oprette tabellen og migreringshistorikken. Start én
API-instans, mens migreringen køres.

- `GET /api/musicrecords/{id}` returnerer `200` eller `404`.
- `POST /api/musicrecords` opretter en plade og returnerer `201` med dens URL.
  Ugyldige felter giver `400`.
- `DELETE /api/musicrecords/{id}` returnerer `204` eller `404`.

Tilføjelse og sletning kræver et JWT for en bruger med rollen `Editor`.
Manglende eller ugyldigt token giver `401`, og en anden rolle giver `403`.
I Swagger kan tokenet indsættes under **Authorize**.

## Postman
Importer `postman/MusicRecords.postman_collection.json`. Variablen `baseUrl`
er som standard `http://localhost:5000`. Kør hele collectionen mod en testdatabase
med en Editor-konto. Login gemmer tokenet for det aktuelle run, som opretter og
sletter en særskilt testplade. Gem testresultaterne til afleveringen.

## Login lokalt

Sæt miljøvariablerne i samme terminal, før du starter API'et. Eksempel med PowerShell:

```powershell
$env:Auth__SigningKey = [Convert]::ToBase64String([System.Security.Cryptography.RandomNumberGenerator]::GetBytes(32))
$env:Auth__Users__0__Username = 'test-editor'
$env:Auth__Users__0__Password = Read-Host 'Adgangskode til den lokale testkonto'
$env:Auth__Users__0__Role = 'Editor'
dotnet run --project src/MusicRecords.Api --launch-profile http
```

Brug næste indeks til en eventuel ekstra konto. Gem værdierne som hemmeligheder i
hostingmiljøet, og brug HTTPS uden for localhost. API'et kan starte uden konti,
så listevisning og søgning stadig kan testes med standardopsætningen.

`POST /api/auth/login` bruger JSON-felterne `username` og `password` og
returnerer `200` med et JWT gyldigt i 30 minutter, `400` ved ugyldigt input
eller `401` ved ukendte legitimationsoplysninger. Kopiér testkontoens værdier
til Postman-variablerne `loginUsername`, `loginPassword` og `loginRole`.
Logout rydder browserens session. Et allerede udstedt token udløber på sit
normale udløbstidspunkt.

## Arbejdsplan
1. Vis alle plader.
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
