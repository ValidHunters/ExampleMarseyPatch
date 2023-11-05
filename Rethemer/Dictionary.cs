using System.Reflection;
using System.Reflection.Emit;
using HarmonyLib;

namespace Rethemer;

public class Dictionary
{
    public static Dictionary<string, Dictionary<string, string>> CreateStyleDictionaries(Type Original, Type Patched)
    {
        var styleDictionaries = new Dictionary<string, Dictionary<string, string>>();

        var fields1 = Original.GetFields(BindingFlags.Public | BindingFlags.Static);
        var fields2 = Patched.GetFields(BindingFlags.Public | BindingFlags.Static);

        foreach (var field1 in fields1)
        {
            // Find a matching field between two types
            var field2 = fields2.FirstOrDefault(f => f.Name == field1.Name);
            if (field2 != null)
            {
                var value1 = field1.GetValue(null).ToString();
                var value2 = field2.GetValue(null).ToString();

                // If styleDict does not contain entry - add one
                if (!styleDictionaries.ContainsKey(field1.Name))
                {
                    styleDictionaries.Add(field1.Name, new Dictionary<string, string>());
                }
                // Add respective field
                styleDictionaries[field1.Name][value1] = value2;
            }
        }

        return styleDictionaries;
    }
    
    public static IEnumerable<CodeInstruction> ModifyInstructions(IEnumerable<CodeInstruction> instructions, Dictionary<string, Dictionary<string, string>> styleDictionaries)
    {
        foreach (var instruction in instructions)
        {
            if (instruction.opcode == OpCodes.Ldstr && instruction.operand is string str)
            {
                foreach (var dict in styleDictionaries.Values)
                {
                    if (dict.ContainsKey(str))
                    {
                        instruction.operand = dict[str];
                        break;
                    }
                }
            }
            yield return instruction;
        }
    }

    public static void PrintChanges(Dictionary<string, Dictionary<string, string>> styleDictionaries)
    {
        // Iterate over each entry in the styleDictionaries.
        foreach (var entry in styleDictionaries)
        {
            // The key of the entry is the field name.
            var fieldName = entry.Key;
            // The value of the entry is another dictionary mapping field values.
            var fieldValueDict = entry.Value;

            // Iterate over each entry in the fieldValueDict.
            foreach (var fieldValueEntry in fieldValueDict)
            {
                // The key of the fieldValueEntry is the original field value.
                var originalFieldValue = fieldValueEntry.Key;
                // The value of the fieldValueEntry is the new field value.
                var newFieldValue = fieldValueEntry.Value;

                Console.WriteLine($"[RETHEMER] {fieldName}: {originalFieldValue} replaced to {newFieldValue}");
            }
        }
    }
}