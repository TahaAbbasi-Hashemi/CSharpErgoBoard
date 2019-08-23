// Using
using System;
using System.Drawing;
using System.Windows.Forms;

namespace FreeErgonomics.Design
{
    /// <summary>
    /// This is intended to be a extension of the combobox class provided by windows forms. 
    /// </summary>
    /// <remarks>
    /// This class exists because the current combo box has a white outline surrounding the button. 
    /// by overriding the draw class we are able to produce a black border ontop of the white border resolving the issue at hand. 
    /// This also allows for ease of converting to dark mode.
    /// </remarks>
    class MyComboBox : ComboBox
    { 
        // Class Attributes
        /// <summary>
        /// The purpose of the class
        /// </summary>
        public String Purpose { get; } = "To allow for multiple modes of pop ups instead of the basic messenger popup";
        /// <summary>
        /// To convert the class to a string.
        /// </summary>
        public new String ToString { get; } = "MyComboBox child of Combobox";

        // Private Constant Members
        /// <summary>
        /// A command provided by windows this is used to indicate painting when dealing with win32 api.
        /// </summary>
        private const int WM_PAINT = 0xF;   // This is a value defined by the actual program.

        // Private Readonly Members
        /// <summary>
        /// The width of the combo box scroll down button.
        /// </summary>
        private readonly Int32  m_buttonWidth = SystemInformation.HorizontalScrollBarArrowWidth;

        // Private Encapsulated Members
        private Boolean m_selectDarkMode = false;

        // Encapsulation Functions 
        /// <summary>
        /// Used to indicate if darkmode is selected or not.
        /// </summary>
        /// <remarks>.
        /// by default this is false.
        /// <list type="bullet">
        /// <item>true</item> <description>\n If the button is on dark mode.</description>
        /// <item>false</item> <description>\n If the button is on light mode.</description>
        /// </list>
        /// </remarks>
        public bool DarkMode { get => m_selectDarkMode; }

        // Functions
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
                            graphics.DrawLine(pen, Width - m_buttonWidth - 1, 0, Width - m_buttonWidth - 1, Height);
                        }
                    }
                    else
                    {
                        using (var pen = new Pen(Color.DimGray))
                        {
                            graphics.DrawRectangle(pen, 0, 0, Width - 1, Height - 1);
                            graphics.DrawLine(pen, Width - m_buttonWidth - 1, 0, Width - m_buttonWidth - 1, Height);
                        }
                    }
                }
            }
        }
        /// <summary>
        /// This is used to change the mode of the combo box
        /// </summary>
        /// <param name="darkModeTrue"> True if dark mode is selected, see <see cref="DarkMode"/> for more information on dark mode.</param>
        public void ModeChange(in Boolean darkModeTrue)
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
