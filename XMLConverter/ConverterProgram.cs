using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text;
using System.Linq;
using System.Xml.Linq;
using System.Xml.Serialization;
using System.CodeDom.Compiler;
using System.Reflection;
using Microsoft.CodeDom.Providers.DotNetCompilerPlatform;
using WebApplications.Utilities;
using System.Xml;

namespace XMLConverter
{
    public class ConverterProgram
    {
        /// <summary>
        /// Tiene in memoria tutti i formati esistenti una volta caricati
        /// </summary>
        private static List<string> _listaFormatiDateTimeStandard { get; set; }
        private static List<Tuple<string, CultureInfo>> _listaTupleFormatiCultureDecimal { get; set; }

        /// <summary>
        /// Contatore Elementi figli
        /// </summary>
        private int _contatoreFiglio = 1;

        /// <summary>
        /// Il formato MMyy può causare problemi quando si hanno nel file dei numeri tipo "1234"
        /// che NON sono date delle carte di credito
        /// </summary>
        private static bool _aggiungiFormatoDateTimeSpeciale { get; set; } = false;

        /// <summary>
        /// Faccio scegliere il namespace perchè altrimenti si creerebbero conflitti se si importassero due DLL generate con lo stesso
        /// Con nomi degli elementi uguali ma implementazioni diverse
        /// </summary>
        private static string _namespaceScelto { get; set; }

        private static void Main()
        {
            // Chiedo il path completo dell'xml all'utilizzatore
            string pathXmlFile = null;
            XDocument documentoCaricato = null;
            do
            {
                Console.WriteLine("Specify the complete path of the xml that you want to convert and initialize:");
                pathXmlFile = Console.ReadLine();
                // Mi accerto che esista il path specificato e che sia un xml valido
                if (!String.IsNullOrWhiteSpace(pathXmlFile) || pathXmlFile.EndsWith(".xml") || File.Exists(pathXmlFile))
                {
                    try
                    {
                        documentoCaricato = XDocument.Load(pathXmlFile);
                    }
                    catch
                    {
                    }
                }

                // Se è null per una qualche ragione il percorso o il file contenuto nel percorso non erano validi
                if (documentoCaricato == null)
                {
                    Console.WriteLine(pathXmlFile + " Is not a valid xml file path.");
                }
            } while (documentoCaricato == null);


            // Chiedo subito se il cliente vuole che io tratti anche il formato mmYY oppure no
            ConverterProgram.ImpostaScelteGlobaliUtente();

            // N.B.
            // Per ciascun elemento controlla:
            // Se ha attributi converte l'elemento in classe
            // Se ha solo un valore diventa una proprietà della classe formata dal primo oggetto padre che lo contiene
            // Se compare più di una volta nel primo elemento padre che si trova già presente nel dizionario, 
            // si deve fare una lista di questo in quel padre
            Dictionary<string, ElementoValido> dizionarioElementiValidi = new Dictionary<string, ElementoValido>();
            Dictionary<string, ElementoValido> dizionarioElementiProprieta = new Dictionary<string, ElementoValido>();
            var listaElementi = documentoCaricato.Descendants();

            string nomeClasseAttuale = ConverterProgram.RendiPrimaLetteraMaiuscola(listaElementi.First().Name.LocalName);
            string nomeFileAttuale = nomeClasseAttuale + ".cs";

            foreach (var elemento in listaElementi)
            {
                // Controlla che non sia nel dizionario
                if (!dizionarioElementiValidi.TryGetValue(elemento.Name.LocalName, out ElementoValido elementoValidoPresente))
                {
                    // Se non è presente lo aggiunge, se va aggiunto, ossia se ha più di un figlio oppure se ha degli attributi
                    if (elemento.Elements().Count() > 0 || elemento.Attributes().Count() > 0)
                    {
                        dizionarioElementiValidi.Add(elemento.Name.LocalName, new ElementoValido(elemento));
                    }
                    else
                    {
                        // Se siamo qui non è presente e non va aggiunto, che significa che deve essere una proprietà 
                        // del primo elemento padre che si incontra nel dizionario.
                        // Questo tipo di verifica va però fatto alla fine del parsing di tutti gli elementi validi per il dizionario,
                        // quindi lo mettiamo in un altro dizionario per ora.
                        // Se è già presente lo aggiungiamo a se stesso, altrimenti al dizionario, ci servirà a capire se è una lista o meno
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

            // Per comodità e chiarezza
            var listaElementiProprieta = dizionarioElementiProprieta.Values;
            var listaElementiValidi = dizionarioElementiValidi.Values;

            foreach (var elementoProprieta in listaElementiProprieta)
            {
                foreach (var elementoValido in listaElementiValidi)
                {
                    // Se almeno uno di loro è contenuto in almeno uno degli altri lo aggiunge alle proprietà del rispettivo
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
            var classeSerializzataString = ConverterProgram.CreaStringaClasseSerializzata(documentoCaricato, listaElementiValidi);

            // Parametri Compilatore
            CompilerParameters compilerParams = new CompilerParameters();

            // Voglio la libreria dll, non l'exe
            compilerParams.GenerateExecutable = false;

            // Riferimenti
            compilerParams.ReferencedAssemblies.Add(@"System.dll");
            compilerParams.ReferencedAssemblies.Add(@"mscorlib.dll");
            compilerParams.ReferencedAssemblies.Add(@"sysglobl.dll");
            compilerParams.ReferencedAssemblies.Add(@"System.Net.dll");
            compilerParams.ReferencedAssemblies.Add(@"System.Core.dll");
            compilerParams.ReferencedAssemblies.Add(@"C:\Program Files (x86)\Reference Assemblies\Microsoft\Framework\.NETFramework\v4.6.1\System.Xml.dll");
            compilerParams.ReferencedAssemblies.Add(@"C:\Program Files (x86)\Reference Assemblies\Microsoft\Framework\.NETFramework\v4.6.1\System.Xml.Linq.dll");
            compilerParams.ReferencedAssemblies.Add(@"C:\Program Files (x86)\Reference Assemblies\Microsoft\Framework\.NETFramework\v4.6.1\System.Data.dll");
            compilerParams.ReferencedAssemblies.Add(@"C:\Program Files (x86)\Reference Assemblies\Microsoft\Framework\.NETFramework\v4.6.1\System.Data.DataSetExtensions.dll");
            // N.B. Scommentare se servono le info per il debug
            // compilerParams.IncludeDebugInformation = true;

            // Chiedo all'utilizzatore se vuole che venga creata la dll.
            // Se è così gli chiedo di specificare il path dove vuole che venga creata
            string dllPath;
            Console.WriteLine("Specify the complete path of the new dll if you want to create it or just press Enter to skip: ");
            dllPath = Console.ReadLine();

            // Compilatore
            CodeDomProvider cdp = new Microsoft.CodeDom.Providers.DotNetCompilerPlatform.CSharpCodeProvider();

            // Se serve salvo la DLL, e poi chiedo di includerla
            if (!String.IsNullOrWhiteSpace(dllPath))
            {
                // Voglio Che salvi l'assembly, non che lo tenga in memoria
                compilerParams.GenerateInMemory = false;

                // Dico dove salvare e quale nome debba avere la dll
                compilerParams.CompilerOptions = " -out:" + dllPath;

                // Compila per salvare la dll, non ho trovato un modo migliore per salvarla
                var risultatoCompilazioneSalvataggio = cdp.CompileAssemblyFromSource(compilerParams, classeSerializzataString);

                // Setta di nuovo le opzioni a null
                compilerParams.CompilerOptions = null;
            }

            // Arrivato qui Voglio che non venga salvato l'assembly ma solo creato un temporaneo
            compilerParams.GenerateInMemory = true;

            // Prova a compilare il file creato
            CompilerResults compilerResults = cdp.CompileAssemblyFromSource(compilerParams, classeSerializzataString);

            // Prende finalmente l'assembly
            Assembly assembly = compilerResults.CompiledAssembly;

            // Crea un istanza dell'oggetto, chiaramente aggiungo il NameSpace che so già 
            object oggettoSerializzato = assembly.CreateInstance($"BELHXmlTool.{_namespaceScelto}.{nomeClasseAttuale}");
            var tipoOggettoAttuale = oggettoSerializzato.GetType();

            // Inizializza il serializer con il tipo dell'oggetto caricato
            XmlSerializer serializer = new XmlSerializer(tipoOggettoAttuale);

            // Carico il loadedDocument in un memoryStream che può essere deserializzato e ne resetto la posizione per poterlo leggere
            var ms = new MemoryStream();
            documentoCaricato.Save(ms);
            ms.Position = 0;
            oggettoSerializzato = serializer.Deserialize(ms);

            ConverterProgram.TestaUguaglianzaDocumentoOggetto(documentoCaricato, oggettoSerializzato);

            // Chiedo all'utente dove vuole inserire il test
            string pathFileTest = null;
            do
            {
                Console.WriteLine("Please add the created dll to your test project before you proceed.");
                Console.WriteLine("Specify the full path of the file where you need the new test: ");
                pathFileTest = Console.ReadLine();
                // Ottengo il file di test dentro al quale si vuole salvare il nuovo metodo
            } while (String.IsNullOrWhiteSpace(pathFileTest) || !pathFileTest.EndsWith(".cs") || !File.Exists(pathFileTest));

            // Chiedo all'utente come vuole chiamare il test
            string nomeNuovoTest = null;
            do
            {
                Console.WriteLine("Specify the name of the new test Method: ");
                nomeNuovoTest = Console.ReadLine();
            } while (String.IsNullOrWhiteSpace(nomeNuovoTest));

            // Rendo il nome corretto
            nomeNuovoTest = ConverterProgram.RendiPrimaLetteraMaiuscola(nomeNuovoTest);

            // Stringa contenente il nuovo metodo
            string nuovoTest =
                ConverterProgram.CreaMetodoTestInizializzazione(oggettoSerializzato, classeSerializzataString,
                    nomeClasseAttuale, nomeNuovoTest, documentoCaricato);

            // Carico il file in cui salvare il nuovo metodo
            var fileTestString = File.ReadAllText(pathFileTest);

            // Cerco se esiste il metodo, se c'è lo sostituisco altrimenti lo inserisco
            if (fileTestString.Contains($"public {tipoOggettoAttuale.Name} {nomeNuovoTest}()"))
            {
                // So che è presente, quindi per prima cosa mi salvo la posizione in cui va inserito
                int indiceInizioMetodo = fileTestString.IndexOf($"public {tipoOggettoAttuale.Name} {nomeNuovoTest}()");

                // Trovo la fine del metodo
                int indiceFineMetodo = 0;
                int indiceContatoreParentesi = 0;
                var carattereFineMetodo = fileTestString.Substring(indiceInizioMetodo)
                          .ToList()
                          .SkipWhile((c, contatore) => ConverterProgram.VerificaContinuaRicerca(c, contatore, ref indiceContatoreParentesi, ref indiceFineMetodo))
                          .FirstOrDefault();

                // Cancello poi il test precedente e il contenuto
                fileTestString = fileTestString.Remove(indiceInizioMetodo, indiceFineMetodo);

                // Infine inserisco il nuovo test
                fileTestString = fileTestString.Insert(indiceInizioMetodo, nuovoTest);
            }
            else
            {
                // Inserisco direttamente il nuovo test subito prima della fine del file, ma dentro la classe
                fileTestString = fileTestString.Insert(fileTestString.Substring(0, fileTestString.LastIndexOf('}') - 1).LastIndexOf('}'), nuovoTest);
            }

            // Scrivo infine il nuovo file
            File.WriteAllText(pathFileTest, fileTestString);
            Console.WriteLine($"{nomeNuovoTest} Method has been correctly created.");

            Console.WriteLine("Press any button to exit.");
            Console.ReadKey();
        }

        /// <summary>
        /// Valorizza i valori necessari per poter continuare
        /// </summary>
        private static void ImpostaScelteGlobaliUtente()
        {
            ConverterProgram.ImpostaGestioneFormatoDateTimeAggiuntivo();
            ConverterProgram.ImpostaNameSpaceGlobale();
        }

        /// <summary>
        /// Permette di impostare il namespace all'utente
        /// </summary>
        private static void ImpostaNameSpaceGlobale()
        {
            do
            {
                Console.WriteLine("Please specify the NameSpace of the generated class:");
                // Non faccio nessun controllo, se l'utente sbaglia dovrà riavviare il programma
                _namespaceScelto = Console.ReadLine();
            } while (String.IsNullOrWhiteSpace(_namespaceScelto));
        }

        /// <summary>
        /// Permette di gestire il formato aggiuntivo se l'utente lo desidera
        /// </summary>
        private static void ImpostaGestioneFormatoDateTimeAggiuntivo()
        {
            Console.WriteLine("Please press y if your xml contains DateTimes in the format 'MMyy', anything else to ignore it.");
            if (Console.ReadKey().KeyChar == 'y')
            {
                Console.WriteLine("Please don't use integers that follow the same pattern anywhere in the xml: '{01-12}{00-99}'");
                ConverterProgram._aggiungiFormatoDateTimeSpeciale = true;
                Console.WriteLine("Press any button to continue");
                Console.ReadKey();
            }
        }

        /// <summary>
        /// Permette di verificare se l'oggetto passato, è uguale a ciò che mi ha permesso di generarlo
        /// </summary>
        public static void TestaUguaglianzaDocumentoOggetto(XDocument documentoCaricato, object oggettoSerializzato)
        {
            // Deserializzo l'oggetto appena serializzato per verificare che sia tutto uguale
            var documentoSerializzato = ConverterProgram.ToXml(oggettoSerializzato);

            // Eseguo il test prima di procedere nel salvataggio per verificare che documento di partenza
            // e quello risultante dalla serializzazione dell'oggetto inizializzato siano uguali
            Tuple<XObject, XObject> result = documentoCaricato.DeepEquals(documentoSerializzato, XObjectComparisonOptions.Semantic);
            if (result != null)
            {
                throw new Exception("Conversion error. Exception: " + result);
            }
        }

        /// <summary>
        /// Permette di trovare la posizione della fine del metodo
        /// </summary>
        private static bool VerificaContinuaRicerca(char c, int contatore, ref int indiceContatoreParentesi, ref int indiceFineMetodo)
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

        /// <summary>
        /// Crea la stringa contenente la classe serializzata
        /// </summary>
        private static string CreaStringaClasseSerializzata(XDocument documentoCaricato, Dictionary<string, ElementoValido>.ValueCollection listaElementiValidi)
        {
            // Sbuilder che contiene la classe principale
            var stringBuilderClasse = new StringBuilder();

            // Inizializzo le using solitamente necessarie
            stringBuilderClasse.AppendLine("using System;");
            stringBuilderClasse.AppendLine("using System.Reflection;");
            stringBuilderClasse.AppendLine("using System.Globalization;");
            stringBuilderClasse.AppendLine("using System.Collections.Generic;");
            stringBuilderClasse.AppendLine("using System.Xml;");
            stringBuilderClasse.AppendLine("using System.Xml.Serialization;");
            stringBuilderClasse.AppendLine("using System.Linq;");
            stringBuilderClasse.AppendLine("using System.Xml.Linq;");
            stringBuilderClasse.AppendLine("");
            stringBuilderClasse.AppendLine($"namespace BELHXmlTool.{_namespaceScelto}");
            stringBuilderClasse.AppendLine("{");

            // Arrivati qui ogni elementovalido è valido e ha le proprietà che dovrebbe avere.
            var converter = new ConverterProgram();
            foreach (var elementoValido in listaElementiValidi)
            {
                string elementoSerializzato = converter.SerializzaElementoValido(documentoCaricato, elementoValido);
                stringBuilderClasse.Append(elementoSerializzato);
            }

            stringBuilderClasse.AppendLine("}");

            // A questo punto intendo tutto splittando ad ogni \n a seconda di quanti {(+) e }(-) trovo
            stringBuilderClasse = ConverterProgram.IndentaListaStringhe(stringBuilderClasse.ToString().Split('\n'));

            // Ciclo di nuovo il loadedDocument per togliere i /n
            stringBuilderClasse = stringBuilderClasse.Replace("\n", Environment.NewLine);

            // Gestione del salvataggio
            return stringBuilderClasse.ToString();
        }

        /// <summary>
        /// Permette di indentare la lista di stringhe passata nel modo corretto
        /// </summary>
        private static StringBuilder IndentaListaStringhe(IEnumerable<string> listaStringheDaIndentare, int counter = 0)
        {
            var asd = new StringBuilder();
            foreach (var stringa in listaStringheDaIndentare)
            {
                if (stringa.StartsWith("}"))
                {
                    counter--;
                }

                asd.Append(ConverterProgram.Indenta(counter))
                    .Append(stringa);

                if (stringa.StartsWith("{"))
                {
                    counter++;
                }
            }

            return asd;
        }

        /// <summary>
        /// Permette di creare la stringa in cui si inizializza il test
        /// </summary>
        private static string CreaMetodoTestInizializzazione(object oggetto, string classeSerializzataString,
            string nomeClasse, string nomeNuovoTest, XDocument documentoCaricato)
        {
            //Finalmente deserializzo la classe e la restituisco
            var gestoreInizializzazione = new TestCreatorManager();
            string stringaMetodoInizializzazioneOggetto = gestoreInizializzazione.CreaTest(oggetto, nomeNuovoTest, documentoCaricato);

            var stringBuilderInizializzazioneOggetto =
                ConverterProgram.IndentaListaStringhe(stringaMetodoInizializzazioneOggetto.Split('\n'), 2)
                    .Replace("\n", Environment.NewLine);
            return stringBuilderInizializzazioneOggetto.ToString();
        }

        /// <summary>
        /// Permette di serializzare l'elemento
        /// </summary>
        private string SerializzaElementoValido(XDocument loadedDocument, ElementoValido elementoValido)
        {
            var sbElemento = new StringBuilder();
            var primoElementoTipoAttuale = elementoValido.ListaElementiTipologiaAttuale[0];
            var nomeClasse = ConverterProgram.RendiPrimaLetteraMaiuscola(primoElementoTipoAttuale.Name.LocalName);
            sbElemento.AppendLine($"[XmlRoot(ElementName=\"{primoElementoTipoAttuale.Name.LocalName}\"{ConverterProgram.CalcolaNameSpace(primoElementoTipoAttuale.Name.NamespaceName)})]");
            sbElemento.AppendLine($"public class {nomeClasse}");
            sbElemento.AppendLine("{");

            foreach (var proprietaAttuale in elementoValido.ListaElementiProprieta)
            {
                // Non può essere null, li ho già esclusi
                var primoElementoProprietaAttuale = proprietaAttuale.ListaElementiTipologiaAttuale[0];

                // Provo a capire di che tipo di property si tratta
                string tipoProprieta = ConverterProgram.TrovaTipoProprietaElementoProprieta(loadedDocument, primoElementoProprietaAttuale.Name,
                    proprietaAttuale.ElementoRipetutoAlmenoUnaVolta, out string nomeProprieta, out bool tipoDateTime, out string formatoDateTime);

                // Continuo con le property aggiuntive necessarie e con il costruttore
                if (tipoDateTime)
                {
                    // Gestione particolare causata dal fatto che serializzando si perde il formato del datetime originale che invece deve essere preservato
                    // La proprieta datetime o lista di dateTime che andrò a modificare nei test
                    sbElemento.AppendLine("[XmlIgnore]");
                    sbElemento.AppendLine($"public {tipoProprieta} {nomeProprieta} {{ get; set; }}");


                    // Nome e namespace dell'elemento corretto
                    sbElemento.AppendLine($"[XmlElement(ElementName = \"{primoElementoProprietaAttuale.Name.LocalName}\"" +
                      $"{ConverterProgram.CalcolaNameSpace(primoElementoProprietaAttuale.Name.NamespaceName)}" +
                      // $",Order = {_contatoreFiglio}" +
                      $")]");

                    // Proprietà 
                    var tipoProprietaString = proprietaAttuale.ElementoRipetutoAlmenoUnaVolta ? "List<string>" : "string";
                    sbElemento.AppendLine($"public {tipoProprietaString} {nomeProprieta}Serializzabile");
                    sbElemento.AppendLine("{");

                    // Esempio:
                    // [XmlIgnore]
                    // public DateTime SomeDate { get; set; }

                    // [XmlElement("SomeDate")]
                    // public string SomeDateString
                    // {
                    //     get { return this.SomeDate.ToString("yyyy-MM-dd HH:mm:ss"); }
                    //     set { this.SomeDate = DateTime.Parse(value); }
                    // }

                    if (proprietaAttuale.ElementoRipetutoAlmenoUnaVolta)
                    {
                        // La stringa relativa renderizzata nel formato corretto
                        sbElemento.AppendLine($"\tget {{ return this.{nomeProprieta}.Where(v => v.HasValue).Select(v => v.Value.ToString(\"{formatoDateTime}\")).ToList(); }}");
                        sbElemento.AppendLine($"\tset {{ this.{nomeProprieta} = value.Select(v => DateTime.ParseExact(v,\"{formatoDateTime}\",CultureInfo.InvariantCulture)).ToList();}}");
                    }
                    else
                    {
                        // La stringa relativa renderizzata nel formato corretto
                        sbElemento.AppendLine($"\tget {{ return this.{nomeProprieta}.Value.ToString(\"{formatoDateTime}\"); }}");
                        sbElemento.AppendLine($"\tset {{ this.{nomeProprieta} = DateTime.ParseExact(value,\"{formatoDateTime}\",CultureInfo.InvariantCulture); }}");
                    }

                    sbElemento.AppendLine("}");
                }
                else
                {
                    // Nome e namespace dell'elemento corretto
                    sbElemento.AppendLine($"[XmlElement(ElementName = \"{primoElementoProprietaAttuale.Name.LocalName}\"" +
                      $"{ConverterProgram.CalcolaNameSpace(primoElementoProprietaAttuale.Name.NamespaceName)}" +
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
                _contatoreFiglio++;
            }

            // Elementi elemento valido attuale
            // Prendo prima i nomi degli elementi diversi presenti
            // Rimuovo quelli che ho già trattato come proprieta
            var listaMassimaNomiElementiFigli = elementoValido.ListaElementiTipologiaAttuale
                .SelectMany(e => e.Elements()
                                 .Where(ef => !elementoValido.ListaElementiProprieta
                                              .Any(evp => evp.ListaElementiTipologiaAttuale[0].Name == ef.Name))
                            .Select(ef => ef.Name))
                .Distinct().ToList();

            foreach (var nomeElemento in listaMassimaNomiElementiFigli)
            {
                // Controllo prima di tutto se ci sono più elementi di questo tipo all'interno dell'elemento attuale o negli altri
                var elementoRipetutoAlmenoUnaVolta = elementoValido.ListaElementiTipologiaAttuale.Any(e => e.Elements(nomeElemento).Count() > 1);
                string tipoProprieta = CalcolaTipoProprietaElementoFiglio(elementoRipetutoAlmenoUnaVolta, nomeElemento.LocalName);

                // Aggiungo Elemento per distinguerli dalle proprieta
                // Scrivo quindi il nome dell'elemento
                string nomeProprieta = null;
                string inizializzazioneProprieta = null;
                if (elementoRipetutoAlmenoUnaVolta)
                {
                    nomeProprieta = "Lista";
                    inizializzazioneProprieta = $"= new {tipoProprieta}();";
                }

                nomeProprieta += "Elemento" + ConverterProgram.RendiPrimaLetteraMaiuscola(nomeElemento.LocalName);

                // Scrivo la proprieta
                sbElemento.AppendLine($"[XmlElement(ElementName=\"{nomeElemento.LocalName}\"" +
                    $"{ConverterProgram.CalcolaNameSpace(nomeElemento.NamespaceName)}" +
                    // $",Order = {_contatoreFiglio}" +
                    $")]");

                sbElemento.AppendLine($"public {tipoProprieta} {nomeProprieta} {{ get; set; }} {inizializzazioneProprieta}");

                // Decide come scrivere cosa renderizzare e cosa no. non è mai null perchè inizializzo sempre le liste, ma può avere count = 0
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
                _contatoreFiglio++;
            }

            // Attributi elemento valido attuale
            // Prendo ora la lista degli attributi
            var listaMassimaNomiAttributi = elementoValido.ListaElementiTipologiaAttuale.SelectMany(e => e.Attributes().Select(a => a.Name)).Distinct().ToList();
            var sbNamespaces = new StringBuilder();
            foreach (var nomeAttributo in listaMassimaNomiAttributi)
            {
                // Prendo tutti gli attributi di quel tipo
                var listaAttributoAttuale = elementoValido.ListaElementiTipologiaAttuale.SelectMany(e => e.Attributes(nomeAttributo)).ToList();
                string nomeProprietaAttributo = ConverterProgram.RendiPrimaLetteraMaiuscola(nomeAttributo.LocalName);

                // Gesione particolare per i namespaces
                if (listaAttributoAttuale.All(a => a.IsNamespaceDeclaration))
                {
                    string nome = null;
                    if (nomeAttributo.LocalName.Equals("xmlns"))
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

                // Gestione particolare causata dal fatto che serializzando si perde il formato del datetime originale che invece deve essere preservato
                bool tipoDateTime = false;
                bool tipoDecimal = false;
                string formatoDateTime = null;
                Tuple<string, CultureInfo> tuplaFormatoDecimal = null;

                string tipoProprieta = null;
                if (tipoProprieta == null)
                {
                    formatoDateTime = ConverterProgram.TrovaFormatoDateTime(listaAttributoAttuale
                        .Select(a => (string)a)
                            .Where(v => !String.IsNullOrEmpty(v))
                        .ToList()
                    );

                    if (formatoDateTime != null)
                    {
                        tipoDateTime = true;
                    }
                }

                if (tipoProprieta == null && !tipoDateTime)
                {
                    tuplaFormatoDecimal = ConverterProgram.TrovaFormatoDecimal(listaAttributoAttuale
                           .Select(a => (string)a)
                               .Where(v => !String.IsNullOrEmpty(v))
                           .ToList()
                   );

                    if (tuplaFormatoDecimal != null)
                    {
                        tipoDecimal = true;
                    }
                }

                if (tipoProprieta == null && !tipoDateTime && !tipoDecimal)
                {
                    tipoProprieta = ConverterProgram.TrovaTipoProprietaAttributo(listaAttributoAttuale);
                }

                // Se è arrivato qui a null 
                if (tipoProprieta == null && !tipoDateTime && !tipoDecimal)
                {
                    tipoProprieta = "string";
                }

                bool serializzabileNecessario = false;
                // Scrivo quindi il nome dell'attributo
                if (tipoDateTime)
                {
                    // La proprieta datetime che andrò a modificare nei test
                    sbElemento.AppendLine("[XmlIgnore]");
                    sbElemento.AppendLine($"public DateTime? {nomeProprietaAttributo} {{ get; set; }}");

                    // La stringa relativa renderizzata nel formato corretto
                    // Facciamo in modo che non venga visto nei test o quando costruiamo l'oggetto
                    sbElemento.AppendLine("[System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]");
                    sbElemento.AppendLine($"[XmlAttribute(AttributeName=\"{nomeAttributo.LocalName}\")]");
                    sbElemento.AppendLine($"public string {nomeProprietaAttributo}Serializzabile");
                    sbElemento.AppendLine("{");
                    sbElemento.AppendLine($"\tget {{ return this.{nomeProprietaAttributo}.Value.ToString(\"{formatoDateTime}\"); }}");
                    sbElemento.AppendLine($"\tset {{ this.{nomeProprietaAttributo} = DateTime.ParseExact(value,\"{formatoDateTime}\",CultureInfo.InvariantCulture); }}");
                    sbElemento.AppendLine("}");

                    // Facciamo in modo che non venga visto nei test o quando costruiamo l'oggetto
                    sbElemento.AppendLine("[System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]");
                    sbElemento.AppendLine("[XmlIgnore]");
                    sbElemento.AppendLine($"public bool {nomeProprietaAttributo}SerializzabileSpecified {{ get {{ return this.{nomeProprietaAttributo}.HasValue; }} }}");

                    // Esempio:
                    //[XmlIgnore]
                    // public DateTime SomeDate { get; set; }

                    // [XmlElement("SomeDate")]
                    // public string SomeDateString
                    // {
                    //     get { return this.SomeDate.ToString("yyyy-MM-dd HH:mm:ss"); }
                    //     set { this.SomeDate = DateTime.Parse(value); }
                    // }

                    // Specified che stabilisce quando serializzare e quando no
                    // [XmlIgnore]
                    // public bool SomeDateStringSpecified { get { return Age >= 0; } }
                }
                else if (tipoDecimal)
                {
                    // La proprieta datetime che andrò a modificare nei test
                    sbElemento.AppendLine("[XmlIgnore]");
                    sbElemento.AppendLine($"public decimal? {nomeProprietaAttributo} {{ get; set; }}");

                    // La stringa relativa renderizzata nel formato corretto
                    // Facciamo in modo che non venga visto nei test o quando costruiamo l'oggetto
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
                    // Specified che stabilisce quando serializzare e quando no
                    // [XmlIgnore]
                    // public bool AgeSpecified { get { return Age >= 0; } }
                    sbElemento.AppendLine("[XmlIgnore]");
                    string serializzabile = serializzabileNecessario ? "Serializzabile" : null;
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

                //sbElemento.AppendLine("set");
                //sbElemento.AppendLine("{");
                //sbElemento.AppendLine("this.Namespaces = new XmlSerializerNamespaces(new XmlQualifiedName[]");
                //sbElemento.AppendLine("{");
                //sbElemento.Append(sbNamespaces.ToString());
                //sbElemento.AppendLine("});");
                //sbElemento.AppendLine("}");

                sbElemento.AppendLine("}");
            }

            // Scrivo prima di chiudere l'elemento il suo valore per l'innertext, se necessario
            if (elementoValido.ListaElementiTipologiaAttuale.Any(e => e.Nodes().Any(n => n.NodeType == System.Xml.XmlNodeType.Text && !String.IsNullOrEmpty(e.Value))))
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
        /// Permette di trovare il formato decimal corretto
        /// </summary>
        private static Tuple<string, CultureInfo> TrovaFormatoDecimal(List<string> listaValori)
        {
            if (ConverterProgram._listaTupleFormatiCultureDecimal == null)
            {
                ConverterProgram.CaricaListaFormatiDecimal();
            }

            // Converto i valori una volta sola
            List<Tuple<string, decimal>> listaTuplaValoriDecimali = listaValori.Select(v =>
                    decimal.TryParse(v, NumberStyles.Number, CultureInfo.InvariantCulture, out var valoreConvertito) ? new Tuple<string, decimal>(v, valoreConvertito) : null)
                    .ToList();

            // Se anche solo uno dei valori non è un decimal, questo elemento non è sempre associabile ad un decimal
            if (listaTuplaValoriDecimali.Any(t => t == null))
            {
                return null;
            }

            foreach (var tuplaFormato in ConverterProgram._listaTupleFormatiCultureDecimal)
            {
                if (listaTuplaValoriDecimali.Count > 0 && listaTuplaValoriDecimali.All(tuplaValore => ConverterProgram.TrovaFormatoDecimalCorretto(tuplaValore, tuplaFormato)))
                {
                    return tuplaFormato;
                }
            }

            return null;
        }

        /// <summary>
        /// Permette di trovare il formato decimal corretto
        /// </summary>
        private static bool TrovaFormatoDecimalCorretto(Tuple<string, decimal> tuplaValore, Tuple<string, CultureInfo> tuplaFormato)
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
        /// Permette di trovare il formato Decimal corretto
        /// </summary>
        private static void CaricaListaFormatiDecimal()
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

            _listaTupleFormatiCultureDecimal = listaFormatiConNumeri.SelectMany(formato => listaCulture.Select(c => new Tuple<string, CultureInfo>(formato, c))).ToList();
        }

        /// <summary>
        /// Permette di trovare il formato DateTime corretto
        /// </summary>
        private static string TrovaFormatoDateTime(List<string> listaValori)
        {
            if (ConverterProgram._listaFormatiDateTimeStandard == null)
            {
                ConverterProgram.CaricaListaFormatiDateTime();
            }

            foreach (var formato in ConverterProgram._listaFormatiDateTimeStandard)
            {
                if (listaValori.All(v => DateTime.TryParseExact(v, formato, CultureInfo.InvariantCulture, DateTimeStyles.AssumeUniversal, out var data)))
                {
                    return formato;
                }
            }

            return null;
        }

        /// <summary>
        /// Carica i formati esistenti se necessario
        /// </summary>
        private static void CaricaListaFormatiDateTime()
        {
            _listaFormatiDateTimeStandard =
                new List<string>
                {
                    "d", "D", "f", "F", "g", "G",
                    "m", "M", "o", "O", "r", "R",
                    "s", "t", "T", "u", "U", "y",
                    "Y", "yyyy-MM-dd","yyyy-MM-dd HH:mm:ss",
                    "yyyy-MM-ddTHH:mm:ssZ",
                };

            if (_aggiungiFormatoDateTimeSpeciale)
            {
                _listaFormatiDateTimeStandard.Add("MMyy");
            }
        }

        /// <summary>
        /// Trova il tipo di property dell'attributo attuale
        /// </summary>
        private static string TrovaTipoProprietaAttributo(List<XAttribute> listaAttributo)
        {
            string tipoProprieta = null;

            // Se almeno uno ha un valore
            if (listaAttributo.Any(e => !String.IsNullOrEmpty(e.Value)))
            {
                // Se è arrivato qui allora riprova gli elementi precedenti, in fila, ma con dei nullable
                if (tipoProprieta == null)
                {
                    tipoProprieta = listaAttributo.All(e => String.IsNullOrEmpty(e.Value) || bool.TryParse(e.Value, out bool valoreBool)) ? "bool?" : null;
                }
                if (tipoProprieta == null)
                {
                    tipoProprieta = listaAttributo.All(e => String.IsNullOrEmpty(e.Value) || short.TryParse(e.Value, out short valore)) ? "short?" : null;
                }
                if (tipoProprieta == null)
                {
                    tipoProprieta = listaAttributo.All(e => String.IsNullOrEmpty(e.Value) || int.TryParse(e.Value, out int valore)) ? "int?" : null;
                }
                // Non cerco per datetime e decimal perchè sono già gestiti fuori
            }

            return tipoProprieta;
        }

        /// <summary>
        /// Permette di calcolare il tipo di proprietà dell'elemento figlio
        /// </summary>
        private static string CalcolaTipoProprietaElementoFiglio(bool elementoRipetutoAlmenoUnaVolta, string nomeElemento)
        {
            if (elementoRipetutoAlmenoUnaVolta)
            {
                return $"List<{ConverterProgram.RendiPrimaLetteraMaiuscola(nomeElemento)}>";
            }
            else
            {
                return ConverterProgram.RendiPrimaLetteraMaiuscola(nomeElemento);
            }
        }

        /// <summary>
        /// Trova il tipo di proprieta
        /// </summary>
        private static string TrovaTipoProprietaElementoProprieta(XDocument loadedDocument, XName nomeProprietaAttuale,
            bool elementoRipetutoAlmenoUnaVolta, out string nomeProprieta, out bool tipoDateTime, out string formatoDateTime)
        {
            string tipoProprieta = null;
            string nomeProprietaNormalizzato = ConverterProgram.RendiPrimaLetteraMaiuscola(nomeProprietaAttuale.LocalName);
            tipoDateTime = false;
            formatoDateTime = null;

            // Se non ha attributi in nessun caso lo trasformo direttamente nella relativa proprieta
            var listaMassimaElementoAttuale = loadedDocument.Descendants(nomeProprietaAttuale);
            if (listaMassimaElementoAttuale.All(e => e.Attributes().Count() == 0))
            {
                // Se almeno uno ha un valore
                if (listaMassimaElementoAttuale.Any(e => !String.IsNullOrEmpty(e.Value)))
                {
                    // Se è arrivato qui allora riprova gli elementi precedenti, in fila, ma con dei nullable
                    if (tipoProprieta == null)
                    {
                        tipoProprieta = listaMassimaElementoAttuale.All(e => String.IsNullOrEmpty(e.Value) || bool.TryParse(e.Value, out bool valoreBool)) ? "bool?" : null;
                    }
                    if (tipoProprieta == null)
                    {
                        tipoProprieta = listaMassimaElementoAttuale.All(e => String.IsNullOrEmpty(e.Value) || short.TryParse(e.Value, out short valore)) ? "short?" : null;
                    }
                    if (tipoProprieta == null)
                    {
                        tipoProprieta = listaMassimaElementoAttuale.All(e => String.IsNullOrEmpty(e.Value) || int.TryParse(e.Value, out int valore)) ? "int?" : null;
                    }
                    if (tipoProprieta == null)
                    {
                        tipoProprieta = listaMassimaElementoAttuale.All(e => String.IsNullOrEmpty(e.Value) || decimal.TryParse(e.Value, out decimal valore)) ? "decimal?" : null;
                    }
                    if (tipoProprieta == null && (listaMassimaElementoAttuale.All(e => String.IsNullOrEmpty(e.Value) || DateTime.TryParse(e.Value, out DateTime valore))))
                    {
                        tipoProprieta = "DateTime?";
                        tipoDateTime = true;
                        formatoDateTime = ConverterProgram.TrovaFormatoDateTime(
                            listaMassimaElementoAttuale
                                .Where(v => !String.IsNullOrEmpty(v.Value))
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
        /// Restituisce la stringa da inserire nel namespace quando serve
        /// </summary>
        private static string CalcolaNameSpace(string nameSpaceString)
        {
            if (!String.IsNullOrWhiteSpace(nameSpaceString))
            {
                return $", Namespace =\"{nameSpaceString}\"";
            }

            return "";
        }

        /// <summary>
        /// Permette di indentare un numero di volte che gli si passa davanti ad ogni riga
        /// </summary>
        private static string Indenta(int counter)
        {
            var sbOut = new StringBuilder(counter);
            for (int i = 0; i < counter; i++)
            {
                sbOut.Append("\t");
            }

            return sbOut.ToString();
        }

        /// <summary>
        /// Rende la prima lettera dell'input maiuscola
        /// </summary>
        private static string RendiPrimaLetteraMaiuscola(string input)
        {
            switch (input)
            {
                case null: throw new ArgumentNullException(nameof(input));
                case "": throw new ArgumentException($"{nameof(input)} cannot be empty", nameof(input));
                default:
                    input = input.Replace("+", "")
                                 .Replace("-", "");
                    return (input.First().ToString().ToUpper() + input.Substring(1));
            }
        }

        /// <summary>
        /// Classe utilizzata dagli elementi validi
        /// </summary>
        private class ElementoValido
        {
            public List<XElement> ListaElementiTipologiaAttuale { get; set; }
            public bool ElementoRipetutoAlmenoUnaVolta { get; set; }
            public List<ElementoValido> ListaElementiProprieta { get; set; }

            public ElementoValido(XElement elemento)
            {
                this.ElementoRipetutoAlmenoUnaVolta = false;
                this.ListaElementiProprieta = new List<ElementoValido>();
                this.ListaElementiTipologiaAttuale =
                    new List<XElement>()
                    {
                        elemento
                    };
            }
        }

        /// <summary>
        /// Trasforma l'oggetto in xml
        /// </summary>
        public static XDocument ToXml(object objToXml)
        {
            StreamWriter stWriter = null;
            XmlSerializer xmlSerializer;
            string buffer;
            try
            {
                xmlSerializer = new XmlSerializer(objToXml.GetType());
                MemoryStream memStream = new MemoryStream();
                stWriter = new StreamWriter(memStream);
                XmlSerializerNamespaces ns = (XmlSerializerNamespaces)objToXml.GetType().GetProperty("Namespaces").GetValue(objToXml);
                if (ns == null)
                {
                    ns = new XmlSerializerNamespaces(new XmlQualifiedName[] { new XmlQualifiedName(String.Empty, String.Empty) });
                }
                xmlSerializer.Serialize(stWriter, objToXml, ns);
                buffer = Encoding.UTF8.GetString(memStream.GetBuffer()).Replace("\x00", "");
            }
            catch (Exception Ex)
            {
                throw Ex;
            }
            finally
            {
                if (stWriter != null) stWriter.Close();
            }

            return XDocument.Parse(buffer);
        }
    }
}