# TechLabs
## Лабораторная работа 1
### Point 1
```cpp
#include "pch.h"
#include "framework.h"

BOOL APIENTRY DllMain( HMODULE hModule,
                       DWORD  ul_reason_for_call,
                       LPVOID lpReserved
                     )
{
    switch (ul_reason_for_call)
    {
    case DLL_PROCESS_ATTACH:
    case DLL_THREAD_ATTACH:
    case DLL_THREAD_DETACH:
    case DLL_PROCESS_DETACH:
        break;
    }
    return TRUE;
}

#include <iostream>

extern "C"
{
    __declspec (dllexport) void __stdcall Java_MyClass_MyMethod()
    {
        std::cout << "Hello Java from C++";
    }

    __declspec (dllexport) void MyMethod()
    {
        std::cout << "Hello C# from C++";
    }
}
```
Для начала, нужно создать срр файл и подключить dll библиотеку, а также перевести конфигурацию с Debug на Release. В конце исходника нужно написать extern "C", а внутри методы, которые мы хотим чтоб вызывались в Java и C#. "__declspec (dllexport)" - требование для каждого dll метода. Без этой приписки работать не будет. "__stdcall" - требование для Java, чтобы вызвать метод из dll. Лично у меня метод называется с припиской Java_"имя класса". В источнике, с которого я брал информацию, было сказано, что это тоже требование для вызова в Java, однако я знаю людей, у которых работало и без этого. В C# приписка "__stdcall" необязательна. 

#### Для работы с Java:
Нужно сбилдить проект и скопировать dll файл (не наш cpp файл, а именно dll) в проект в IDE, но только не внутрь src или каких-то других папок. Код в Java выглядит так:
```java
public class MyClass {
    public native void MyMethod();

    static
    {
        System.loadLibrary("Dll1");
    }
    public static void main(String args[]) {
        new MyClass().MyMethod();
    }
}
```
Мы объявляем метод срр файла с модификатором native как раз для того, чтобы вызывать его из dll. Через System.loadLibrary("имя dll") загружаем библиотеку и в main вызываем метод.

#### Для работы с C#:
Нужно сбилдить проект и скопировать путь на dll файл, затем импортировать dll: в DllImport указать этот путь и указать CallingConvention = Cdecl, иначе выкинется ошибка. У метода ставится модификатор extern (как в Java native). Код выглядит так:
```cs
using System;
using System.Runtime.InteropServices;

namespace CPPInterop
{
    internal static class Program
    {
        private const string _dllPath = @"C:\Users\volok\source\repos\InteropCPP\x64\Release\Dll1";

        [DllImport(_dllPath, CallingConvention = CallingConvention.Cdecl)]

        public static extern void MyMethod();
        
        private static void Main()
        {
            MyMethod();
        }
    }
}
```

Из сложностей работы с интеропом особо ничего не могу выделить - у меня проблем не было. Ограничений работы я тоже не увидел.
Источники информации:
https://www.youtube.com/watch?v=o-ass4mkdiA&t=0s
https://www.youtube.com/watch?v=41leCIAzSd0&t=0s

### Point 2
#### F# и декомпиляция в C#:
Код на F#:
```fs
let multi x y = x * y
let plus x y = x + y
type IntOrBool =
    | I of int
    | B of bool
let result x y z = multi x y |> plus z
let unionTrial x =
    let i = I x
    let b = B (x % 2 = 0)
    printfn $"%d{x}
    printfn $"{i} is odd?"
    printfn $"{b}!"
```
Честно, разбирался с Computation Expressions, но мало что понял, поэтому код очень простой и написан с использованием Pipe Operator и Discriminated Union. Его вызов в C#:
```cs
using System;

namespace CSharpTesting
{
    internal static class Program
    {
        private static void Main()
        {
            var res = global::Program.result(1, 2, 3);
            global::Program.unionTrial(res);
        }
    }
}
```
Для вызова F# методов в C# требуется только вызвать их с global(и то, насколько я знаю, у некоторых работало и без него). Чтобы декомпилировать в C# я использовал dotPeek.
При декомпиляции в C# Discriminated Union превращается в абстрактный класс, и из-за этого код значительно раздувается. Сам код декомпиляции лежит в [SharpDecompiler.md](SharpDecompiler.md)
#### Scala и декомпиляция в Java:
Код на Scala:
```scala
import scala.util.chaining.scalaUtilChainingOps

class ScalaTrial {
  def multi(x : Int, y : Int): Int = x * y
  def plus(x : Int, y : Int): Int = x + y

  def pipe() : Int = {
    val x = 2.pipe(plus(_, 3)).pipe(multi(_, 2))
    x
  }
}
```
Что касается Scala, то, насколько я понял, там нет ни Discriminated Union, ни Computation Expressions, поэтому я написал максимально простой код с использованием .pipe(). Вызов кода в Java:
```java
public class JavaTesting {
    public static void main(String[] args) {
        ScalaTrial myObj = new ScalaTrial();
        int x = myObj.pipe();
        System.out.println(x);
    }
}
```
Как видно, ничего не мешает просто вызвать Scala метод в Java. Декомпилировал я с помощью View->Show Bytecode в IntelliJ. Код декомпиляции лежит в [JavaDecompiler.md](JavaDecompiler.md), и по факту декомпилятор просто подключил модуль Scala и использовал .pipe().
### Point 3
#### Работа с C#
DFS и BFS:
```cs
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CSharpGraphFs
{
    public class Graph
    {
        private int _V;
    
        private LinkedList<int>[] _adj;
        public Graph(int V)
        {
            _adj = new LinkedList<int>[V];
            for(var i = 0; i < _adj.Length; i++)
            {
                _adj[i] = new LinkedList<int>();
            }
            _V = V;
        }
         
    
        public void AddEdge(int v, int w)
        {        
            _adj[v].AddLast(w);
         
        }
        
        public void DFSUtil(int v, bool[] visited)
        {
            visited[v] = true;
            Console.Write(v + " ");
            var vList = _adj[v];
            foreach (var n in vList.Where(n => !visited[n]))
            {
                DFSUtil(n, visited);
            }
        }
 
        public void DFS(int v)
        {
            var visited = new bool[_V];
            DFSUtil(v, visited);
        }
        
        public void BFS(int s)
        {
            
            var visited = new bool[_V];
            for(var i = 0; i < _V; i++)
                visited[i] = false;
   
            var queue = new LinkedList<int>();
            visited[s] = true;
            queue.AddLast(s);        
         
            while(queue.Any())
            {
                s = queue.First();
                Console.Write(s + " " );
                queue.RemoveFirst();
                var list = _adj[s];
         
                foreach (var val in list.Where(val => !visited[val]))
                {
                    visited[val] = true;
                    queue.AddLast(val);
                }
            }
        }
    }
}
```
Собрать пакет можно с помощью NuGet. Для этого надо нажать пкм по проекту, выбрать Advanced Build Actions -> Pack Selected Project. После этого зайти в NuGet, нажать на sources, а затем New feed, а путь указать до Debug нашего проекта. Все готово, можно использовать пакет в другом проекте, добавив в начале using:
```cs
using CSharpGraphFs;

namespace PackageUsing
{
    internal static class Program
    {
        private static void Main()
        {
            Graph graph = new Graph(6);
            graph.AddEdge(0, 1);
            graph.AddEdge(0, 2);
            graph.AddEdge(1, 0);
            graph.AddEdge(1, 3);
            graph.AddEdge(2, 0);
            graph.AddEdge(2, 3);
            graph.AddEdge(3, 4);
            graph.AddEdge(3, 5);
            graph.AddEdge(4, 3);
            graph.AddEdge(5, 3);

            graph.DFS(0);
            Console.WriteLine('\n');
            graph.BFS(0);
        }
    }
}
```
Для себя могу отметить сложность, которая у меня возникала: если обходы графа были изначально не совсем правильно написаны, а мы уже запаковали, то я не нашёл другого способа, кроме как пересоздать проект и запаковать заново.
#### Работа с Java
DFS и BFS:
```java
import java.util.Iterator;
import java.util.LinkedList;
import java.util.Stack;

public class JavaGraph {
    int V;

    LinkedList<Integer>[] adj;

    JavaGraph(int V)
    {
        this.V = V;
        adj = new LinkedList[V];
        for (int i = 0; i < adj.length; i++)
            adj[i] = new LinkedList<Integer>();
    }

    void addEdge(int v, int w)
    {
        adj[v].add(w);
    }

    void DFS(int n)
    {
        boolean[] nodes = new boolean[V];
        Stack<Integer> stack = new Stack<>();
        stack.push(n);
        int a = 0;
        while(!stack.empty())
        {
            n = stack.peek();
            stack.pop();
            if(!nodes[n])
            {
                System.out.print(n + " ");
                nodes[n] = true;
            }
            for (int i = 0; i < adj[n].size(); i++)
            {
                a = adj[n].get(i);
                if (!nodes[a])
                {
                    stack.push(a);
                }
            }
        }
    }

    void BFS(int n)
    {
        boolean visited[] = new boolean[V];
        LinkedList<Integer> queue = new LinkedList<Integer>();
        visited[n]=true;
        queue.add(n);

        while (queue.size() != 0)
        {
            n = queue.poll();
            System.out.print(n + " ");
            Iterator<Integer> i = adj[n].listIterator();
            while (i.hasNext())
            {
                int next = i.next();
                if (!visited[next])
                {
                    visited[next] = true;
                    queue.add(next);
                }
            }
        }
    }
}
```
С Java все немного сложнее. Лично я использовал Framework поддержку Maven и Groovy. Сначала с помощью Maven пакетируем проект, а потом нажимаем пкм по проекту -> Open Module Settings и добавляем в Libraries .jar файл, который создался, когда мы пакетировали проект (лежит в target). Все, пакет создался, можем его использовать:
```java
public class MyOwnMavenPackageTesting {
    public static void main(String[] args) {
        JavaGraph graph = new JavaGraph(6);
        graph.addEdge(0, 1);
        graph.addEdge(0, 2);
        graph.addEdge(1, 0);
        graph.addEdge(1, 3);
        graph.addEdge(2, 0);
        graph.addEdge(2, 3);
        graph.addEdge(3, 4);
        graph.addEdge(3, 5);
        graph.addEdge(4, 3);
        graph.addEdge(5, 3);

        graph.DFS(0);
        System.out.println('\n');
        graph.BFS(0);
    }
}
```
При использовании пакета ничего добавлять не нужно, пакет используется автоматически.
### Point 4
Я выбрал Gnome sort, Bubble sort и встроенную сортировку для анализа их работы с помощью Benchmark.
#### Работа с C#
Алгоритмы сортировок:
```cs
using System;
using BenchmarkDotNet.Attributes;

namespace Benchmarks
{
    [MemoryDiagnoser]
    public class Sorting
    {
        private const int Size = 10000;
        
        private readonly int[] _arr;
        
        public Sorting()
        {
            _arr = new int[Size];
            for (var i = 0; i < Size; i++)
            {
                _arr[i] = new Random().Next();
            }
        }
        
        [Benchmark]
        public int[] BubbleSort()
        {
            int temp;
            var arr = _arr.ToArray();
            for (var i = 0; i < arr.Length; i++)
            {
                for (var j = i + 1; j < arr.Length; j++)
                {
                    if (arr[i] <= arr[j]) continue;
                    temp = arr[i];
                    arr[i] = arr[j];
                    arr[j] = temp;
                }            
            }

            return arr;
        }
        private void Swap(ref int item1, ref int item2)
        {
            (item1, item2) = (item2, item1);
        }
        
        [Benchmark]
        public int[] GnomeSort()
        {
            var index = 1;
            var nextIndex = index + 1;
            var arr = _arr.ToArray();
            while (index < arr.Length)
            {
                if (arr[index - 1] < arr[index])
                {
                    index = nextIndex;
                    nextIndex++;
                }
                else
                {
                    Swap(ref arr[index - 1], ref arr[index]);
                    index--;
                    if (index != 0) continue;
                    index = nextIndex;
                    nextIndex++;
                }
            }

            return arr;
        }

        [Benchmark]
        public int[] BuiltInSort()
        {
            var arr = _arr.ToArray();
            Array.Sort(arr);
            return arr;
        }

    }
}
```
Нам нужно установить BenchmarkDotNet NuGet пакет. Затем для анализа аллокации памяти добавить атрибут "MemoryDiagnoser" перед объявлением класса, а для анализа времени выполнения добавить атрибут "Benchmark" до каждого определения функции. Код выполнения Benchmark:
```cs
using BenchmarkDotNet.Reports;
using BenchmarkDotNet.Running;

namespace Benchmarks
{
    internal static class Program
    {
        private static void Main()
        {
            var summary = BenchmarkRunner.Run<Sorting>();
        }
    }
}
```
Результат выполнения:\
![image](https://user-images.githubusercontent.com/79001610/156830279-ae95ce74-8fa2-45d8-b640-fde9382e8c60.png)\
Как и ожидалось, bubble sort отработала медленней всех, а встроенная быстрее всех, при этом память выделяется почти одинаково во всех алгоритмах.
#### Работа с Java
Алгоритмы сортировок:
```java
package com.Benchmarks;

import org.openjdk.jmh.annotations.*;

import java.util.Arrays;
import java.util.concurrent.TimeUnit;

@BenchmarkMode(Mode.AverageTime)
@OutputTimeUnit(TimeUnit.NANOSECONDS)
@State(Scope.Benchmark)
@Fork(value = 2, jvmArgs = {"-Xms2G", "-Xmx2G"})
public class Sorting {

    @Param({"10000"})
    private int Size;

    private int[] _arr;

    public static void main(String[] args) throws Exception {
        org.openjdk.jmh.Main.main(args);
    }

    public Sorting()
    {
        _arr = new int[Size];
        for (var i = 0; i < Size; i++)
        {
            _arr[i] = (int) (Math.random() * 10000);
        }
    }

    @org.openjdk.jmh.annotations.Benchmark
    @Fork(value = 1, warmups = 1)
    @Warmup(iterations = 2)
    public int[] BubbleSort()
    {
        var arr = _arr;
        for (int i = 0; i < arr.length - 1; i++) {
            for (int j = 0; j < arr.length - i - 1; j++)
                if (arr[j] > arr[j + 1]) {
                    int temp = arr[j];
                    arr[j] = arr[j + 1];
                    arr[j + 1] = temp;
                }
        }

        return arr;
    }

    @org.openjdk.jmh.annotations.Benchmark
    @Fork(value = 1, warmups = 1)
    @Warmup(iterations = 2)
    public int[] GnomeSort()
    {
        int index = 0;
        var arr = _arr;
        while (index < arr.length) {
            if (index == 0)
                index++;
            if (arr[index] >= arr[index - 1])
                index++;
            else {
                int temp = 0;
                temp = arr[index];
                arr[index] = arr[index - 1];
                arr[index - 1] = temp;
                index--;
            }
        }
        return arr;
    }

    @org.openjdk.jmh.annotations.Benchmark
    @Fork(value = 1, warmups = 1)
    @Warmup(iterations = 2)
    public int[] BuiltInSort()
    {
        var arr = _arr;
        Arrays.sort(arr);
        return arr;
    }
}
```
В Java JMH работает крайне сложно и неприятно. Для начала запустить Maven и открыть pom файл, туда добавить зависимостей:
```xml
<dependencies>
    <dependency>
        <groupId>org.openjdk.jmh</groupId>
        <artifactId>jmh-core</artifactId>
        <version>1.33</version>
    </dependency>
    <dependency>
        <groupId>org.openjdk.jmh</groupId>
        <artifactId>jmh-generator-annprocess</artifactId>
        <version>1.33</version>
    </dependency>
</dependencies>
```
Затем в Maven обновить все проекты. Касаемо алгоритмов сортировок: лично я добавил
```java
@BenchmarkMode(Mode.AverageTime)
@OutputTimeUnit(TimeUnit.NANOSECONDS)
@State(Scope.Benchmark)
@Fork(value = 2, jvmArgs = {"-Xms2G", "-Xmx2G"})
```
до объявления класса, а перед каждым алгоритмом добавлял вот это:
```java
@org.openjdk.jmh.annotations.Benchmark
@Fork(value = 1, warmups = 1)
@Warmup(iterations = 2)
```
Затем надо запустить JMH:
```java
public static void main(String[] args) throws Exception {
        org.openjdk.jmh.Main.main(args);
    }
```
Некоторые из атрибутов опциональны и нужны просто для большей информации.\
Результат работы:\
![image](https://user-images.githubusercontent.com/79001610/156833698-8e0fd4c4-24cb-4283-8190-6ee699d3ce75.png)\
Очень странно получилось, но почему-то в Java получилось всё наоборот, скорость bubble sort самая большая, а встроенная самая медленная (по количеству операций в наносекунду).
Источники: https://mkyong.com/java/java-jmh-benchmark-tutorial/
### Point 5
Для анализа я использовал dotMemory.\
С файловой системой:\
![image](https://user-images.githubusercontent.com/79001610/156853257-5195520c-b8b6-4dd5-8450-052de1b28928.png)\
Не с файловой системой:\
![image](https://user-images.githubusercontent.com/79001610/156853438-42919d60-4b0c-4c2e-b94f-d89e8286e319.png)\
Почему-то получились абсолютно одинаковые результаты, хотя с файловой системой сборщик мусора начинает удалять точки, и общая картина должна выглядеть примерно как зубы акулы, но этого не происходит - странно.
