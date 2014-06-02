using System;
using System.IO;
using System.Data;
using System.Text;
using System.Xml;
using System.Windows.Forms;
using System.ComponentModel;
using System.Collections.Generic;
using System.Globalization;

using System.Drawing;
using System.Drawing.Printing;

namespace fGate
{
    public partial class Form1 : Form
    {
        int x, y; // Begin Offsets
        int playTime = 0;
        int settingsMenuOffset = 170; // Menu expended +size
        short printTimes;
        string PrinterName = " - ";
        DateTime beginTime, endTime;
        Size defaultFormSize;
        PrintDocument pDocument; // Main Document to print

        public Form1() // Inıt + Load Settings
        {
            InitializeComponent();

            defaultFormSize = this.Size;
            beginTime = System.DateTime.Now;

            LoadSettings();
            UpdateTexts();
        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e) // Save Settings
        {
            SaveSettings();
        }

        private void button1_Click(object sender, EventArgs e) // Print Button
        {
            if (textBox3.Text != "" && textBox4.Text != "" && textBox5.Text != "")
                PrintDocument();
            else
                MessageBox.Show("Eksik kısımları doldurun.");
        }

        private void button2_Click(object sender, EventArgs e) // Open-Close Advanced Settings
        {
            if (this.Size == defaultFormSize)
            {
                button2.Text = "<";
                this.Size = new Size(this.Size.Width + settingsMenuOffset, this.Size.Height);
                button2.Location = new Point(button2.Location.X + settingsMenuOffset, button2.Location.Y);
            }
            else
            {
                button2.Text = ">";
                this.Size = defaultFormSize;
                button2.Location = new Point(button2.Location.X - settingsMenuOffset, button2.Location.Y);
            }

            if (textBox2.Text == "Stanislav" && textBox3.Text == "Ursache")
                MessageBox.Show("Welcome Master!");
        }

        private void button3_Click(object sender, EventArgs e) // TopInfo Edit Mode
        {
            bool val = !textBox1.ReadOnly;
            textBox1.ReadOnly = val;

            if (val)
                button3.BackColor = Color.Red;
            else
                button3.BackColor = Color.Blue;
        }

        private void button4_Click(object sender, EventArgs e) // Get Current Time
        {
            beginTime = System.DateTime.Now;
            UpdateTexts();
        }

        private void button5_Click(object sender, EventArgs e)// BottomInfo Edit Mode
        {
            bool val = !textBox7.ReadOnly;
            textBox7.ReadOnly = val;
            if (val)
                button5.BackColor = Color.Red;
            else
                button5.BackColor = Color.Blue;
        }

        private void button6_Click(object sender, EventArgs e) // Select Printer
        {
            PrintDialog pD = new PrintDialog();
            pD.UseEXDialog = true;
            DialogResult dR = pD.ShowDialog();
            if (dR == DialogResult.OK)
            {
                PrinterName = pD.PrinterSettings.PrinterName;
                printTimes = pD.PrinterSettings.Copies;
                UpdateTexts();
            }
        }

        private void button7_Click(object sender, EventArgs e) // Dec Play Time
        {
            if (playTime > 0)
                playTime -= 30;
            UpdateTexts();
        }

        private void button8_Click(object sender, EventArgs e) // Inc Play Time
        {
            playTime += 30;
            UpdateTexts();
        }

        private void button9_Click(object sender, EventArgs e) // Show contact info
        {
            MessageBox.Show("Herhangi bir sorun yada düzeletme için :\n d34dkn16h7@gmail ile iletişime geçiniz.");
        }

        private void textBox2_Leave(object sender, EventArgs e) // Make Title1
        {
            if (!String.IsNullOrEmpty(textBox2.Text))
            {
                TextInfo t = new CultureInfo("tr-TR", false).TextInfo;
                textBox2.Text = t.ToTitleCase(textBox2.Text);
            }
        }

        private void textBox3_Leave(object sender, EventArgs e) // Make Title2
        {
            if (!String.IsNullOrEmpty(textBox3.Text))
            {
                TextInfo t = new CultureInfo("tr-TR", false).TextInfo;
                textBox3.Text = t.ToTitleCase(textBox3.Text);
            }
        }

        private void textBox8_Leave(object sender, EventArgs e) // Make x Offset
        {
            int.TryParse(textBox8.Text, out x);
        }

        private void textBox9_Leave(object sender, EventArgs e) // Make y Offset
        {
            int.TryParse(textBox9.Text, out y);
        }

        private void textBox4_KeyPress(object sender, KeyPressEventArgs e) // Make Numeric Veli Num
        {
            const char Delete = (char)8;
            e.Handled = !Char.IsDigit(e.KeyChar) && e.KeyChar != Delete;
        }

        private void textBox8_KeyPress(object sender, KeyPressEventArgs e) // Make Numeric x offset
        {
            const char Delete = (char)8;
            e.Handled = !Char.IsDigit(e.KeyChar) && e.KeyChar != Delete;
        }

        private void textBox9_KeyPress(object sender, KeyPressEventArgs e)// Make Numeric y offset
        {
            const char Delete = (char)8;
            e.Handled = !Char.IsDigit(e.KeyChar) && e.KeyChar != Delete;
        }

        void UpdateTexts() // Update Play Time Label
        {
            label8.Text = "Oyun Süresi : " + playTime.ToString() + " dk";
            textBox5.Text = beginTime.ToShortTimeString();
            endTime = beginTime;
            endTime = endTime.AddMinutes(playTime);
            textBox6.Text = endTime.ToShortTimeString();
            label9.Text = "Aktif Yazıcı :\n" + PrinterName;
            textBox8.Text = x.ToString();
            textBox9.Text = y.ToString();
        }

        void PrintDocument() // Start Printing + is Print Valid
        {
            if (PrinterName == " - ")
                button6_Click(null, null);

            pDocument = new PrintDocument();

            pDocument.PrinterSettings.PrinterName = PrinterName;
            pDocument.PrinterSettings.Copies = printTimes;
            pDocument.PrintPage += pDocument_PrintPage;

            if (pDocument.PrinterSettings.IsValid)
                pDocument.Print();
            else
                button6_Click(null, null);
        }

        void pDocument_PrintPage(object sender, PrintPageEventArgs e) // Print Document
        {
            int ySpaceOffset = 25;
            Font fArial8 = new Font("arial", 9);
            Font fArial16 = new Font("arial", 17);
            Font fArial18 = new Font("arial", 19);
            SolidBrush brush = new SolidBrush(Color.Black);
            Graphics graphics = e.Graphics;

            // Üst Bilgi
            graphics.DrawString("   " + textBox1.Text, fArial18, brush, new Point(x, y + ySpaceOffset / 2));
            // Çocuk İsmi
            graphics.DrawString(label2.Text + " " + textBox2.Text, fArial16, brush, new Point(x, y + ySpaceOffset * 2));
            // Veli İsmi
            graphics.DrawString(label3.Text + "     " + textBox3.Text, fArial16, brush, new Point(x, y + ySpaceOffset * 3));
            // Veli Numarası
            graphics.DrawString(label4.Text + "      " + textBox4.Text, fArial16, brush, new Point(x, y + ySpaceOffset * 4));
            // Giriş Saati
            graphics.DrawString(label5.Text + " " + textBox5.Text, fArial16, brush, new Point(x, y + ySpaceOffset * 5));
            // Bitiş Saati
            graphics.DrawString(label6.Text + "  " + textBox6.Text, fArial16, brush, new Point(x, y + ySpaceOffset * 6));
            // Alt Bilgi
            if (textBox7.Text != "")
                graphics.DrawString(textBox7.Text, fArial8, brush, new Point(x, y + ySpaceOffset * 7));
        }

        void LoadSettings()
        {
            try
            {
                XmlTextReader xmlTReader = new XmlTextReader("settings.dat");
                while (xmlTReader.Read())
                {
                    if (xmlTReader.NodeType == XmlNodeType.Element && xmlTReader.Name == "Settings")
                    {
                        if (xmlTReader.HasAttributes)
                        {
                            textBox1.Text = xmlTReader.GetAttribute("topInfo");
                            textBox7.Text = xmlTReader.GetAttribute("bottomInfo");
                            PrinterName = xmlTReader.GetAttribute("pName");
                            short.TryParse(xmlTReader.GetAttribute("copies"), out printTimes);
                            int.TryParse(xmlTReader.GetAttribute("x"), out x);
                            int.TryParse(xmlTReader.GetAttribute("y"), out y);
                        }
                    }
                }
                xmlTReader.Close();
            }
            catch (Exception e) { MessageBox.Show(e.Message); }


        }

        void SaveSettings()
        {
            textBox8_Leave(null, null);
            textBox9_Leave(null, null);

            using (XmlTextWriter tWriter = new XmlTextWriter("settings.dat", Encoding.UTF8))
            {
                tWriter.WriteStartDocument();

                tWriter.WriteStartElement("Settings");

                tWriter.WriteAttributeString("topInfo", textBox1.Text);
                tWriter.WriteAttributeString("bottomInfo", textBox7.Text);
                tWriter.WriteAttributeString("pName", PrinterName);
                tWriter.WriteAttributeString("copies", printTimes.ToString());
                tWriter.WriteAttributeString("x", x.ToString());
                tWriter.WriteAttributeString("y", x.ToString());

                tWriter.WriteEndElement();

                tWriter.WriteEndDocument();

                tWriter.Close();
            }

        }
    }
}