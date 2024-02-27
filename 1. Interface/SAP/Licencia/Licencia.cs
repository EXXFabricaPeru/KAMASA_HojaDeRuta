using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Exxis.Addon.HojadeRutaAGuia.Interface.Licencia
{
    public class Licencia
    {
        string _codProducto;

        public string CodProducto
        {
            get { return _codProducto; }
            set { _codProducto = value; }
        }
        string _instllNumber;

        public string InstllNumber
        {
            get { return _instllNumber; }
            set { _instllNumber = value; }
        }
        DateTime _fechavigencia;

        public DateTime Fechavigencia
        {
            get { return _fechavigencia; }
            set { _fechavigencia = value; }
        }
        private string _raw;

        public string Raw
        {
            get { return _raw; }
            set { _raw = value; }
        }

        public Licencia()
        {
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="tabs">la licencia ccon sus propiedades separados por tabulaciones</param>
        public Licencia(String tabs)
        {
            string[] partes = tabs.Split('|');
            _codProducto = partes[0];
            _instllNumber = partes[1];
            _fechavigencia = DateTime.ParseExact(partes[2], "yyyyMMdd", null);
            _raw = tabs;
        }
    }
}
