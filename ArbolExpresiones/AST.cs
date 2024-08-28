using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PSO.ArbolExpresiones
{
    public class AST
    {
        public Nodo RAIZ;
        public List<string> INCOGNITAS;

        public AST(string expresion)
        {
            INCOGNITAS = new List<string>();

            RAIZ = construir_arbol(expresion);
        }

        private Nodo construir_arbol(string expresion)
        {
            Stack<Nodo> nodos = new Stack<Nodo>();
            Stack<char> operadores = new Stack<char>();
                        
            //Iterar cada uno de los elementos de la expresión
            for (int i = 0; i < expresion.Length; i++)
            {
                char c = expresion[i];

                //En caso de que esa un número o una incógnita, entonces
                //continuar acumulando la operación mientras en la expresión sea numérica (entera o decimal)
                if (char.IsDigit(c) || c == '.')
                {
                    string numero = c.ToString();
                    while (i + 1 < expresion.Length && (char.IsDigit(expresion[i + 1]) || expresion[i + 1] == '.'))
                    {
                        numero += expresion[++i];
                    }
                    nodos.Push(new Nodo(numero));
                }
                else if (char.IsLetter(c))
                {
                    // Acumulación de incógnitas
                    string incognita = c.ToString();

                    while (i + 1 < expresion.Length && char.IsLetterOrDigit(expresion[i + 1]))
                    {
                        incognita += expresion[++i];
                    }

                    nodos.Push(new Nodo(incognita));

                    if (!INCOGNITAS.Contains(incognita)) INCOGNITAS.Add(incognita);
                }
                else if (c == '(')
                {
                    operadores.Push(c);
                }
                else if (c == ')')
                {
                    //Retroceder en la cola (tomar el primer elemento o desencolar)
                    //hasta que se encuenre el paréntesis de apertura
                    while (operadores.Count > 0 && operadores.Peek() != '(')
                    {
                        nodos.Push(segmentar_operacion(operadores.Pop(), nodos.Pop(), nodos.Pop()));
                    }

                    //Eliminar (desencolar) el paréntesis de apertura
                    operadores.Pop();
                }
                else if (es_operador(c))
                {
                    //Sí el nuveo operador tiene una precendia menor que la última en cola, entonces
                    //crear el nuevo nodo de la operación ya que tiene prioridad.
                    while (operadores.Count > 0 && precedencia_operaciones(c) <= precedencia_operaciones(operadores.Peek()))
                    {
                        nodos.Push(segmentar_operacion(operadores.Pop(), nodos.Pop(), nodos.Pop()));
                    }

                    //Agregar el último operador
                    operadores.Push(c);
                }
            }

            //Agregar los nodos residuales en conjunto a sus operadores            
            while (operadores.Count > 0)
            {
                nodos.Push(segmentar_operacion(operadores.Pop(), nodos.Pop(), nodos.Pop()));
            }

            return nodos.Pop();
        }

        private bool es_operador(char c)
        {
            return c == '+' || c == '-' || c == '*' || c == '/' || c == '^';
        }

        private int precedencia_operaciones (char op)
        {
            if (op == '+' || op == '-') return 1;
            if (op == '*' || op == '/') return 2;
            if (op == '^') return 3;

            return 0;
        }

        private Nodo segmentar_operacion(char op, Nodo derecha, Nodo izquierda)
        {
            Nodo nodo = new Nodo(op.ToString());
            nodo.Izquierda = izquierda;
            nodo.Derecha = derecha;
            return nodo;
        }

        public void imprimir_arbol(Nodo nodo, string espaciado = "", bool ultimo_en_nodo = true)
        {
            //Mientras el árbol sea diferente de nulo, imprimir el nodo con sus hijos
            if (nodo != null)
            {
                Console.Write(espaciado);

                //Sí es el último elemento, entonces se dibujan las líneas
                //que unen el par de operaciones
                if (ultimo_en_nodo)
                {
                    Console.Write("└── ");
                    espaciado += "    ";
                }
                else
                {
                    //Caso contrario, se empieza a segmentar las operaciones
                    Console.Write("├── ");
                    espaciado += "|   ";
                }

                //Se muestra el valor del nodo actual
                Console.WriteLine(nodo.Valor);

                imprimir_arbol(nodo.Izquierda, espaciado, false);
                imprimir_arbol(nodo.Derecha, espaciado, true);
            }
        }

        public void mostrar_arbol()
        {
            imprimir_arbol(RAIZ);
        }


        public double evaluar(Dictionary<string, double> valores_incognitas)
        {
            return evaluar_nodo(RAIZ, valores_incognitas);
        }

        private double evaluar_nodo(Nodo nodo, Dictionary<string, double> valores_incognitas)
        {
            //Sí izquierda y derecha es nulo, se trata de un operando que no tiene operaciones
            //por ningún lado
            if (nodo.Izquierda == null && nodo.Derecha == null)
            {
                //Sí el valor del operando es numérico, entonces es un valor "constante"
                if (double.TryParse(nodo.Valor, out double valor))
                {
                    return valor;
                }
                //Caso contrario es una incógnita; por lo tanto, se accede al diccionario
                //para buscar el valor asociado a dicha incógnita
                else if (valores_incognitas.TryGetValue(nodo.Valor, out double valorIncognita))
                {
                    return valorIncognita;
                }
                //Sí no existe un valor asociado a la incógnita mostrar error
                else
                {
                    Console.WriteLine($"Error, el valor de la incógnita {nodo.Valor} no ha sido asignada.");
                    throw new Exception($"Incógnita '{nodo.Valor}' no tiene valor definido.");
                }
            }
            //Sí existen nodos a los lados del evaluado, entonces es un operador (+, -, *, /,...)
            else
            {
                double valor_izquierda = evaluar_nodo(nodo.Izquierda, valores_incognitas);
                double valor_derecha = evaluar_nodo(nodo.Derecha, valores_incognitas);

                switch (nodo.Valor)
                {
                    case "+":
                        Console.WriteLine($"{valor_izquierda} + {valor_derecha}");
                        Console.WriteLine(valor_izquierda + valor_derecha);
                        return valor_izquierda + valor_derecha;
                    case "-":
                        Console.WriteLine($"{valor_izquierda} - {valor_derecha}");
                        Console.WriteLine(valor_izquierda - valor_derecha);
                        return valor_izquierda - valor_derecha;
                    case "*":
                        Console.WriteLine($"{valor_izquierda} * {valor_derecha}");
                        Console.WriteLine(valor_izquierda * valor_derecha);
                        return valor_izquierda * valor_derecha;
                    case "/":
                        if (valor_derecha == 0)
                        {
                            throw new DivideByZeroException("No se puede dividir entre cero.");
                        }
                        Console.WriteLine($"{valor_izquierda} / {valor_derecha}");
                        Console.WriteLine(valor_izquierda / valor_derecha);
                        return valor_izquierda / valor_derecha;
                    case "^":
                        Console.WriteLine($"{valor_izquierda} ^ {valor_derecha}");
                        Console.WriteLine(Math.Pow(valor_izquierda, valor_derecha));
                        return Math.Pow(valor_izquierda, valor_derecha);
                    default:
                        throw new Exception($"Operador desconocido '{nodo.Valor}'.");
                }
            }
        }
    }
}
