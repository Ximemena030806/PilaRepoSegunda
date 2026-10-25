using System;
using System.Collections.Generic;

namespace PilaRepo.Shared
{
    /// <summary>Nodo de la pila dinámica: guarda un dato y el enlace Prev al nodo de abajo.</summary>
    internal class NodoPila<T>
    {
        public T Dato { get; }
        public NodoPila<T>? Prev { get; set; }

        public NodoPila(T dato)
        {
            Dato = dato;
            Prev = null;
        }

        public override string ToString() => Dato?.ToString() ?? string.Empty;
    }

    /// <summary>
    /// PROPIA · LISTA ENLAZADA (sin arreglos): top + Prev, sin límite de tamaño (diapositivas
    /// "Listas enlazadas"). Push: el nodo nuevo apunta con Prev al top anterior y pasa a ser el top.
    /// Pop: el top baja a Prev. Count recorre los nodos con un cursor desde top hasta null (O(n)).
    /// </summary>
    internal class PilaNodos<T> : IPilaVista<T>
    {
        private const string MsgVacia = "La pila está vacía.";
        private NodoPila<T>? _top;

        public bool EsEstatica => false;
        public int Capacidad => 0;
        public int Top => Count() - 1;

        public void Push(T item)
        {
            if (item == null) throw new ArgumentNullException(nameof(item));
            var nuevo = new NodoPila<T>(item);
            nuevo.Prev = _top;     // apunta al que antes era el top
            _top = nuevo;          // el nuevo es el top
        }

        public T Pop()
        {
            NodoPila<T> val = _top ?? throw new InvalidOperationException(MsgVacia);
            _top = val.Prev;
            return val.Dato;
        }

        public T Peek()
        {
            NodoPila<T> top = _top ?? throw new InvalidOperationException(MsgVacia);
            return top.Dato;
        }

        public void Clear() => _top = null;

        public int Count()
        {
            int count = 0;
            NodoPila<T>? cursor = _top;
            while (cursor != null)
            {
                count++;
                cursor = cursor.Prev;
            }
            return count;
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
            for (NodoPila<T>? t = _top; t != null; t = t.Prev) copia.Add(t.Dato);
            copia.Reverse();
            return copia;
        }
    }
}
