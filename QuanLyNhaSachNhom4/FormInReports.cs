using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace QuanLyNhaSachNhom4
{
    public partial class FormInReports : Form
    {
        public FormInReports(Object report)
        {
            InitializeComponent();
            crystalReportViewer1.ReportSource = report;
        }
    }
}
