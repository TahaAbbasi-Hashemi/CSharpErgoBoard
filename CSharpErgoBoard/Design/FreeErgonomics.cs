﻿//using System.Collections.Generic;
//using System.Management;
//using System.ComponentModel;
//using System.Data;
//using System.Drawing;
//using System.Linq;
//using System.Text;
//using System.Threading;
//using System.Threading.Tasks;
//using System.Windows.Forms;
//using System.IO.Ports;

namespace CSharpErgoBoard.Design
{
    /// <summary>
    /// The graphical interface class
    /// </summary>
    /// <remarks>
    /// This holds all the information regarding what the interface does and what the buttons on the interface do.
    /// </remarks>
    public partial class FreeErgonomics : System.Windows.Forms.Form
    {
        // Class Atributes
        /// <summary>
        /// The purpose of the class
        /// </summary>
        public System.String Purpose { get; } = "To display and work with the Graphical interface";
        /// <summary>
        /// To convert the class to a System.String.
        /// </summary>
        public new System.String ToString { get; } = "Ergoboard child of Form";

        // Private Variables
        /// <summary>
        /// This essentially acts as a reference to the button that is selected by the user.
        /// </summary>
        private Design.MyButton m_leftSelectedKeyButton = null;
        private Design.MyButton m_rightSelectedKeyButton = null;
        private readonly Programming.FreeErgonomicsBrain m_processing = new Programming.FreeErgonomicsBrain();
        private System.Boolean m_selectedDarkMode = false;

        // Functions
        /// <summary>
        /// Default constructor.
        /// </summary>
        public FreeErgonomics()
        {
            InitializeComponent();
            // List the serial port names.
            System.Collections.Generic.List<System.String> possiblePorts = m_processing.GetPorts();
            foreach (System.String port in possiblePorts)
            {
                id_comboboxRightKeyComPort.Items.Add(port);
                id_comboboxLeftKeyComPort.Items.Add(port);
                id_comboboxLeftLedComPort.Items.Add(port);
                id_comboboxRightLedComPort.Items.Add(port);
            }

            if (Properties.Settings.Default.Darkmode)
            {
                SelectDarkMode();
            }
            else
            {
                SelectLightMode();
            }
        }



        /// <summary>
        /// Changes the System.Drawing.Color layout of the application to a dark mode style. 
        /// </summary>
        public void SelectDarkMode()
        {
            Programming.Logging.Instance.Log("Selected Dark Mode");
            m_selectedDarkMode = true;

            System.Drawing.Color backgroundBlack = System.Drawing.Color.FromArgb(255, 20, 20, 20);

            // Main Window
            BackColor = backgroundBlack;
            ForeColor = System.Drawing.Color.WhiteSmoke;
            id_panelMain.BackColor = backgroundBlack;
            id_panelMain.ForeColor = System.Drawing.Color.WhiteSmoke;

            //Menu Strip
            menuStrip1.BackColor = System.Drawing.Color.DimGray;
            menuStrip1.ForeColor = System.Drawing.Color.WhiteSmoke;
            id_menuFile.BackColor = System.Drawing.Color.DimGray;
            id_menuEdit.BackColor = System.Drawing.Color.DimGray;
            id_menuView.BackColor = System.Drawing.Color.DimGray;
            id_menuHelp.BackColor = System.Drawing.Color.DimGray;
            id_menuAbout.BackColor = System.Drawing.Color.DimGray;
            id_menuAuthor.BackColor = System.Drawing.Color.DimGray;
            id_menuReferences.BackColor = System.Drawing.Color.DimGray;
            id_menuCopyright.BackColor = System.Drawing.Color.DimGray;
            id_menuLightMode.BackColor = System.Drawing.Color.DimGray;
            id_menuDarkMode.BackColor = System.Drawing.Color.DimGray;
            id_menuQuit.BackColor = System.Drawing.Color.DimGray;
            id_menuFile.ForeColor = System.Drawing.Color.WhiteSmoke;
            id_menuEdit.ForeColor = System.Drawing.Color.WhiteSmoke;
            id_menuView.ForeColor = System.Drawing.Color.WhiteSmoke;
            id_menuHelp.ForeColor = System.Drawing.Color.WhiteSmoke;
            id_menuAbout.ForeColor = System.Drawing.Color.WhiteSmoke;
            id_menuAuthor.ForeColor = System.Drawing.Color.WhiteSmoke;
            id_menuReferences.ForeColor = System.Drawing.Color.WhiteSmoke;
            id_menuCopyright.ForeColor = System.Drawing.Color.WhiteSmoke;
            id_menuLightMode.ForeColor = System.Drawing.Color.WhiteSmoke;
            id_menuDarkMode.ForeColor = System.Drawing.Color.WhiteSmoke;
            id_menuQuit.ForeColor = System.Drawing.Color.WhiteSmoke;


            // All other properties
            ModeUpdated();
        }

        /// <summary>
        /// Changes the System.Drawing.Color layout of the application to a light mode style
        /// </summary>
        /// <remarks>
        /// This is active by default.
        /// </remarks>
        public void SelectLightMode()
        {
            Programming.Logging.Instance.Log("Selected Light Mode");
            m_selectedDarkMode = false;

            // Main Window 
            BackColor = System.Drawing.Color.White;
            ForeColor = System.Drawing.Color.Black;
            id_panelMain.BackColor = System.Drawing.Color.White;
            id_panelMain.ForeColor = System.Drawing.Color.Black;

            // Menu Strip
            menuStrip1.BackColor = System.Drawing.Color.Gainsboro;
            menuStrip1.ForeColor = System.Drawing.Color.Black;
            id_menuFile.BackColor = System.Drawing.Color.Gainsboro;
            id_menuEdit.BackColor = System.Drawing.Color.Gainsboro;
            id_menuView.BackColor = System.Drawing.Color.Gainsboro;
            id_menuHelp.BackColor = System.Drawing.Color.Gainsboro;
            id_menuAbout.BackColor = System.Drawing.Color.Gainsboro;
            id_menuAuthor.BackColor = System.Drawing.Color.Gainsboro;
            id_menuReferences.BackColor = System.Drawing.Color.Gainsboro;
            id_menuCopyright.BackColor = System.Drawing.Color.Gainsboro;
            id_menuLightMode.BackColor = System.Drawing.Color.Gainsboro;
            id_menuDarkMode.BackColor = System.Drawing.Color.Gainsboro;
            id_menuQuit.BackColor = System.Drawing.Color.Gainsboro;
            id_menuFile.ForeColor = System.Drawing.Color.Black;
            id_menuEdit.ForeColor = System.Drawing.Color.Black;
            id_menuView.ForeColor = System.Drawing.Color.Black;
            id_menuHelp.ForeColor = System.Drawing.Color.Black;
            id_menuAbout.ForeColor = System.Drawing.Color.Black;
            id_menuAuthor.ForeColor = System.Drawing.Color.Black;
            id_menuReferences.ForeColor = System.Drawing.Color.Black;
            id_menuCopyright.ForeColor = System.Drawing.Color.Black;
            id_menuLightMode.ForeColor = System.Drawing.Color.Black;
            id_menuDarkMode.ForeColor = System.Drawing.Color.Black;
            id_menuQuit.ForeColor = System.Drawing.Color.Black;

            ModeUpdated();
        }

        /// <summary>
        /// If lightmode or darkmode is selected this forces a update for buttons, combo boxes and textboxes. 
        /// </summary>
        /// <remarks>
        /// Because certain objects had their own border or carried their own process I created childs of those classes. 
        /// These child classes would be called Colored 
        /// </remarks>
        public void ModeUpdated()
        {


            Properties.Settings.Default.Darkmode = m_selectedDarkMode;
            Properties.Settings.Default.Save();

            // Converts all Combo Boxes to their respective modes.
            id_comboboxLeftKeyValue.ModeChange(m_selectedDarkMode);
            id_comboboxLeftKeyLayer.ModeChange(m_selectedDarkMode);
            id_comboboxLeftKeyComPort.ModeChange(m_selectedDarkMode);
            id_comboboxRightKeyLayer.ModeChange(m_selectedDarkMode);
            id_comboboxRightKeyComPort.ModeChange(m_selectedDarkMode);
            id_comboboxRightKeyValue.ModeChange(m_selectedDarkMode);
            id_comboboxLeftLedComPort.ModeChange(m_selectedDarkMode);
            id_comboboxRightLedComPort.ModeChange(m_selectedDarkMode);
            // Non Keyboard Buttons
            id_buttonLeftKeyConnectComPort.ModeChange(m_selectedDarkMode);
            id_buttonRightKeyConnectComPort.ModeChange(m_selectedDarkMode);
            id_buttonRightUpdateKey.ModeChange(m_selectedDarkMode);
            id_buttonLeftUpdateKey.ModeChange(m_selectedDarkMode);
            id_buttonLeftUpdateLed.ModeChange(m_selectedDarkMode);
            id_buttonLeftLedConnectComPort.ModeChange(m_selectedDarkMode);
            id_buttonRightUpdateLed.ModeChange(m_selectedDarkMode);
            id_buttonRightLedConnectComPort.ModeChange(m_selectedDarkMode);

            // Left hand side
            // Keyboard Keys
            {
                // Column 1
                id_buttonLeftR1C1.ModeChange(m_selectedDarkMode, "Wide");
                id_buttonLeftR2C1.ModeChange(m_selectedDarkMode, "Wide");
                id_buttonLeftR3C1.ModeChange(m_selectedDarkMode, "Wide");
                id_buttonLeftR4C1.ModeChange(m_selectedDarkMode, "Wide");
                id_buttonLeftR5C1.ModeChange(m_selectedDarkMode, "Wide");
                id_buttonLeftR6C1.ModeChange(m_selectedDarkMode, "Wide");
                // Column 2
                id_buttonLeftR1C2.ModeChange(m_selectedDarkMode, "Single");
                id_buttonLeftR2C2.ModeChange(m_selectedDarkMode, "Single");
                id_buttonLeftR3C2.ModeChange(m_selectedDarkMode, "Single");
                id_buttonLeftR4C2.ModeChange(m_selectedDarkMode, "Single");
                id_buttonLeftR5C2.ModeChange(m_selectedDarkMode, "Single");
                id_buttonLeftR6C2.ModeChange(m_selectedDarkMode, "Single");
                // Column 3
                id_buttonLeftR1C3.ModeChange(m_selectedDarkMode, "Single");
                id_buttonLeftR2C3.ModeChange(m_selectedDarkMode, "Single");
                id_buttonLeftR3C3.ModeChange(m_selectedDarkMode, "Single");
                id_buttonLeftR4C3.ModeChange(m_selectedDarkMode, "Single");
                id_buttonLeftR5C3.ModeChange(m_selectedDarkMode, "Single");
                id_buttonLeftR6C3.ModeChange(m_selectedDarkMode, "Single");
                // Column 4
                id_buttonLeftR1C4.ModeChange(m_selectedDarkMode, "Single");
                id_buttonLeftR2C4.ModeChange(m_selectedDarkMode, "Single");
                id_buttonLeftR3C4.ModeChange(m_selectedDarkMode, "Single");
                id_buttonLeftR4C4.ModeChange(m_selectedDarkMode, "Single");
                id_buttonLeftR5C4.ModeChange(m_selectedDarkMode, "Single");
                id_buttonLeftR6C4.ModeChange(m_selectedDarkMode, "Single");
                // Column 5
                id_buttonLeftR1C5.ModeChange(m_selectedDarkMode, "Single");
                id_buttonLeftR2C5.ModeChange(m_selectedDarkMode, "Single");
                id_buttonLeftR3C5.ModeChange(m_selectedDarkMode, "Single");
                id_buttonLeftR4C5.ModeChange(m_selectedDarkMode, "Single");
                id_buttonLeftR5C5.ModeChange(m_selectedDarkMode, "Single");
                id_buttonLeftR6C5.ModeChange(m_selectedDarkMode, "Single");
                // Column 6
                id_buttonLeftR1C6.ModeChange(m_selectedDarkMode, "Single");
                id_buttonLeftR2C6.ModeChange(m_selectedDarkMode, "Single");
                id_buttonLeftR3C6.ModeChange(m_selectedDarkMode, "Single");
                id_buttonLeftR4C6.ModeChange(m_selectedDarkMode, "Single");
                id_buttonLeftR5C6.ModeChange(m_selectedDarkMode, "Single");
                id_buttonLeftR6C6.ModeChange(m_selectedDarkMode, "Tall");
                // Column 7
                id_buttonLeftR1C7.ModeChange(m_selectedDarkMode, "Single");
                id_buttonLeftR2C7.ModeChange(m_selectedDarkMode, "Single");
                id_buttonLeftR3C7.ModeChange(m_selectedDarkMode, "Single");
                id_buttonLeftR4C7.ModeChange(m_selectedDarkMode, "Single");
                id_buttonLeftR5C7.ModeChange(m_selectedDarkMode, "Single");
                id_buttonLeftR6C7.ModeChange(m_selectedDarkMode, "Tall");
                // Column 8
                id_buttonLeftR3C8.ModeChange(m_selectedDarkMode, "Single");
                id_buttonLeftR4C8.ModeChange(m_selectedDarkMode, "Single");
                id_buttonLeftR5C8.ModeChange(m_selectedDarkMode, "Single");
                id_buttonLeftR6C8.ModeChange(m_selectedDarkMode, "Single");
            }
            //LEDS
            {
                // Column 1
                id_buttonLeftLedR1C1.ModeChange(m_selectedDarkMode, "WideLED");
                id_buttonLeftLedR2C1.ModeChange(m_selectedDarkMode, "WideLED");
                id_buttonLeftLedR3C1.ModeChange(m_selectedDarkMode, "WideLED");
                id_buttonLeftLedR4C1.ModeChange(m_selectedDarkMode, "WideLED");
                id_buttonLeftLedR5C1.ModeChange(m_selectedDarkMode, "WideLED");
                id_buttonLeftLedR6C1.ModeChange(m_selectedDarkMode, "WideLED");
                // Column 2
                id_buttonLeftLedR1C2.ModeChange(m_selectedDarkMode, "SingleLED");
                id_buttonLeftLedR2C2.ModeChange(m_selectedDarkMode, "SingleLED");
                id_buttonLeftLedR3C2.ModeChange(m_selectedDarkMode, "SingleLED");
                id_buttonLeftLedR4C2.ModeChange(m_selectedDarkMode, "SingleLED");
                id_buttonLeftLedR5C2.ModeChange(m_selectedDarkMode, "SingleLED");
                id_buttonLeftLedR6C2.ModeChange(m_selectedDarkMode, "SingleLED");
                // Column 3
                id_buttonLeftLedR1C3.ModeChange(m_selectedDarkMode, "SingleLED");
                id_buttonLeftLedR2C3.ModeChange(m_selectedDarkMode, "SingleLED");
                id_buttonLeftLedR3C3.ModeChange(m_selectedDarkMode, "SingleLED");
                id_buttonLeftLedR4C3.ModeChange(m_selectedDarkMode, "SingleLED");
                id_buttonLeftLedR5C3.ModeChange(m_selectedDarkMode, "SingleLED");
                id_buttonLeftLedR6C3.ModeChange(m_selectedDarkMode, "SingleLED");
                // Column 4
                id_buttonLeftLedR1C4.ModeChange(m_selectedDarkMode, "SingleLED");
                id_buttonLeftLedR2C4.ModeChange(m_selectedDarkMode, "SingleLED");
                id_buttonLeftLedR3C4.ModeChange(m_selectedDarkMode, "SingleLED");
                id_buttonLeftLedR4C4.ModeChange(m_selectedDarkMode, "SingleLED");
                id_buttonLeftLedR5C4.ModeChange(m_selectedDarkMode, "SingleLED");
                id_buttonLeftLedR6C4.ModeChange(m_selectedDarkMode, "SingleLED");
                // Column 5
                id_buttonLeftLedR1C5.ModeChange(m_selectedDarkMode, "SingleLED");
                id_buttonLeftLedR2C5.ModeChange(m_selectedDarkMode, "SingleLED");
                id_buttonLeftLedR3C5.ModeChange(m_selectedDarkMode, "SingleLED");
                id_buttonLeftLedR4C5.ModeChange(m_selectedDarkMode, "SingleLED");
                id_buttonLeftLedR5C5.ModeChange(m_selectedDarkMode, "SingleLED");
                id_buttonLeftLedR6C5.ModeChange(m_selectedDarkMode, "SingleLED");
                // Column 6
                id_buttonLeftLedR1C6.ModeChange(m_selectedDarkMode, "SingleLED");
                id_buttonLeftLedR2C6.ModeChange(m_selectedDarkMode, "SingleLED");
                id_buttonLeftLedR3C6.ModeChange(m_selectedDarkMode, "SingleLED");
                id_buttonLeftLedR4C6.ModeChange(m_selectedDarkMode, "SingleLED");
                id_buttonLeftLedR5C6.ModeChange(m_selectedDarkMode, "SingleLED");
                id_buttonLeftLedR6C6.ModeChange(m_selectedDarkMode, "TallLED");
                // Column 7
                id_buttonLeftLedR1C7.ModeChange(m_selectedDarkMode, "SingleLED");
                id_buttonLeftLedR2C7.ModeChange(m_selectedDarkMode, "SingleLED");
                id_buttonLeftLedR3C7.ModeChange(m_selectedDarkMode, "SingleLED");
                id_buttonLeftLedR4C7.ModeChange(m_selectedDarkMode, "SingleLED");
                id_buttonLeftLedR5C7.ModeChange(m_selectedDarkMode, "SingleLED");
                id_buttonLeftLedR6C7.ModeChange(m_selectedDarkMode, "TallLED");
                // Column 8
                id_buttonLeftLedR3C8.ModeChange(m_selectedDarkMode, "SingleLED");
                id_buttonLeftLedR4C8.ModeChange(m_selectedDarkMode, "SingleLED");
                id_buttonLeftLedR5C8.ModeChange(m_selectedDarkMode, "SingleLED");
                id_buttonLeftLedR6C8.ModeChange(m_selectedDarkMode, "SingleLED");
            }

            // Right Hand side
            // Keyboard Keys
            {
                // Column 1
                id_buttonRightR1C1.ModeChange(m_selectedDarkMode, "Wide");
                id_buttonRightR2C1.ModeChange(m_selectedDarkMode, "Wide");
                id_buttonRightR3C1.ModeChange(m_selectedDarkMode, "Wide");
                id_buttonRightR4C1.ModeChange(m_selectedDarkMode, "Wide");
                id_buttonRightR5C1.ModeChange(m_selectedDarkMode, "Wide");
                id_buttonRightR6C1.ModeChange(m_selectedDarkMode, "Wide");
                // Column 2
                id_buttonRightR1C2.ModeChange(m_selectedDarkMode, "Single");
                id_buttonRightR2C2.ModeChange(m_selectedDarkMode, "Single");
                id_buttonRightR3C2.ModeChange(m_selectedDarkMode, "Single");
                id_buttonRightR4C2.ModeChange(m_selectedDarkMode, "Single");
                id_buttonRightR5C2.ModeChange(m_selectedDarkMode, "Single");
                id_buttonRightR6C2.ModeChange(m_selectedDarkMode, "Single");
                // Column 3
                id_buttonRightR1C3.ModeChange(m_selectedDarkMode, "Single");
                id_buttonRightR2C3.ModeChange(m_selectedDarkMode, "Single");
                id_buttonRightR3C3.ModeChange(m_selectedDarkMode, "Single");
                id_buttonRightR4C3.ModeChange(m_selectedDarkMode, "Single");
                id_buttonRightR5C3.ModeChange(m_selectedDarkMode, "Single");
                id_buttonRightR6C3.ModeChange(m_selectedDarkMode, "Single");
                // Column 4
                id_buttonRightR1C4.ModeChange(m_selectedDarkMode, "Single");
                id_buttonRightR2C4.ModeChange(m_selectedDarkMode, "Single");
                id_buttonRightR3C4.ModeChange(m_selectedDarkMode, "Single");
                id_buttonRightR4C4.ModeChange(m_selectedDarkMode, "Single");
                id_buttonRightR5C4.ModeChange(m_selectedDarkMode, "Single");
                id_buttonRightR6C4.ModeChange(m_selectedDarkMode, "Single");
                // Column 5
                id_buttonRightR1C5.ModeChange(m_selectedDarkMode, "Single");
                id_buttonRightR2C5.ModeChange(m_selectedDarkMode, "Single");
                id_buttonRightR3C5.ModeChange(m_selectedDarkMode, "Single");
                id_buttonRightR4C5.ModeChange(m_selectedDarkMode, "Single");
                id_buttonRightR5C5.ModeChange(m_selectedDarkMode, "Single");
                id_buttonRightR6C5.ModeChange(m_selectedDarkMode, "Single");
                // Column 6
                id_buttonRightR1C6.ModeChange(m_selectedDarkMode, "Single");
                id_buttonRightR2C6.ModeChange(m_selectedDarkMode, "Single");
                id_buttonRightR3C6.ModeChange(m_selectedDarkMode, "Single");
                id_buttonRightR4C6.ModeChange(m_selectedDarkMode, "Single");
                id_buttonRightR5C6.ModeChange(m_selectedDarkMode, "Single");
                id_buttonRightR6C6.ModeChange(m_selectedDarkMode, "Tall");
                // Column 7
                id_buttonRightR1C7.ModeChange(m_selectedDarkMode, "Single");
                id_buttonRightR2C7.ModeChange(m_selectedDarkMode, "Single");
                id_buttonRightR3C7.ModeChange(m_selectedDarkMode, "Single");
                id_buttonRightR4C7.ModeChange(m_selectedDarkMode, "Single");
                id_buttonRightR5C7.ModeChange(m_selectedDarkMode, "Single");
                id_buttonRightR6C7.ModeChange(m_selectedDarkMode, "Tall");
                // Column 8
                id_buttonRightR3C8.ModeChange(m_selectedDarkMode, "Single");
                id_buttonRightR4C8.ModeChange(m_selectedDarkMode, "Single");
                id_buttonRightR5C8.ModeChange(m_selectedDarkMode, "Single");
                id_buttonRightR6C8.ModeChange(m_selectedDarkMode, "Single");
            }
            //LEDS
            {
                // Column 1
                id_buttonRightLedR1C1.ModeChange(m_selectedDarkMode, "WideLED");
                id_buttonRightLedR2C1.ModeChange(m_selectedDarkMode, "WideLED");
                id_buttonRightLedR3C1.ModeChange(m_selectedDarkMode, "WideLED");
                id_buttonRightLedR4C1.ModeChange(m_selectedDarkMode, "WideLED");
                id_buttonRightLedR5C1.ModeChange(m_selectedDarkMode, "WideLED");
                id_buttonRightLedR6C1.ModeChange(m_selectedDarkMode, "WideLED");
                // Column 2
                id_buttonRightLedR1C2.ModeChange(m_selectedDarkMode, "SingleLED");
                id_buttonRightLedR2C2.ModeChange(m_selectedDarkMode, "SingleLED");
                id_buttonRightLedR3C2.ModeChange(m_selectedDarkMode, "SingleLED");
                id_buttonRightLedR4C2.ModeChange(m_selectedDarkMode, "SingleLED");
                id_buttonRightLedR5C2.ModeChange(m_selectedDarkMode, "SingleLED");
                id_buttonRightLedR6C2.ModeChange(m_selectedDarkMode, "SingleLED");
                // Column 3
                id_buttonRightLedR1C3.ModeChange(m_selectedDarkMode, "SingleLED");
                id_buttonRightLedR2C3.ModeChange(m_selectedDarkMode, "SingleLED");
                id_buttonRightLedR3C3.ModeChange(m_selectedDarkMode, "SingleLED");
                id_buttonRightLedR4C3.ModeChange(m_selectedDarkMode, "SingleLED");
                id_buttonRightLedR5C3.ModeChange(m_selectedDarkMode, "SingleLED");
                id_buttonRightLedR6C3.ModeChange(m_selectedDarkMode, "SingleLED");
                // Column 4
                id_buttonRightLedR1C4.ModeChange(m_selectedDarkMode, "SingleLED");
                id_buttonRightLedR2C4.ModeChange(m_selectedDarkMode, "SingleLED");
                id_buttonRightLedR3C4.ModeChange(m_selectedDarkMode, "SingleLED");
                id_buttonRightLedR4C4.ModeChange(m_selectedDarkMode, "SingleLED");
                id_buttonRightLedR5C4.ModeChange(m_selectedDarkMode, "SingleLED");
                id_buttonRightLedR6C4.ModeChange(m_selectedDarkMode, "SingleLED");
                // Column 5
                id_buttonRightLedR1C5.ModeChange(m_selectedDarkMode, "SingleLED");
                id_buttonRightLedR2C5.ModeChange(m_selectedDarkMode, "SingleLED");
                id_buttonRightLedR3C5.ModeChange(m_selectedDarkMode, "SingleLED");
                id_buttonRightLedR4C5.ModeChange(m_selectedDarkMode, "SingleLED");
                id_buttonRightLedR5C5.ModeChange(m_selectedDarkMode, "SingleLED");
                id_buttonRightLedR6C5.ModeChange(m_selectedDarkMode, "SingleLED");
                // Column 6
                id_buttonRightLedR1C6.ModeChange(m_selectedDarkMode, "SingleLED");
                id_buttonRightLedR2C6.ModeChange(m_selectedDarkMode, "SingleLED");
                id_buttonRightLedR3C6.ModeChange(m_selectedDarkMode, "SingleLED");
                id_buttonRightLedR4C6.ModeChange(m_selectedDarkMode, "SingleLED");
                id_buttonRightLedR5C6.ModeChange(m_selectedDarkMode, "SingleLED");
                id_buttonRightLedR6C6.ModeChange(m_selectedDarkMode, "TallLED");
                // Column 7
                id_buttonRightLedR1C7.ModeChange(m_selectedDarkMode, "SingleLED");
                id_buttonRightLedR2C7.ModeChange(m_selectedDarkMode, "SingleLED");
                id_buttonRightLedR3C7.ModeChange(m_selectedDarkMode, "SingleLED");
                id_buttonRightLedR4C7.ModeChange(m_selectedDarkMode, "SingleLED");
                id_buttonRightLedR5C7.ModeChange(m_selectedDarkMode, "SingleLED");
                id_buttonRightLedR6C7.ModeChange(m_selectedDarkMode, "TallLED");
                // Column 8
                id_buttonRightLedR3C8.ModeChange(m_selectedDarkMode, "SingleLED");
                id_buttonRightLedR4C8.ModeChange(m_selectedDarkMode, "SingleLED");
                id_buttonRightLedR5C8.ModeChange(m_selectedDarkMode, "SingleLED");
                id_buttonRightLedR6C8.ModeChange(m_selectedDarkMode, "SingleLED");
            }
        }

        /// <summary>
        /// This function happens automatically when the program is loaded.
        /// </summary>
        /// <param name="sender">The System.Object that called this function </param> 
        /// <param name="reason">What event caused the System.Object to call this function </param>
        private void ErgoboardLoad(System.Object sender, System.EventArgs reason) => FormClosing += Ergoboard_Closing;

        /// <summary>
        /// Quits the program using the menu button.
        /// </summary>
        /// <param name="sender">The System.Object that called this function </param> 
        /// <param name="reason">What event caused the System.Object to call this function </param>
        private void Id_menuQuit_Click(System.Object sender, System.EventArgs reason) => Close();

        /// <summary>
        /// Acts as a deafult destructor. Closes any open threads and ends all the singletons.
        /// </summary>
        /// <param name="sender">The System.Object that called this function </param> 
        /// <param name="reason">What event caused the System.Object to call this function </param>
        private void Ergoboard_Closing(System.Object sender, System.Windows.Forms.FormClosingEventArgs reason)
        {
            Programming.Logging.Instance.Log("The program will be closing in 1 second");
            System.Threading.Thread.Sleep(1000);
            Programming.Logging.Instance.End();
            Programming.SystemMonitor.Instance.End();
            this.m_processing.Close();
        }

        /// <summary>
        /// When the user of the application chooses to enter dark mode. 
        /// </summary>
        /// <param name="sender">The System.Object that called this function </param> 
        /// <param name="reason">What event caused the System.Object to call this function </param>
        private void Id_menuDarkMode_Click(System.Object sender, System.EventArgs reason)
        {
            SelectDarkMode();
        }

        /// <summary>
        /// When the user of the application chooses to enter light mode. 
        /// </summary>
        /// <param name="sender">The System.Object that called this function </param> 
        /// <param name="reason">What event caused the System.Object to call this function </param>
        private void Id_menuLightMode_Click(System.Object sender, System.EventArgs reason)
        {
            SelectLightMode();
        }

        // Keyboard Keys
        private void Id_buttonLeftR1C1_Click(System.Object sender, System.EventArgs reason)
        {
            if (m_leftSelectedKeyButton != null)
            {
                m_leftSelectedKeyButton.UnSelected();
            }
            m_leftSelectedKeyButton = id_buttonLeftR1C1;
            m_leftSelectedKeyButton.Selected();
            id_textboxLeftKeyValue.Text = m_leftSelectedKeyButton.MakeKeyNameValue();

            Programming.Logging.Instance.Log($"Left keyboard \" {id_textboxLeftKeyValue.Text} \" key was selected by \" {sender.ToString()} \" selected by \" {reason.ToString()} \" ",
                                  "Information");
        }
        private void Id_buttonLeftR1C2_Click(System.Object sender, System.EventArgs reason)
        {
            if (m_leftSelectedKeyButton != null)
            {
                m_leftSelectedKeyButton.UnSelected();
            }
            m_leftSelectedKeyButton = id_buttonLeftR1C2;
            m_leftSelectedKeyButton.Selected();
            id_textboxLeftKeyValue.Text = m_leftSelectedKeyButton.MakeKeyNameValue();

            Programming.Logging.Instance.Log($"Left keyboard \" {id_textboxLeftKeyValue.Text} \" key was selected by \" {sender.ToString()} \" selected by \" {reason.ToString()} \" ",
                                  "Information");
        }
        private void Id_buttonLeftR1C3_Click(System.Object sender, System.EventArgs reason)
        {
            if (m_leftSelectedKeyButton != null)
            {
                m_leftSelectedKeyButton.UnSelected();
            }
            m_leftSelectedKeyButton = id_buttonLeftR1C3;
            m_leftSelectedKeyButton.Selected();
            id_textboxLeftKeyValue.Text = m_leftSelectedKeyButton.MakeKeyNameValue();

            Programming.Logging.Instance.Log($"Left keyboard \" {id_textboxLeftKeyValue.Text} \" key was selected by \" {sender.ToString()} \" selected by \" {reason.ToString()} \" ",
                                  "Information");
        }
        private void Id_buttonLeftR1C4_Click(System.Object sender, System.EventArgs reason)
        {
            if (m_leftSelectedKeyButton != null)
            {
                m_leftSelectedKeyButton.UnSelected();
            }
            m_leftSelectedKeyButton = id_buttonLeftR1C4;
            m_leftSelectedKeyButton.Selected();
            id_textboxLeftKeyValue.Text = m_leftSelectedKeyButton.MakeKeyNameValue();

            Programming.Logging.Instance.Log($"Left keyboard \" {id_textboxLeftKeyValue.Text} \" key was selected by \" {sender.ToString()} \" selected by \" {reason.ToString()} \" ",
                                  "Information");
        }
        private void Id_buttonLeftR1C5_Click(System.Object sender, System.EventArgs reason)
        {
            if (m_leftSelectedKeyButton != null)
            {
                m_leftSelectedKeyButton.UnSelected();
            }
            m_leftSelectedKeyButton = id_buttonLeftR1C5;
            m_leftSelectedKeyButton.Selected();
            id_textboxLeftKeyValue.Text = m_leftSelectedKeyButton.MakeKeyNameValue();

            Programming.Logging.Instance.Log($"Left keyboard \" {id_textboxLeftKeyValue.Text} \" key was selected by \" {sender.ToString()} \" selected by \" {reason.ToString()} \" ",
                                  "Information");
        }
        private void Id_buttonLeftR1C6_Click(System.Object sender, System.EventArgs reason)
        {
            if (m_leftSelectedKeyButton != null)
            {
                m_leftSelectedKeyButton.UnSelected();
            }
            m_leftSelectedKeyButton = id_buttonLeftR1C6;
            m_leftSelectedKeyButton.Selected();
            id_textboxLeftKeyValue.Text = m_leftSelectedKeyButton.MakeKeyNameValue();

            Programming.Logging.Instance.Log($"Left keyboard \" {id_textboxLeftKeyValue.Text} \" key was selected by \" {sender.ToString()} \" selected by \" {reason.ToString()} \" ",
                                  "Information");
        }
        private void Id_buttonLeftR1C7_Click(System.Object sender, System.EventArgs reason)
        {
            if (m_leftSelectedKeyButton != null)
            {
                m_leftSelectedKeyButton.UnSelected();
            }
            m_leftSelectedKeyButton = id_buttonLeftR1C7;
            m_leftSelectedKeyButton.Selected();
            id_textboxLeftKeyValue.Text = m_leftSelectedKeyButton.MakeKeyNameValue();

            Programming.Logging.Instance.Log($"Left keyboard \" {id_textboxLeftKeyValue.Text} \" key was selected by \" {sender.ToString()} \" selected by \" {reason.ToString()} \" ",
                                  "Information");
        }
        private void Id_buttonLeftR2C1_Click(System.Object sender, System.EventArgs reason)
        {
            if (m_leftSelectedKeyButton != null)
            {
                m_leftSelectedKeyButton.UnSelected();
            }
            m_leftSelectedKeyButton = id_buttonLeftR2C1;
            m_leftSelectedKeyButton.Selected();
            id_textboxLeftKeyValue.Text = m_leftSelectedKeyButton.MakeKeyNameValue();

            Programming.Logging.Instance.Log($"Left keyboard \" {id_textboxLeftKeyValue.Text} \" key was selected by \" {sender.ToString()} \" selected by \" {reason.ToString()} \" ",
                                  "Information");
        }
        private void Id_buttonLeftR2C2_Click(System.Object sender, System.EventArgs reason)
        {
            if (m_leftSelectedKeyButton != null)
            {
                m_leftSelectedKeyButton.UnSelected();
            }
            m_leftSelectedKeyButton = id_buttonLeftR2C2;
            m_leftSelectedKeyButton.Selected();
            id_textboxLeftKeyValue.Text = m_leftSelectedKeyButton.MakeKeyNameValue();

            Programming.Logging.Instance.Log($"Left keyboard \" {id_textboxLeftKeyValue.Text} \" key was selected by \" {sender.ToString()} \" selected by \" {reason.ToString()} \" ",
                                  "Information");
        }
        private void Id_buttonLeftR2C3_Click(System.Object sender, System.EventArgs reason)
        {
            if (m_leftSelectedKeyButton != null)
            {
                m_leftSelectedKeyButton.UnSelected();
            }
            m_leftSelectedKeyButton = id_buttonLeftR2C3;
            m_leftSelectedKeyButton.Selected();
            id_textboxLeftKeyValue.Text = m_leftSelectedKeyButton.MakeKeyNameValue();

            Programming.Logging.Instance.Log($"Left keyboard \" {id_textboxLeftKeyValue.Text} \" key was selected by \" {sender.ToString()} \" selected by \" {reason.ToString()} \" ",
                                  "Information");
        }
        private void Id_buttonLeftR2C4_Click(System.Object sender, System.EventArgs reason)
        {
            if (m_leftSelectedKeyButton != null)
            {
                m_leftSelectedKeyButton.UnSelected();
            }
            m_leftSelectedKeyButton = id_buttonLeftR2C4;
            m_leftSelectedKeyButton.Selected();
            id_textboxLeftKeyValue.Text = m_leftSelectedKeyButton.MakeKeyNameValue();

            Programming.Logging.Instance.Log($"Left keyboard \" {id_textboxLeftKeyValue.Text} \" key was selected by \" {sender.ToString()} \" selected by \" {reason.ToString()} \" ",
                                  "Information");
        }
        private void Id_buttonLeftR2C5_Click(System.Object sender, System.EventArgs reason)
        {
            if (m_leftSelectedKeyButton != null)
            {
                m_leftSelectedKeyButton.UnSelected();
            }
            m_leftSelectedKeyButton = id_buttonLeftR2C5;
            m_leftSelectedKeyButton.Selected();
            id_textboxLeftKeyValue.Text = m_leftSelectedKeyButton.MakeKeyNameValue();

            Programming.Logging.Instance.Log($"Left keyboard \" {id_textboxLeftKeyValue.Text} \" key was selected by \" {sender.ToString()} \" selected by \" {reason.ToString()} \" ",
                                  "Information");
        }
        private void Id_buttonLeftR2C6_Click(System.Object sender, System.EventArgs reason)
        {
            if (m_leftSelectedKeyButton != null)
            {
                m_leftSelectedKeyButton.UnSelected();
            }
            m_leftSelectedKeyButton = id_buttonLeftR2C6;
            m_leftSelectedKeyButton.Selected();
            id_textboxLeftKeyValue.Text = m_leftSelectedKeyButton.MakeKeyNameValue();

            Programming.Logging.Instance.Log($"Left keyboard \" {id_textboxLeftKeyValue.Text} \" key was selected by \" {sender.ToString()} \" selected by \" {reason.ToString()} \" ",
                                  "Information");
        }
        private void Id_buttonLeftR2C7_Click(System.Object sender, System.EventArgs reason)
        {
            if (m_leftSelectedKeyButton != null)
            {
                m_leftSelectedKeyButton.UnSelected();
            }
            m_leftSelectedKeyButton = id_buttonLeftR2C7;
            m_leftSelectedKeyButton.Selected();
            id_textboxLeftKeyValue.Text = m_leftSelectedKeyButton.MakeKeyNameValue();

            Programming.Logging.Instance.Log($"Left keyboard \" {id_textboxLeftKeyValue.Text} \" key was selected by \" {sender.ToString()} \" selected by \" {reason.ToString()} \" ",
                                  "Information");
        }
        private void Id_buttonLeftR3C1_Click(System.Object sender, System.EventArgs reason)
        {
            if (m_leftSelectedKeyButton != null)
            {
                m_leftSelectedKeyButton.UnSelected();
            }
            m_leftSelectedKeyButton = id_buttonLeftR3C1;
            m_leftSelectedKeyButton.Selected();
            id_textboxLeftKeyValue.Text = m_leftSelectedKeyButton.MakeKeyNameValue();

            Programming.Logging.Instance.Log($"Left keyboard \" {id_textboxLeftKeyValue.Text} \" key was selected by \" {sender.ToString()} \" selected by \" {reason.ToString()} \" ",
                                  "Information");

        }
        private void Id_buttonLeftR3C2_Click(System.Object sender, System.EventArgs reason)
        {
            if (m_leftSelectedKeyButton != null)
            {
                m_leftSelectedKeyButton.UnSelected();
            }
            m_leftSelectedKeyButton = id_buttonLeftR3C2;
            m_leftSelectedKeyButton.Selected();
            id_textboxLeftKeyValue.Text = m_leftSelectedKeyButton.MakeKeyNameValue();

            Programming.Logging.Instance.Log($"Left keyboard \" {id_textboxLeftKeyValue.Text} \" key was selected by \" {sender.ToString()} \" selected by \" {reason.ToString()} \" ",
                                  "Information");

        }
        private void Id_buttonLeftR3C3_Click(System.Object sender, System.EventArgs reason)
        {
            if (m_leftSelectedKeyButton != null)
            {
                m_leftSelectedKeyButton.UnSelected();
            }
            m_leftSelectedKeyButton = id_buttonLeftR3C3;
            m_leftSelectedKeyButton.Selected();
            id_textboxLeftKeyValue.Text = m_leftSelectedKeyButton.MakeKeyNameValue();

            Programming.Logging.Instance.Log($"Left keyboard \" {id_textboxLeftKeyValue.Text} \" key was selected by \" {sender.ToString()} \" selected by \" {reason.ToString()} \" ",
                                  "Information");
        }
        private void Id_buttonLeftR3C4_Click(System.Object sender, System.EventArgs reason)
        {
            if (m_leftSelectedKeyButton != null)
            {
                m_leftSelectedKeyButton.UnSelected();
            }
            m_leftSelectedKeyButton = id_buttonLeftR3C4;
            m_leftSelectedKeyButton.Selected();
            id_textboxLeftKeyValue.Text = m_leftSelectedKeyButton.MakeKeyNameValue();

            Programming.Logging.Instance.Log($"Left keyboard \" {id_textboxLeftKeyValue.Text} \" key was selected by \" {sender.ToString()} \" selected by \" {reason.ToString()} \" ",
                                  "Information");
        }
        private void Id_buttonLeftR3C5_Click(System.Object sender, System.EventArgs reason)
        {
            if (m_leftSelectedKeyButton != null)
            {
                m_leftSelectedKeyButton.UnSelected();
            }
            m_leftSelectedKeyButton = id_buttonLeftR3C5;
            m_leftSelectedKeyButton.Selected();
            id_textboxLeftKeyValue.Text = m_leftSelectedKeyButton.MakeKeyNameValue();

            Programming.Logging.Instance.Log($"Left keyboard \" {id_textboxLeftKeyValue.Text} \" key was selected by \" {sender.ToString()} \" selected by \" {reason.ToString()} \" ",
                                  "Information");
        }
        private void Id_buttonLeftR3C6_Click(System.Object sender, System.EventArgs reason)
        {
            if (m_leftSelectedKeyButton != null)
            {
                m_leftSelectedKeyButton.UnSelected();
            }
            m_leftSelectedKeyButton = id_buttonLeftR3C6;
            m_leftSelectedKeyButton.Selected();
            id_textboxLeftKeyValue.Text = m_leftSelectedKeyButton.MakeKeyNameValue();

            Programming.Logging.Instance.Log($"Left keyboard \" {id_textboxLeftKeyValue.Text} \" key was selected by \" {sender.ToString()} \" selected by \" {reason.ToString()} \" ",
                                  "Information");
        }
        private void Id_buttonLeftR3C7_Click(System.Object sender, System.EventArgs reason)
        {
            if (m_leftSelectedKeyButton != null)
            {
                m_leftSelectedKeyButton.UnSelected();
            }
            m_leftSelectedKeyButton = id_buttonLeftR3C7;
            m_leftSelectedKeyButton.Selected();
            id_textboxLeftKeyValue.Text = m_leftSelectedKeyButton.MakeKeyNameValue();

            Programming.Logging.Instance.Log($"Left keyboard \" {id_textboxLeftKeyValue.Text} \" key was selected by \" {sender.ToString()} \" selected by \" {reason.ToString()} \" ",
                                  "Information");
        }
        private void Id_buttonLeftR3C8_Click(System.Object sender, System.EventArgs reason)
        {
            if (m_leftSelectedKeyButton != null)
            {
                m_leftSelectedKeyButton.UnSelected();
            }
            m_leftSelectedKeyButton = id_buttonLeftR3C8;
            m_leftSelectedKeyButton.Selected();
            id_textboxLeftKeyValue.Text = m_leftSelectedKeyButton.MakeKeyNameValue();

            Programming.Logging.Instance.Log($"Left keyboard \" {id_textboxLeftKeyValue.Text} \" key was selected by \" {sender.ToString()} \" selected by \" {reason.ToString()} \" ",
                                  "Information");
        }
        private void Id_buttonLeftR4C1_Click(System.Object sender, System.EventArgs reason)
        {
            if (m_leftSelectedKeyButton != null)
            {
                m_leftSelectedKeyButton.UnSelected();
            }
            m_leftSelectedKeyButton = id_buttonLeftR4C1;
            m_leftSelectedKeyButton.Selected();
            id_textboxLeftKeyValue.Text = m_leftSelectedKeyButton.MakeKeyNameValue();

            Programming.Logging.Instance.Log($"Left keyboard \" {id_textboxLeftKeyValue.Text} \" key was selected by \" {sender.ToString()} \" selected by \" {reason.ToString()} \" ",
                                  "Information");
        }
        private void Id_buttonLeftR4C2_Click(System.Object sender, System.EventArgs reason)
        {
            if (m_leftSelectedKeyButton != null)
            {
                m_leftSelectedKeyButton.UnSelected();
            }
            m_leftSelectedKeyButton = id_buttonLeftR4C2;
            m_leftSelectedKeyButton.Selected();
            id_textboxLeftKeyValue.Text = m_leftSelectedKeyButton.MakeKeyNameValue();

            Programming.Logging.Instance.Log($"Left keyboard \" {id_textboxLeftKeyValue.Text} \" key was selected by \" {sender.ToString()} \" selected by \" {reason.ToString()} \" ",
                                  "Information");
        }
        private void Id_buttonLeftR4C3_Click(System.Object sender, System.EventArgs reason)
        {
            if (m_leftSelectedKeyButton != null)
            {
                m_leftSelectedKeyButton.UnSelected();
            }
            m_leftSelectedKeyButton = id_buttonLeftR4C3;
            m_leftSelectedKeyButton.Selected();
            id_textboxLeftKeyValue.Text = m_leftSelectedKeyButton.MakeKeyNameValue();

            Programming.Logging.Instance.Log($"Left keyboard \" {id_textboxLeftKeyValue.Text} \" key was selected by \" {sender.ToString()} \" selected by \" {reason.ToString()} \" ",
                                  "Information");
        }
        private void Id_buttonLeftR4C4_Click(System.Object sender, System.EventArgs reason)
        {
            if (m_leftSelectedKeyButton != null)
            {
                m_leftSelectedKeyButton.UnSelected();
            }
            m_leftSelectedKeyButton = id_buttonLeftR4C4;
            m_leftSelectedKeyButton.Selected();
            id_textboxLeftKeyValue.Text = m_leftSelectedKeyButton.MakeKeyNameValue();

            Programming.Logging.Instance.Log($"Left keyboard \" {id_textboxLeftKeyValue.Text} \" key was selected by \" {sender.ToString()} \" selected by \" {reason.ToString()} \" ",
                                  "Information");
        }
        private void Id_buttonLeftR4C5_Click(System.Object sender, System.EventArgs reason)
        {
            if (m_leftSelectedKeyButton != null)
            {
                m_leftSelectedKeyButton.UnSelected();
            }
            m_leftSelectedKeyButton = id_buttonLeftR4C5;
            m_leftSelectedKeyButton.Selected();
            id_textboxLeftKeyValue.Text = m_leftSelectedKeyButton.MakeKeyNameValue();

            Programming.Logging.Instance.Log($"Left keyboard \" {id_textboxLeftKeyValue.Text} \" key was selected by \" {sender.ToString()} \" selected by \" {reason.ToString()} \" ",
                                  "Information");
        }
        private void Id_buttonLeftR4C6_Click(System.Object sender, System.EventArgs reason)
        {
            if (m_leftSelectedKeyButton != null)
            {
                m_leftSelectedKeyButton.UnSelected();
            }
            m_leftSelectedKeyButton = id_buttonLeftR4C6;
            m_leftSelectedKeyButton.Selected();
            id_textboxLeftKeyValue.Text = m_leftSelectedKeyButton.MakeKeyNameValue();

            Programming.Logging.Instance.Log($"Left keyboard \" {id_textboxLeftKeyValue.Text} \" key was selected by \" {sender.ToString()} \" selected by \" {reason.ToString()} \" ",
                                  "Information");
        }
        private void Id_buttonLeftR4C7_Click(System.Object sender, System.EventArgs reason)
        {
            if (m_leftSelectedKeyButton != null)
            {
                m_leftSelectedKeyButton.UnSelected();
            }
            m_leftSelectedKeyButton = id_buttonLeftR4C7;
            m_leftSelectedKeyButton.Selected();
            id_textboxLeftKeyValue.Text = m_leftSelectedKeyButton.MakeKeyNameValue();

            Programming.Logging.Instance.Log($"Left keyboard \" {id_textboxLeftKeyValue.Text} \" key was selected by \" {sender.ToString()} \" selected by \" {reason.ToString()} \" ",
                                  "Information");
        }
        private void Id_buttonLeftR4C8_Click(System.Object sender, System.EventArgs reason)
        {
            if (m_leftSelectedKeyButton != null)
            {
                m_leftSelectedKeyButton.UnSelected();
            }
            m_leftSelectedKeyButton = id_buttonLeftR4C8;
            m_leftSelectedKeyButton.Selected();
            id_textboxLeftKeyValue.Text = m_leftSelectedKeyButton.MakeKeyNameValue();

            Programming.Logging.Instance.Log($"Left keyboard \" {id_textboxLeftKeyValue.Text} \" key was selected by \" {sender.ToString()} \" selected by \" {reason.ToString()} \" ",
                                  "Information");
        }
        private void Id_buttonLeftR5C1_Click(System.Object sender, System.EventArgs reason)
        {
            if (m_leftSelectedKeyButton != null)
            {
                m_leftSelectedKeyButton.UnSelected();
            }
            m_leftSelectedKeyButton = id_buttonLeftR5C1;
            m_leftSelectedKeyButton.Selected();
            id_textboxLeftKeyValue.Text = m_leftSelectedKeyButton.MakeKeyNameValue();

            Programming.Logging.Instance.Log($"Left keyboard \" {id_textboxLeftKeyValue.Text} \" key was selected by \" {sender.ToString()} \" selected by \" {reason.ToString()} \" ",
                                  "Information");
        }
        private void Id_buttonLeftR5C2_Click(System.Object sender, System.EventArgs reason)
        {
            if (m_leftSelectedKeyButton != null)
            {
                m_leftSelectedKeyButton.UnSelected();
            }
            m_leftSelectedKeyButton = id_buttonLeftR5C2;
            m_leftSelectedKeyButton.Selected();
            id_textboxLeftKeyValue.Text = m_leftSelectedKeyButton.MakeKeyNameValue();

            Programming.Logging.Instance.Log($"Left keyboard \" {id_textboxLeftKeyValue.Text} \" key was selected by \" {sender.ToString()} \" selected by \" {reason.ToString()} \" ",
                                  "Information");
        }
        private void Id_buttonLeftR5C3_Click(System.Object sender, System.EventArgs reason)
        {
            if (m_leftSelectedKeyButton != null)
            {
                m_leftSelectedKeyButton.UnSelected();
            }
            m_leftSelectedKeyButton = id_buttonLeftR5C3;
            m_leftSelectedKeyButton.Selected();
            id_textboxLeftKeyValue.Text = m_leftSelectedKeyButton.MakeKeyNameValue();

            Programming.Logging.Instance.Log($"Left keyboard \" {id_textboxLeftKeyValue.Text} \" key was selected by \" {sender.ToString()} \" selected by \" {reason.ToString()} \" ",
                                  "Information");
        }
        private void Id_buttonLeftR5C4_Click(System.Object sender, System.EventArgs reason)
        {
            if (m_leftSelectedKeyButton != null)
            {
                m_leftSelectedKeyButton.UnSelected();
            }
            m_leftSelectedKeyButton = id_buttonLeftR5C4;
            m_leftSelectedKeyButton.Selected();
            id_textboxLeftKeyValue.Text = m_leftSelectedKeyButton.MakeKeyNameValue();

            Programming.Logging.Instance.Log($"Left keyboard \" {id_textboxLeftKeyValue.Text} \" key was selected by \" {sender.ToString()} \" selected by \" {reason.ToString()} \" ",
                                  "Information");
        }
        private void Id_buttonLeftR5C5_Click(System.Object sender, System.EventArgs reason)
        {
            if (m_leftSelectedKeyButton != null)
            {
                m_leftSelectedKeyButton.UnSelected();
            }
            m_leftSelectedKeyButton = id_buttonLeftR5C5;
            m_leftSelectedKeyButton.Selected();
            id_textboxLeftKeyValue.Text = m_leftSelectedKeyButton.MakeKeyNameValue();

            Programming.Logging.Instance.Log($"Left keyboard \" {id_textboxLeftKeyValue.Text} \" key was selected by \" {sender.ToString()} \" selected by \" {reason.ToString()} \" ",
                                  "Information");
        }
        private void Id_buttonLeftR5C6_Click(System.Object sender, System.EventArgs reason)
        {
            if (m_leftSelectedKeyButton != null)
            {
                m_leftSelectedKeyButton.UnSelected();
            }
            m_leftSelectedKeyButton = id_buttonLeftR5C6;
            m_leftSelectedKeyButton.Selected();
            id_textboxLeftKeyValue.Text = m_leftSelectedKeyButton.MakeKeyNameValue();

            Programming.Logging.Instance.Log($"Left keyboard \" {id_textboxLeftKeyValue.Text} \" key was selected by \" {sender.ToString()} \" selected by \" {reason.ToString()} \" ",
                                  "Information");
        }
        private void Id_buttonLeftR5C7_Click(System.Object sender, System.EventArgs reason)
        {
            if (m_leftSelectedKeyButton != null)
            {
                m_leftSelectedKeyButton.UnSelected();
            }
            m_leftSelectedKeyButton = id_buttonLeftR5C7;
            m_leftSelectedKeyButton.Selected();
            id_textboxLeftKeyValue.Text = m_leftSelectedKeyButton.MakeKeyNameValue();

            Programming.Logging.Instance.Log($"Left keyboard \" {id_textboxLeftKeyValue.Text} \" key was selected by \" {sender.ToString()} \" selected by \" {reason.ToString()} \" ",
                                  "Information");
        }
        private void Id_buttonLeftR5C8_Click(System.Object sender, System.EventArgs reason)
        {
            if (m_leftSelectedKeyButton != null)
            {
                m_leftSelectedKeyButton.UnSelected();
            }
            m_leftSelectedKeyButton = id_buttonLeftR5C8;
            m_leftSelectedKeyButton.Selected();
            id_textboxLeftKeyValue.Text = m_leftSelectedKeyButton.MakeKeyNameValue();

            Programming.Logging.Instance.Log($"Left keyboard \" {id_textboxLeftKeyValue.Text} \" key was selected by \" {sender.ToString()} \" selected by \" {reason.ToString()} \" ",
                                  "Information");
        }
        private void Id_buttonLeftR6C1_Click(System.Object sender, System.EventArgs reason)
        {
            if (m_leftSelectedKeyButton != null)
            {
                m_leftSelectedKeyButton.UnSelected();
            }
            m_leftSelectedKeyButton = id_buttonLeftR6C1;
            m_leftSelectedKeyButton.Selected();
            id_textboxLeftKeyValue.Text = m_leftSelectedKeyButton.MakeKeyNameValue();

            Programming.Logging.Instance.Log($"Left keyboard \" {id_textboxLeftKeyValue.Text} \" key was selected by \" {sender.ToString()} \" selected by \" {reason.ToString()} \" ",
                                  "Information");
        }
        private void Id_buttonLeftR6C2_Click(System.Object sender, System.EventArgs reason)
        {
            if (m_leftSelectedKeyButton != null)
            {
                m_leftSelectedKeyButton.UnSelected();
            }
            m_leftSelectedKeyButton = id_buttonLeftR6C2;
            m_leftSelectedKeyButton.Selected();
            id_textboxLeftKeyValue.Text = m_leftSelectedKeyButton.MakeKeyNameValue();

            Programming.Logging.Instance.Log($"Left keyboard \" {id_textboxLeftKeyValue.Text} \" key was selected by \" {sender.ToString()} \" selected by \" {reason.ToString()} \" ",
                                  "Information");
        }
        private void Id_buttonLeftR6C3_Click(System.Object sender, System.EventArgs reason)
        {
            if (m_leftSelectedKeyButton != null)
            {
                m_leftSelectedKeyButton.UnSelected();
            }
            m_leftSelectedKeyButton = id_buttonLeftR6C3;
            m_leftSelectedKeyButton.Selected();
            id_textboxLeftKeyValue.Text = m_leftSelectedKeyButton.MakeKeyNameValue();

            Programming.Logging.Instance.Log($"Left keyboard \" {id_textboxLeftKeyValue.Text} \" key was selected by \" {sender.ToString()} \" selected by \" {reason.ToString()} \" ",
                                  "Information");
        }
        private void Id_buttonLeftR6C4_Click(System.Object sender, System.EventArgs reason)
        {
            if (m_leftSelectedKeyButton != null)
            {
                m_leftSelectedKeyButton.UnSelected();
            }
            m_leftSelectedKeyButton = id_buttonLeftR6C4;
            m_leftSelectedKeyButton.Selected();
            id_textboxLeftKeyValue.Text = m_leftSelectedKeyButton.MakeKeyNameValue();

            Programming.Logging.Instance.Log($"Left keyboard \" {id_textboxLeftKeyValue.Text} \" key was selected by \" {sender.ToString()} \" selected by \" {reason.ToString()} \" ",
                                  "Information");
        }
        private void Id_buttonLeftR6C5_Click(System.Object sender, System.EventArgs reason)
        {
            if (m_leftSelectedKeyButton != null)
            {
                m_leftSelectedKeyButton.UnSelected();
            }
            m_leftSelectedKeyButton = id_buttonLeftR6C5;
            m_leftSelectedKeyButton.Selected();
            id_textboxLeftKeyValue.Text = m_leftSelectedKeyButton.MakeKeyNameValue();

            Programming.Logging.Instance.Log($"Left keyboard \" {id_textboxLeftKeyValue.Text} \" key was selected by \" {sender.ToString()} \" selected by \" {reason.ToString()} \" ",
                                  "Information");
        }
        private void Id_buttonLeftR6C6_Click(System.Object sender, System.EventArgs reason)
        {
            if (m_leftSelectedKeyButton != null)
            {
                m_leftSelectedKeyButton.UnSelected();
            }
            m_leftSelectedKeyButton = id_buttonLeftR6C6;
            m_leftSelectedKeyButton.Selected();
            id_textboxLeftKeyValue.Text = m_leftSelectedKeyButton.MakeKeyNameValue();

            Programming.Logging.Instance.Log($"Left keyboard \" {id_textboxLeftKeyValue.Text} \" key was selected by \" {sender.ToString()} \" selected by \" {reason.ToString()} \" ",
                                  "Information");
        }
        private void Id_buttonLeftR6C7_Click(System.Object sender, System.EventArgs reason)
        {
            if (m_leftSelectedKeyButton != null)
            {
                m_leftSelectedKeyButton.UnSelected();
            }
            m_leftSelectedKeyButton = id_buttonLeftR6C7;
            m_leftSelectedKeyButton.Selected();
            id_textboxLeftKeyValue.Text = m_leftSelectedKeyButton.MakeKeyNameValue();

            Programming.Logging.Instance.Log($"Left keyboard \" {id_textboxLeftKeyValue.Text} \" key was selected by \" {sender.ToString()} \" selected by \" {reason.ToString()} \" ",
                                  "Information");
        }
        private void Id_buttonLeftR6C8_Click(System.Object sender, System.EventArgs reason)
        {
            if (m_leftSelectedKeyButton != null)
            {
                m_leftSelectedKeyButton.UnSelected();
            }
            m_leftSelectedKeyButton = id_buttonLeftR6C8;
            m_leftSelectedKeyButton.Selected();
            id_textboxLeftKeyValue.Text = m_leftSelectedKeyButton.MakeKeyNameValue();

            Programming.Logging.Instance.Log($"Left keyboard \" {id_textboxLeftKeyValue.Text} \" key was selected by \" {sender.ToString()} \" selected by \" {reason.ToString()} \" ",
                                  "Information");
        }
        // Right keyboard side.
        private void Id_buttonRightR1C1_Click(System.Object sender, System.EventArgs reason)
        {
            if (m_rightSelectedKeyButton != null)
            {
                m_rightSelectedKeyButton.UnSelected();
            }
            m_rightSelectedKeyButton = id_buttonRightR1C1;
            m_rightSelectedKeyButton.Selected();
            id_textboxRightKeyValue.Text = m_rightSelectedKeyButton.MakeKeyNameValue();

            Programming.Logging.Instance.Log($"Right keyboard \" {id_textboxLeftKeyValue.Text} \" key was selected by \" {sender.ToString()} \" selected by \" {reason.ToString()} \" ",
                                  "Information");
        }
        private void Id_buttonRightR1C2_Click(System.Object sender, System.EventArgs reason)
        {
            if (m_rightSelectedKeyButton != null)
            {
                m_rightSelectedKeyButton.UnSelected();
            }
            m_rightSelectedKeyButton = id_buttonRightR1C2;
            m_rightSelectedKeyButton.Selected();
            id_textboxRightKeyValue.Text = m_rightSelectedKeyButton.MakeKeyNameValue();

            Programming.Logging.Instance.Log($"Right keyboard \" {id_textboxLeftKeyValue.Text} \" key was selected by \" {sender.ToString()} \" selected by \" {reason.ToString()} \" ",
                                  "Information");
        }
        private void Id_buttonRightR1C3_Click(System.Object sender, System.EventArgs reason)
        {
            if (m_rightSelectedKeyButton != null)
            {
                m_rightSelectedKeyButton.UnSelected();
            }
            m_rightSelectedKeyButton = id_buttonRightR1C3;
            m_rightSelectedKeyButton.Selected();
            id_textboxRightKeyValue.Text = m_rightSelectedKeyButton.MakeKeyNameValue();

            Programming.Logging.Instance.Log($"Right keyboard \" {id_textboxLeftKeyValue.Text} \" key was selected by \" {sender.ToString()} \" selected by \" {reason.ToString()} \" ",
                                  "Information");
        }
        private void Id_buttonRightR1C4_Click(System.Object sender, System.EventArgs reason)
        {
            if (m_rightSelectedKeyButton != null)
            {
                m_rightSelectedKeyButton.UnSelected();
            }
            m_rightSelectedKeyButton = id_buttonRightR1C4;
            m_rightSelectedKeyButton.Selected();
            id_textboxRightKeyValue.Text = m_rightSelectedKeyButton.MakeKeyNameValue();

            Programming.Logging.Instance.Log($"Right keyboard \" {id_textboxLeftKeyValue.Text} \" key was selected by \" {sender.ToString()} \" selected by \" {reason.ToString()} \" ",
                                  "Information");
        }
        private void Id_buttonRightR1C5_Click(System.Object sender, System.EventArgs reason)
        {
            if (m_rightSelectedKeyButton != null)
            {
                m_rightSelectedKeyButton.UnSelected();
            }
            m_rightSelectedKeyButton = id_buttonRightR1C5;
            m_rightSelectedKeyButton.Selected();
            id_textboxRightKeyValue.Text = m_rightSelectedKeyButton.MakeKeyNameValue();

            Programming.Logging.Instance.Log($"Right keyboard \" {id_textboxLeftKeyValue.Text} \" key was selected by \" {sender.ToString()} \" selected by \" {reason.ToString()} \" ",
                                  "Information");
        }
        private void Id_buttonRightR1C6_Click(System.Object sender, System.EventArgs reason)
        {
            if (m_rightSelectedKeyButton != null)
            {
                m_rightSelectedKeyButton.UnSelected();
            }
            m_rightSelectedKeyButton = id_buttonRightR1C6;
            m_rightSelectedKeyButton.Selected();
            id_textboxRightKeyValue.Text = m_rightSelectedKeyButton.MakeKeyNameValue();

            Programming.Logging.Instance.Log($"Right keyboard \" {id_textboxLeftKeyValue.Text} \" key was selected by \" {sender.ToString()} \" selected by \" {reason.ToString()} \" ",
                                  "Information");
        }
        private void Id_buttonRightR1C7_Click(System.Object sender, System.EventArgs reason)
        {
            if (m_rightSelectedKeyButton != null)
            {
                m_rightSelectedKeyButton.UnSelected();
            }
            m_rightSelectedKeyButton = id_buttonRightR1C7;
            m_rightSelectedKeyButton.Selected();
            id_textboxRightKeyValue.Text = m_rightSelectedKeyButton.MakeKeyNameValue();

            Programming.Logging.Instance.Log($"Right keyboard \" {id_textboxLeftKeyValue.Text} \" key was selected by \" {sender.ToString()} \" selected by \" {reason.ToString()} \" ",
                                  "Information");
        }

        /// <summary>
        /// This function correspond to the left keyboard connect button
        /// </summary>
        /// <remarks>
        /// When this button is pressed it will try to connect to the corresponding serial port.
        /// If it can not find that serial port it will output a popup telling you that you have an error.
        /// </remarks>

        private System.String FullPortToJustCom(System.String fullPortName)
        {
            //TODO remove hard coding here. 
            return "COM4";
        }

        /// <summary>
        /// Updates the selected button text and tells the processing unit to update the left keyboard controller
        /// </summary>
        /// <remarks>
        /// Produces several popups if conditions are not met for producing a keyboard update. 
        /// <list type="bullet">
        /// <item>Has a connection been made to the keyboard controller.</item>
        /// <item>Was a key value selected.</item>
        /// <item>Was a layer seleected.</item>
        /// <item>Was a key selected.</item>
        /// </list>
        /// If any of those conditions were not met then no update is made. 
        /// </remarks>
        /// <param name="sender"> The System.Object that called this function</param>
        /// <param name="reason"> What event caused the System.Object to call this function</param>
        private void Id_buttonLeftUpdateKey_Click(System.Object sender, System.EventArgs reason)
        {
            Programming.Logging.Instance.Log($"Left Connect button pressed \" {sender.ToString()} \" by \" {reason.ToString()} \" ",
                                 "Information");

            // Has a connection been made. 
            if (!m_processing.IsConnected("Left Keyboard"))
            {
                Programming.Logging.Instance.Log("Update was attempted to be made before connection", severity: "Error");
                Popup noKeyError = new Popup("A connection must be made before a key can be updated.",
                                             "Updating Keyboard Error",
                                             m_selectedDarkMode);
                return;
            }
            // If no key value was selected
            if (id_comboboxLeftKeyValue.SelectedIndex == -1)
            {
                Programming.Logging.Instance.Log("A keyboard value was not selected", severity: "Error");
                Popup noKeyError = new Popup("You need to enter a key value to continue",
                                             "Updating Keyboard Error",
                                             m_selectedDarkMode);
                return;
            }
            // If no layer was selected
            if (id_comboboxLeftKeyLayer.SelectedIndex == -1)
            {
                Programming.Logging.Instance.Log("A layer was not selected.", severity: "Error");
                Popup noKeyError = new Popup("You need to enter a layer to continue",
                                             "Updating Keyboard Error",
                                             m_selectedDarkMode);
                return;
            }
            // If no button was selected
            if (id_textboxLeftKeyValue.Text == "Selected Button")
            {
                Programming.Logging.Instance.Log("No key was selected", severity: "Error");
                Popup noKeyError = new Popup("You need to select a key to continue",
                                             "Updating Keyboard Error",
                                             m_selectedDarkMode);
                return;
            }

            m_leftSelectedKeyButton.Text = (System.String)id_comboboxLeftKeyValue.SelectedItem;    // Updates the button 

            m_processing.Update("Left Keyboard",
                                (System.String)id_comboboxLeftKeyLayer.SelectedItem,
                                m_leftSelectedKeyButton.MakeKeyName(),
                                (System.String)id_comboboxLeftKeyValue.SelectedItem);
        }

        // Connect Buttons
        private void Id_buttonLeftKeyConnectComPort_Click(System.Object sender, System.EventArgs reason)
        {
            Programming.Logging.Instance.Log($"Left Keyboard Connect button pressed \" {sender.ToString()} \" by \" {reason.ToString()} \" ",
                                 "Information");
            System.String currentPort = FullPortToJustCom((System.String)id_comboboxLeftKeyComPort.SelectedItem);
            Programming.Logging.Instance.Log(currentPort, "Information");

            System.String error = null;
            System.Boolean err = false;
            err = m_processing.Connect("Left Keyboard", currentPort, out error);

            if (!err)
            {
                Popup errorPopup = new Popup(error, "Connecting Error", m_selectedDarkMode);
            }
        }
        private void Id_buttonRightKeyConnectComPort_Click(System.Object sender, System.EventArgs reason)
        {
            Programming.Logging.Instance.Log($"Right Keyboard Connect button pressed \" {sender.ToString()} \" by \" {reason.ToString()} \" ",
                                 "Information");
            System.String currentPort = FullPortToJustCom((System.String)id_comboboxRightKeyComPort.SelectedItem);
            Programming.Logging.Instance.Log(currentPort, "Information");

            System.String error = null;
            System.Boolean err = false;
            err = m_processing.Connect("Right Keyboard", currentPort, out error);

            if (!err)
            {
                Popup errorPopup = new Popup(error, "Connecting Error", m_selectedDarkMode);
            }
        }
        private void Id_buttonLeftLedConnectComPort_Click(System.Object sender, System.EventArgs reason)
        {
            Programming.Logging.Instance.Log($"Left LED Connect button pressed \" {sender.ToString()} \" by \" {reason.ToString()} \" ",
                     "Information");
            System.String currentPort = FullPortToJustCom((System.String)id_comboboxLeftLedComPort.SelectedItem);
            Programming.Logging.Instance.Log(currentPort, "Information");

            System.String error = null;
            System.Boolean err = false;
            err = m_processing.Connect("Left LED", currentPort, out error);

            if (!err)
            {
                Popup errorPopup = new Popup(error, "Connecting Error", m_selectedDarkMode);
            }
        }
        private void Id_buttonRightLedConnectComPort_Click(System.Object sender, System.EventArgs reason)
        {
            Programming.Logging.Instance.Log($"Right LED Connect button pressed \" {sender.ToString()} \" by \" {reason.ToString()} \" ",
                     "Information");
            System.String currentPort = FullPortToJustCom((System.String)id_comboboxRightLedComPort.SelectedItem);
            Programming.Logging.Instance.Log(currentPort, "Information");

            System.String error = null;
            System.Boolean err = false;
            err = m_processing.Connect("Right LED", currentPort, out error);

            if (!err)
            {
                Popup errorPopup = new Popup(error, "Connecting Error", m_selectedDarkMode);
            }
        }

        private void Id_buttonLeftUpdateLed_Click(System.Object sender, System.EventArgs reason)
        {
            Programming.Logging.Instance.Log($"Right LED Connect button pressed \" {sender.ToString()} \" by \" {reason.ToString()} \" ",
                                 "Information");

            // Has a connection been made. 
            if (!m_processing.IsConnected("Right LED"))
            {
                Programming.Logging.Instance.Log("Update was attempted to be made before connection", severity: "Error");
                Popup noKeyError = new Popup("A connection must be made before a key can be updated.",
                                             "Updating Keyboard Error",
                                             m_selectedDarkMode);
                return;
            }
            // If no key value was selected
            if (id_comboboxLeftKeyValue.SelectedIndex == -1)
            {
                Programming.Logging.Instance.Log("A keyboard value was not selected", severity: "Error");
                Popup noKeyError = new Popup("You need to enter a key value to continue",
                                             "Updating Keyboard Error",
                                             m_selectedDarkMode);
                return;
            }
            // If no layer was selected
            if (id_comboboxLeftKeyLayer.SelectedIndex == -1)
            {
                Programming.Logging.Instance.Log("A layer was not selected.", severity: "Error");
                Popup noKeyError = new Popup("You need to enter a layer to continue",
                                             "Updating Keyboard Error",
                                             m_selectedDarkMode);
                return;
            }
            // If no button was selected
            if (id_textboxLeftKeyValue.Text == "Selected Button")
            {
                Programming.Logging.Instance.Log("No key was selected", severity: "Error");
                Popup noKeyError = new Popup("You need to select a key to continue",
                                             "Updating Keyboard Error",
                                             m_selectedDarkMode);
                return;
            }

            m_leftSelectedKeyButton.Text = (System.String)id_comboboxLeftKeyValue.SelectedItem;    // Updates the button 

            m_processing.Update("Left Keyboard",
                                (System.String)id_comboboxLeftKeyLayer.SelectedItem,
                                m_leftSelectedKeyButton.MakeKeyName(),
                                (System.String)id_comboboxLeftKeyValue.SelectedItem);
        }

        private void Id_buttonRightUpdateLed_Click(System.Object sender, System.EventArgs reason)
        {

        }

  
    }
}
