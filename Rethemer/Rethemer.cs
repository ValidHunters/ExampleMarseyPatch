using System.Drawing;
using System.Reflection;
using System.Collections;
using System.Reflection.Emit;
using HarmonyLib;
using Rethemer;

/// <summary>
/// Changes colors for MenuButtons and StyleNano
/// This is not **full** control over colors.
/// Due to theming in general is a fucking shitshow upstream.
/// Which obviously nobody bothered to fix, nor will I!
///
/// See Style.cs for Style colors and Button.cs for button colors.
/// </summary>

// "STLYE SHEETS WERE A MISTAKE. KILL ALL OF THIS WITH FIRE" [sic]
// t. Jezithyr, 12.10.22

public static class MarseyPatch
{
    // This is defined by the MarseyPatcher, as it has access to the assemblies.
    public static Assembly RobustClient = null; 
    public static Assembly RobustShared = null;
    public static Assembly ContentClient = null;
    public static Assembly ContentShared = null;
    
    public static string Name = "Rethemer";
    public static string Description = "Changes the game's theme to your colors";
}

// This is a logging class used by the patch to send logging messages to the loader.
//           logging                                 logging
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
    
    /// <see cref="Restyle.Finalizer"/>
    public static void Log(LogType type, string message)
    {
        logDelegate?.Invoke(Assembly.GetExecutingAssembly().GetName(), $"[{type.ToString()}] {message}");
    }
}

[HarmonyPatch]
public static class Restyle
{
    // This returns a ctor for StyleNano class, from the Content.Client.Stylesheets namespace.
    // There is no ".cctor" method in the class, to return its constructor you return the Type's TypeInitializer.
    private static MethodBase TargetMethod() 
    {
        Type StyleNano = MarseyPatch.ContentClient.GetType("Content.Client.Stylesheets.StyleNano")!;
        return StyleNano.TypeInitializer;
    }

    // Transpilers alter the source code after they've been compiled.
    // Unlike prefix or postfix methods the changes are in the method itself.
    // However, you cannot edit C# code, you're editing IL code instead.
    // https://harmony.pardeike.net/articles/patching-transpiler.html
    [HarmonyTranspiler]
    public static IEnumerable<CodeInstruction> Retheme(IEnumerable<CodeInstruction> instructions)
    {
        var styleDictionaries = Dictionary.CreateStyleDictionaries(typeof(StyleNano), typeof(StyleMarsey));
        return Dictionary.ModifyInstructions(instructions, styleDictionaries);
    }

    // This executes after all other patch methods have finished.
    // If there was no exception - __exception would be null.
    // https://harmony.pardeike.net/articles/patching-finalizer.html
    [HarmonyFinalizer]
    static void Finalizer(Exception __exception)
    {
        if (__exception != null)
            MarseyLogger.Log(MarseyLogger.LogType.FATL, $"StyleSheet failed patch. \n {__exception.Message}");
        else
            MarseyLogger.Log(MarseyLogger.LogType.INFO, "StyleSheet patched.");
    }
}

[HarmonyPatch]
public static class Rebutton
{
    private static Dictionary<string, Dictionary<string, string>> styleDictionaries;
    // While its *possible* to have a single patch with TargetMethods instead of TargetMethod - it would be contrived and unreadable.
    // MarseyPatcher accepts multiple HarmonyPatch classes in a single assembly.
    private static MethodBase TargetMethod()
    {
        Type MenuButton = MarseyPatch.ContentClient.GetType("Content.Client.UserInterface.Controls.MenuButton");
        return MenuButton.TypeInitializer;
    }

    [HarmonyTranspiler]
    public static IEnumerable<CodeInstruction> Retheme(IEnumerable<CodeInstruction> instructions)
    {
        styleDictionaries = Dictionary.CreateStyleDictionaries(typeof(MenuButton), typeof(MenuButtonMarsey));
        return Dictionary.ModifyInstructions(instructions, styleDictionaries);
    }
    
    [HarmonyFinalizer]
    static void Finalizer(Exception __exception)
    {
        if (__exception != null)
            MarseyLogger.Log(MarseyLogger.LogType.FATL, $"MenuButton failed patch. \n {__exception.Message}");
        else
            MarseyLogger.Log(MarseyLogger.LogType.INFO, "MenuButton patched.");
    }
}