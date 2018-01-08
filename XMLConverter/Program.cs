using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace XMLConverter
{
    public class Program
    {
        private static void Main()
        {
            // Prende tutti gli xml della cartella Resources
            var path = Environment.CurrentDirectory + "/Resources";
            var listaFileStrings = new List<string>();
            do
            {
                if (Directory.Exists(path))
                {
                    listaFileStrings = Directory.GetFiles(path).ToList();
                    if (listaFileStrings.Count == 0)
                    {
                        Console.Out.WriteLine("Resources directory has been found but no valid xml was loaded. \nPlease insert an XML file in it. " +
                            "\n Press any button to continue or q to quit.");
                        if (Console.ReadKey().KeyChar.Equals('q'))
                        {
                            return;
                        }
                    }
                }
                else
                {
                    Directory.CreateDirectory(path);
                    Console.Out.WriteLine("Resources directory has been created. \nPlease insert an XML file in it. " +
                        "\n Press any button to continue or q to quit.");
                    if (Console.ReadKey().KeyChar.Equals('q'))
                    {
                        return;
                    }
                }
            } while ((!Directory.Exists(path) || listaFileStrings.Count == 0));
            var listaDocumentiValidi = new List<XDocument>();
            foreach (var filestring in listaFileStrings)
            {
                // Per ciascun XML scorre gli elementi, e prende per forza la radice, che sarà il nome della classe principale,
                // Più una lista di elementi validi all'interno
                // Prova a convertirlo, se non ci riesce lo scrive
                try
                {
                    var documento = XDocument.Load(filestring);
                    listaDocumentiValidi.Add(documento);
                }
                catch
                {
                    Console.Out.WriteLine($"Non e' stato possibile convertire il file {filestring} poichè non è un XML ben formato.");
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
                        if (elemento.Elements().Count() > 1 || elemento.Attributes().Count() > 0)
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

                var listaElementiProprieta = dizionarioElementiProprieta.Values;
                var listaElementiValidi = dizionarioElementiValidi.Values;

                // Questo tipo di verifica va però fatto alla fine del parsing di tutti gli elementi validi per il dizionario,
                // quindi lo mettiamo in un altro dizionario per ora.
                // Se è già presente lo aggiungiamo a se stesso, altrimenti al dizionario, ci servirà a capire se è una lista o meno
                foreach (var elementoProprieta in listaElementiProprieta)
                {
                    foreach (var elementoValido in listaElementiValidi)
                    {
                        // Se almeno uno di loro è contenuto in almeno uno degli altri lo aggiunge alle proprietà del rispettivo
                        foreach (var elementoTipoAttuale in elementoProprieta.ListaElementiTipologiaAttuale)
                        {
                            if (elementoValido.ListaElementiTipologiaAttuale.Any(e => e.Element(elementoTipoAttuale.Name) != null))
                            {
                                elementoProprieta.ElementoRipetuto = elementoValido.ListaElementiTipologiaAttuale.Any(e => e.Elements(elementoTipoAttuale.Name).Count() > 1);
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
                sbDocumento.AppendLine("namespace BLHXML");
                sbDocumento.AppendLine("{");

                // Arrivati qui ogni elementovalido è valido e ha le proprietà che dovrebbe avere.
                foreach (var elementoValido in listaElementiValidi)
                {
                    string elementoSerializzato = Program.SerializzaElementoValido(elementoValido);
                    sbDocumento.Append(elementoSerializzato);
                }

                sbDocumento.AppendLine("}");

                // A questo punto intendo tutto splittando ad ogni \n e indentando a seconda di quanti {(+) e }(-) trovo
                // Salvo tutto in un nuovo file  $"{Root.Name}.cs" in una cartella chiamata Classes
                Console.Out.WriteLine(sbDocumento.ToString());
                string destPath = Path.Combine(Environment.CurrentDirectory, "Classes", nomeFile);
                File.WriteAllText(destPath, sbDocumento.ToString());
            }

            Console.WriteLine($"{listaDocumentiValidi.Count} valid XML files were correctly loaded and converted. \n Press any button to exit.");
            Console.ReadLine();
        }

        /// <summary>
        /// Permette di serializzare l'elemento
        /// </summary>
        private static string SerializzaElementoValido(ElementoValido elementoValido)
        {
            var sbElemento = new StringBuilder();
            var primoElementoTipoAttuale = elementoValido.ListaElementiTipologiaAttuale[0];
            sbElemento.AppendLine($"[XmlRoot(ElementName=\"{primoElementoTipoAttuale.Name.LocalName}\"{Program.CalcolaNameSpace(primoElementoTipoAttuale.Name.NamespaceName)})]");
            sbElemento.AppendLine($"public class {Program.RendiPrimaLetteraMaiuscola(primoElementoTipoAttuale.Name.LocalName)}");
            sbElemento.AppendLine("{");

            foreach (var proprietaAttuale in elementoValido.ListaElementiProprieta)
            {
                var primoElementoProprietaAttuale = proprietaAttuale.ListaElementiTipologiaAttuale[0];
                sbElemento.AppendLine($"[XmlElement(ElementName = \"{primoElementoProprietaAttuale.Name.LocalName}\"{Program.CalcolaNameSpace(primoElementoProprietaAttuale.Name.NamespaceName)})]");

                // Non può essere null, li ho già esclusi. Provo a capire di che tipo si tratta
                string tipoProprieta = proprietaAttuale.ListaElementiTipologiaAttuale.All(e => bool.TryParse(e.Value, out bool valoreBool)) ? "bool" : null;
                if (tipoProprieta == null)
                {
                    tipoProprieta = proprietaAttuale.ListaElementiTipologiaAttuale.All(e => decimal.TryParse(e.Value, out decimal valoredecimal)) ? "decimal" : null;
                }
                if (tipoProprieta == null)
                {
                    tipoProprieta = "string";
                }

                if (proprietaAttuale.ElementoRipetuto)
                {
                    tipoProprieta = $"List<{tipoProprieta}>";
                }

                sbElemento.AppendLine($" public {tipoProprieta} {Program.RendiPrimaLetteraMaiuscola(primoElementoProprietaAttuale.Name.LocalName)} {{ get; set; }}");
            }

            // TODO -oFBE: parsa qui gli attributi

            sbElemento.AppendLine("}");

            return sbElemento.ToString();
        }

        private static string CalcolaNameSpace(string nameSpaceString)
        {
            if (!String.IsNullOrWhiteSpace(nameSpaceString))
            {
                return $", Namespace =\"{nameSpaceString}\"";
            }

            return "";
        }

        private static string Indenta(int counter)
        {
            var sbOut = new StringBuilder();
            for (int i = 0; i < counter; i++)
            {
                sbOut.Append("/t");
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
                default: return input.First().ToString().ToUpper() + input.Substring(1);
            }
        }

        /// <summary>
        /// Classe utilizzata dagli elementi validi
        /// </summary>
        private class ElementoValido
        {
            public List<XElement> ListaElementiTipologiaAttuale { get; set; }
            public bool ElementoRipetuto { get; set; }
            public List<ElementoValido> ListaElementiProprieta { get; set; }

            public ElementoValido()
            {
                this.ListaElementiProprieta = new List<ElementoValido>();
                this.ListaElementiTipologiaAttuale = new List<XElement>();
            }

            public ElementoValido(XElement elemento)
            {
                this.ElementoRipetuto = false;
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