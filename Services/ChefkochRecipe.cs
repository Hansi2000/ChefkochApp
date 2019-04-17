using System.Collections.Generic;

namespace ChefkochApp.Services
{
    public class ChefkochRecipe
    {
        public string id { get; set; }
        public string title { get; set; }
        public bool hasImage { get; set; }
        public int? previewImageId { get; set; }
        public string instructions { get; set; }
        public List<ChefkochIngredientGroup> ingredientGroups { get; set; }

    }

    public class ChefkochIngredientGroup
    {
        public List<ChefkochIngredient> ingredients { get; set; }
    }

    public class ChefkochIngredient
    {
        public string id { get; set; }
        public string name { get; set; }
        public string unit { get; set; }
        public double amount { get; set; }
    }

}
