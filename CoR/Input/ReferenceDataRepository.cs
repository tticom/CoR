using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Linq;

namespace CoR.Input
{

    public class ReferenceDataRepository
    {
        public ReferenceData ReferenceData { get; set; }

        public ReferenceDataRepository()
        {
            ReferenceData = new ReferenceData();
            Load();
        }

        private void Load()
        {
            if (!File.Exists(Configuration.ReferenceDataFile)) throw new FileNotFoundException("No reference data, {Configuration.ReferemceDataFile} could not be found");

            var xReferenceData = XElement.Load(Configuration.ReferenceDataFile);

            var xFactors =
                from xf in xReferenceData.Elements("Factors")
                select xf;
            
            var valueFactors =
                from factors in xFactors.Elements()
                where factors.Name == "ValueFactor"
                select factors;

            var emissionsFactors =
                from factors in xFactors.Elements()
                where factors.Name == "EmissionsFactor"
                select factors;

            ReferenceData.ValueFactor = GetDataFromFactor(valueFactors);
            ReferenceData.EmissionsFactor = GetDataFromFactor(emissionsFactors);
        }

        private Factor GetDataFromFactor(IEnumerable<XElement> xfactor)
        {
            var high = (from xfe in xfactor.Elements() where xfe.Name == "High" select xfe.Value).FirstOrDefault();
            var med = (from xfe in xfactor.Elements() where xfe.Name == "Medium" select xfe.Value).FirstOrDefault();
            var low = (from xfe in xfactor.Elements() where xfe.Name == "Low" select xfe.Value).FirstOrDefault();

            return new Factor
            {
                High = !string.IsNullOrEmpty(high) ? (float)Math.Round(float.Parse(high), 3) : 0f,
                Medium = !string.IsNullOrEmpty(med) ? (float)Math.Round(float.Parse(med), 3) : 0f,
                Low = !string.IsNullOrEmpty(low) ? (float)Math.Round(float.Parse(low), 3) : 0f
            };
        }
    }
}