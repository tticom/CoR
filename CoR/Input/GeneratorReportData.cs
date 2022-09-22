using System;
using System.Collections.Generic;

namespace CoR.Input
{
    public class Day
    {
        public DateTime Date { get; set; }
        public float Energy { get; set; }
        public float Price { get; set; }
    }

    public interface IGenerator
    {
        string Name { get; set; }
        List<Day> Generation { get; set; }
        string Location { get; set; }
        float? EmissionsRating { get; set; }
        float? TotalHeatInput { get; set; }
        float? ActualNetGeneration { get; set; }
    }

    public class Generator : IGenerator
    {
        public string Name { get; set; }
        public List<Day> Generation { get; set; }
        public string Location { get; set; }
        public float? EmissionsRating { get; set; }
        public float? TotalHeatInput { get; set; }
        public float? ActualNetGeneration { get; set; }

        public Generator()
        {
            Generation = new List<Day>();
        }
    }
    
    public class GeneratorReportData
    {
        public List<IGenerator> WindGenerators { get; set; }
        public List<IGenerator> GasGenerators { get; set; }
        public List<IGenerator> CoalGenerators { get; set; }

        public GeneratorReportData()
        {
            WindGenerators = new List<IGenerator>();
            GasGenerators = new List<IGenerator>();
            CoalGenerators = new List<IGenerator>();
        }
    }
}