using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace XMLConverter
{
    internal static class Util
    {
        public static XmlSerializerNamespaces GetXmlNamespaceDeclarations<T>(this T obj)
        {
            if (obj == null)
                return null;
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
    }
}
