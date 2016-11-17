using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Text.RegularExpressions;
using System.Xml;
using System.Xml.Serialization;
using System.IO.Ports;
using System.Timers;
using System.Threading;
   


namespace deneme
{
    public partial class Form1 : Form
    {

        #region MyRegion
        Data myd = new Data();
        string str;
        string str2;
        List<string> to_screen = new List<string>();
        Encoding enc = System.Text.Encoding.GetEncoding(28591);
        XmlWriter myx;
        XmlWriterSettings mysett = new XmlWriterSettings();
        string[] data_names = { "SOC", "IvmeX", "IvmeY", "FB", "SB", "TB", "FOB", "BaraG", "BaraA", "HIZ", "MotorS", "Gidilen", "HarcananE", "MpptA", "MpptS", "Egim", "Kazanilan" };
        double power;
        List<Control> port_proporties; 
        #endregion


        public Form1()
        {
            InitializeComponent();
            button2.Enabled = false;
            serialPort1.Encoding = enc;
            mysett.Indent = true;
            port_proporties = new List<Control>(new Control[] { comboBox2, comboBox1, comboBox5, textBox2 });
            
            
        }

        private void serialPort1_DataReceived(object sender, System.IO.Ports.SerialDataReceivedEventArgs e)
        {
            if (serialPort1.IsOpen != false)
            {
                try
                {
                    str = serialPort1.ReadLine(); //thread

                    if (str.Substring(0,6) == "$RFGAE")
                        BeginInvoke(new EventHandler(write)); //delegate
                }
                catch
                {
                    return;
                }

            }
        }


        private void write(object sender, EventArgs e)
        {


            try
            {
                str2 = Regex.Split(str, "EAGEND\r").First();
                str2 = str2.Substring(6, str2.Length - 6);
                myd.str_form = str2;
                myd.encode();
                myd.parse_it();
                to_screen = myd.write_collect();
                Control[] mycont = this.Controls.Find("label" + myd.f, true);
                mycont[0].Text = to_screen[myd.f - 1];
                Application.DoEvents();
                if (myd.f == 13)
                    label34.Text = to_screen[16];
                
                else if (myd.f == 8 || myd.f == 9)
                {


                    power = Convert.ToDouble(to_screen[7]) * Convert.ToDouble(to_screen[8]);
                    
                    label39.Text = power.ToString();
                }
                

                if (checkBox1.Checked)
                {
                    int i = 0;
                    myx.WriteStartElement("Data");
                    myx.WriteStartElement("Zaman");
                    myx.WriteString(DateTime.Now.ToString());
                    myx.WriteEndElement();
                    
                    foreach (string dt in to_screen)
                    {
                        myx.WriteStartElement(data_names[i]);
                        myx.WriteString(dt);
                        myx.WriteEndElement();
                        i++;
                    }
                    myx.WriteEndElement();
                }
            }
            catch
            {
                return;
            }


        }


    
 
           
        

        private void button1_Click(object sender, EventArgs e)
        {




            try
            {

                serialPort1.PortName = SerialPort.GetPortNames().First();
                serialPort1.BaudRate = Convert.ToInt32(textBox2.Text);
                serialPort1.Parity = (Parity)Enum.Parse(typeof(Parity), comboBox1.SelectedItem.ToString());
                serialPort1.StopBits = (StopBits)Enum.Parse(typeof(StopBits), comboBox2.SelectedItem.ToString());
                serialPort1.DataBits = Convert.ToInt32(comboBox5.SelectedItem);
            }
            catch(Exception ex)
            {
                MessageBox.Show("Port ayarları doğru değil" + ex.Message);
                return;
            }

            try
            {

                if (checkBox1.Checked)
                {
                    myx = XmlWriter.Create("ARIBALOG-" + DateTime.Now.ToString("yyyy_MM_dd_HH_mm_ss"), mysett);

                    myx.WriteStartElement("ARIBALOG");

                    if (textBox1.Text != null)
                    {
                        myx.WriteStartElement("AÇIKLAMA");
                        myx.WriteString(textBox1.Text);
                        myx.WriteEndElement();
                    }
                }
                serialPort1.Open();
                
            }
            catch (Exception ex)
            {
                MessageBox.Show("Kablo takılı değil" + ex.Message);
                return;

                
            }
            port_proporties.ForEach(x => x.Enabled = false);
            textBox1.Enabled = false;
            checkBox1.Enabled = false;
            button1.Enabled = false;
            button2.Enabled = true;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (checkBox1.Checked)
            {
                
                
                myx.WriteEndElement();
                myx.Close();
            }
            
            serialPort1.Dispose();
            serialPort1.Close();
            checkBox1.Enabled = true;
            textBox1.Enabled = true;
            button1.Enabled = true;
            button2.Enabled = false;
            port_proporties.ForEach(x => x.Enabled = true);
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox1.Checked) textBox1.Enabled = true;
            else textBox1.Enabled = false;
        }

        private void label30_Click(object sender, EventArgs e)
        {

        }
        private void label16_Click(object sender, EventArgs e)
        {

        }
    }
}
