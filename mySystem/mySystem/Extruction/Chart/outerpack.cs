using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using mySystem.Extruction.Process;
using Newtonsoft.Json.Linq;
using System.Data.OleDb;

namespace mySystem.Extruction.Chart
{
    public partial class outerpack :   mySystem.BaseForm
    {
        public outerpack(mySystem.MainForm mainform)
            : base(mainform)
        {
            InitializeComponent();
        }
    }
}
