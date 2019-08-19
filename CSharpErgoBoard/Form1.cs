using System;
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
        private String m_selectedKey = "NONE";
        private Boolean m_selectedDarkMode = false;

        //Private Variables
        private SerialPort m_leftKeyConnection = new SerialPort();
        private SerialPort m_rightKeyConnection = new SerialPort();
        private SerialPort m_leftLEDConnection = new SerialPort();
        private SerialPort m_rightLEDConnection = new SerialPort();


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
        public String SelectedKey { get => m_selectedKey; }

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
                Logging.Instance.Log("Added " + port + "To the selection list");
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

            Color kindofBlack = Color.FromArgb(255, 50, 50, 50);
            Color backgroundBlack = Color.FromArgb(255, 20, 20, 20);
            //Color darkBlue = Color.FromArgb(255, 0, 0, 50);

            // Main Window
            id_panelMain.BackColor = backgroundBlack;
            id_panelMain.ForeColor = Color.WhiteSmoke;
            menuStrip1.BackColor = Color.DimGray;
            menuStrip1.ForeColor = Color.WhiteSmoke;
            BackColor = backgroundBlack;
            ForeColor = Color.WhiteSmoke;

            //Left Hand Side
            //id_listboxLeftKeyLayer.BackColor = kindofBlack;
            //id_listboxLeftKeyLayer.ForeColor = Color.WhiteSmoke;
            //id_listboxLeftKeyComPort.BackColor = kindofBlack;
            //id_listboxLeftKeyComPort.ForeColor = Color.WhiteSmoke;
            id_textboxLeftKeyValue.BackColor = kindofBlack;
            id_textboxLeftKeyValue.ForeColor = Color.WhiteSmoke;
            id_buttonLeftUpdateKey.BackColor = kindofBlack;
            id_buttonLeftKeyConnectComPort.BackColor = kindofBlack;

            id_listboxLeftLedComPort.BackColor = kindofBlack;
            id_listboxLeftLedComPort.ForeColor = Color.WhiteSmoke;
            id_buttonLeftLedConnectComPort.BackColor = kindofBlack;
            id_buttonLeftLedConnectComPort.ForeColor = Color.WhiteSmoke;
            id_listboxLeftLedStyle.BackColor = kindofBlack;
            id_listboxLeftLedStyle.ForeColor = Color.WhiteSmoke;
            id_buttonLeftUpdateLed.BackColor = kindofBlack;
            id_buttonLeftUpdateLed.ForeColor = Color.WhiteSmoke;

            //Right Hand Side
            //id_listboxRightKeyLayer.BackColor = kindofBlack;
            //id_listboxRightKeyLayer.ForeColor = Color.WhiteSmoke;
            //id_listboxRightKeyComPort.BackColor = kindofBlack;
            //id_listboxRightKeyComPort.ForeColor = Color.WhiteSmoke;
            id_textboxRightKeyValue.BackColor = kindofBlack;
            id_textboxRightKeyValue.ForeColor = System.Drawing.Color.WhiteSmoke;
            id_buttonRightUpdateKey.BackColor = kindofBlack;
            id_buttonRightKeyConnectComPort.BackColor = kindofBlack;

            // Left Hand side
            // Keyboard Keys
            {
                // Column 1
                id_buttonLeftR1C1.Image = Properties.Resources.WideKeyDark;
                id_buttonLeftR2C1.Image = Properties.Resources.WideKeyDark;
                id_buttonLeftR3C1.Image = Properties.Resources.WideKeyDark;
                id_buttonLeftR4C1.Image = Properties.Resources.WideKeyDark;
                id_buttonLeftR5C1.Image = Properties.Resources.WideKeyDark;
                id_buttonLeftR6C1.Image = Properties.Resources.WideKeyDark;
                // Column 2
                id_buttonLeftR1C2.Image = Properties.Resources.SingleKeyDark;
                id_buttonLeftR2C2.Image = Properties.Resources.SingleKeyDark;
                id_buttonLeftR3C2.Image = Properties.Resources.SingleKeyDark;
                id_buttonLeftR4C2.Image = Properties.Resources.SingleKeyDark;
                id_buttonLeftR5C2.Image = Properties.Resources.SingleKeyDark;
                id_buttonLeftR6C2.Image = Properties.Resources.SingleKeyDark;
                // Column 3
                id_buttonLeftR1C3.Image = Properties.Resources.SingleKeyDark;
                id_buttonLeftR2C3.Image = Properties.Resources.SingleKeyDark;
                id_buttonLeftR3C3.Image = Properties.Resources.SingleKeyDark;
                id_buttonLeftR4C3.Image = Properties.Resources.SingleKeyDark;
                id_buttonLeftR5C3.Image = Properties.Resources.SingleKeyDark;
                id_buttonLeftR6C3.Image = Properties.Resources.SingleKeyDark;
                // Column 4
                id_buttonLeftR1C4.Image = Properties.Resources.SingleKeyDark;
                id_buttonLeftR2C4.Image = Properties.Resources.SingleKeyDark;
                id_buttonLeftR3C4.Image = Properties.Resources.SingleKeyDark;
                id_buttonLeftR4C4.Image = Properties.Resources.SingleKeyDark;
                id_buttonLeftR5C4.Image = Properties.Resources.SingleKeyDark;
                id_buttonLeftR6C4.Image = Properties.Resources.SingleKeyDark;
                // Column 5
                id_buttonLeftR1C5.Image = Properties.Resources.SingleKeyDark;
                id_buttonLeftR2C5.Image = Properties.Resources.SingleKeyDark;
                id_buttonLeftR3C5.Image = Properties.Resources.SingleKeyDark;
                id_buttonLeftR4C5.Image = Properties.Resources.SingleKeyDark;
                id_buttonLeftR5C5.Image = Properties.Resources.SingleKeyDark;
                id_buttonLeftR6C5.Image = Properties.Resources.SingleKeyDark;
                // Column 6
                id_buttonLeftR1C6.Image = Properties.Resources.SingleKeyDark;
                id_buttonLeftR2C6.Image = Properties.Resources.SingleKeyDark;
                id_buttonLeftR3C6.Image = Properties.Resources.SingleKeyDark;
                id_buttonLeftR4C6.Image = Properties.Resources.SingleKeyDark;
                id_buttonLeftR5C6.Image = Properties.Resources.SingleKeyDark;
                id_buttonLeftR6C6.Image = Properties.Resources.TallKeyDark;
                // Column 7
                id_buttonLeftR1C7.Image = Properties.Resources.SingleKeyDark;
                id_buttonLeftR2C7.Image = Properties.Resources.SingleKeyDark;
                id_buttonLeftR3C7.Image = Properties.Resources.SingleKeyDark;
                id_buttonLeftR4C7.Image = Properties.Resources.SingleKeyDark;
                id_buttonLeftR5C7.Image = Properties.Resources.SingleKeyDark;
                id_buttonLeftR6C7.Image = Properties.Resources.TallKeyDark;
                // Column 8
                id_buttonLeftR3C8.Image = Properties.Resources.SingleKeyDark;
                id_buttonLeftR4C8.Image = Properties.Resources.SingleKeyDark;
                id_buttonLeftR5C8.Image = Properties.Resources.SingleKeyDark;
                id_buttonLeftR6C8.Image = Properties.Resources.SingleKeyDark;
            }
            //LEDS
            {
                // Column 1
                id_buttonLeftLedR1C1.Image = Properties.Resources.WideKeyDarkLED;
                id_buttonLeftLedR2C1.Image = Properties.Resources.WideKeyDarkLED;
                id_buttonLeftLedR3C1.Image = Properties.Resources.WideKeyDarkLED;
                id_buttonLeftLedR4C1.Image = Properties.Resources.WideKeyDarkLED;
                id_buttonLeftLedR5C1.Image = Properties.Resources.WideKeyDarkLED;
                id_buttonLeftLedR6C1.Image = Properties.Resources.WideKeyDarkLED;
                // Column 2
                id_buttonLeftLedR1C2.Image = Properties.Resources.SingleKeyDarkLED;
                id_buttonLeftLedR2C2.Image = Properties.Resources.SingleKeyDarkLED;
                id_buttonLeftLedR3C2.Image = Properties.Resources.SingleKeyDarkLED;
                id_buttonLeftLedR4C2.Image = Properties.Resources.SingleKeyDarkLED;
                id_buttonLeftLedR5C2.Image = Properties.Resources.SingleKeyDarkLED;
                id_buttonLeftLedR6C2.Image = Properties.Resources.SingleKeyDarkLED;
                // Column 3
                id_buttonLeftLedR1C3.Image = Properties.Resources.SingleKeyDarkLED;
                id_buttonLeftLedR2C3.Image = Properties.Resources.SingleKeyDarkLED;
                id_buttonLeftLedR3C3.Image = Properties.Resources.SingleKeyDarkLED;
                id_buttonLeftLedR4C3.Image = Properties.Resources.SingleKeyDarkLED;
                id_buttonLeftLedR5C3.Image = Properties.Resources.SingleKeyDarkLED;
                id_buttonLeftLedR6C3.Image = Properties.Resources.SingleKeyDarkLED;
                // Column 4
                id_buttonLeftLedR1C4.Image = Properties.Resources.SingleKeyDarkLED;
                id_buttonLeftLedR2C4.Image = Properties.Resources.SingleKeyDarkLED;
                id_buttonLeftLedR3C4.Image = Properties.Resources.SingleKeyDarkLED;
                id_buttonLeftLedR4C4.Image = Properties.Resources.SingleKeyDarkLED;
                id_buttonLeftLedR5C4.Image = Properties.Resources.SingleKeyDarkLED;
                id_buttonLeftLedR6C4.Image = Properties.Resources.SingleKeyDarkLED;
                // Column 5
                id_buttonLeftLedR1C5.Image = Properties.Resources.SingleKeyDarkLED;
                id_buttonLeftLedR2C5.Image = Properties.Resources.SingleKeyDarkLED;
                id_buttonLeftLedR3C5.Image = Properties.Resources.SingleKeyDarkLED;
                id_buttonLeftLedR4C5.Image = Properties.Resources.SingleKeyDarkLED;
                id_buttonLeftLedR5C5.Image = Properties.Resources.SingleKeyDarkLED;
                id_buttonLeftLedR6C5.Image = Properties.Resources.SingleKeyDarkLED;
                // Column 6
                id_buttonLeftLedR1C6.Image = Properties.Resources.SingleKeyDarkLED;
                id_buttonLeftLedR2C6.Image = Properties.Resources.SingleKeyDarkLED;
                id_buttonLeftLedR3C6.Image = Properties.Resources.SingleKeyDarkLED;
                id_buttonLeftLedR4C6.Image = Properties.Resources.SingleKeyDarkLED;
                id_buttonLeftLedR5C6.Image = Properties.Resources.SingleKeyDarkLED;
                id_buttonLeftLedR6C6.Image = Properties.Resources.TallKeyDarkLED;
                // Column 7
                id_buttonLeftLedR1C7.Image = Properties.Resources.SingleKeyDarkLED;
                id_buttonLeftLedR2C7.Image = Properties.Resources.SingleKeyDarkLED;
                id_buttonLeftLedR3C7.Image = Properties.Resources.SingleKeyDarkLED;
                id_buttonLeftLedR4C7.Image = Properties.Resources.SingleKeyDarkLED;
                id_buttonLeftLedR5C7.Image = Properties.Resources.SingleKeyDarkLED;
                id_buttonLeftLedR6C7.Image = Properties.Resources.TallKeyDarkLED;
                // Column 8
                id_buttonLeftLedR3C8.Image = Properties.Resources.SingleKeyDarkLED;
                id_buttonLeftLedR4C8.Image = Properties.Resources.SingleKeyDarkLED;
                id_buttonLeftLedR5C8.Image = Properties.Resources.SingleKeyDarkLED;
                id_buttonLeftLedR6C8.Image = Properties.Resources.SingleKeyDarkLED;
            }


            // Right Hand side
            // Keyboard Keys
            {
                // Column 1
                id_buttonRightR1C1.Image = Properties.Resources.WideKeyDark;
                id_buttonRightR2C1.Image = Properties.Resources.WideKeyDark;
                id_buttonRightR3C1.Image = Properties.Resources.WideKeyDark;
                id_buttonRightR4C1.Image = Properties.Resources.WideKeyDark;
                id_buttonRightR5C1.Image = Properties.Resources.WideKeyDark;
                id_buttonRightR6C1.Image = Properties.Resources.WideKeyDark;
                // Column 2
                id_buttonRightR1C2.Image = Properties.Resources.SingleKeyDark;
                id_buttonRightR2C2.Image = Properties.Resources.SingleKeyDark;
                id_buttonRightR3C2.Image = Properties.Resources.SingleKeyDark;
                id_buttonRightR4C2.Image = Properties.Resources.SingleKeyDark;
                id_buttonRightR5C2.Image = Properties.Resources.SingleKeyDark;
                id_buttonRightR6C2.Image = Properties.Resources.SingleKeyDark;
                // Column 3
                id_buttonRightR1C3.Image = Properties.Resources.SingleKeyDark;
                id_buttonRightR2C3.Image = Properties.Resources.SingleKeyDark;
                id_buttonRightR3C3.Image = Properties.Resources.SingleKeyDark;
                id_buttonRightR4C3.Image = Properties.Resources.SingleKeyDark;
                id_buttonRightR5C3.Image = Properties.Resources.SingleKeyDark;
                id_buttonRightR6C3.Image = Properties.Resources.SingleKeyDark;
                // Column 4
                id_buttonRightR1C4.Image = Properties.Resources.SingleKeyDark;
                id_buttonRightR2C4.Image = Properties.Resources.SingleKeyDark;
                id_buttonRightR3C4.Image = Properties.Resources.SingleKeyDark;
                id_buttonRightR4C4.Image = Properties.Resources.SingleKeyDark;
                id_buttonRightR5C4.Image = Properties.Resources.SingleKeyDark;
                id_buttonRightR6C4.Image = Properties.Resources.SingleKeyDark;
                // Column 5
                id_buttonRightR1C5.Image = Properties.Resources.SingleKeyDark;
                id_buttonRightR2C5.Image = Properties.Resources.SingleKeyDark;
                id_buttonRightR3C5.Image = Properties.Resources.SingleKeyDark;
                id_buttonRightR4C5.Image = Properties.Resources.SingleKeyDark;
                id_buttonRightR5C5.Image = Properties.Resources.SingleKeyDark;
                id_buttonRightR6C5.Image = Properties.Resources.SingleKeyDark;
                // Column 6
                id_buttonRightR1C6.Image = Properties.Resources.SingleKeyDark;
                id_buttonRightR2C6.Image = Properties.Resources.SingleKeyDark;
                id_buttonRightR3C6.Image = Properties.Resources.SingleKeyDark;
                id_buttonRightR4C6.Image = Properties.Resources.SingleKeyDark;
                id_buttonRightR5C6.Image = Properties.Resources.SingleKeyDark;
                id_buttonRightR6C6.Image = Properties.Resources.TallKeyDark;
                // Column 7
                id_buttonRightR1C7.Image = Properties.Resources.SingleKeyDark;
                id_buttonRightR2C7.Image = Properties.Resources.SingleKeyDark;
                id_buttonRightR3C7.Image = Properties.Resources.SingleKeyDark;
                id_buttonRightR4C7.Image = Properties.Resources.SingleKeyDark;
                id_buttonRightR5C7.Image = Properties.Resources.SingleKeyDark;
                id_buttonRightR6C7.Image = Properties.Resources.TallKeyDark;
                // Column 8
                id_buttonRightR3C8.Image = Properties.Resources.SingleKeyDark;
                id_buttonRightR4C8.Image = Properties.Resources.SingleKeyDark;
                id_buttonRightR5C8.Image = Properties.Resources.SingleKeyDark;
                id_buttonRightR6C8.Image = Properties.Resources.SingleKeyDark;
            }
            // LEDS
            {
                // Column 1
                id_buttonRightLedR1C1.Image = Properties.Resources.WideKeyDarkLED;
                id_buttonRightLedR2C1.Image = Properties.Resources.WideKeyDarkLED;
                id_buttonRightLedR3C1.Image = Properties.Resources.WideKeyDarkLED;
                id_buttonRightLedR4C1.Image = Properties.Resources.WideKeyDarkLED;
                id_buttonRightLedR5C1.Image = Properties.Resources.WideKeyDarkLED;
                id_buttonRightLedR6C1.Image = Properties.Resources.WideKeyDarkLED;
                // Column 2
                id_buttonRightLedR1C2.Image = Properties.Resources.SingleKeyDarkLED;
                id_buttonRightLedR2C2.Image = Properties.Resources.SingleKeyDarkLED;
                id_buttonRightLedR3C2.Image = Properties.Resources.SingleKeyDarkLED;
                id_buttonRightLedR4C2.Image = Properties.Resources.SingleKeyDarkLED;
                id_buttonRightLedR5C2.Image = Properties.Resources.SingleKeyDarkLED;
                id_buttonRightLedR6C2.Image = Properties.Resources.SingleKeyDarkLED;
                // Column 3
                id_buttonRightLedR1C3.Image = Properties.Resources.SingleKeyDarkLED;
                id_buttonRightLedR2C3.Image = Properties.Resources.SingleKeyDarkLED;
                id_buttonRightLedR3C3.Image = Properties.Resources.SingleKeyDarkLED;
                id_buttonRightLedR4C3.Image = Properties.Resources.SingleKeyDarkLED;
                id_buttonRightLedR5C3.Image = Properties.Resources.SingleKeyDarkLED;
                id_buttonRightLedR6C3.Image = Properties.Resources.SingleKeyDarkLED;
                // Column 4
                id_buttonRightLedR1C4.Image = Properties.Resources.SingleKeyDarkLED;
                id_buttonRightLedR2C4.Image = Properties.Resources.SingleKeyDarkLED;
                id_buttonRightLedR3C4.Image = Properties.Resources.SingleKeyDarkLED;
                id_buttonRightLedR4C4.Image = Properties.Resources.SingleKeyDarkLED;
                id_buttonRightLedR5C4.Image = Properties.Resources.SingleKeyDarkLED;
                id_buttonRightLedR6C4.Image = Properties.Resources.SingleKeyDarkLED;
                // Column 5
                id_buttonRightLedR1C5.Image = Properties.Resources.SingleKeyDarkLED;
                id_buttonRightLedR2C5.Image = Properties.Resources.SingleKeyDarkLED;
                id_buttonRightLedR3C5.Image = Properties.Resources.SingleKeyDarkLED;
                id_buttonRightLedR4C5.Image = Properties.Resources.SingleKeyDarkLED;
                id_buttonRightLedR5C5.Image = Properties.Resources.SingleKeyDarkLED;
                id_buttonRightLedR6C5.Image = Properties.Resources.SingleKeyDarkLED;
                // Column 6
                id_buttonRightLedR1C6.Image = Properties.Resources.SingleKeyDarkLED;
                id_buttonRightLedR2C6.Image = Properties.Resources.SingleKeyDarkLED;
                id_buttonRightLedR3C6.Image = Properties.Resources.SingleKeyDarkLED;
                id_buttonRightLedR4C6.Image = Properties.Resources.SingleKeyDarkLED;
                id_buttonRightLedR5C6.Image = Properties.Resources.SingleKeyDarkLED;
                id_buttonRightLedR6C6.Image = Properties.Resources.TallKeyDarkLED;
                // Column 7
                id_buttonRightLedR1C7.Image = Properties.Resources.SingleKeyDarkLED;
                id_buttonRightLedR2C7.Image = Properties.Resources.SingleKeyDarkLED;
                id_buttonRightLedR3C7.Image = Properties.Resources.SingleKeyDarkLED;
                id_buttonRightLedR4C7.Image = Properties.Resources.SingleKeyDarkLED;
                id_buttonRightLedR5C7.Image = Properties.Resources.SingleKeyDarkLED;
                id_buttonRightLedR6C7.Image = Properties.Resources.TallKeyDarkLED;
                // Column 8
                id_buttonRightLedR3C8.Image = Properties.Resources.SingleKeyDarkLED;
                id_buttonRightLedR4C8.Image = Properties.Resources.SingleKeyDarkLED;
                id_buttonRightLedR5C8.Image = Properties.Resources.SingleKeyDarkLED;
                id_buttonRightLedR6C8.Image = Properties.Resources.SingleKeyDarkLED;
            }
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
            id_panelMain.BackColor = System.Drawing.Color.White;
            id_panelMain.ForeColor = System.Drawing.Color.Black;
            menuStrip1.BackColor = System.Drawing.Color.Gainsboro;
            menuStrip1.ForeColor = System.Drawing.Color.Black;

            //id_listboxLeftKeyLayer.BackColor = System.Drawing.Color.WhiteSmoke;
            //id_listboxLeftKeyLayer.ForeColor = System.Drawing.Color.Black;
            id_textboxLeftKeyValue.BackColor = System.Drawing.Color.WhiteSmoke;
            id_textboxLeftKeyValue.ForeColor = System.Drawing.Color.Black;
            id_buttonLeftUpdateKey.BackColor = System.Drawing.Color.WhiteSmoke;
            id_buttonLeftKeyConnectComPort.BackColor = System.Drawing.Color.WhiteSmoke;

            id_textboxRightKeyValue.BackColor = System.Drawing.Color.WhiteSmoke;
            id_textboxRightKeyValue.ForeColor = System.Drawing.Color.Black;

            ForeColor = System.Drawing.Color.Black;
            BackColor = System.Drawing.Color.WhiteSmoke;


            id_buttonRightKeyConnectComPort.BackColor = System.Drawing.Color.WhiteSmoke;
            id_buttonRightUpdateKey.BackColor = System.Drawing.Color.WhiteSmoke;

            // Left hand side
            // Keyboard Keys
            {
                // Column 1
                id_buttonLeftR1C1.Image = Properties.Resources.WideKeyLight;
                id_buttonLeftR2C1.Image = Properties.Resources.WideKeyLight;
                id_buttonLeftR3C1.Image = Properties.Resources.WideKeyLight;
                id_buttonLeftR4C1.Image = Properties.Resources.WideKeyLight;
                id_buttonLeftR5C1.Image = Properties.Resources.WideKeyLight;
                id_buttonLeftR6C1.Image = Properties.Resources.WideKeyLight;
                // Column 2
                id_buttonLeftR1C2.Image = Properties.Resources.SingleKeyLight;
                id_buttonLeftR2C2.Image = Properties.Resources.SingleKeyLight;
                id_buttonLeftR3C2.Image = Properties.Resources.SingleKeyLight;
                id_buttonLeftR4C2.Image = Properties.Resources.SingleKeyLight;
                id_buttonLeftR5C2.Image = Properties.Resources.SingleKeyLight;
                id_buttonLeftR6C2.Image = Properties.Resources.SingleKeyLight;
                // Column 3
                id_buttonLeftR1C3.Image = Properties.Resources.SingleKeyLight;
                id_buttonLeftR2C3.Image = Properties.Resources.SingleKeyLight;
                id_buttonLeftR3C3.Image = Properties.Resources.SingleKeyLight;
                id_buttonLeftR4C3.Image = Properties.Resources.SingleKeyLight;
                id_buttonLeftR5C3.Image = Properties.Resources.SingleKeyLight;
                id_buttonLeftR6C3.Image = Properties.Resources.SingleKeyLight;
                // Column 4
                id_buttonLeftR1C4.Image = Properties.Resources.SingleKeyLight;
                id_buttonLeftR2C4.Image = Properties.Resources.SingleKeyLight;
                id_buttonLeftR3C4.Image = Properties.Resources.SingleKeyLight;
                id_buttonLeftR4C4.Image = Properties.Resources.SingleKeyLight;
                id_buttonLeftR5C4.Image = Properties.Resources.SingleKeyLight;
                id_buttonLeftR6C4.Image = Properties.Resources.SingleKeyLight;
                // Column 5
                id_buttonLeftR1C5.Image = Properties.Resources.SingleKeyLight;
                id_buttonLeftR2C5.Image = Properties.Resources.SingleKeyLight;
                id_buttonLeftR3C5.Image = Properties.Resources.SingleKeyLight;
                id_buttonLeftR4C5.Image = Properties.Resources.SingleKeyLight;
                id_buttonLeftR5C5.Image = Properties.Resources.SingleKeyLight;
                id_buttonLeftR6C5.Image = Properties.Resources.SingleKeyLight;
                // Column 6
                id_buttonLeftR1C6.Image = Properties.Resources.SingleKeyLight;
                id_buttonLeftR2C6.Image = Properties.Resources.SingleKeyLight;
                id_buttonLeftR3C6.Image = Properties.Resources.SingleKeyLight;
                id_buttonLeftR4C6.Image = Properties.Resources.SingleKeyLight;
                id_buttonLeftR5C6.Image = Properties.Resources.SingleKeyLight;
                id_buttonLeftR6C6.Image = Properties.Resources.TallKeyLight;
                // Column 7
                id_buttonLeftR1C7.Image = Properties.Resources.SingleKeyLight;
                id_buttonLeftR2C7.Image = Properties.Resources.SingleKeyLight;
                id_buttonLeftR3C7.Image = Properties.Resources.SingleKeyLight;
                id_buttonLeftR4C7.Image = Properties.Resources.SingleKeyLight;
                id_buttonLeftR5C7.Image = Properties.Resources.SingleKeyLight;
                id_buttonLeftR6C7.Image = Properties.Resources.TallKeyLight;
                // Column 8
                id_buttonLeftR3C8.Image = Properties.Resources.SingleKeyLight;
                id_buttonLeftR4C8.Image = Properties.Resources.SingleKeyLight;
                id_buttonLeftR5C8.Image = Properties.Resources.SingleKeyLight;
                id_buttonLeftR6C8.Image = Properties.Resources.SingleKeyLight;
            }
            //LEDS
            {
                // Column 1
                id_buttonLeftLedR1C1.Image = Properties.Resources.WideKeyLightLED;
                id_buttonLeftLedR2C1.Image = Properties.Resources.WideKeyLightLED;
                id_buttonLeftLedR3C1.Image = Properties.Resources.WideKeyLightLED;
                id_buttonLeftLedR4C1.Image = Properties.Resources.WideKeyLightLED;
                id_buttonLeftLedR5C1.Image = Properties.Resources.WideKeyLightLED;
                id_buttonLeftLedR6C1.Image = Properties.Resources.WideKeyLightLED;
                // Column 2
                id_buttonLeftLedR1C2.Image = Properties.Resources.SingleKeyLightLED;
                id_buttonLeftLedR2C2.Image = Properties.Resources.SingleKeyLightLED;
                id_buttonLeftLedR3C2.Image = Properties.Resources.SingleKeyLightLED;
                id_buttonLeftLedR4C2.Image = Properties.Resources.SingleKeyLightLED;
                id_buttonLeftLedR5C2.Image = Properties.Resources.SingleKeyLightLED;
                id_buttonLeftLedR6C2.Image = Properties.Resources.SingleKeyLightLED;
                // Column 3
                id_buttonLeftLedR1C3.Image = Properties.Resources.SingleKeyLightLED;
                id_buttonLeftLedR2C3.Image = Properties.Resources.SingleKeyLightLED;
                id_buttonLeftLedR3C3.Image = Properties.Resources.SingleKeyLightLED;
                id_buttonLeftLedR4C3.Image = Properties.Resources.SingleKeyLightLED;
                id_buttonLeftLedR5C3.Image = Properties.Resources.SingleKeyLightLED;
                id_buttonLeftLedR6C3.Image = Properties.Resources.SingleKeyLightLED;
                // Column 4
                id_buttonLeftLedR1C4.Image = Properties.Resources.SingleKeyLightLED;
                id_buttonLeftLedR2C4.Image = Properties.Resources.SingleKeyLightLED;
                id_buttonLeftLedR3C4.Image = Properties.Resources.SingleKeyLightLED;
                id_buttonLeftLedR4C4.Image = Properties.Resources.SingleKeyLightLED;
                id_buttonLeftLedR5C4.Image = Properties.Resources.SingleKeyLightLED;
                id_buttonLeftLedR6C4.Image = Properties.Resources.SingleKeyLightLED;
                // Column 5
                id_buttonLeftLedR1C5.Image = Properties.Resources.SingleKeyLightLED;
                id_buttonLeftLedR2C5.Image = Properties.Resources.SingleKeyLightLED;
                id_buttonLeftLedR3C5.Image = Properties.Resources.SingleKeyLightLED;
                id_buttonLeftLedR4C5.Image = Properties.Resources.SingleKeyLightLED;
                id_buttonLeftLedR5C5.Image = Properties.Resources.SingleKeyLightLED;
                id_buttonLeftLedR6C5.Image = Properties.Resources.SingleKeyLightLED;
                // Column 6
                id_buttonLeftLedR1C6.Image = Properties.Resources.SingleKeyLightLED;
                id_buttonLeftLedR2C6.Image = Properties.Resources.SingleKeyLightLED;
                id_buttonLeftLedR3C6.Image = Properties.Resources.SingleKeyLightLED;
                id_buttonLeftLedR4C6.Image = Properties.Resources.SingleKeyLightLED;
                id_buttonLeftLedR5C6.Image = Properties.Resources.SingleKeyLightLED;
                id_buttonLeftLedR6C6.Image = Properties.Resources.TallKeyLightLED;
                // Column 7
                id_buttonLeftLedR1C7.Image = Properties.Resources.SingleKeyLightLED;
                id_buttonLeftLedR2C7.Image = Properties.Resources.SingleKeyLightLED;
                id_buttonLeftLedR3C7.Image = Properties.Resources.SingleKeyLightLED;
                id_buttonLeftLedR4C7.Image = Properties.Resources.SingleKeyLightLED;
                id_buttonLeftLedR5C7.Image = Properties.Resources.SingleKeyLightLED;
                id_buttonLeftLedR6C7.Image = Properties.Resources.TallKeyLightLED;
                // Column 8
                id_buttonLeftLedR3C8.Image = Properties.Resources.SingleKeyLightLED;
                id_buttonLeftLedR4C8.Image = Properties.Resources.SingleKeyLightLED;
                id_buttonLeftLedR5C8.Image = Properties.Resources.SingleKeyLightLED;
                id_buttonLeftLedR6C8.Image = Properties.Resources.SingleKeyLightLED;
            }

            // Right Hand side
            // Keyboard Keys
            {
                // Column 1
                id_buttonRightR1C1.Image = Properties.Resources.WideKeyLight;
                id_buttonRightR2C1.Image = Properties.Resources.WideKeyLight;
                id_buttonRightR3C1.Image = Properties.Resources.WideKeyLight;
                id_buttonRightR4C1.Image = Properties.Resources.WideKeyLight;
                id_buttonRightR5C1.Image = Properties.Resources.WideKeyLight;
                id_buttonRightR6C1.Image = Properties.Resources.WideKeyLight;
                // Column 2
                id_buttonRightR1C2.Image = Properties.Resources.SingleKeyLight;
                id_buttonRightR2C2.Image = Properties.Resources.SingleKeyLight;
                id_buttonRightR3C2.Image = Properties.Resources.SingleKeyLight;
                id_buttonRightR4C2.Image = Properties.Resources.SingleKeyLight;
                id_buttonRightR5C2.Image = Properties.Resources.SingleKeyLight;
                id_buttonRightR6C2.Image = Properties.Resources.SingleKeyLight;
                // Column 3
                id_buttonRightR1C3.Image = Properties.Resources.SingleKeyLight;
                id_buttonRightR2C3.Image = Properties.Resources.SingleKeyLight;
                id_buttonRightR3C3.Image = Properties.Resources.SingleKeyLight;
                id_buttonRightR4C3.Image = Properties.Resources.SingleKeyLight;
                id_buttonRightR5C3.Image = Properties.Resources.SingleKeyLight;
                id_buttonRightR6C3.Image = Properties.Resources.SingleKeyLight;
                // Column 4
                id_buttonRightR1C4.Image = Properties.Resources.SingleKeyLight;
                id_buttonRightR2C4.Image = Properties.Resources.SingleKeyLight;
                id_buttonRightR3C4.Image = Properties.Resources.SingleKeyLight;
                id_buttonRightR4C4.Image = Properties.Resources.SingleKeyLight;
                id_buttonRightR5C4.Image = Properties.Resources.SingleKeyLight;
                id_buttonRightR6C4.Image = Properties.Resources.SingleKeyLight;
                // Column 5
                id_buttonRightR1C5.Image = Properties.Resources.SingleKeyLight;
                id_buttonRightR2C5.Image = Properties.Resources.SingleKeyLight;
                id_buttonRightR3C5.Image = Properties.Resources.SingleKeyLight;
                id_buttonRightR4C5.Image = Properties.Resources.SingleKeyLight;
                id_buttonRightR5C5.Image = Properties.Resources.SingleKeyLight;
                id_buttonRightR6C5.Image = Properties.Resources.SingleKeyLight;
                // Column 6
                id_buttonRightR1C6.Image = Properties.Resources.SingleKeyLight;
                id_buttonRightR2C6.Image = Properties.Resources.SingleKeyLight;
                id_buttonRightR3C6.Image = Properties.Resources.SingleKeyLight;
                id_buttonRightR4C6.Image = Properties.Resources.SingleKeyLight;
                id_buttonRightR5C6.Image = Properties.Resources.SingleKeyLight;
                id_buttonRightR6C6.Image = Properties.Resources.TallKeyLight;
                // Column 7
                id_buttonRightR1C7.Image = Properties.Resources.SingleKeyLight;
                id_buttonRightR2C7.Image = Properties.Resources.SingleKeyLight;
                id_buttonRightR3C7.Image = Properties.Resources.SingleKeyLight;
                id_buttonRightR4C7.Image = Properties.Resources.SingleKeyLight;
                id_buttonRightR5C7.Image = Properties.Resources.SingleKeyLight;
                id_buttonRightR6C7.Image = Properties.Resources.TallKeyLight;
                // Column 8
                id_buttonRightR3C8.Image = Properties.Resources.SingleKeyLight;
                id_buttonRightR4C8.Image = Properties.Resources.SingleKeyLight;
                id_buttonRightR5C8.Image = Properties.Resources.SingleKeyLight;
                id_buttonRightR6C8.Image = Properties.Resources.SingleKeyLight;
            }
            //LEDS
            {
                // Column 1
                id_buttonRightLedR1C1.Image = Properties.Resources.WideKeyLightLED;
                id_buttonRightLedR2C1.Image = Properties.Resources.WideKeyLightLED;
                id_buttonRightLedR3C1.Image = Properties.Resources.WideKeyLightLED;
                id_buttonRightLedR4C1.Image = Properties.Resources.WideKeyLightLED;
                id_buttonRightLedR5C1.Image = Properties.Resources.WideKeyLightLED;
                id_buttonRightLedR6C1.Image = Properties.Resources.WideKeyLightLED;
                // Column 2
                id_buttonRightLedR1C2.Image = Properties.Resources.SingleKeyLightLED;
                id_buttonRightLedR2C2.Image = Properties.Resources.SingleKeyLightLED;
                id_buttonRightLedR3C2.Image = Properties.Resources.SingleKeyLightLED;
                id_buttonRightLedR4C2.Image = Properties.Resources.SingleKeyLightLED;
                id_buttonRightLedR5C2.Image = Properties.Resources.SingleKeyLightLED;
                id_buttonRightLedR6C2.Image = Properties.Resources.SingleKeyLightLED;
                // Column 3
                id_buttonRightLedR1C3.Image = Properties.Resources.SingleKeyLightLED;
                id_buttonRightLedR2C3.Image = Properties.Resources.SingleKeyLightLED;
                id_buttonRightLedR3C3.Image = Properties.Resources.SingleKeyLightLED;
                id_buttonRightLedR4C3.Image = Properties.Resources.SingleKeyLightLED;
                id_buttonRightLedR5C3.Image = Properties.Resources.SingleKeyLightLED;
                id_buttonRightLedR6C3.Image = Properties.Resources.SingleKeyLightLED;
                // Column 4
                id_buttonRightLedR1C4.Image = Properties.Resources.SingleKeyLightLED;
                id_buttonRightLedR2C4.Image = Properties.Resources.SingleKeyLightLED;
                id_buttonRightLedR3C4.Image = Properties.Resources.SingleKeyLightLED;
                id_buttonRightLedR4C4.Image = Properties.Resources.SingleKeyLightLED;
                id_buttonRightLedR5C4.Image = Properties.Resources.SingleKeyLightLED;
                id_buttonRightLedR6C4.Image = Properties.Resources.SingleKeyLightLED;
                // Column 5
                id_buttonRightLedR1C5.Image = Properties.Resources.SingleKeyLightLED;
                id_buttonRightLedR2C5.Image = Properties.Resources.SingleKeyLightLED;
                id_buttonRightLedR3C5.Image = Properties.Resources.SingleKeyLightLED;
                id_buttonRightLedR4C5.Image = Properties.Resources.SingleKeyLightLED;
                id_buttonRightLedR5C5.Image = Properties.Resources.SingleKeyLightLED;
                id_buttonRightLedR6C5.Image = Properties.Resources.SingleKeyLightLED;
                // Column 6
                id_buttonRightLedR1C6.Image = Properties.Resources.SingleKeyLightLED;
                id_buttonRightLedR2C6.Image = Properties.Resources.SingleKeyLightLED;
                id_buttonRightLedR3C6.Image = Properties.Resources.SingleKeyLightLED;
                id_buttonRightLedR4C6.Image = Properties.Resources.SingleKeyLightLED;
                id_buttonRightLedR5C6.Image = Properties.Resources.SingleKeyLightLED;
                id_buttonRightLedR6C6.Image = Properties.Resources.TallKeyLightLED;
                // Column 7
                id_buttonRightLedR1C7.Image = Properties.Resources.SingleKeyLightLED;
                id_buttonRightLedR2C7.Image = Properties.Resources.SingleKeyLightLED;
                id_buttonRightLedR3C7.Image = Properties.Resources.SingleKeyLightLED;
                id_buttonRightLedR4C7.Image = Properties.Resources.SingleKeyLightLED;
                id_buttonRightLedR5C7.Image = Properties.Resources.SingleKeyLightLED;
                id_buttonRightLedR6C7.Image = Properties.Resources.TallKeyLightLED;
                // Column 8
                id_buttonRightLedR3C8.Image = Properties.Resources.SingleKeyLightLED;
                id_buttonRightLedR4C8.Image = Properties.Resources.SingleKeyLightLED;
                id_buttonRightLedR5C8.Image = Properties.Resources.SingleKeyLightLED;
                id_buttonRightLedR6C8.Image = Properties.Resources.SingleKeyLightLED;

            }
        }

        private void Form1_Load(object sender, EventArgs e) => FormClosing += Form1_FormClosing;

        /// <summary>
        /// Quits the program using the menu button.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Id_menuQuit_Click(object sender, EventArgs e) => Close();

        /// <summary>
        /// Acts as a deafult destructor. Closes any open threads and does closing actions. 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            Logging.Instance.Log("The program will be closing in 1 second");
            Thread.Sleep(1000);
            Logging.Instance.End();
            SystemMonitor.Instance.End();
        }

        /// <summary>
        /// When the user of the application chooses to enter dark mode. 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Id_menuDarkMode_Click(object sender, EventArgs e)
        {
            SelectDarkMode();
        }

        /// <summary>
        /// When the user of the application chooses to enter light mode. 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Id_menuLightMode_Click(object sender, EventArgs e)
        {
            SelectLightMode();
        }

        // Keyboard Key Presses
        /// <summary>
        /// This is when the key on row 1 coloum 1 was clicked by the mouse
        /// </summary>
        /// <param name="sender"></param> The object that called this function
        /// <param name="e"></param> What event caused the object to call this function
        private void Id_buttonLeftR1C1_Click(object sender, EventArgs e)
        {
            Logging.Instance.Log($"Left keyboard R1 C1 key was selected by \" {sender.ToString()} \" selected by \" {e.ToString()} \" ",
                                 "Information");
            m_selectedKey = "R1C1";
            id_textboxLeftKeyValue.Text = "Row 1, Col 1";
            if (m_selectedDarkMode)
            {
                SelectDarkMode();
                id_buttonLeftR1C1.Image = Properties.Resources.WideKeyDarkSelected;
            }
            else
            {
                SelectLightMode();
                id_buttonLeftR1C1.Image = Properties.Resources.WideKeyLightSelected;
            }

        }
        /// <summary>
        /// This is when the key on row 1 coloum 2 was clicked by the mouse
        /// </summary>
        /// <param name="sender"></param> The object that called this function
        /// <param name="e"></param> What event caused the object to call this function
        private void Id_buttonLeftR1C2_Click(object sender, EventArgs e)
        {
            Logging.Instance.Log($"Left keyboard R1 C2 key was selected by \" {sender.ToString()} \" selected by \" {e.ToString()} \" ",
                                 "Information");
            m_selectedKey = "R1C2";
            id_textboxLeftKeyValue.Text = "Row 1, Col 2";
            if (m_selectedDarkMode)
            {
                SelectDarkMode();
                id_buttonLeftR1C2.Image = Properties.Resources.SingleKeyDarkSelected;
            }
            else
            {
                SelectLightMode();
                id_buttonLeftR1C2.Image = Properties.Resources.SingleKeyLightSelected;
            }
        }
        /// <summary>
        /// This is when the key on row 1 coloum 3 was clicked by the mouse
        /// </summary>
        /// <param name="sender"></param> The object that called this function
        /// <param name="e"></param> What event caused the object to call this function
        private void Id_buttonLeftR1C3_Click(object sender, EventArgs e)
        {
            Logging.Instance.Log($"Left keyboard R1 C3 key was selected by \" {sender.ToString()} \" selected by \" {e.ToString()} \" ",
                                 "Information");
            m_selectedKey = "R1C3";
            id_textboxLeftKeyValue.Text = "Row 1, Col 3";
            if (m_selectedDarkMode)
            {
                SelectDarkMode();
                id_buttonLeftR1C3.Image = Properties.Resources.SingleKeyDarkSelected;
            }
            else
            {
                SelectLightMode();
                id_buttonLeftR1C3.Image = Properties.Resources.SingleKeyLightSelected;
            }
        }

        private void Id_buttonLeftR1C4_Click(object sender, EventArgs e)
        {
            Logging.Instance.Log($"Left keyboard R1 C4 key was selected by \" {sender.ToString()} \" selected by \" {e.ToString()} \" ",
                     "Information");
            m_selectedKey = "R1C4";
            id_textboxLeftKeyValue.Text = "Row 1, Col 4";
            if (m_selectedDarkMode)
            {
                SelectDarkMode();
                id_buttonLeftR1C4.Image = Properties.Resources.SingleKeyDarkSelected;
            }
            else
            {
                SelectLightMode();
                id_buttonLeftR1C4.Image = Properties.Resources.SingleKeyLightSelected;
            }
        }

        private void Id_buttonLeftR1C5_Click(object sender, EventArgs e)
        {
            Logging.Instance.Log($"Left keyboard R1 C5 key was selected by \" {sender.ToString()} \" selected by \" {e.ToString()} \" ",
                 "Information");
            m_selectedKey = "R1C5";
            id_textboxLeftKeyValue.Text = "Row 1, Col 5";
            if (m_selectedDarkMode)
            {
                SelectDarkMode();
                id_buttonLeftR1C5.Image = Properties.Resources.SingleKeyDarkSelected;
            }
            else
            {
                SelectLightMode();
                id_buttonLeftR1C5.Image = Properties.Resources.SingleKeyLightSelected;
            }
        }
        private void Id_buttonLeftR1C6_Click(object sender, EventArgs e)
        {
            Logging.Instance.Log($"Left keyboard R1 C6 key was selected by \" {sender.ToString()} \" selected by \" {e.ToString()} \" ",
                 "Information");
            m_selectedKey = "R1C6";
            id_textboxLeftKeyValue.Text = "Row 1, Col 6";
            if (m_selectedDarkMode)
            {
                SelectDarkMode();
                id_buttonLeftR1C6.Image = Properties.Resources.SingleKeyDarkSelected;
            }
            else
            {
                SelectLightMode();
                id_buttonLeftR1C6.Image = Properties.Resources.SingleKeyLightSelected;
            }
        }

        private void Id_buttonLeftR1C7_Click(object sender, EventArgs e)
        {
            Logging.Instance.Log($"Left keyboard R1 C7 key was selected by \" {sender.ToString()} \" selected by \" {e.ToString()} \" ",
             "Information");
            m_selectedKey = "R1C7";
            id_textboxLeftKeyValue.Text = "Row 1, Col 7";
            if (m_selectedDarkMode)
            {
                SelectDarkMode();
                id_buttonLeftR1C7.Image = Properties.Resources.SingleKeyDarkSelected;
            }
            else
            {
                SelectLightMode();
                id_buttonLeftR1C7.Image = Properties.Resources.SingleKeyLightSelected;
            }
        }

        private void Id_buttonLeftR2C1_Click(object sender, EventArgs e)
        {

        }

        private void Id_buttonLeftR2C2_Click(object sender, EventArgs e)
        {

        }

        private void Id_buttonLeftR2C3_Click(object sender, EventArgs e)
        {

        }

        private void Id_buttonLeftR2C4_Click(object sender, EventArgs e)
        {

        }

        private void Id_buttonLeftR2C5_Click(object sender, EventArgs e)
        {

        }

        private void Id_buttonLeftR2C6_Click(object sender, EventArgs e)
        {

        }

        private void Id_buttonLeftR2C7_Click(object sender, EventArgs e)
        {

        }

        private void ComboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void Id_listboxLeftKeyComPort_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        /// <summary>
        /// This is when you select to 
        /// </summary>
        /// <param name="sender"> </param> 
        /// <param name="e"></param>
        private void Id_buttonLeftKeyConnectComPort_Click(object sender, EventArgs reason)
        {
            Logging.Instance.Log($"Left Connect button pressed \" {sender.ToString()} \" by \" {reason.ToString()} \" ",
                     "Information");
            String currentPort = FullPortToJustCom((String)id_comboboxLeftKeyComPort.SelectedItem);
            Logging.Instance.Log(currentPort);

            m_leftKeyConnection.PortName = currentPort;
            m_leftKeyConnection.Open();
            m_leftKeyConnection.WriteLine("Testing");
            m_leftKeyConnection.Close();
        }

        private String FullPortToJustCom(String fullPortName)
        {
            return "COM2";
        }
    }
}
