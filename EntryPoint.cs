using AssetsLib;
using SRML;
using SRML.Utils.Enum;
using UnityEngine;
using Library = ShortcutLib.Shortcut;
using static TestModForVideoSR1.CustomIds;
namespace TestModForVideoSR1
{
    public static class ColorToFloatExtensions { public static Color GetFloatVersion(this Color32 color) => new Color(color.r / 255, color.g / 255, color.b / 255, color.a / 255); }
    
    [EnumHolder]
    public class CustomIds
    {
        public static readonly Identifiable.Id TEST_SLIME;
        public static readonly Identifiable.Id TEST_PLORT;
    }

    public class EntryPoint : ModEntryPoint
    {
        public override void PreLoad() => HarmonyInstance.PatchAll();

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
    }
}