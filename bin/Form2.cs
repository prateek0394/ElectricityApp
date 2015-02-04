using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Configuration;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.IO;
using CrystalDecisions.CrystalReports.Engine;
namespace ElectricityBills
{
    
    public partial class Form2 : Form
    {
        string quator_key;
        string staffkey;
        int month;
        int year;
        decimal unit,min_amount;
        public Form2()
        {
            InitializeComponent();
        }

        private void splitContainer1_Panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            string con = Properties.Settings.Default.electricity;
            SqlDataAdapter sd = new SqlDataAdapter("select * from staff where staffID='" + textBox1.Text + "'", con);
            DataSet ds = new DataSet();
            sd.Fill(ds);
            if (ds.Tables[0].Rows.Count != 1) {
                label27.Visible = true;
            }
            else
            {
                string name = ds.Tables[0].Rows[0][2].ToString();
                label27.Visible = false;
                staffkey = ds.Tables[0].Rows[0][0].ToString();
                 sd = new SqlDataAdapter("select * from allotment where staff_key='" + ds.Tables[0].Rows[0][0].ToString() + "'", con);
                ds = new DataSet();
                sd.Fill(ds);
                int j;
                               for (j = 0; j < ds.Tables[0].Rows.Count; j++) {
                   
                    if (ds.Tables[0].Rows[j][3].ToString() =="") {
                        //j = j + 1;
                        break;
                    }
                    else if (DateTime.Compare(Convert.ToDateTime(ds.Tables[0].Rows[j][3]), DateTime.Today) >= 0) {
                        break;
                    }
                }
                quator_key = ds.Tables[0].Rows[j][0].ToString();
                sd = new SqlDataAdapter("select * from bitsmu_quarter where quator_key='" + quator_key + "'", con);
                ds = new DataSet();
                sd.Fill(ds);
                string address = "Quarter number " + ds.Tables[0].Rows[0][1].ToString() + ", " + ds.Tables[0].Rows[0][4].ToString() + ", " + ds.Tables[0].Rows[0][6].ToString() + " floor";
                label4.Text = name;
                label5.Text = address;
                //code for previous reading
                sd = new SqlDataAdapter("select * from electricity_bills_dup where staff_key=" + staffkey + "", con);
                ds = new DataSet();
                sd.Fill(ds);
                label11.Text = ds.Tables[0].Rows[0][5].ToString();
                int last_year = Convert.ToInt32(ds.Tables[0].Rows[0][2].ToString());
                int last_month = Convert.ToInt32(ds.Tables[0].Rows[0][3].ToString());
                int month_diff = (year - last_year) * 12 + (month - last_month);
                sd = new SqlDataAdapter("select * from admin_details", con);
                ds = new DataSet();
                sd.Fill(ds);
                decimal penalty;
                unit =Convert.ToDecimal(ds.Tables[0].Rows[0][0].ToString());
                min_amount = Convert.ToDecimal(ds.Tables[0].Rows[0][2].ToString());
                penalty =Convert.ToDecimal(ds.Tables[0].Rows[0][3].ToString());
                label13.Text = ds.Tables[0].Rows[0][1].ToString();
                label28.Text = Convert.ToString(penalty*(month_diff-1));
                //label14 total;
                panel1.Visible = false;
                panel2.Visible = true;
            }
        }
        private void currentreading(object sender, EventArgs e) {
            int current = Convert.ToInt32(textBox2.Text);
            label12.Text = Convert.ToString(current - Convert.ToInt32(label11.Text));
            decimal total = (current - Convert.ToInt32(label11.Text)) * unit + Convert.ToDecimal(label13.Text) + Convert.ToDecimal(label28.Text);
            if (min_amount > total)
            {
                label14.Text = Convert.ToString(min_amount);
            }
            else {
                label14.Text = Convert.ToString(total);
            }
        }
        private void actxt_KeyPress(object sender, KeyPressEventArgs evtargs)
{
            evtargs.Handled = (!char.IsDigit(evtargs.KeyChar)) && (!char.IsControl(evtargs.KeyChar));
}
        private void label13_Click(object sender, EventArgs e)
        {

        }

        private void Form2_Load(object sender, EventArgs e)
        {
            string con = Properties.Settings.Default.electricity;//ConfigurationManager.ConnectionStrings["Electricity"].ConnectionString;
            SqlDataAdapter sd = new SqlDataAdapter("select * from admin_details", con);
            ///OleDbDataAdapter da = new OleDbDataAdapter("select * from admin_details", cn);
            DataSet ds = new DataSet();
            sd.Fill(ds);
            label16.Text = ds.Tables[0].Rows[0][0].ToString();
            label18.Text = ds.Tables[0].Rows[0][2].ToString();
            label20.Text = ds.Tables[0].Rows[0][1].ToString();
            label23.Text = ds.Tables[0].Rows[0][3].ToString();
            panel3.Visible = false;
            panel4.Visible = false;
            //panel5.Visible = true;           
            comboBox1.Items.Add("January");
            comboBox1.Items.Add("February");
            comboBox1.Items.Add("March");
            comboBox1.Items.Add("April");
            comboBox1.Items.Add("May");
            comboBox1.Items.Add("June");
            comboBox1.Items.Add("July");
            comboBox1.Items.Add("August");
            comboBox1.Items.Add("September");
            comboBox1.Items.Add("October");
            comboBox1.Items.Add("November");
            comboBox1.Items.Add("December");
            comboBox1.SelectedIndex = comboBox1.FindStringExact("January");

            int i = DateTime.Today.Year;
            for (int j = 0; j <= 20; j++) {
                comboBox2.Items.Add(Convert.ToString(i + j));
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            textBox3.Text = label16.Text;
            textBox4.Text = label18.Text;
            textBox5.Text = label20.Text;
            textBox6.Text = label23.Text;
            panel3.Visible = false;
            panel4.Visible = true; 
        }

        private void button4_Click(object sender, EventArgs e)
        {
            string con = Properties.Settings.Default.electricity;
            SqlConnection conn = new SqlConnection(con);
            conn.Open();
            SqlCommand comm = new SqlCommand("update admin_details set unit_rate='" + textBox3.Text + "',maintenance='" + textBox4.Text + "',min_amount='" + textBox5.Text + "',penalty='" + textBox6.Text + "'", conn);
            comm.ExecuteNonQuery();
            conn.Close();
            SqlDataAdapter sd = new SqlDataAdapter("select * from admin_details", con);
            ///OleDbDataAdapter da = new OleDbDataAdapter("select * from admin_details", cn);
            DataSet ds = new DataSet();
            sd.Fill(ds);
            label16.Text = ds.Tables[0].Rows[0][0].ToString();
            label18.Text = ds.Tables[0].Rows[0][2].ToString();
            label20.Text = ds.Tables[0].Rows[0][1].ToString();
            label23.Text = ds.Tables[0].Rows[0][3].ToString();
            panel3.Visible = true;
            panel4.Visible = false;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                decimal total = Convert.ToDecimal(label14.Text);
                int current_reading = Convert.ToInt32(textBox2.Text);
                int previous_reading = Convert.ToInt32(label11.Text);
                string con = Properties.Settings.Default.electricity;
                SqlConnection conn = new SqlConnection(con);
                conn.Open();
                SqlCommand comm = new SqlCommand("insert into electricity_bills values(" + staffkey + "," + quator_key + "," + year + "," + month + "," + previous_reading + "," + current_reading + "," + total + ");", conn);
                comm.ExecuteNonQuery();
                comm = new SqlCommand("update electricity_bills_dup set year=" + year + ",month=" + month + ",previous_reading=" + previous_reading + ",current_reading=" + current_reading + ",bill_amount =" + total + " where staff_key = " + staffkey + "and quator_key = " + quator_key + ";", conn);
                comm.ExecuteNonQuery();
                conn.Close();
                label30.Visible = true;
                panel1.Visible = true;
                panel2.Visible = false;
                label31.Visible = false;
            }
            catch {
                label31.Visible = true;

            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            year = Convert.ToInt32(comboBox2.SelectedItem.ToString());//textBox7.Text
            month = Convert.ToInt32(comboBox1.SelectedIndex+1);//textBox8.Text
            panel5.Visible = false;
            panel1.Visible = true;
        }

        private void button6_Click(object sender, EventArgs e)
        {
            panel5.Visible = true;
            panel1.Visible = false;
        }

        private void button7_Click(object sender, EventArgs e)
        {
            //Form3 x = new Form3();
            //x.ShowDialog();
            ////this.Close();
            string con = Properties.Settings.Default.electricity;
            string sql = "select d.staffID,d.name,b.quator_no,d.department,b.marg,c.previous_reading,c.current_reading,a.maintenance,c.bill_amount from admin_details a,bitsmu_quarter b,electricity_bills_dup c,staff d where  c.month="+month+" and c.year ="+year+" and c.staff_key = d.staffkey and c.quator_key = b.quator_key ";
            SqlDataAdapter sd = new SqlDataAdapter(sql, con);
            DataSet ds = new DataSet();
            sd.Fill(ds);
            Bills cyr = new Bills();
            cyr.SetDataSource(ds);
            cyr.Refresh();
            dataGridView1.DataSource = ds;
            ReportDocument rpt = new ReportDocument();
            rpt.Load(@"C:\Users\PRATEEK\Desktop\projects\ElectricityBills\ElectricityBills\Bills.rpt");
            rpt.SetDataSource(ds);
            rpt.SetDatabaseLogon("sa", "prateek");
            rpt.ExportToDisk(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat, Path.GetDirectoryName(Application.ExecutablePath) + "\\test.pdf");
            dataGridView1.AutoResizeColumns(
            DataGridViewAutoSizeColumnsMode.AllCellsExceptHeader);
                 cyr.SetDatabaseLogon("sa","prateek");
            cyr.ExportToDisk(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat,Path.GetDirectoryName(Application.ExecutablePath)+ "\\test.pdf");
        }

        private void showStandardValuesToolStripMenuItem_Click(object sender, EventArgs e)
        {
           
            panel3.Visible = true;
            panel4.Visible = false; 
        }

        private void editStandardValuesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            textBox3.Text = label16.Text;
            textBox4.Text = label18.Text;
            textBox5.Text = label20.Text;
            textBox6.Text = label23.Text;
            panel3.Visible = false;
            panel4.Visible = true; 
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void generateBillsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string month = DateTime.Today.Month.ToString();
            string year = DateTime.Today.Year.ToString();
            string con = Properties.Settings.Default.electricity;
            string sql = "select d.staffID,d.name,b.quator_no,d.department,b.marg, DateName( month , DateAdd( month , c.month , 0 ) - 1 ) as month,c.year,c.previous_reading,c.current_reading,a.maintenance,c.bill_amount,(c.bill_amount-(c.current_reading-c.previous_reading)*a.unit_rate-a.maintenance) as penalty from admin_details a,bitsmu_quarter b,electricity_bills_dup c,staff d where  c.month=" + month + " and c.year = " + year + " and c.staff_key = d.staffkey and c.quator_key = b.quator_key and LEN(d.staffID)!=4 ";
            SqlDataAdapter sd = new SqlDataAdapter(sql, con);
            DataSet ds = new DataSet();
            sd.Fill(ds);
            DataTable x = new DataTable("DataTable1");
            x = ds.Tables[0];
            x.TableName = "DataTable1";
            Bills cyr = new Bills();
            cyr.Database.Tables["DataTable1"].SetDataSource(x);
            //cyr.Refresh();
            //cyr.SetDatabaseLogon("sa", "prateek");
            cyr.ExportToDisk(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat, Path.GetDirectoryName(Application.ExecutablePath) + "\\" + "Bills" + month + year + ".pdf");
            //ReportDocument rpt = new ReportDocument();
            //rpt.Load(@"C:\Users\PRATEEK\Desktop\projects\ElectricityBills\ElectricityBills\Bills.rpt");
            //rpt.SetDataSource(ds);
            //rpt.SetDatabaseLogon("sa", "prateek");
            //rpt.ExportToDisk(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat, Path.GetDirectoryName(Application.ExecutablePath) + "\\test.pdf");
            var t = new Timer();
            t.Interval = 3000; // it will Tick in 3 seconds
            t.Tick += (s, ev) =>
            {
                label2.Hide();
                t.Stop();
            };
            label2.Visible = true;
            t.Start();
        }

        private void generateReportToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string month = DateTime.Today.Month.ToString();
            string year = DateTime.Today.Year.ToString();
            string con = Properties.Settings.Default.electricity;
            string sql = "select d.staffID,d.name,b.quator_no,d.department,b.marg, DateName( month , DateAdd( month , c.month , 0 ) - 1 ) as month,c.year,c.previous_reading,c.current_reading,a.maintenance,c.bill_amount,(c.bill_amount-(c.current_reading-c.previous_reading)*a.unit_rate-a.maintenance) as penalty from admin_details a,bitsmu_quarter b,electricity_bills_dup c,staff d where  c.month=" + month + " and c.year = " + year + " and c.staff_key = d.staffkey and c.quator_key = b.quator_key and LEN(d.staffID)=4 ";
            SqlDataAdapter sd = new SqlDataAdapter(sql, con);
            DataSet ds = new DataSet();
            sd.Fill(ds);
            DataTable x = new DataTable("DataTable1");
            x = ds.Tables[0];
            x.TableName = "DataTable1";         
            Report cyr = new Report();
            cyr.Database.Tables["DataTable1"].SetDataSource(x);
            cyr.ExportToDisk(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat, Path.GetDirectoryName(Application.ExecutablePath) + "\\" + "Report"+ month + year +".pdf");
            var t = new Timer();
            t.Interval = 3000; // it will Tick in 3 seconds
            t.Tick += (s, ev) =>
            {
                label2.Hide();
                t.Stop();
            };
            label2.Visible = true;
            t.Start();
        }
    }
}
