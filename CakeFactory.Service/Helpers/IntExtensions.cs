namespace CakeFactory.Service.Helpers
{
    public static class IntExtensions
    {
        public static int ToMilliseconds(this int duration) => duration * 1000;
    }
}
