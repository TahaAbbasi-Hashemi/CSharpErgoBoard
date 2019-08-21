// Using
using System;
using System.Media;
using System.Drawing;
using System.Windows.Forms;

namespace CSharpErgoBoard
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
        /// <summary>
        /// The default constructor for the popup.
        /// </summary>
        /// <param name="text"> The message intended to appear</param>
        /// <param name="caption"> The name of the popup</param>
        /// <param name="darkMode"> If darkmode is selected at the time of the popups creation. <value>true</value> for darkmode.</param>
        public Popup(String text, String caption, Boolean darkMode)
        {
            SystemSounds.Exclamation.Play();
            Logging.Instance.Log("A popup window was made.", "Information");
            InitializeComponent();
            id_labelText.Text = text;
            Text = caption;
            if (darkMode)
            {
                Color kindofBlack = Color.FromArgb(255, 50, 50, 50);
                Color backgroundBlack = Color.FromArgb(255, 20, 20, 20);
                BackColor = kindofBlack;
                ForeColor = Color.WhiteSmoke;
                id_buttonOkay.BackColor = backgroundBlack;
                id_buttonOkay.ForeColor = System.Drawing.Color.WhiteSmoke;
            }
        }

        /// <summary>
        /// A function corresponding to pressing the okay button on the popup. This ends the life of the popup.
        /// </summary>
        /// <param name="sender"> The object that called this function</param>
        /// <param name="reason"> What event caused the object to call this function</param>
        private void Id_buttonOkay_Click(object sender, EventArgs reason)
        {
            Logging.Instance.Log($"Popup okay button was pressed \" {sender.ToString()} \" by \" {reason.ToString()} \" ",
                                 "Information");
            this.Close();
        }
    }
}
