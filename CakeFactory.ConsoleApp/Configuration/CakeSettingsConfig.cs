using System.Collections.Generic;

namespace CakeFactory.ConsoleApp.Configuration
{
    public class CakeSettingsConfig
    {
        public int Stock { get; set; }
        public DurationSettingsConfig DurationSettings { get; set; }
        public ParallelismSettingsConfig ParallelismSettings { get; set; }
    }
    public class DurationSettingsConfig
    {
        public int[] PrepareDurationInterval { get; set; }
        public int CookDuration { get; set; }
        public int PackageDuration { get; set; }
        public int ReportingDuration { get; set; }
    }
    public class ParallelismSettingsConfig
    {
        public int PrepareMaxDegree { get; set; }
        public int CookMaxDegree { get; set; }
        public int PackageMaxDegree { get; set; }
    }
}
