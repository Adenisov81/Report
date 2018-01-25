using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Serialization;

namespace ExcelReport
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            ReportData rd = null;
            XmlSerializer serializer = new XmlSerializer(typeof(ReportData));
            StreamReader reader = new StreamReader("TestData.xml");
            rd = (ReportData)serializer.Deserialize(reader);
            reader.Close();
            Report rpt = new Report();
            rpt.Data = rd;
            rpt.FileName = "Отчет по грузообороту.xlsx";
            rpt.AddWorksheet("ReportDay.xml");
            rpt.AddWorksheet("Dynamic.xml");
            rpt.AddWorksheet("Comparison.xml");
            //Отчет по грузообороту
            if (rpt.CreateReport()) rpt.OpenFile(); 
        }
    }
}
