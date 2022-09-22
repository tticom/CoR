namespace CoR.Input
{
    public class Factor
    {
        public float High { get; set; }
        public float Medium { get; set; }
        public float Low { get; set; }
    }

    public class ReferenceData
    {
        public Factor ValueFactor { get; set; }
        public Factor EmissionsFactor { get; set; }
    }
}