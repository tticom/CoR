using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Linq;

namespace CoR.Input
{
    public class GenerationReportRepository
    {
        public string Filename { get; set; }
        public GeneratorReportData GeneratorReportData { get; set; }

        public GenerationReportRepository(string filename)
        {
            Filename = filename;
            Load();
        }

        private void Load()
        {
            try
            {
                if (!File.Exists(Filename))
                    throw new FileNotFoundException("No Generation Report Data, {Filename} could not befound");

                var xGenerationReportData = XElement.Load(Filename);
                GeneratorReportData = new GeneratorReportData();

                var wind =
                    from grd in xGenerationReportData.Elements().Elements("WindGenerator")
                    select grd;

                var gas =
                    from grd in xGenerationReportData.Elements().Elements("GasGenerator")
                    select grd;

                var coal =
                    from grd in xGenerationReportData.Elements().Elements("CoalGenerator")
                    select grd;

                ExtractWindGenerationData(wind);
                ExtractGasGenerationData(gas);
                ExtractCoalGenerationData(coal);
            }
            catch (FileNotFoundException fnfe)
            {
                Console.WriteLine(fnfe.Message + " " + fnfe.FileName);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                FileError.MoveErrorFile(Filename);
            }
        }

        private void ExtractWindGenerationData(IEnumerable<XElement> windElements)
        {
            foreach (var windElement in windElements)
            {
                var windGenerator = new Generator
                {
                    Name = windElement.Element("Name").Value,
                    Location = windElement.Element("Location").Value
                };

                var generations = 
                    from gen in windElement.Elements("Generation")
                    select gen;

                foreach (var generation in generations)
                {
                    var days = 
                        from d in generation.Elements("Day")
                        select d;

                    foreach (var day in days)
                    {
                        windGenerator.Generation.Add(GetDayData(day));
                    }
                }
                GeneratorReportData.WindGenerators.Add(windGenerator);
            }
        }

        private void ExtractGasGenerationData(IEnumerable<XElement> gasElements)
        {
            foreach (var gasElement in gasElements)
            {
                var gasGenerator = new Generator {Name = gasElement.Element("Name").Value};

                var emissions = gasElement.Element("EmissionsRating").Value;
                gasGenerator.EmissionsRating = !string.IsNullOrEmpty(emissions) ? (float)Math.Round(double.Parse(emissions), 3): 0f;

                var generations = 
                    from gen in gasElement.Elements("Generation")
                    select gen;

                foreach (var generation in generations)
                {
                    var days = 
                        from d in generation.Elements("Day")
                        select d;

                    foreach (var day in days)
                    {
                        gasGenerator.Generation.Add(GetDayData(day));
                    }
                }
                GeneratorReportData.GasGenerators.Add(gasGenerator);
            }
        }

        private void ExtractCoalGenerationData(IEnumerable<XElement> coalElements)
        {
            foreach (var coalElement in coalElements)
            {
                var coalGenerator = new Generator {Name = coalElement.Element("Name").Value};

                var totalHeatInput = coalElement.Element("TotalHeatInput").Value;
                coalGenerator.TotalHeatInput = !string.IsNullOrEmpty(totalHeatInput) ? (float)Math.Round(double.Parse(totalHeatInput), 3): 0f;
                
                var actualNetGeneration = coalElement.Element("ActualNetGeneration").Value;
                coalGenerator.ActualNetGeneration = !string.IsNullOrEmpty(actualNetGeneration) ? (float)Math.Round(double.Parse(actualNetGeneration), 3): 0f;
                
                var emissions = coalElement.Element("EmissionsRating").Value;
                coalGenerator.EmissionsRating = !string.IsNullOrEmpty(emissions) ? (float)Math.Round(double.Parse(emissions), 3): 0f;


                var generations = 
                    from gen in coalElement.Elements("Generation")
                    select gen;

                foreach (var generation in generations)
                {
                    var days = 
                        from d in generation.Elements("Day")
                        select d;

                    foreach (var day in days)
                    {
                        coalGenerator.Generation.Add(GetDayData(day));
                    }
                }
                GeneratorReportData.CoalGenerators.Add(coalGenerator);
            }
        }

        private Day GetDayData(XElement day)
        {
            var date = day.Element("Date").Value;
            var energy = day.Element("Energy").Value;
            var price = day.Element("Price").Value;

            return new Day
            {
                Date = !string.IsNullOrEmpty(date) ? DateTime.Parse(date) : new DateTime(),
                Energy = !string.IsNullOrEmpty(energy) ? (float)Math.Round(float.Parse(energy), 3): 0f,
                Price = !string.IsNullOrEmpty(price) ? (float)Math.Round(float.Parse(price), 3) : 0f
            };
        }
    }
}