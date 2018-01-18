using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace XMLConverter
{
    public class Program
    {
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
                        Console.Out.WriteLine("Resources directory has been found but no valid xml was loaded. \nPlease insert an XML file in it. \nPress any button to continue or q to quit.");
                        if (Console.ReadKey().KeyChar.Equals('q'))
                        {
                            return;
                        }
                    }
                }
                else
                {
                    Directory.CreateDirectory(path);
                    Console.Out.WriteLine("Resources directory has been created. \nPlease insert an XML file in it. \nPress any button to continue or q to quit.");
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
                    Console.Out.WriteLine($"File: {filePath}\nis not a valid XML and could not be parsed.");
                }
            }

            // Per ciascun elemento controlla:
            // Se ha attributi converte l'elemento in classe
            // Se ha solo un valore diventa una proprietà della classe formata dal primo oggetto padre che lo contiene
            // Se compare più di una volta nel primo elemento padre che si trova già presente nel dizionario, si deve fare una lista di questo in quel padre
            Dictionary<string, ElementoValido> dizionarioElementiValidi;
            Dictionary<string, ElementoValido> dizionarioElementiProprieta;
            string nomeFile = null;

            foreach (var documento in listaDocumentiValidi)
            {
                dizionarioElementiValidi = new Dictionary<string, ElementoValido>();
                dizionarioElementiProprieta = new Dictionary<string, ElementoValido>();

                foreach (var elemento in documento.Descendants())
                {
                    // Prende il primo elemento utile, quindi finchè il dizionario è vuoto
                    if (dizionarioElementiValidi.Count == 0)
                    {
                        nomeFile = Program.RendiPrimaLetteraMaiuscola(elemento.Name.LocalName) + ".cs";
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
                sbDocumento.AppendLine("using System.Collections.Generic;");
                sbDocumento.AppendLine("using System.Xml;");
                sbDocumento.AppendLine("using System.Xml.Serialization;");
                sbDocumento.AppendLine("");
                sbDocumento.AppendLine("namespace BELHXmlTool");
                sbDocumento.AppendLine("{");

                // Arrivati qui ogni elementovalido è valido e ha le proprietà che dovrebbe avere.
                List<ElementoValido> listaProprietaConAttributi = new List<ElementoValido>();
                foreach (var elementoValido in listaElementiValidi)
                {
                    string elementoSerializzato = Program.SerializzaElementoValido(documento, elementoValido, ref listaProprietaConAttributi);
                    sbDocumento.Append(elementoSerializzato);
                }

                sbDocumento.AppendLine("}");

                // A questo punto intendo tutto splittando ad ogni \n a seconda di quanti {(+) e }(-) trovo
                var listaStringhe = sbDocumento.ToString().Split('\n');

                sbDocumento = new StringBuilder();
                int counter = 0;
                foreach (var stringa in listaStringhe)
                {
                    if (stringa.StartsWith("}"))
                    {
                        counter--;
                    }

                    sbDocumento.Append(Program.Indenta(counter))
                        .Append(stringa);

                    if (stringa.StartsWith("{"))
                    {
                        counter++;
                    }
                }

                // Gestione del salvataggio
                // Salvo tutto in un nuovo file  $"{Root.Name}.cs" in una cartella chiamata Classes
                string percorsoCartellaDestinazione = Path.Combine(Environment.CurrentDirectory, "Classes");
                if (!Directory.Exists(percorsoCartellaDestinazione))
                {
                    Directory.CreateDirectory(percorsoCartellaDestinazione);
                    Console.Out.WriteLine("\nClasses directory has been created.\n");
                }

                var percorsoFileSerializzato = Path.Combine(percorsoCartellaDestinazione, nomeFile);
                File.WriteAllText(percorsoFileSerializzato, sbDocumento.ToString());
                Console.Out.WriteLine($"{percorsoFileSerializzato} has been correctly created.");

                //Test
                //XmlSerializer serializer =
                //new XmlSerializer(typeof(Breakfast_menu));
                //var nomeFileXML = nomeFile.Substring(0, nomeFile.Length - 3) + ".xml";
                //var percorsoFileXMLSerializzato = Path.Combine(percorsoCartellaDestinazione, nomeFileXML);
                //TextWriter writer = new StreamWriter(percorsoFileXMLSerializzato);
                //Breakfast_menu breakFast = new Breakfast_menu();

                //breakFast.ListaElementoFood = new List<Food>
                //{
                //    new Food
                //    {
                //        Calories = 123,
                //        Description = "descrizione",
                //        Name = new Name{Id = true},
                //        Price = "254"
                //    }
                //};

                //serializer.Serialize(writer, breakFast);
                //writer.Close();
            }

            Console.WriteLine($"\n{listaDocumentiValidi.Count} valid XML files were correctly loaded and converted. \nPress any button to exit.");
            Console.ReadKey();
            return;
        }

        /// <summary>
        /// Permette di serializzare l'elemento
        /// </summary>
        private static string SerializzaElementoValido(XDocument documento, ElementoValido elementoValido,
            ref List<ElementoValido> listaProprietaConAttributi)
        {
            var sbElemento = new StringBuilder();
            var primoElementoTipoAttuale = elementoValido.ListaElementiTipologiaAttuale[0];
            var nomeClasse = Program.RendiPrimaLetteraMaiuscola(primoElementoTipoAttuale.Name.LocalName);
            sbElemento.AppendLine($"[XmlRoot(ElementName=\"{primoElementoTipoAttuale.Name.LocalName}\"{Program.CalcolaNameSpace(primoElementoTipoAttuale.Name.NamespaceName)})]");
            sbElemento.AppendLine($"public class {nomeClasse}");
            sbElemento.AppendLine("{");

            // Elementi figli seppur indiretti
            foreach (var proprietaAttuale in elementoValido.ListaElementiProprieta)
            {
                // Non può essere null, li ho già esclusi
                var primoElementoProprietaAttuale = proprietaAttuale.ListaElementiTipologiaAttuale[0];

                // Provo a capire di che tipo di property si tratta
                string tipoProprieta = TrovaTipoProprietaElementoProprieta(documento, primoElementoProprietaAttuale.Name,
                    proprietaAttuale.ElementoRipetutoAlmenoUnaVolta, out string nomeProprieta, out bool elementoProprieta, out bool tipoDateTime);

                sbElemento.AppendLine($"[XmlElement(ElementName = \"{primoElementoProprietaAttuale.Name.LocalName}\"{Program.CalcolaNameSpace(primoElementoProprietaAttuale.Name.NamespaceName)}{AggiungiDataType(tipoDateTime)})]");

                // Scrivo finalmente la proprietà
                sbElemento.AppendLine($"public {tipoProprieta} {nomeProprieta} {{ get; set; }}");

                // Lo aggiungo alla lista di elementi da trattare alla fine se ha attributi
                if (elementoProprieta)
                {
                    listaProprietaConAttributi.Add(proprietaAttuale);
                }
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
                if (elementoRipetutoAlmenoUnaVolta)
                {
                    nomeProprieta = "Lista";
                }

                nomeProprieta += "Elemento" + Program.RendiPrimaLetteraMaiuscola(nomeElemento.LocalName);

                // Scrivo la proprieta
                sbElemento.AppendLine($"[XmlElement(ElementName=\"{nomeElemento.LocalName}\"{Program.CalcolaNameSpace(nomeElemento.NamespaceName)})]");
                sbElemento.AppendLine($"public {tipoProprieta} {nomeProprieta} {{ get; set; }} = new {tipoProprieta}();");
            }

            // Attributi elemento valido attuale
            // Prendo ora la lista degli attributi
            var listaMassimaNomiAttributi = elementoValido.ListaElementiTipologiaAttuale.SelectMany(e => e.Attributes().Select(a => a.Name)).Distinct().ToList();
            foreach (var nomeAttributo in listaMassimaNomiAttributi)
            {
                // Prendo tutti gli attributi di quel tipo
                var listaAttributoAttuale = elementoValido.ListaElementiTipologiaAttuale.SelectMany(e => e.Attributes(nomeAttributo)).ToList();
                string tipoProprieta = TrovaTipoProprietaAttributo(listaAttributoAttuale, out bool tipoDateTime);

                // Scrivo quindi il nome dell'attributo 
                sbElemento.AppendLine($"[XmlAttribute(AttributeName=\"{nomeAttributo.LocalName}\"{AggiungiDataType(tipoDateTime)})]");
                sbElemento.AppendLine($"public {tipoProprieta} {Program.RendiPrimaLetteraMaiuscola(nomeAttributo.LocalName)} {{ get; set; }}");
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
        /// Aggiunge se necessario il datatype
        /// </summary>
        private static string AggiungiDataType(bool tipoDateTime)
        {
            if (tipoDateTime)
            {
                return ", DataType = \"date\"";
            }

            return null;
        }

        private static string TrovaTipoProprietaAttributo(List<XAttribute> listaAttributo, out bool tipoDateTime)
        {
            tipoDateTime = false;
            string tipoProprieta = listaAttributo.All(e => bool.TryParse(e.Value, out bool valoreBool)) ? "bool" : null;
            if (tipoProprieta == null && listaAttributo.All(e => DateTime.TryParse(e.Value, out DateTime valoredecimal)))
            {
                tipoProprieta = "DateTime";
                tipoDateTime = true;
            }
            if (tipoProprieta == null)
            {
                tipoProprieta = listaAttributo.All(e => decimal.TryParse(e.Value, out decimal valoredecimal)) ? "decimal" : null;
            }
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
                return $"List<{Program.RendiPrimaLetteraMaiuscola(nomeElemento)}>";
            }
            else
            {
                return Program.RendiPrimaLetteraMaiuscola(nomeElemento);
            }
        }

        private static string TrovaTipoProprietaElementoProprieta(XDocument documento, XName nomeProprietaAttuale, bool elementoRipetutoAlmenoUnaVolta,
            out string nomeProprieta, out bool elementoProprieta, out bool tipoDateTime)
        {
            string tipoProprieta = null;
            elementoProprieta = false;
            string nomeProprietaNormalizzato = Program.RendiPrimaLetteraMaiuscola(nomeProprietaAttuale.LocalName);
            tipoDateTime = false;

            // Se non ha attributi in nessun caso
            var listaMassimaElementoAttuale = documento.Descendants(nomeProprietaAttuale);
            if (listaMassimaElementoAttuale.All(e => e.Attributes().Count() == 0))
            {
                tipoProprieta = listaMassimaElementoAttuale.All(e => bool.TryParse(e.Value, out bool valoreBool)) ? "bool" : null;
                if (tipoProprieta == null && listaMassimaElementoAttuale.All(e => DateTime.TryParse(e.Value, out DateTime valoredecimal)))
                {
                    tipoProprieta = "DateTime";
                    tipoDateTime = true;
                }
                if (tipoProprieta == null)
                {
                    tipoProprieta = listaMassimaElementoAttuale.All(e => decimal.TryParse(e.Value, out decimal valoredecimal)) ? "decimal" : null;
                }
                if (tipoProprieta == null)
                {
                    tipoProprieta = "string";
                }
            }
            else
            {
                // Se ne ha
                tipoProprieta = nomeProprietaNormalizzato;
                elementoProprieta = true;
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
            var sbOut = new StringBuilder();
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