using GeneticAlgo.Shared.Models;

namespace GeneticAlgo.Shared
{
    public interface IExecutionContext
    {
        void Reset();

        int GetSize();
        Task<IterationResult> ExecuteIterationAsync();

        BarrierCircle[] GetCircles();

        int PointsCount();
        void ReportStatistics(IStatisticsConsumer statisticsConsumer, BarrierCircle[] circles);
    }
}