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
        private UInt32 m_row = 0;
        private UInt32 m_col = 0;

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
    }
}
