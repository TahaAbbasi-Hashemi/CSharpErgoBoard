using System;
using System.Drawing;
using System.Windows.Forms;

namespace CSharpErgoBoard
{
    /// <summary>
    /// A Button that can convert to dark mode
    /// </summary>
    class MyButton : Button
    {
        private Boolean m_selectDarkMode = false;
        private Color m_kindofBlack = Color.FromArgb(255, 50, 50, 50);
        private Boolean m_selected = false;
        private String m_type = "Single";

        /// <summary>
        /// Allows conversion between light or dark mode.
        /// </summary>
        /// <param name="darkModeTrue"> <value>true</value> if you are using dark mode, <value>false</value> if you are using light mode</param>
        public void ModeChange(Boolean darkModeTrue)
        {
            m_selectDarkMode = darkModeTrue;

            // If dark Mode Selected
            if (darkModeTrue)
            {
                BackColor = m_kindofBlack;
                ForeColor = Color.WhiteSmoke;
            }
            // If Light Mode Selected
            else
            {
                BackColor = Color.WhiteSmoke;
                ForeColor = Color.Black;
            }
        }
        public void ModeChange(Boolean darkModeTrue, String type)
        {
            m_selectDarkMode = darkModeTrue;

            if (!type.Contains("LED"))
            {
                ModeChange(darkModeTrue);
            }
            else
            {
                Text = "";
                BackColor = BackColor;
            }
            // Dark Mode
            if (darkModeTrue)
            {
                // If not selected
                if (!m_selected)
                {
                    if (type == "Tall")
                    {
                        Image = Properties.Resources.TallKeyDark;
                    }
                    else if (type == "TallLED")
                    {
                        Image = Properties.Resources.TallKeyDarkLED;
                    }
                    else if (type == "Wide")
                    {
                        Image = Properties.Resources.WideKeyDark;
                    }
                    else if (type == "WideLED")
                    {
                        Image = Properties.Resources.WideKeyDarkLED;
                    }
                    else if (type == "Single")
                    {
                        Image = Properties.Resources.SingleKeyDark;
                    }
                    else if (type == "SingleLED")
                    {
                        Image = Properties.Resources.SingleKeyDarkLED;
                    }
                }
                // If selected
                else
                {
                    if (type == "Tall")
                    {
                        Image = Properties.Resources.TallKeyDarkSelected;
                    }
                    else if (type == "Wide")
                    {
                        Image = Properties.Resources.WideKeyDarkSelected;
                    }
                    else if (type == "Single")
                    {
                        Image = Properties.Resources.SingleKeyDarkSelected;
                    }
                }
            }

            // Light Mode
            else
            {
                // Keyboard Buttons
                if (!m_selected)
                {
                    if (type == "Tall")
                    {
                        Image = Properties.Resources.TallKeyLight;
                    }
                    else if (type == "TallLED")
                    {
                        Image = Properties.Resources.TallKeyLightLED;
                    }
                    else if (type == "Wide")
                    {
                        Image = Properties.Resources.WideKeyLight;
                    }
                    else if (type == "WideLED")
                    {
                        Image = Properties.Resources.WideKeyLightLED;
                    }
                    else if (type == "Single")
                    {
                        Image = Properties.Resources.SingleKeyLight;
                    }
                    else if (type == "SingleLED")
                    {
                        Image = Properties.Resources.SingleKeyLightLED;
                    }
                }
                else
                {
                    if (type == "Tall")
                    {
                        Image = Properties.Resources.TallKeyLightSelected;
                    }
                    else if (type == "Wide")
                    {
                        Image = Properties.Resources.WideKeyLightSelected;
                    }
                    else if (type == "Single")
                    {
                        Image = Properties.Resources.SingleKeyLightSelected;
                    }
                }
            }
        }

        public void Selected(String type)
        {
            m_selected = true;
            m_type = type;
            ModeChange(m_selectDarkMode, type);
        }

        public void UnSelected()
        {
            m_selected = false;
            ModeChange(m_selectDarkMode, m_type);
        }
    }
}
