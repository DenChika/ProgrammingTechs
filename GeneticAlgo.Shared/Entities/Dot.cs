using System.Buffers;
using System.Numerics;
using System.Text.Json;

namespace GeneticAlgo.Shared.Entities;

public class Dot
{
    public Vector2 Position;
    public Vector2 Speed;
    public Vector2 Acceleration;
    public Brain Brain;
    public bool IsDead;
    public bool IsReached;
    public bool IsBest;
    public bool IsSlow;
    public double Fitness;

    public Dot()
    {
        Brain = new Brain(Settings.StepsCount);
        IsDead = false;
        IsReached = false;
        IsBest = false;
        IsSlow = false;
        Fitness = 0.0;
        Position = new Vector2(0.0f, 0.0f);
        Speed = new Vector2(0.0f, 0.0f);
        Acceleration = new Vector2(0.0f, 0.0f);
    }

    public void Move()
    {
        if (Brain.Step < Brain.Directions.Length)
        {
            Acceleration = Brain.Directions[Brain.Step];
            Brain.Step++;
        }
        else
        {
            IsDead = true;
            IsSlow = true;
        }
        
        var newVec = new Vector2(Speed.X + Acceleration.X, Speed.Y + Acceleration.Y);
        if (newVec.Length() >= Settings.LimitSpeed)
        {
            Speed.X %= (float) Settings.LimitSpeed;
            Speed.Y %= (float) Settings.LimitSpeed;
            
        }
        
        Speed.X += Acceleration.X;
        Speed.Y += Acceleration.Y;
        Position.X += Speed.X;
        Position.Y += Speed.Y;
    }

    public void NextIteration(double width, double height)
    {
        if (!IsDead && !IsReached)
        {
            Move();
            double distX = Position.X - Settings.Goal.X;
            double distY = Position.Y - Settings.Goal.Y;
            double dist = Math.Sqrt(distX * distX + distY * distY);
            if (Position.X > width / 2 || Position.Y > height / 2 || Position.X < -width / 2 ||
                Position.Y < -height / 2 || IsBumped())
                IsDead = true;
            else if (dist < 0.025)
                IsReached = true;
        }
    }
    
    public Dot GetBaby()
    {
        Dot baby = new Dot();
        baby.Brain = Brain.CloneBrain();
        return baby;
    }
    public bool IsBumped()
    {
        var barriers = Settings.Barriers;
        foreach (var barrier in barriers)
        {
            var radius = barrier.Radius / 155;
            var distX = barrier.Center.X - Position.X;
            var distY = barrier.Center.Y - Position.Y;
            if (Math.Sqrt(distX * distX + distY * distY) > radius) continue;
            return true;
        }

        return false;
    }
    
    public void CalculateFitness()
    {
        if (IsReached)
            Fitness = 50000.0 / (Brain.Step * Brain.Step);
        else if (!IsSlow)
        {
            double distX = Position.X - Settings.Goal.X;
            double distY = Position.Y - Settings.Goal.Y;
            double dist = Math.Sqrt(distX * distX + distY * distY);
            Fitness = 0.1 / (dist * dist);
        }
        else
        {
            Fitness = 0.0;
        }
    }

    public void Clear()
    {
        Brain.Clear();
    }
}