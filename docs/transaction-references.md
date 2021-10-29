# Transaction Reference

## algemeen

De volgende eigenschappen zijn van belang:

- max 16 karakters
- uniek in tijd en over applicaties/machines
- singleton en thread safe

Deze ```TransactionReference``` wordt gebruikt in (SWIFT) berichtenbverkeer. Elk bericht moet een unieke waarde bevatten.

Echter ook voor interne projecten is deze interessant, bij wijze van correlatie-id.


## opzet

De opzet is om een ```DateTime``` als uitgangspunt te nemen. De huidige tijd (zonder timezones).

De tijd en datum zijn niet het onderwerp en zijn niet relevant op zichzelf. De uniciteit van de waarde is het belangrijkst.

Het idee is om een "semi-leesbare" transaction reference op te leveren, die ook nog uit te spreken en te lezen is in een user interface.

In de waarde zit een vast gegeven verwerkt, een _identity_. Dat bestaat uit 1 of 2 karakters, bij voorkeur een hoofdletter of cijfer. Bijvoorbeeld:
"A", "DB", of "A1".

> Door deze identity per instance van de applicatie en per applicatie te laten verschillen, kunnen unieke waarden worden aangemaakt binnen de origanisatie

## formaat

Het formaat is een datum en een tijd in oplopende volgorde met tussen datum en tijd een _identity_ waarde.

Elke gegenereerde waarde is exact 16 karakters. 
De resolutie is 1 milliseconde.

Het formaat ziet er zo uit:

```
    <YYY><DOY><ID><HHMMSSmmm>
```

De volgende elementen

- YYY = het jaartal
  - 4 cijfers (volledig) als er geen _identity_ is meegegeven (niet aan te raden, niet schaalbaar)
  - 3 cijfers (bijv. 021 voor 2021) met een _identity_ van 1 teken. Goed voor ~900 jaar
  - 2 cijfers (bijc 21 voor 2021) met een _identity_ van 2 tekens. Goed voor een eeuw.
- DOY = dag van het jaar. Een waarde tussen 1 en 366. Vervangt dag en maand.
- ID = de _identity_ van 1 of twee tekens
- HHMMSSmmm = uur, minuten, seconden en milliseconden. Aan elkaar, en altijd uit dit aantal tekens (ook bij waarden < 10).


De _day of year_ maakt de datum 1 teken korter dan een MAAND-DAG combinatie en bevat dezelfde informatie.

> Het decimale stelsel is gebruikt om de waarden weer te geven.
> Hexadecimaal kan ook, maar levert op zich weinig winst op (al kan dan wel het volledige jaar meegenomen worden bij een _identity_ van twee tekens)


