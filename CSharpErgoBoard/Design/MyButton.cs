using System;
using System.Collections.Specialized;
using System.Drawing;
using System.Windows.Forms;
using System.Xml;
using System.Configuration;

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
    /// 
    /// Very often the changeMode functions act as secondary constructors. This is because while they do not construct anything they are always called immedatly when the program starts. 
    /// This allows for any GUI hard changes to go through and allow for soft modifications to happen after. This is not a good method but it does the job as expected.;
    /// </remarks>
    class MyButton : Button
    {
        // Class Attributes
        /// <summary>
        /// The purpose of the class
        /// </summary>
        public String Purpose { get; } = "To allow for ease of when buttons are converted to and from dark mode along side with entering selection mode for keyboard buttons.";
        /// <summary>
        /// To convert the class to a String.
        /// </summary>
        public new String ToString { get; } = "Design.MyButton child of Button";

        /// <summary>
        /// Design.MyButton Class error.
        /// </summary>
        [Serializable()]
        private class MyButtonError : Exception
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
            public MyButtonError(String message) : base(message) { }
            /// <summary>
            /// A Constructor with a inner error
            /// </summary>
            /// <param name="message"> The message being reported.</param>
            /// <param name="inner"> The error that progogated this error.</param>
            public MyButtonError(String message, Exception inner) : base(message, inner) { }
        }

        // Private Encapsulated Members
        /// <summary>
        /// True if darkmode is selected, false if light mode is selected
        /// </summary>
        private Boolean m_selectDarkMode = false;
        /// <summary>
        /// If the button is currently selected by the user.
        /// </summary>
        private Boolean m_selected = false;
        /// <summary>
        /// The side that the key is on.
        /// </summary>
        private String m_side = null;
        /// <summary>
        /// If the key is a led or regular key
        /// </summary>
        private String m_type = null;
        /// <summary>
        /// If it is single, tall or side.
        /// </summary>
        private String m_size = null;
        /// <summary>
        /// The layer the left keyboard is on
        /// </summary>
        private static UInt32 m_leftLayer = 1;
        /// <summary>
        /// The layer that the right keyboard is on.
        /// </summary>
        private static UInt32 m_rightLayer = 1;

        // Private Readonly Members
        /// <summary>
        /// This is a dark black that has less strain on the average persons eye. This is is more of a very dark gray.
        /// </summary>
        private readonly Color m_kindofBlack = Color.FromArgb(255, 50, 50, 50);



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
        public Boolean DarkMode { get => m_selectDarkMode; }
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
        public Boolean IsSelected { get => m_selected; }
        /// <summary>
        /// A String indicating the type of button is being used.  
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
        public String ButtonSize { get => m_size; }
        /// <summary>
        /// The row that the Image button is on relative to the keyboard. This can only be used by image buttons.
        /// </summary>
        public Int32 Row { get; set; } = 0;
        /// <summary>
        /// The colum that the image button is on relative to the keyboard. This can only be used by image buttons.
        /// </summary>
        public Int32 Col { get; set; } = 0;
        /// <summary>
        /// The layer that the left keyboard is currently on. this is static so that all the keys on the left hand side are on the exact same layer.
        /// </summary>
        public static UInt32 LeftLayer { get => m_leftLayer; set => m_leftLayer = value; }
        /// <summary>
        /// The layer that the right keyboard is currently on. This is static so that all the keys on the right hand side are on the same exact layer.
        /// </summary>
        public static UInt32 RightLayer { get => m_rightLayer; set => m_rightLayer = value; }

        // Functions
        /// <summary>
        /// Default constructor sets up the internal values by finding out the contents of the name. 
        /// </summary>
        public MyButton()
        {

        }
        /// <summary>
        /// This is used for mode conversion between light and dark mode for basic button types.
        /// </summary>
        /// <BugFix> 
        /// Added Select() to prevent a bug where buttons are considered selected when they should not be.
        /// </BugFix>
        /// <param name="darkModeTrue"> true if darkmode is selected. See <see cref="DarkMode"/> for more information.</param>
        public void ModeChange(in Boolean darkModeTrue)
        {
            m_selectDarkMode = darkModeTrue;
            Select();   // This is to get rid of a bug where buttons are randomly selected when the program starts.

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
        /// <summary>
        /// This is for mode conversion between light and dark for image type buttons.
        /// </summary>
        /// <param name="darkModeTrue"> true if darkmode is selected. See <see cref="DarkMode"/> for more information.</param>
        /// <param name="size">The type of button that is undergoing the mode change. See <see cref="ButtonSize"/> for button types.</param>
        /// <exception cref="MyButtonError"> When a unusable type is selected.</exception>
        public void ModeChange(in Boolean darkModeTrue, in String size)
        {
            m_selectDarkMode = darkModeTrue;
            m_size = size;
            UpdateValues();

            if (!size.Contains("LED"))
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
                    if (size == "Tall")
                    {
                        Image = Properties.Resources.TallKeyDark;
                    }
                    else if (size == "TallLED")
                    {
                        Image = Properties.Resources.TallKeyDarkLED;
                    }
                    else if (size == "Wide")
                    {
                        Image = Properties.Resources.WideKeyDark;
                    }
                    else if (size == "WideLED")
                    {
                        Image = Properties.Resources.WideKeyDarkLED;
                    }
                    else if (size == "Single")
                    {
                        Image = Properties.Resources.SingleKeyDark;
                    }
                    else if (size == "SingleLED")
                    {
                        Image = Properties.Resources.SingleKeyDarkLED;
                    }
                    else
                    {
                        throw new MyButtonError("The wrong size of button was selected on button with name :" + Name);
                    }
                }
                // If selected
                else
                {
                    if (size == "Tall")
                    {
                        Image = Properties.Resources.TallKeyDarkSelected;
                    }
                    else if (size == "Wide")
                    {
                        Image = Properties.Resources.WideKeyDarkSelected;
                    }
                    else if (size == "Single")
                    {
                        Image = Properties.Resources.SingleKeyDarkSelected;
                    }
                    else
                    {
                        throw new MyButtonError("The wrong size of button was selected on button with name :" + Name);
                    }
                }
            }

            // Light Mode
            else
            {
                // Keyboard Buttons
                if (!m_selected)
                {
                    if (size == "Tall")
                    {
                        Image = Properties.Resources.TallKeyLight;
                    }
                    else if (size == "TallLED")
                    {
                        Image = Properties.Resources.TallKeyLightLED;
                    }
                    else if (size == "Wide")
                    {
                        Image = Properties.Resources.WideKeyLight;
                    }
                    else if (size == "WideLED")
                    {
                        Image = Properties.Resources.WideKeyLightLED;
                    }
                    else if (size == "Single")
                    {
                        Image = Properties.Resources.SingleKeyLight;
                    }
                    else if (size == "SingleLED")
                    {
                        Image = Properties.Resources.SingleKeyLightLED;
                    }
                    else
                    {
                        throw new MyButtonError("The wrong size of button was selected on button with name :" + Name);
                    }
                }
                else
                {
                    if (size == "Tall")
                    {
                        Image = Properties.Resources.TallKeyLightSelected;
                    }
                    else if (size == "Wide")
                    {
                        Image = Properties.Resources.WideKeyLightSelected;
                    }
                    else if (size == "Single")
                    {
                        Image = Properties.Resources.SingleKeyLightSelected;
                    }
                    else
                    {
                        throw new MyButtonError("The wrong size of button was selected on button with name :" + Name);
                    }
                }
            }

            UpdateButton();
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
            m_selected = true;
            ModeChange(m_selectDarkMode, m_size);
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
            if (m_size == null)
            {
                throw new MyButtonError("Unselected function occured before selection function on button with name :" + Name);
            }
            m_selected = false;
            ModeChange(m_selectDarkMode, m_size);
        }
        /// <summary>
        /// Produces a controller readbale value that can be used.
        /// </summary>
        /// <returns>A String containing the row and colums. EG : R1C1</returns>
        public String MakeKeyName()
        {
            return "R" + Name[Name.Length - 3] + "C" + Name[Name.Length - 1];
        }
        /// <summary>
        /// Produces a human reablable value that can be used.
        /// </summary>
        /// <returns> A String containing the row and column. EG: Row 1, Column 1</returns>
        public String MakeKeyNameValue()
        {
            return "Row " + Row.ToString() + ", Column " + Col.ToString();
        }
        /// <summary>
        /// Produces name that is used by the setting saving function.
        /// </summary>
        /// <returns>A setting readbale name of the button</returns>
        private String MakeSettingName()
        {
            // "LeftKeyR1C1L1
            String name = "";
            name += m_side;
            name += m_type;
            name += MakeKeyName();
            name += "L";
            if (m_side == "Left")
            {
                if (m_leftLayer == 0)
                    name += m_leftLayer.ToString();
            }
            else if (m_side == "Right")
            {
                name += m_rightLayer.ToString();
            }
            return name;
        }
        /// <summary>
        /// The name of the button holds the information about the button. 
        /// Unforuntatly name is not part of the constructor so it goes here instead. 
        /// </summary>
        private void UpdateValues()
        {
            // Update row and col
            Row = (Int32)(Name[Name.Length - 3]) - '0';
            Col = (Int32)(Name[Name.Length - 1]) - '0';
            if (Name.Length == 0)
            {
                throw new Exception("something is wrong here");
            }
            if (Name.Contains("id_buttonRight"))
            {
                m_side = "Right";
            }
            else if (Name.Contains("Id_buttonLeft"))
            {
                m_side = "Left";
            }
            if (Name.Contains("Key"))
            {
                m_type = "Key";
            }
            else if (Name.Contains("Led"))
            {
                m_type = "Led";
            }
        }
        /// <summary>
        /// Loads information from the user settings.  
        /// </summary>
        /// <returns>True if the button was properly updated, and false if it failed.</returns>
        public Boolean UpdateButton()
        {
            UpdateValues();
            try
            {
                NameValueCollection appSettings = ConfigurationManager.AppSettings;
                if (appSettings[MakeSettingName()] != null)
                {
                    Text = appSettings[MakeSettingName()];
                }
            }
            catch (ConfigurationErrorsException)
            {
                new Popup("Error", "Error", true);  //I do not know what to do if we have a error here.
                return false;
            }
            return true;
        }
        /// <summary>
        /// Saves the value of the button to the user settings
        /// </summary>
        /// <param name="value">The text that the button currently holds</param>
        /// <returns>True if the informatino was saved, false if it was not.</returns>
        public Boolean SaveButton(in String value = "None")
        {
            try
            {
                Configuration configFile = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
                KeyValueConfigurationCollection settings = configFile.AppSettings.Settings;
                if (settings[MakeSettingName()] == null)
                {
                    settings.Add(MakeSettingName(), value);
                }
                else
                {
                    settings[MakeSettingName()].Value = value;
                }
                configFile.Save(ConfigurationSaveMode.Modified);
                ConfigurationManager.RefreshSection(configFile.AppSettings.SectionInformation.Name);
            }
            catch (ConfigurationErrorsException)
            {
                new Popup("Error", "Error", true);  // I do not know what to do if we have a error here.
                return false;
            }
            return UpdateButton();
        }
    }
}
