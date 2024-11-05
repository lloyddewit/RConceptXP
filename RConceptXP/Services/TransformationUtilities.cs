using Newtonsoft.Json;
using RInsightF461;
using System;
using System.Collections.Generic;
using System.IO;

namespace RConceptXP.Services;

internal static class TransformationUtilities
{
    public static string GetRScript(string dialogName, Dictionary<string, string> dataBindings)
    {
        // Build the R model from the R script
        string scriptReset = File.ReadAllText(
                @"C:\Users\steph\source\repos\RConceptXP\RConceptXP\RViews\" 
                + dialogName + @"\" + dialogName + ".R");
        RScript rScript = new RScript(scriptReset);

        // Update the R model from the configurable values
        string transformationsRJson = File.ReadAllText(
                @"C:\Users\steph\source\repos\RConceptXP\RConceptXP\RViews\" 
                + dialogName + @"\" + $"{dialogName}.json");
        List<TransformationRModel>? transformationsToScript = 
                JsonConvert.DeserializeObject<List<TransformationRModel>>(transformationsRJson);
        if (transformationsToScript == null)
            throw new Exception("Failed to deserialize JSON");

        foreach (TransformationRModel transform in transformationsToScript)
        {
            transform.UpdateRModel(rScript, dataBindings);
        }

        return rScript.GetAsExecutableScript();
    }
}
