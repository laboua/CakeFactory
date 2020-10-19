using CakeFactory.Service.Helpers;
using System;

namespace CakeFactory.Service.Settings
{
    public class DurationSettings
    {
        public DurationSettings(int prepareDuration,
            int cookDuration,
            int packageDuration,
            int prepareMaxDegree,
            int cookMaxDegree,
            int packageMaxDegree,
            int reportingDuration)
        {
            PrepareDuration = prepareDuration;
            CookDuration = cookDuration ;
            PackageDuration = packageDuration;
            PrepareMaxDegree = prepareMaxDegree;
            CookMaxDegree = cookMaxDegree;
            PackageMaxDegree = packageMaxDegree;
            ReportingDuration = reportingDuration;
        }

        public int PrepareDuration { get; }
        public int CookDuration { get; }
        public int PackageDuration { get; }
        public int DeliveryDuration => new Random().Next(10, 20).ToMilliseconds();
        public int PrepareMaxDegree { get; }
        public int CookMaxDegree { get; }
        public int PackageMaxDegree { get; }
        public int ReportingDuration { get; }
    }
}