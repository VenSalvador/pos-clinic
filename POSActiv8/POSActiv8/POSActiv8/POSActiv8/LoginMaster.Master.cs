using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace POSActiv8
{
    public partial class LoginMaster : System.Web.UI.MasterPage
    {
        public void Alert(String strMess)
        {
            String strScript = "alert('" + strMess + "');";
            ScriptManager.RegisterStartupScript(this, this.GetType(), "MyScript", strScript, true);
        }

        //Page Load

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["Mess"] == null)
            { }

            else
            {
                if (Session["Mess"].ToString().Trim().Length > 0)
                {
                    Alert(Session["Mess"].ToString());
                    Session["Mess"] = null;
                }
            }
        }
    }
}