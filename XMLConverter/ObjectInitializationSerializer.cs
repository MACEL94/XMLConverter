using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace XMLConverter
{
    public class ObjectInitializationSerializer
    {
        public ObjectInitializationSerializer() { }
        private string RicavaInizializzazioneProprieta(object o)
        {
            if (o is bool)
            {
                return $"{o.ToString().ToLower()}";
            }
            if (o is short)
            {
                return $"{o.ToString()}";
            }
            if (o is int)
            {
                return $"{o}";
            }
            if (o is decimal)
            {
                // Siamo sempre precisi nella inizializzazione della proprietà(4 cifre dopo lo 0), 
                // ci pensano poi i campi dell'oggetto a serializzare e deserializzare sempre nel formato corretto
                return $"{((decimal)o).ToString("00.0000", CultureInfo.InvariantCulture)}m";
            }
            if (o is DateTime)
            {
                return $"DateTime.Parse(\"{o}\")";
            }
            if (o is string)
            {
                return $"\"{o}\"";
            }
            if (o is IEnumerable)
            {
                string stringaDaRitornare = null;
                var inizializzazioneElementi = this.RicavaElementi((IEnumerable)o);
                if (inizializzazioneElementi.Replace(",", "").Replace("\n", "").Replace("\r", "").Length > 0)
                {
                    stringaDaRitornare = $"new {this.RicavaNomeClasse(o)} {Environment.NewLine}{{{inizializzazioneElementi}{Environment.NewLine}}}";
                }
                return stringaDaRitornare;
            }

            return this.InizializzaOggetto(o);
        }

        private string RicavaElementi(IEnumerable items)
        {
            return items.Cast<object>().Aggregate(string.Empty, (current, item) => current + $"{Environment.NewLine}{this.RicavaInizializzazioneProprieta(item)},");
        }

        private string InizializzaOggetto(object o)
        {
            var objectBuilder = new StringBuilder();
            var propertiesBuilder = new StringBuilder();
            var dizionarioProprieta = o.GetType().GetProperties().ToDictionary(p => p.Name);
            foreach (var property in dizionarioProprieta.Values)
            {
                // Se finisce con specified va saltato, basta valorizzare il resto
                if (property.Name.EndsWith("Specified") || property.Name.EndsWith("Serializzabile"))
                {
                    continue;
                }

                var value = property.GetValue(o);
                if (value != null)
                {
                    var inizializzazioneProprieta = this.RicavaInizializzazioneProprieta(value);
                    if (inizializzazioneProprieta != null && inizializzazioneProprieta.Length > 0)
                    {
                        propertiesBuilder.AppendLine($"{property.Name} = {inizializzazioneProprieta},");
                    }
                }
            }

            if (propertiesBuilder.Length > 0)
            {
                objectBuilder.AppendLine($"new {this.RicavaNomeClasse(o)}")
                             .AppendLine("{")
                             .Append(propertiesBuilder.ToString())
                             .Append("}");
            }

            return objectBuilder.ToString();
        }

        private string RicavaNomeClasse(object o)
        {
            var type = o.GetType();

            if (type.IsGenericType)
            {
                var arg = type.GetGenericArguments().First().Name;
                return type.Name.Replace("`1", $"<{arg}>");
            }

            return type.Name;
        }

        /// <summary>
        ///  Inizializza la classe dell'oggetto passato con i suoi valori
        /// </summary>
        public string CreaTest(object o, string nomeTest, XDocument documentoCaricato)
        {
            // Stringa di inizializzazione dell'oggetto
            var stringaInizializzazioneOggetto = this.InizializzaOggetto(o);

            // TODO -oFBE: Prima o poi vorrei, prima di restituire il tutto, controllare di nuovo che siano uguali 
            // il documento di partenza e l'inizializzazione dell'oggetto rispettivo
            // ConverterProgram.TestaUguaglianzaDocumentoOggetto(documentoCaricato, oggettoSerializzato);
            return new StringBuilder()
                .AppendLine($"public {this.RicavaNomeClasse(o)} {nomeTest}()")
                .AppendLine("{")
                .AppendLine($"return {stringaInizializzazioneOggetto};")
                .AppendLine("}")
                .ToString();
        }
    }
}