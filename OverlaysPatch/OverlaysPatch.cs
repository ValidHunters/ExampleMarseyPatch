using System.Reflection;
using HarmonyLib;

public static class MarseyPatch
{
    public static Assembly? RobustClient = null; 
    public static Assembly? RobustShared = null;
    public static Assembly? ContentClient = null;
    public static Assembly? ContentShared = null;
    
    public static string Name = "Overlays Patch";
    public static string Description = "Patches: DrunkOverlay, RainbowOverlay, BlurryVisionOverlay, BlindOverlay";
}

[HarmonyPatch]
    public static class OverlaysPatch 
    {        
    	[HarmonyTargetMethods]
    	private static IEnumerable<MethodBase> TargetMethods() 
        {
            var tp = MarseyPatch.RobustClient;
            Type? DrunkOverlay = tp.GetType("Content.Client.Drunk.DrunkOverlay"); // Disable drunk by disabling Draw https://github.com/space-wizards/space-station-14/blob/master/Content.Client/Drunk/DrunkOverlay.cs
            if (DrunkOverlay != null)
            {
            	MethodInfo? method = DrunkOverlay.GetMethod("Draw", BindingFlags.NonPublic | BindingFlags.Instance);
            	if (method != null) 
            	{
            		yield return method;
            	}
            }
            Type? RainbowOverlay = tp.GetType("Content.Client.Drugs.RainbowOverlay"); // Disable space drug effect by disabling method Draw https://github.com/space-wizards/space-station-14/blob/master/Content.Client/Drugs/RainbowOverlay.cs
            if (RainbowOverlay != null)
            {
            	MethodInfo? method = RainbowOverlay.GetMethod("Draw", BindingFlags.NonPublic | BindingFlags.Instance);
            	if (method != null) 
            	{
            		yield return method;
            	}
            }
            Type? BlurryVisionOverlay = tp.GetType("Content.Client.Eye.Blinding.BlurryVisionOverlay"); // Disable the blurred-eye effect by turning off the Draw method https://github.com/space-wizards/space-station-14/blob/master/Content.Client/Eye
            if (BlurryVisionOverlay != null)
            {
            	MethodInfo? method = BlurryVisionOverlay.GetMethod("Draw", BindingFlags.NonPublic | BindingFlags.Instance);
            	if (method != null) 
            	{
            		yield return method;
            	}
            }
            Type? BlindOverlay = tp.GetType("Content.Client.Eye.Blinding.BlindOverlay"); // Disable the effect of blinding by turning off the Draw method https://github.com/space-wizards/space-station-14/blob/master/Content.Client/Eye/Blinding/BlindOverlay.cs
    {
            if (BlindOverlay != null)
            {
            	MethodInfo? method = BlindOverlay.GetMethod("Draw", BindingFlags.NonPublic | BindingFlags.Instance);
            	if (method != null) 
            	{
            		yield return method;
            	}
            }
        }
	}
	[HarmonyPostfix]
	private static bool Postfix(bool value) => false;
}
