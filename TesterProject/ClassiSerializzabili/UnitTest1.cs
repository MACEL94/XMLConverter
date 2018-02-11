using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml.Serialization;
using BELHXmlTool;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace TesterProject
{
    [TestClass]
    public class ClasseTest
    {
        [TestMethod]
        public void ProvaCreaOggetto()
        {
            // Inizializza il serializer con il tipo dell'oggetto caricato
            XmlSerializer serializer = new XmlSerializer(typeof(OTA_ResRetrieveRS));
            var i = 0;
        }

        /// <summary>
        /// Trasforma l'oggetto in xml
        /// </summary>
        public string ToXml(object objToXml)
        {
            StreamWriter stWriter = null;
            XmlSerializer xmlSerializer;
            string buffer;
            try
            {
                xmlSerializer = new XmlSerializer(objToXml.GetType());
                MemoryStream memStream = new MemoryStream();
                stWriter = new StreamWriter(memStream);
                xmlSerializer.Serialize(stWriter, objToXml);
                buffer = Encoding.ASCII.GetString(memStream.GetBuffer());
            }
            catch (Exception Ex)
            {
                throw Ex;
            }
            finally
            {
                if (stWriter != null) stWriter.Close();
            }
            return buffer;
        }

    }
}
