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

namespace XMLConverter
{
    public class ConverterProgram
    {
        /// <summary>
        /// Tiene in memoria tutti i formati esistenti una volta caricati
        /// </summary>
        private static List<string> _listaFormatiStandard { get; set; }

        // Contatore Elementi figli
        private int _contatoreFiglio = 1;

        private static void Main()
        {
            // Mi accerto che esista la cartella, se non è così la creo e aspetto che l'utente ci metta qualcosa
            var path = Environment.CurrentDirectory + "/Resources";
            var listaFilePaths = new List<string>();
            do
            {
                if (Directory.Exists(path))
                {
                    listaFilePaths = Directory.GetFiles(path).ToList();
                    if (listaFilePaths.Count == 0)
                    {
                        Console.WriteLine("Resources directory has been found but no valid xml was loaded. \nPlease insert an XML file in it. \nPress any button to continue or q to quit.");
                        if (Console.ReadKey().KeyChar.Equals('q'))
                        {
                            return;
                        }
                    }
                }
                else
                {
                    Directory.CreateDirectory(path);
                    Console.WriteLine("Resources directory has been created. \nPlease insert an XML file in it. \nPress any button to continue or q to quit.");
                    if (Console.ReadKey().KeyChar.Equals('q'))
                    {
                        return;
                    }
                }
            } while ((!Directory.Exists(path) || listaFilePaths.Count == 0));

            var listaDocumentiValidi = new List<XDocument>();
            foreach (var filePath in listaFilePaths)
            {
                // Per ciascun XML scorre gli elementi, e prende per forza la radice, che sarà il nome della classe principale,
                // Più una lista di elementi validi all'interno
                // Prova a convertirlo, se non ci riesce lo scrive
                try
                {
                    var documento = XDocument.Load(filePath);
                    listaDocumentiValidi.Add(documento);
                }
                catch
                {
                    Console.WriteLine($"File: {filePath}\nis not a valid XML and could not be parsed.");
                }
            }

            // Per ciascun elemento controlla:
            // Se ha attributi converte l'elemento in classe
            // Se ha solo un valore diventa una proprietà della classe formata dal primo oggetto padre che lo contiene
            // Se compare più di una volta nel primo elemento padre che si trova già presente nel dizionario, si deve fare una lista di questo in quel padre
            Dictionary<string, ElementoValido> dizionarioElementiValidi;
            Dictionary<string, ElementoValido> dizionarioElementiProprieta;
            string nomeFileAttuale = null;
            string nomeClasseAttuale = null;

            foreach (var documento in listaDocumentiValidi)
            {
                dizionarioElementiValidi = new Dictionary<string, ElementoValido>();
                dizionarioElementiProprieta = new Dictionary<string, ElementoValido>();
                foreach (var elemento in documento.Descendants())
                {
                    // Prende il primo elemento utile, quindi finchè il dizionario è vuoto
                    if (dizionarioElementiValidi.Count == 0)
                    {
                        nomeClasseAttuale = ConverterProgram.RendiPrimaLetteraMaiuscola(elemento.Name.LocalName);
                        nomeFileAttuale = nomeClasseAttuale + ".cs";
                    }

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

                // Sbuilder che contiene la classe principale
                var sbDocumento = new StringBuilder();

                // Inizializzo le using solitamente necessarie
                sbDocumento.AppendLine("using System;");
                sbDocumento.AppendLine("using System.Reflection;");
                sbDocumento.AppendLine("using System.Globalization;");
                sbDocumento.AppendLine("using System.Collections.Generic;");
                sbDocumento.AppendLine("using System.Xml;");
                sbDocumento.AppendLine("using System.Xml.Serialization;");
                sbDocumento.AppendLine("using System.Linq;");
                sbDocumento.AppendLine("using System.Xml.Linq;");
                sbDocumento.AppendLine("");
                sbDocumento.AppendLine("namespace BELHXmlTool");
                sbDocumento.AppendLine("{");

                // Arrivati qui ogni elementovalido è valido e ha le proprietà che dovrebbe avere.
                var converter = new ConverterProgram();
                foreach (var elementoValido in listaElementiValidi)
                {
                    string elementoSerializzato = converter.SerializzaElementoValido(documento, elementoValido);
                    sbDocumento.Append(elementoSerializzato);
                }

                sbDocumento.AppendLine("}");

                // A questo punto intendo tutto splittando ad ogni \n a seconda di quanti {(+) e }(-) trovo
                sbDocumento = ConverterProgram.IndentaListaStringhe(sbDocumento.ToString().Split('\n'));

                // Ciclo di nuovo il documento per togliere i /n
                sbDocumento = sbDocumento.Replace("\n", Environment.NewLine);

                // Gestione del salvataggio
                // Salvo tutto in un nuovo file  $"{Root.Name}.cs" in una cartella chiamata Classes
                string percorsoCartellaDestinazione = Path.Combine(Environment.CurrentDirectory, "Classes");
                if (!Directory.Exists(percorsoCartellaDestinazione))
                {
                    Directory.CreateDirectory(percorsoCartellaDestinazione);
                    Console.WriteLine(Environment.NewLine + "Classes directory has been created." + Environment.NewLine);
                }

                var percorsoFileSerializzato = Path.Combine(percorsoCartellaDestinazione, nomeFileAttuale);
                string sbDocumentoString = sbDocumento.ToString();
                File.WriteAllText(percorsoFileSerializzato, sbDocumentoString);
                Console.WriteLine($"{percorsoFileSerializzato} has been correctly created.");

                // Test da scommentare o commentare a seconda che serva o meno
                // Stringa contenente l'oggetto inizializzato
                string inizializzazioneOggetto = ConverterProgram.CreaStringaOggettoInizializzato(documento, sbDocumentoString, nomeClasseAttuale);

                // Salvo l'oggetto in un nuovo file di testo, sempre nella stessa cartella, cambiando il nome del file
                nomeFileAttuale = nomeFileAttuale.Substring(0, nomeFileAttuale.Length - 3) + ".txt";
                percorsoFileSerializzato = Path.Combine(percorsoCartellaDestinazione, nomeFileAttuale);
                File.WriteAllText(percorsoFileSerializzato, inizializzazioneOggetto);
                Console.WriteLine($"{percorsoFileSerializzato} has been correctly created.");
            }

            Console.WriteLine($"{Environment.NewLine}{listaDocumentiValidi.Count} valid XML files were correctly loaded and converted.");
            Console.WriteLine("Press any button to exit.");
            Console.ReadKey();
            return;
        }

        /// <summary>
        /// Permette di indentare la lista di stringhe passata nel modo corretto
        /// </summary>
        private static StringBuilder IndentaListaStringhe(IEnumerable<string> listaStringheDaIndentare)
        {
            var asd = new StringBuilder();
            int counter = 0;
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
        private static string CreaStringaOggettoInizializzato(XDocument documento, string classeSerializzataString, string nomeClasse)
        {
            CodeDomProvider cdp = new Microsoft.CodeDom.Providers.DotNetCompilerPlatform.CSharpCodeProvider();
            CompilerParameters cp = new CompilerParameters();
            cp.GenerateInMemory = true;

            cp.ReferencedAssemblies.Add(@"System.dll");
            cp.ReferencedAssemblies.Add(@"mscorlib.dll");
            cp.ReferencedAssemblies.Add(@"sysglobl.dll");
            cp.ReferencedAssemblies.Add(@"System.Net.dll");
            cp.ReferencedAssemblies.Add(@"System.Core.dll");
            cp.ReferencedAssemblies.Add(@"C:\Program Files (x86)\Reference Assemblies\Microsoft\Framework\.NETFramework\v4.6.1\System.Xml.dll");
            cp.ReferencedAssemblies.Add(@"C:\Program Files (x86)\Reference Assemblies\Microsoft\Framework\.NETFramework\v4.6.1\System.Xml.Linq.dll");
            cp.ReferencedAssemblies.Add(@"C:\Program Files (x86)\Reference Assemblies\Microsoft\Framework\.NETFramework\v4.6.1\System.Data.dll");
            cp.ReferencedAssemblies.Add(@"C:\Program Files (x86)\Reference Assemblies\Microsoft\Framework\.NETFramework\v4.6.1\System.Data.DataSetExtensions.dll");
            cp.IncludeDebugInformation = true;

            // Prova a compilare il file creato
            CompilerResults compilerResults = cdp.CompileAssemblyFromSource(cp, classeSerializzataString);

            // Prende l'assembly
            Assembly assembly = compilerResults.CompiledAssembly;

            // Crea un istanza dell'oggetto, chiaramente aggiungo il NameSpace che so già essere sempre lo stesso
            object oggettoAttuale = assembly.CreateInstance("BELHXmlTool." + nomeClasse);
            var tipoOggettoAttuale = oggettoAttuale.GetType();

            // Inizializza il serializer con il tipo dell'oggetto caricato
            XmlSerializer serializer = new XmlSerializer(tipoOggettoAttuale);

            // Carico il documento in un memoryStream che può essere deserializzato e ne resetto la posizione per poterlo leggere
            var ms = new MemoryStream();
            documento.Save(ms);
            ms.Position = 0;
            oggettoAttuale = serializer.Deserialize(ms);

            //Finalmente deserializzo la classe e la restituisco
            string stringaInizializzazioneOggetto = ObjectInitializationSerializer.SerializeToInitializerClass(oggettoAttuale);
            return ConverterProgram.IndentaListaStringhe(stringaInizializzazioneOggetto.Split('\n')).Replace("\n", Environment.NewLine).ToString();
        }

        /// <summary>
        /// Permette di serializzare l'elemento
        /// </summary>
        private string SerializzaElementoValido(XDocument documento, ElementoValido elementoValido)
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
                string tipoProprieta = ConverterProgram.TrovaTipoProprietaElementoProprieta(documento, primoElementoProprietaAttuale.Name,
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
                      $",Order = {_contatoreFiglio})]");

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
                      $",Order = {_contatoreFiglio})]");

                    sbElemento.AppendLine($"public {tipoProprieta} {nomeProprieta} {{ get; set; }}");
                }

                string condizioneAggiuntivaSerializzazione = null;
                if (proprietaAttuale.ElementoRipetutoAlmenoUnaVolta)
                {
                    condizioneAggiuntivaSerializzazione = $" && this.{ nomeProprieta }.Count > 0";
                }

                // Fa in modo che solo ciò che è valorizzato venga serializzato
                // Facciamo in modo che non venga visto nei test o quando costruiamo l'oggetto
                sbElemento.AppendLine("[System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]");
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
                    $",Order = {_contatoreFiglio})]");

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
            foreach (var nomeAttributo in listaMassimaNomiAttributi)
            {
                // Prendo tutti gli attributi di quel tipo
                var listaAttributoAttuale = elementoValido.ListaElementiTipologiaAttuale.SelectMany(e => e.Attributes(nomeAttributo)).ToList();
                // Gestione particolare causata dal fatto che serializzando si perde il formato del datetime originale che invece deve essere preservato
                bool tipoDateTime = false;
                string formato = ConverterProgram.TrovaFormatoDateTime(listaAttributoAttuale
                    .Where(a => !String.IsNullOrEmpty(a.Value))
                        .Select(a => a.Value)
                        .ToList()
                );

                string tipoProprieta;
                if (formato != null)
                {
                    tipoDateTime = true;
                    tipoProprieta = "DateTime?";
                }
                else
                {
                    tipoProprieta = TrovaTipoProprietaAttributo(listaAttributoAttuale);
                }

                string nomeProprietaAttributo = ConverterProgram.RendiPrimaLetteraMaiuscola(nomeAttributo.LocalName);
                bool serializzabileNecessario = false;
                // Scrivo quindi il nome dell'attributo
                if (tipoDateTime)
                {
                    // La proprieta datetime che andrò a modificare nei test
                    sbElemento.AppendLine("[XmlIgnore]");
                    sbElemento.AppendLine($"public {tipoProprieta} {nomeProprietaAttributo} {{ get; set; }}");

                    // La stringa relativa renderizzata nel formato corretto
                    sbElemento.AppendLine($"[XmlAttribute(AttributeName=\"{nomeAttributo.LocalName}\")]");
                    sbElemento.AppendLine($"public string {nomeProprietaAttributo}Serializzabile");
                    sbElemento.AppendLine("{");
                    sbElemento.AppendLine($"\tget {{ return this.{nomeProprietaAttributo}.Value.ToString(\"{formato}\"); }}");
                    sbElemento.AppendLine($"\tset {{ this.{nomeProprietaAttributo} = DateTime.ParseExact(value,\"{formato}\",CultureInfo.InvariantCulture); }}");
                    sbElemento.AppendLine("}");

                    // Specified che stabilisce quando serializzare e quando no
                    // [XmlIgnore]
                    // public bool AgeSpecified { get { return Age >= 0; } }
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
                        sbElemento.AppendLine($"[XmlAttribute(AttributeName=\"{nomeAttributo.LocalName}\")]");
                        sbElemento.AppendLine($"public {tipoProprieta.Substring(0, tipoProprieta.Length - 1)} {nomeProprietaAttributo}Serializzabile {{ get => this.{nomeProprietaAttributo}.Value; set => this.{nomeProprietaAttributo} = value; }}");
                        condizionePerSerializzare = ".HasValue";
                    }

                    // Specified che stabilisce quando serializzare e quando no
                    // [XmlIgnore]
                    // public bool AgeSpecified { get { return Age >= 0; } }
                    sbElemento.AppendLine("[XmlIgnore]");
                    string serializzabile = serializzabileNecessario ? "Serializzabile" : null;
                    sbElemento.AppendLine($"public bool {nomeProprietaAttributo}{serializzabile}Specified {{ get {{ return this.{nomeProprietaAttributo}{condizionePerSerializzare}; }} }}");
                }
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
        /// Permette di trovare il formato corretto
        /// </summary>
        private static string TrovaFormatoDateTime(List<string> listaValori)
        {
            if (ConverterProgram._listaFormatiStandard == null)
            {
                ConverterProgram.CaricaListaFormati();
            }

            foreach (var formato in ConverterProgram._listaFormatiStandard)
            {
                if (listaValori.All(v => ConverterProgram.TryParseExact(v, formato)))
                {
                    return formato;
                }
            }

            return null;
        }

        /// <summary>
        /// Tryparse di tipo exact
        /// </summary>
        private static bool TryParseExact(string value, string formato)
        {
            try
            {
                DateTime.ParseExact(value, formato, CultureInfo.InvariantCulture);
                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Carica i formati esistenti se necessario
        /// </summary>
        private static void CaricaListaFormati()
        {
            ConverterProgram._listaFormatiStandard =
                new List<string>
                {
                "d", "D", "f", "F", "g", "G",
                "m", "M", "o", "O", "r", "R",
                "s", "t", "T", "u", "U", "y",
                "Y", "yyyy-MM-dd","yyyy-MM-dd HH:mm:ss",
                "MMyy",
                };
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
                if (tipoProprieta == null)
                {
                    tipoProprieta = listaAttributo.All(e => String.IsNullOrEmpty(e.Value) || decimal.TryParse(e.Value, out decimal valore)) ? "decimal?" : null;
                }
            }

            // Se è arrivato qui a null 
            if (tipoProprieta == null)
            {
                tipoProprieta = "string";
            }

            return tipoProprieta;
        }

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
        private static string TrovaTipoProprietaElementoProprieta(XDocument documento, XName nomeProprietaAttuale,
            bool elementoRipetutoAlmenoUnaVolta, out string nomeProprieta, out bool tipoDateTime, out string formatoDateTime)
        {
            string tipoProprieta = null;
            string nomeProprietaNormalizzato = ConverterProgram.RendiPrimaLetteraMaiuscola(nomeProprietaAttuale.LocalName);
            tipoDateTime = false;
            formatoDateTime = null;

            // Se non ha attributi in nessun caso lo trasformo direttamente nella relativa proprieta
            var listaMassimaElementoAttuale = documento.Descendants(nomeProprietaAttuale);
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
    }
}