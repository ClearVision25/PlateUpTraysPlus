using Kitchen;
using KitchenData;
using KitchenLib.Customs;
using KitchenLib.References;
using KitchenLib.Utils;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

namespace KitchenTraysPlus
{
    internal class DishTub : CustomItem
    {
        public override int BaseGameDataObjectID => ItemReferences.Tray;
        public override string UniqueNameID => "DishTub";
        public override GameObject Prefab => Mod.Bundle.LoadAsset<GameObject>("Dish_Tub");
        
        public override List<IItemProperty> Properties => new(new IItemProperty[] {
            new CToolStorage{Capacity = 5},
            new CEquippableTool{CanHoldItems = false},
            new CRestrictedToolStorage{ItemKey = "DishTub"}

        });
        public override void OnRegister(Item item)
        {
            base.OnRegister(item);

            var materials = new Material[] { MaterialUtils.GetExistingMaterial("Metal Very Dark")};
            MaterialUtils.ApplyMaterial(Prefab, "Cube", materials);

            FieldInfo storage = ReflectionUtils.GetField<ItemVariableStorageView>("Storage");
            List<GameObject> storages = new()
            {
                GameObjectUtils.GetChildObject(Prefab, "Storage 1"),
                GameObjectUtils.GetChildObject(Prefab, "Storage 2"),
                GameObjectUtils.GetChildObject(Prefab, "Storage 3"),
                GameObjectUtils.GetChildObject(Prefab, "Storage 4"),
                GameObjectUtils.GetChildObject(Prefab, "Storage 5")
            };

            ItemVariableStorageView ivsv = Prefab.AddComponent<ItemVariableStorageView>();
            storage.SetValue(ivsv, storages);

            item.IsIndisposable = true;

        }
    }
}
