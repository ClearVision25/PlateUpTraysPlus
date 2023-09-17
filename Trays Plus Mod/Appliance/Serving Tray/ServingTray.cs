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
    internal class ServingTray : CustomItem
    {
        public override int BaseGameDataObjectID => ItemReferences.Tray;
        public override string UniqueNameID => "ServingTray";
        public override GameObject Prefab => Mod.Bundle.LoadAsset<GameObject>("Serving_Tray");
        public override List<IItemProperty> Properties => new(new IItemProperty[] {
            new CToolStorage{Capacity = 4},
            new CEquippableTool{CanHoldItems = false},
            new CRestrictedToolStorage{ItemKey = "ServingTray"}
        });

    public override void OnRegister(Item item)
        {
            base.OnRegister(item);

            var materials = new Material[1];

            materials[0] = MaterialUtils.GetExistingMaterial("Danger Hob");
            MaterialUtils.ApplyMaterial(Prefab, "Cylinder", materials);

            FieldInfo storage = ReflectionUtils.GetField<ItemVariableStorageView>("Storage");
            List<GameObject> storages = new()
            {
                GameObjectUtils.GetChildObject(Prefab, "Storage 1"),
                GameObjectUtils.GetChildObject(Prefab, "Storage 2"),
                GameObjectUtils.GetChildObject(Prefab, "Storage 3"),
                GameObjectUtils.GetChildObject(Prefab, "Storage 4")
            };

            ItemVariableStorageView ivsv = Prefab.AddComponent<ItemVariableStorageView>();
            storage.SetValue(ivsv, storages);

            item.IsIndisposable = true;
        }
    }
}
