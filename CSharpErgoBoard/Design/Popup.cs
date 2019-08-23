// Using
using System;
using System.Media;
using System.Drawing;
using System.Windows.Forms;

namespace CSharpErgoBoard.Design
{
    /// <summary>
    /// This is intended to be a message box that is able to enter dark mode.
    /// </summary>
    /// <remarks>
    /// As regular message boxes are unable to change apperence this is made to allow for appearance changes in the message box. 
    /// This is generally created to let the user know that they have made a error somewhere and need to edit or change something.
    /// The popup also produces a windows based noise for error popups. 
    /// </remarks>
    public partial class Popup : Form
    {
        // Class Atributes
        /// <summary>
        /// The purpose of the class
        /// </summary>
        public String Purpose { get; } = "To allow for multiple modes of pop ups instead of the basic messenger popup";
        /// <summary>
        /// To convert the class to a string.
        /// </summary>
        public new String ToString { get; } = "Popup child of Form";
        
        // Functions
        /// <summary>
        /// The default constructor for the popup.
        /// </summary>
        /// <param name="text"> The message intended to appear</param>
        /// <param name="caption"> The name of the popup</param>
        /// <param name="darkMode"> If darkmode is selected at the time of the popups creation. <value>true</value> for darkmode.</param>
        public Popup(String text, String caption, Boolean darkMode)
        {
            SystemSounds.Exclamation.Play();
            Programming.Logging.Instance.Log("A popup window was made with the caption : " + caption, "Debug");
            InitializeComponent();
            id_labelText.Text = text;
            Text = caption;

            // Dark mode 
            if (darkMode)
            {
                Color kindofBlack = Color.FromArgb(255, 50, 50, 50);
                Color backgroundBlack = Color.FromArgb(255, 20, 20, 20);
                BackColor = kindofBlack;
                ForeColor = Color.WhiteSmoke;
                id_buttonOkay.BackColor = backgroundBlack;
                id_buttonOkay.ForeColor = System.Drawing.Color.WhiteSmoke;
            }

            // Resizing
            Width = id_labelText.Size.Width + 100;

            Point labelLocation = id_labelText.Location;
            labelLocation.X = (Width / 2) - (id_labelText.Size.Width / 2);
            id_labelText.Location = labelLocation;

            Point buttonLocation = id_buttonOkay.Location;
            buttonLocation.X = (Width / 2) - (id_buttonOkay.Size.Width / 2);
            buttonLocation.Y = id_labelText.Size.Height + labelLocation.Y + 10;
            id_buttonOkay.Location = buttonLocation;


            Height = buttonLocation.Y + id_buttonOkay.Size.Height + 50;

            this.Show();
            this.Update();
            
        }
        /// <summary>
        /// A function corresponding to pressing the okay button on the popup. This ends the life of the popup.
        /// </summary>
        /// <param name="sender"> The object that called this function</param>
        /// <param name="reason"> What event caused the object to call this function</param>
        private void Id_buttonOkay_Click(object sender, EventArgs reason)
        {
            Programming.Logging.Instance.Log("Popup with caption: '" + Text + "' okay button was pressed", "Debug");
            this.Close();
        }
        /// <summary>
        /// A function that automatically occurs when the popup is loaded.
        /// </summary>
        /// <param name="sender">The object that called this function </param> 
        /// <param name="reason">What event caused the object to call this function </param>
        private void Popup_Load(object sender, EventArgs reason) => FormClosing += Popup_Closing;
        /// <summary>
        /// A function that occurs when the popup closes.
        /// </summary>
        /// <param name="sender">The object that called this function </param> 
        /// <param name="reason">What event caused the object to call this function </param>
        private void Popup_Closing(object sender, EventArgs reason)
        {
            Programming.Logging.Instance.Log("Popup with caption: '" + Text + "' is now closing", "Debug");
        }
    }
}
