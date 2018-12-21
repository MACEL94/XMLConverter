# XMLConverter

[![Build Status](https://francesco-belacca.visualstudio.com/XMLConverter/_apis/build/status/XMLConverter%20-%201%20-%20CI?branchName=master)](https://francesco-belacca.visualstudio.com/XMLConverter/_build/latest?definitionId=2?branchName=master)

Volevo un tool che convertisse un xml in classi di c# serializzabili, ma in modo corretto e prendendo solo gli elementi utili.
Non trovandolo me lo sono creato!

Deserializza un xml che gli si da in pasto, e crea le corrispettive classi in c# mantenendo i formati di datetime e decimal.
Compila la classe e chiede dove si vuole salvare la DLL, ho reso visibili solo i campi interessanti all'utilizzatore,
lasciando privato tutto ciò che mi serve per la gestione dei formati e eventuali serializzazioni o deserializzazioni dei tipi nullable.

Infine crea un metodo che istanzia un oggetto della classe presente nella dll appena creata, popolandolo con i dati dell'xml di partenza,
cosa che torna molto utile in caso di unit testing, è gestito autonomamente la creazione di un nuovo metodo o la sovrascrittura di questo
se già presente, all'interno del file path che si sceglie di specificare.

Francesco Belacca

https://xmlconverter.azurewebsites.net/
