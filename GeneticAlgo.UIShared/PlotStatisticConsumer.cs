using GeneticAlgo.Shared;
using GeneticAlgo.Shared.Models;
using OxyPlot.Series;

namespace GeneticAlgo.UIShared;

public class PlotStatisticConsumer : IStatisticsConsumer
{
    private readonly ScatterSeries _circleSeries;
    private readonly ScatterSeries _scatterSeries;
    private readonly LinearBarSeries _linearBarSeries;
    private readonly ScatterSeries _goalSeries;
    private readonly ScatterSeries _bestDotSeries;

    public PlotStatisticConsumer(ScatterSeries circleSeries, ScatterSeries scatterSeries, 
        LinearBarSeries linearBarSeries, ScatterSeries goalSeries, ScatterSeries bestDotSeries)
    {
        _scatterSeries = scatterSeries;
        _linearBarSeries = linearBarSeries;
        _circleSeries = circleSeries;
        _goalSeries = goalSeries;
        _bestDotSeries = bestDotSeries;
    }

    public void Consume(IReadOnlyCollection<Statistic> statistics, IReadOnlyCollection<BarrierCircle> barriers, Point best)
    {
        _scatterSeries.Points.Clear();

        foreach (var statistic in statistics)
        {
            var point = statistic.Point;
            _scatterSeries.Points.Add(new ScatterPoint(point.X, point.Y));
        }
        
        _goalSeries.Points.Clear();
        _goalSeries.Points.Add(new ScatterPoint(1, 1));
        _bestDotSeries.Points.Clear();
        _bestDotSeries.Points.Add(new ScatterPoint(best.X, best.Y));
        _circleSeries.Points.Clear();
        
        foreach (var (point, radius) in barriers)
        {
            _circleSeries.Points.Add(new ScatterPoint(point.X, point.Y, radius));
        }

        _linearBarSeries.ItemsSource = statistics
            .Select(s => new FitnessModel(s.Id, s.Fitness))
            .ToArray();
    }
}