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

        public static Dictionary<Identifiable.Id, Dictionary<Identifiable.Id, Identifiable.Id>> largos = new Dictionary<Identifiable.Id, Dictionary<Identifiable.Id, Identifiable.Id>>();
    }
}