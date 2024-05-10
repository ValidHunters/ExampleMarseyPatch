using System.Reflection;
using HarmonyLib;

public static class MarseyPatch
{
    public static string Name = "Overlays patch";

    public static string Description =
        "Disables Draw on multiple overlays";

    public static bool ignoreFields = true;
}


[HarmonyPatch]
/// <summary>
///  Disable overlays by disabling Draw. For example: https://github.com/space-wizards/space-station-14/blob/master/Content.Client/Drunk/DrunkOverlay.cs
/// </summary>
public static class OverlaysPatch
{
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
