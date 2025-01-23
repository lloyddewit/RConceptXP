using System;

namespace RConceptXP.ViewModels;

internal class BoxplotDataTransfer : ICloneable
{
    public string? Comment;
    public bool? DataFrame;
    public bool? FacetBy;
    public bool? FacetByType;
    public bool? Factor;
    public bool? GroupToConnectSummary;
    public bool? IsAddPoints;
    public bool? IsBoxPlot;
    public bool? IsBoxPlotExtra;
    public bool? IsComment;
    public bool? IsGroupToConnect;
    public bool? IsHorizontalBoxPlot;
    public bool? IsJitter;
    public bool? IsLegend;
    public bool? IsSaveGraph;
    public bool? IsSingle;
    public bool? IsTufte;
    public bool? IsVarWidth;
    public bool? IsViolin;
    public bool? IsWidth;
    public bool? JitterExtra;
    public bool? LegendPosition;
    public bool? MultipleVariables;
    public bool? SaveName;
    public bool? SecondFactor;
    public bool? SelectedTabIndex;
    public bool? SingleVariable;
    public bool? Transparency;
    public bool? Width;
    public bool? WidthExtra;

    public object Clone()
    {
        return this.MemberwiseClone();
    }
}
