using System.Numerics;
using System.Text.Json;
using GeneticAlgo.Shared.Entities;
using GeneticAlgo.Shared.Models;

namespace GeneticAlgo.MemoryTest;

public class Program
{
    public static void Main(string[] args)
    {
        var execution = new Execution(4, 4, 1000, 500);
        while (true)
        {
            execution.ExecuteIteration();
            if (execution.Population.AllDead())
            {
                execution.ExecuteIfDead();
            }
        }
    }
}
