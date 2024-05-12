using System.Reflection;
using HarmonyLib;

public static class MarseyPatch
{
    public static string Name = "Command Permission Patch";

    public static string Description =
        "Disable permission checking on all commands";

    public static bool ignoreFields = true;
}

[HarmonyPatch]
/// <summary>
/// Sets the ability to run a command to always true via PostFix: https://github.com/space-wizards/RobustToolbox/blob/master/Robust.Client/Console/ClientConsoleHost.cs#L198
/// </summary>
static class CommandPermissionPatch
{
    [HarmonyTargetMethod]
    private static MethodBase TargetMethod()
    {
        return AccessTools.Method(AccessTools.TypeByName("Robust.Client.Console.ClientConsoleHost"), "CanExecute");
    }

    [HarmonyPostfix]
    private static void Postfix(ref bool __result)
    {
        __result = true;
    }
}