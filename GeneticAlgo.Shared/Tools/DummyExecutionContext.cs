using System.Numerics;
using System.Text.Json;
using GeneticAlgo.Shared.Entities;
using GeneticAlgo.Shared.Models;
using Serilog;

namespace GeneticAlgo.Shared.Tools;

public class DummyExecutionContext : IExecutionContext
{
    private readonly int _circleCount;
    private readonly int _size;
    private readonly int _maximumValue;
    private readonly Vector2 _goal;
    private readonly Execution _execution;
    private Dot BestDot;
    private readonly BarrierCircle[] _circles;

    public DummyExecutionContext(int size, int maximumValue, int circleCount)
    {
        _size = size;
        _maximumValue = maximumValue;
        _circleCount = circleCount;
        _goal = new Vector2((float) 1.0, (float) 1.0);
        _circles = ReportCircles();
        _execution = new Execution(4, 4, _size, 500);
        BestDot = new Dot();
        Logger.Init();
    }

    private double NextPosition => Random.Shared.NextDouble() * _maximumValue;
    private double NextRadius => 0.3 + Random.Shared.NextDouble() * _maximumValue * 2;
    
    public void Reset() { }

    public int GetSize()
    {
        return _size;
    }
    public Task<IterationResult> ExecuteIterationAsync()
    {
        return Task.FromResult(IterationResult.IterationFinished);
    }

    public BarrierCircle[] GetCircles()
    {
        return _circles;
    }

    public int PointsCount()
    {
        return _execution.Population.Dots.Length;
    }
    public BarrierCircle[] ReportCircles()
    {
        var circles = new BarrierCircle[_circleCount];
        for (int i = 0; i < _circleCount; i++)
        {
            var positionX = NextPosition;
            var positionY = NextPosition;
            var radius = NextRadius;
            while (Math.Sqrt(positionX * positionX + positionY * positionY) < radius || 
                   Math.Sqrt((positionX - _goal.X) * (positionX - _goal.X) + 
                             (positionY - _goal.Y) * (positionY - _goal.Y)) < radius)
            {
                positionX = NextPosition;
                positionY = NextPosition;
                radius = NextRadius;
            }

            circles[i] = new BarrierCircle(new Point(positionX, positionY), 50 * radius);
        }

        return circles;
    }
    public void ReportStatistics(IStatisticsConsumer statisticsConsumer, BarrierCircle[] circles)
    {
        _execution.ExecuteIteration();
        Statistic[] statistics = Enumerable.Range(0, _execution.Population.Dots.Length)
            .Select(i => new Statistic(i, 
                new Point(_execution.Population.Dots[i].Position.X, _execution.Population.Dots[i].Position.Y)
            ,_execution.Population.Dots[i].Fitness))
            .ToArray();
        
        if (_execution.Population.Gen == 1)
        {
            BestDot.Position.X = -10;
            BestDot.Position.Y = -10;
        } 
        //Log.Information("Best: {0} {1}", best.Position.X, best.Position.Y);
       statisticsConsumer.Consume(statistics, circles, new Point(BestDot.Position.X, BestDot.Position.Y));
        
        /*for (int i = 0; i < _execution.Population.Dots.Length; i++)
        {
            Log.Information("Fitness: {0} \n Steps: {1}", _execution.Population.Dots[i].Fitness, 
                _execution.Population.Dots[i].Brain.Step);
        }*/

        if (_execution.Population.AllDead())
        {
            _execution.ExecuteIfDead();
            BestDot = _execution.Population.Dots.First(x => x.IsBest);
        }
    }
}