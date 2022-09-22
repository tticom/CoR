using System;
using CoR.Input;

namespace CoR.Output
{
    public class TotalGeneratorCalculator
    {
        internal ReferenceData _referenceData;
        private AbstractTotalGeneratorCalculator _chain;

        public TotalGeneratorCalculator(ReferenceData referenceData)
        {
            _referenceData = referenceData;
            _chain = new WindTotalGeneratorCalculator(_referenceData);
            _chain.SetSuccessor(new CoalGasTotalGeneratorCalculator(_referenceData));
        }

        public Generator Calculate(IGenerator generator)
        {
            return _chain.Calculate(generator);
        }
    }

    public abstract class AbstractTotalGeneratorCalculator
    {
        protected AbstractTotalGeneratorCalculator Successor;
        internal ReferenceData _referenceData;

        protected AbstractTotalGeneratorCalculator(ReferenceData referenceData)
        {
            _referenceData = referenceData;
        }

        public void SetSuccessor(AbstractTotalGeneratorCalculator successor)
        {
            Successor = successor;
        }

        public abstract Generator Calculate(IGenerator report);
    }

    public class WindTotalGeneratorCalculator : AbstractTotalGeneratorCalculator
    {
        public WindTotalGeneratorCalculator(ReferenceData referenceData) : base(referenceData) {}

        public override Generator Calculate(IGenerator report)
        {
            if (!string.IsNullOrEmpty(report.Location))
            {
                var generated = new Generator
                {
                    Name = report.Name
                };

                if (report.Location == "Offshore")
                {
                    foreach (var day in report.Generation)
                        generated.Total += day.Energy * day.Price * _referenceData.ValueFactor.Low;
                }
                else
                {
                    foreach (var day in report.Generation)
                        generated.Total += day.Energy * day.Price * _referenceData.ValueFactor.High;
                }

                return generated;
            }
            return Successor?.Calculate(report);
        }
    }

    public class CoalGasTotalGeneratorCalculator : AbstractTotalGeneratorCalculator
    {
        public CoalGasTotalGeneratorCalculator(ReferenceData referenceData) : base(referenceData) {}

        public override Generator Calculate(IGenerator report)
        {
            if(report.EmissionsRating.HasValue)
            {
                var generated = new Generator
                {
                    Name = report.Name
                };

                foreach (var day in report.Generation)
                {
                    generated.Total += day.Energy * day.Price * _referenceData.ValueFactor.Medium;
                }

                return generated;
            } 
            return Successor?.Calculate(report);
        }
    }
}