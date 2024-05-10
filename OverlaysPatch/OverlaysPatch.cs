using System.Reflection;
using HarmonyLib;

public static class MarseyPatch
{
    public static string Name = "Overlays Patch v2";

    public static string Description =
        "Working now! The patch disables rendering of these overlays: DrunkOverlay, RainbowOverlay, BlurryVisionOverlay, BlindOverlay";

    public static bool ignoreFields = true;
}

[HarmonyPatch]
public static class OverlaysPatch
{
    /// <summary>
    ///  Disable overlays by disabling Draw. For example: https://github.com/space-wizards/space-station-14/blob/master/Content.Client/Drunk/DrunkOverlay.cs
    /// </summary>
    /// <param name="type">Path to type</param>
    /// <returns>MethodInfo</returns>
    private static MethodInfo GetOverlayDraw(string type)
    {
        return AccessTools.Method(AccessTools.TypeByName(type), "Draw");
    }

    private static IEnumerable<MethodBase> TargetMethods()
    {
        yield return GetOverlayDraw("Content.Client.Drunk.DrunkOverlay");
        yield return GetOverlayDraw("Content.Client.Drugs.RainbowOverlay");
        yield return GetOverlayDraw("Content.Client.Eye.Blinding.BlurryVisionOverlay");
        yield return GetOverlayDraw("Content.Client.Eye.Blinding.BlindOverlay");
    }

    [HarmonyPrefix]
    static bool Prefix() => false;
}