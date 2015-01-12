<%@ Control Language="C#" CodeBehind="DateTime_Edit.ascx.cs" Inherits="Metrona.Wt.Web.DateTime_EditField" %>

<asp:TextBox ID="TextBox1" runat="server" CssClass="form-control" Text='<%# FieldValueEditString %>' Columns="20"></asp:TextBox>

<asp:RequiredFieldValidator runat="server" ID="RequiredFieldValidator1" CssClass="DDControl DDValidator text-danger" ControlToValidate="TextBox1" Display="Dynamic" Enabled="false" />
<asp:RegularExpressionValidator runat="server" ID="RegularExpressionValidator1" CssClass="DDControl DDValidator text-danger" ControlToValidate="TextBox1" Display="Dynamic" Enabled="false" />
<asp:DynamicValidator runat="server" ID="DynamicValidator1" CssClass="DDControl DDValidator text-danger" ControlToValidate="TextBox1" Display="Dynamic" />
<asp:CustomValidator runat="server" ID="DateValidator" CssClass="DDControl DDValidator text-danger" ControlToValidate="TextBox1" Display="Dynamic" EnableClientScript="false" Enabled="false" OnServerValidate="DateValidator_ServerValidate" />

<asp:CompareValidator ID="DateFormatValidator" runat="server" ControlToValidate="TextBox1"
                                              CssClass="text-danger" Display="Dynamic" ErrorMessage="Geben Sie bitte ein Datum ein." Operator="DataTypeCheck"
                                              SetFocusOnError="True" ToolTip="Geben Sie bitte ein Datum ein." Type="Date">
</asp:CompareValidator>

