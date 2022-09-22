using System;
using System.Collections.Generic;

namespace CoR.Output
{
    public class Generator
    {
        public string Name { get; set; }
        public double Total { get; set; }
    }

    public class Day
    {
        public string Name { get; set; }
        public DateTime Date { get; set; }
        public double Emission { get; set; }
    }

    public class HeatRate
    {
        public string Name { get; set; }
        public double Rate { get; set; }
    }

    public class GenerationData
    {
        public List<Generator> Totals { get; set; }
        public List<Day> MaxEmissionGenerators { get; set; }
        public List<HeatRate> ActualHeatRates { get; set; }

        public GenerationData()
        {
            Totals = new List<Generator>();
            MaxEmissionGenerators = new List<Day>();
            ActualHeatRates = new List<HeatRate>();
        }
    }
}