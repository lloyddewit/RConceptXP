using System;

namespace RConceptXP.ViewModels;

internal class BoxplotDataTransfer : ICloneable
{
    //todo convert to readonly properties?
    public string Comment;
    public string DataFrame;
    public string FacetBy;
    public string FacetByType;
    public string Factor;
    public string GroupToConnectSummary;
    public bool IsAddPoints;
    public bool IsBoxPlot;
    public bool IsBoxPlotExtra;
    public bool IsComment;
    public bool IsGroupToConnect;
    public bool IsHorizontalBoxPlot;
    public bool IsJitter;
    public bool IsLegend;
    public bool IsSaveGraph;
    public bool IsSingle;
    public bool IsTufte;
    public bool IsVarWidth;
    public bool IsViolin;
    public bool IsWidth;
    public string JitterExtra;
    public string LegendPosition;
    public string MultipleVariables;
    public string SaveName;
    public string SecondFactor;
    public int SelectedTabIndex;
    public string SingleVariable;
    public string Transparency;
    public string Width;
    public string WidthExtra;

    //constructor
    public BoxplotDataTransfer()
    {
        Comment = "Dialog: Boxplot Options";
        DataFrame = "survey"; // todo hard coded for testing
        FacetBy = "";
        FacetByType = "";
        Factor = "";
        GroupToConnectSummary = "";
        IsAddPoints = false;
        IsBoxPlot = true;
        IsBoxPlotExtra = false;
        IsComment = true;
        IsGroupToConnect = false;
        IsHorizontalBoxPlot = false;
        IsJitter = false;
        IsLegend = false;
        IsSaveGraph = false;
        IsSingle = true;
        IsTufte = false;
        IsVarWidth = false;
        IsViolin = false;
        IsWidth = false;
        JitterExtra = "0.20";
        LegendPosition = "";
        MultipleVariables = "";
        SaveName = "plot1";
        SecondFactor = "";
        SelectedTabIndex = 0;
        SingleVariable = "";
        Transparency = "1.00";
        Width = "0.25";
        WidthExtra = "0.5";
    }

    public BoxplotDataTransfer(BoxplotViewModel viewModel)
    {
        Comment = viewModel.Comment;
        DataFrame = viewModel.DataFrame;
        FacetBy = viewModel.FacetBy;
        FacetByType = viewModel.FacetByType;
        Factor = viewModel.Factor;
        GroupToConnectSummary = viewModel.GroupToConnectSummary;
        IsAddPoints = viewModel.IsAddPoints;
        IsBoxPlot = viewModel.IsBoxPlot;
        IsBoxPlotExtra = viewModel.IsBoxPlotExtra;
        IsComment = viewModel.IsComment;
        IsGroupToConnect = viewModel.IsGroupToConnect;
        IsHorizontalBoxPlot = viewModel.IsHorizontalBoxPlot;
        IsJitter = viewModel.IsJitter;
        IsLegend = viewModel.IsLegend;
        IsSaveGraph = viewModel.IsSaveGraph;
        IsSingle = viewModel.IsSingle;
        IsTufte = viewModel.IsTufte;
        IsVarWidth = viewModel.IsVarWidth;
        IsViolin = viewModel.IsViolin;
        IsWidth = viewModel.IsWidth;
        JitterExtra = viewModel.JitterExtra;
        LegendPosition = viewModel.LegendPosition;
        MultipleVariables = viewModel.MultipleVariables;
        SaveName = viewModel.SaveName;
        SecondFactor = viewModel.SecondFactor;
        SelectedTabIndex = viewModel.SelectedTabIndex;
        SingleVariable = viewModel.SingleVariable;
        Transparency = viewModel.Transparency;
        Width = viewModel.Width;
        WidthExtra = viewModel.WidthExtra;
    }

    public object Clone()
    {
        return this.MemberwiseClone();
    }

    public override bool Equals(object? obj)
    {
        if (obj is not BoxplotDataTransfer other)
            return false;

        return Comment == other.Comment &&
                   DataFrame == other.DataFrame &&
                   FacetBy == other.FacetBy &&
                   FacetByType == other.FacetByType &&
                   Factor == other.Factor &&
                   GroupToConnectSummary == other.GroupToConnectSummary &&
                   IsAddPoints == other.IsAddPoints &&
                   IsBoxPlot == other.IsBoxPlot &&
                   IsBoxPlotExtra == other.IsBoxPlotExtra &&
                   IsComment == other.IsComment &&
                   IsGroupToConnect == other.IsGroupToConnect &&
                   IsHorizontalBoxPlot == other.IsHorizontalBoxPlot &&
                   IsJitter == other.IsJitter &&
                   IsLegend == other.IsLegend &&
                   IsSaveGraph == other.IsSaveGraph &&
                   IsSingle == other.IsSingle &&
                   IsTufte == other.IsTufte &&
                   IsVarWidth == other.IsVarWidth &&
                   IsViolin == other.IsViolin &&
                   IsWidth == other.IsWidth &&
                   JitterExtra == other.JitterExtra &&
                   LegendPosition == other.LegendPosition &&
                   MultipleVariables == other.MultipleVariables &&
                   SaveName == other.SaveName &&
                   SecondFactor == other.SecondFactor &&
                   SelectedTabIndex == other.SelectedTabIndex &&
                   SingleVariable == other.SingleVariable &&
                   Transparency == other.Transparency &&
                   Width == other.Width &&
                   WidthExtra == other.WidthExtra;
    }

    // If we override the Equals method then we also need to override the GetHashCodeMethod
    // (or we get a compilation warning).
    public override int GetHashCode()
    {
        HashCode hash = new();
        hash.Add(Comment);
        hash.Add(DataFrame);
        hash.Add(FacetBy);
        hash.Add(FacetByType);
        hash.Add(Factor);
        hash.Add(GroupToConnectSummary);
        hash.Add(IsAddPoints);
        hash.Add(IsBoxPlot);
        hash.Add(IsBoxPlotExtra);
        hash.Add(IsComment);
        hash.Add(IsGroupToConnect);
        hash.Add(IsHorizontalBoxPlot);
        hash.Add(IsJitter);
        hash.Add(IsLegend);
        hash.Add(IsSaveGraph);
        hash.Add(IsSingle);
        hash.Add(IsTufte);
        hash.Add(IsVarWidth);
        hash.Add(IsViolin);
        hash.Add(IsWidth);
        hash.Add(JitterExtra);
        hash.Add(LegendPosition);
        hash.Add(MultipleVariables);
        hash.Add(SaveName);
        hash.Add(SecondFactor);
        hash.Add(SelectedTabIndex);
        hash.Add(SingleVariable);
        hash.Add(Transparency);
        hash.Add(Width);
        hash.Add(WidthExtra);
        return hash.ToHashCode();
    }

}
