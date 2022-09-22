using System;
using System.IO;
using System.Text;
using System.Xml.Linq;

namespace CoR.Output
{
    public class Writer
    {
        private readonly GenerationData _generationData;
        private readonly string _filepath;
        private XDocument _generationOutput;

        public Writer(GenerationData generationData, string filepath)
        {
            _generationData = generationData;
            _filepath = filepath;
        }

        public void WriteToFile()
        {
            var targetInputFilePath = _filepath.Replace(Configuration.ProcessingFolder, Configuration.ProcessedFolder);
            var targetOutputFilePath = new StringBuilder(_filepath.Replace(Configuration.ProcessingFolder, Configuration.OutputFolder));

            var start = targetOutputFilePath.ToString().LastIndexOf(@"\", StringComparison.Ordinal) + 1;
            var end = targetOutputFilePath.ToString().LastIndexOf(@".xml") - 15;

            targetOutputFilePath.Remove(start, end - start -1);
            targetOutputFilePath.Insert(start, Configuration.OutputFilename);
            
            var filename = _filepath.Replace(Configuration.ProcessingFolder, "");

            if (!File.Exists(_filepath))
                throw new FileNotFoundException($"{filename} has been moved from {Configuration.ProcessingFolder}");
            
            if (!Directory.Exists(Configuration.ProcessingFolder))
                Directory.CreateDirectory(Configuration.ProcessingFolder);

            File.Copy(_filepath, targetInputFilePath, true);
            File.Delete(_filepath);

            CreateOutputXml(targetOutputFilePath.ToString());
        }

        private void CreateOutputXml(string outputFilePath)
        {
            var totals = new XElement("Totals");
            foreach (var generationDataTotal in _generationData.Totals)
            {
                totals.Add(new XElement("Generator",
                    new XElement("Name", generationDataTotal.Name),
                    new XElement("Total", generationDataTotal.Total)));
            }

            var maxEmissions = new XElement("MaxEmissionGenerators");
            foreach (var maxEmission in _generationData.MaxEmissionGenerators)
            {
                maxEmissions.Add(new XElement("Day",
                    new XElement("Name", maxEmission.Name),
                    new XElement("Date", maxEmission.Date),
                    new XElement("Emission", maxEmission.Emission)));
            }

            var heatRates = new XElement("ActualHeatRates");
            foreach (var heatRate in _generationData.ActualHeatRates)
            {
                heatRates.Add(new XElement("Name", heatRate.Name),
                    new XElement("HeatRate", heatRate.Rate));
            }

            _generationOutput = new XDocument(
                new XDeclaration("1.0", "utf-8", "yes"), 
                new XElement("GenerationOutput", totals, maxEmissions, heatRates));

            _generationOutput.Save(outputFilePath);
        }
    }
}