using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CSharpErgoBoard.Design
{
    class MyColorDialog: ColorDialog
    {
        public MyColorDialog(in Boolean darkMode)
        {

            this.ShowDialog();
        }

        public void ModeChanged(in Boolean darkMode)
        {
            ;
        }
    }
}
