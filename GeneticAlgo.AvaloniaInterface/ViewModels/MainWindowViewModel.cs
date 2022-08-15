using System.Linq;
using System.Threading.Tasks;
using Avalonia;
using GeneticAlgo.Shared;
using GeneticAlgo.Shared.Models;
using GeneticAlgo.UIShared;
using OxyPlot;
using OxyPlot.Series;
using Serilog;

namespace GeneticAlgo.AvaloniaInterface.ViewModels;

public class MainWindowViewModel : AvaloniaObject
{
    private readonly IStatisticsConsumer _statisticsConsumer;
    private readonly IExecutionContext _executionContext;
    private readonly ExecutionConfiguration _configuration;
    private readonly BarrierCircle[] _circles;

    public MainWindowViewModel(
        IExecutionContext executionContext,
        ExecutionConfiguration configuration)
    {
        Logger.Init();
        _executionContext = executionContext;
        _configuration = configuration;
        _circles = _executionContext.GetCircles();

        IsRunning = AvaloniaProperty
            .RegisterAttached<MainWindowViewModel, bool>(nameof(IsRunning), typeof(MainWindowViewModel));

        var lineSeries = new ScatterSeries
        {
            MarkerSize = 1,
            MarkerStroke = OxyColors.ForestGreen,
            MarkerType = MarkerType.Circle,
        };

        var goalSeries = new ScatterSeries
        {
            MarkerSize = 3,
            MarkerStroke = OxyColors.Blue,
            MarkerType = MarkerType.Circle,
        };

        var bestDotSeries = new ScatterSeries
        {
            MarkerSize = 2,
            MarkerStroke = OxyColors.Violet,
            MarkerType = MarkerType.Circle,
        };
        
        var circleSeries = new ScatterSeries
        {
            MarkerFill = OxyColors.Red,
            MarkerType = MarkerType.Circle,
        };

        ScatterModel = new PlotModel
        {
            Title = "Points",
            Series = { circleSeries, lineSeries, goalSeries, bestDotSeries },
        };

        var barSeries = new LinearBarSeries
        {
            DataFieldX = nameof(FitnessModel.X),
            DataFieldY = nameof(FitnessModel.Y),
        };

        BarModel = new PlotModel
        {
            Title = "Fitness",
            Series = { barSeries },
        };

        _statisticsConsumer = new PlotStatisticConsumer(circleSeries, lineSeries, barSeries, goalSeries, bestDotSeries);
    }

    public PlotModel ScatterModel { get; }
    public PlotModel BarModel { get; }

    public AttachedProperty<bool> IsRunning { get; }

    public async Task RunAsync()
    {
        foreach (var series in ScatterModel.Series.OfType<XYAxisSeries>())
        {
            series.XAxis.Maximum = _configuration.MaximumValue;
            series.XAxis.Minimum = _configuration.MinimumValue;
            series.YAxis.Maximum = _configuration.MaximumValue;
            series.YAxis.Minimum = _configuration.MinimumValue;
        }

        foreach (var series in BarModel.Series.OfType<XYAxisSeries>())
        {
            series.XAxis.Maximum = _executionContext.PointsCount();
            series.XAxis.Minimum = 0;
            series.YAxis.Maximum = 30;
            series.YAxis.Minimum = 0;
        }

        SetValue(IsRunning, true);
        _executionContext.Reset();

        IterationResult iterationResult;

        do
        {
            iterationResult = await _executionContext.ExecuteIterationAsync();
            _executionContext.ReportStatistics(_statisticsConsumer, _circles);

            ScatterModel.InvalidatePlot(true);
            BarModel.InvalidatePlot(true);
            
            await Task.Delay(_configuration.IterationDelay);
        }
        while (GetValue(IsRunning));
    }

    public void Stop()
    {
        SetValue(IsRunning, false);
    }
}