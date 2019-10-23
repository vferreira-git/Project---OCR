<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="WebApplication6.Default" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link rel="stylesheet" href="https://stackpath.bootstrapcdn.com/bootstrap/4.3.1/css/bootstrap.min.css" integrity="sha384-ggOyR0iXCbMQv3Xipma34MD+dH/1fQ784/j6cY/iJTQUOhcWr7x9JvoRxT2MZw1T" crossorigin="anonymous"/>
</head>
<body style="margin:25px;">
    <form id="form1" runat="server">
        <div>
            <asp:CheckBox AutoPostBack="true" OnCheckedChanged="checkBoxOCR_CheckedChanged" ID="checkBoxOCR" Text="Use OCR" Checked="false" runat="server" />
                <br />
            <asp:FileUpload CssClass="form-control" ID="fileUpload" runat="server"/>
                <br />
            <asp:Panel ID="fileUploadHolder" Visible="false" runat="server">
                <asp:FileUpload ID="page1Upload" CssClass="form-control" accept="image/*" AllowMultiple="false" runat="server" />
                <br />
                <asp:FileUpload ID="page2Upload" CssClass="form-control" accept="image/*" AllowMultiple="false" runat="server" />
                <br />
                <asp:FileUpload ID="page3Upload" CssClass="form-control" accept="image/*" AllowMultiple="false" runat="server" />
                <br />
                <asp:FileUpload ID="page4Upload" CssClass="form-control" accept="image/*" AllowMultiple="false" runat="server" />
                <br />
                <asp:FileUpload ID="page5Upload" CssClass="form-control" accept="image/*" AllowMultiple="false" runat="server" />
                <br />
            </asp:Panel>
            <asp:Button OnClick="Unnamed_Click" CssClass="btn btn-success" Text="Start" runat="server" />
        </div>
    </form>
</body>
</html>
