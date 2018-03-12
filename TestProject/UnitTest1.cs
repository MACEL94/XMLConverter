using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WebApplications.Utilities;

namespace TestProject
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
            //var asd = TestMethod();

            // Deserializzo l'oggetto appena serializzato per verificare che sia tutto uguale
            //var documentoSerializzato = ToXml(asd);

            // Eseguo il test prima di procedere nel salvataggio per verificare che documento di partenza
            // e quello risultante dalla serializzazione dell'oggetto inizializzato siano uguali
            //Tuple<XObject, XObject> result = XDocument.Load("C:/Users/franc/source/repos/XMLConverter/XMLConverter/ResourcesExample/BookingExpertXML.xml").DeepEquals(documentoSerializzato, XObjectComparisonOptions.Semantic);

            //if (result != null)
            //{
            //    throw new Exception("Conversion error. Exception: " + result);
            //}
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
                XmlSerializerNamespaces ns = objToXml.GetXmlNamespaceDeclarations();
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
