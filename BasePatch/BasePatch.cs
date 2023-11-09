using System.Reflection;
using HarmonyLib;

// MAIN FILE WITH PATCH DATA SHOULD NOT BE NAMESPACED
//namespace BasePatch;

/// <summary>
/// This class holds the data used by all 3 parts - the launcher, the loader and the patches themselves.
/// </summary>
public static class MarseyPatch
{
    // This is defined by the MarseyPatcher, as it has access to the assemblies.
    public static Assembly RobustClient = null; 
    public static Assembly RobustShared = null;
    public static Assembly ContentClient = null;
    public static Assembly ContentShared = null;
    
    public static string Name = "Base patch";
    public static string Description = "This genuinely does nothing, this is a patch base";
}

/// <summary>
/// This is a logging class used by the patch to send logging messages to the loader.
/// </summary>
public static class MarseyLogger
{
    // Info enums are identical to those in the loader however they cant be easily casted between the two
    public enum LogType
    {
        INFO,
        WARN,
        FATL,
        DEBG
    }

    // Delegate gets casted to Marsey::Utility::Log(AssemblyName, string) at runtime by the loader
    public delegate void Forward(AssemblyName asm, string message);
    
    public static Forward? logDelegate;
    
    /// <see cref="BasePatch.Finalizer"/>
    public static void Log(LogType type, string message)
    {
        logDelegate?.Invoke(Assembly.GetExecutingAssembly().GetName(), $"[{type.ToString()}] {message}");
    }
}

/// <summary>
/// This is a patch class, this is used by the Loader to alter functions in the game.
/// Patch classes are annotated with a HarmonyPatch attribute.
/// </summary>
[HarmonyPatch]
public class BasePatch
{
    /// <summary>
    /// This is a TargetMethod function, this tells the patcher which method is getting altered.
    /// In this case this will return EntryPoint::Init() from the Content.Client assembly.
    /// </summary>
    [HarmonyTargetMethod]
    public static MethodBase Target()
    {
        Type entryPointType = MarseyPatch.ContentClient.GetType("Content.Client.Entry.Entrypoint")!; // Namespace.Class
        MethodInfo entryPointInit = entryPointType.GetMethod("Init", BindingFlags.Public)!; // public override void Init(), thus the BindingFlags.Public
        return entryPointInit;
    }
    
    /// <summary>
    /// Your patch methods go between Target and Finalizer
    /// See other patches in the solution for examples.
    /// https://harmony.pardeike.net/articles/patching.html
    /// </summary>

    /// <summary>
    /// This executes after all other patch methods have finished.
    /// https://harmony.pardeike.net/articles/patching-finalizer.html
    /// </summary>
    /// <param name="__exception">The exception if any patch had failed, is null otherwise</param>
    [HarmonyFinalizer]
    static void Finalizer(Exception __exception)
    {
        if (__exception != null)
            MarseyLogger.Log(MarseyLogger.LogType.FATL, $"BasePatch failed . \n {__exception.Message}");
        else
            MarseyLogger.Log(MarseyLogger.LogType.INFO, "BasePatch patched.");
    }
}