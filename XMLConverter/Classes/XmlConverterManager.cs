using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace XMLConverter.Classes
{
    public class XmlConverterManager
    {
        #region Private Fields

        /// <summary>
        /// Contatore Elementi figli 
        /// </summary>
        private int _contatoreFiglio = 1;

        /// <summary>
        /// Faccio scegliere il namespace perchè altrimenti si creerebbero conflitti se si
        /// importassero due DLL generate con lo stesso Con nomi degli elementi uguali ma
        /// implementazioni diverse
        /// </summary>
        private string _namespaceScelto;

        #endregion Private Fields

        #region Public Constructors

        public XmlConverterManager()
        {
            this.AggiungiFormatoDateTimeSpeciale = false;
        }

        #endregion Public Constructors

        #region Private Properties

        /// <summary>
        /// Il formato MMyy può causare problemi quando si hanno nel file dei numeri tipo "1234" che
        /// NON sono date delle carte di credito
        /// </summary>
        private bool AggiungiFormatoDateTimeSpeciale { get; set; }

        /// <summary>
        /// Tiene in memoria tutti i formati esistenti una volta caricati 
        /// </summary>
        private List<string> ListaFormatiDateTimeStandard { get; set; }

        private List<Tuple<string, CultureInfo>> ListaTupleFormatiCultureDecimal { get; set; }

        #endregion Private Properties

        #region Public Methods

        public void Start()
        {
            // Chiedo il path completo dell'xml all'utilizzatore
            string pathXmlFile = null;
            XDocument documentoCaricato = null;
            do
            {
                Console.WriteLine("Specify the complete path of the xml that you want to convert and initialize:");
                pathXmlFile = Console.ReadLine();
                Console.WriteLine();
                // Mi accerto che esista il path specificato e che sia un xml valido
                if (!string.IsNullOrWhiteSpace(pathXmlFile) || pathXmlFile.EndsWith(".xml") || File.Exists(pathXmlFile))
                {
                    try
                    {
                        documentoCaricato = XDocument.Load(pathXmlFile);
                    }
                    catch
                    {
                    }
                }

                // Se è null per una qualche ragione il percorso o il file contenuto nel percorso non
                // erano validi
                if (documentoCaricato == null)
                {
                    Console.WriteLine(pathXmlFile + " Is not a valid xml file path.");
                }
            } while (documentoCaricato == null);

            // Chiedo subito se il cliente vuole che io tratti anche il formato mmYY oppure no
            this.ImpostaScelteGlobaliUtente();

            // N.B. Per ciascun elemento controlla: Se ha attributi converte l'elemento in classe Se
            // ha solo un valore diventa una proprietà della classe formata dal primo oggetto padre
            // che lo contiene Se compare più di una volta nel primo elemento padre che si trova già
            // presente nel dizionario, si deve fare una lista di questo in quel padre
            var dizionarioElementiValidi = new Dictionary<string, ElementoValido>();
            var dizionarioElementiProprieta = new Dictionary<string, ElementoValido>();
            var listaElementi = documentoCaricato.Descendants();

            var nomeClasseAttuale = Util.RendiPrimaLetteraMaiuscola(listaElementi.First().Name.LocalName);
            var nomeFileAttuale = nomeClasseAttuale + ".cs";

            foreach (var elemento in listaElementi)
            {
                // Controlla che non sia nel dizionario
                if (!dizionarioElementiValidi.TryGetValue(elemento.Name.LocalName, out ElementoValido elementoValidoPresente))
                {
                    // Se non è presente lo aggiunge, se va aggiunto, ossia se ha più di un figlio
                    // oppure se ha degli attributi
                    if (elemento.Elements().Count() > 0 || elemento.Attributes().Count() > 0)
                    {
                        dizionarioElementiValidi.Add(elemento.Name.LocalName, new ElementoValido(elemento));
                    }
                    else
                    {
                        // Se siamo qui non è presente e non va aggiunto, che significa che deve
                        // essere una proprietà del primo elemento padre che si incontra nel
                        // dizionario. Questo tipo di verifica va però fatto alla fine del parsing di
                        // tutti gli elementi validi per il dizionario, quindi lo mettiamo in un
                        // altro dizionario per ora. Se è già presente lo aggiungiamo a se stesso,
                        // altrimenti al dizionario, ci servirà a capire se è una lista o meno
                        if (dizionarioElementiProprieta.TryGetValue(elemento.Name.LocalName, out ElementoValido elementoProprietaPresente))
                        {
                            elementoProprietaPresente.ListaElementiTipologiaAttuale.Add(elemento);
                        }
                        else
                        {
                            dizionarioElementiProprieta.Add(elemento.Name.LocalName, new ElementoValido(elemento));
                        }
                    }
                }
                else
                {
                    //Se invece è presente, lo aggiunge alla lista di elementi di questo tipo,
                    // poi verrà utilizzato per confrontare quali proprietà sono di quale tipo
                    elementoValidoPresente.ListaElementiTipologiaAttuale.Add(elemento);
                }
            }

            // Se ci sono elementi sia in un dizionario che nell'altro, sono sempre elementi validi
            // ma che in casi particolari sembrano attributi
            foreach (var voceDizionarioProprieta in dizionarioElementiProprieta.ToList())
            {
                if (dizionarioElementiValidi.TryGetValue(voceDizionarioProprieta.Key, out var elementoValido))
                {
                    // Li sposto nell'altro dizionario
                    elementoValido.ListaElementiTipologiaAttuale.AddRange(voceDizionarioProprieta.Value.ListaElementiTipologiaAttuale);

                    // Li elimino da questo
                    dizionarioElementiProprieta.Remove(voceDizionarioProprieta.Key);
                }
            }

            // Per comodità e chiarezza
            var listaElementiProprieta = dizionarioElementiProprieta.Values;
            var listaElementiValidi = dizionarioElementiValidi.Values;

            foreach (var elementoProprieta in listaElementiProprieta)
            {
                foreach (var elementoValido in listaElementiValidi)
                {
                    // Se almeno uno di loro è contenuto in almeno uno degli altri lo aggiunge alle
                    // proprietà del rispettivo
                    foreach (var elementoTipoAttuale in elementoProprieta.ListaElementiTipologiaAttuale)
                    {
                        if (elementoValido.ListaElementiTipologiaAttuale.Any(e => e.Element(elementoTipoAttuale.Name) != null))
                        {
                            elementoProprieta.ElementoRipetutoAlmenoUnaVolta = elementoValido.ListaElementiTipologiaAttuale.Any(e => e.Elements(elementoTipoAttuale.Name).Count() > 1);
                            elementoValido.ListaElementiProprieta.Add(elementoProprieta);
                            break;
                        }
                    }
                }
            }

            // Classe serializzata
            var classeSerializzataString = this.CreaStringaClasseSerializzata(documentoCaricato, listaElementiValidi);

            var oggettoSerializzato = Util.CreaOggettoSerializzato(documentoCaricato, nomeClasseAttuale, classeSerializzataString, this._namespaceScelto);

            Util.TestaUguaglianzaDocumentoOggetto(documentoCaricato, oggettoSerializzato);

            // Chiedo all'utente dove vuole salvare la classe, se serve
            string classPath;
            Console.WriteLine("Specify the complete path of the new class if you want to create it or just press Enter to skip: ");
            classPath = Console.ReadLine();

            // Se serve salvo la classe, e poi chiedo di includerla
            if (!string.IsNullOrWhiteSpace(classPath) && classPath.EndsWith(".cs"))
            {
                File.WriteAllText(classPath, classeSerializzataString);
            }

            // Chiedo all'utente dove vuole inserire il test
            string pathFileTest = null;
            do
            {
                Console.WriteLine("Please add the new dll/class to your test project if you didn't do it already.");
                Console.WriteLine("Specify the full path of the file where you need the initialization of this object: ");
                pathFileTest = Console.ReadLine();
                Console.WriteLine();
                // Ottengo il file di test dentro al quale si vuole salvare il nuovo metodo
            } while (string.IsNullOrWhiteSpace(pathFileTest) || !pathFileTest.EndsWith(".cs") || !File.Exists(pathFileTest));

            // Chiedo all'utente come vuole chiamare il test
            string nomeNuovoTest = null;
            do
            {
                Console.WriteLine("Specify the name of the method that will initialize this object: ");
                nomeNuovoTest = Console.ReadLine();
                Console.WriteLine();
            } while (string.IsNullOrWhiteSpace(nomeNuovoTest));

            // Rendo il nome corretto
            nomeNuovoTest = Util.RendiPrimaLetteraMaiuscola(nomeNuovoTest);

            // Stringa contenente il nuovo metodo
            var nuovoTest =
                this.CreaMetodoTestInizializzazione(oggettoSerializzato, classeSerializzataString,
                    nomeClasseAttuale, nomeNuovoTest, documentoCaricato);

            // Carico il file in cui salvare il nuovo metodo
            var fileTestString = File.ReadAllText(pathFileTest);

            // Cerco se esiste il metodo, se c'è lo sostituisco altrimenti lo inserisco
            var tipoOggettoAttuale = oggettoSerializzato.GetType();
            if (fileTestString.Contains($"public {tipoOggettoAttuale.Name} {nomeNuovoTest}()"))
            {
                // So che è presente, quindi per prima cosa mi salvo la posizione in cui va inserito
                var indiceInizioMetodo = fileTestString.IndexOf($"public {tipoOggettoAttuale.Name} {nomeNuovoTest}()");

                // Trovo la fine del metodo
                var indiceFineMetodo = 0;
                var indiceContatoreParentesi = 0;
                var carattereFineMetodo = fileTestString.Substring(indiceInizioMetodo)
                          .ToList()
                          .SkipWhile((c, contatore) => this.VerificaContinuaRicerca(c, contatore, ref indiceContatoreParentesi, ref indiceFineMetodo))
                          .FirstOrDefault();

                // Cancello poi il test precedente e il contenuto
                fileTestString = fileTestString.Remove(indiceInizioMetodo, indiceFineMetodo);

                // Infine inserisco il nuovo test
                fileTestString = fileTestString.Insert(indiceInizioMetodo, nuovoTest);
            }
            else
            {
                // Inserisco direttamente il nuovo test subito prima della fine del file, ma dentro
                // la classe
                fileTestString = fileTestString.Insert(fileTestString.Substring(0, fileTestString.LastIndexOf('}') - 1).LastIndexOf('}'), nuovoTest);
            }

            // Scrivo infine il nuovo file
            File.WriteAllText(pathFileTest, fileTestString);
            Console.WriteLine($"{nomeNuovoTest} Method has been correctly created.");

            Console.WriteLine("Press any button to exit.");
            Console.ReadKey();
        }

        #endregion Public Methods

        #region Private Methods

        /// <summary>
        /// Restituisce la stringa da inserire nel namespace quando serve 
        /// </summary>
        private string CalcolaNameSpace(string nameSpaceString)
        {
            if (!string.IsNullOrWhiteSpace(nameSpaceString))
            {
                return $", Namespace =\"{nameSpaceString}\"";
            }

            return "";
        }

        /// <summary>
        /// Permette di calcolare il tipo di proprietà dell'elemento figlio 
        /// </summary>
        private string CalcolaTipoProprietaElementoFiglio(bool elementoRipetutoAlmenoUnaVolta, string nomeElemento)
        {
            if (elementoRipetutoAlmenoUnaVolta)
            {
                return $"List<{Util.RendiPrimaLetteraMaiuscola(nomeElemento)}>";
            }
            else
            {
                return Util.RendiPrimaLetteraMaiuscola(nomeElemento);
            }
        }

        /// <summary>
        /// Carica i formati esistenti se necessario 
        /// </summary>
        private void CaricaListaFormatiDateTime()
        {
            this.ListaFormatiDateTimeStandard =
                new List<string>
                {
                    "d", "D", "f", "F", "g", "G",
                    "m", "M", "o", "O", "r", "R",
                    "s", "t", "T", "u", "U", "y",
                    "Y", "yyyy-MM-dd","yyyy-MM-dd HH:mm:ss",
                    "yyyy-MM-ddTHH:mm:ssZ",
                };

            if (this.AggiungiFormatoDateTimeSpeciale)
            {
                this.ListaFormatiDateTimeStandard.Add("MMyy");
            }
        }

        /// <summary>
        /// Permette di trovare il formato Decimal corretto 
        /// </summary>
        private void CaricaListaFormatiDecimal()
        {
            var listaFormatiStandard =
                new List<string>
                {
                    "F", "C",
                    // Scommentare in caso di bisogno, ma attenzione!
                    // Rallenta MOLTO il programma
                    //"E", "D",
                    //"G", "N",
                    //"P", "R","X"
                };
            var listaNumeriDopoVirgola = new List<int> { 0, 1, 2, 3, 4 };
            var listaFormatiConNumeri = listaFormatiStandard.SelectMany(f => listaNumeriDopoVirgola.Select(n => $"{f}{n}").ToList()).ToList();
            // Aggiungo poi la lista di formati normali
            listaFormatiConNumeri.AddRange(listaFormatiStandard);

            // Riordino
            listaFormatiConNumeri = listaFormatiConNumeri.OrderBy(f => f).ToList();

            var listaCulture = new List<CultureInfo>() {
                new CultureInfo("it-IT"),
                new CultureInfo("en-US"),
                new CultureInfo("fr-FR"),
                new CultureInfo("de-DE"),
                new CultureInfo("en-UK"),
            };

            this.ListaTupleFormatiCultureDecimal = listaFormatiConNumeri.SelectMany(formato => listaCulture.Select(c => new Tuple<string, CultureInfo>(formato, c))).ToList();
        }

        /// <summary>
        /// Permette di creare la stringa in cui si inizializza il test 
        /// </summary>
        private string CreaMetodoTestInizializzazione(object oggettoSerializzato, string classeSerializzataString,
            string nomeClasse, string nomeNuovoTest, XDocument documentoCaricato)
        {
            //Finalmente deserializzo la classe e la restituisco
            var gestoreInizializzazione = new TestCreatorManager();
            var stringaMetodoInizializzazioneOggetto = gestoreInizializzazione.CreaTest(oggettoSerializzato, nomeNuovoTest, documentoCaricato);

            var stringBuilderInizializzazioneOggetto =
                this.IndentaListaStringhe(stringaMetodoInizializzazioneOggetto.Split('\n'), 2)
                    .Replace("\n", Environment.NewLine);
            return stringBuilderInizializzazioneOggetto.ToString();
        }

        /// <summary>
        /// Crea la stringa contenente la classe serializzata 
        /// </summary>
        private string CreaStringaClasseSerializzata(XDocument documentoCaricato, Dictionary<string, ElementoValido>.ValueCollection listaElementiValidi)
        {
            // Sbuilder che contiene la classe principale
            var stringBuilderClasse = new StringBuilder();

            // Inizializzo le using solitamente necessarie
            stringBuilderClasse.AppendLine("using System;");
            stringBuilderClasse.AppendLine("using System.Globalization;");
            stringBuilderClasse.AppendLine("using System.Collections.Generic;");
            stringBuilderClasse.AppendLine("using System.Xml;");
            stringBuilderClasse.AppendLine("using System.Xml.Serialization;");
            stringBuilderClasse.AppendLine("");
            stringBuilderClasse.AppendLine($"namespace BELHXmlTool.{this._namespaceScelto}");
            stringBuilderClasse.AppendLine("{");

            // Arrivati qui ogni elementovalido è valido e ha le proprietà che dovrebbe avere.
            foreach (var elementoValido in listaElementiValidi)
            {
                var elementoSerializzato = this.SerializzaElementoValido(documentoCaricato, elementoValido);
                stringBuilderClasse.Append(elementoSerializzato);
            }

            stringBuilderClasse.AppendLine("}");

            // A questo punto intendo tutto splittando ad ogni \n a seconda di quanti {(+) e }(-) trovo
            stringBuilderClasse = this.IndentaListaStringhe(stringBuilderClasse.ToString().Split('\n'));

            // Ciclo di nuovo il loadedDocument per togliere i /n
            stringBuilderClasse = stringBuilderClasse.Replace("\n", Environment.NewLine);

            // Gestione del salvataggio
            return stringBuilderClasse.ToString();
        }

        /// <summary>
        /// Permette di gestire il formato aggiuntivo se l'utente lo desidera 
        /// </summary>
        private void ImpostaGestioneFormatoDateTimeAggiuntivo()
        {
            Console.WriteLine("Please press y if your xml contains DateTimes in the format 'MMyy', anything else to ignore it.");
            if (Console.ReadKey().KeyChar == 'y')
            {
                Console.WriteLine();
                Console.WriteLine("Please don't use integers that follow the same pattern anywhere in the xml: '{01-12}{00-99}'");
                this.AggiungiFormatoDateTimeSpeciale = true;
                Console.WriteLine("Press any button to continue");
                Console.ReadKey();
            }
            Console.WriteLine();
        }

        /// <summary>
        /// Permette di impostare il namespace all'utente 
        /// </summary>
        private void ImpostaNameSpaceGlobale()
        {
            do
            {
                Console.WriteLine("Please specify the NameSpace of the generated class:");
                // Non faccio nessun controllo, se l'utente sbaglia dovrà riavviare il programma
                this._namespaceScelto = Console.ReadLine();
                Console.WriteLine();
            } while (string.IsNullOrWhiteSpace(this._namespaceScelto));
        }

        /// <summary>
        /// Valorizza i valori necessari per poter continuare 
        /// </summary>
        private void ImpostaScelteGlobaliUtente()
        {
            this.ImpostaGestioneFormatoDateTimeAggiuntivo();
            this.ImpostaNameSpaceGlobale();
        }

        /// <summary>
        /// Permette di indentare la lista di stringhe passata nel modo corretto 
        /// </summary>
        private StringBuilder IndentaListaStringhe(IEnumerable<string> listaStringheDaIndentare, int counter = 0)
        {
            var asd = new StringBuilder();
            foreach (var stringa in listaStringheDaIndentare)
            {
                if (stringa.StartsWith("}"))
                {
                    counter--;
                }

                asd.Append(Util.Indenta(counter))
                   .Append(stringa);

                if (stringa.StartsWith("{"))
                {
                    counter++;
                }
            }

            return asd;
        }

        /// <summary>
        /// Permette di serializzare l'elemento 
        /// </summary>
        private string SerializzaElementoValido(XDocument loadedDocument, ElementoValido elementoValido)
        {
            var sbElemento = new StringBuilder();
            var primoElementoTipoAttuale = elementoValido.ListaElementiTipologiaAttuale[0];
            var nomeClasse = Util.RendiPrimaLetteraMaiuscola(primoElementoTipoAttuale.Name.LocalName);
            sbElemento.AppendLine($"[XmlRoot(ElementName=\"{primoElementoTipoAttuale.Name.LocalName}\"{this.CalcolaNameSpace(primoElementoTipoAttuale.Name.NamespaceName)})]");
            sbElemento.AppendLine($"public class {nomeClasse}");
            sbElemento.AppendLine("{");

            foreach (var proprietaAttuale in elementoValido.ListaElementiProprieta)
            {
                // Non può essere null, li ho già esclusi
                var primoElementoProprietaAttuale = proprietaAttuale.ListaElementiTipologiaAttuale[0];

                // Provo a capire di che tipo di property si tratta
                var tipoProprieta = this.TrovaTipoProprietaElementoProprieta(loadedDocument, primoElementoProprietaAttuale.Name,
                    proprietaAttuale.ElementoRipetutoAlmenoUnaVolta, out var nomeProprieta, out var tipoDateTime, out var formatoDateTime);

                // Continuo con le property aggiuntive necessarie e con il costruttore
                if (tipoDateTime)
                {
                    // Gestione particolare causata dal fatto che serializzando si perde il formato
                    // del datetime originale che invece deve essere preservato La proprieta datetime
                    // o lista di dateTime che andrò a modificare nei test
                    sbElemento.AppendLine("[XmlIgnore]");
                    sbElemento.AppendLine($"public {tipoProprieta} {nomeProprieta} {{ get; set; }}");

                    // Nome e namespace dell'elemento corretto
                    sbElemento.AppendLine($"[XmlElement(ElementName = \"{primoElementoProprietaAttuale.Name.LocalName}\"" +
                      $"{this.CalcolaNameSpace(primoElementoProprietaAttuale.Name.NamespaceName)}" +
                      // $",Order = {_contatoreFiglio}" +
                      $")]");

                    // Proprietà
                    var tipoProprietaString = proprietaAttuale.ElementoRipetutoAlmenoUnaVolta ? "List<string>" : "string";
                    sbElemento.AppendLine($"public {tipoProprietaString} {nomeProprieta}Serializzabile");
                    sbElemento.AppendLine("{");

                    // Esempio: [XmlIgnore] public DateTime SomeDate { get; set; }

                    // [XmlElement("SomeDate")] public string SomeDateString { get { return
                    // this.SomeDate.ToString("yyyy-MM-dd HH:mm:ss"); } set { this.SomeDate =
                    // DateTime.Parse(value); } }

                    // Gestione particolare formato specifico
                    var toUniversal = string.Empty;
                    if (formatoDateTime.EndsWith("z", true, CultureInfo.InvariantCulture))
                    {
                        toUniversal = ".ToUniversalTime()";
                    }

                    if (proprietaAttuale.ElementoRipetutoAlmenoUnaVolta)
                    {
                        // La stringa relativa renderizzata nel formato corretto
                        sbElemento.AppendLine($"\tget {{ return this.{nomeProprieta}.Where(v => v.HasValue).Select(v => v.Value{toUniversal}.ToString(\"{formatoDateTime}\")).ToList(); }}");
                        sbElemento.AppendLine($"\tset {{ this.{nomeProprieta} = value.Select(v => DateTime.ParseExact(v,\"{formatoDateTime}\",CultureInfo.InvariantCulture)).ToList();}}");
                    }
                    else
                    {
                        // La stringa relativa renderizzata nel formato corretto
                        sbElemento.AppendLine($"\tget {{ return this.{nomeProprieta}.Value{toUniversal}.ToString(\"{formatoDateTime}\"); }}");
                        sbElemento.AppendLine($"\tset {{ this.{nomeProprieta} = DateTime.ParseExact(value,\"{formatoDateTime}\",CultureInfo.InvariantCulture); }}");
                    }

                    sbElemento.AppendLine("}");
                }
                else
                {
                    // Nome e namespace dell'elemento corretto
                    sbElemento.AppendLine($"[XmlElement(ElementName = \"{primoElementoProprietaAttuale.Name.LocalName}\"" +
                      $"{this.CalcolaNameSpace(primoElementoProprietaAttuale.Name.NamespaceName)}" +
                      // $",Order = {_contatoreFiglio}" +
                      $")]");

                    sbElemento.AppendLine($"public {tipoProprieta} {nomeProprieta} {{ get; set; }}");
                }

                string condizioneAggiuntivaSerializzazione = null;
                if (proprietaAttuale.ElementoRipetutoAlmenoUnaVolta)
                {
                    condizioneAggiuntivaSerializzazione = $" && this.{ nomeProprieta }.Count > 0";
                }

                // Facciamo in modo che non venga visto nei test o quando costruiamo l'oggetto
                sbElemento.AppendLine("[System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]");
                // Fa in modo che solo ciò che è valorizzato venga serializzato
                if (tipoDateTime)
                {
                    sbElemento.AppendLine($"public bool ShouldSerialize{nomeProprieta}Serializzabile() {{ return this.{nomeProprieta} != null{condizioneAggiuntivaSerializzazione}; }}");
                }
                else
                {
                    sbElemento.AppendLine($"public bool ShouldSerialize{nomeProprieta}() {{ return this.{nomeProprieta} != null{condizioneAggiuntivaSerializzazione}; }}");
                }

                // Incremento il contatore del numero di figli che mi permette di mantenere la struttura
                this._contatoreFiglio++;
            }

            // Elementi elemento valido attuale Prendo prima i nomi degli elementi diversi presenti
            // Rimuovo quelli che ho già trattato come proprieta
            var listaMassimaNomiElementiFigli = elementoValido.ListaElementiTipologiaAttuale
                .SelectMany(e => e.Elements()
                                 .Where(ef => !elementoValido.ListaElementiProprieta
                                              .Any(evp => evp.ListaElementiTipologiaAttuale[0].Name == ef.Name))
                            .Select(ef => ef.Name))
                .Distinct().ToList();

            foreach (var nomeElemento in listaMassimaNomiElementiFigli)
            {
                // Controllo prima di tutto se ci sono più elementi di questo tipo all'interno
                // dell'elemento attuale o negli altri
                var elementoRipetutoAlmenoUnaVolta = elementoValido.ListaElementiTipologiaAttuale.Any(e => e.Elements(nomeElemento).Count() > 1);
                var tipoProprieta = this.CalcolaTipoProprietaElementoFiglio(elementoRipetutoAlmenoUnaVolta, nomeElemento.LocalName);

                // Aggiungo Elemento per distinguerli dalle proprieta Scrivo quindi il nome dell'elemento
                string nomeProprieta = null;
                string inizializzazioneProprieta = null;
                if (elementoRipetutoAlmenoUnaVolta)
                {
                    nomeProprieta = "Lista";
                    inizializzazioneProprieta = $"= new {tipoProprieta}();";
                }

                nomeProprieta += "Elemento" + Util.RendiPrimaLetteraMaiuscola(nomeElemento.LocalName);

                // Scrivo la proprieta
                sbElemento.AppendLine($"[XmlElement(ElementName=\"{nomeElemento.LocalName}\"" +
                    $"{this.CalcolaNameSpace(nomeElemento.NamespaceName)}" +
                    // $",Order = {_contatoreFiglio}" +
                    $")]");

                sbElemento.AppendLine($"public {tipoProprieta} {nomeProprieta} {{ get; set; }} {inizializzazioneProprieta}");

                // Decide come scrivere cosa renderizzare e cosa no. non è mai null perchè
                // inizializzo sempre le liste, ma può avere count = 0
                string condizioneAggiuntivaSerializzazione = null;
                if (elementoRipetutoAlmenoUnaVolta)
                {
                    condizioneAggiuntivaSerializzazione = $" && { nomeProprieta }.Count > 0";
                }

                // Facciamo in modo che non venga visto nei test o quando costruiamo l'oggetto
                sbElemento.AppendLine("[System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]");
                // Fa in modo che solo ciò che è valorizzato venga serializzato
                sbElemento.AppendLine($"public bool ShouldSerialize{nomeProprieta}() {{ return { nomeProprieta } != null{condizioneAggiuntivaSerializzazione}; }}");

                // Incremento il contatore
                this._contatoreFiglio++;
            }

            // Attributi elemento valido attuale Prendo ora la lista degli attributi
            var listaMassimaNomiAttributi = elementoValido.ListaElementiTipologiaAttuale.SelectMany(e => e.Attributes().Select(a => a.Name)).Distinct().ToList();
            var sbNamespaces = new StringBuilder();
            foreach (var nomeAttributo in listaMassimaNomiAttributi)
            {
                // Prendo tutti gli attributi di quel tipo
                var listaAttributoAttuale = elementoValido.ListaElementiTipologiaAttuale.SelectMany(e => e.Attributes(nomeAttributo)).ToList();
                var nomeProprietaAttributo = Util.RendiPrimaLetteraMaiuscola(nomeAttributo.LocalName);

                // Gesione particolare per i namespaces del primo elemento
                if (listaAttributoAttuale.All(a => a.IsNamespaceDeclaration))
                {
                    string nome = null;
                    if (nomeAttributo.LocalName.Equals("xmlns", StringComparison.Ordinal))
                    {
                        nome = "String.Empty";
                    }
                    else
                    {
                        nome = "\"" + nomeAttributo.LocalName + "\"";
                    }

                    sbNamespaces.AppendLine($"new XmlQualifiedName({nome}, \"{listaAttributoAttuale.First().Value}\"),");
                    continue;
                }

                // Gestione particolare causata dal fatto che serializzando si perde il formato del
                // datetime originale che invece deve essere preservato
                var tipoDateTime = false;
                var tipoDecimal = false;
                string formatoDateTime = null;
                Tuple<string, CultureInfo> tuplaFormatoDecimal = null;

                string tipoProprieta = null;
                if (tipoProprieta == null)
                {
                    formatoDateTime = this.TrovaFormatoDateTime(listaAttributoAttuale
                        .Select(a => (string)a)
                            .Where(v => !string.IsNullOrEmpty(v))
                        .ToList()
                    );

                    if (formatoDateTime != null)
                    {
                        tipoDateTime = true;
                    }
                }

                if (tipoProprieta == null && !tipoDateTime)
                {
                    tuplaFormatoDecimal = this.TrovaFormatoDecimal(listaAttributoAttuale
                           .Select(a => (string)a)
                               .Where(v => !string.IsNullOrEmpty(v))
                           .ToList()
                   );

                    if (tuplaFormatoDecimal != null)
                    {
                        tipoDecimal = true;
                    }
                }

                if (tipoProprieta == null && !tipoDateTime && !tipoDecimal)
                {
                    tipoProprieta = this.TrovaTipoProprietaAttributo(listaAttributoAttuale);
                }

                // Se è arrivato qui a null
                if (tipoProprieta == null && !tipoDateTime && !tipoDecimal)
                {
                    tipoProprieta = "string";
                }

                var serializzabileNecessario = false;
                // Scrivo quindi il nome dell'attributo
                if (tipoDateTime)
                {
                    // Gesitone particolare datetime
                    var toUniversal = string.Empty;
                    if (formatoDateTime.EndsWith("z", true, CultureInfo.InvariantCulture))
                    {
                        toUniversal = ".ToUniversalTime()";
                    }

                    // La proprieta datetime che andrò a modificare nei test
                    sbElemento.AppendLine("[XmlIgnore]");
                    sbElemento.AppendLine($"public DateTime? {nomeProprietaAttributo} {{ get; set; }}");

                    // La stringa relativa renderizzata nel formato corretto Facciamo in modo che non
                    // venga visto nei test o quando costruiamo l'oggetto
                    sbElemento.AppendLine("[System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]");
                    sbElemento.AppendLine($"[XmlAttribute(AttributeName=\"{nomeAttributo.LocalName}\")]");
                    sbElemento.AppendLine($"public string {nomeProprietaAttributo}Serializzabile");
                    sbElemento.AppendLine("{");
                    sbElemento.AppendLine($"\tget {{ return this.{nomeProprietaAttributo}.Value{toUniversal}.ToString(\"{formatoDateTime}\"); }}");
                    sbElemento.AppendLine($"\tset {{ this.{nomeProprietaAttributo} = DateTime.ParseExact(value,\"{formatoDateTime}\",CultureInfo.InvariantCulture); }}");
                    sbElemento.AppendLine("}");

                    // Facciamo in modo che non venga visto nei test o quando costruiamo l'oggetto
                    sbElemento.AppendLine("[System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]");
                    sbElemento.AppendLine("[XmlIgnore]");
                    sbElemento.AppendLine($"public bool {nomeProprietaAttributo}SerializzabileSpecified {{ get {{ return this.{nomeProprietaAttributo}.HasValue; }} }}");

                    // Esempio:
                    //[XmlIgnore]
                    // public DateTime SomeDate { get; set; }

                    // [XmlElement("SomeDate")] public string SomeDateString { get { return
                    // this.SomeDate.ToString("yyyy-MM-dd HH:mm:ss"); } set { this.SomeDate =
                    // DateTime.Parse(value); } }

                    // Specified che stabilisce quando serializzare e quando no [XmlIgnore] public
                    // bool SomeDateStringSpecified { get { return Age >= 0; } }
                }
                else if (tipoDecimal)
                {
                    // La proprieta datetime che andrò a modificare nei test
                    sbElemento.AppendLine("[XmlIgnore]");
                    sbElemento.AppendLine($"public decimal? {nomeProprietaAttributo} {{ get; set; }}");

                    // La stringa relativa renderizzata nel formato corretto Facciamo in modo che non
                    // venga visto nei test o quando costruiamo l'oggetto
                    sbElemento.AppendLine("[System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]");
                    sbElemento.AppendLine($"[XmlAttribute(AttributeName=\"{nomeAttributo.LocalName}\")]");
                    sbElemento.AppendLine($"public string {nomeProprietaAttributo}Serializzabile");
                    sbElemento.AppendLine("{");
                    sbElemento.AppendLine($"\tget {{ return this.{nomeProprietaAttributo}.Value.ToString(\"{tuplaFormatoDecimal.Item1}\", new CultureInfo(\"{tuplaFormatoDecimal.Item2.Name}\")); }}");
                    sbElemento.AppendLine($"\tset {{ this.{nomeProprietaAttributo} = decimal.Parse(value, CultureInfo.InvariantCulture); }}");
                    sbElemento.AppendLine("}");

                    // Facciamo in modo che non venga visto nei test o quando costruiamo l'oggetto
                    sbElemento.AppendLine("[System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]");
                    sbElemento.AppendLine("[XmlIgnore]");
                    sbElemento.AppendLine($"public bool {nomeProprietaAttributo}SerializzabileSpecified {{ get {{ return this.{nomeProprietaAttributo}.HasValue; }} }}");
                }
                else
                {
                    // Proprietà fantoccio da serializzare, che è sempre valorizzata quando serializzata
                    string condizionePerSerializzare;
                    if (tipoProprieta == "string")
                    {
                        sbElemento.AppendLine($"[XmlAttribute(AttributeName=\"{nomeAttributo.LocalName}\")]");
                        sbElemento.AppendLine($"public {tipoProprieta} {nomeProprietaAttributo} {{ get; set; }}");
                        condizionePerSerializzare = " != null";
                    }
                    else
                    {
                        serializzabileNecessario = true;
                        // Proprieta effettiva non sempre serializzabile
                        sbElemento.AppendLine("[XmlIgnore]");
                        sbElemento.AppendLine($"public {tipoProprieta} {nomeProprietaAttributo} {{ get; set; }}");
                        // Facciamo in modo che non venga visto nei test o quando costruiamo l'oggetto
                        sbElemento.AppendLine("[System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]");
                        sbElemento.AppendLine($"[XmlAttribute(AttributeName=\"{nomeAttributo.LocalName}\")]");
                        sbElemento.AppendLine($"public {tipoProprieta.Substring(0, tipoProprieta.Length - 1)} {nomeProprietaAttributo}Serializzabile {{ get => this.{nomeProprietaAttributo}.Value; set => this.{nomeProprietaAttributo} = value; }}");
                        condizionePerSerializzare = ".HasValue";
                    }

                    // Facciamo in modo che non venga visto nei test o quando costruiamo l'oggetto
                    sbElemento.AppendLine("[System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]");
                    // Specified che stabilisce quando serializzare e quando no [XmlIgnore] public
                    // bool AgeSpecified { get { return Age >= 0; } }
                    sbElemento.AppendLine("[XmlIgnore]");
                    var serializzabile = serializzabileNecessario ? "Serializzabile" : null;
                    sbElemento.AppendLine($"public bool {nomeProprietaAttributo}{serializzabile}Specified {{ get {{ return this.{nomeProprietaAttributo}{condizionePerSerializzare}; }} }}");
                }
            }

            // Solo se necessario gestisco i namespaces presenti
            if (sbNamespaces.Length > 0)
            {
                //                [XmlNamespaceDeclarations]
                //public XmlSerializerNamespaces Namespaces
                //{
                //    get { return this._namespaces; }
                //}
                //            this._namespaces = new XmlSerializerNamespaces(new XmlQualifiedName[] {
                //    new XmlQualifiedName(string.Empty, "urn:Abracadabra")
                //});
                sbElemento.AppendLine("[System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]");
                sbElemento.AppendLine("[XmlNamespaceDeclarations]");
                sbElemento.AppendLine("public XmlSerializerNamespaces Namespaces");
                sbElemento.AppendLine("{");

                sbElemento.AppendLine("get");
                sbElemento.AppendLine("{");
                sbElemento.AppendLine("return new XmlSerializerNamespaces(new XmlQualifiedName[]");
                sbElemento.AppendLine("{");
                sbElemento.Append(sbNamespaces.ToString());
                sbElemento.AppendLine("});");
                sbElemento.AppendLine("}");

                sbElemento.AppendLine("set");
                sbElemento.AppendLine("{");
                sbElemento.AppendLine("}");

                sbElemento.AppendLine("}");
            }

            // Scrivo prima di chiudere l'elemento il suo valore per l'innertext, se necessario
            if (elementoValido.ListaElementiTipologiaAttuale.Any(e => e.Nodes().Any(n => n.NodeType == System.Xml.XmlNodeType.Text && !string.IsNullOrEmpty(e.Value))))
            {
                //[XmlText]
                //public string ValoreElemento { get; set; }
                sbElemento.AppendLine("[XmlText]");
                sbElemento.AppendLine($"public string Valore{nomeClasse} {{ get; set; }}");
            }

            sbElemento.AppendLine("}");

            return sbElemento.ToString();
        }

        /// <summary>
        /// Permette di trovare il formato DateTime corretto 
        /// </summary>
        private string TrovaFormatoDateTime(List<string> listaValori)
        {
            if (this.ListaFormatiDateTimeStandard == null)
            {
                this.CaricaListaFormatiDateTime();
            }

            foreach (var formato in this.ListaFormatiDateTimeStandard)
            {
                if (listaValori.All(v => DateTime.TryParseExact(v, formato, CultureInfo.InvariantCulture, DateTimeStyles.AssumeUniversal, out var data)))
                {
                    return formato;
                }
            }

            return null;
        }

        /// <summary>
        /// Permette di trovare il formato decimal corretto 
        /// </summary>
        private Tuple<string, CultureInfo> TrovaFormatoDecimal(List<string> listaValori)
        {
            if (this.ListaTupleFormatiCultureDecimal == null)
            {
                this.CaricaListaFormatiDecimal();
            }

            // Converto i valori una volta sola
            var listaTuplaValoriDecimali = listaValori.Select(v =>
                    decimal.TryParse(v, NumberStyles.Number, CultureInfo.InvariantCulture, out var valoreConvertito) ? new Tuple<string, decimal>(v, valoreConvertito) : null)
                    .ToList();

            // Se anche solo uno dei valori non è un decimal, questo elemento non è sempre
            // associabile ad un decimal
            if (listaTuplaValoriDecimali.Any(t => t == null))
            {
                return null;
            }

            foreach (var tuplaFormato in this.ListaTupleFormatiCultureDecimal)
            {
                if (listaTuplaValoriDecimali.Count > 0 && listaTuplaValoriDecimali.All(tuplaValore => this.TrovaFormatoDecimalCorretto(tuplaValore, tuplaFormato)))
                {
                    return tuplaFormato;
                }
            }

            return null;
        }

        /// <summary>
        /// Permette di trovare il formato decimal corretto 
        /// </summary>
        private bool TrovaFormatoDecimalCorretto(Tuple<string, decimal> tuplaValore, Tuple<string, CultureInfo> tuplaFormato)
        {
            try
            {
                if (tuplaValore.Item2.ToString(tuplaFormato.Item1, tuplaFormato.Item2) == tuplaValore.Item1)
                {
                    return true;
                }
            }
            catch
            { }

            return false;
        }

        /// <summary>
        /// Trova il tipo di property dell'attributo attuale 
        /// </summary>
        private string TrovaTipoProprietaAttributo(List<XAttribute> listaAttributo)
        {
            string tipoProprieta = null;

            // Se almeno uno ha un valore
            if (listaAttributo.Any(e => !string.IsNullOrEmpty(e.Value)))
            {
                // Se è arrivato qui allora riprova gli elementi precedenti, in fila, ma con dei nullable
                if (tipoProprieta == null)
                {
                    tipoProprieta = listaAttributo.All(e => string.IsNullOrEmpty(e.Value) || bool.TryParse(e.Value, out var valoreBool)) ? "bool?" : null;
                }
                if (tipoProprieta == null)
                {
                    tipoProprieta = listaAttributo.All(e => string.IsNullOrEmpty(e.Value) || short.TryParse(e.Value, out var valore)) ? "short?" : null;
                }
                if (tipoProprieta == null)
                {
                    tipoProprieta = listaAttributo.All(e => string.IsNullOrEmpty(e.Value) || int.TryParse(e.Value, out var valore)) ? "int?" : null;
                }
                // Non cerco per datetime e decimal perchè sono già gestiti fuori
            }

            return tipoProprieta;
        }

        /// <summary>
        /// Trova il tipo di proprieta 
        /// </summary>
        private string TrovaTipoProprietaElementoProprieta(XDocument loadedDocument, XName nomeProprietaAttuale,
            bool elementoRipetutoAlmenoUnaVolta, out string nomeProprieta, out bool tipoDateTime, out string formatoDateTime)
        {
            string tipoProprieta = null;
            var nomeProprietaNormalizzato = Util.RendiPrimaLetteraMaiuscola(nomeProprietaAttuale.LocalName);
            tipoDateTime = false;
            formatoDateTime = null;

            // Se non ha attributi in nessun caso lo trasformo direttamente nella relativa proprieta
            var listaMassimaElementoAttuale = loadedDocument.Descendants(nomeProprietaAttuale);
            if (listaMassimaElementoAttuale.All(e => e.Attributes().Count() == 0))
            {
                // Se almeno uno ha un valore
                if (listaMassimaElementoAttuale.Any(e => !string.IsNullOrEmpty(e.Value)))
                {
                    // Se è arrivato qui allora riprova gli elementi precedenti, in fila, ma con dei nullable
                    if (tipoProprieta == null)
                    {
                        tipoProprieta = listaMassimaElementoAttuale.All(e => string.IsNullOrEmpty(e.Value) || bool.TryParse(e.Value, out var valoreBool)) ? "bool?" : null;
                    }
                    if (tipoProprieta == null)
                    {
                        tipoProprieta = listaMassimaElementoAttuale.All(e => string.IsNullOrEmpty(e.Value) || short.TryParse(e.Value, out var valore)) ? "short?" : null;
                    }
                    if (tipoProprieta == null)
                    {
                        tipoProprieta = listaMassimaElementoAttuale.All(e => string.IsNullOrEmpty(e.Value) || int.TryParse(e.Value, out var valore)) ? "int?" : null;
                    }
                    if (tipoProprieta == null)
                    {
                        tipoProprieta = listaMassimaElementoAttuale.All(e => string.IsNullOrEmpty(e.Value) || decimal.TryParse(e.Value, out var valore)) ? "decimal?" : null;
                    }
                    if (tipoProprieta == null && (listaMassimaElementoAttuale.All(e => string.IsNullOrEmpty(e.Value) || DateTime.TryParse(e.Value, out DateTime valore))))
                    {
                        tipoProprieta = "DateTime?";
                        tipoDateTime = true;
                        formatoDateTime = this.TrovaFormatoDateTime(
                            listaMassimaElementoAttuale
                                .Where(v => !string.IsNullOrEmpty(v.Value))
                                    .Select(v => v.Value)
                                    .ToList()
                        );

                        //Se non ci sono riuscito lancio eccezione
                        if (formatoDateTime == null)
                        {
                            throw new Exception($"Error: it wasn't possible to find a reliable datetime format pattern for the attribute {nomeProprietaAttuale.LocalName}");
                        }
                    }
                }

                // Se è ancora null è una stringa
                if (tipoProprieta == null)
                {
                    tipoProprieta = "string";
                }
            }
            else
            {
                // Se invece ha figli
                tipoProprieta = nomeProprietaNormalizzato;
            }

            if (elementoRipetutoAlmenoUnaVolta)
            {
                tipoProprieta = $"List<{tipoProprieta}>";
                nomeProprieta = $"Lista{nomeProprietaNormalizzato}";
            }
            else
            {
                nomeProprieta = nomeProprietaNormalizzato;
            }

            return tipoProprieta;
        }

        /// <summary>
        /// Permette di trovare la posizione della fine del metodo 
        /// </summary>
        private bool VerificaContinuaRicerca(char c, int contatore, ref int indiceContatoreParentesi, ref int indiceFineMetodo)
        {
            switch (c)
            {
                case '{':
                    indiceContatoreParentesi++;
                    break;

                case '}':
                    indiceContatoreParentesi--;
                    break;

                default:
                    // Di default continua e basta
                    return true;
            }

            // Se arriva qui il char era una parentesi
            if (indiceContatoreParentesi == 0)
            {
                // Aggiungo uno per la parentesi attuale
                indiceFineMetodo = contatore + 1;
                return false;
            }
            else
            {
                return true;
            }
        }

        #endregion Private Methods
    }
}