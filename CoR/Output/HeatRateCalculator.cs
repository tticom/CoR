using CoR.Input;

namespace CoR.Output
{
    public class HeatRateCalculator
    {
        public HeatRate Calculate(IGenerator generator)
        {
            if (generator.TotalHeatInput.HasValue && generator.ActualNetGeneration.HasValue)
            {
                return new HeatRate
                {
                    Name = generator.Name,
                    Rate = generator.TotalHeatInput.Value / generator.ActualNetGeneration.Value
                };
            }

            return null;
        }
    }
}