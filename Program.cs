using System;
using System.Collections.Generic;
using PSO.ArbolExpresiones;

namespace PSO
{
    class Program
    {    
        public static void Main()
        {
            //AST ast = new AST("1/(2^x)");
            string ecuacion = "(10-x)^2+100*(y-x^2)^2";
            AST ast = new AST(ecuacion);
            
            ast.mostrar_arbol();

            Dictionary<string, double> valores_incognitas = new Dictionary<string, double>
            {
                { "x", 10 },
                { "y", 100 },
            };

            for(int i = 0; i < ast.INCOGNITAS.Count; i++)
            {
                Console.WriteLine(ast.INCOGNITAS[i]);
            }

            double resultado = ast.evaluar(valores_incognitas);
            Console.WriteLine(resultado);

            /*
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
            */
        }
    }
}
