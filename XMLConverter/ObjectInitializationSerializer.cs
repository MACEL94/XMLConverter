using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XMLConverter
{
    public static class ObjectInitializationSerializer
    {
        private static string GetCSharpString(object o)
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
                return $"{((decimal)o).ToString("00.00", CultureInfo.InvariantCulture)}m";
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
                var inizializzazioneElementi = GetItems((IEnumerable)o);
                if (inizializzazioneElementi.Replace(",", "").Replace("\n", "").Replace("\r", "").Length > 0)
                {
                    stringaDaRitornare = $"new {GetClassName(o)} \r\n{{\r\n{inizializzazioneElementi}}}";
                }
                return stringaDaRitornare;
            }


            return CreateObject(o).ToString();
        }

        private static string GetItems(IEnumerable items)
        {
            return items.Cast<object>().Aggregate(string.Empty, (current, item) => current + $"{GetCSharpString(item)},\r\n");
        }

        private static StringBuilder CreateObject(object o)
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

                //// Si tratta di un fantoccio, va serializzato solo se il padre è presente ed ha un valore, altrimenti lancia eccezione
                //if (property.Name.EndsWith("Serializzabile"))
                //{
                //    if (!dizionarioProprieta.TryGetValue(property.Name.Replace("Serializzabile", ""), out var propertyEffettiva) || propertyEffettiva.GetValue(o) == null)
                //    {
                //        continue;
                //    }
                //}
                var value = property.GetValue(o);
                if (value != null)
                {
                    var inizializzazioneProprieta = GetCSharpString(value);
                    if (inizializzazioneProprieta != null && inizializzazioneProprieta.Length > 0)
                    {
                        propertiesBuilder.Append($"{property.Name} = {inizializzazioneProprieta},\r\n");
                    }
                }
            }

            if (propertiesBuilder.Length > 0)
            {
                objectBuilder.Append($"new {GetClassName(o)} \r\n{{\r\n")
                    .Append(propertiesBuilder.ToString())
                    .Append("}");
            }

            return objectBuilder;
        }

        private static string GetClassName(object o)
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
        public static string SerializeToInitializerClass(object o)
        {
            return $"var newObject = {CreateObject(o)};";
        }
    }
}