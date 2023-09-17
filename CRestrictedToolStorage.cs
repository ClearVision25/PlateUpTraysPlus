using KitchenData;
using KitchenMods;
using Unity.Collections;
using Unity.Entities;

namespace KitchenTraysPlus
{
    public struct CRestrictedToolStorage : IItemProperty, IComponentData
    {
        public FixedString32 ItemKey;
    }
}
