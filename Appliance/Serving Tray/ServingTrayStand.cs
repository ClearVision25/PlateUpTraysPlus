using ApplianceLib.Api.Prefab;
using Kitchen;
using KitchenData;
using KitchenLib.Customs;
using KitchenLib.Utils;
using System.Collections.Generic;
using UnityEngine;

namespace KitchenTraysPlus
{
    internal class ServingTrayStand : CustomAppliance
    {
        public override string UniqueNameID => "ServingTrayStand";
        public override GameObject Prefab => Mod.Bundle.LoadAsset<GameObject>("Serving_Tray_Counter");
        public override RarityTier RarityTier => RarityTier.Rare;
        public override bool IsPurchasable => false;
        public override bool IsPurchasableAsUpgrade => true;
        public override PriceTier PriceTier => PriceTier.Medium;
        public override List<Appliance.ApplianceProcesses> Processes => ((Appliance)GDOUtils.GetExistingGDO(KitchenLib.References.ApplianceReferences.Countertop)).Processes;
        public override ShoppingTags ShoppingTags => ShoppingTags.FrontOfHouse;
        public override List<(Locale, ApplianceInfo)> InfoList => new()
        {
            (Locale.English, LocalisationUtils.CreateApplianceInfo(
                "Serving Tray Stand",
                "Be a real server with this.",
                new List<Appliance.Section>() { 
                    new Appliance.Section() { Title = "Tool - Serving Tray", Description = "Serve a table 4 items at once!" }, 
                    new Appliance.Section() { Title = "Server Specialist", Description = "Can carry items that can be served to a table" } },
                new()))
        };
        
        public override List<IApplianceProperty> Properties => new List<IApplianceProperty>(new IApplianceProperty[] {
        KitchenPropertiesUtils.GetCItemProvider(GDOUtils.GetCustomGameDataObject<ServingTray>().ID,1,1,false,false,true,false,false,true,false),
        new Kitchen.CItemHolder{}
        });

        public override void SetupPrefab(GameObject prefab)
        {
            var materials = new Material[] { MaterialUtils.GetExistingMaterial("Danger Hob") };
            MaterialUtils.ApplyMaterial(prefab, "HoldPoint/Serving_Tray/Cylinder", materials);

            PrefabBuilder.AttachCounter(prefab, CounterType.DoubleDoors);

            var holdTransform = GameObjectUtils.GetChildObject(prefab, "HoldPoint").transform;
            var holdPoint = prefab.AddComponent<HoldPointContainer>();
            holdPoint.HoldPoint = holdTransform;

            var sourceView = prefab.AddComponent<LimitedItemSourceView>();
            sourceView.HeldItemPosition = holdTransform;

            ReflectionUtils.GetField<LimitedItemSourceView>("Items").SetValue(sourceView, new List<GameObject>()
            {
                GameObjectUtils.GetChildObject(prefab, "HoldPoint/Serving_Tray")
            });
        }
    }
}
