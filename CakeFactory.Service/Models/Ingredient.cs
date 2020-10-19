namespace CakeFactory.Service
{
    public class Ingredient
    {
        public IngredientType IngredientType { get; }

        public Ingredient(IngredientType ingredientType)
        {
            IngredientType = ingredientType;
        }
    }
}