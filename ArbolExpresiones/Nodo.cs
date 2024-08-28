using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PSO.ArbolExpresiones
{
    public class Nodo
    {
        public string Valor { get; set; }
        public Nodo Izquierda { get; set; }
        public Nodo Derecha { get; set; }

        public Nodo(string valor)
        {
            Valor = valor;
            Izquierda = null;
            Derecha = null;
        }
    }
}
