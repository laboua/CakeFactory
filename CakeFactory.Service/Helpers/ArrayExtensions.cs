using System;

namespace CakeFactory.Service.Helpers
{
    public static class ArrayExtensions
    {
        private static readonly Random Random = new Random();
        public static int GetRandom(this Array durationInterval) =>
            durationInterval.Length > 2 ?
            Random.Next((int)durationInterval.GetValue(0), (int)durationInterval.GetValue(1)) :
            Random.Next(5, 8);
    }
}
