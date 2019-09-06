using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Oracle.ManagedDataAccess.Client;
using System.Configuration;


namespace MSL
{
    public partial class Main : Form
    {

        static string connectionString1 = ConfigurationManager.ConnectionStrings["LMS4P"].ConnectionString;
        static string connectionString2 = ConfigurationManager.ConnectionStrings["LMSHS"].ConnectionString;
        static string connectionString3 = ConfigurationManager.ConnectionStrings["LMSDD"].ConnectionString;

        OracleConnection con = new OracleConnection(connectionString1);
        OracleConnection con2 = new OracleConnection(connectionString2);
        OracleConnection con3 = new OracleConnection(connectionString3);

        LMSSystem LMS = new LMSSystem();

        public Main()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            LMS.LoadComboBoxSystem(cbSystem);
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void btnQuerry_Click(object sender, EventArgs e)
        {
            try
            {
                string strQuerry = "select * from LMS_MGR.TB_COMPLIST where REEL_ID = '" + txtReelID.Text.ToString() + "'";
                OracleDataAdapter daQuerry = new OracleDataAdapter();

                if (cbSystem.SelectedValue == "4P")
                {
                    daQuerry =  new OracleDataAdapter(strQuerry, con);
                }
                else if(cbSystem.SelectedValue == "HSE")
                {
                    daQuerry = new OracleDataAdapter(strQuerry, con2);
                }
                else if(cbSystem.SelectedValue == "DDE")
                {
                    daQuerry = new OracleDataAdapter(strQuerry, con3);
                }
                DataTable dtQuerry = new DataTable();
                daQuerry.Fill(dtQuerry);

                lblReelID.Text = txtReelID.Text.ToString();
                lblUsed.Text = dtQuerry.Rows[0]["USED"].ToString();
                lblPartNo.Text = dtQuerry.Rows[0]["PART_NO"].ToString();
                lblInputDate.Text = dtQuerry.Rows[0]["OPENTIMESTAMP"].ToString();
                lblLeftTime.Text = dtQuerry.Rows[0]["LEFT_TIME"].ToString();
                lblAmount.Text = dtQuerry.Rows[0]["AMOUNT"].ToString();

                if (lblLeftTime.Text.ToString() == "0")
                {
                    lblExpired.Text = "YES";
                    lblExpired.ForeColor = Color.Red;

                }
                else if (lblLeftTime.Text.ToString() != "0")
                {
                    lblExpired.Text = "NO";
                    lblExpired.ForeColor = Color.Blue;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                con.Close();
            }
        }
    }
}
