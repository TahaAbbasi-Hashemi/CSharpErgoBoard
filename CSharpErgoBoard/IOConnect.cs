//using System;
//using System.Collections;
//using System.Collections.Generic;
//using System.Management;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using System.IO.Ports;
//using System.Threading;

//namespace CSharpErgoBoard
//{
//    /// <summary>
//    /// A class with its own dedicated thread. Used for Processing serial IO between the keyboard and the computer.
//    /// </summary>
//    class IOConnect
//    {
//        // Constants
//        private const int m_maxBufferSize = 255;

//        private static SerialPort m_serialConnection = new SerialPort();
//        private static Boolean m_initalized = false;
//        private int m_baudRate = 9600;
//        private int m_dataBits = 8;
//        private Handshake m_handshake = Handshake.None; // This can cause issues on Arduino devices if enabled
//        private Parity m_pairty = Parity.None;
//        private StopBits m_stopBits = StopBits.One;
//        private int m_timeout = 500;
//        private static Queue<String> m_output = new Queue<String>();
//        private static Queue<String> m_input = new Queue<String>();
//        private static Mutex m_outputLock = new Mutex();
//        private static Mutex m_inputLock = new Mutex();
//        private Thread m_thread = new Thread(DataStream);

//        /// <summary>
//        /// Starts a serial port instance. 
//        /// </summary>
//        /// <param name="comPort"></param>
//        /// <returns>True once the port was setup.</returns>
//        public Boolean Setup(String comPort)
//        {
//            //m_serialConnection.StopBits;
//            m_serialConnection.PortName = comPort;
//            m_serialConnection.BaudRate = m_baudRate;
//            m_serialConnection.Parity = m_pairty;
//            m_serialConnection.DataBits = m_dataBits;
//            m_serialConnection.StopBits = m_stopBits;
//            m_serialConnection.Handshake = m_handshake;

//            m_serialConnection.ReadTimeout = m_timeout;
//            m_serialConnection.WriteTimeout = m_timeout;

//            m_serialConnection.Open();
//            m_initalized = m_serialConnection.IsOpen;
//            m_initalized = true;
//            m_thread.Start();


//            return m_initalized;
//        }

//        public static void DataStream()
//        {
//            while (m_initalized)
//            {
//                try
//                {
//                    //String readMessage = m_serialConnection.ReadLine();
//                    //if (readMessage.Count() > 0)
//                    //{
//                    //    m_inputLock.WaitOne();
//                    //    m_input.Enqueue(readMessage);
//                    //    m_inputLock.ReleaseMutex();
//                    //}

//                    if (m_output.Count() > 0)
//                    {
//                        m_outputLock.WaitOne();
//                        String writeMessage = m_output.Dequeue();
//                        m_outputLock.ReleaseMutex();
//                        m_serialConnection.WriteLine(writeMessage);
//                    }
//                }
//                catch (TimeoutException)
//                {
//                    ;
//                }
//            }
//        }

//        /// <summary>
//        /// Adds a System::String to the output Queue. 
//        /// </summary>
//        /// <param name="writtenLine"></param>
//        /// <returns>True if we add a string to queue, false if the string is too short or too long.</returns>
//        public bool WriteString(String writtenLine)
//        {
//            // Line is too big
//            if (writtenLine.Length > m_maxBufferSize)
//            {
//                return false;
//            }
//            // The line is empty
//            if (writtenLine.Length == 0)
//            {
//                return false;
//            }

//            m_outputLock.WaitOne();
//            m_output.Enqueue(writtenLine);
//            m_outputLock.ReleaseMutex();

//            return true;
//        }

//        public void End()
//        {
//            m_initalized = false;
//            m_thread.Join();
//        }
//    }
//}
