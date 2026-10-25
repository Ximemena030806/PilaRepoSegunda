# PilaRepo — Pila de libros en C# / .NET 8

Una sola clase de datos: **`Libro`** (Título y Autor; dos libros son iguales si tienen el mismo título).
La misma pila se implementa de **6 formas**, en 3 parejas: una **propia** (la programamos nosotros) y una
**nativa** (la del framework de .NET).

| Pareja | Propia | Nativa |
|---|---|---|
| **Ambas en ARREGLOS** | `PilaArreglo<T>`: arreglo `T[]` de tamaño fijo + `topIndex` | `PilaStack<T>`: `Stack<T>` (arreglo interno que crece solo) |
| **Ambas en LISTAS** | `PilaNodos<T>`: nodos enlazados con `Prev` y `top`, sin arreglos | `PilaLinkedList<T>`: `LinkedList<T>`, sin arreglos |
| **Ambas en LISTAS con ARREGLOS** | `PilaListaArreglo<T>`: lista hecha sobre un arreglo que duplica su tamaño al llenarse | `PilaLista<T>`: `List<T>` |

Todas arrancan **precargadas con los mismos 3 libros** (El Principito, 1984 y Cien Años de Soledad = tope)
y tienen las mismas operaciones: `Push`, `Pop`, `Peek`, `Count()`, `Clear` y `Contains`
(`Contains` recorre una **copia** con el cursor `current`, de la cima a la base, como en las diapositivas).

## Diferencias que se pueden observar

| Pila | Overflow | Push | Count | Memoria |
|---|---|---|---|---|
| Arreglo propio | **Sí**: lanza `La pila estática está llena (Overflow).` | O(1) estricto | `top + 1`, O(1) | arreglo fijo |
| `Stack<T>` | No | O(1) amortizado | O(1) | arreglo que crece |
| Nodos propios | No | O(1) estricto | recorre los nodos, O(n) | un nodo por libro |
| `LinkedList<T>` | No | O(1) estricto | O(1) | un nodo por libro (Next y Prev) |
| Lista con arreglo propia | No | O(1) amortizado (copia al duplicar) | O(1) | arreglo que crece |
| `List<T>` | No | O(1) amortizado | O(1) | arreglo que crece |

## Estructura

```
PilaRepo/
├── PilaRepo.sln
├── Shared/                       (una sola copia, la usan los dos proyectos)
│   ├── Libro.cs
│   ├── IPilaVista.cs             operaciones comunes de las 6 pilas
│   ├── PilaArreglo.cs            propia · arreglo
│   ├── PilaStack.cs              nativa · arreglo (Stack<T>)
│   ├── PilaNodos.cs              propia · lista enlazada
│   ├── PilaLinkedList.cs         nativa · lista enlazada (LinkedList<T>)
│   ├── PilaListaArreglo.cs       propia · lista con arreglo
│   ├── PilaLista.cs              nativa · lista con arreglo (List<T>)
│   └── FabricaPilas.cs           crea las 6 pilas precargadas
├── PilaWinForms_Tabs/            Windows Forms (net8.0-windows), 7 pestañas
└── PilaConsoleApp/               Consola (net8.0), dos menús
```

## Interfaz

* **WinForms (7 pestañas):** Arreglo (propia) · Stack&lt;T&gt; (nativa) · Nodos (propia) · LinkedList&lt;T&gt; (nativa) ·
  Lista con arreglo (propia) · List&lt;T&gt; (nativa) · **Comparación**.
  Cada pestaña tiene un botón por operación; cada uno refresca la grilla y muestra la acción hecha (o el error en
  rojo). La de arreglo permite elegir el tamaño (mínimo 3). La pestaña **Comparación** aplica la misma operación
  (Push, Pop, Peek, Contains, Count, Clear, Llenar N) a las 6 pilas a la vez y muestra el resultado y el tiempo
  de cada una; con *Llenar N* se ve cómo solo el arreglo propio se desborda.
* **Consola:** Menú 1 (elegir una de las 6 pilas, agrupadas por pareja) y Menú 2 (Push, Pop, Peek, Count, Clear,
  Contains, Ver pila, Cambiar de modo, Salir).

## Compilar y ejecutar

```
dotnet build PilaRepo.sln
dotnet run --project PilaConsoleApp
dotnet run --project PilaWinForms_Tabs      # solo en Windows
```
(En Visual Studio 2022: abrir `PilaRepo.sln`.)
