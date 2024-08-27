using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PSO.Clases
{
    class Particula
    {
        public double[] Posicion;
        public double[] Velocidad;
        public double[] MejorPosicion;
        public double PBest;

        public Particula(int dimension)
        {
            Posicion = new double[dimension];
            Velocidad = new double[dimension];
            MejorPosicion = new double[dimension];
            PBest = double.MaxValue;
        }
    }

}
