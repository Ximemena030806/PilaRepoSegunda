using System;
using System.Drawing;
using System.Windows.Forms;
using PilaRepo.Shared;

namespace PilaWinForms_Tabs
{
    /// <summary>
    /// Ventana principal: 7 pestañas. Las 6 primeras son la misma pila de LIBROS en 6 formas
    /// (parejas propia / nativa) y la última compara las 6.
    ///
    ///   Ambas en ARREGLOS:          Arreglo (propia)          · Stack&lt;T&gt; (nativa)
    ///   Ambas en LISTAS:            Nodos (propia)            · LinkedList&lt;T&gt; (nativa)
    ///   Ambas en LISTAS + ARREGLOS: Lista con arreglo (propia) · List&lt;T&gt; (nativa)
    /// </summary>
    internal class MainForm : Form
    {
        private readonly TabControl _tabs = new TabControl { Dock = DockStyle.Fill };

        public MainForm()
        {
            Text = "Pila de libros: propias y nativas, en arreglos y en listas";
            ClientSize = new Size(1200, 660);
            MinimumSize = new Size(1000, 560);
            StartPosition = FormStartPosition.CenterScreen;

            // Pareja 1: arreglos
            _tabs.TabPages.Add(CrearTabArreglo());
            _tabs.TabPages.Add(NuevaPagina("Stack<T> (nativa)", NuevoPanelLibros(FabricaPilas.StackNativa())));
            // Pareja 2: listas
            _tabs.TabPages.Add(NuevaPagina("Nodos (propia)", NuevoPanelLibros(FabricaPilas.NodosPropia())));
            _tabs.TabPages.Add(NuevaPagina("LinkedList<T> (nativa)", NuevoPanelLibros(FabricaPilas.LinkedListNativa())));
            // Pareja 3: listas con arreglos
            _tabs.TabPages.Add(NuevaPagina("Lista con arreglo (propia)", NuevoPanelLibros(FabricaPilas.ListaArregloPropia())));
            _tabs.TabPages.Add(NuevaPagina("List<T> (nativa)", NuevoPanelLibros(FabricaPilas.ListaNativa())));
            // Comparativa de las 6
            _tabs.TabPages.Add(NuevaPagina("Comparación", new PanelComparacion()));

            // Al cambiar de pestaña se refresca la grilla visible.
            _tabs.SelectedIndexChanged += (s, e) =>
            {
                if (_tabs.SelectedTab != null)
                    foreach (Control c in _tabs.SelectedTab.Controls)
                        if (c is IRefrescable r) r.Refrescar();
            };

            Controls.Add(_tabs);
        }

        private static TabPage NuevaPagina(string titulo, Control contenido)
        {
            var tp = new TabPage(titulo);
            tp.Controls.Add(contenido);
            return tp;
        }

        // Panel de una pila de libros: PRECARGADA con 3 libros (el último es el tope).
        private static PanelPila<Libro> NuevoPanelLibros(IPilaVista<Libro> pila) =>
            new PanelPila<Libro>(pila,
                new[] { "Título", "Autor" }, new[] { "Título:", "Autor:" },
                new[] { "Don Quijote de la Mancha", "Miguel de Cervantes" },
                c => new Libro(c[0], c[1]),
                c => new Libro(c[0], ""),                  // Contains busca solo por título
                l => new[] { l.Titulo, l.Autor },
                "Contains (por título)");

        // Pila propia de arreglo: tamaño fijo, con selector de tamaño (mínimo 3 por los 3 libros).
        private static TabPage CrearTabArreglo()
        {
            var panel = NuevoPanelLibros(FabricaPilas.ArregloPropia());

            var num = new NumericUpDown { Minimum = 3, Maximum = 50, Value = 5, Width = 60, Margin = new Padding(3, 5, 3, 3) };
            panel.AgregarEncabezado(UiHelper.Etiqueta("Tamaño del arreglo:"));
            panel.AgregarEncabezado(num);
            panel.AgregarEncabezado(UiHelper.Boton("Crear / Reiniciar Pila", (s, e) =>
            {
                panel.PilaActual = FabricaPilas.ArregloPropia((int)num.Value);
                panel.Estado($"Pila de arreglo creada: tamaño {(int)num.Value} (índices 0-{(int)num.Value - 1}), con 3 libros precargados");
            }, 160));

            return NuevaPagina("Arreglo (propia)", panel);
        }
    }
}
