using System;
using System.Collections.Generic;
using AssetsLib;
using SRML;
using SRML.SR;
using SRML.Utils.Enum;
using TestModForVideoSR1.SlimeBehaviors;
using UnityEngine;
using Library = ShortcutLib.Shortcut;
using static TestModForVideoSR1.CustomIDs;
namespace TestModForVideoSR1
{
    

    public class EntryPoint : ModEntryPoint
    {
        public override void PreLoad()
        {
            PreloadLargos(TEST_SLIME);
            HarmonyInstance.PatchAll();
        }

        public override void Load()
        {
            CreateSlimeAndPlort(TEST_SLIME,
                TEST_PLORT,
                Identifiable.Id.PINK_SLIME,
                Identifiable.Id.PINK_SLIME,
                Identifiable.Id.PINK_PLORT,
                "Test Slime",
                "Test Plort",
                new []{ SlimeEat.FoodGroup.FRUIT, SlimeEat.FoodGroup.MEAT },
                new []{Identifiable.Id.HEN},
                "",
                "",
                new Color32(255, 255, 255, 255).GetFloatVersion(),
                Color.green,
                Color.green,
                Color.green,
                Color.green,
                Color.green,
                Color.green,
                Color.green,
                Vacuumable.Size.NORMAL,
                99999,
                9999,
                out var slimeOBJ,
                out var plortOBJ,
                out var slimeAPP,
                out var slimeDEF);

            slimeOBJ.AddComponent<FlingOnTouchBehavior>();
            
            
        }

        public override void PostLoad()
        {
            CreateGordo(
                TEST_GORDO,
                TEST_SLIME,
                Identifiable.Id.PINK_GORDO,
                "Test Gordo",
                Color.green,
                Color.green,
                Color.green,
                "",
                ZoneDirector.Zone.RANCH,
                15,
                new List<GameObject>(),
                out var gordoDEF,
                out var gordoOBJ);

            SRCallbacks.PreSaveGameLoaded += sceneContext =>
            {
                var gordo = SRBehaviour.InstantiateDynamic(gordoOBJ, new Vector3(53.9358f, 12.33f, -98.78f), Quaternion.Euler(0f, 150.0974f, 0f));
                sceneContext.GameModel.RegisterGordo("testGordo", gordo);
            };
            
            CreateLargos(TEST_SLIME);
        }

        public static void CreateSlimeAndPlort(
            Identifiable.Id newSlimeID,
            Identifiable.Id newPlortID,
            Identifiable.Id baseSlimeDefinitionID,
            Identifiable.Id baseSlimeObjectID,
            Identifiable.Id basePlortID,
            string slimeName,
            string plortName,
            SlimeEat.FoodGroup[] foodGroups,
            Identifiable.Id[] favoriteFoods,
            string slimeIconPNGName,
            string plortIconPNGName,
            Color32 slimeVacColor,
            Color32 slimeTopSplatColor,
            Color32 slimeMiddleSplatColor,
            Color32 slimeBottomSplatColor,
            Color32 plortVacColor,
            Color32 topColor,
            Color32 middleColor,
            Color32 bottomColor,
            Vacuumable.Size slimeVacSize,
            float plortPrice,
            float plortSaturation,
            out GameObject slimeObject,
            out GameObject plortObject,
            out SlimeAppearance slimeApp,
            out SlimeDefinition slimeDef)
        {
            Sprite tex1 = null;
            Sprite tex2 = null;
            if (!string.IsNullOrEmpty(slimeIconPNGName))
                tex1 = TextureUtils.CreateSprite(TextureUtils.LoadImage(slimeIconPNGName));
            if (!string.IsNullOrEmpty(plortIconPNGName))
                tex2 = TextureUtils.CreateSprite(TextureUtils.LoadImage(plortIconPNGName));
            

            var slime = Library.Slime.CreateSlime(baseSlimeDefinitionID,
                baseSlimeObjectID,
                newSlimeID,
                slimeName,
                tex1,
                slimeVacColor,
                slimeTopSplatColor,
                slimeMiddleSplatColor,
                slimeBottomSplatColor,
                slimeVacColor,
                slimeVacSize);

            plortObject = Library.Slime.CreatePlort(
                basePlortID,
                newPlortID,
                plortName,
                tex2,
                plortVacColor,
                plortPrice,
                plortSaturation);

            Library.Slime.ColorSlime(newSlimeID, baseSlimeObjectID, topColor, middleColor, bottomColor, middleColor);
            Library.Slime.ColorPlort(newPlortID, topColor, middleColor, bottomColor);

            
            slimeObject = slime.Item2;
            slimeApp = slime.Item3;
            slimeDef = slime.Item1;
            
            slimeDef.Diet.Produces = new[] { newPlortID };
            slimeDef.Diet.Favorites = favoriteFoods;
            slimeDef.Diet.MajorFoodGroups = foodGroups;
        }

        public const SlimeRegistry.LargoProps DEFAULT_PROPS = SlimeRegistry.LargoProps.RECOLOR_BASE_MAT_AS_SLIME1 |
                                                              SlimeRegistry.LargoProps.RECOLOR_SLIME2_ADDON_MATS |
                                                              SlimeRegistry.LargoProps.INHERIT_STRIPE_FROM_SLIME2;
        public static void CreateLargos(Identifiable.Id moddedID, SlimeRegistry.LargoProps props = DEFAULT_PROPS)
        {
            foreach (var slime in Identifiable.SLIME_CLASS)
            {
                try
                {
                    var largo = largos[moddedID][slime];
                    Library.Largo.CreateLargo($"{moddedID.ToString().Split('_')[0].ToTitleCase()} {slime.ToString().Split('_')[0].ToTitleCase()} Largo",
                        moddedID,
                        slime,
                        largo,
                        0.5f,
                        1,
                        props);
                    
                    Identifiable.LARGO_CLASS.Add(largo);
                }
                catch { }
                
            }
        }
        public static void PreloadLargos(Identifiable.Id moddedID)
        {
            var dict = new Dictionary<Identifiable.Id, Identifiable.Id>();
            foreach (var slime in Identifiable.SLIME_CLASS)
            {
                try
                {
                    var largo = EnumPatcher.AddEnumValue<Identifiable.Id>(
                        moddedID.ToString().Split('_')[0] +
                        "_" +
                        slime.ToString().Split('_')[0] +
                        "_LARGO"
                    );
                    dict.Add(slime, largo);
                }
                catch { }
            }        
            largos.Add(moddedID, dict);
        }

        public static void CreateGordo(
            Identifiable.Id newGordoID,
            Identifiable.Id baseSlimeID,
            Identifiable.Id gordoPrefabID,
            string gordoName,
            Color32 topColor,
            Color32 middleColor,
            Color32 bottomColor,
            string mapIconPNGName,
            ZoneDirector.Zone zone,
            int targetEatCount,
            List<GameObject> popRewards,
            out SlimeDefinition gordoDef,
            out GameObject gordoObject)
        {
            
            Sprite tex = null;
            if (!string.IsNullOrEmpty(mapIconPNGName))
                tex = TextureUtils.CreateSprite(TextureUtils.LoadImage(mapIconPNGName));
            
            var gordo = Library.Slime.CreateGordo(
                gordoPrefabID,
                baseSlimeID,
                newGordoID,
                tex,
                gordoName,
                gordoName,
                zone,
                targetEatCount,
                popRewards);
            
            gordoObject = gordo.Item2;
            gordoDef = gordo.Item1;

            foreach (var renderer in gordoObject.GetComponentsInChildren<SkinnedMeshRenderer>())
            {
                renderer.material.SetColor("_TopColor", topColor);
                renderer.material.SetColor("_MiddleColor", middleColor);
                renderer.material.SetColor("_BottomColor", bottomColor);
            }
        }
        
        public static Dictionary<Identifiable.Id, Dictionary<Identifiable.Id, Identifiable.Id>> largos = new Dictionary<Identifiable.Id, Dictionary<Identifiable.Id, Identifiable.Id>>();
    }
}