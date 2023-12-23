using HarmonyLib;
using System.Reflection;

namespace Overlays {
    [HarmonyPatch]
    public static class DrunkOverlay // Отключаем эффект от алкашки, отключая метод Draw https://github.com/space-wizards/space-station-14/blob/master/Content.Client/Drunk/DrunkOverlay.cs
    {
        private static MethodBase TargetMethod() {
            var DrunkOverlay = MarseyPatch.ContentClient.GetType("Content.Client.Drunk.DrunkOverlay") !;
            return DrunkOverlay.GetMethod("Draw", BindingFlags.NonPublic | BindingFlags.Instance);
        }

        [HarmonyPrefix]
        private static bool PrefSkip() {
            return false;
        }
    }
}
