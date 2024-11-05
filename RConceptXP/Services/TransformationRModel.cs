using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using RInsightF461;

namespace RConceptXP.Services;

/// <summary>
/// Each instance of this class specifies how to make a single update to an R model (a 
/// representation of a valid R script). The R model is updated based on the current state of an 
/// entry in a dictionary of configurable values.<para>
/// A list of instances of this class can be used to fully update an R model based upon the 
/// current state of the dictionary.</para>
/// </summary>
public class TransformationRModel
{

    /// <summary>
    /// Surround the updated value in the R model with quotes (e.g. 'f("myValue")').
    /// </summary>
    public bool IsQuoted = false;

    /// <summary>
    /// Used when locating the function to update in the R model. If the function occurs more 
    /// than once in the statement, this value specifies which occurrence to update.
    /// </summary>
    public uint Occurence;

    /// <summary>
    /// The parameter to update (starting from 0).
    /// </summary>
    public uint ParameterNumber;

    /// <summary>
    /// The statement number in the R model to update.
    /// </summary>
    public uint StatementNumber;

    /// <summary>
    /// A list of child transformations to execute if the specified condition is successful.
    /// </summary>
    public List<TransformationRModel>? Transformations;

    /// <summary>
    /// The name of the function to update in the R model.
    /// </summary>
    public string? FunctionName;

    /// <summary>
    /// The name of the parameter to update in the R model.
    /// </summary>
    public string? ParameterName;

    /// <summary>
    /// A valid R script to update the R model with.
    /// </summary>
    public string? Script;

    /// <summary>
    /// Used as part of the condition in some transformations.
    /// </summary>
    public string ValueDefault = "TRUE";
    public string? ValueKey;

    /// <summary>
    /// The type of transformation to perform
    /// </summary>
    public enum TransformationTypes
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
    public TransformationTypes TransformationType = TransformationTypes.functionUpdateParamValue;


    /// <summary>
    /// Updates <paramref name="rModel"/> using <paramref name="configurableValues"/> and the 
    /// data members of this object.
    /// </summary>
    /// <param name="rModel">               The R model to update</param>
    /// <param name="configurableValues"> The dictionary of configurable values to reference 
    ///                                      when performing the transformation</param>
    public void UpdateRModel(RScript rModel, Dictionary<string, string> configurableValues)
    {

        string? configurableValue = string.IsNullOrEmpty(ValueKey) ? Script : configurableValues[ValueKey];

        switch (TransformationType)
        {
            case TransformationTypes.functionAddParam:
                {
                    rModel.FunctionAddParam(StatementNumber, FunctionName, ParameterName, configurableValue, ParameterNumber, IsQuoted);
                    break;
                }

            case TransformationTypes.functionAddRemoveParamByName:
                {
                    if (string.IsNullOrEmpty(configurableValue) || (configurableValue ?? "") == (ValueDefault ?? ""))
                    {
                        rModel.FunctionRemoveParamByName(StatementNumber, FunctionName, ParameterName);
                    }
                    else
                    {
                        rModel.FunctionAddParam(StatementNumber, FunctionName, ParameterName, configurableValue, ParameterNumber, IsQuoted);
                    }

                    break;
                }

            case TransformationTypes.functionRemoveParamByName:
                {
                    rModel.FunctionRemoveParamByName(StatementNumber, FunctionName, ParameterName);
                    break;
                }

            case TransformationTypes.functionUpdateParamValue:
                {
                    rModel.FunctionUpdateParamValue(StatementNumber, FunctionName, ParameterNumber, configurableValue, IsQuoted, Occurence);
                    break;
                }

            case TransformationTypes.ifFalseExecuteChildTransformations:
                {
                    if (!((configurableValue ?? "") == (ValueDefault ?? "")))
                    {
                        ExecuteChildTransformations(rModel, configurableValues);
                    }

                    break;
                }

            case TransformationTypes.ifTrueExecuteChildTransformations:
                {
                    if ((configurableValue ?? "") == (ValueDefault ?? ""))
                    {
                        ExecuteChildTransformations(rModel, configurableValues);
                    }

                    break;
                }

            case TransformationTypes.operatorAddParam:
                {
                    rModel.OperatorAddParam(StatementNumber, FunctionName, ParameterNumber, configurableValue);
                    break;
                }

            case TransformationTypes.operatorUpdateParam:
                {
                    rModel.OperatorUpdateParam(StatementNumber, FunctionName, ParameterNumber, configurableValue);
                    break;
                }

            case TransformationTypes.operatorUpdateParamPresentation:
                {
                    rModel.OperatorUpdateParamPresentation(StatementNumber, FunctionName, ParameterNumber, configurableValue);
                    break;
                }

            case TransformationTypes.quoteEmptyString:
                {
                    if (string.IsNullOrEmpty(configurableValue))
                    {
                        rModel.FunctionUpdateParamValue(StatementNumber, FunctionName, ParameterNumber, "", true);
                    }
                    else
                    {
                        rModel.FunctionUpdateParamValue(StatementNumber, FunctionName, ParameterNumber, configurableValue, IsQuoted);
                    }

                    break;
                }
        }
    }

    private void ExecuteChildTransformations(RScript rScript, Dictionary<string, string> dctConfigurableValues)
    {
        if (Transformations is null) return;

        foreach (TransformationRModel transform in Transformations)
            transform.UpdateRModel(rScript, dctConfigurableValues);
    }
}