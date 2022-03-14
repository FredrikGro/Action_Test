[![Open in Visual Studio Code](https://classroom.github.com/assets/open-in-vscode-f059dc9a6f8d3a56e377f745f24479a46679e63a5d9fe6f495e02850cd0d8118.svg)](https://classroom.github.com/online_ide?assignment_repo_id=7227226&assignment_repo_type=AssignmentRepo)

### KOM IGÅNG:

Ugå från följande uppgift på Github Classroom: https://classroom.github.com/a/MNosQLzV

När du accepterat uppgiften har du ett repo med ett färdigt Webb-API baserat på REST. Kvar att göra i detta repo är att skapa enhets- och integrationstester samt förbereda ett mindre "CI"-system via **Github Actions**.

Applikationen är så klar som den behöver vara - det du ska göra är att skriva tester och förbereda för agil vidareutveckling med Github. Du kommer behöva sätta dig in i och förstå applikationskoden.


### INLÄMNING:

Kontrollera att ditt repo uppfyller nedanstående kravlista och lämna sedan in en länk till ditt repository. Notera att Pull Request:en som skapas ska alltså inte *mergas* utan finnas kvar vid inlämning.

G KRAV:

 - Skapa till projektet ett **testprojekt** för att skapa enhetstester och integrationstester.
 - Identifiera och skapa minst **2 lämpliga enhetstester** till den givna applikationen och implementera dem. 
 - Skapa minst **ett lämpligt integrationstest** som kan användas lokalt eller efter driftsättning, integrationstestet ska testa svaret från åtkomstpunkt via en HTTP förfrågan och inte importera applikationskoden direkt

VG KRAV:

 - I *main-branchen* av ditt repo skapar en yaml-konfigurationsfil kallad **"deployandtest.yml"** som ligger i rätt mappstruktur för att kunna läsas av github actions.
 - I denna fil ska det finnas **en action** som kör testerna när du gör en Pull Request (PR) mot branchen *main*.
 - Skapa en ny **feature-branch** för ditt repo att arbeta ifrån - ändringarna du gör i denna branch kan vara ytliga (e.g. i readme filen).
 - Skapa på Github en **Pull Request** mot branchen *main*, du ser nu att dina tester körs!sdfdsf

### Scenario:

Webbapplikation du fått i denna  riktad till föreningar som är anslutna till Sveriges Förenade Filmstudios (SFF), där man via ett API och senare även ett klientgränssnitt kan boka/beställa filmer till sin biografverksamhet.

SFF fungerar så att lokala filmintresserade bildar föreningar (en filmstudio), dessa föreningar ingår i medlemskap hos SFF som är förbund för alla filmstudios i Sverige. SFF köper rättigheter från filmdistributörerer att låna ett visst antal exemplar av olika filmer, som SFF sen skickar till lokala föreningar. Filmstudion visar sedan dem på på exempelvis kulturhus och mindre biografer runt om i landet.

Förut skedde detta via blanketter, och filmerna man kunde låna fraktades som stora filmrullar, varför filmer bara kunde visas samtidigt på ett begränsat antal ställen i taget. I dag skickas, och visas, filmerna digitalt - men avtalen ser fortfarande likadana ut; så SFF måste begränsa hur många filmstudios som samtidigt kan visa en viss film!
