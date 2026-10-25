using System;

namespace PilaRepo.Shared
{
    /// <summary>
    /// Punto único donde se crean las 6 pilas de LIBROS, todas PRECARGADAS con los mismos 3 libros
    /// (el último, Cien Años de Soledad, queda como tope).
    ///
    ///                 PROPIA                    NATIVA
    ///   ARREGLO       PilaArreglo (T[] fijo)    PilaStack (Stack&lt;T&gt;)
    ///   LISTA         PilaNodos (nodos+Prev)    PilaLinkedList (LinkedList&lt;T&gt;)
    ///   LISTA+ARREGLO PilaListaArreglo          PilaLista (List&lt;T&gt;)
    /// </summary>
    internal static class FabricaPilas
    {
        private static IPilaVista<Libro> Cargar(IPilaVista<Libro> pila)
        {
            pila.Push(new Libro("El Principito", "A. de Saint-Exupéry"));
            pila.Push(new Libro("1984", "George Orwell"));
            pila.Push(new Libro("Cien Años de Soledad", "G. García Márquez"));
            return pila;
        }

        /// <summary>Propia con arreglo de tamaño fijo. Mínimo 3, porque ya trae 3 libros.</summary>
        public static IPilaVista<Libro> ArregloPropia(int tamaño = 5) => Cargar(new PilaArreglo<Libro>(tamaño));

        /// <summary>Nativa con arreglo: Stack&lt;T&gt;.</summary>
        public static IPilaVista<Libro> StackNativa() => Cargar(new PilaStack<Libro>());

        /// <summary>Propia con lista enlazada (nodos con Prev, sin arreglos).</summary>
        public static IPilaVista<Libro> NodosPropia() => Cargar(new PilaNodos<Libro>());

        /// <summary>Nativa con lista enlazada (sin arreglos): LinkedList&lt;T&gt;.</summary>
        public static IPilaVista<Libro> LinkedListNativa() => Cargar(new PilaLinkedList<Libro>());

        /// <summary>Propia con lista sobre un arreglo que crece solo.</summary>
        public static IPilaVista<Libro> ListaArregloPropia() => Cargar(new PilaListaArreglo<Libro>());

        /// <summary>Nativa con lista y arreglo interno: List&lt;T&gt;.</summary>
        public static IPilaVista<Libro> ListaNativa() => Cargar(new PilaLista<Libro>());
    }
}
