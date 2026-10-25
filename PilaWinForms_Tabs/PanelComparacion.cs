using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using PilaRepo.Shared;

namespace PilaWinForms_Tabs
{
    /// <summary>
    /// Pestaña de comparación: las SEIS pilas de libros lado a lado (3 propias y 3 nativas), cada
    /// una con mini-grilla, top y resultado. Los botones aplican la misma operación a las seis y se
    /// muestra cuál falló y por qué (Overflow / Underflow), con el tiempo de cada una.
    /// Los libros se generan como "Libro N" para poder hacer Push / Contains rápido.
    /// </summary>
    internal class PanelComparacion : TableLayoutPanel, IRefrescable
    {
        private static readonly string[] Nombres =
        {
            "Arreglo (propia)", "Stack<T> (nativa)",
            "Nodos (propia)", "LinkedList<T> (nativa)",
            "Lista+arreglo (propia)", "List<T> (nativa)"
        };

        private readonly IPilaVista<Libro>[] _pilas = new IPilaVista<Libro>[6];
        private readonly DataGridView[] _grids = new DataGridView[6];
        private readonly Label[] _counts = new Label[6];
        private readonly Label[] _errs = new Label[6];

        private readonly NumericUpDown _numLibro = new NumericUpDown { Minimum = 1, Maximum = 100000, Value = 4, Width = 70 };
        private readonly NumericUpDown _numLlenar = new NumericUpDown { Minimum = 1, Maximum = 1000, Value = 4, Width = 60 };
        private readonly NumericUpDown _numTam = new NumericUpDown { Minimum = 3, Maximum = 50, Value = 5, Width = 60 };
        private readonly Label _lblEstado;

        public PanelComparacion()
        {
            CrearPilas();

            Dock = DockStyle.Fill;
            Padding = new Padding(8);
            ColumnCount = 1;
            RowCount = 3;
            ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));
            RowStyles.Add(new RowStyle(SizeType.AutoSize));          // controles
            RowStyles.Add(new RowStyle(SizeType.Percent, 100));      // las seis pilas
            RowStyles.Add(new RowStyle(SizeType.Absolute, 100));     // estado general

            var controles = UiHelper.NuevoFlow();
            controles.Controls.Add(UiHelper.Etiqueta("Libro N°:"));
            _numLibro.Margin = new Padding(3, 5, 3, 3);
            controles.Controls.Add(_numLibro);
            controles.Controls.Add(UiHelper.Boton("Push en las 6", (s, e) => OnPush(), 120));
            controles.Controls.Add(UiHelper.Boton("Pop en las 6", (s, e) => OnPop(), 120));
            controles.Controls.Add(UiHelper.Boton("Peek en las 6", (s, e) => OnPeek(), 120));
            controles.Controls.Add(UiHelper.Boton("Contains en las 6", (s, e) => OnContains(), 130));
            controles.Controls.Add(UiHelper.Boton("Count en las 6", (s, e) => OnCount(), 120));
            controles.Controls.Add(UiHelper.Boton("Clear en las 6", (s, e) => OnClear(), 120));
            controles.Controls.Add(UiHelper.Etiqueta("Llenar con N:"));
            _numLlenar.Margin = new Padding(3, 5, 3, 3);
            controles.Controls.Add(_numLlenar);
            controles.Controls.Add(UiHelper.Boton("Llenar N en las 6", (s, e) => OnLlenar(), 130));
            controles.Controls.Add(UiHelper.Etiqueta("Tamaño del arreglo:"));
            _numTam.Margin = new Padding(3, 5, 3, 3);
            controles.Controls.Add(_numTam);
            controles.Controls.Add(UiHelper.Boton("Reiniciar las 6", (s, e) => OnReiniciar(), 120));

            var seis = new TableLayoutPanel { Dock = DockStyle.Fill, ColumnCount = 6, RowCount = 1 };
            for (int i = 0; i < 6; i++) seis.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100f / 6));
            seis.RowStyles.Add(new RowStyle(SizeType.Percent, 100));
            for (int i = 0; i < 6; i++)
                seis.Controls.Add(CrearGrupo(i), i, 0);

            _lblEstado = new Label { Dock = DockStyle.Fill, AutoSize = false };

            Controls.Add(controles, 0, 0);
            Controls.Add(seis, 0, 1);
            Controls.Add(_lblEstado, 0, 2);

            Refrescar();
        }

        /// <summary>Las 6 pilas, cada una precargada con los mismos 3 libros.</summary>
        private void CrearPilas()
        {
            _pilas[0] = FabricaPilas.ArregloPropia((int)_numTam.Value);
            _pilas[1] = FabricaPilas.StackNativa();
            _pilas[2] = FabricaPilas.NodosPropia();
            _pilas[3] = FabricaPilas.LinkedListNativa();
            _pilas[4] = FabricaPilas.ListaArregloPropia();
            _pilas[5] = FabricaPilas.ListaNativa();
        }

        private GroupBox CrearGrupo(int i)
        {
            var grupo = new GroupBox { Text = Nombres[i], Dock = DockStyle.Fill, Padding = new Padding(4) };
            var t = new TableLayoutPanel { Dock = DockStyle.Fill, ColumnCount = 1, RowCount = 3 };
            t.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));
            t.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            t.RowStyles.Add(new RowStyle(SizeType.Percent, 100));
            t.RowStyles.Add(new RowStyle(SizeType.Absolute, 64));

            _counts[i] = new Label { AutoSize = true, Margin = new Padding(3) };
            _counts[i].Font = new Font(_counts[i].Font, FontStyle.Bold);
            _grids[i] = UiHelper.CrearGrid("Libro");
            _errs[i] = new Label { Dock = DockStyle.Fill, AutoSize = false };

            t.Controls.Add(_counts[i], 0, 0);
            t.Controls.Add(_grids[i], 0, 1);
            t.Controls.Add(_errs[i], 0, 2);
            grupo.Controls.Add(t);
            return grupo;
        }

        public void Refrescar()
        {
            for (int i = 0; i < _pilas.Length; i++)
            {
                IPilaVista<Libro> pila = _pilas[i];
                UiHelper.LlenarGrid(_grids[i], pila.ObtenerCopia(), pila.EsEstatica ? pila.Capacidad : 0,
                                    l => new[] { l.Titulo }, 1);
                _counts[i].Text = pila.EsEstatica
                    ? $"top = {pila.Top}  (tamaño {pila.Capacidad})"
                    : $"top = {pila.Top}";
            }
        }

        private static Libro Crear(int n) => new Libro($"Libro {n}", $"Autor {n}");

        /// <summary>Aplica la operación a las seis pilas y reporta cuál falló y por qué.</summary>
        private void EnLasSeis(string operacion, Func<IPilaVista<Libro>, string> accion)
        {
            var partes = new List<string>();
            bool hayError = false;

            for (int i = 0; i < _pilas.Length; i++)
            {
                var cron = System.Diagnostics.Stopwatch.StartNew();
                try
                {
                    string r = accion(_pilas[i]);
                    cron.Stop();
                    string t = $"{cron.Elapsed.TotalMilliseconds:0.000} ms";
                    _errs[i].ForeColor = Color.DarkGreen;
                    _errs[i].Text = $"OK: {r} [{t}]";
                    partes.Add($"{Nombres[i]}: OK ({t})");
                }
                catch (InvalidOperationException ex)       // Overflow / Underflow
                {
                    cron.Stop();
                    hayError = true;
                    _errs[i].ForeColor = Color.Red;
                    _errs[i].Text = "FALLÓ: " + ex.Message;
                    partes.Add($"{Nombres[i]}: FALLÓ -> {ex.Message}");
                }
            }

            Refrescar();
            UiHelper.MostrarEstado(_lblEstado, $"{operacion} en las 6:  " + string.Join("   |   ", partes), hayError);
        }

        private void OnPush()
        {
            int n = (int)_numLibro.Value;
            EnLasSeis($"Push(Libro {n})", p => { p.Push(Crear(n)); return $"Push(Libro {n})"; });
            if (_numLibro.Value < _numLibro.Maximum) _numLibro.Value += 1;   // siguiente libro para la demo
        }

        private void OnPop() => EnLasSeis("Pop()", p => $"Pop() = {p.Pop().Titulo}");

        private void OnPeek() => EnLasSeis("Peek()", p => $"Peek() = {p.Peek().Titulo}");

        private void OnContains()
        {
            int n = (int)_numLibro.Value;
            EnLasSeis($"Contains(Libro {n})", p => $"Contains = {p.Contains(new Libro($"Libro {n}", ""))}");
        }

        private void OnCount() => EnLasSeis("Count", p => $"Count = {p.Count()}");

        private void OnClear() => EnLasSeis("Clear()", p => { p.Clear(); return "Clear()"; });

        /// <summary>N Push seguidos en cada pila: la de arreglo se desborda al llegar a su tamaño.</summary>
        private void OnLlenar()
        {
            int n = (int)_numLlenar.Value;
            int inicio = (int)_numLibro.Value;
            EnLasSeis($"Llenar {n}", p =>
            {
                for (int i = 0; i < n; i++) p.Push(Crear(inicio + i));
                return $"{n} Push";
            });
        }

        private void OnReiniciar()
        {
            CrearPilas();
            foreach (Label l in _errs) l.Text = "";
            Refrescar();
            UiHelper.MostrarEstado(_lblEstado,
                $"Las 6 pilas se reiniciaron con 3 libros (el arreglo de tamaño {_pilas[0].Capacidad}).", false);
        }
    }
}
