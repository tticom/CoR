using CoR.Input;

namespace CoR.Output
{
    public class Generation
    {
        private readonly ReferenceData _referenceData;
        private readonly GeneratorReportData _generatorReportData;
        public GenerationData GenerationData { get; set; }

        public Generation(ReferenceData referenceData, GeneratorReportData generatorReportData)
        {
            _referenceData = referenceData;
            _generatorReportData = generatorReportData;
            GenerationData = new GenerationData();
        }

        public void Calculate()
        {
            var totalGeneratorCalculator = new TotalGeneratorCalculator(_referenceData);
            var maxEmissionsGeneratorCalculator = new MaxEmissionsGeneratorCalculator(_referenceData);
            var heatRateCalculator = new HeatRateCalculator();

            foreach (var windGenerator in _generatorReportData.WindGenerators)
            {
                var calculated = totalGeneratorCalculator.Calculate(windGenerator);
                if (calculated == null) continue;
                GenerationData.Totals.Add(calculated);
            }

            foreach (var gasGenerator in _generatorReportData.GasGenerators)
            {
                var calculated = totalGeneratorCalculator.Calculate(gasGenerator);
                if (calculated != null) 
                    GenerationData.Totals.Add(calculated);

                var calculatedList = maxEmissionsGeneratorCalculator.Calculate(gasGenerator);
                if (calculatedList == null) continue;
                foreach (var day in calculatedList)
                    GenerationData.MaxEmissionGenerators.Add(day);
            }

            foreach (var coalGenerator in _generatorReportData.CoalGenerators)
            {
                var calculated = totalGeneratorCalculator.Calculate(coalGenerator);
                if (calculated != null) 
                    GenerationData.Totals.Add(calculated);

                var calculatedList = maxEmissionsGeneratorCalculator.Calculate(coalGenerator);
                if (calculatedList != null) 
                foreach (var day in calculatedList)
                    GenerationData.MaxEmissionGenerators.Add(day);

                var heatRate = heatRateCalculator.Calculate(coalGenerator);
                if(heatRate == null) continue;
                GenerationData.ActualHeatRates.Add(heatRate);
            }
        }
    }
}