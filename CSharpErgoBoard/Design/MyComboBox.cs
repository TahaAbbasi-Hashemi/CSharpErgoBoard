using System;
using System.Drawing;
using System.Windows.Forms;

namespace CSharpErgoBoard
{
    /// <summary>
    /// This is a Combo Box that can convert from Light to dark mode.
    /// </summary>
    class MyComboBox : ComboBox
    {
        private Boolean m_selectDarkMode = false;
        private const int WM_PAINT = 0xF;
        private int buttonWidth = SystemInformation.HorizontalScrollBarArrowWidth;
        /// <summary>
        /// This adds a small black border around the ComboBox. Normally there is a small white border but this adds a black one now. 
        /// </summary>
        /// <param name="command"> The type of command we are sending the combo box.</param>
        protected override void WndProc(ref Message command)
        {
            base.WndProc(ref command);
            if (command.Msg == WM_PAINT)
            {
                using (var graphics = Graphics.FromHwnd(Handle))
                {
                    if (m_selectDarkMode)
                    {
                        using (var pen = new Pen(Color.Black))
                        {
                            graphics.DrawRectangle(pen, 0, 0, Width - 1, Height - 1);
                            graphics.DrawLine(pen, Width - buttonWidth - 1, 0, Width - buttonWidth - 1, Height);
                        }
                    }
                    else
                    {
                        using (var pen = new Pen(Color.DimGray))
                        {
                            graphics.DrawRectangle(pen, 0, 0, Width - 1, Height - 1);
                            graphics.DrawLine(pen, Width - buttonWidth - 1, 0, Width - buttonWidth - 1, Height);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// This is used to change the mode of the combo box
        /// </summary>
        /// <param name="darkModeTrue"><value>True</value> if dark mode is selected, <value>False</value> if Light mode is selected</param>
        public void ModeChange(Boolean darkModeTrue)
        {
            m_selectDarkMode = darkModeTrue;
            Color kindofBlack = Color.FromArgb(255, 50, 50, 50);
            // If dark Mode Selected
            if (darkModeTrue)
            {
                BackColor = kindofBlack;
                ForeColor = Color.WhiteSmoke;

            }
            // If Light Mode Selected
            else
            {
                BackColor = Color.WhiteSmoke;
                ForeColor = Color.Black;
            }
        }
    }
}
