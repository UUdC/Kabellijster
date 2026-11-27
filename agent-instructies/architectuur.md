# Architectuur (levend document)
WinForms-app die een Excel-kabellijst controleert tegen een PDF-tekeningenpakket.
De C#-namespace is `Kabellijster`.

## Doel
Verschillen vinden tussen kabels in een Excel-kabellijst en een PDF-tekeningenpakket.
PDF-uitlezing, vergelijking en rapportage volgen later.

## Mappen
- `UI/`: WinForms-schermen en bediening.
- `Readers/`: inlezen van bestanden en bijbehorende instellingen.
- `Assets/`: voorbeeld- en projectbestanden.
- `agent-instructies/`: agentinstructies en projectdocumentatie.

## Richtlijnen
- Elk bestand heeft één functie of doel.
- Modulariteit en flexibiliteit door externalisering van data en instellingen.

## Excel
- `excel-instellingen.json` naast de applicatie bepaalt de kolomletters en eerste datarij;
  het projectbestand kopieert de instellingen bij het bouwen naar de uitvoermap.
- De kolommen zijn voorbeelden: controleer vooral of diameter, bronkast en doelkast
  overeenkomen met de gekozen kabellijst. G is in het voorbeeldbestand aderdoorsnede.
- Alle tabbladen worden vanaf de ingestelde rij gelezen. Elke niet-lege rij wordt
  als kabel opgenomen, ook als de ingestelde kolommen of kabelnaam leeg zijn.