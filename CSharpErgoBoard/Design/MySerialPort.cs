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

        public string Type { get => m_type; }

        public MySerialPort()
        {
            ReadTimeout = 1000;
            BaudRate = 9600;
            //Parity = Parity.None;
            DataBits = 8;
            StopBits = StopBits.One;
            Handshake = Handshake.None;
            ReceivedBytesThreshold = 1;
        }

        public Boolean MakeConnection(in String port, in String wantedType)
        {
            PortName = port;
            Open();

            WriteLine("Name");
            Thread.Sleep(5000);
            m_type = ReadLine();

            if (m_type.Contains(wantedType))
            {
                return true;
            }

            return false;
        }
    }
}
