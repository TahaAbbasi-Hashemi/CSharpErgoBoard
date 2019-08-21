﻿using System;
using System.Collections.Generic;
using System.Management;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO.Ports;

namespace CSharpErgoBoard
{
    /// <summary>
    /// The graphical interface class
    /// </summary>
    /// <remarks>
    /// This holds all the information regarding what the interface does and what the buttons on the interface do.
    /// </remarks>
    public partial class ErgoBoard : Form
    {
        // Class Atributes
        /// <summary>
        /// The purpose of the class
        /// </summary>
        public String Purpose { get; } = "To display and work with the Graphical interface";
        /// <summary>
        /// To convert the class to a string.
        /// </summary>
        public new String ToString { get; } = "A Ergoboard Class to do GUI procesing";

        // Encapsulated Private Variables
        private String m_leftSelectedKey = null;
        private MyButton m_leftSelectedKeyButton = null;
        private String m_leftSelectedLayer = null;
        private Boolean m_selectedDarkMode = false;

        //Private Variables
        private readonly SerialPort m_leftKeyConnection = new SerialPort();
        private readonly SerialPort m_rightKeyConnection = new SerialPort();
        private readonly SerialPort m_leftLEDConnection = new SerialPort();
        private readonly SerialPort m_rightLEDConnection = new SerialPort();


        // Encapsulation Funtions
        /// <summary>
        /// Used to idenify if dark mode is selected. <value>True</value> if darkmode is selected and <value>False</value> otherwise.
        /// </summary>
        public Boolean SelectedDarkMode { get => m_selectedDarkMode; }
        /// <summary>
        /// Used to idenify which key is currently being pressed by the user.
        /// </summary>
        /// <remarks>
        /// It should be noted that this is only to be used for editing the keyboard configuration and detecting key presses.
        /// </remarks>
        public String SelectedKey { get => m_leftSelectedKey; }

        // Functions
        /// <summary>
        /// Default constructor.
        /// </summary>
        public ErgoBoard()
        {
            InitializeComponent();
            // List the serial port names.
            List<String> possiblePorts = GetPorts();
            foreach (String port in possiblePorts)
            {
                id_comboboxRightKeyComPort.Items.Add(port);
                id_comboboxLeftKeyComPort.Items.Add(port);
            }

            SelectLightMode();
        }

        /// <summary>
        /// Finds a list of serial ports and their friendly descriptions and returns it. 
        /// </summary>
        /// <returns> A list of strings housing the COM ports and their friendly descriptions. </returns>
        public List<String> GetPorts()
        {
            List<String> names = new List<string>();
            using (ManagementObjectSearcher searcher = new ManagementObjectSearcher("root\\CIMV2",
                                                                                    "SELECT * FROM Win32_PnPEntity WHERE ClassGuid=\"{4d36e978-e325-11ce-bfc1-08002be10318}\""))
            {
                Logging.Instance.Log("Something was found as " + searcher.ToString());
                foreach (ManagementObject queryObj in searcher.Get())
                {
                    names.Add(queryObj["Name"].ToString());
                    Logging.Instance.Log("A serial port was found as " + queryObj["Name"].ToString());
                }
            }

            return names;
        }


        /// <summary>
        /// Changes the color layout of the application to a dark mode style. 
        /// </summary>
        public void SelectDarkMode()
        {
            Logging.Instance.Log("Selected Dark Mode");
            m_selectedDarkMode = true;
            
            Color backgroundBlack = Color.FromArgb(255, 20, 20, 20);

            // Main Window
            BackColor = backgroundBlack;
            ForeColor = Color.WhiteSmoke;
            id_panelMain.BackColor = backgroundBlack;
            id_panelMain.ForeColor = Color.WhiteSmoke;

            //Menu Strip
            menuStrip1.BackColor = Color.DimGray;
            menuStrip1.ForeColor = Color.WhiteSmoke;
            id_menuFile.BackColor = Color.DimGray;
            id_menuEdit.BackColor = Color.DimGray;
            id_menuView.BackColor = Color.DimGray;
            id_menuHelp.BackColor = Color.DimGray;
            id_menuAbout.BackColor = Color.DimGray;
            id_menuAuthor.BackColor = Color.DimGray;
            id_menuReferences.BackColor = Color.DimGray;
            id_menuCopyright.BackColor = Color.DimGray;
            id_menuLightMode.BackColor = Color.DimGray;
            id_menuDarkMode.BackColor = Color.DimGray;
            id_menuQuit.BackColor = Color.DimGray;
            id_menuFile.ForeColor = Color.WhiteSmoke;
            id_menuEdit.ForeColor = Color.WhiteSmoke;
            id_menuView.ForeColor = Color.WhiteSmoke;
            id_menuHelp.ForeColor = Color.WhiteSmoke;
            id_menuAbout.ForeColor = Color.WhiteSmoke;
            id_menuAuthor.ForeColor = Color.WhiteSmoke;
            id_menuReferences.ForeColor = Color.WhiteSmoke;
            id_menuCopyright.ForeColor = Color.WhiteSmoke;
            id_menuLightMode.ForeColor = Color.WhiteSmoke;
            id_menuDarkMode.ForeColor = Color.WhiteSmoke;
            id_menuQuit.ForeColor = Color.WhiteSmoke;


            // All other properties
            ModeUpdated();
        }

        /// <summary>
        /// Changes the color layout of the application to a light mode style
        /// </summary>
        /// <remarks>
        /// This is active by default.
        /// </remarks>
        public void SelectLightMode()
        {
            Logging.Instance.Log("Selected Light Mode");
            m_selectedDarkMode = false;

            // Main Window 
            BackColor = Color.White;
            ForeColor = Color.Black;
            id_panelMain.BackColor = Color.White;
            id_panelMain.ForeColor = Color.Black;

            // Menu Strip
            menuStrip1.BackColor = Color.Gainsboro;
            menuStrip1.ForeColor = Color.Black;
            id_menuFile.BackColor = Color.Gainsboro;
            id_menuEdit.BackColor = Color.Gainsboro;
            id_menuView.BackColor = Color.Gainsboro;
            id_menuHelp.BackColor = Color.Gainsboro;
            id_menuAbout.BackColor = Color.Gainsboro;
            id_menuAuthor.BackColor = Color.Gainsboro;
            id_menuReferences.BackColor = Color.Gainsboro;
            id_menuCopyright.BackColor = Color.Gainsboro;
            id_menuLightMode.BackColor = Color.Gainsboro;
            id_menuDarkMode.BackColor = Color.Gainsboro;
            id_menuQuit.BackColor = Color.Gainsboro;
            id_menuFile.ForeColor = Color.Black;
            id_menuEdit.ForeColor = Color.Black;
            id_menuView.ForeColor = Color.Black;
            id_menuHelp.ForeColor = Color.Black;
            id_menuAbout.ForeColor = Color.Black;
            id_menuAuthor.ForeColor = Color.Black;
            id_menuReferences.ForeColor = Color.Black;
            id_menuCopyright.ForeColor = Color.Black;
            id_menuLightMode.ForeColor = Color.Black;
            id_menuDarkMode.ForeColor = Color.Black;
            id_menuQuit.ForeColor = Color.Black;

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
            // Converts all Combo Boxes to their respective modes.
            id_comboboxLeftKeyValue.ModeChange(m_selectedDarkMode);
            id_comboboxLeftKeyValue.ModeChange(m_selectedDarkMode);
            id_comboboxLeftKeyLayer.ModeChange(m_selectedDarkMode);
            id_comboboxLeftKeyComPort.ModeChange(m_selectedDarkMode);
            id_comboboxRightKeyLayer.ModeChange(m_selectedDarkMode);
            id_comboboxRightKeyComPort.ModeChange(m_selectedDarkMode);
            id_comboboxRightKeyValue.ModeChange(m_selectedDarkMode);
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
                id_buttonLeftR1C1.ModeChange(m_selectedDarkMode, "Wide");;
                id_buttonLeftR2C1.ModeChange(m_selectedDarkMode, "Wide");;
                id_buttonLeftR3C1.ModeChange(m_selectedDarkMode, "Wide");;
                id_buttonLeftR4C1.ModeChange(m_selectedDarkMode, "Wide");;
                id_buttonLeftR5C1.ModeChange(m_selectedDarkMode, "Wide");;
                id_buttonLeftR6C1.ModeChange(m_selectedDarkMode, "Wide");;
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
                id_buttonRightR1C1.ModeChange(m_selectedDarkMode, "Wide");;
                id_buttonRightR2C1.ModeChange(m_selectedDarkMode, "Wide");;
                id_buttonRightR3C1.ModeChange(m_selectedDarkMode, "Wide");;
                id_buttonRightR4C1.ModeChange(m_selectedDarkMode, "Wide");;
                id_buttonRightR5C1.ModeChange(m_selectedDarkMode, "Wide");;
                id_buttonRightR6C1.ModeChange(m_selectedDarkMode, "Wide");;
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

        private void ErgoboardLoad(object sender, EventArgs reason) => FormClosing += Ergoboard_Closing;

        /// <summary>
        /// Quits the program using the menu button.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="reason"></param>
        private void Id_menuQuit_Click(object sender, EventArgs reason) => Close();

        /// <summary>
        /// Acts as a deafult destructor. Closes any open threads and does closing actions. 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="reason"></param>
        private void Ergoboard_Closing(object sender, FormClosingEventArgs reason)
        {
            Logging.Instance.Log("The program will be closing in 1 second");
            Thread.Sleep(1000);
            Logging.Instance.End();
            SystemMonitor.Instance.End();
            if (m_leftKeyConnection.IsOpen)
            {
                m_leftKeyConnection.Close();
            }
        }

        /// <summary>
        /// When the user of the application chooses to enter dark mode. 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="reason"></param>
        private void Id_menuDarkMode_Click(object sender, EventArgs reason)
        {
            SelectDarkMode();
        }

        /// <summary>
        /// When the user of the application chooses to enter light mode. 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="reason"></param>
        private void Id_menuLightMode_Click(object sender, EventArgs reason)
        {
            SelectLightMode();
        }

        // Keyboard Key Presses
        /// <summary>
        /// This is when the key on left side row 1 coloum 1 was clicked by the mouse
        /// </summary>
        /// <param name="sender">The object that called this function </param> 
        /// <param name="reason">What event caused the object to call this function </param>
        private void Id_buttonLeftR1C1_Click(object sender, EventArgs reason)
        {
            Logging.Instance.Log($"Left keyboard R1 C1 key was selected by \" {sender.ToString()} \" selected by \" {reason.ToString()} \" ",
                                 "Information");
            m_leftSelectedKey = "R1C1";
            id_textboxLeftKeyValue.Text = "Row 1, Col 1";

            if (m_leftSelectedKeyButton != null)
            {
                m_leftSelectedKeyButton.UnSelected();
            }
            m_leftSelectedKeyButton = id_buttonLeftR1C1;
            m_leftSelectedKeyButton.Selected("Wide");

        }
        /// <summary>
        /// This is when the key on left side row 1 coloum 2 was clicked by the mouse
        /// </summary>
        /// <param name="sender">The object that called this function </param> 
        /// <param name="reason">What event caused the object to call this function </param>
        private void Id_buttonLeftR1C2_Click(object sender, EventArgs reason)
        {
            Logging.Instance.Log($"Left keyboard R1 C2 key was selected by \" {sender.ToString()} \" selected by \" {reason.ToString()} \" ",
                                 "Information");
            m_leftSelectedKey = "R1C2";
            id_textboxLeftKeyValue.Text = "Row 1, Col 2";

            if (m_leftSelectedKeyButton != null)
            {
                m_leftSelectedKeyButton.UnSelected();
            }
            m_leftSelectedKeyButton = id_buttonLeftR1C2;
            m_leftSelectedKeyButton.Selected("Single");
        }
        /// <summary>
        /// This is when the key on left side row 1 coloum 3 was clicked by the mouse
        /// </summary>
        /// <param name="sender">The object that called this function </param> 
        /// <param name="reason">What event caused the object to call this function </param>
        private void Id_buttonLeftR1C3_Click(object sender, EventArgs reason)
        {
            Logging.Instance.Log($"Left keyboard R1 C3 key was selected by \" {sender.ToString()} \" selected by \" {reason.ToString()} \" ",
                                 "Information");
            m_leftSelectedKey = "R1C3";
            id_textboxLeftKeyValue.Text = "Row 1, Col 3";

            if (m_leftSelectedKeyButton != null)
            {
                m_leftSelectedKeyButton.UnSelected();
            }
            m_leftSelectedKeyButton = id_buttonLeftR1C3;
            m_leftSelectedKeyButton.Selected("Single");
        }
        /// <summary>
        /// This is when the key on left side row 1 coloum 4 was clicked by the mouse
        /// </summary>
        /// <param name="sender">The object that called this function </param> 
        /// <param name="reason">What event caused the object to call this function </param>
        private void Id_buttonLeftR1C4_Click(object sender, EventArgs reason)
        {
            Logging.Instance.Log($"Left keyboard R1 C4 key was selected by \" {sender.ToString()} \" selected by \" {reason.ToString()} \" ",
                     "Information");
            m_leftSelectedKey = "R1C4";
            id_textboxLeftKeyValue.Text = "Row 1, Col 4";

            if (m_leftSelectedKeyButton != null)
            {
                m_leftSelectedKeyButton.UnSelected();
            }
            m_leftSelectedKeyButton = id_buttonLeftR1C4;
            m_leftSelectedKeyButton.Selected("Single");
        }
        /// <summary>
        /// This is when the key on left side row 1 coloum 5 was clicked by the mouse
        /// </summary>
        /// <param name="sender">The object that called this function </param> 
        /// <param name="reason">What event caused the object to call this function </param>
        private void Id_buttonLeftR1C5_Click(object sender, EventArgs reason)
        {
            Logging.Instance.Log($"Left keyboard R1 C5 key was selected by \" {sender.ToString()} \" selected by \" {reason.ToString()} \" ",
                 "Information");
            m_leftSelectedKey = "R1C5";
            id_textboxLeftKeyValue.Text = "Row 1, Col 5";


            if (m_leftSelectedKeyButton != null)
            {
                m_leftSelectedKeyButton.UnSelected();
            }
            m_leftSelectedKeyButton = id_buttonLeftR1C5;
            m_leftSelectedKeyButton.Selected("Single");
        }
        private void Id_buttonLeftR1C6_Click(object sender, EventArgs reason)
        {
            Logging.Instance.Log($"Left keyboard R1 C6 key was selected by \" {sender.ToString()} \" selected by \" {reason.ToString()} \" ",
                 "Information");
            m_leftSelectedKey = "R1C6";
            id_textboxLeftKeyValue.Text = "Row 1, Col 6";


            if (m_leftSelectedKeyButton != null)
            {
                m_leftSelectedKeyButton.UnSelected();
            }
            m_leftSelectedKeyButton = id_buttonLeftR1C6;
            m_leftSelectedKeyButton.Selected("Single");
        }

        private void Id_buttonLeftR1C7_Click(object sender, EventArgs reason)
        {
            Logging.Instance.Log($"Left keyboard R1 C7 key was selected by \" {sender.ToString()} \" selected by \" {reason.ToString()} \" ",
             "Information");
            m_leftSelectedKey = "R1C7";
            id_textboxLeftKeyValue.Text = "Row 1, Col 7";

            if (m_leftSelectedKeyButton != null)
            {
                m_leftSelectedKeyButton.UnSelected();
            }
            m_leftSelectedKeyButton = id_buttonLeftR1C7;
            m_leftSelectedKeyButton.Selected("Single");
        }

        private void Id_buttonLeftR2C1_Click(object sender, EventArgs reason)
        {

        }

        private void Id_buttonLeftR2C2_Click(object sender, EventArgs reason)
        {

        }

        private void Id_buttonLeftR2C3_Click(object sender, EventArgs reason)
        {

        }

        private void Id_buttonLeftR2C4_Click(object sender, EventArgs reason)
        {

        }

        private void Id_buttonLeftR2C5_Click(object sender, EventArgs reason)
        {

        }

        private void Id_buttonLeftR2C6_Click(object sender, EventArgs reason)
        {

        }

        private void Id_buttonLeftR2C7_Click(object sender, EventArgs reason)
        {

        }

        private void ComboBox1_SelectedIndexChanged(object sender, EventArgs reason)
        {

        }

        private void Id_listboxLeftKeyComPort_SelectedIndexChanged(object sender, EventArgs reason)
        {

        }

        /// <summary>
        /// This function correspond to the left keyboard connect button
        /// </summary>
        /// <remarks>
        /// When this button is pressed it will try to connect to the corresponding serial port.
        /// If it can not find that serial port it will output a popup telling you that you have an error.
        /// </remarks>
        /// <param name="sender"> The object that called this function</param>
        /// <param name="reason"> What event caused the object to call this function</param>
        private void Id_buttonLeftKeyConnectComPort_Click(object sender, EventArgs reason)
        {
            Logging.Instance.Log($"Left Connect button pressed \" {sender.ToString()} \" by \" {reason.ToString()} \" ",
                                 "Information");
            String currentPort = FullPortToJustCom((String)id_comboboxLeftKeyComPort.SelectedItem);
            Logging.Instance.Log(currentPort, "Information");

            String response;
            String writeup;
            Boolean connected;

            if (m_leftKeyConnection.IsOpen)
            {
                Popup noKeyError = new Popup("You are already connected. \n"
                                             + "If you wish to change connection please disconnect.",
                                             "Connection Error", 
                                             m_selectedDarkMode);
                noKeyError.Show(this);
                return;
            }

            m_leftKeyConnection.PortName = currentPort;
            m_leftKeyConnection.ReadTimeout = 1000;
            m_leftKeyConnection.BaudRate = 9600;
            m_leftKeyConnection.Parity = Parity.None;
            m_leftKeyConnection.DataBits = 8;
            m_leftKeyConnection.StopBits = StopBits.One;
            m_leftKeyConnection.Handshake = Handshake.None;
            m_leftKeyConnection.Open();

            connected = false;  // This is false unless made true
            response = "default";
            if (m_leftKeyConnection.IsOpen)
            {
                // Send a command and wait for a response
                m_leftKeyConnection.WriteLine("Name");

                // If we fail to get a response indicate it.
                try
                {
                    response = m_leftKeyConnection.ReadLine();
                }
                catch(TimeoutException)
                {
                    MessageBoxButtons buttons = MessageBoxButtons.OK;
                    MessageBox.Show(response, "Error during serial connection", buttons);
                    response = "NA";
                }

                // Checking what our response was
                if (response == "NA")
                {
                    writeup = "Failed to get a response from the device, please try again.";
                    Logging.Instance.Log(writeup, "Error");
                }
                else if (response.StartsWith("LED"))
                {
                    writeup = "This is a LED controller not a keyboard controller, please change port.";
                    Logging.Instance.Log(writeup, "Error");
                }
                else if (response.StartsWith("Key"))
                {
                    writeup = "Left keyboard connection was properly setup.";
                    Logging.Instance.Log(writeup, "Success");
                    connected = true;
                }
                else
                {
                    writeup = "Incorrect information was gotten, please try again";
                    Logging.Instance.Log(writeup, "Error");
                }

            }
            else
            {
                writeup = "Failed to make connection with selected port";
                Logging.Instance.Log(writeup, "Error");
            }

            // If we failed to find a keyboard controller
            if (!connected)
            {
                Popup noKeyError = new Popup(writeup, "Updating Keyboard Error", m_selectedDarkMode);
                noKeyError.Show(this);
            }

        }

        private String FullPortToJustCom(String fullPortName)
        {
            //TODO remove hard coding here. 
            return "COM3";
        }

        /// <summary>
        /// This function updates the key on the keyboard controller. 
        /// </summary>
        /// <remarks>
        /// This is one of the few functions that directly speaks to the controller.
        /// </remarks>
        /// <param name="sender"> The object that called this function</param>
        /// <param name="reason"> What event caused the object to call this function</param>
        private void Id_buttonLeftUpdateKey_Click(object sender, EventArgs reason)
        {
            Logging.Instance.Log($"Left Connect button pressed \" {sender.ToString()} \" by \" {reason.ToString()} \" ",
                                 "Information");

            // If no key value was selected
            if (id_comboboxLeftKeyValue.SelectedIndex == -1)
            {
                Logging.Instance.Log("A keyboard value was not selected", "Error");
                Popup noKeyError = new Popup("You need to enter a key value to continue", "Updating Keyboard Error", m_selectedDarkMode);
                noKeyError.Show(this);
                return;
            }
            // If no layer was selected
            if (id_comboboxLeftKeyLayer.SelectedIndex == -1)
            {
                Logging.Instance.Log("A layer was not selected.", "Error");
                Popup noKeyError = new Popup("You need to enter a layer to continue", "Updating Keyboard Error", m_selectedDarkMode);
                noKeyError.Show(this);
                return;
            }
            // If no button was selected
            if (m_leftSelectedKey == null)
            {
                Logging.Instance.Log("No key was selected", "Error");
                Popup noKeyError = new Popup("You need to enter a key to continue", "Updating Keyboard Error", m_selectedDarkMode);
                noKeyError.Show(this);
                return;
            }
            // Not connected to the keybaord.
            if (!m_leftKeyConnection.IsOpen)
            {
                Logging.Instance.Log("Not connected to a serial connection", "Error");
                Popup noKeyError = new Popup("Makesure you are connected to the keyboard befor continuing", "Updating Keyboard Error", m_selectedDarkMode);
                noKeyError.Show(this);
                return;
            }

            String message;

            message = m_leftSelectedKey;
            // Layer Selection
            switch (id_comboboxLeftKeyLayer.SelectedIndex)
            {
                case 0:
                    message += "L1";
                    break;
                case 1:
                    message += "L2";
                    break;
                case 2:
                    message += "L3";
                    break;
                case 3:
                    message += "L4";
                    break;
                case 4:
                    message += "L5";
                    break;
                default:
                    break;
            }
            message += "'";
            message += (String)id_comboboxLeftKeyValue.SelectedItem;
            message += "'";

            m_leftKeyConnection.WriteLine(message); // Send the message

            m_leftSelectedKeyButton.Text = (String)id_comboboxLeftKeyValue.SelectedItem;    // Update the  button 

        }
    }
}
