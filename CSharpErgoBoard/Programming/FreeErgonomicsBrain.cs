using System;
using System.Collections.Generic;
using System.Management;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO.Ports;


namespace CSharpErgoBoard.Programming
{
    class FreeErgonomicsBrain
    {
        //Private Readonly Members
        private readonly Dictionary<String, UInt32> m_conversion = new Dictionary<String, UInt32>();
        private readonly Design.MySerialPort m_leftKeyConnection = new Design.MySerialPort();
        private readonly Design.MySerialPort m_rightKeyConnection = new Design.MySerialPort();
        private readonly Design.MySerialPort m_leftLEDConnection = new Design.MySerialPort();
        private readonly Design.MySerialPort m_rightLEDConnection = new Design.MySerialPort();
        
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
            m_conversion.Add("layer 3", 3);
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
            // All 24 F keys.
            int j = 1;
            for (UInt32 i = 194; i < 252; i++)
            {
                String name = "F" + j.ToString();
                m_conversion.Add(name, i);
                j++;
            }
            // Characters
            m_conversion.Add("Space", 32);
            String fullTable = "`1234567890-=qwertyuiop[]\asdfghjkl;'zxcvbnm,./";
            foreach (Char character in fullTable)
            {
                m_conversion.Add(character.ToString(), (UInt32)character);
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
        /// Using the value type finds the correct serial port connection required
        /// </summary>
        /// <param name="type"> The type of serial port you want. </param>
        /// <param name="serialPort"> An out value, this is the serial port you are given.</param>
        /// <param name="error">An out value, indicating what went wrong</param>
        /// <returns>True if the process worked, false if it failed.</returns>
        private Boolean GetSerialPort(in String type, out Design.MySerialPort serialPort, out String error)
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
            Design.MySerialPort connectingPort;

            GetSerialPort(type, out connectingPort, out error);

            // Already connected.
            if (connectingPort.IsOpen)
            {
                error = "Connection is already made. \n" +
                    "If you wish to change connection please disconnect first. ";
                Logging.Instance.Log("Connection is already made.", "Error");
                return false;
            }

            // Make connection and find type
            Boolean rightType = false;
            try
            {
                rightType = connectingPort.MakeConnection(comPort, type);
            }
            catch (System.IO.IOException)
            {
                error = "Can not connect to the com port selected";
                Logging.Instance.Log(error, "Error");
                return false;
            }
            catch (TimeoutException)
            {
                error = "Failed to get a response from the device.\n" +
                    "Please try again or change com port.";
                Logging.Instance.Log("Failed to get a response from the device", "Error");
            }
            catch (UnauthorizedAccessException)
            {
                error = "This com port is already in use by another device";
                Logging.Instance.Log("Tried to connect to a already existing serial port", "Debug");
            }

            // Wrong type
            if (!rightType)
            {
                Logging.Instance.Log("Connection was made, but wrong information gotten", "Debug");
                Logging.Instance.Log(connectingPort.Type, "Debug");
                connectingPort.Close();

                if (connectingPort.Type == "NA")
                {
                    error = "No response was gotten from the port.\n" +
                        "Please make sure this is the correct port or try again.";

                }
                else
                {
                    error = "This port is a :" + connectingPort.Type + "\n" +
                        "Please try a different connection for a :" + type + "connection.";
                }
                return false;
            }
            Logging.Instance.Log("A serial port connection for '" + type + "' was made at '" + comPort + "'", "Success");

            //// Return
            //if (type == "Left Keyboard")
            //{
            //    m_leftKeyConnection = connectingPort;
            //}
            //else if (type == "Right Keyboard")
            //{
            //    m_rightKeyConnection = connectingPort;
            //}
            //else if (type == "Left LED")
            //{
            //    m_leftLEDConnection = connectingPort;
            //}
            //else if (type == "Right LED")
            //{
            //    m_rightLEDConnection = connectingPort;
            //}

            return true;
        }
        /// <summary>
        /// Checks to see if the serial port is already connected
        /// </summary>
        /// <param name="type">The type of serial port connection wanted</param>
        /// <returns>True if a connection is already made, false if no connection is made.</returns>
        public Boolean IsConnected(in String type)
        {
            Design.MySerialPort connectingPort;

            GetSerialPort(type, out connectingPort, out String error);
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
        /// Sends a update command to the controller to update the 
        /// </summary>
        /// <param name="type"> The type of serial port connection wanted. EG: Left keyboard, right keyboard</param>
        /// <param name="layer">What layer the updated key is going on. </param>
        /// <param name="key">What is the name of the keyboard key.</param>
        /// <param name="value">What is the value of the new keyboard key.</param>
        /// <returns>True if a update was sent to the controller.</returns>
        public Boolean Update(in String type, in String layer, in String key, in String value)
        {
            Design.MySerialPort connectingPort;
            String message;

            GetSerialPort(type, out connectingPort, out String error);

            message = "U";  // The update character
            message += key; // Key contains the row and column in a R1C1 fashion. 
            if (layer == "Layer 1")
            {
                message += "L1";
            }
            else if (layer == "Layer 2")
            {
                message += "L2";
            }
            else if (layer == "Layer 3")
            {
                message += "L3";
            }
            else if (layer == "Layer 4")
            {
                message += "L4";
            }
            else if (layer == "Layer 5")
            {
                message += "L5";
            }
            message += "'";
            message += value;
            message += "'";

            connectingPort.WriteLine(message);

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
