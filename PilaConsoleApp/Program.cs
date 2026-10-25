using System;
using System.Text;
using PilaRepo.Shared;

namespace PilaConsoleApp
{
    /// <summary>Lectura de datos desde la consola.</summary>
    internal static class Entrada
    {
        public static string LeerTexto(string mensaje)
        {
            Console.Write(mensaje);
            return Console.ReadLine() ?? string.Empty;
        }

        public static int LeerEntero(string mensaje)
        {
            while (true)
            {
                Console.Write(mensaje);
                if (int.TryParse(Console.ReadLine(), out int v)) return v;
                Console.WriteLine("Número inválido, intenta de nuevo.");
            }
        }
    }

    internal static class Program
    {
        private static void Main()
        {
            Console.OutputEncoding = Encoding.UTF8;
            bool salir = false;

            while (!salir)
            {
                IMenuPila? pila = ElegirModo();          // Menú 1
                if (pila == null) return;                // "Salir"

                bool cambiarModo = false;
                while (!cambiarModo && !salir)           // Menú 2
                {
                    MostrarMenuOperaciones(pila.Nombre);
                    string opcion = (Console.ReadLine() ?? "").Trim();
                    try
                    {
                        switch (opcion)
                        {
                            case "1": pila.Push(); break;
                            case "2": pila.Pop(); break;
                            case "3": pila.Peek(); break;
                            case "4": pila.Count(); break;
                            case "5": pila.Clear(); break;
                            case "6": pila.Contains(); break;
                            case "7": pila.Ver(); break;
                            case "8": cambiarModo = true; break;
                            case "9": salir = true; break;
                            default: Console.WriteLine("Opción no válida."); break;
                        }
                    }
                    catch (InvalidOperationException ex)   // Stack Overflow / Stack Underflow
                    {
                        Console.WriteLine($"ERROR: {ex.Message}");
                    }
                }
            }
        }

        // ---------------- Menú 1: elegir la pila de libros ----------------
        private static IMenuPila? ElegirModo()
        {
            while (true)
            {
                Console.WriteLine();
                Console.WriteLine("===== PILA DE LIBROS: elige la implementación =====");
                Console.WriteLine("-- Ambas en ARREGLOS --");
                Console.WriteLine("1. Arreglo propio (T[], tamaño fijo)");
                Console.WriteLine("2. Stack<T> (nativa)");
                Console.WriteLine("-- Ambas en LISTAS --");
                Console.WriteLine("3. Nodos propios (lista enlazada con Prev)");
                Console.WriteLine("4. LinkedList<T> (nativa)");
                Console.WriteLine("-- Ambas en LISTAS con ARREGLOS --");
                Console.WriteLine("5. Lista con arreglo propia (arreglo que crece solo)");
                Console.WriteLine("6. List<T> (nativa)");
                Console.WriteLine("7. Salir");
                Console.Write("Opción: ");

                IPilaVista<Libro> pila;
                string nombre;
                switch ((Console.ReadLine() ?? "").Trim())
                {
                    case "1":
                        int tam = Entrada.LeerEntero("Tamaño del arreglo (mínimo 3, ya trae 3 libros precargados): ");
                        if (tam < 3) { Console.WriteLine("El tamaño debe ser al menos 3."); continue; }
                        pila = FabricaPilas.ArregloPropia(tam);
                        nombre = $"Arreglo propio T[] (tamaño {tam})";
                        break;
                    case "2":
                        pila = FabricaPilas.StackNativa();
                        nombre = "Stack<T> nativa";
                        break;
                    case "3":
                        pila = FabricaPilas.NodosPropia();
                        nombre = "Nodos propios (lista enlazada)";
                        break;
                    case "4":
                        pila = FabricaPilas.LinkedListNativa();
                        nombre = "LinkedList<T> nativa";
                        break;
                    case "5":
                        pila = FabricaPilas.ListaArregloPropia();
                        nombre = "Lista con arreglo propia";
                        break;
                    case "6":
                        pila = FabricaPilas.ListaNativa();
                        nombre = "List<T> nativa";
                        break;
                    case "7":
                        return null;
                    default:
                        Console.WriteLine("Opción no válida.");
                        continue;
                }

                return new AdaptadorPila<Libro>(nombre, pila,
                    () => new Libro(Entrada.LeerTexto("Título: "), Entrada.LeerTexto("Autor: ")),
                    () => new Libro(Entrada.LeerTexto("Título a buscar: "), ""),
                    l => $"{l.Titulo} / {l.Autor}");
            }
        }

        // ---------------- Menú 2: operaciones ----------------
        private static void MostrarMenuOperaciones(string nombre)
        {
            Console.WriteLine();
            Console.WriteLine($"===== {nombre} =====");
            Console.WriteLine("1. Push");
            Console.WriteLine("2. Pop");
            Console.WriteLine("3. Peek");
            Console.WriteLine("4. Count");
            Console.WriteLine("5. Clear");
            Console.WriteLine("6. Contains");
            Console.WriteLine("7. Ver pila completa");
            Console.WriteLine("8. Cambiar de modo");
            Console.WriteLine("9. Salir");
            Console.Write("Opción: ");
        }
    }
}
