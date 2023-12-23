using HarmonyLib;
using System.Reflection;

namespace Overlays {
    [HarmonyPatch]
    public static class BlurryVisionOverlayPatch // Отключаем эффект от помутненного зрения, выключая метод Draw https://github.com/space-wizards/space-station-14/blob/master/Content.Client/Eye/Blinding/BlurryVisionOverlay.cs
    {
        private static MethodBase TargetMethod() {
            var BlurryVisionOverlay = MarseyPatch.ContentClient.GetType("Content.Client.Eye.Blinding.BlurryVisionOverlay")!;
            return BlurryVisionOverlay.GetMethod("Draw", BindingFlags.NonPublic | BindingFlags.Instance);
        }

        [HarmonyPrefix]
        private static bool PrefSkip() {
            return false;
        }
    }
}
