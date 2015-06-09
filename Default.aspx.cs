using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;



public partial class CodeColorer_Default : System.Web.UI.Page
{
    string strSchemePath;
    protected void Page_Load(object sender, EventArgs e)
    {

            strSchemePath = Server.MapPath("") + "\\Schemes\\";
            if (!Page.IsPostBack)
            {
                DirectoryInfo di = new DirectoryInfo(strSchemePath);
                FileInfo[] fis = di.GetFiles();
                ddlProgLang.Items.Clear();
                foreach (FileInfo fi in fis)
                {
                    net.UyghurDev.Text.CodeColorer tempCC = new net.UyghurDev.Text.CodeColorer(fi.FullName);
                    ListItem li = new ListItem(tempCC.Name, fi.FullName.Substring(fi.FullName.LastIndexOf('\\')));
                    ddlProgLang.Items.Add(li);
                }
            }
    }
    protected void btnColor_Click(object sender, EventArgs e)
    {
        net.UyghurDev.Text.CodeColorer colorer = new net.UyghurDev.Text.CodeColorer(strSchemePath + ddlProgLang.SelectedValue);
        ltrlView.Text = colorer.ConvertHtmlSimple(txtCode.Text);

        switch (ddlType.SelectedIndex)
        { 
            case 0:
                txtColoredCode.Text = colorer.ConvertHtml(txtCode.Text);
                break;
            case 1:
                txtColoredCode.Text = colorer.ConvertHtmlSimple(txtCode.Text);
                break;
            case 2:
                txtColoredCode.Text = colorer.ConvertRtf(txtCode.Text);
                break;
            case 3:
                txtColoredCode.Text = colorer.ConvertBbCodes(txtCode.Text);
                break;
            default:
                txtColoredCode.Text = txtCode.Text;
                break;
        }
        
    }
}
