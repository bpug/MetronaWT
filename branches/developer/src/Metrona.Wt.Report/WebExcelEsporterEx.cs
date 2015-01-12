// Type: Infragistics.Web.UI.GridControls.WebExcelExporter
// Assembly: Infragistics45.Web.v14.2, Version=14.2.20142.1028, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb
// MVID: 90743258-8925-461D-8901-B2378FD85AAF
// Assembly location: C:\Program Files (x86)\Infragistics\2014.2\ASP.NET\CLR4.5\Bin\Infragistics45.Web.v14.2.dll

using Infragistics.Documents.Excel;
using Infragistics.Documents.Reports;
using Infragistics.Web.UI;
using Infragistics.Web.UI.Framework;
using Infragistics.Web.UI.Framework.Data;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Infragistics.Web.UI.GridControls
{


    

    /// <summary>
    /// Provides Excel document exporting functionality to
    ///             the <see cref="T:Infragistics.Web.UI.GridControls.WebDataGrid"/> and
    ///             <see cref="T:Infragistics.Web.UI.GridControls.WebHierarchicalDataGrid"/>.
    /// 
    /// </summary>
    //[LicenseProvider(typeof(UltraLicenseProvider))]
    //[UltraLicense("WebExcelExporter")]
    [assembly: InternalsVisibleTo("Infragistics45.Web.v14.2")]
    [assembly: InternalsVisibleTo("Infragistics45.WebUI.Documents.Reports.v14.2")]
    [ToolboxData("<{0}:WebExcelExporter runat=\"server\"></{0}:WebExcelExporter>")]
    [ToolboxBitmap(typeof(WebExcelExporter), "Export.WebExcelExporter.png")]
    [Designer("Infragistics.Web.UI.Design.BaseExporterDesigner,Infragistics45.Web.Design.v14.2, Version=14.2.20142.1028, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb")]
    public class WebExcelExporterEx : BaseExporter
    {
        private static readonly object EventExporting = new object();
        private static readonly object EventExported = new object();
        private static readonly object EventRowExporting = new object();
        private static readonly object EventRowExported = new object();
        private static readonly object EventCellExporting = new object();
        private static readonly object EventCellExported = new object();
        private static readonly object EventGridFieldCaptionExporting = new object();
        private static readonly object EventGridFieldCaptionExported = new object();
        private static readonly object EventGridRecordItemExporting = new object();
        private static readonly object EventGridRecordItemExported = new object();
        private static readonly object EventSummaryCellExporting = new object();
        private static readonly object EventSummaryCellExported = new object();
        private int _gridsRowSpacing = 1;
        private const string DefaultFileExtension = "xls";
        private const string Excel2007FileExtension = "xlsx";
        private const string Excel2007MacroEnabledFileExtension = "xlsm";
        private const string DefaultWorksheetName = "WorkSheet1";
        private const string WorksheetNameFormat = "WorkSheet{0}";
        private const int MaxExcelFormulaParametersCount = 30;
        private const int DefaultExcelFontHeight = 240;
        private WorkbookFormat _workbookFormat;
        private WorkbookFormat _currentWorkbookFormat;
        private StyleSheet _currentStyleSheet;
        private bool _disableCellValueFormatting;
        private TemplateControlRender _tmlRender;

        /// <summary>
        /// Determines how many rows will be inserted between multiple grids when they are exported on a single worksheet.
        ///             The default value is 1.
        /// 
        /// </summary>
        [LocalizedDescription("LD_EE_P_GridsRowSpacing")]
        [LocalizedCategory("LC_Behavior")]
        [DefaultValue(1)]
        [NotifyParentProperty(true)]
        public int GridsRowSpacing
        {
            get
            {
                return this._gridsRowSpacing;
            }
            set
            {
                if (value < 0)
                    throw new ArgumentOutOfRangeException((string)null, (object)value, Infragistics.Web.UI.SR.GetString("EE_GridsRowSpacing_Range_Exception"));
                this._gridsRowSpacing = value;
            }
        }

        /// <summary>
        /// Determines the format of the exported excel file.
        /// 
        /// </summary>
        [LocalizedDescription("LD_P_WorkbookFormat")]
        [NotifyParentProperty(true)]
        [DefaultValue(WorkbookFormat.Excel97To2003)]
        [LocalizedCategory("LC_Behavior")]
        public WorkbookFormat WorkbookFormat
        {
            get
            {
                return this._workbookFormat;
            }
            set
            {
                this._workbookFormat = value;
            }
        }

        /// <summary>
        /// Returns the MimeType that should be used for the file that's being exported.
        /// 
        /// </summary>
        protected override string MimeType
        {
            get
            {
                switch (this.ExportMode)
                {
                    case ExportMode.InBrowser:
                        switch (this._currentWorkbookFormat)
                        {
                            case WorkbookFormat.Excel2007:
                                return "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                            case WorkbookFormat.Excel2007MacroEnabled:
                                return "application/vnd.ms-excel.sheet.macroEnabled.12";
                            case WorkbookFormat.Excel2007MacroEnabledTemplate:
                                return "application/vnd.ms-excel.template.macroEnabled.12";
                            case WorkbookFormat.Excel2007Template:
                                return "application/vnd.openxmlformats-officedocument.spreadsheetml.template";
                            default:
                                return "application/vnd.ms-excel";
                        }
                    case ExportMode.Download:
                        return "application/ms-excel";
                    default:
                        return string.Empty;
                }
            }
        }

        /// <summary>
        /// Returns the download extension for the workbook format being used
        /// 
        /// </summary>
        protected override string DownloadExtension
        {
            get
            {
                switch (this._currentWorkbookFormat)
                {
                    case WorkbookFormat.Excel2007:
                        return "xlsx";
                    case WorkbookFormat.Excel2007MacroEnabled:
                        return "xlsm";
                    default:
                        return "xls";
                }
            }
        }

        /// <summary>
        /// Disables formatting of the cell value when grid BoundDataField or UnboundField has DataFormatString specified.
        /// 
        /// </summary>
        [NotifyParentProperty(true)]
        [LocalizedDescription("LD_P_DisableCellValueFormatting")]
        [DefaultValue(false)]
        [LocalizedCategory("LC_Behavior")]
        public bool DisableCellValueFormatting
        {
            get
            {
                return this._disableCellValueFormatting;
            }
            set
            {
                this._disableCellValueFormatting = value;
            }
        }

        private TemplateControlRender TemplateRender
        {
            get
            {
                if (this._tmlRender == null)
                    this._tmlRender = new TemplateControlRender();
                return this._tmlRender;
            }
        }

        /// <summary>
        /// Occurs before the exporting start.
        /// 
        /// </summary>
        [LocalizedDescription("LD_EE_E_Exporting")]
        public event ExcelExportingEventHandler Exporting
        {
            add
            {
                this.Events.AddHandler(WebExcelExporter.EventExporting, (Delegate)value);
            }
            remove
            {
                this.Events.RemoveHandler(WebExcelExporter.EventExporting, (Delegate)value);
            }
        }

        /// <summary>
        /// Occurs when exporting is complete.
        /// 
        /// </summary>
        [LocalizedDescription("LD_EE_E_Exported")]
        public event ExcelExportedEventHandler Exported
        {
            add
            {
                this.Events.AddHandler(WebExcelExporter.EventExported, (Delegate)value);
            }
            remove
            {
                this.Events.RemoveHandler(WebExcelExporter.EventExported, (Delegate)value);
            }
        }

        /// <summary>
        /// Occurs before row exporting start.
        /// 
        /// </summary>
        [LocalizedDescription("LD_EE_E_RowExporting")]
        public event ExcelRowExportingEventHandler RowExporting
        {
            add
            {
                this.Events.AddHandler(WebExcelExporter.EventRowExporting, (Delegate)value);
            }
            remove
            {
                this.Events.RemoveHandler(WebExcelExporter.EventRowExporting, (Delegate)value);
            }
        }

        /// <summary>
        /// Occurs when row exporting is complete.
        /// 
        /// </summary>
        [LocalizedDescription("LD_EE_E_RowExported")]
        public event ExcelRowExportedEventHandler RowExported
        {
            add
            {
                this.Events.AddHandler(WebExcelExporter.EventRowExported, (Delegate)value);
            }
            remove
            {
                this.Events.RemoveHandler(WebExcelExporter.EventRowExported, (Delegate)value);
            }
        }

        /// <summary>
        /// Occurs before grid field caption item exporting starts.
        /// 
        /// </summary>
        public event GridFieldCaptionExportingEventHandler GridFieldCaptionExporting
        {
            add
            {
                this.Events.AddHandler(WebExcelExporter.EventGridFieldCaptionExporting, (Delegate)value);
            }
            remove
            {
                this.Events.RemoveHandler(WebExcelExporter.EventGridFieldCaptionExporting, (Delegate)value);
            }
        }

        /// <summary>
        /// Occurs when grid field caption exporting is complete.
        /// 
        /// </summary>
        public event GridFieldCaptionExportedEventHandler GridFieldCaptionExported
        {
            add
            {
                this.Events.AddHandler(WebExcelExporter.EventGridFieldCaptionExported, (Delegate)value);
            }
            remove
            {
                this.Events.RemoveHandler(WebExcelExporter.EventGridFieldCaptionExported, (Delegate)value);
            }
        }

        /// <summary>
        /// Occurs before grid record item exporting starts.
        /// 
        /// </summary>
        public event GridRecordItemExportingEventHandler GridRecordItemExporting
        {
            add
            {
                this.Events.AddHandler(WebExcelExporter.EventGridRecordItemExporting, (Delegate)value);
            }
            remove
            {
                this.Events.RemoveHandler(WebExcelExporter.EventGridRecordItemExporting, (Delegate)value);
            }
        }

        /// <summary>
        /// Occurs when grid record item exporting is complete.
        /// 
        /// </summary>
        public event GridRecordItemExportedEventHandler GridRecordItemExported
        {
            add
            {
                this.Events.AddHandler(WebExcelExporter.EventGridRecordItemExported, (Delegate)value);
            }
            remove
            {
                this.Events.RemoveHandler(WebExcelExporter.EventGridRecordItemExported, (Delegate)value);
            }
        }

        /// <summary>
        /// Occurs before summary cell exporting starts.
        /// 
        /// </summary>
        public event SummaryCellExportingEventHandler SummaryCellExporting
        {
            add
            {
                this.Events.AddHandler(WebExcelExporter.EventSummaryCellExporting, (Delegate)value);
            }
            remove
            {
                this.Events.RemoveHandler(WebExcelExporter.EventSummaryCellExporting, (Delegate)value);
            }
        }

        /// <summary>
        /// Occurs when summary cell exporting is complete.
        /// 
        /// </summary>
        public event SummaryCellExportedEventHandler SummaryCellExported
        {
            add
            {
                this.Events.AddHandler(WebExcelExporter.EventSummaryCellExported, (Delegate)value);
            }
            remove
            {
                this.Events.RemoveHandler(WebExcelExporter.EventSummaryCellExported, (Delegate)value);
            }
        }

        /// <summary>
        /// Occurs before cell exporting start.
        ///             This event is obsolete. Use GridFieldCaptionExporting, GridRecordItemExporting, SummaryCellExporting instead.
        /// 
        /// </summary>
        [LocalizedDescription("LD_EE_E_CellExporting")]
        [Obsolete("Use GridFieldCaptionExporting, GridRecordItemExporting, SummaryCellExporting instead.")]
        public event ExcelCellExportingEventHandler CellExporting
        {
            add
            {
                this.Events.AddHandler(WebExcelExporter.EventCellExporting, (Delegate)value);
            }
            remove
            {
                this.Events.RemoveHandler(WebExcelExporter.EventCellExporting, (Delegate)value);
            }
        }

        /// <summary>
        /// Occurs when cell exporting is complete.
        ///             This event is obsolete. Use GridFieldCaptionExported, GridRecordItemExported, SummaryCellExported instead.
        /// 
        /// </summary>
        [Obsolete("Use GridFieldCaptionExported, GridRecordItemExported, SummaryCellExported instead.")]
        [LocalizedDescription("LD_EE_E_CellExported")]
        public event ExcelCellExportedEventHandler CellExported
        {
            add
            {
                this.Events.AddHandler(WebExcelExporter.EventCellExported, (Delegate)value);
            }
            remove
            {
                this.Events.RemoveHandler(WebExcelExporter.EventCellExported, (Delegate)value);
            }
        }

        static WebExcelExporterEx()
        {
        }

        /// <summary>
        /// Exports a single flat grid.
        /// 
        /// </summary>
        public override void Export(WebDataGrid grid)
        {
            Workbook workbook = new Workbook(this.WorkbookFormat);
            this.Export(grid, workbook);
        }

        /// <summary>
        /// Exports a single hierarchical grid.
        /// 
        /// </summary>
        public override void Export(WebHierarchicalDataGrid grid)
        {
            Workbook workbook = new Workbook(this.WorkbookFormat);
            this.Export(grid, workbook);
        }

        /// <summary>
        /// Exports a single hierarchical grid to the specified workbook.
        /// 
        /// </summary>
        public virtual void Export(WebHierarchicalDataGrid grid, Workbook workbook)
        {
            Worksheet worksheet = workbook.Worksheets.Add("WorkSheet1");
            this.Export(grid, worksheet);
        }

        /// <summary>
        /// Exports a single hierarchical grid to the specified worksheet.
        /// 
        /// </summary>
        public virtual void Export(WebHierarchicalDataGrid grid, Worksheet worksheet)
        {
            this.Export(grid, worksheet, 0, 0);
        }

        /// <summary>
        /// Exports a single hierarchical grid to the specified worksheet.
        ///             Exporting starts from the defined row and column offsets.
        /// 
        /// </summary>
        public virtual void Export(WebHierarchicalDataGrid grid, Worksheet worksheet, int rowOffset, int columnOffset)
        {
            this._currentWorkbookFormat = worksheet.Workbook.CurrentFormat;
            ExcelExportingEventArgs e = new ExcelExportingEventArgs(worksheet, rowOffset, columnOffset, 0);
            this.OnExporting(e);
            if (e.Cancel)
                return;
            int currentRowIndex = e.CurrentRowIndex;
            this.InitStyleSheet(grid.RunBot);
            this.ExportHierarchicalGrid(grid, worksheet, ref currentRowIndex, columnOffset);
            this._currentStyleSheet = (StyleSheet)null;
            this.OnExported(new ExcelExportedEventArgs(worksheet, currentRowIndex, columnOffset, 0));
            this.DownloadWorkbook(worksheet.Workbook);
        }

        /// <summary>
        /// Exports a single hierarchical grid to the specified worksheet.
        ///             Exporting starts from the defined rowIndex and columnOffset.
        ///             The rowIndex is updated to point after the last exported row.
        /// 
        /// </summary>
        protected virtual void ExportHierarchicalGrid(WebHierarchicalDataGrid grid, Worksheet worksheet, ref int rowIndex, int columnOffset)
        {
            int initialDataBindDepth = grid.InitialDataBindDepth;
            bool shouldEnableVirtualScrolling = false;
            bool shouldEnablePaging = false;
            VirtualScrolling behavior1 = grid.Behaviors.GetBehavior<VirtualScrolling>();
            Paging behavior2 = grid.Behaviors.GetBehavior<Paging>();
            this.RebindToAllData(ref shouldEnableVirtualScrolling, ref shouldEnablePaging, behavior1, behavior2);
            if (this.DataExportMode == DataExportMode.AllDataInDataSource)
            {
                grid.InitialDataBindDepth = -1;
                grid.GridView.RequireRecordsClear = true;
                grid.CollapseAll();
            }
            this.ExportGrid((WebDataGrid)grid.GridView, worksheet, ref rowIndex, columnOffset, 0);
            this.RestoreBinding(shouldEnableVirtualScrolling, shouldEnablePaging, behavior1, behavior2);
            grid.InitialDataBindDepth = initialDataBindDepth;
        }

        /// <summary>
        /// Exports multiple grids to a single worksheet.
        /// 
        /// </summary>
        public override void Export(params WebControl[] grids)
        {
            this.Export(false, grids);
        }

        /// <summary>
        /// Exports multiple grids. Depending on the exportSingleGridPerSheet parameter the grids are exported to a single
        ///             worksheet or each grid on a separate sheet.
        /// 
        /// </summary>
        public virtual void Export(bool exportSingleGridPerSheet, params WebControl[] grids)
        {
            Workbook workbook = new Workbook(this.WorkbookFormat);
            if (exportSingleGridPerSheet)
                this.Export(workbook, 0, 0, grids);
            else
                this.Export(workbook.Worksheets.Add("WorkSheet1"), 0, 0, grids);
        }

        /// <summary>
        /// Exports multiple grids on a separate worksheets in the specified workbook.
        /// 
        /// </summary>
        public virtual void Export(Workbook workbook, int rowOffset, int columnOffset, params WebControl[] grids)
        {
            this._currentWorkbookFormat = workbook.CurrentFormat;
            ExcelExportingEventArgs e = new ExcelExportingEventArgs((Worksheet)null, rowOffset, columnOffset, 0);
            this.OnExporting(e);
            if (e.Cancel)
                return;
            rowOffset = e.CurrentRowIndex;
            int num = 1;
            foreach (object obj in grids)
            {
                int rowIndex = rowOffset;
                Worksheet worksheet = workbook.Worksheets.Add(string.Format("WorkSheet{0}", (object)num));
                if (obj is WebDataGrid)
                {
                    this.InitStyleSheet(((FlatDataBoundControl)obj).RunBot);
                    this.ExportGrid((WebDataGrid)obj, worksheet, ref rowIndex, columnOffset, 0);
                    this._currentStyleSheet = (StyleSheet)null;
                }
                else if (obj is WebHierarchicalDataGrid)
                {
                    this.InitStyleSheet(((HierarchicalDataBoundControlMain)obj).RunBot);
                    this.ExportHierarchicalGrid((WebHierarchicalDataGrid)obj, worksheet, ref rowIndex, columnOffset);
                    this._currentStyleSheet = (StyleSheet)null;
                }
                else
                    throw new InvalidOperationException(Infragistics.Web.UI.SR.GetString("EE_CONTROL_NOT_SUPPORTED_EXCEPTION", new object[1]
          {
            (object) obj.GetType().Name
          }));
                ++num;
            }
            this.OnExported(new ExcelExportedEventArgs((Worksheet)null, rowOffset, columnOffset, 0));
            this.DownloadWorkbook(workbook);
        }

        /// <summary>
        /// Exports multiple grids on a single worksheet.
        /// 
        /// </summary>
        public virtual void Export(Worksheet worksheet, int rowOffset, int columnOffset, params WebControl[] grids)
        {
            this._currentWorkbookFormat = worksheet.Workbook.CurrentFormat;
            ExcelExportingEventArgs e = new ExcelExportingEventArgs(worksheet, rowOffset, columnOffset, 0);
            this.OnExporting(e);
            if (e.Cancel)
                return;
            int currentRowIndex = e.CurrentRowIndex;
            foreach (object obj in grids)
            {
                if (obj is WebDataGrid)
                {
                    this.InitStyleSheet(((FlatDataBoundControl)obj).RunBot);
                    this.ExportGrid((WebDataGrid)obj, worksheet, ref currentRowIndex, columnOffset, 0);
                    this._currentStyleSheet = (StyleSheet)null;
                    currentRowIndex += this._gridsRowSpacing;
                }
                else if (obj is WebHierarchicalDataGrid)
                {
                    this.InitStyleSheet(((HierarchicalDataBoundControlMain)obj).RunBot);
                    this.ExportHierarchicalGrid((WebHierarchicalDataGrid)obj, worksheet, ref currentRowIndex, columnOffset);
                    this._currentStyleSheet = (StyleSheet)null;
                    currentRowIndex += this._gridsRowSpacing;
                }
                else
                    throw new InvalidOperationException(Infragistics.Web.UI.SR.GetString("EE_CONTROL_NOT_SUPPORTED_EXCEPTION", new object[1]
          {
            (object) obj.GetType().Name
          }));
            }
            this.OnExported(new ExcelExportedEventArgs(worksheet, currentRowIndex, columnOffset, 0));
            this.DownloadWorkbook(worksheet.Workbook);
        }

        public virtual void Export(Worksheet worksheet, params WebDataGridExport[] grids)
        {
            this._currentWorkbookFormat = worksheet.Workbook.CurrentFormat;
            var e = new ExcelExportingEventArgs(worksheet, grids[0].RowOffset, grids[0].ColumnOffset, 0);
            this.OnExporting(e);
            if (e.Cancel)
            {
                return;
            }
            int currentRowIndex = e.CurrentRowIndex;
            foreach (var grid in grids)
            {
                int rowIndex = grid.RowOffset;
                this.InitStyleSheet(grid.WebDataGrid.RunBot);
                this.ExportGrid(grid.WebDataGrid, worksheet, ref rowIndex, grid.ColumnOffset, 0);
                this._currentStyleSheet = (StyleSheet)null;
                currentRowIndex += this._gridsRowSpacing;
            }
            this.OnExported(new ExcelExportedEventArgs(worksheet, currentRowIndex, columnOffset, 0));
            this.DownloadWorkbook(worksheet.Workbook);
        }

        /// <summary>
        /// Exports a set of row islands to the specified worksheet.
        /// 
        /// </summary>
        protected virtual void ExportRowIslands(ContainerGridCollection rowIslands, Worksheet worksheet, ref int rowIndex, int columnOffset, int outlineLevel)
        {
            foreach (WebDataGrid grid in (CollectionBase)rowIslands)
                this.ExportGrid(grid, worksheet, ref rowIndex, columnOffset, outlineLevel);
        }

        /// <summary>
        /// Exports a single flat grid to the specified workbook.
        /// 
        /// </summary>
        public virtual void Export(WebDataGrid grid, Workbook workbook)
        {
            Worksheet worksheet = workbook.Worksheets.Add("WorkSheet1");
            this.Export(grid, worksheet);
        }

        /// <summary>
        /// Exports a single flat grid to the specified worksheet.
        /// 
        /// </summary>
        public virtual void Export(WebDataGrid grid, Worksheet worksheet)
        {
            this.Export(grid, worksheet, 0, 0);
        }

        /// <summary>
        /// Exports a single flat grid to the specified worksheet.
        ///             Exporting starts from the defined row and column offsets.
        /// 
        /// </summary>
        public virtual void Export(WebDataGrid grid, Worksheet worksheet, int rowOffset, int columnOffset)
        {
            this._currentWorkbookFormat = worksheet.Workbook.CurrentFormat;
            ExcelExportingEventArgs e = new ExcelExportingEventArgs(worksheet, rowOffset, columnOffset, 0);
            this.OnExporting(e);
            if (e.Cancel)
                return;
            rowOffset = e.CurrentRowIndex;
            this.InitStyleSheet(grid.RunBot);
            this.ExportGrid(grid, worksheet, ref rowOffset, columnOffset, 0);
            this._currentStyleSheet = (StyleSheet)null;
            this.OnExported(new ExcelExportedEventArgs(worksheet, rowOffset, columnOffset, 0));
            this.DownloadWorkbook(worksheet.Workbook);
        }

        /// <summary>
        /// Exports a single flat grid to the specified worksheet.
        ///             Exporting starts from the defined rowIndex and columnOffset.
        ///             The rowIndex is updated to point after the last exported row.
        /// 
        /// </summary>
        protected virtual void ExportGrid(WebDataGrid grid, Worksheet worksheet, ref int rowIndex, int columnOffset, int outlineLevel)
        {
            bool hidden = grid is ContainerGrid && !this.IsGridVisible((ContainerGrid)grid);
            bool hasHiddenCols = false;
            foreach (GridField gridField in (CollectionBase)grid.Fields)
            {
                if (gridField.Hidden || gridField.HiddenByParent)
                {
                    hasHiddenCols = true;
                    break;
                }
            }
            string styleClassString1 = grid.RunBot.StyleBot.GetStyleClassString(0, grid.ControlStyle.CssClass);
            string styleClassString2 = grid.RunBot.StyleBot.GetStyleClassString(5, grid.ItemCssClass);
            string styleClassString3 = grid.RunBot.StyleBot.GetStyleClassString(6, grid.AltItemCssClass);
            if (grid.ShowHeader)
            {
                WorksheetRow worksheetRow1 = worksheet.Rows[rowIndex];
                worksheetRow1.OutlineLevel = outlineLevel;
                worksheetRow1.Hidden = hidden;
                ExcelRowExportingEventArgs exportingEventArgs = new ExcelRowExportingEventArgs(worksheet, worksheetRow1, rowIndex, columnOffset, outlineLevel, true, false, false);
                string styleClassString4 = grid.RunBot.StyleBot.GetStyleClassString(1);
                if (grid.Fields.Count == 0)
                    grid.EnsureDataBoundInternal();
                int columnIndex1 = 0;
                if (grid.HasHeaderLayout)
                {
                    for (int index = 0; index < grid.HeaderLayout.Count; ++index)
                    {
                        int columnIndex2 = 0;
                        WorksheetRow worksheetRow2 = worksheet.Rows[rowIndex];
                        worksheetRow2.OutlineLevel = outlineLevel;
                        worksheetRow2.Hidden = hidden;
                        ExcelRowExportingEventArgs e = new ExcelRowExportingEventArgs(worksheet, worksheetRow2, rowIndex, columnOffset, outlineLevel, true, false, false);
                        this.OnRowExporting(e);
                        if (!e.Cancel)
                        {
                            foreach (ControlDataField controlDataField in (IVisibleItemsEnumerable)grid.HeaderLayout[index])
                            {
                                if (!controlDataField.Hidden && !controlDataField.HiddenByParent && (!(controlDataField is GroupField) || !((GroupField)controlDataField).HiddenByChildren))
                                {
                                    GridFieldCaption caption = controlDataField.HeaderCaption as GridFieldCaption;
                                    WorksheetCell wsCell = worksheetRow2.Cells[columnOffset + columnIndex2];
                                    int rowSpan = controlDataField.RowSpanInternal == 0 ? 1 : controlDataField.RowSpanInternal;
                                    int colSpan = controlDataField is GroupField ? (controlDataField as GroupField).ColSapn : 1;
                                    for (; wsCell.AssociatedMergedCellsRegion != null; wsCell = worksheetRow2.Cells[wsCell.AssociatedMergedCellsRegion.LastColumn + 1])
                                        columnIndex2 = wsCell.AssociatedMergedCellsRegion.LastColumn + 1 - columnOffset;
                                    this.ExportGridFieldCaption(grid, caption, worksheetRow2, wsCell, rowIndex, columnIndex2, styleClassString1, styleClassString4, true, rowSpan, colSpan, columnOffset);
                                    columnIndex2 += colSpan;
                                }
                            }
                            this.OnRowExported(new ExcelRowExportedEventArgs(worksheet, worksheetRow2, rowIndex, columnOffset, outlineLevel, true, false, false));
                            ++rowIndex;
                        }
                    }
                }
                else
                {
                    ExcelRowExportingEventArgs e = new ExcelRowExportingEventArgs(worksheet, worksheetRow1, rowIndex, columnOffset, outlineLevel, true, false, false);
                    this.OnRowExporting(e);
                    if (e.Cancel)
                        return;
                    foreach (ControlDataField controlDataField in (IVisibleItemsEnumerable)grid.Fields)
                    {
                        if (!controlDataField.Hidden)
                        {
                            GridFieldCaption caption = controlDataField.HeaderCaption as GridFieldCaption;
                            WorksheetCell wsCell = worksheetRow1.Cells[columnOffset + columnIndex1];
                            this.ExportGridFieldCaption(grid, caption, worksheetRow1, wsCell, rowIndex, columnIndex1, styleClassString1, styleClassString4, true, 0, 0, columnOffset);
                            ++columnIndex1;
                        }
                    }
                    this.OnRowExported(new ExcelRowExportedEventArgs(worksheet, worksheetRow1, rowIndex, columnOffset, outlineLevel, true, false, false));
                    ++rowIndex;
                }
            }
            bool flag1 = false;
            bool flag2 = false;
            VirtualScrolling behavior1 = grid.Behaviors.GetBehavior<VirtualScrolling>();
            Paging behavior2 = grid.Behaviors.GetBehavior<Paging>();
            if (this.DataExportMode == DataExportMode.AllDataInDataSource)
            {
                if (behavior1 != null && behavior1.Enabled)
                {
                    behavior1.Enabled = false;
                    flag1 = true;
                }
                if (behavior2 != null && behavior2.Enabled)
                {
                    behavior2.Enabled = false;
                    flag2 = true;
                }
            }
            int firstDataRowIndex = rowIndex;
            if (grid is ContainerGrid && ((ContainerGrid)grid).HasGroupedRows && ((ContainerGrid)grid).GroupedRows.Count > 0)
            {
                this.ExportGroupedRows(((ContainerGrid)grid).GroupedRows, worksheet, ref rowIndex, columnOffset, hasHiddenCols, outlineLevel, hidden);
            }
            else
            {
                foreach (GridRecord gridRecord in (ControlDataRecordCollection)grid.Rows)
                {
                    string itemCssClass = styleClassString2;
                    if (gridRecord.Index % 2 == 1)
                        itemCssClass = itemCssClass + " " + styleClassString3;
                    if (!string.IsNullOrEmpty(gridRecord.CssClass))
                        itemCssClass = itemCssClass + " " + gridRecord.CssClass;
                    this.ExportRow(gridRecord, worksheet, ref rowIndex, columnOffset, hasHiddenCols, outlineLevel, hidden, styleClassString1, itemCssClass);
                }
            }
            if (flag1)
                behavior1.Enabled = true;
            if (flag2)
                behavior2.Enabled = true;
            if (grid.ShowFooter)
            {
                WorksheetRow worksheetRow = worksheet.Rows[rowIndex];
                worksheetRow.OutlineLevel = outlineLevel;
                worksheetRow.Hidden = hidden;
                ExcelRowExportingEventArgs e = new ExcelRowExportingEventArgs(worksheet, worksheetRow, rowIndex, columnOffset, outlineLevel, false, true, false);
                this.OnRowExporting(e);
                if (e.Cancel)
                    return;
                string styleClassString4 = grid.RunBot.StyleBot.GetStyleClassString(3);
                int columnIndex = 0;
                foreach (ControlDataField controlDataField in (IVisibleItemsEnumerable)grid.Fields)
                {
                    if (!controlDataField.Hidden && !controlDataField.HiddenByParent && (!(controlDataField is GroupField) || !((GroupField)controlDataField).HiddenByChildren))
                    {
                        GridFieldFooterCaption fieldFooterCaption = controlDataField.FooterCaption as GridFieldFooterCaption;
                        WorksheetCell wsCell = worksheetRow.Cells[columnOffset + columnIndex];
                        int colSpanResolved = fieldFooterCaption.ColSpanResolved;
                        if (wsCell.AssociatedMergedCellsRegion == null)
                            this.ExportGridFieldCaption(grid, (GridFieldCaption)fieldFooterCaption, worksheetRow, wsCell, rowIndex, columnIndex, styleClassString1, styleClassString4, false, 1, colSpanResolved, columnOffset);
                        ++columnIndex;
                    }
                }
                this.OnRowExported(new ExcelRowExportedEventArgs(worksheet, worksheetRow, rowIndex, columnOffset, outlineLevel, false, true, false));
                ++rowIndex;
            }
            SummaryRow behavior3 = grid.Behaviors.GetBehavior<SummaryRow>();
            if (behavior3 == null || !behavior3.Enabled)
                return;
            this.ExportSummaries(grid, worksheet, ref rowIndex, columnOffset, outlineLevel, hidden, styleClassString1, firstDataRowIndex);
        }

        /// <summary>
        /// Exports a single grid row to the specified worksheet.
        /// 
        /// </summary>
        protected virtual void ExportRow(GridRecord gridRecord, Worksheet worksheet, ref int rowIndex, int columnOffset, bool hasHiddenCols, int outlineLevel, bool hidden, string gridCssClass, string itemCssClass)
        {
            WorksheetRow worksheetRow = worksheet.Rows[rowIndex];
            worksheetRow.OutlineLevel = outlineLevel;
            worksheetRow.Hidden = hidden;
            int rowIndex1 = rowIndex;
            ExcelRowExportingEventArgs e = new ExcelRowExportingEventArgs(worksheet, worksheetRow, rowIndex1, columnOffset, outlineLevel, gridRecord, false, false, false);
            this.OnRowExporting(e);
            if (e.Cancel)
                return;
            WebDataGrid webDataGrid = gridRecord.Owner.ControlMain as WebDataGrid;
            int num = 0;
            foreach (ControlDataField controlDataField in (IVisibleItemsEnumerable)webDataGrid.Fields)
            {
                if (!controlDataField.Hidden && !controlDataField.HiddenByParent)
                {
                    this.ExportCell(gridRecord.Items[controlDataField.Index], worksheetRow, rowIndex, columnOffset + num, gridCssClass, itemCssClass);
                    ++num;
                }
            }
            ++rowIndex;
            if (gridRecord is ContainerGridRecord && ((ContainerGridRecord)gridRecord).HasRowIslands)
                this.ExportRowIslands(((ContainerGridRecord)gridRecord).RowIslands, worksheet, ref rowIndex, columnOffset + 1, outlineLevel + 1);
            this.OnRowExported(new ExcelRowExportedEventArgs(worksheet, worksheetRow, rowIndex1, columnOffset, outlineLevel, gridRecord, false, false, false));
        }

        /// <summary>
        /// Exports a grid field caption (header or footer) to the specified worksheet cell.
        /// 
        /// </summary>
        protected virtual void ExportGridFieldCaption(WebDataGrid grid, GridFieldCaption caption, WorksheetRow wsRow, WorksheetCell wsCell, int rowIndex, int columnIndex, string gridCssClass, string captionRowCssClass, bool isHeader, int rowSpan, int colSpan, int colOffset)
        {
            WorksheetMergedCellsRegion mergedCellsRegion = rowSpan > 1 || colSpan > 1 ? wsRow.Worksheet.MergedCellsRegions.Add(rowIndex, columnIndex + colOffset, rowIndex + rowSpan - 1, columnIndex + colOffset + colSpan - 1) : (WorksheetMergedCellsRegion)null;
            ExcelCellExportingEventArgs e1 = new ExcelCellExportingEventArgs(wsRow.Worksheet, wsCell, rowIndex, columnIndex, wsRow.OutlineLevel, isHeader, !isHeader, false);
            this.OnCellExporting(e1);
            if (e1.Cancel)
                return;
            GridFieldCaptionExportingEventArgs e2 = new GridFieldCaptionExportingEventArgs(caption, wsRow.Worksheet, wsCell, rowIndex, columnIndex, wsRow.OutlineLevel, isHeader, !isHeader, false);
            this.OnGridFieldCaptionExporting(e2);
            if (e2.Cancel)
                return;
            if (mergedCellsRegion != null)
                mergedCellsRegion.Value = (object)caption.Text;
            else
                wsCell.Value = (object)caption.Text;
            if (this.EnableStylesExport)
            {
                string styleClassString = grid.RunBot.StyleBot.GetStyleClassString((int)caption.Role, caption.CssClassResolved);
                if (caption.OwnerField is GroupField && caption.Role == WebDataGridRoles.HeaderCaption)
                    styleClassString = grid.RunBot.StyleBot.GetStyleClassString(10, styleClassString);
                CssSelector selector1 = new CssSelector();
                selector1.AddClasses(string.Format("{0} {1} {2}", (object)gridCssClass, (object)captionRowCssClass, (object)styleClassString));
                CssStyle styleObject1 = this._currentStyleSheet.GetStyleObject(selector1);
                this.ApplyCellFormatFromStyle(wsCell.Worksheet.Workbook, wsCell.CellFormat, styleObject1, grid);
                if (mergedCellsRegion != null)
                    this.ApplyCellFormatFromStyle(mergedCellsRegion.Worksheet.Workbook, mergedCellsRegion.CellFormat, styleObject1, grid);
                CssSelector selector2 = new CssSelector();
                selector2.AddClasses(styleClassString);
                CssStyle styleObject2 = this._currentStyleSheet.GetStyleObject(selector2);
                this.ApplyCellBorderFromStyle(wsCell.CellFormat, styleObject2);
                if (mergedCellsRegion != null)
                    this.ApplyCellBorderFromStyle(mergedCellsRegion.CellFormat, styleObject2);
            }
            if (wsCell.Value != null)
            {
                this.SetColumnWidth(wsCell.Worksheet.Columns[wsCell.ColumnIndex], wsCell);
                if (wsCell.Value.ToString().Contains(Environment.NewLine))
                    wsCell.CellFormat.WrapText = ExcelDefaultableBoolean.True;
            }
            if (mergedCellsRegion != null && mergedCellsRegion.Value != null && mergedCellsRegion.Value.ToString().Contains(Environment.NewLine))
                mergedCellsRegion.CellFormat.WrapText = ExcelDefaultableBoolean.True;
            this.OnGridFieldCaptionExported(new GridFieldCaptionExportedEventArgs(caption, wsRow.Worksheet, wsCell, rowIndex, columnIndex, wsRow.OutlineLevel, isHeader, !isHeader, false));
            this.OnCellExported(new ExcelCellExportedEventArgs(wsRow.Worksheet, wsCell, rowIndex, columnIndex, wsRow.OutlineLevel, isHeader, !isHeader, false));
        }

        /// <summary>
        /// Exports a grid record item to its corresponding worksheet cell.
        /// 
        /// </summary>
        protected virtual void ExportCell(GridRecordItem item, WorksheetRow worksheetRow, int rowIndex, int columnIndex, string gridCssClass, string itemCssClass)
        {
            WorksheetCell worksheetCell = worksheetRow.Cells[columnIndex];
            ExcelCellExportingEventArgs e1 = new ExcelCellExportingEventArgs(worksheetRow.Worksheet, worksheetCell, rowIndex, columnIndex, worksheetRow.OutlineLevel, false, false, false);
            this.OnCellExporting(e1);
            if (e1.Cancel)
                return;
            GridRecordItemExportingEventArgs e2 = new GridRecordItemExportingEventArgs(item, worksheetRow.Worksheet, worksheetCell, rowIndex, columnIndex, worksheetRow.OutlineLevel, false, false, false);
            this.OnGridRecordItemExporting(e2);
            if (e2.Cancel)
                return;
            worksheetCell.Value = !item.HasTemplate ? (!(item.Column is FormattedGridField) || this.DisableCellValueFormatting || string.IsNullOrEmpty(((FormattedGridField)item.Column).DataFormatString) ? item.Value : (object)string.Format(((FormattedGridField)item.Column).DataFormatString, item.Value)) : (object)this.RenderTemplate(item.TemplateContainer);
            if (this.EnableStylesExport)
            {
                if (!string.IsNullOrEmpty(item.CssClass))
                    itemCssClass = itemCssClass + " " + item.CssClass;
                if (!string.IsNullOrEmpty(item.Column.CssClass))
                    itemCssClass = itemCssClass + " " + item.Column.CssClass;
                CssSelector selector1 = new CssSelector();
                selector1.AddClasses(string.Format("{0} {1}", (object)gridCssClass, (object)itemCssClass));
                CssStyle styleObject1 = this._currentStyleSheet.GetStyleObject(selector1);
                this.ApplyCellFormatFromStyle(worksheetCell.Worksheet.Workbook, worksheetCell.CellFormat, styleObject1, (WebDataGrid)item.Row.ControlMain);
                CssSelector selector2 = new CssSelector();
                selector2.AddClasses(itemCssClass);
                CssStyle styleObject2 = this._currentStyleSheet.GetStyleObject(selector2);
                this.ApplyCellBorderFromStyle(worksheetCell.CellFormat, styleObject2);
            }
            if (worksheetCell.Value != null)
            {
                this.SetColumnWidth(worksheetRow.Worksheet.Columns[columnIndex], worksheetCell);
                if (worksheetCell.Value.ToString().Contains(Environment.NewLine))
                    worksheetCell.CellFormat.WrapText = ExcelDefaultableBoolean.True;
            }
            this.OnGridRecordItemExported(new GridRecordItemExportedEventArgs(item, worksheetRow.Worksheet, worksheetCell, rowIndex, columnIndex, worksheetRow.OutlineLevel, false, false, false));
            this.OnCellExported(new ExcelCellExportedEventArgs(worksheetRow.Worksheet, worksheetCell, rowIndex, columnIndex, worksheetRow.OutlineLevel, false, false, false));
        }

        /// <summary>
        /// Renders the specified TemplateContainer to string and returns the value.
        /// 
        /// </summary>
        protected virtual string RenderTemplate(TemplateContainer container)
        {
            try
            {
                string str = this.TemplateRender.RenderControlToString((Control)container);
                if (str != null)
                    return str.Trim();
                else
                    return string.Empty;
            }
            catch
            {
                return string.Empty;
            }
        }

        /// <summary>
        /// Exports set of grouped rows to the specified worksheet.
        /// 
        /// </summary>
        protected virtual void ExportGroupedRows(GroupedRecordCollection groupedRows, Worksheet worksheet, ref int rowIndex, int columnOffset, bool hasHiddenCols, int outlineLevel, bool hidden)
        {
            ContainerGrid owner = groupedRows.Owner;
            WebHierarchicalDataGrid mainGrid = owner.MainGrid;
            string styleClassString1 = mainGrid.RunBot.StyleBot.GetStyleClassString(0, mainGrid.ControlStyle.CssClass);
            string styleClassString2 = owner.RunBot.StyleBot.GetStyleClassString(5, owner.ItemCssClass);
            string styleClassString3 = owner.RunBot.StyleBot.GetStyleClassString(6, owner.AltItemCssClass);
            string styleClassString4 = mainGrid.RunBot.StyleBot.GetStyleClassString(11, owner.Band != null ? owner.Band.GroupingSettings.GroupedRowCssClassResolved : "");
            int count = groupedRows.Owner.Fields.Count;
            if (hasHiddenCols)
            {
                foreach (ControlDataField controlDataField in (CollectionBase)groupedRows.Owner.Fields)
                {
                    if (controlDataField.Hidden || controlDataField.HiddenByParent)
                        --count;
                }
            }
            foreach (GroupedRecord groupedRecord in (CollectionBase)groupedRows)
            {
                WorksheetRow worksheetRow = worksheet.Rows[rowIndex];
                worksheetRow.OutlineLevel = outlineLevel;
                worksheetRow.Hidden = hidden;
                WorksheetMergedCellsRegion mergedCellsRegion = worksheet.MergedCellsRegions.Add(rowIndex, columnOffset, rowIndex, columnOffset + count - 1);
                mergedCellsRegion.Value = (object)groupedRecord.Text;
                if (this.EnableStylesExport)
                {
                    CssSelector selector1 = new CssSelector();
                    selector1.AddClasses(string.Format("{0} {1}", (object)styleClassString1, (object)styleClassString4));
                    CssStyle styleObject1 = this._currentStyleSheet.GetStyleObject(selector1);
                    this.ApplyCellFormatFromStyle(worksheet.Workbook, mergedCellsRegion.CellFormat, styleObject1, (WebDataGrid)owner);
                    CssSelector selector2 = new CssSelector();
                    selector2.AddClasses(styleClassString4);
                    CssStyle styleObject2 = this._currentStyleSheet.GetStyleObject(selector2);
                    this.ApplyCellBorderFromStyle(mergedCellsRegion.CellFormat, styleObject2);
                }
                ++rowIndex;
                bool hidden1 = hidden || !groupedRecord.Expanded;
                if (groupedRecord.HasChildGroupRows)
                {
                    this.ExportGroupedRows(groupedRecord.ChildGroupRows, worksheet, ref rowIndex, columnOffset, hasHiddenCols, outlineLevel + 1, hidden1);
                }
                else
                {
                    groupedRecord.Rows.Reset();
                    while (groupedRecord.Rows.MoveNext())
                    {
                        ContainerGridRecord containerGridRecord = (ContainerGridRecord)groupedRecord.Rows.Current;
                        string itemCssClass = styleClassString2;
                        if (containerGridRecord.Index % 2 == 1)
                            itemCssClass = itemCssClass + " " + styleClassString3;
                        if (!string.IsNullOrEmpty(containerGridRecord.CssClass))
                            itemCssClass = itemCssClass + " " + containerGridRecord.CssClass;
                        this.ExportRow((GridRecord)containerGridRecord, worksheet, ref rowIndex, columnOffset, hasHiddenCols, outlineLevel + 1, hidden1, styleClassString1, itemCssClass);
                    }
                }
            }
        }

        /// <summary>
        /// Exports grid SummaryRow to the specified worksheet.
        /// 
        /// </summary>
        protected virtual void ExportSummaries(WebDataGrid grid, Worksheet worksheet, ref int rowIndex, int columnOffset, int outlineLevel, bool hidden, string gridCssClass, int firstDataRowIndex)
        {
            SummaryRow behavior = grid.Behaviors.GetBehavior<SummaryRow>();
            int index1 = rowIndex - (grid.ShowFooter ? 2 : 1);
            bool renderingResolved = behavior.EnableCompactRenderingResolved;
            int summaryNumberVisible = behavior.ColumnSummaries.GetMaxSummaryNumberVisible();
            int maxSummaryRows = behavior.ColumnSummaries.GetMaxSummaryRows();
            string str1 = string.Empty;
            if (this.EnableStylesExport)
                str1 = behavior.ResolveCssClass(behavior.SummariesCssClass, 0);
            for (int index2 = 0; index2 < maxSummaryRows; ++index2)
            {
                if ((index2 < summaryNumberVisible || !renderingResolved) && (renderingResolved || behavior.ColumnSummaries.IsVisibleRow(index2)))
                {
                    WorksheetRow worksheetRow = worksheet.Rows[rowIndex];
                    worksheetRow.OutlineLevel = outlineLevel;
                    worksheetRow.Hidden = hidden;
                    int rowIndex1 = rowIndex;
                    ExcelRowExportingEventArgs e1 = new ExcelRowExportingEventArgs(worksheet, worksheetRow, rowIndex1, columnOffset, outlineLevel, false, false, true);
                    this.OnRowExporting(e1);
                    if (!e1.Cancel)
                    {
                        int columnIndex = columnOffset;
                        foreach (ControlDataField summaryCell in (IVisibleItemsEnumerable)grid.Fields)
                        {
                            if (!summaryCell.Hidden && !summaryCell.HiddenByParent)
                            {
                                WorksheetCell worksheetCell = worksheetRow.Cells[columnIndex];
                                Summary summaryByOrder = behavior.ColumnSummaries.GetSummaryByOrder(summaryCell.Key, index2);
                                SummarySetting summarySetting = (SummarySetting)null;
                                ExcelCellExportingEventArgs e2 = new ExcelCellExportingEventArgs(worksheet, worksheetCell, rowIndex, columnIndex, outlineLevel, false, false, true);
                                e2.Summary = summaryByOrder;
                                this.OnCellExporting(e2);
                                if (e2.Cancel)
                                {
                                    ++columnIndex;
                                }
                                else
                                {
                                    SummaryCellExportingEventArgs e3 = new SummaryCellExportingEventArgs(summaryCell, worksheet, worksheetCell, rowIndex, columnIndex, outlineLevel, false, false, true);
                                    e3.Summary = summaryByOrder;
                                    this.OnSummaryCellExporting(e3);
                                    if (e3.Cancel)
                                    {
                                        ++columnIndex;
                                    }
                                    else
                                    {
                                        if (summaryByOrder == null)
                                        {
                                            worksheetCell.Value = (object)behavior.EmptyFooterText;
                                        }
                                        else
                                        {
                                            SummaryRowSetting sumRowSetting = behavior.ColumnSettings[summaryCell.Key];
                                            summarySetting = sumRowSetting != null ? (summaryByOrder.SummaryType == SummaryType.Custom ? sumRowSetting.SummarySettings.CustomSummaryByName(summaryByOrder.CustomSummaryName) : sumRowSetting.SummarySettings[summaryByOrder.SummaryType]) : (SummarySetting)null;
                                            string format1 = summarySetting == null ? (sumRowSetting == null ? behavior.FormatString : sumRowSetting.FormatString) : summarySetting.FormatString;
                                            if (summaryByOrder.SummaryType == SummaryType.Custom)
                                                worksheetCell.Value = (object)string.Format(format1, (object)summaryByOrder.CustomSummaryName, summaryByOrder.Value);
                                            else if (summaryByOrder.SummaryType == SummaryType.Custom)
                                            {
                                                worksheetCell.Value = (object)string.Format(format1, (object)summaryByOrder.CustomSummaryName, summaryByOrder.Value);
                                            }
                                            else
                                            {
                                                List<string> list = (List<string>)null;
                                                string str2 = string.Empty;
                                                string str3;
                                                if (grid is ContainerGrid && ((ContainerGrid)grid).HasChildGrids())
                                                {
                                                    list = new List<string>();
                                                    for (int index3 = firstDataRowIndex; index3 <= index1; ++index3)
                                                    {
                                                        if (worksheet.Rows[index3].OutlineLevel == outlineLevel)
                                                            list.Add(worksheet.Rows[index3].Cells[columnIndex].ToString(CellReferenceMode.A1, false, true, true));
                                                    }
                                                    str3 = string.Join(",", list.ToArray());
                                                }
                                                else
                                                    str3 = string.Format("{0}:{1}", (object)worksheet.Rows[firstDataRowIndex].Cells[columnIndex].ToString(CellReferenceMode.A1, false, true, true), (object)worksheet.Rows[index1].Cells[columnIndex].ToString(CellReferenceMode.A1, false, true, true));
                                                int num1 = format1.IndexOf("{1");
                                                string format2;
                                                if (num1 > -1)
                                                {
                                                    string str4 = "\"" + format1.Substring(0, num1) + "\" & ";
                                                    string str5 = format1.Substring(num1);
                                                    int num2 = str5.IndexOf("}");
                                                    string str6 = str5.Insert(num2 + 1, " & \"") + "\"";
                                                    format2 = "=" + str4 + str6;
                                                }
                                                else
                                                    format2 = "=\"" + format1 + "\"";
                                                string format3 = string.Format(format2, (object)"{0}", (object)"{1}({3})").Replace("\"\" &", "").Replace("& \"\"", "");
                                                if (summaryByOrder.SummaryType == SummaryType.Count && format3.Contains("{1"))
                                                    format3 = format3.Replace("{1}({3})", "{1}({3}) + {2}({3})");
                                                string[] strArray = this.GetExcelFunctionName(summaryByOrder.SummaryType).Split(new char[1]
                        {
                          '|'
                        }, StringSplitOptions.RemoveEmptyEntries);
                                                string str7 = strArray.Length > 1 ? strArray[1] : "";
                                                string str8 = string.Format(format3, (object)behavior.GetCultureInvariantSummaryString(summaryByOrder.SummaryType, summaryByOrder.CustomSummaryName), (object)strArray[0], (object)str7, (object)str3);
                                                if (list != null && (list.Count > 30 || summaryByOrder.SummaryType == SummaryType.Count))
                                                    worksheetCell.Value = (object)behavior.GetSummaryValue((GridField)summaryCell, summaryByOrder, behavior.ColumnSummaries[summaryCell.Key], sumRowSetting, false);
                                                else
                                                    worksheetCell.ApplyFormula(str8);
                                            }
                                        }
                                        if (this.EnableStylesExport)
                                        {
                                            string classes = str1;
                                            if (summarySetting != null && !string.IsNullOrEmpty(summarySetting.CssClass))
                                                classes = classes + " " + summarySetting.CssClass;
                                            if (summaryByOrder != null && !string.IsNullOrEmpty(summaryByOrder.CssClass))
                                                classes = classes + " " + summaryByOrder.CssClass;
                                            CssSelector selector1 = new CssSelector();
                                            selector1.AddClasses(string.Format("{0} {1}", (object)gridCssClass, (object)classes));
                                            CssStyle styleObject1 = this._currentStyleSheet.GetStyleObject(selector1);
                                            this.ApplyCellFormatFromStyle(worksheetCell.Worksheet.Workbook, worksheetCell.CellFormat, styleObject1, grid);
                                            CssSelector selector2 = new CssSelector();
                                            selector2.AddClasses(classes);
                                            CssStyle styleObject2 = this._currentStyleSheet.GetStyleObject(selector2);
                                            this.ApplyCellBorderFromStyle(worksheetCell.CellFormat, styleObject2);
                                        }
                                        SummaryCellExportedEventArgs e4 = new SummaryCellExportedEventArgs(summaryCell, worksheet, worksheetCell, rowIndex1, columnIndex, outlineLevel, false, false, true);
                                        e4.Summary = summaryByOrder;
                                        this.OnSummaryCellExported(e4);
                                        ExcelCellExportedEventArgs e5 = new ExcelCellExportedEventArgs(worksheet, worksheetCell, rowIndex1, columnIndex, outlineLevel, false, false, true);
                                        e5.Summary = summaryByOrder;
                                        this.OnCellExported(e5);
                                        ++columnIndex;
                                    }
                                }
                            }
                        }
                        ++rowIndex;
                        this.OnRowExported(new ExcelRowExportedEventArgs(worksheet, worksheetRow, rowIndex1, columnOffset, outlineLevel, false, false, true));
                    }
                }
            }
        }

        /// <summary>
        /// Takes a SummaryType as an argument and returns the corresponding Excel function name.
        ///             Returns null for SummaryType.Custom.
        /// 
        /// </summary>
        protected virtual string GetExcelFunctionName(SummaryType summaryType)
        {
            switch (summaryType)
            {
                case SummaryType.Count:
                    return "COUNTA|COUNTBLANK";
                case SummaryType.Min:
                    return "MIN";
                case SummaryType.Max:
                    return "MAX";
                case SummaryType.Average:
                    return "AVERAGE";
                case SummaryType.Sum:
                    return "SUM";
                default:
                    return (string)null;
            }
        }

        private bool IsGridVisible(ContainerGrid containerGrid)
        {
            for (ContainerGridRecord parentRow = containerGrid.ParentRow; parentRow != null; parentRow = parentRow.OwnerControl.ParentRow)
            {
                if (!parentRow.Expanded)
                    return false;
            }
            return true;
        }

        /// <summary>
        /// Downloads the work book to the browser
        /// 
        /// </summary>
        /// <param name="workbook">The workbook to be downloaded</param>
        protected virtual void DownloadWorkbook(Workbook workbook)
        {
            if (this.ExportMode == ExportMode.Custom)
                return;
            MemoryStream memoryStream = new MemoryStream();
            workbook.Save((Stream)memoryStream, (WorkbookSaveOptions)null);
            this.DownloadDocument(memoryStream.ToArray());
        }

        /// <summary>
        /// Raises the Exporting Event.
        /// 
        /// </summary>
        protected virtual void OnExporting(ExcelExportingEventArgs e)
        {
            ExcelExportingEventHandler exportingEventHandler = this.Events[WebExcelExporter.EventExporting] as ExcelExportingEventHandler;
            if (exportingEventHandler == null)
                return;
            exportingEventHandler((object)this, e);
        }

        /// <summary>
        /// Raises the Exported Event.
        /// 
        /// </summary>
        protected virtual void OnExported(ExcelExportedEventArgs e)
        {
            ExcelExportedEventHandler exportedEventHandler = this.Events[WebExcelExporter.EventExported] as ExcelExportedEventHandler;
            if (exportedEventHandler == null)
                return;
            exportedEventHandler((object)this, e);
        }

        /// <summary>
        /// Raises the Row Exporting Event.
        /// 
        /// </summary>
        protected virtual void OnRowExporting(ExcelRowExportingEventArgs e)
        {
            ExcelRowExportingEventHandler exportingEventHandler = this.Events[WebExcelExporter.EventRowExporting] as ExcelRowExportingEventHandler;
            if (exportingEventHandler == null)
                return;
            exportingEventHandler((object)this, e);
        }

        /// <summary>
        /// Raises the Row Exported Event.
        /// 
        /// </summary>
        protected virtual void OnRowExported(ExcelRowExportedEventArgs e)
        {
            ExcelRowExportedEventHandler exportedEventHandler = this.Events[WebExcelExporter.EventRowExported] as ExcelRowExportedEventHandler;
            if (exportedEventHandler == null)
                return;
            exportedEventHandler((object)this, e);
        }

        /// <summary>
        /// Raises the Cell Exporting Event.
        /// 
        /// </summary>
        protected virtual void OnCellExporting(ExcelCellExportingEventArgs e)
        {
            ExcelCellExportingEventHandler exportingEventHandler = this.Events[WebExcelExporter.EventCellExporting] as ExcelCellExportingEventHandler;
            if (exportingEventHandler == null)
                return;
            exportingEventHandler((object)this, e);
        }

        /// <summary>
        /// Raises the Cell Exported Event.
        /// 
        /// </summary>
        protected virtual void OnCellExported(ExcelCellExportedEventArgs e)
        {
            ExcelCellExportedEventHandler exportedEventHandler = this.Events[WebExcelExporter.EventCellExported] as ExcelCellExportedEventHandler;
            if (exportedEventHandler == null)
                return;
            exportedEventHandler((object)this, e);
        }

        /// <summary>
        /// Raises the GridFieldCaptionExporting Event.
        /// 
        /// </summary>
        protected virtual void OnGridFieldCaptionExporting(GridFieldCaptionExportingEventArgs e)
        {
            GridFieldCaptionExportingEventHandler exportingEventHandler = this.Events[WebExcelExporter.EventGridFieldCaptionExporting] as GridFieldCaptionExportingEventHandler;
            if (exportingEventHandler == null)
                return;
            exportingEventHandler((object)this, e);
        }

        /// <summary>
        /// Raises the GridFieldCaptionExported Event.
        /// 
        /// </summary>
        protected virtual void OnGridFieldCaptionExported(GridFieldCaptionExportedEventArgs e)
        {
            GridFieldCaptionExportedEventHandler exportedEventHandler = this.Events[WebExcelExporter.EventGridFieldCaptionExported] as GridFieldCaptionExportedEventHandler;
            if (exportedEventHandler == null)
                return;
            exportedEventHandler((object)this, e);
        }

        /// <summary>
        /// Raises the GridRecordItemExporting Event.
        /// 
        /// </summary>
        protected virtual void OnGridRecordItemExporting(GridRecordItemExportingEventArgs e)
        {
            GridRecordItemExportingEventHandler exportingEventHandler = this.Events[WebExcelExporter.EventGridRecordItemExporting] as GridRecordItemExportingEventHandler;
            if (exportingEventHandler == null)
                return;
            exportingEventHandler((object)this, e);
        }

        /// <summary>
        /// Raises the GridRecordItemExported Event.
        /// 
        /// </summary>
        protected virtual void OnGridRecordItemExported(GridRecordItemExportedEventArgs e)
        {
            GridRecordItemExportedEventHandler exportedEventHandler = this.Events[WebExcelExporter.EventGridRecordItemExported] as GridRecordItemExportedEventHandler;
            if (exportedEventHandler == null)
                return;
            exportedEventHandler((object)this, e);
        }

        /// <summary>
        /// Raises the SummaryCellExporting Event.
        /// 
        /// </summary>
        protected virtual void OnSummaryCellExporting(SummaryCellExportingEventArgs e)
        {
            SummaryCellExportingEventHandler exportingEventHandler = this.Events[WebExcelExporter.EventSummaryCellExporting] as SummaryCellExportingEventHandler;
            if (exportingEventHandler == null)
                return;
            exportingEventHandler((object)this, e);
        }

        /// <summary>
        /// Raises the SummaryCellExported Event.
        /// 
        /// </summary>
        protected virtual void OnSummaryCellExported(SummaryCellExportedEventArgs e)
        {
            SummaryCellExportedEventHandler exportedEventHandler = this.Events[WebExcelExporter.EventSummaryCellExported] as SummaryCellExportedEventHandler;
            if (exportedEventHandler == null)
                return;
            exportedEventHandler((object)this, e);
        }

        private void InitStyleSheet(RunBot runBot)
        {
            if (!this.EnableStylesExport || this._currentStyleSheet != null)
                return;
            this._currentStyleSheet = new StyleSheet();
            this._currentStyleSheet.LinkedClasses.AddStyles(this.LoadLinkedStyles(runBot));
            this._currentStyleSheet.PageClasses.AddStyles(this.LoadPageStyles());
        }

        private IWorksheetCellFormat ApplyCellFormatFromStyle(Workbook workbook, IWorksheetCellFormat cellFormat, CssStyle cssStyle, WebDataGrid grid)
        {
            if (Color.Empty != cssStyle.Background.Color && cssStyle.Background.Color.ToArgb() != Color.White.ToArgb())
            {
                cellFormat.FillPatternForegroundColor = cssStyle.Background.Color;
                cellFormat.FillPattern = FillPatternStyle.Solid;
            }
            IWorkbookFont newWorkbookFont = workbook.CreateNewWorkbookFont();
            newWorkbookFont.Color = grid.ForeColor.IsEmpty ? cssStyle.Color : grid.ForeColor;
            newWorkbookFont.Name = string.IsNullOrEmpty(grid.Font.Name) ? cssStyle.Font.FamilyName : grid.Font.Name;
            switch (cssStyle.Font.SizeEnum)
            {
                case Infragistics.Documents.Reports.FontSize.NotSet:
                    newWorkbookFont.Height = cssStyle.Font.SizeUnit.Type != UnitType.Point || cssStyle.Font.SizeUnit.Value < 1.0 ? (cssStyle.Font.SizeUnit.Type != UnitType.Pixel || cssStyle.Font.SizeUnit.Value < 1.0 ? 240 : (int)(cssStyle.Font.SizeUnit.Value * 20.0)) : (int)(cssStyle.Font.SizeUnit.Value * 20.0);
                    break;
                case Infragistics.Documents.Reports.FontSize.Large:
                case Infragistics.Documents.Reports.FontSize.Larger:
                    newWorkbookFont.Height = 280;
                    break;
                case Infragistics.Documents.Reports.FontSize.Small:
                case Infragistics.Documents.Reports.FontSize.Smaller:
                    newWorkbookFont.Height = 200;
                    break;
                case Infragistics.Documents.Reports.FontSize.X_Large:
                    newWorkbookFont.Height = 360;
                    break;
                case Infragistics.Documents.Reports.FontSize.X_Small:
                    newWorkbookFont.Height = 180;
                    break;
                case Infragistics.Documents.Reports.FontSize.XX_Large:
                    newWorkbookFont.Height = 480;
                    break;
                case Infragistics.Documents.Reports.FontSize.XX_Small:
                    newWorkbookFont.Height = 160;
                    break;
                default:
                    newWorkbookFont.Height = 240;
                    break;
            }
            if (cssStyle.Font.IsBold || cssStyle.Font.Style == Infragistics.Documents.Reports.FontStyle.Oblique || grid.Font.Bold)
                newWorkbookFont.Bold = ExcelDefaultableBoolean.True;
            if (cssStyle.Font.IsItalic || cssStyle.Font.Style == Infragistics.Documents.Reports.FontStyle.Italic || grid.Font.Italic)
                newWorkbookFont.Italic = ExcelDefaultableBoolean.True;
            if (cssStyle.TextDecoration == TextDecoration.Line_Through || grid.Font.Strikeout)
                newWorkbookFont.Strikeout = ExcelDefaultableBoolean.True;
            if (cssStyle.TextDecoration == TextDecoration.Underline || grid.Font.Underline)
                newWorkbookFont.UnderlineStyle = FontUnderlineStyle.Single;
            cellFormat.Font.SetFontFormatting(newWorkbookFont);
            switch (cssStyle.TextAlign)
            {
                case Infragistics.Documents.Reports.TextAlign.Center:
                    cellFormat.Alignment = HorizontalCellAlignment.Center;
                    break;
                case Infragistics.Documents.Reports.TextAlign.Justify:
                    cellFormat.Alignment = HorizontalCellAlignment.Justify;
                    break;
                case Infragistics.Documents.Reports.TextAlign.Left:
                    cellFormat.Alignment = HorizontalCellAlignment.Left;
                    break;
                case Infragistics.Documents.Reports.TextAlign.Right:
                    cellFormat.Alignment = HorizontalCellAlignment.Right;
                    break;
                default:
                    cellFormat.Alignment = HorizontalCellAlignment.General;
                    break;
            }
            switch (cssStyle.VerticalAlign)
            {
                case Infragistics.Documents.Reports.VerticalAlign.Bottom:
                    cellFormat.VerticalAlignment = VerticalCellAlignment.Bottom;
                    break;
                case Infragistics.Documents.Reports.VerticalAlign.Middle:
                    cellFormat.VerticalAlignment = VerticalCellAlignment.Center;
                    break;
                case Infragistics.Documents.Reports.VerticalAlign.Top:
                    cellFormat.VerticalAlignment = VerticalCellAlignment.Top;
                    break;
                default:
                    cellFormat.VerticalAlignment = VerticalCellAlignment.Default;
                    break;
            }
            return cellFormat;
        }

        private void ApplyCellBorderFromStyle(IWorksheetCellFormat cellFormat, CssStyle cssStyle)
        {
            if (this.IsValidBorder(cssStyle.Border.Left) && cellFormat.LeftBorderColor.IsEmpty)
                cellFormat.LeftBorderColor = cssStyle.Border.Left.Color;
            if (this.IsValidBorder(cssStyle.Border.Right) && cellFormat.RightBorderColor.IsEmpty)
                cellFormat.RightBorderColor = cssStyle.Border.Right.Color;
            if (this.IsValidBorder(cssStyle.Border.Top) && cellFormat.TopBorderColor.IsEmpty)
                cellFormat.TopBorderColor = cssStyle.Border.Top.Color;
            if (this.IsValidBorder(cssStyle.Border.Bottom) && cellFormat.BottomBorderColor.IsEmpty)
                cellFormat.BottomBorderColor = cssStyle.Border.Bottom.Color;
            cellFormat.LeftBorderStyle = this.ConvertWebToExcelBorderStyle(cellFormat.LeftBorderStyle, cssStyle.Border.Left.Style, cssStyle.Border.Left.Width);
            cellFormat.TopBorderStyle = this.ConvertWebToExcelBorderStyle(cellFormat.TopBorderStyle, cssStyle.Border.Top.Style, cssStyle.Border.Top.Width);
            cellFormat.RightBorderStyle = this.ConvertWebToExcelBorderStyle(cellFormat.RightBorderStyle, cssStyle.Border.Right.Style, cssStyle.Border.Right.Width);
            cellFormat.BottomBorderStyle = this.ConvertWebToExcelBorderStyle(cellFormat.BottomBorderStyle, cssStyle.Border.Bottom.Style, cssStyle.Border.Bottom.Width);
        }

        private bool IsValidBorder(CssBorderDetail cssBorderDetail)
        {
            if (cssBorderDetail.HasBorder)
                return cssBorderDetail.Color.ToArgb() != Color.White.ToArgb();
            else
                return false;
        }

        /// <summary>
        /// Converts a Web BorderStyle into an Excel BorderStyle.
        /// 
        /// </summary>
        private CellBorderLineStyle ConvertWebToExcelBorderStyle(CellBorderLineStyle currentStyle, Infragistics.Documents.Reports.BorderStyle webBorderStyle, Unit width)
        {
            if (currentStyle != CellBorderLineStyle.Default)
                return currentStyle;
            double num = width.Value;
            if (num == 0.0)
                return CellBorderLineStyle.Default;
            CellBorderLineStyle cellBorderLineStyle;
            switch (webBorderStyle)
            {
                case Infragistics.Documents.Reports.BorderStyle.Double:
                case Infragistics.Documents.Reports.BorderStyle.Groove:
                    cellBorderLineStyle = CellBorderLineStyle.Double;
                    break;
                case Infragistics.Documents.Reports.BorderStyle.Inset:
                    cellBorderLineStyle = CellBorderLineStyle.Hair;
                    break;
                case Infragistics.Documents.Reports.BorderStyle.None:
                    cellBorderLineStyle = CellBorderLineStyle.Default;
                    break;
                case Infragistics.Documents.Reports.BorderStyle.Outset:
                case Infragistics.Documents.Reports.BorderStyle.Ridge:
                case Infragistics.Documents.Reports.BorderStyle.Solid:
                    cellBorderLineStyle = num < 2.0 ? CellBorderLineStyle.Thin : (num < 5.0 ? CellBorderLineStyle.Medium : CellBorderLineStyle.Thick);
                    break;
                case Infragistics.Documents.Reports.BorderStyle.Dotted:
                    cellBorderLineStyle = CellBorderLineStyle.Dotted;
                    break;
                case Infragistics.Documents.Reports.BorderStyle.Dashed:
                    cellBorderLineStyle = num < 3.0 ? CellBorderLineStyle.Dashed : CellBorderLineStyle.MediumDashed;
                    break;
                default:
                    cellBorderLineStyle = CellBorderLineStyle.Thin;
                    break;
            }
            return cellBorderLineStyle;
        }

        /// <summary>
        /// Sets the width of the columns in the exported document.
        /// 
        /// </summary>
        /// 
        /// <remarks>
        /// If the contents of the current cell are wider than the
        ///             contents of previous cells in the same column, then the column's width
        ///             is expanded to accomodate the widest cell.
        /// 
        /// </remarks>
        private void SetColumnWidth(WorksheetColumn wsCol, WorksheetCell cell)
        {
            string str = cell.Value != null ? cell.Value.ToString() : string.Empty;
            if (str == string.Empty)
                return;
            double a = !str.Contains(Environment.NewLine) ? this.GetWidthOfText(str) : this.GetWidestLineLength(str);
            if (cell.CellFormat.Font.Height > 0 && cell.CellFormat.Font.Height != 240)
                a *= (double)cell.CellFormat.Font.Height / 240.0;
            if (cell.CellFormat.Font.Bold == ExcelDefaultableBoolean.True)
                a *= 1.1;
            double num = (double)(int)Math.Ceiling(a);
            if (num <= (double)wsCol.Width)
                return;
            wsCol.Width = (int)num;
        }

        /// <summary>
        /// Gets the width in characters of a multi-line cell's value.
        /// 
        /// </summary>
        /// 
        /// <returns>
        /// The number of characters in the longest line of text
        ///             within a multi-line cell.
        /// </returns>
        protected double GetWidestLineLength(string cellText)
        {
            string[] strArray = cellText.Replace(Environment.NewLine, "\n").Split(new char[1]
      {
        '\n'
      });
            int length = strArray.Length;
            double val2 = 0.0;
            while (length-- > 0)
                val2 = Math.Max(this.GetWidthOfText(strArray[length]), val2);
            return val2;
        }

        private double GetWidthOfText(string cell)
        {
            int length = cell.Length;
            double num1 = (double)length;
            while (length-- > 0)
            {
                int num2 = (int)cell[length];
                if (num2 > 2000 || num2 == 87)
                    num1 += 0.8;
                else if (num2 == 77 || num2 == 64)
                    num1 += 0.6;
                else if (char.IsUpper((char)num2) || num2 >= 35 && num2 <= 38)
                    num1 += 0.35;
            }
            return num1 * 384.0;
        }
    }
}
