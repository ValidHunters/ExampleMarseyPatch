using System.Reflection;
using Overlays;

public static class MarseyPatch
{
    // База
    public static Assembly RobustClient = null; 
    public static Assembly RobustShared = null;
    public static Assembly ContentClient = null;
    public static Assembly ContentShared = null;
    
    public static string Name = "Overlays Patch";
    public static string Description = "Патчит: эффект от космических наркотиков, от опьянения, от слепоты и разрушения зрения сваркой";
}

