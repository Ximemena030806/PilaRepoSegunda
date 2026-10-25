using System;
using System.Collections.Generic;

namespace PilaRepo.Shared
{
    /// <summary>
    /// PROPIA · LISTA CON ARREGLO: una lista hecha por nosotros sobre un arreglo T[] que CRECE
    /// solo (es la idea interna de List&lt;T&gt;). Guarda el arreglo y un contador _count.
    /// Push: si el arreglo se llenó, crea uno del DOBLE de tamaño y copia todo (O(n) de vez en
    /// cuando, O(1) amortizado). Por eso nunca hay Overflow, a diferencia de PilaArreglo.
    /// </summary>
    internal class PilaListaArreglo<T> : IPilaVista<T>
    {
        private const string MsgVacia = "La pila está vacía.";
        private const int CapacidadInicial = 4;

        private T[] _datos = new T[CapacidadInicial];
        private int _count;

        public bool EsEstatica => false;
        public int Capacidad => 0;
        public int Top => _count - 1;

        /// <summary>Tamaño actual del arreglo interno (informativo; crece 4 -> 8 -> 16 ...).</summary>
        public int CapacidadInterna => _datos.Length;

        public int Count() => _count;

        public void Push(T item)
        {
            if (item == null) throw new ArgumentNullException(nameof(item));
            if (_count == _datos.Length) Redimensionar();
            _datos[_count] = item;
            _count++;
        }

        private void Redimensionar()
        {
            T[] nuevo = new T[_datos.Length * 2];   // arreglo del doble de tamaño
            Array.Copy(_datos, nuevo, _count);      // copia lo que ya había
            _datos = nuevo;
        }

        public T Pop()
        {
            if (_count == 0) throw new InvalidOperationException(MsgVacia);
            _count--;
            T val = _datos[_count];
            _datos[_count] = default!;              // suelta la referencia
            return val;
        }

        public T Peek()
        {
            if (_count == 0) throw new InvalidOperationException(MsgVacia);
            return _datos[_count - 1];
        }

        public void Clear()
        {
            Array.Clear(_datos, 0, _datos.Length);
            _count = 0;
        }

        public bool Contains(T item)
        {
            List<T> copia = ObtenerCopia();
            int current = copia.Count - 1;
            while (current >= 0)
            {
                if (EqualityComparer<T>.Default.Equals(copia[current], item)) return true;
                current--;
            }
            return false;
        }

        public List<T> ObtenerCopia()
        {
            var copia = new List<T>();
            for (int i = 0; i < _count; i++) copia.Add(_datos[i]);
            return copia;
        }
    }
}
