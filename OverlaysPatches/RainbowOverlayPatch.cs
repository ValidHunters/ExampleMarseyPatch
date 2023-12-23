using HarmonyLib;
using System.Reflection;

namespace Overlays {
    [HarmonyPatch]
    public static class RainbowOverlay // Отключаем эффект от наркоты, отключая метод Draw https://github.com/space-wizards/space-station-14/blob/master/Content.Client/Drugs/RainbowOverlay.cs
    {
        private static MethodBase TargetMethod() {
            var RainbowOverlay = MarseyPatch.ContentClient.GetType("Content.Client.Drugs.RainbowOverlay") !;
            return RainbowOverlay.GetMethod("Draw", BindingFlags.NonPublic | BindingFlags.Instance);
        }

        [HarmonyPrefix]
        private static bool PrefSkip() {
            return false;
        }
    }
}
