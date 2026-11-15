# Arbejdsproces

## Aftale
- En ny feature branch pr. brugerhistorie i de relevante repositories.
- Ingen push uden brugerens udtrykkelige godkendelse.
- Efter godkendt push: pull request til main, review og efterfølgende merge.
- Ingen ny brugerhistorie før backend, frontend og tests for den aktuelle er færdige.
- Bevar historikken; undgå squash ved merge, så arbejdsprocessen kan følges.

## Brugerhistorie 1: Vis alle musikplader
Branch i alle tre repositories: `feature/list-music-records`.

1. Backend: record-model, listebaseret repository, GET-endpoint, CORS og Swagger.
2. Repository-tests: tom liste, alle felter, eksempeldata og isolering af input/output.
3. Frontend: Vue/Axios, tabel og tilstande for indlæsning, tom liste og fejl.
4. Postman-collection og separat MSTest/Selenium-solution.
5. Ved første UI-test startede API med tom liste: dependency injection valgte
   konstruktøren med IEnumerable. Rettet med eksplicit factory-registrering.
6. Afsluttende verifikation: 4 repository-tests, 3 Postman-assertions via Newman
   og 1 Selenium-test bestod mod lokalt kørende backend og frontend.
7. Swagger kontrolleret: GET /api/musicrecords dokumenterer HTTP 200.

De to nye repositories har en lokal main med kun .gitignore som grundlag for
feature-branchen. Disse main-branches skal også pushes ved første publicering,
så pull requests kan oprettes. Afventer brugerens godkendelse.

Implementering er udført med Codex. Commits repræsenterer det faktisk udførte
arbejde; de er ikke konstrueret som om udviklingen blev udført manuelt over tid.
Azure/FTP er endnu ikke konfigureret; backend anvender opgavens localhost-mulighed.
