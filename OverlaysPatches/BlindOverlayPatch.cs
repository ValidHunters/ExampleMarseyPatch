using HarmonyLib;
using System.Reflection;

namespace Overlays {
    [HarmonyPatch]
    public static class BlindOverlayPatch // Отключаем эффект от ослепления, выключая метод Draw https://github.com/space-wizards/space-station-14/blob/master/Content.Client/Eye/Blinding/BlindOverlay.cs
    {
        private static MethodBase TargetMethod() {
            var BlindOverlay = MarseyPatch.ContentClient.GetType("Content.Client.Eye.Blinding.BlindOverlay")!;
            return BlindOverlay.GetMethod("Draw", BindingFlags.NonPublic | BindingFlags.Instance);
        }

        [HarmonyPrefix]
        private static bool PrefSkip() {
            return false;
        }
    }
}
