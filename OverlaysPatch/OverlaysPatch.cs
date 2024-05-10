using System.Reflection;
using HarmonyLib;

public static class MarseyPatch
{
    public static string Name = "Overlays Patch v2";
    public static string Description = "Working now! Patches: DrunkOverlay, RainbowOverlay, BlurryVisionOverlay, BlindOverlay";
}

public static class MarseyLogger
{
    public enum LogType
    {
        INFO,
        WARN,
        FATL,
        DEBG
    }

    public delegate void Forward(AssemblyName asm, string message);

    public static Forward? logDelegate;

    /// <see cref="BasePatch.Finalizer"/>
    public static void Log(LogType type, string message)
    {
        logDelegate?.Invoke(Assembly.GetExecutingAssembly().GetName(), $"[{type.ToString()}] {message}");
    }
}

[HarmonyPatch]
public static class OverlaysPatch
{
    private static MethodInfo? GetOverlayBeforeDraw(string type)
        // Disable overlays by disabling BeforeDraw. For example: https://github.com/space-wizards/space-station-14/blob/master/Content.Client/Drunk/DrunkOverlay.cs
    {
        var tp = AccessTools.TypeByName(type);
        if (tp == null)
        {
            MarseyLogger.Log(MarseyLogger.LogType.WARN, $"Can't find overlay {type}.");
            return null;
        }

        var method = tp.GetMethod("BeforeDraw", BindingFlags.NonPublic | BindingFlags.Instance);
        if (method != null) return method;
        MarseyLogger.Log(MarseyLogger.LogType.WARN, $"Can't find BeforeDraw method in {type}.");
        return null;
    }

    private static IEnumerable<MethodBase> TargetMethods()
    {
        var drunkOverlay = GetOverlayBeforeDraw("Content.Client.Drunk.DrunkOverlay");
        if (drunkOverlay != null)
            yield return drunkOverlay;

        var rainbowOverlay = GetOverlayBeforeDraw("Content.Client.Drugs.RainbowOverlay");
        if (rainbowOverlay != null)
            yield return rainbowOverlay;

        var blurryVisionOverlay = GetOverlayBeforeDraw("Content.Client.Eye.Blinding.BlurryVisionOverlay");
        if (blurryVisionOverlay != null)
            yield return blurryVisionOverlay;

        var blindOverlay = GetOverlayBeforeDraw("Content.Client.Eye.Blinding.BlindOverlay");
        if (blindOverlay != null)
            yield return blindOverlay;
    }

    [HarmonyPrefix]
    static bool Prefix() => true;
}