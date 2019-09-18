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
        int LeftTime;
        int TIME_IN_CHAMBER;
        DateTime OpenTime;

        string clear = "CLEAR";

        public Main()
        {
            InitializeComponent();
            txtReelID.Focus();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            LMS.LoadComboBoxSystem(cbSystem);
            txtReelID.Focus();
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
                string strOpenTime = "select a.OPEN_TIME from LMS_MGR.TB_DRYCOMPONENT a " +
                    "inner join LMS_MGR.TB_COMPLIST b on a.PART_NO = b.PART_NO " +
                    "where REEL_ID = '" + txtReelID.Text.ToString() + "'";
                OracleDataAdapter daQuerry = new OracleDataAdapter();
                OracleDataAdapter daOpen = new OracleDataAdapter();

                if (cbSystem.SelectedValue == "4P")
                {
                    daQuerry = new OracleDataAdapter(strQuerry, con);
                    daOpen = new OracleDataAdapter(strOpenTime, con);
                }
                else if (cbSystem.SelectedValue == "HSE")
                {
                    daQuerry = new OracleDataAdapter(strQuerry, con2);
                    daOpen = new OracleDataAdapter(strOpenTime, con2);
                }
                else if (cbSystem.SelectedValue == "DDE")
                {
                    daQuerry = new OracleDataAdapter(strQuerry, con3);
                    daOpen = new OracleDataAdapter(strOpenTime, con3);
                }
                DataTable dtQuerry = new DataTable();
                daQuerry.Fill(dtQuerry);

                DataTable dtOpen = new DataTable();
                daOpen.Fill(dtOpen);

                lblReelID.Text = txtReelID.Text.ToString();
                lblUsed.Text = dtQuerry.Rows[0]["USED"].ToString();
                lblPartNo.Text = dtQuerry.Rows[0]["PART_NO"].ToString();
                lblInputDate.Text = dtQuerry.Rows[0]["OPENTIMESTAMP"].ToString();
                lblLeftTime.Text = dtQuerry.Rows[0]["LEFT_TIME"].ToString();
                lblAmount.Text = dtQuerry.Rows[0]["AMOUNT"].ToString();

                DateTime Current = DateTime.Now;
                if (!(dtQuerry.Rows[0]["OPENTIMESTAMP"] is DBNull))
                {
                    OpenTime = (DateTime)dtQuerry.Rows[0]["OPENTIMESTAMP"];
                }
                TimeSpan timeSpan = Current.Subtract(OpenTime);

                int OPENTIME = Convert.ToInt32(dtOpen.Rows[0]["OPEN_TIME"]);

                //If the system has no data of Left Time for Component

                if (!(dtQuerry.Rows[0]["TIME_IN_CHAMBER"] is DBNull))
                {
                    TIME_IN_CHAMBER = Convert.ToInt32(dtQuerry.Rows[0]["TIME_IN_CHAMBER"]);
                    LeftTime = OPENTIME - (int)timeSpan.TotalMinutes + TIME_IN_CHAMBER; //algorithm to calculate the Left Time
                    lblLeftCurrent.Text = LeftTime.ToString();
                }

                if (dtQuerry.Rows[0]["LEFT_TIME"] is DBNull)
                {
                    lblLeftTime.Text = LeftTime.ToString();
                }


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


                txtReelID.Focus();
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

        private void txtReelID_KeyDown(object sender, KeyEventArgs e)
        {

            string ID = txtReelID.Text.ToString();
            if (e.KeyCode == Keys.Enter)
            {
                if (ID.Substring(ID.Length - clear.Length) == clear)
                {
                    btnClear_Click(this, new EventArgs());
                }
                else
                {
                    btnQuerry_Click(this, new EventArgs());
                }
            }

        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            txtReelID.ResetText();
            txtReelID.Focus();
            lblAmount.ResetText();
            lblExpired.ResetText();
            lblInputDate.ResetText();
            lblLeftTime.ResetText();
            lblPartNo.ResetText();
            lblReelID.ResetText();
            lblUsed.ResetText();
            lblLeftCurrent.ResetText();
        }

        private void txtReelID_TextChanged(object sender, EventArgs e)
        {
            /*/
            string clear = txtReelID.Text.ToString();
            if (clear.Substring(clear.Length-5, clear.Length - 1) == "CLEAR")
            {
                btnClear_Click(this, new EventArgs());
            }
            /*/
        }

        private void cbSystem_SelectedIndexChanged(object sender, EventArgs e)
        {
            txtReelID.Focus();
        }
    }
}
