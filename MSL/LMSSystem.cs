using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data;

namespace MSL
{
    class LMSSystem
    {
        public DataTable LoadSystemTable()
        {
            DataTable dtSystem = new DataTable();
            dtSystem.Columns.Add("System_Code", typeof(string));
            dtSystem.Columns.Add("System_Name", typeof(string));

            dtSystem.Rows.Add("4P", "LMS 4P");
            dtSystem.Rows.Add("HSE", "LMS Haengsung");
            dtSystem.Rows.Add("DDE", "LMS Dongdo");

            return dtSystem;
        }
        public void LoadComboBoxSystem(ComboBox cbSystem)
        {
            DataTable dtSystem = LoadSystemTable();
            cbSystem.DataSource = dtSystem;
            cbSystem.ValueMember = "System_Code";
            cbSystem.DisplayMember = "System_Name";

        }
    }
}
