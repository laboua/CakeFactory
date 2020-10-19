using System;
using System.Collections.Concurrent;
using System.Threading.Tasks;

namespace CakeFactory.Service
{
    public class Stock
    {
        private static readonly Random Random = new Random();

        private readonly ConcurrentQueue<Recipe> _recipes;

        public int Size => _recipes?.Count ?? 0;

        public int DurationForNextRecipe { get; set; } = GetDurationForNextRecipe();

        public Stock(int total)
        {
            _recipes = new ConcurrentQueue<Recipe>();

            for(var index = 0; index < total; index++)
            {
                _recipes.Enqueue(new Recipe());
            }
        }

        public async Task<Recipe> GetNextRecipeAsync()
        {
            await Task.Delay(DurationForNextRecipe);
            return _recipes.TryDequeue(out var recipe) ? recipe : null;
        }

        private static int GetDurationForNextRecipe()
        {
            var duration = Random.Next(1, 10);

            return duration * 1000;
        }
    }
}