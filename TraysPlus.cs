using ApplianceLib.Api;
using Kitchen;
using KitchenData;
using KitchenLib;
using KitchenLib.Event;
using KitchenLib.References;
using KitchenLib.Utils;
using KitchenMods;
using System.Linq;
using System.Reflection;
using UnityEngine;

// Namespace should have "Kitchen" in the beginning
namespace KitchenTraysPlus
{

    public class Mod : BaseMod, IModSystem
    {
        // GUID must be unique and is recommended to be in reverse domain name notation
        // Mod Name is displayed to the player and listed in the mods menu
        // Mod Version must follow semver notation e.g. "1.2.3"
        public const string MOD_GUID = "Clear.PlateUp.TraysPlus";
        public const string MOD_NAME = "Trays Plus";
        public const string MOD_VERSION = "0.1.3";
        public const string MOD_AUTHOR = "Clear";
        public const string MOD_GAMEVERSION = ">=1.1.4";
        // Game version this mod is designed for in semver
        // e.g. ">=1.1.3" current and all future
        // e.g. ">=1.1.3 <=1.2.3" for all from/until


        // Boolean constant whose value depends on whether you built with DEBUG or RELEASE mode, useful for testing
#if DEBUG
        public const bool DEBUG_MODE = true;
#else
        public const bool DEBUG_MODE = false;
#endif

        public static AssetBundle Bundle;

        public Mod() : base(MOD_GUID, MOD_NAME, MOD_AUTHOR, MOD_VERSION, MOD_GAMEVERSION, Assembly.GetExecutingAssembly()) { }

        protected override void OnInitialise()
        {
            LogWarning($"{MOD_GUID} v{MOD_VERSION} in use!");
            Appliance servingTrayStand = GDOUtils.GetCastedGDO<Appliance, ServingTrayStand>();
            Appliance dishTubStand = GDOUtils.GetCastedGDO<Appliance, DishTubStand>();
            Appliance trayStand = GDOUtils.GetExistingGDO(ApplianceReferences.TrayStand) as Appliance;

            dishTubStand.Upgrades.Add(servingTrayStand);
            servingTrayStand.Upgrades.Add(dishTubStand);

            trayStand.Upgrades.Add(servingTrayStand);
            trayStand.Upgrades.Add(dishTubStand);


        }

        private void AddGameData()
        {
            LogInfo("Attempting to register game data...");

            AddGameDataObject<ServingTray>();
            AddGameDataObject<DishTub>();

            AddGameDataObject<ServingTrayStand>();
            AddGameDataObject<DishTubStand>();


            LogInfo("Done loading game data.");
        }

        protected override void OnUpdate()
        {
        }



        protected override void OnPostActivate(KitchenMods.Mod mod)
        {
            // Load asset bundle
            LogInfo("Attempting to load asset bundle...");
            Bundle = mod.GetPacks<AssetBundleModPack>().SelectMany(e => e.AssetBundles).First();
            LogInfo("Done loading asset bundle.");

            // Register custom GDOs
            AddGameData();

            // Perform actions when game data is built
            Events.BuildGameDataPostViewInitEvent += delegate (object s, BuildGameDataEventArgs args)
            {
                if (args.firstBuild)
                {
                    //===============
                    //  Serving Tray
                    //===============
                    LogInfo("Add Dishes");
                    // Add Dishes to Serving Tray Allow List
                    foreach (Dish dish in args.gamedata.Get<Dish>())
                    {
                        foreach(Dish.MenuItem item in dish.UnlocksMenuItems)
                        {
                            RestrictedItemTransfers.AllowItem("ServingTray", item.Item);
                            //LogInfo(item.Item.name);
                        }

                        foreach (Dish.IngredientUnlock extra in dish.ExtraOrderUnlocks)
                        {
                            RestrictedItemTransfers.AllowItem("ServingTray", extra.Ingredient);
                            //LogInfo(item.Item.name);
                        }

                    }

                    foreach(Item item in args.gamedata.Get<Item>())
                    {
                        if(item.ItemStorageFlags == ItemStorage.StackableFood)
                        {
                            RestrictedItemTransfers.AllowItem("ServingTray", item);
                            //LogInfo(item.Item.name);
                        }
                    }

                    RestrictedItemTransfers.AllowItem("ServingTray", ItemReferences.Plate);
                    RestrictedItemTransfers.AllowItem("ServingTray", ItemReferences.ServingBoard);

                    RestrictedItemTransfers.AllowItem("ServingTray", ItemReferences.Napkin);
                    RestrictedItemTransfers.AllowItem("ServingTray", ItemReferences.SharpCutlery);
                    RestrictedItemTransfers.AllowItem("ServingTray", ItemReferences.Breadsticks);
                    RestrictedItemTransfers.AllowItem("ServingTray", ItemReferences.Candle);
                    RestrictedItemTransfers.AllowItem("ServingTray", ItemReferences.Menu);
                    RestrictedItemTransfers.AllowItem("ServingTray", ItemReferences.SpecialsMenu);
                    RestrictedItemTransfers.AllowItem("ServingTray", ItemReferences.SupplyBox);

                    RestrictedItemTransfers.AllowItem("ServingTray", ItemReferences.ForgetMeNot);
                    RestrictedItemTransfers.AllowItem("ServingTray", ItemReferences.Leave);
                    RestrictedItemTransfers.AllowItem("ServingTray", ItemReferences.Patience);


                    // Burrito Mod
                    RestrictedItemTransfers.AllowItem("ServingTray", 1580505650); // Burrito Basket

                    //==============
                    //   Dish Tub
                    //==============
                    LogInfo("Add Dirty Dishes");
                    // Add DDirty Dishes to Dish Tub Allow List

                    foreach (Item item in args.gamedata.Get<Item>())
                    {
                        if (item.DirtiesTo != null)
                        {
                            Item dirtyitem = item.DirtiesTo;
                            RestrictedItemTransfers.AllowItem("DishTub", dirtyitem);

                        }

                        foreach (Item.ItemProcess process in item.DerivedProcesses)
                        {
                            if (process.Process.ID == ProcessReferences.Clean && process.Result != null)
                            {
                                int cleanitem = process.Result.ID;

                                RestrictedItemTransfers.AllowItem("DishTub", cleanitem);
                                RestrictedItemTransfers.AllowItem("DishTub", item);

                            }

                        }


                    }





                }







            };
        }
        #region Logging
        public static void LogInfo(string _log) { Debug.Log($"[{MOD_NAME}] " + _log); }
        public static void LogWarning(string _log) { Debug.LogWarning($"[{MOD_NAME}] " + _log); }
        public static void LogError(string _log) { Debug.LogError($"[{MOD_NAME}] " + _log); }
        public static void LogInfo(object _log) { LogInfo(_log.ToString()); }
        public static void LogWarning(object _log) { LogWarning(_log.ToString()); }
        public static void LogError(object _log) { LogError(_log.ToString()); }
        #endregion
    }
}
