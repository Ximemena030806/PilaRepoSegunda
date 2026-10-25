using System.Collections.Generic;

namespace PilaRepo.Shared
{
    /// <summary>
    /// Lo que tienen en común las 6 pilas de libros (3 propias y 3 nativas). Así la interfaz
    /// gráfica y la consola las usan todas con el mismo código.
    /// Count es MÉTODO, como en la diapositiva 100 (public int Count()).
    /// </summary>
    internal interface IPilaVista<T>
    {
        void Push(T item);
        T Pop();
        T Peek();
        int Count();
        bool Contains(T item);
        void Clear();

        /// <summary>Copia de los elementos en orden base -> tope (el último es el tope).</summary>
        List<T> ObtenerCopia();

        /// <summary>true solo en la pila de arreglo de tamaño fijo.</summary>
        bool EsEstatica { get; }

        /// <summary>Tamaño fijo del arreglo (solo la de arreglo fijo); 0 en las demás.</summary>
        int Capacidad { get; }

        /// <summary>Índice del tope (-1 si está vacía).</summary>
        int Top { get; }
    }
}
