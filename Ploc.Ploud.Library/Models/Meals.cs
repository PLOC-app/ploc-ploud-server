using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ploc.Ploud.Library
{
    [Flags]
    public enum Meals
    {
        None = 0,
        
        Aperitif = 1,
        
        Soup = 2,
        
        Entry = 4,
        
        PorkButchery = 8,
        
        SeaFood = 16,
        
        Fish = 32,
        
        WhiteMeat = 64,
        
        Game = 128,
        
        Poultry = 256,
        
        RedMeat = 512,
        
        DishAndVegetable = 1024,
        
        Cheese = 2048,
        
        Dessert = 4096
    }
}
