using System.Buffers;
using System.Numerics;
using System.Text.Json;
using GeneticAlgo.Shared.Models;
using GeneticAlgo.Shared.Tools;

namespace GeneticAlgo.Shared;

public class Settings
{
    private static Settings? _instance = null;
    public static readonly Vector2 Goal = new((float) 1.0, (float) 1.0);
    public static readonly double LimitSpeed = 0.03;
    public static readonly int StepsCount = 400;
    public static readonly IExecutionContext Dummy = new DummyExecutionContext(200, 1, 3);
    public static readonly BarrierCircle[] Barriers = Dummy.GetCircles();
    public static readonly ArrayPool<Vector2> Pool = ArrayPool<Vector2>.Create(StepsCount, Dummy.GetSize());

    private Settings()
    {
        
    }

    public static Settings GetInstance()
    {
        if (_instance is null)
        {
            _instance = new Settings();
        }

        return _instance;
    }
}