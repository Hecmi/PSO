using System;

namespace PSO
{
    class Program
    {    
        public static void Main()
        {
            int NUMERO_PARTICULAS = 30;
            int NUMERO_ITERACIONES = 300;
            double FACTOR_INERCIA = 0.5;
            double FACTOR_COGNITIVO = 1.5;
            double FACTOR_SOCIAL = 1.5;

            Clases.PSO pso = new Clases.PSO(
                NUMERO_PARTICULAS, 
                NUMERO_ITERACIONES, 
                FACTOR_INERCIA, 
                FACTOR_COGNITIVO, 
                FACTOR_SOCIAL
            );
            pso.ejecutar();            
        }
    }
}
