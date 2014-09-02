using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Text.RegularExpressions;

namespace CompareLists
{
    public partial class frmFileCompare : Form
    {
        public frmFileCompare()
        {
            InitializeComponent();
        }

      
        private void btnSelectFile1_Click(object sender, EventArgs e)
        {
            OpenFileDialog fd = new OpenFileDialog();
            fd.ShowDialog();
            txtFile1.Text = fd.FileName;
        }

        private void btnSelectFile2_Click(object sender, EventArgs e)
        {
            OpenFileDialog fd = new OpenFileDialog();
            fd.ShowDialog();
            txtFile2.Text = fd.FileName;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            List<string> l1 = CreateList(txtFile1.Text);
            List<string> l2 = CreateList(txtFile2.Text);

            List<string> lout = CompareLists(l1, l2);

            StreamWriter sw = new StreamWriter(txtOutput.Text);
            foreach (string s in lout)
                sw.WriteLine(s);
            sw.Close();
            MessageBox.Show("Finished");

        }

        private List<string> CompareLists (List<string> List1, List<string> list2)
        {
            List<string> result = new List<string>();

            var q = from i in List1
                    where (!list2.Contains(i))
                    select i;

            foreach (string s in q)
                result.Add(s);

            return result;
        }

        private List<string> CreateList(string filename)
        {
            StreamReader sr = new StreamReader(filename);
            List<string> list = new List<string>();

            int fieldnum = Convert.ToInt32 (cboField.Text);

            while (sr.Peek() > -1)
            {
                string dataline = sr.ReadLine();
                string[] flds = Regex.Split(dataline, "\t");
                list.Add (flds[fieldnum]);
            }
            sr.Close();
            return list;
        }

        private void btnSelectOutput_Click(object sender, EventArgs e)
        {
            SaveFileDialog sd = new SaveFileDialog();
            sd.ShowDialog();
            txtOutput.Text = sd.FileName;
        }

        private void btnCompare2_Click(object sender, EventArgs e)
        {
            List<string> thelist = CreateList(txtFile2.Text);
            CompareFileToList(txtFile1.Text, thelist);
            MessageBox.Show("Finished");
        }

        private void CompareFileToList(string filename, List<string> l)
        {
            StreamReader sr = new StreamReader(filename);
            StreamWriter sw = new StreamWriter(txtOutput.Text);
            while (sr.Peek() > -1)
            {
                string dataline = sr.ReadLine();
                string[] flds = Regex.Split(dataline, "\t");
                int fieldtocheck = Convert.ToInt32(cboField.Text);
                if (!l.Contains(flds[fieldtocheck]))
                {
                    //write to output file
                    sw.WriteLine(flds[fieldtocheck]);
                }

            }
            sr.Close();
            sw.Close();
        }

    }
}
