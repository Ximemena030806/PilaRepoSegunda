using System;
using System.Collections.Generic;

namespace PilaRepo.Shared
{
    /// <summary>
    /// NATIVA · LISTA ENLAZADA (sin arreglos): System.Collections.Generic.LinkedList&lt;T&gt;.
    /// Push = AddLast, Pop = RemoveLast, Peek = Last. El tope es el ÚLTIMO nodo.
    /// Todo O(1) estricto y sin Overflow; cada elemento gasta un nodo con Next y Prev.
    /// </summary>
    internal class PilaLinkedList<T> : IPilaVista<T>
    {
        private const string MsgVacia = "La pila está vacía.";
        private readonly LinkedList<T> _lista = new LinkedList<T>();

        public bool EsEstatica => false;
        public int Capacidad => 0;
        public int Top => _lista.Count - 1;

        public int Count() => _lista.Count;

        public void Push(T item)
        {
            if (item == null) throw new ArgumentNullException(nameof(item));
            _lista.AddLast(item);
        }

        public T Pop()
        {
            if (_lista.Last == null) throw new InvalidOperationException(MsgVacia);
            T tope = _lista.Last.Value;
            _lista.RemoveLast();
            return tope;
        }

        public T Peek()
        {
            if (_lista.Last == null) throw new InvalidOperationException(MsgVacia);
            return _lista.Last.Value;
        }

        public void Clear() => _lista.Clear();

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

        public List<T> ObtenerCopia() => new List<T>(_lista);
    }
}
