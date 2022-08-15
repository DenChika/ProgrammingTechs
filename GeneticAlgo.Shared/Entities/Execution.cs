using System.Numerics;
using System.Reflection.Metadata.Ecma335;

namespace GeneticAlgo.Shared.Entities;
public class Execution
{
    public Population Population;
    public double Width;
    public double Height;

    public Execution(double width, double height, int size, int minStep)
    {
        Width = width;
        Height = height;
        Population = new Population(minStep, size);
    }
    public void ExecuteIfDead()
    {
        Population.NextGeneration();
        Population.MutateBabies();
    }
    public void ExecuteIteration()
    {
        if (!Population.AllDead())
        {
            Population.NextIteration(Width, Height);
            Population.CalculateFitness();
        }
    }
}