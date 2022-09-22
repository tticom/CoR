using System;
using System.Collections.Generic;
using CoR.Input;

namespace CoR.Output
{
    public class MaxEmissionsGeneratorCalculator
    {
        internal ReferenceData _referenceData;
        private AbstractMaxEmissionsCalculator _chain;

        public MaxEmissionsGeneratorCalculator(ReferenceData referenceData)
        {
            _referenceData = referenceData;
            _chain = new GasMaxEmissionsCalculator(_referenceData);
            _chain.SetSuccessor(new CoalMaxEmissionsCalculator(_referenceData));
        }

        public List<Day> Calculate(IGenerator generator)
        {
            return _chain.Calculate(generator);
        }
    }

    public abstract class AbstractMaxEmissionsCalculator
    {
        protected AbstractMaxEmissionsCalculator Successor;
        internal ReferenceData _referenceData;

        protected AbstractMaxEmissionsCalculator(ReferenceData referenceData)
        {
            _referenceData = referenceData;
        }

        public void SetSuccessor(AbstractMaxEmissionsCalculator successor)
        {
            Successor = successor;
        }

        public abstract List<Day> Calculate(IGenerator report);
    }

    public class GasMaxEmissionsCalculator : AbstractMaxEmissionsCalculator
    {
        public GasMaxEmissionsCalculator(ReferenceData referenceData) : base(referenceData) {}

        public override List<Day> Calculate(IGenerator report)
        {
            if (!report.TotalHeatInput.HasValue)
            {
                var days = new List<Day>();
                
                foreach (var generationDay in report.Generation)
                {
                    var day = new Day
                    {
                        Name = report.Name,
                        Date = generationDay.Date,
                        Emission = generationDay.Energy * report.EmissionsRating.Value * _referenceData.EmissionsFactor.Medium
                    };

                    days.Add(day);
                }

                return days;
            }
            return Successor?.Calculate(report);
        }
    }

    public class CoalMaxEmissionsCalculator : AbstractMaxEmissionsCalculator
    {
        public CoalMaxEmissionsCalculator(ReferenceData referenceData) : base(referenceData) {}

        public override List<Day> Calculate(IGenerator report)
        {
            if (!report.TotalHeatInput.HasValue)
            {
                var days = new List<Day>();
                
                foreach (var generationDay in report.Generation)
                {
                    var day = new Day
                    {
                        Name = report.Name,
                        Date = generationDay.Date,
                        Emission = generationDay.Energy * report.EmissionsRating.Value * _referenceData.EmissionsFactor.High
                    };

                    days.Add(day);
                }

                return days;
            }
            return Successor?.Calculate(report);
        }
    }
}