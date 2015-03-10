<%@ Control Language="C#" CodeBehind="DateTime_Edit.ascx.cs" Inherits="Metrona.Wt.Web.DateTime_EditField" %>

<asp:TextBox ID="TextBox1" runat="server" CssClass="form-control" Text='<%# FieldValueEditString %>' Columns="20"></asp:TextBox>

<asp:RequiredFieldValidator runat="server" ID="RequiredFieldValidator1" CssClass="DDControl DDValidator text-danger" ControlToValidate="TextBox1" Display="none" Enabled="false" />
<asp:RegularExpressionValidator runat="server" ID="RegularExpressionValidator1" CssClass="DDControl DDValidator text-danger" ControlToValidate="TextBox1" Display="Dynamic" Enabled="false" />
<asp:DynamicValidator runat="server" ID="DynamicValidator1" CssClass="DDControl DDValidator text-danger" ControlToValidate="TextBox1" Display="Dynamic" />
<asp:CustomValidator runat="server" ID="DateValidator" CssClass="DDControl DDValidator text-danger" ControlToValidate="TextBox1" Display="Dynamic" EnableClientScript="True" Enabled="false" OnServerValidate="DateValidator_ServerValidate" />

<asp:CompareValidator ID="DateFormatValidator1" runat="server" ControlToValidate="TextBox1"
                                              CssClass="text-danger" Display="None" ErrorMessage="Geben Sie bitte ein g�ltiges Datum im Format TT.MM.JJJJ ein." Operator="DataTypeCheck"
                                              SetFocusOnError="True" ToolTip="Geben Sie bitte ein g�ltiges Datum im Format TT.MM.JJJJ ein." Type="Date" ValidationGroup="vgKlima">
</asp:CompareValidator>

