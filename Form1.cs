using Microsoft.VisualBasic.ApplicationServices;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace HarryPlotterV1
{
    public partial class Form1 : Form
    {
        private Button currentButton;
        private Random random;
        private int tempIndex;
        private Form activeForm;
        public string sPath = System.AppDomain.CurrentDomain.BaseDirectory;
        public Form1()
        {
            InitializeComponent();
            random = new Random();
            //btnCloseChildForm.Visible = false;
            this.Text = string.Empty;
            this.ControlBox = false;
            this.MaximizedBounds = Screen.FromHandle(this.Handle).WorkingArea;
        }
        [DllImport("user32.DLL", EntryPoint = "ReleaseCapture")]
        private extern static void ReleaseCapture();
        [DllImport("user32.DLL", EntryPoint = "SendMessage")]
        private extern static void SendMessage(System.IntPtr hWnd, int wMsg, int wParam, int lParam);
        //Methods
        private Color Selectcolorclass()
        {
            int index = random.Next(colorclass.ColorList.Count);
            while (tempIndex == index)
            {
                index = random.Next(colorclass.ColorList.Count);
            }
            tempIndex = index;
            string color = colorclass.ColorList[index];
            return ColorTranslator.FromHtml(color);
        }
        private void ActivateButton(object btnSender)
        {
            if (btnSender != null)
            {
                if (currentButton != (Button)btnSender)
                {
                    DisableButton();
                    Color color = Selectcolorclass();
                    currentButton = (Button)btnSender;
                    currentButton.BackColor = color;
                    currentButton.ForeColor = Color.White;
                    currentButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 12.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                    paneltitle.BackColor = color;
                    panellogo.BackColor = colorclass.ChangeColorBrightness(color, -0.3);
                    colorclass.PrimaryColor = color;
                    colorclass.SecondaryColor = colorclass.ChangeColorBrightness(color, -0.3);
                    //btnCloseChildForm.Visible = true;
                }
            }
        }
        private void DisableButton()
        {
            foreach (Control previousBtn in panelmenu.Controls)
            {
                if (previousBtn.GetType() == typeof(Button))
                {
                    previousBtn.BackColor = Color.FromArgb(51, 51, 76);
                    previousBtn.ForeColor = Color.Gainsboro;
                    previousBtn.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                }
            }
        }
        private void btnCloseChildForm_Click(object sender, EventArgs e)
        {
            if (activeForm != null)
                activeForm.Close();
            Reset();
        }
        private void Reset()
        {
            DisableButton();
            lblTitle.Text = "HOME";
            paneltitle.BackColor = Color.FromArgb(0, 150, 136);
            panellogo.BackColor = Color.FromArgb(39, 39, 58);
            currentButton = null;
            //btnCloseChildForm.Visible = false;
        }
        private void paneltitle_MouseDown(object sender, MouseEventArgs e)
        {
            ReleaseCapture();
            SendMessage(this.Handle, 0x112, 0xf012, 0);
        }
        private void btnClose_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
        private void btnMaximize_Click(object sender, EventArgs e)
        {
            if (WindowState == FormWindowState.Normal)
                this.WindowState = FormWindowState.Maximized;
            else
                this.WindowState = FormWindowState.Normal;
        }
        private void bntMinimize_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        private void buttonview_Click(object sender, EventArgs e)
        {
            ActivateButton(sender);
            //view tabpage 1 in tabcontrol
            tabControl1.SelectTab(0);
        }

        private void PlotData_Click(object sender, EventArgs e)
        {
            ActivateButton(sender);
            tabControl1.SelectTab(2);
        }

        private void dayinter_Click(object sender, EventArgs e)
        {
            ActivateButton(sender);
            tabControl1.SelectTab(3);
        }

        private void Interpolator_Click(object sender, EventArgs e)
        {
            ActivateButton(sender);
            tabControl1.SelectTab(4);
        }

        private void SelectData_Click(object sender, EventArgs e)
        {
            ActivateButton(sender);
            tabControl1.SelectTab(1);
        }

        private void moretools_Click(object sender, EventArgs e)
        {
            ActivateButton(sender);
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void dataGridView2_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void Form1_Load_1(object sender, EventArgs e)
        {
            tabControl1.Appearance = TabAppearance.FlatButtons;
            tabControl1.ItemSize = new Size(0, 1);
            tabControl1.SizeMode = TabSizeMode.Fixed;

            foreach (TabPage tab in tabControl1.TabPages)
            {
                tab.Text = "";
            }
            System.Diagnostics.Process process = new System.Diagnostics.Process();
            process.StartInfo.FileName = "cmd.exe";
            process.StartInfo.Arguments = @"/C where python";
            process.StartInfo.UseShellExecute = false;
            process.StartInfo.CreateNoWindow = true;
            process.StartInfo.RedirectStandardOutput = true;
            process.StartInfo.RedirectStandardInput = true;
            process.Start();
            string q = "";
            while (!process.HasExited)
            {
                q += process.StandardOutput.ReadToEnd();
            }
            textBox3.Text = q;
            //create a folder and add pythonek folder from solution explorer
       
        }

        private void BindData1(string filePath)
        {
            DataTable dt = new DataTable();
            string[] lines = System.IO.File.ReadAllLines(filePath);
            if (lines.Length > 0)
            {
                //first line to create header
                string firstLine = lines[0];
                string[] headerLabels = firstLine.Split(',');

                foreach (string headerWord in headerLabels)
                {
                    listBox1.Items.Add(headerWord);
                }
                foreach (string headerWord in headerLabels)
                {
                    dt.Columns.Add(new DataColumn(headerWord));
                }
                //For Data
                for (int i = 1; i < lines.Length; i++)
                {
                    string[] dataWords = lines[i].Split(',');
                    DataRow dr = dt.NewRow();
                    int columnIndex = 0;
                    foreach (string headerWord in headerLabels)
                    {
                        dr[headerWord] = dataWords[columnIndex++];
                    }
                    dt.Rows.Add(dr);
                }
            }
            if (dt.Rows.Count > 0)
            {
                dataGridView1.DataSource = dt;
            }
        }
        private void BindData2(string filePath)
        {
            DataTable dt = new DataTable();
            string[] lines = System.IO.File.ReadAllLines(filePath);
            if (lines.Length > 0)
            {
                //first line to create header
                string firstLine = lines[0];
                string[] headerLabels = firstLine.Split(',');
                foreach (string headerWord in headerLabels)
                {
                    listBox2.Items.Add(headerWord);
                }
                foreach (string headerWord in headerLabels)
                {
                    dt.Columns.Add(new DataColumn(headerWord));
                }
                //For Data
                for (int i = 1; i < lines.Length; i++)
                {
                    string[] dataWords = lines[i].Split(',');
                    DataRow dr = dt.NewRow();
                    int columnIndex = 0;
                    foreach (string headerWord in headerLabels)
                    {
                        dr[headerWord] = dataWords[columnIndex++];
                    }
                    dt.Rows.Add(dr);
                }
            }
            if (dt.Rows.Count > 0)
            {
                dataGridView2.DataSource = dt;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            openFileDialog1.ShowDialog();
            textBox1.Text = openFileDialog1.FileName;
            listBox1.Items.Clear();
            BindData1(textBox1.Text);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            openFileDialog2.ShowDialog();
            textBox2.Text = openFileDialog2.FileName;
            listBox2.Items.Clear();
            BindData2(textBox2.Text);
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (label28.Text == "Equal")
            {
                string filePath1 = textBox1.Text;
                string filePath2 = textBox2.Text;
                string selectedColumn1 = listBox1.SelectedItem.ToString();
                string selectedColumn2 = listBox2.SelectedItem.ToString();
                string command = "py \"mainn\\main.py\" 1 \"" + filePath1 + "|" + selectedColumn1 + "\" \"" + filePath2 + "|" + selectedColumn2 + "\" 2";
                ExecuteCommand(command);
                Thread.Sleep(1500);
                if (File.Exists(sPath + "\\lineplot X=" + selectedColumn1 + " Y=" + selectedColumn2 + ".png"))
                {
                    pictureBox1.Image = Image.FromFile(sPath + "\\lineplot X=" + selectedColumn1 + " Y=" + selectedColumn2 + ".png");
                }
                else
                {
                    Thread.Sleep(2000);
                    pictureBox1.Image = Image.FromFile(sPath + "\\lineplot X=" + selectedColumn1 + " Y=" + selectedColumn2 + ".png");
                }
            }
            else
            {
                MessageBox.Show("Please select the same number of columns");
            }
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            label7.Text = listBox1.SelectedItem.ToString();
            label12.Text = listBox1.SelectedItem.ToString();
            label16.Text = listBox1.SelectedItem.ToString();
            label14.Text = listBox1.SelectedItem.ToString();
            label18.Text = listBox1.SelectedItem.ToString();
            label20.Text = listBox1.SelectedItem.ToString();
            label22.Text = listBox1.SelectedItem.ToString();
            label26.Text = listBox1.SelectedItem.ToString();
            label32.Text = listBox1.SelectedItem.ToString();
        }

        private void listBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            label9.Text = listBox2.SelectedItem.ToString();
            label10.Text = listBox2.SelectedItem.ToString();
            label24.Text = listBox2.SelectedItem.ToString();
            label33.Text = listBox2.SelectedItem.ToString();
            if (listBox1.Items.Count == listBox2.Items.Count)
            {
                label28.Text = "Equal";
                label28.ForeColor = Color.Green;
            }
            else
            {
                label28.Text = "NotEqual";
                label28.ForeColor = Color.Red;
            }
        }

        public void ExecuteCommand(String command)
        {
            System.Diagnostics.Process process = new System.Diagnostics.Process();
            process.StartInfo.FileName = "cmd.exe";
            process.StartInfo.Arguments = @"/C "+command;
            process.StartInfo.UseShellExecute = false;
            process.StartInfo.CreateNoWindow = true;
            process.StartInfo.RedirectStandardInput = true;
            process.Start();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (label28.Text == "Equal")
            {
                string filePath1 = textBox1.Text;
                string filePath2 = textBox2.Text;
                string selectedColumn1 = listBox1.SelectedItem.ToString();
                string selectedColumn2 = listBox2.SelectedItem.ToString();
                string command = "py \"mainn\\main.py\" 1 \"" + filePath1 + "|" + selectedColumn1 + "\" \"" + filePath2 + "|" + selectedColumn2 + "\" 1";
                ExecuteCommand(command);
                Thread.Sleep(1000);
                if (File.Exists(sPath + "\\scatterplot X=" + selectedColumn1 + " Y=" + selectedColumn2 + ".png")) { 
                    pictureBox1.Image = Image.FromFile(sPath + "\\scatterplot X=" + selectedColumn1+ " Y="+selectedColumn2+".png");
                }
                else
                {
                    Thread.Sleep(2000);
                    pictureBox1.Image = Image.FromFile(sPath + "\\scatterplot X=" + selectedColumn1 + " Y=" + selectedColumn2 + ".png");
                }
            }
                
            else
            {
                MessageBox.Show("Please select the same number of columns");
            }
            //ExecuteCommand("py \"mainn\\main.py\" 1 \"C:\\Users\\MrM\\Desktop\\internship project\\mydata\\CSX.csv|DATE\" \"C:\\Users\\MrM\\Desktop\\internship project\\mydata\\CSX.csv|CSX1\" 1");
            
        }

        private void button5_Click(object sender, EventArgs e)
        {
            if (label16.Text != "")
            {
                string filePath1 = textBox1.Text;
                string filePath2 = textBox2.Text;
                string selectedColumn1 = listBox1.SelectedItem.ToString();
                string selectedColumn2 = listBox2.SelectedItem.ToString();
                string command = "py \"mainn\\main.py\" 1 \"" + filePath1 + "|" + selectedColumn1 + "\" \"" + filePath2 + "|" + selectedColumn2 + "\" 3";
                ExecuteCommand(command);
                Thread.Sleep(1000);
                if (File.Exists(sPath + "\\histogram X=" + selectedColumn1  + ".png"))
                {
                    pictureBox1.Image = Image.FromFile(sPath + "\\histogram X=" + selectedColumn1  + ".png");
                }
                else
                {
                    Thread.Sleep(2000);
                    pictureBox1.Image = Image.FromFile(sPath + "\\histogram X=" + selectedColumn1  + ".png");
                }
            }
            else
            {
                MessageBox.Show("Please select the same number of columns");
            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            if (label16.Text != "")
            {
                string filePath1 = textBox1.Text;
                string filePath2 = textBox2.Text;
                string selectedColumn1 = listBox1.SelectedItem.ToString();
                string selectedColumn2 = listBox2.SelectedItem.ToString();
                string command = "py \"mainn\\main.py\" 1 \"" + filePath1 + "|" + selectedColumn1 + "\" \"" + filePath2 + "|" + selectedColumn2 + "\" 4";
                ExecuteCommand(command);
                Thread.Sleep(1000);
                if (File.Exists(sPath + "\\boxplot X=" + selectedColumn1 + ".png"))
                {
                    pictureBox1.Image = Image.FromFile(sPath + "\\boxplot X=" + selectedColumn1 + ".png");
                }
                
                {
                    Thread.Sleep(2000);
                    pictureBox1.Image = Image.FromFile(sPath + "\\boxplot X=" + selectedColumn1 + ".png");
                }
            }
            else
            {
                MessageBox.Show("Please select the same number of columns");
            }

        }

        private void button7_Click(object sender, EventArgs e)
        {
            if (label16.Text != "")
            {
                string filePath1 = textBox1.Text;
                string filePath2 = textBox2.Text;
                string selectedColumn1 = listBox1.SelectedItem.ToString();
                string selectedColumn2 = listBox2.SelectedItem.ToString();
                string command = "py \"mainn\\main.py\" 1 \"" + filePath1 + "|" + selectedColumn1 + "\" \"" + filePath2 + "|" + selectedColumn2 + "\" 5";
                ExecuteCommand(command);
                Thread.Sleep(1000);
                if (File.Exists(sPath + "\\autocorrelation X=" + selectedColumn1 + ".png"))
                {
                    pictureBox1.Image = Image.FromFile(sPath + "\\autocorrelation X=" + selectedColumn1 + ".png");
                }
                else
                {
                    Thread.Sleep(2000);
                    pictureBox1.Image = Image.FromFile(sPath + "\\autocorrelation X=" + selectedColumn1 + ".png");
                }
            }
            else
            {
                MessageBox.Show("Please select the same number of columns");
            }
        }

        private void button8_Click(object sender, EventArgs e)
        {
            if (label16.Text != "")
            {
                string filePath1 = textBox1.Text;
                string filePath2 = textBox2.Text;
                string selectedColumn1 = listBox1.SelectedItem.ToString();
                string selectedColumn2 = listBox2.SelectedItem.ToString();
                string command = "py \"mainn\\main.py\" 1 \"" + filePath1 + "|" + selectedColumn1 + "\" \"" + filePath2 + "|" + selectedColumn2 + "\" 6";
                ExecuteCommand(command);
                Thread.Sleep(1000);
                if (File.Exists(sPath + "\\acf X=" + selectedColumn1 + ".png"))
                {
                    pictureBox1.Image = Image.FromFile(sPath + "\\acf X=" + selectedColumn1 + ".png");
                }
                else
                {
                    Thread.Sleep(2000);
                    pictureBox1.Image = Image.FromFile(sPath + "\\acf X=" + selectedColumn1 + ".png");
                }
            }
            else
            {
                MessageBox.Show("Please select the same number of columns");
            }
        }

        private void button9_Click(object sender, EventArgs e)
        {
            if (label16.Text != "")
            {
                string filePath1 = textBox1.Text;
                string filePath2 = textBox2.Text;
                string selectedColumn1 = listBox1.SelectedItem.ToString();
                string selectedColumn2 = listBox2.SelectedItem.ToString();
                string command = "py \"mainn\\main.py\" 1 \"" + filePath1 + "|" + selectedColumn1 + "\" \"" + filePath2 + "|" + selectedColumn2 + "\" 7";
                ExecuteCommand(command);
                Thread.Sleep(1000);
                if (File.Exists(sPath + "\\pacf X=" + selectedColumn1 + ".png"))
                {
                    pictureBox1.Image = Image.FromFile(sPath + "\\pacf X=" + selectedColumn1 + ".png");
                }
                else
                {
                    Thread.Sleep(2000);
                    pictureBox1.Image = Image.FromFile(sPath + "\\pacf X=" + selectedColumn1 + ".png");
                }
            }
            else
            {
                MessageBox.Show("Please select the same number of columns");
            }
        }

        private void button10_Click(object sender, EventArgs e)
        {
            if (label28.Text == "Equal")
            {
                string filePath1 = textBox1.Text;
                string filePath2 = textBox2.Text;
                string selectedColumn1 = listBox1.SelectedItem.ToString();
                string selectedColumn2 = listBox2.SelectedItem.ToString();
                string command = "py \"mainn\\main.py\" 1 \"" + filePath1 + "|" + selectedColumn1 + "\" \"" + filePath2 + "|" + selectedColumn2 + "\" 8";
                ExecuteCommand(command);
            }
            else
            {
                MessageBox.Show("Please select the same number of columns");
            }
        }

        private void tabPage4_Click(object sender, EventArgs e)
        {

        }

        private void button11_Click(object sender, EventArgs e)
        {
            if (label28.Text == "Equal")
            {
                string filePath1 = textBox1.Text;
                string filePath2 = textBox2.Text;
                string selectedColumn1 = listBox1.SelectedItem.ToString();
                string selectedColumn2 = listBox2.SelectedItem.ToString();
                string command = "py \"mainn\\main.py\" 2 \"" + filePath1 + "|" + selectedColumn1 + "\" \"" + filePath2 + "|" + selectedColumn2 ;
                ExecuteCommand(command);
            }
            else
            {
                MessageBox.Show("Please select the same number of columns");
            }
        }

        private void button12_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void button13_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
            
        }

        private void button14_Click(object sender, EventArgs e)
        {
            string filePath1 = textBox1.Text;
            string filePath2 = textBox2.Text;
            string selectedColumn1 = "Null";
            string selectedColumn2 = "Null";
            string command = "py \"mainn\\main.py\" 3 \"" + filePath1 + "|" + selectedColumn1 + "\" \"" + filePath2 + "|" + selectedColumn2;
            ExecuteCommand(command);
            textBox4.Text = command;
        }

        private void textBox4_TextChanged(object sender, EventArgs e)
        {

        }
    }
}