namespace CSharpErgoBoard.Design
{
    /// <summary>
    /// This is intended to be a extension of the button class provided by windows forms. 
    /// </summary>
    /// <remarks>
    /// This class is built with the intention of allowing for a ease of converting buttons to dark mode. 
    /// This also supports the keyboard buttons which use images. 
    /// 
    /// In order to unselect a function a selection must first be made.
    /// This is because you can only select a button knowing what the button is, but you do not have to know what the button is to unselect it to select something else.
    /// If you were to select another button you must first unselect the currently selected button.
    /// </remarks>
    class MyButton : System.Windows.Forms.Button
    {
        // Class Attributes
        /// <summary>
        /// The purpose of the class
        /// </summary>
        public System.String Purpose { get; } = "To allow for ease of when buttons are converted to and from dark mode along side with entering selection mode for keyboard buttons.";
        /// <summary>
        /// To convert the class to a System.String.
        /// </summary>
        public new System.String ToString { get; } = "Design.MyButton child of Button";

        /// <summary>
        /// Design.MyButton Class error.
        /// </summary>
        [System.Serializable()]
        private class MyButtonError : System.Exception
        {
            // Functions
            /// <summary>
            /// Default constructor
            /// </summary>
            public MyButtonError() : base() { }
            /// <summary>
            /// Constructor with a message.
            /// </summary>
            /// <param name="message"> The message being reported to the user.</param>
            public MyButtonError(System.String message) : base(message) { }
            /// <summary>
            /// A Constructor with a inner error
            /// </summary>
            /// <param name="message"> The message being reported.</param>
            /// <param name="inner"> The error that progogated this error.</param>
            public MyButtonError(System.String message, System.Exception inner) : base(message, inner) { }
        }

        // Private Encapsulated Members
        private System.Boolean m_selectDarkMode = false;
        private System.Boolean m_selected = false;
        private System.String m_type = null;

        // Private Readonly Members
        /// <summary>
        /// This is a dark black that has less strain on the average persons eye. This is is more of a very dark gray.
        /// </summary>
        private readonly System.Drawing.Color m_kindofBlack = System.Drawing.Color.FromArgb(255, 50, 50, 50);

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
        public System.Boolean DarkMode { get => m_selectDarkMode; }
        /// <summary>
        /// Used to indicate if we are currently selected or not.
        /// </summary>
        /// <remarks>
        /// This is intended to only be used by the image buttons. 
        /// By default this is false.
        /// 
        /// <list type="bullet">
        /// <item>true</item> <description>\n If the user has currently selected this button.</description>
        /// <item>false</item> <description>\n If the user has not currently selected this button</description>
        /// </list>
        /// </remarks>
        public System.Boolean IsSelected { get => m_selected; }
        /// <summary>
        /// A System.String indicating the type of button is being used.  
        /// </summary>
        /// <remarks>
        /// Only keyboard buttons use this feature.
        /// By default this is null.
        /// 
        /// <list type="bullet">
        /// <item>Tall</item> <description>\n This is intended for vertical 2U key caps.</description>
        /// <item>TallLED</item> <description>\n This is for the led version of Tall.</description>
        /// <item>Wide</item> <description>\n This is intended for horizontal 1.5U key caps.</description>
        /// <item>WideLED</item> <description>\n This is for the LED version of Wide.</description>
        /// <item>Single</item> <description>\n This is intended for 1U key caps. </description>
        /// <item>SingleLED</item> <description>\n This is for the LED version of Single.</description>
        /// </list>
        /// </remarks>
        public System.String ButtonType { get => m_type; }
        /// <summary>
        /// The row that the Image button is on relative to the keyboard. This can only be used by image buttons.
        /// </summary>
        public System.Int32 Row { get; set; } = 0;
        /// <summary>
        /// The colum that the image button is on relative to the keyboard. This can only be used by image buttons.
        /// </summary>
        public System.Int32 Col { get; set; } = 0;

        // Functions
        /// <summary>
        /// This is used for mode conversion between light and dark mode for basic button types.
        /// </summary>
        /// <BugFix> 
        /// Added Select() to prevent a bug where buttons are considered selected when they should not be.
        /// </BugFix>
        /// <param name="darkModeTrue"> true if darkmode is selected. See <see cref="DarkMode"/> for more information.</param>
        public void ModeChange(in System.Boolean darkModeTrue)
        {
            m_selectDarkMode = darkModeTrue;
            Select();   // This is to get rid of a bug where buttons are randomly selected when the program starts.

            // If dark Mode Selected
            if (darkModeTrue)
            {
                BackColor = m_kindofBlack;
                ForeColor = System.Drawing.Color.WhiteSmoke;
            }
            // If Light Mode Selected
            else
            {
                BackColor = System.Drawing.Color.WhiteSmoke;
                ForeColor = System.Drawing.Color.Black;
            }
        }
        /// <summary>
        /// This is for mode conversion between light and dark for image type buttons.
        /// </summary>
        /// <param name="darkModeTrue"> true if darkmode is selected. See <see cref="DarkMode"/> for more information.</param>
        /// <param name="type">The type of button that is undergoing the mode change. See <see cref="ButtonType"/> for button types.</param>
        /// <exception cref="MyButtonError"> When a unusable type is selected.</exception>
        public void ModeChange(in System.Boolean darkModeTrue, in System.String type)
        {
            m_selectDarkMode = darkModeTrue;
            m_type = type;

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
                    else
                    {
                        throw new MyButtonError("The wrong type of button was selected on button with name :" + Name);
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
                    else
                    {
                        throw new MyButtonError("The wrong type of button was selected on button with name :" + Name);
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
                    else
                    {
                        throw new MyButtonError("The wrong type of button was selected on button with name :" + Name);
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
                    else
                    {
                        throw new MyButtonError("The wrong type of button was selected on button with name :" + Name);
                    }
                }
            }
        }
        /// <summary>
        /// If one of the image buttons is selected.
        /// </summary>
        /// <remarks>
        /// See <see cref="IsSelected"/> for more information on what selection means.\n
        /// See <see cref="UnSelected"/> for more information in regards to unselecting a button.
        /// </remarks>
        /// <exception cref="MyButtonError"> When a unusable type is selected.</exception>
        public void Selected()
        {
            // Update row and col
            Row = (System.Int32)(Name[Name.Length - 3]) - '0';
            Col = (System.Int32)(Name[Name.Length - 1]) - '0';
            m_selected = true;
            ModeChange(m_selectDarkMode, m_type);
        }
        /// <summary>
        /// If one of the image buttons is unselected.
        /// </summary>
        /// <remarks>
        /// See <see cref="IsSelected"/> for more information on what selection means.\n
        /// See <see cref="Selected"/> for more information in regards to selecting a button.
        /// </remarks>
        /// <exception cref="MyButtonError"> When Unselected occurs before selection a selection was made.</exception>
        public void UnSelected()
        {
            if (m_type == null)
            {
                throw new MyButtonError("Unselected function occured before selection function on button with name :" + Name);
            }
            m_selected = false;
            ModeChange(m_selectDarkMode, m_type);
        }
        /// <summary>
        /// Produces a controller readbale value that can be used.
        /// </summary>
        /// <returns>A System.String containing the row and colums. EG : R1C1</returns>
        public System.String MakeKeyName()
        {
            return "R" + Name[Name.Length-3] + "C" + Name[Name.Length - 1];
        }
        /// <summary>
        /// Produces a human reablable value that can be used.
        /// </summary>
        /// <returns> A System.String containing the row and column. EG: Row 1, Column 1</returns>
        public System.String MakeKeyNameValue()
        {
            return "Row " + Row.ToString() + ", Column " + Col.ToString();
        }
    }
}
