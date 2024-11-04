using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using RInsightF461;

namespace RConceptXP.Services
{

    /// <summary>
/// Each instance of this class specifies how to make a single update to an R model (a 
/// representation of a valid R script). The R model is updated based on the current state of an 
/// entry in a dictionary of configurable values.<para>
/// A list of instances of this class can be used to fully update an R model based upon the 
/// current state of the dictionary.</para>
/// </summary>
    public class clsTransformationRModel
    {

        /// <summary>
    /// Surround the updated value in the R model with quotes (e.g. 'f("myValue")').
    /// </summary>
        public bool bIsQuoted = false;

        /// <summary>
    /// Used when locating the function to update in the R model. If the function occurs more 
    /// than once in the statement, this value specifies which occurrence to update.
    /// </summary>
        public uint iOccurence;

        /// <summary>
    /// The parameter to update (starting from 0).
    /// </summary>
        public uint iParameterNumber;

        /// <summary>
    /// The statement number in the R model to update.
    /// </summary>
        public uint iStatementNumber;

        /// <summary>
    /// A list of child transformations to execute if the specified condition is successful.
    /// </summary>
        public List<clsTransformationRModel>? lstTransformations;

        /// <summary>
    /// The name of the function to update in the R model.
    /// </summary>
        public string? strFunctionName;

        /// <summary>
    /// The name of the parameter to update in the R model.
    /// </summary>
        public string? strParameterName;

        /// <summary>
    /// A valid R script to update the R model with.
    /// </summary>
        public string? strScript;

        /// <summary>
    /// Used as part of the condition in some transformations.
    /// </summary>
        public string strValueDefault = "TRUE";
        public string? strValueKey;

        /// <summary>
    /// The type of transformation to perform
    /// </summary>
        public enum TransformationType
        {
            functionAddParam,
            functionAddRemoveParamByName,
            functionRemoveParamByName,
            functionUpdateParamValue,
            ifFalseExecuteChildTransformations,
            ifTrueExecuteChildTransformations,
            operatorAddParam,
            operatorUpdateParam,
            operatorUpdateParamPresentation,
            quoteEmptyString
        }

        [JsonConverter(typeof(StringEnumConverter))]
        public TransformationType enumTransformationType = TransformationType.functionUpdateParamValue;


        /// <summary>
    /// Updates <paramref name="rScript"/> using <paramref name="dctConfigurableValues"/> and the 
    /// data members of this object.
    /// </summary>
    /// <param name="rScript">               The R model to update</param>
    /// <param name="dctConfigurableValues"> The dictionary of configurable values to reference 
    ///                                      when performing the transformation</param>
        public void updateRModel(RScript rScript, Dictionary<string, string> dctConfigurableValues)
        {

            string? strValue = string.IsNullOrEmpty(strValueKey) ? strScript : dctConfigurableValues[strValueKey];

            switch (enumTransformationType)
            {
                case TransformationType.functionAddParam:
                    {
                        rScript.FunctionAddParam(iStatementNumber, strFunctionName, strParameterName, strValue, iParameterNumber, bIsQuoted);
                        break;
                    }

                case TransformationType.functionAddRemoveParamByName:
                    {
                        if (string.IsNullOrEmpty(strValue) || (strValue ?? "") == (strValueDefault ?? ""))
                        {
                            rScript.FunctionRemoveParamByName(iStatementNumber, strFunctionName, strParameterName);
                        }
                        else
                        {
                            rScript.FunctionAddParam(iStatementNumber, strFunctionName, strParameterName, strValue, iParameterNumber, bIsQuoted);
                        }

                        break;
                    }

                case TransformationType.functionRemoveParamByName:
                    {
                        rScript.FunctionRemoveParamByName(iStatementNumber, strFunctionName, strParameterName);
                        break;
                    }

                case TransformationType.functionUpdateParamValue:
                    {
                        rScript.FunctionUpdateParamValue(iStatementNumber, strFunctionName, iParameterNumber, strValue, bIsQuoted, iOccurence);
                        break;
                    }

                case TransformationType.ifFalseExecuteChildTransformations:
                    {
                        if (!((strValue ?? "") == (strValueDefault ?? "")))
                        {
                            ExecuteChildTransformations(rScript, dctConfigurableValues);
                        }

                        break;
                    }

                case TransformationType.ifTrueExecuteChildTransformations:
                    {
                        if ((strValue ?? "") == (strValueDefault ?? ""))
                        {
                            ExecuteChildTransformations(rScript, dctConfigurableValues);
                        }

                        break;
                    }

                case TransformationType.operatorAddParam:
                    {
                        rScript.OperatorAddParam(iStatementNumber, strFunctionName, iParameterNumber, strValue);
                        break;
                    }

                case TransformationType.operatorUpdateParam:
                    {
                        rScript.OperatorUpdateParam(iStatementNumber, strFunctionName, iParameterNumber, strValue);
                        break;
                    }

                case TransformationType.operatorUpdateParamPresentation:
                    {
                        rScript.OperatorUpdateParamPresentation(iStatementNumber, strFunctionName, iParameterNumber, strValue);
                        break;
                    }

                case TransformationType.quoteEmptyString:
                    {
                        if (string.IsNullOrEmpty(strValue))
                        {
                            rScript.FunctionUpdateParamValue(iStatementNumber, strFunctionName, iParameterNumber, "", true);
                        }
                        else
                        {
                            rScript.FunctionUpdateParamValue(iStatementNumber, strFunctionName, iParameterNumber, strValue, bIsQuoted);
                        }

                        break;
                    }
            }
        }

        private void ExecuteChildTransformations(RScript rScript, Dictionary<string, string> dctConfigurableValues)
        {
            if (lstTransformations is null) return;
            
            foreach (clsTransformationRModel transform in lstTransformations)
                transform.updateRModel(rScript, dctConfigurableValues);
        }
    }
}