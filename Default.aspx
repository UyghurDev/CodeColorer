<%@ Page Title=" كود رەڭلەش(Code Colorer)" Language="C#" MasterPageFile="~/Common/Public.master" AutoEventWireup="true" CodeFile="Default.aspx.cs" Inherits="CodeColorer_Default" ValidateRequest="false" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
<script type="text/javascript" src="tabber.js"></script>
<link rel="stylesheet" href="example.css" TYPE="text/css" MEDIA="screen">
<script type="text/javascript">

/* Optional: Temporarily hide the "tabber" class so it does not "flash"
   on the page as plain HTML. After tabber runs, the class is changed
   to "tabberlive" and it will appear. */

document.write('<style type="text/css">.tabber{display:none;}<\/style>');
</script>
    <table cellpadding="0" cellspacing="0" style="width: 100%">
        <tr>
            <td style="text-align: center">
                <br />
                كود رەڭلەش<br />
                Code Colorer<br />
                <hr class="HorzentalLineHeader" dir="rtl" />
            </td>
        </tr>
        <tr>
        <td >
         <asp:Label ID="lblProgLang" runat="server" Text="پروگرامما تىلى:"></asp:Label>
         <asp:DropDownList ID="ddlProgLang" runat="server">
                </asp:DropDownList>
            &nbsp;&nbsp;
            <asp:Label ID="lblType" runat="server" Text="رەڭلەش شەكلى:"></asp:Label>
            <asp:DropDownList ID="ddlType" runat="server">
                <asp:ListItem>HTML</asp:ListItem>
                <asp:ListItem Selected="True">ئاددى HTML</asp:ListItem>
                <asp:ListItem>RTF</asp:ListItem>
                <asp:ListItem>BB Code</asp:ListItem>
            </asp:DropDownList>
&nbsp;<asp:Button ID="btnColor" runat="server" Text="رەڭلەش" 
                onclick="btnColor_Click" />
        </td>
        </tr>
        <tr>
            <td dir="ltr">
           
                <div class="tabber">

     <div class="tabbertab">
	  <h2>ئەسلى كود(Code Text)</h2>
	  <p>
          <asp:TextBox ID="txtCode" runat="server" Height="300px" TextMode="MultiLine" 
              Width="100%"></asp:TextBox>
                                            </p>
     </div>


     <div class="tabbertab">
	  <h2>كۆرۈنۈشى(View)</h2>
	  <p>
          <asp:Literal ID="ltrlView" runat="server"></asp:Literal>
                                            </p>
     </div>


     <div class="tabbertab">
	  <h2>رەڭلەنگەن تېكست(Colored Text)</h2>
	  <p>
          <asp:TextBox ID="txtColoredCode" runat="server" Height="300px" TextMode="MultiLine" 
              Width="100%"></asp:TextBox>
                                            </p>
     </div>

</div>
</td>
</tr>
<tr>
<td>
                <br />
                مۇناسىۋەتلىك ئۇلىنىشلار<hr align="right" class="HorzentalLineLinks" />
               <a href="http://www.codeplex.com/codecolorer">http://www.codeplex.com/codecolorer</a><br />
            </td>
        </tr>
    </table>
</asp:Content>

