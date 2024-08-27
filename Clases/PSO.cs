using System;

namespace PSO.Clases
{
    class PSO
    {        
        //Variables definidas para la configuración del algoritmo
        private int NUMERO_PARTICULAS = 30;
        private double W = 0.5;
        private double C1 = 1.5;
        private double C2 = 1.5;
        private int ITERACIONES = 100;

        private Random random = new Random();

        public PSO(int NUMERO_PARTICULAS, int ITERACIONES, double W, double C1, double C2)
        {
            this.NUMERO_PARTICULAS = NUMERO_PARTICULAS;
            this.ITERACIONES = ITERACIONES;

            this.W = W;
            this.C1 = C1;
            this.C2 = C2;
        }

        private double evaluar_funcion_objetivo(double[] x) {
            double a = 1;
            double b = 100;
            double c = 30;
            double resultado = 0.0;

            //f(x,y) = (a-x)^2 + b(y-x^2)^2
            //f(x,y) = (1-x)^2 + 100(y-x^2)^2
           
            resultado += Math.Pow(a - x[0], 2) + b * Math.Pow(x[1] - x[0] * x[0], 2) + c*Math.Pow(x[2] - Math.Pow(x[1], 2), 2);
            
            return resultado;
        }

        public void ejecutar()
        {
            //Número de dimensiones que es equivalente a la cantidad de 
            //incógnitas de la función objetivo
            int dimensiones = 3;

            //Inicializar las variables correspondientes
            Particula[] particulas = new Particula[NUMERO_PARTICULAS];
            double[] mejor_posicion_global = new double[dimensiones];
            double gBest = double.MaxValue;

            // Initialize particles
            for (int i = 0; i < NUMERO_PARTICULAS; i++)
            {
                particulas[i] = new Particula(dimensiones);
                for (int d = 0; d < dimensiones; d++)
                {
                    //Iniciar la partícula i en una posición aleatoria
                    //en este caso entre -5 y 5
                    particulas[i].Posicion[d] = random.NextDouble() * 10 - 5;
                    particulas[i].Velocidad[d] = random.NextDouble() * 2 - 1;

                    particulas[i].MejorPosicion[d] = particulas[i].Posicion[d];
                }

                double ajuste_actual = evaluar_funcion_objetivo(particulas[i].Posicion);
                particulas[i].PBest = ajuste_actual;

                if (ajuste_actual < gBest)
                {
                    gBest = ajuste_actual;
                    Array.Copy(particulas[i].MejorPosicion, mejor_posicion_global, dimensiones);
                }
            }

            //Ejecutar el algoritmo PSO
            for (int iter = 0; iter < ITERACIONES; iter++)
            {
                foreach (Particula particle in particulas)
                {
                    //Actualizar la velocidad y posición de cada partícula
                    for (int d = 0; d < dimensiones; d++)
                    {
                        double r1 = random.NextDouble();
                        double r2 = random.NextDouble();

                        particle.Velocidad[d] = W * particle.Velocidad[d]
                                            + C1 * r1 * (particle.MejorPosicion[d] - particle.Posicion[d])
                                            + C2 * r2 * (mejor_posicion_global[d] - particle.Posicion[d]);

                        particle.Posicion[d] += particle.Velocidad[d];
                    }

                    //Evaluar la nueva posición de cada partícula
                    double ajuste_actual = evaluar_funcion_objetivo(particle.Posicion);
                    if (ajuste_actual < particle.PBest)
                    {
                        particle.PBest = ajuste_actual;
                        Array.Copy(particle.Posicion, particle.MejorPosicion, dimensiones);

                        //Sí el valor es menor, entonces ajustar el mejor global (gBest)
                        if (ajuste_actual < gBest)
                        {
                            gBest = ajuste_actual;
                            Array.Copy(particle.Posicion, mejor_posicion_global, dimensiones);
                        }
                    }
                }

                Console.WriteLine($"Iteración {iter + 1}: Mejor posición = ({string.Join(", ", mejor_posicion_global)}), gBest = {gBest}");
            }
        }
    }
}
