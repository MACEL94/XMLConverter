using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace XMLConverter.Classes
{
    /// <summary>
    /// Classe utilizzata dagli elementi validi 
    /// </summary>
    public class ElementoValido
    {
        #region Public Constructors

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

        #endregion Public Constructors

        #region Public Properties

        public bool ElementoRipetutoAlmenoUnaVolta { get; set; }
        public List<ElementoValido> ListaElementiProprieta { get; set; }
        public List<XElement> ListaElementiTipologiaAttuale { get; set; }

        #endregion Public Properties
    }
}