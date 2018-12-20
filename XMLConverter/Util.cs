using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;
using WebApplications.Utilities;

namespace XMLConverter
{
    public static class Util
    {
        #region Public Methods

        public static object CreaOggettoSerializzato(XDocument documentoCaricato, string nomeClasseAttuale, string classeSerializzataString, string nameSpaceScelto)
        {
            // Parametri Compilatore
            var compilerParams = new CompilerParameters
            {
                // Voglio la libreria dll, non l'exe
                GenerateExecutable = false
            };

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

            // Compilatore
            CodeDomProvider cdp = new Microsoft.CodeDom.Providers.DotNetCompilerPlatform.CSharpCodeProvider();

            /* Gestione salvataggio dll, per ora non serve
            // Chiedo all'utilizzatore se vuole che venga creata la dll. Se è così gli chiedo di
            // specificare il path dove vuole che venga creata
            string dllPath;
            Console.WriteLine("Specify the complete path of the new dll if you want to create it or just press Enter to skip: ");
            dllPath = Console.ReadLine();

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
            */

            // Arrivato qui Voglio che non venga salvato l'assembly ma solo creato un temporaneo
            compilerParams.GenerateInMemory = true;

            // Prova a compilare il file creato
            CompilerResults compilerResults = cdp.CompileAssemblyFromSource(compilerParams, classeSerializzataString);

            // Prende finalmente l'assembly
            Assembly assembly = compilerResults.CompiledAssembly;

            // Crea un istanza dell'oggetto, chiaramente aggiungo il NameSpace che so già
            var oggettoSerializzato = assembly.CreateInstance($"BELHXmlTool.{nameSpaceScelto}.{nomeClasseAttuale}");

            // Inizializza il serializer con il tipo dell'oggetto caricato
            var serializer = new XmlSerializer(oggettoSerializzato.GetType());

            // Carico il loadedDocument in un memoryStream che può essere deserializzato e ne resetto
            // la posizione per poterlo leggere
            var ms = new MemoryStream();
            documentoCaricato.Save(ms);
            ms.Position = 0;
            oggettoSerializzato = serializer.Deserialize(ms);

            return oggettoSerializzato;
        }

        /// <summary>
        /// Trasforma l'oggetto in xml 
        /// </summary>
        public static XDocument CreateXmlFromObj(object objToXml)
        {
            StreamWriter stWriter = null;
            string buffer = null;
            try
            {
                var xmlSerializer = new XmlSerializer(objToXml.GetType());
                var memStream = new MemoryStream();
                stWriter = new StreamWriter(memStream);
                XmlSerializerNamespaces ns = objToXml.GetXmlNamespaceDeclarations();
                if (ns == null)
                {
                    ns = new XmlSerializerNamespaces(new XmlQualifiedName[] { new XmlQualifiedName(string.Empty, string.Empty) });
                }
                xmlSerializer.Serialize(stWriter, objToXml, ns);
                buffer = Encoding.UTF8.GetString(memStream.GetBuffer()).Replace("\x00", "");
            }
            catch (Exception ex)
            {
                // Loggo solamente
                Console.WriteLine(ex);
            }
            finally
            {
                if (stWriter != null)
                {
                    stWriter.Close();
                }
            }

            return XDocument.Parse(buffer);
        }

        public static XmlSerializerNamespaces GetXmlNamespaceDeclarations<T>(this T obj)
        {
            if (obj == null)
            {
                return null;
            }

            var type = obj.GetType();
            return type.GetFields()
                .Where(f => Attribute.IsDefined(f, typeof(XmlNamespaceDeclarationsAttribute)))
                .Select(f => f.GetValue(obj))
                .Concat(type.GetProperties()
                    .Where(p => Attribute.IsDefined(p, typeof(XmlNamespaceDeclarationsAttribute)))
                    .Select(p => p.GetValue(obj, null)))
                .OfType<XmlSerializerNamespaces>()
                .SingleOrDefault();
        }

        /// <summary>
        /// Permette di indentare un numero di volte che gli si passa davanti ad ogni riga 
        /// </summary>
        public static string Indenta(int counter)
        {
            var sbOut = new StringBuilder(counter);
            for (var i = 0; i < counter; i++)
            {
                sbOut.Append("\t");
            }

            return sbOut.ToString();
        }

        /// <summary>
        /// Rende la prima lettera dell'input maiuscola 
        /// </summary>
        public static string RendiPrimaLetteraMaiuscola(string input)
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
        /// Permette di verificare se l'oggetto passato, è uguale a ciò che mi ha permesso di generarlo 
        /// </summary>
        public static void TestaUguaglianzaDocumentoOggetto(XDocument documentoCaricato, object oggettoSerializzato)
        {
            // Deserializzo l'oggetto appena serializzato per verificare che sia tutto uguale
            var documentoSerializzato = Util.CreateXmlFromObj(oggettoSerializzato);

            // Eseguo il test prima di procedere nel salvataggio per verificare che documento di
            // partenza e quello risultante dalla serializzazione dell'oggetto inizializzato siano uguali
            Tuple<XObject, XObject> result = documentoCaricato.DeepEquals(documentoSerializzato, XObjectComparisonOptions.Semantic);
            if (result != null)
            {
                throw new Exception("Conversion error. Exception: " + result);
            }

            result = documentoSerializzato.DeepEquals(documentoCaricato, XObjectComparisonOptions.Semantic);
            if (result != null)
            {
                throw new Exception("Conversion error. Exception: " + result);
            }
        }

        #endregion Public Methods
    }
}