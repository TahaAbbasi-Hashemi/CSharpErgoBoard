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

        public FreeErgonomicsBrain()
        {

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
                return false;
                ;
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

        public Boolean Update(in String type, in String layer, in String key, in String value)
        {
            Design.MySerialPort connectingPort;
            String message;

            GetSerialPort(type, out connectingPort, out String error);

            message = key;
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

        public void Close()
        {
            m_leftKeyConnection.Close();
            m_rightKeyConnection.Close();
            m_leftLEDConnection.Close();
            m_rightLEDConnection.Close();
        }
    }
}
