using System.Reflection;
using HarmonyLib;

public static class MarseyPatch
{
    public static string ReqAsm = "RC"; // This defines which assembly is required for this patch. RC stands for Robust.Client, CC stands for Content.Client, et cetera.
    public static Assembly TargetAssembly = null; // This is defined by the MarseyPatcher, as it has access to the assemblies. 
    public static string Name = "FOV Disabler";
    public static string Description = "Disables Draw Occlusion Depth, disabling FOV and shadow rendering.";
}

[HarmonyPatch]
public static class DODPatch
{
    // This must return a MethodInfo for the function you're patching. In this case it's the function called "DrawOcclusionDepth" located in the "Robust.Client.Graphics.Clyde.Clyde" type.
    private static MethodBase TargetMethod() 
    {
        var tp = MarseyPatch.TargetAssembly.GetType("Robust.Client.Graphics.Clyde.Clyde"); // Namespace.ClassName
        return tp.GetMethod("DrawOcclusionDepth", BindingFlags.NonPublic | BindingFlags.Instance); // Function name, note the binding flags as the function is private.
    }

    // Prefix that will be executed before the original method
    // This is a simple "don't run this function at all" prefix, which returns false.
    [HarmonyPrefix]
    private static bool PrefSkip()
    {
        return false;
    }
}