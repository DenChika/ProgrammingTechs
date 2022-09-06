using System.Buffers;
using System.Numerics;

namespace GeneticAlgo.Shared.Entities;

public class Brain
{
    public Vector2[] Directions;
    public int Step;
    public double MutateChance;

    public Brain(int size)
    {
        Directions = ArrayPool<Vector2>.Shared.Rent(size);
        Step = 0;
        MutateChance = 0.025;
        Randomize();
    }

    public void Randomize()
    {
        for (var i = 0; i < Directions.Length; i++)
        {
            var angle = Random.Shared.NextDouble() * 2 * Math.PI;
            var x = Math.Cos(angle) * 0.005;
            var y = Math.Sin(angle) * 0.005;
            Directions[i] = new Vector2((float) x, (float) y);
        }
    }
    public Brain CloneBrain()
    {
        Brain clone = new Brain(Directions.Length);
        for (int i = 0; i < Directions.Length; i++)
            clone.Directions[i] = Directions[i];
        return clone;
    }
    public void Mutate()
    {
        for (var i = 0; i < Directions.Length; i++)
        {
            var rand = Random.Shared.NextDouble();
            if (rand > MutateChance) continue;
            var angle = Random.Shared.NextDouble() * 2 * Math.PI;
            var x = Math.Cos(angle) * 0.005;
            var y = Math.Sin(angle) * 0.005;
            Directions[i] = new Vector2((float) x, (float) y);
        }
    }

    public void Clear()
    {
        ArrayPool<Vector2>.Shared.Return(Directions);
    }
}