using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace mySystem.Setting
{
    public partial class SetPeopleForm : BaseForm
    {
        public SetPeopleForm(MainForm mainform):base(mainform)
        {
            InitializeComponent();
            RoleInit();
        }

        private void RoleInit()
        {
            switch (base.mainform.userRole)
            {
                case 1:
                    SetPeoplePanelBottom.Visible = false;
                    break;
                case 2:
                    SetPeoplePanelBottom.Visible = false;
                    break;
                case 3:
                    SetPeoplePanelBottom.Visible = true;
                    break;
                default:
                    break;
            }

        }

    }
}
