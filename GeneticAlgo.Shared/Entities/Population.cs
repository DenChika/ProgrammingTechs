using System.Buffers;
using System.Numerics;
using Serilog;

namespace GeneticAlgo.Shared.Entities;

public class Population
{
    public int Gen = 1;
    public double FitnessSum;
    public Dot[] Dots;
    public int BestDot;
    public int MinStep;

    public Population(int minStep, int dotsCount)
    {
        BestDot = 0;
        MinStep = minStep;
        Dots = new Dot[dotsCount];
        for (int i = 0; i < Dots.Length; i++)
            Dots[i] = new Dot();
        Logger.Init();
    }

    public void NextIteration(double width, double height)
    {
        for (int i = 0; i < Dots.Length; i++)
        {
            if (Dots[i].Brain.Step > MinStep + 200)
            {
                Dots[i].IsDead = true;
                Dots[i].IsSlow = true;
            }
            else
            {
                Dots[i].NextIteration(width, height);
            }
        }
    }

    public void CalculateFitness()
    {
        for (int i = 0; i < Dots.Length; i++)
        {
            Dots[i].CalculateFitness();
        }
    }

    public bool AllDead()
    {
        for (int i = 0; i < Dots.Length; i++) {
            if (!Dots[i].IsDead && !Dots[i].IsReached) { 
                return false;
            }
        }

        return true;
    }
    
    public void NextGeneration() 
    {
        var newDots = new Dot[Dots.Length];
        SetBestDot();
        CalculateFitnessSum();
        
        newDots[0] = Dots[BestDot].GetBaby();
        newDots[0].IsBest = true;
        for (int i = 1; i < newDots.Length; i++)
        {
            var parent = SelectParent();
            var baby = parent.GetBaby();
            newDots[i] = baby;
        }
        
        for (int i = 0; i < newDots.Length; i++)
        {
            Dots[i].Clear();
        }
        Dots = newDots;
        Gen++;
    }

    public void CalculateFitnessSum()
    {
        FitnessSum = 0;
        for (int i = 0; i < Dots.Length; i++) {
            FitnessSum += Dots[i].Fitness;
        }
    }
    
    public Dot SelectParent()
    {
        var rand = Random.Shared.NextDouble() * FitnessSum;
        double runningSum = 0;
        for (int i = 0; i < Dots.Length; i++) 
        {
            runningSum += Dots[i].Fitness;
            if (runningSum > rand)
                return Dots[i];
        }
        
        return null;
    }
    
    public void MutateBabies() 
    {
        for (int i = 1; i< Dots.Length; i++)
            Dots[i].Brain.Mutate();
    }
    
    public void SetBestDot() 
    {
        double max = 0;
        int maxIndex = 0;
        for (int i = 0; i < Dots.Length; i++) 
        {
            if (Dots[i].Fitness > max) 
            {
                max = Dots[i].Fitness;
                maxIndex = i;
            }
        }

        BestDot = maxIndex;

        if (Dots[BestDot].IsReached)
        {
            MinStep = Dots[BestDot].Brain.Step;
            //Log.Information("Steps: {0} \n Fitness: {1} number of best: {2}", MinStep, Dots[_bestDot].Fitness, _bestDot);
        }
            
    }
}