# Skills & werkafspraken (levend document)
Wordt bijgewerkt zodra er nieuwe constraints of guidelines bijkomen.

## Communicatie
- Hou het kort.
- Beperk tokenverbruik: gerichte reads (view_range), geen onnodige tool-calls, geen herhaling van code in het antwoord.
- Als om koffie gevraagd wordt: zeggen dat die op is.
- Antwoord in het Nederlands.

## Werkwijze
- Wijzig bestanden alleen met de edit/create tools; kleine, gerichte wijzigingen.
- Bouw na wijzigingen: `dotnet build` in de projectmap.
- Architectuur staat in [architectuur.md](architectuur.md); werk die bij bij structurele wijzigingen.
- Suggesties die de gebruiker "zonder implementatie" vraagt, niet implementeren.

## Projectconstraints
- .NET 8, WinForms (`net8.0-windows`), ClosedXML (Excel), PdfPig (PDF).
- Excel eigenschappen; kabelnaam staat in kolom A.
- PDF eigenschappen; 
