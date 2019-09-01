using System;
using System.Collections.Generic;
using System.Management;
using CSharpErgoBoard.Design;
using CSharpErgoBoard.Properties;


namespace CSharpErgoBoard.Programming
{
    class FreeErgonomicsBrain
    {
        //Private Readonly Members
        private readonly Dictionary<String, UInt32> m_conversion = new Dictionary<String, UInt32>();
        private readonly Dictionary<UInt32, String> m_reverseConversion = new Dictionary<UInt32, String>();
        private readonly MySerialPort m_leftKeyConnection = new MySerialPort(true);
        private readonly MySerialPort m_rightKeyConnection = new MySerialPort(true);
        private readonly MySerialPort m_leftLEDConnection = new MySerialPort(false);
        private readonly MySerialPort m_rightLEDConnection = new MySerialPort(false);

        public Dictionary<String, UInt32> Conversion => m_conversion;
        public Dictionary<UInt32, String> ReverseConversion => m_reverseConversion;

        /// <summary>
        /// Programming.FreeErgonomicsBrain Class error.
        /// </summary>
        [Serializable()]
        private class FreeErgonomicsBrainError : Exception
        {
            // Functions
            /// <summary>
            /// Default constructor
            /// </summary>
            public FreeErgonomicsBrainError() : base() { }
            /// <summary>
            /// Constructor with a message.
            /// </summary>
            /// <param name="message"> The message being reported to the user.</param>
            public FreeErgonomicsBrainError(in String message) : base(message) { }
            /// <summary>
            /// A Constructor with a inner error
            /// </summary>
            /// <param name="message"> The message being reported.</param>
            /// <param name="inner"> The error that progogated this error.</param>
            public FreeErgonomicsBrainError(in String message, in Exception inner) : base(message, inner) { }
        }

        // Functions.
        /// <summary>
        /// Default constructor. 
        /// </summary>
        /// <remarks>
        /// This is used as a way of ensuring all the proper values are saved and stored. 
        /// This adds all the conversion values to the conversion table.
        /// </remarks>
        public FreeErgonomicsBrain()
        {
            // Layers
            m_conversion.Add("None", 0);
            m_conversion.Add("Layer 1", 1);
            m_conversion.Add("Layer 2", 2);
            m_conversion.Add("Layer 3", 3);
            m_conversion.Add("Layer 4", 4);
            m_conversion.Add("Layer 5", 5);
            // Commands
            m_conversion.Add("Left Ctrl", 128);
            m_conversion.Add("Left Shift", 129);
            m_conversion.Add("Left Alt", 130);
            m_conversion.Add("Left Win", 131);
            m_conversion.Add("Right Ctrl", 132);
            m_conversion.Add("Right Shift", 133);
            m_conversion.Add("Right Alt", 134);
            m_conversion.Add("Right Win", 135);
            m_conversion.Add("Up Arrow", 218);
            m_conversion.Add("Down Arrow", 217);
            m_conversion.Add("Left Arrow", 216);
            m_conversion.Add("Right Arrow", 215);
            m_conversion.Add("Backspace", 178);
            m_conversion.Add("Tab", 179);
            m_conversion.Add("Return", 176);
            m_conversion.Add("Esc", 177);
            m_conversion.Add("Insert", 209);
            m_conversion.Add("Delete", 212);
            m_conversion.Add("Page Up", 211);
            m_conversion.Add("Page Down", 214);
            m_conversion.Add("Home", 210);
            m_conversion.Add("End", 213);
            m_conversion.Add("Caps Lock", 193);
            m_conversion.Add("Space", 32);
            // All 24 F keys.
            int j = 1;
            for (UInt32 i = 194; i < 206; i++)
            {
                String name = "F" + j.ToString();
                m_conversion.Add(name, i);
                j++;
            }
            // Characters
            String fullTable = "`1234567890-=qwertyuiop[]\\asdfghjkl;'zxcvbnm,./~!@#$%^&*()_+QWERTYUIOP{}|ASDFGHJKL:\"ZXCVBNM<>?";
            foreach (Char character in fullTable)
            {
                m_conversion.Add(character.ToString(), (UInt32)character);
            }

            // Make reverse conversion 
            foreach (KeyValuePair<String, UInt32> group in m_conversion)
            {
                try
                {
                    m_reverseConversion.Add(group.Value, group.Key);
                }
                catch (ArgumentException)
                {
                    new Popup("I am not sure what just happened", group.Key);
                }
            }
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
        /// Converts the comport with the description to just the com port.
        /// </summary>
        /// <param name="comPort">The com port being used. EG: "Arduino Leonardo (COM4)"</param>
        /// <exception cref="FreeErgonomicsBrainError"> An error is thrown when the right serial port can not be found. This can happen if you select a printer port.</exception>
        /// <returns>Just the com port, from the example above it would return "COM4"</returns>
        private String JustComPort(in String comPort)
        {
            foreach (String com in MySerialPort.GetPortNames())
            {
                Logging.Instance.Log(com);
                if (comPort.Contains("(" + com + ")"))
                {
                    return com;
                }
            }
            throw new FreeErgonomicsBrainError("Could not find the right serial port");
        }
        /// <summary>
        /// Using the value type finds the correct serial port connection required
        /// </summary>
        /// <param name="type"> The type of serial port you want. </param>
        /// <param name="serialPort"> An out value, this is the serial port you are given.</param>
        /// <param name="error">An out value, indicating what went wrong</param>
        /// <returns>True if the process worked, false if it failed.</returns>
        private Boolean GetSerialPort(in String type, out MySerialPort serialPort, out String error)
        {
            error = null;
            serialPort = null;

            if (type == "Left Keyboard")
            {
                serialPort = m_leftKeyConnection;
            }
            else if (type == "Right Keyboard")
            {
                serialPort = m_rightKeyConnection;
            }
            else if (type == "Left LED")
            {
                serialPort = m_leftLEDConnection;
            }
            else if (type == "Right LED")
            {
                serialPort = m_rightLEDConnection;
            }
            else
            {
                error = "Invalid type provided for connection";
                Logging.Instance.Log(error);
                throw new FreeErgonomicsBrainError(error);
            }

            return true;
        }
        /// <summary>
        /// Attempts to connect to the selected serial port type.
        /// </summary>
        /// <param name="type">The type of serial port connection wanted</param>
        /// <param name="comPort">The communication port the serial connection is on.</param>
        /// <param name="error">An out value, indicating what went wrong. Null if everything worked out correctly</param>
        /// <returns>True if a connection was made, false if a connection was not made.</returns>
        public Boolean Connect(in String type, in String comPort, out String error)
        {
            error = null;

            GetSerialPort(type, out MySerialPort connectingPort, out error);

            for (Byte i = 0; i < 1; i++)
            {   // A for loop that happens once.
                // Already connected.
                if (connectingPort.IsOpen)
                {
                    error = "Connection is already made. \n" +
                        "If you wish to change connection please disconnect first. ";
                    Logging.Instance.Log("Connection is already made.", "Error");
                    break;
                }

                // Make connection and find type
                Boolean rightType = false;
                try
                {
                    rightType = connectingPort.MakeConnection(JustComPort(comPort), type);
                }
                catch (System.IO.IOException)
                {
                    error = "Can not connect to the com port selected";
                    Logging.Instance.Log(error, "Error");
                    break;
                }
                catch (TimeoutException)
                {
                    error = "Failed to get a response from the device.\n" +
                        "Please try again or change com port.";
                    Logging.Instance.Log("Failed to get a response from the device", "Error");
                    break;
                }
                catch (UnauthorizedAccessException)
                {
                    error = "This com port is already in use by another device";
                    Logging.Instance.Log("Tried to connect to a already existing serial port", "Debug");
                    break;
                }
                catch (NullReferenceException)
                {
                    error = "No COM port has been selected\n" +
                        "Please select one and try again.";
                    Logging.Instance.Log("Attempt to connect without com port was made", "Debug");
                    break;
                }
                catch (FreeErgonomicsBrainError)
                {
                    error = "You may have selected a printer port.\n" +
                        "Please make sure you have a com port selected";
                    Logging.Instance.Log("A not found port name was selected.", "Error");
                    break;
                }

                // Wrong type
                if (!rightType)
                {
                    Logging.Instance.Log("Connection was made, but wrong information gotten", "Debug");
                    Logging.Instance.Log(connectingPort.Type, "Debug");
                    connectingPort.Close();

                    if (connectingPort.Type == null)
                    {
                        error = "No response was gotten from the port.\n" +
                            "Please make sure this is the correct port or try again.";
                    }
                    else
                    {
                        error = "This port is a :" + connectingPort.Type + "\n" +
                            "Please try a different connection for a :" + type + " connection.";
                    }
                    break;
                }
            }

            if (error == null)
            {
                Logging.Instance.Log("A serial port connection for '" + type + "' was made at '" + comPort + "'", "Success");

                return true;
            }
            else
            {
                connectingPort.Close();
                return false;
            }
        }
        /// <summary>
        /// Checks to see if the serial port is already connected
        /// </summary>
        /// <param name="type">The type of serial port connection wanted</param>
        /// <returns>True if a connection is already made, false if no connection is made.</returns>
        public Boolean IsConnected(in String type)
        {
            GetSerialPort(type, out MySerialPort connectingPort, out String error);
            try
            {
                return connectingPort.IsOpen;
            }
            catch (InvalidOperationException)
            {
                // The port is closed.
                return false;
            }
        }
        /// <summary>
        /// Sends a set command to the controller to update keys or leds.
        /// </summary>
        /// <remarks>
        /// Originally all the error checking was done in the GUI class this function is meant to be a inbetween the real update function
        /// and the GUI. This function just sets up the real update function as well as doing all the error checking
        /// so that the GUI is dedicated to GUI processing.
        /// </remarks>
        /// <param name="type"> The type of controller you are trying to communate with</param>
        /// <param name="layer"> What layer you want to edit the controls on. </param>
        /// <param name="key"> The button that is selected</param>
        /// <param name="value">The value that is going to be edited</param>
        /// <param name="error">If anything goes wrong it would be printed out here and used for a popup box.</param>
        /// <returns>True if everything worked, false if something goes wrong exactly what happened would be in error.</returns>
        public Boolean Update(in String type, in MyComboBox layer, in KeyButton key, in MyComboBox value, out String error)
        {
            error = null;
            if (!IsConnected(type))
            {
                error = "A connection must be made before a key can be updated.";
                Logging.Instance.Log("Attempt to update before making connection was made", "Debug");
                return false;
            }
            if (layer.SelectedIndex == -1)
            {
                error = "A layer must be selected before a key can be updated.";
                Logging.Instance.Log("Attempt to update before selecting a layer was made", "Debug");
                return false;
            }
            if (key == null)
            {
                error = "A key must be selected before a key can be updated";
                return false;
            }
            if (value.SelectedIndex == -1)
            {
                error = "A key value must be selected before a key can be updated";
                return false;
            }

            String message;
            if (!GetSerialPort(type, out MySerialPort connectingPort, out error))
            {
                return false;
            }

            message = "Set";  // The update character
            message += key.KeyName;
            message += "-";
            Conversion.TryGetValue((String)value.SelectedItem, out UInt32 uintValue);
            message += uintValue.ToString();
            Logging.Instance.Log(message);

            connectingPort.WriteLine(message);

            return true;
        }
        public Boolean ReloadKey(in String type, in MyComboBox layer, in KeyButton key, out String error, out String text)
        {
            text = null;
            error = null;
            String message;

            if (!IsConnected(type))
            {
                error = "A connection must be made before a key can be updated.";
                Logging.Instance.Log("Attempt to update before making connection was made", "Debug");
                return false;
            }
            if (layer.SelectedIndex == -1)
            {
                error = "A layer must be selected before a key can be updated.";
                Logging.Instance.Log("Attempt to update before selecting a layer was made", "Debug");
                return false;
            }
            if (!GetSerialPort(type, out MySerialPort connectingPort, out error))
            {
                return false;
            }

            message = "Get";  // The update character
            message += key.KeyName; ;
            connectingPort.WriteLine(message);

            try
            {
                message = String.Empty;
                message = connectingPort.ReadLine();
                message = message.Trim();

                UInt32 number = UInt32.Parse(message);
                message = String.Empty;
                m_reverseConversion.TryGetValue(number, out message);
            }
            catch (TimeoutException)
            {
                error = "A Key value could not be found.";
                return false;
            }
            text = message;
            connectingPort.DiscardInBuffer();

            return true;
        }
        /// <summary>
        /// Closes all the serial port connections. This should always be called otherwise issues do happen.
        /// </summary>
        public void Close()
        {
            m_leftKeyConnection.Close();
            m_rightKeyConnection.Close();
            m_leftLEDConnection.Close();
            m_rightLEDConnection.Close();
        }

    }
}
