using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO.Ports;
using System.Threading;

namespace CSharpErgoBoard.Design
{
    class MySerialPort : SerialPort
    {
        private String m_type = "NA";
        /// <summary>
        /// The type of controller currently connected to the serial port. Before a connection is made this is "NA"
        /// </summary>
        public string Type { get => m_type; }

        /// <summary>
        /// The default constructor
        /// </summary>
        /// <remarks>
        /// A problem found is that the arduino leonardo requires a DTR enable, while the arduino UNO does not. 
        /// </remarks>
        /// <param name="dtr">A boolean value representing if DTR is enabled.</param>
        public MySerialPort(in Boolean dtr)
        {
            ReadTimeout = 1000;
            BaudRate = 9600;
            Parity = Parity.None;
            DataBits = 8;
            StopBits = StopBits.One;
            Handshake = Handshake.None;
            DtrEnable = dtr;
            //ReceivedBytesThreshold = 1;
        }
        /// <summary>
        /// Set up a serial port connection to a controller and make sure the contorller is the expected controller.
        /// </summary>
        /// <param name="port"> The COM that the controller is connected to.</param>
        /// <param name="wantedType"> What is the expected type of the controller</param>
        /// <returns>True if the type of controller is the same as the wanted type. False if anything goes wrong.</returns>
        public Boolean MakeConnection(in String port, in String wantedType)
        {
            PortName = port;
            Open();
            WriteLine("Name.\n");
            Thread.Sleep(150);
            m_type = ReadLine();

            if (m_type.Contains(wantedType))
            {
                return true;
            }
            
            return false;
        }
    }
}
