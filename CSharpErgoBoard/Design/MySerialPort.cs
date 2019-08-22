using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO.Ports;

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
            Parity = Parity.None;
            DataBits = 8;
            StopBits = StopBits.One;
            Handshake = Handshake.None;
        }

        public Boolean MakeConnection(in String port, in String wantedType)
        {
            PortName = port;
            Open();

            WriteLine("Name");

            m_type = ReadLine();
            if (m_type == wantedType)
            {
                return true;
            }

            return false;
        }
    }
}
