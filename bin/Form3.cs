using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using CrystalDecisions.CrystalReports.Engine;
using System.Data.SqlClient;
namespace ElectricityBills
{
    public partial class Form3 : Form
    {
        public Form3()
        {
            InitializeComponent();
        }
       
        private void splitContainer1_Panel2_Paint(object sender, PaintEventArgs e)
        {

        }

        private void Form3_Load(object sender, EventArgs e)
        {
            string con = Properties.Settings.Default.electricity;
            string sql = "select d.staffID,d.name,b.quator_no,d.department,b.marg,c.previous_reading,c.current_reading,a.maintenance,c.bill_amount from admin_details a,bitsmu_quarter b,electricity_bills_dup c,staff d where  c.month=1 and c.year = 2015 and c.staff_key = d.staffkey and c.quator_key = b.quator_key ";
            SqlDataAdapter sd = new SqlDataAdapter(sql, con);
            DataSet ds = new DataSet();
            sd.Fill(ds);
            ReportDocument cryst = new ReportDocument();
            cryst.Load(@"C:\Users\PRATEEK\Desktop\projects\ElectricityBills\ElectricityBills\Bills.rpt");
            cryst.SetDataSource(ds);
            crystalReportViewer1.ReportSource = cryst;
            crystalReportViewer1.Refresh();
        }

       
    }
}
