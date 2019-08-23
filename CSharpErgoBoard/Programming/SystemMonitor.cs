// Using
using System;
using System.Collections.Generic;
using System.Threading;
using OpenHardwareMonitor.Hardware;


namespace CSharpErgoBoard.Programming
{
    /// <summary>
    /// A class which runs and records the properties of the PC. 
    /// </summary>
    /// <remarks>
    /// The class is a singleton class, this allows for it to exist only once and be universal in its use. 
    /// This class calls on the logging class to updates.
    /// </remarks>
    class SystemMonitor : Singleton
    {
        // Class Atributes
        /// <summary>
        /// The name of the class
        /// </summary>
        public new String Name { get; } = "System Monitor";
        /// <summary>
        /// The purpose of the class
        /// </summary>
        public new String Purpose { get; } = "To store the propertes of the hardware";
        /// <summary>
        /// To convert the class to a string.
        /// </summary>
        public new String ToString { get; } = "A System Monitoring Class";

        // Private Encapsulated Variables
        private static Boolean m_AMD = false;
        private static Int16 m_cpuCores = 0;  // This is expected to be greater than or equal to 1. 
        private static readonly List<Double> m_cpuClock = new List<Double>();
        private static readonly List<Double> m_cpuLoad = new List<Double>();
        private static readonly List<Double> m_cpuTemp = new List<Double>();
        private static readonly List<Double> m_gpuClock = new List<Double>();
        private static readonly List<Double> m_gpuLoad = new List<Double>();
        private static readonly List<Double> m_gpuTemp = new List<Double>();
        private static readonly List<Double> m_ramLoad = new List<Double>();
        private static readonly List<Double> m_hddLoad = new List<Double>();
        protected new static SystemMonitor m_instance = null;

        // Readonly Private Variables
        /// <summary>
        /// The thread for the singleton
        /// </summary>
        protected new static readonly Thread m_thread = new Thread(ThreadFunction);
        /// <summary>
        /// To ensure that the singleton is thread safe
        /// </summary>
        protected new static readonly Object m_padlock = new Object();
        /// <summary>
        /// The computer being used for finding hardware values
        /// </summary>
        private static readonly Computer m_computer = new Computer();
        /// <summary>
        /// The thread safe lock for updating.
        /// </summary>
        private static readonly Mutex m_updateLock = new Mutex();

        // Encapulation Functions
        /// <summary>
        /// Gets the total amount of CPU cores found in a system. 
        /// </summary>
        public static Int16 CpuCores { get => m_cpuCores; }
        /// <summary>
        /// Get the Clock speed of each CPU core in a list, with the first value in the list being the total CPU clock speed
        /// </summary>
        public static List<Double> CpuClock
        {
            get
            {
                m_updateLock.WaitOne();
                List<Double> temp = m_cpuClock;
                m_updateLock.ReleaseMutex();

                return temp;
            }
        }
        /// <summary>
        /// Get the load of each CPU core in a list
        /// </summary>
        public static List<Double> CpuLoad
        {
            get
            {
                m_updateLock.WaitOne();
                List<Double> temp = m_cpuLoad;
                m_updateLock.ReleaseMutex();

                return temp;
            }
        }
        /// <summary>
        /// Get the temperture of each CPU core in a list
        /// </summary>
        public static List<Double> CpuTemp
        {
            get
            {
                m_updateLock.WaitOne();
                List<Double> temp = m_cpuTemp;
                m_updateLock.ReleaseMutex();

                return temp;
            }
        }
        /// <summary>
        /// Get the clock speed of the graphic cards
        /// </summary>
        public static List<Double> GpuClock
        {
            get
            {
                m_updateLock.WaitOne();
                List<Double> temp = m_gpuClock;
                m_updateLock.ReleaseMutex();

                return temp;
            }
        }
        /// <summary>
        /// Get the load of the graphics card
        /// </summary>
        public static List<Double> GpuLoad
        {
            get
            {
                m_updateLock.WaitOne();
                List<Double> temp = m_gpuLoad;
                m_updateLock.ReleaseMutex();

                return temp;
            }
        }
        /// <summary>
        /// Get the temperture of the graphics card
        /// </summary>
        public static List<Double> GpuTemp
        {
            get
            {
                m_updateLock.WaitOne();
                List<Double> temp = m_gpuTemp;
                m_updateLock.ReleaseMutex();

                return temp;
            }
        }
        /// <summary>
        /// Get the load of the ram
        /// </summary>
        public static List<Double> RamLoad
        {
            get
            {
                m_updateLock.WaitOne();
                List<Double> temp = m_ramLoad;
                m_updateLock.ReleaseMutex();

                return temp;
            }
        }
        /// <summary>
        /// Get the load of the hard disk drive
        /// </summary>
        public static List<Double> HddLoad
        {
            get
            {
                m_updateLock.WaitOne();
                List<Double> temp = m_hddLoad;
                m_updateLock.ReleaseMutex();

                return temp;
            }
        }
        /// <summary>
        /// Controls if the program uses a Nvidea or AMD gpu. 
        /// </summary>
        /// <value>
        ///     <c>true</c> if we are using a AMD GPU; <c>false</c> if we are using a Nvidea GPU.
        /// </value>
        public static Boolean AMD
        {
            get
            {
                m_updateLock.WaitOne();
                Boolean temp = m_AMD;
                m_updateLock.ReleaseMutex();

                return temp;
            }
            set
            {
                Boolean temp = value;

                m_updateLock.WaitOne();
                m_AMD = temp;
                m_updateLock.ReleaseMutex();
            }
        }
        /// <summary>
        /// This is the instance of the singleton class. Any commands must be called using  
        /// </summary>
        /// <remarks>
        /// A singleton class has one or no instances. In order to use the instance this must be called. 
        /// This also starts the threading and monitoring process of the class.
        /// </remarks>
        public new static SystemMonitor Instance
        {
            get
            {
                if (m_instance == null)
                {
                    lock (m_padlock)
                    {
                        if (m_instance == null)
                        {
                            m_instance = new SystemMonitor();
                            m_running = true;
                            m_computer.Open();
                            m_thread.Start();
                        }
                    }
                }
                return m_instance;
            }
        }

        // Functions
        static SystemMonitor() { }

        /// <summary>
        /// Updates the computer hardware values stored on our end. 
        /// </summary>
        public static void UpdateValues()
        {
            Programming.Logging.Instance.Log("A update was run");

            try
            {
                m_computer.CPUEnabled = true;
                m_computer.GPUEnabled = true;
                m_computer.HDDEnabled = true;
                m_computer.RAMEnabled = true;

                for (Int32 i = 0; i < m_computer.Hardware.Length; i++)
                {
                    // CPU
                    if (m_computer.Hardware[i].HardwareType == HardwareType.CPU)
                    {
                        m_cpuClock.Clear();
                        m_cpuLoad.Clear();
                        m_cpuTemp.Clear();
                        for (Int32 j = 0; j < m_computer.Hardware[i].Sensors.Length; j++)
                        {
                            if (m_computer.Hardware[i].Sensors[j].SensorType == SensorType.Clock)
                            {
                                m_cpuClock.Add((float)(m_computer.Hardware[i].Sensors[j].Value));
                            }
                            else if (m_computer.Hardware[i].Sensors[j].SensorType == SensorType.Load)
                            {
                                m_cpuLoad.Add((float)(m_computer.Hardware[i].Sensors[j].Value));
                            }
                            else if (m_computer.Hardware[i].Sensors[j].SensorType == SensorType.Temperature)
                            {
                                m_cpuTemp.Add((float)(m_computer.Hardware[i].Sensors[j].Value));
                            }
                        }
                    }
                    // AMD GPU
                    else if ((m_computer.Hardware[i].HardwareType == HardwareType.GpuAti) && m_AMD)
                    {
                        m_gpuClock.Clear();
                        m_gpuLoad.Clear();
                        m_gpuTemp.Clear();
                        for (Int32 j = 0; j < m_computer.Hardware[i].Sensors.Length; j++)
                        {
                            if (m_computer.Hardware[i].Sensors[j].SensorType == SensorType.Clock)
                            {
                                m_gpuClock.Add((float)(m_computer.Hardware[i].Sensors[j].Value));
                            }
                            else if (m_computer.Hardware[i].Sensors[j].SensorType == SensorType.Load)
                            {
                                m_gpuLoad.Add((float)(m_computer.Hardware[i].Sensors[j].Value));
                            }
                            else if (m_computer.Hardware[i].Sensors[j].SensorType == SensorType.Temperature)
                            {
                                m_gpuTemp.Add((float)(m_computer.Hardware[i].Sensors[j].Value));
                            }
                        }
                    }
                    // Nvidea GPU
                    else if ((m_computer.Hardware[i].HardwareType == HardwareType.GpuNvidia) && m_AMD)
                    {
                        m_gpuClock.Clear();
                        m_gpuLoad.Clear();
                        m_gpuTemp.Clear();
                        for (Int32 j = 0; j < m_computer.Hardware[i].Sensors.Length; j++)
                        {
                            if (m_computer.Hardware[i].Sensors[j].SensorType == SensorType.Clock)
                            {
                                m_gpuClock.Add((float)(m_computer.Hardware[i].Sensors[j].Value));
                            }
                            else if (m_computer.Hardware[i].Sensors[j].SensorType == SensorType.Load)
                            {
                                m_gpuLoad.Add((float)(m_computer.Hardware[i].Sensors[j].Value));
                            }
                            else if (m_computer.Hardware[i].Sensors[j].SensorType == SensorType.Temperature)
                            {
                                m_gpuTemp.Add((float)(m_computer.Hardware[i].Sensors[j].Value));
                            }
                        }
                    }
                    // RAM
                    else if (m_computer.Hardware[i].HardwareType == HardwareType.RAM)
                    {
                        m_ramLoad.Clear();
                        for (Int32 j = 0; j < m_computer.Hardware[i].Sensors.Length; j++)
                        {
                            if (m_computer.Hardware[i].Sensors[j].SensorType == SensorType.Load)
                            {
                                //m_ramLoad.Add((float)(m_computer.Hardware[i].Sensors[j].Value));
                            }
                        }
                    }
                    // HDD
                    else if (m_computer.Hardware[i].HardwareType == HardwareType.HDD)
                    {
                        m_hddLoad.Clear();
                        for (Int32 j = 0; j < m_computer.Hardware[i].Sensors.Length; j++)
                        {
                            if (m_computer.Hardware[i].Sensors[j].SensorType == SensorType.Load)
                            {
                                //m_hddLoad.Add((float)(m_computer.Hardware[i].Sensors[j].Value));
                            }
                        }
                    }
                }

                m_computer.CPUEnabled = false;
                m_computer.GPUEnabled = false;
                m_computer.HDDEnabled = false;
                m_computer.RAMEnabled = false;
            }
            catch (NullReferenceException)
            {
                ;
            }
        }

        /// <summary>
        /// This is the function that does the monitoring process.
        /// </summary>
        protected new static void ThreadFunction()
        {
            m_computer.CPUEnabled = true;

            // Gets the total amount of CPU cores. 
            for (Int32 i = 0; i < m_computer.Hardware.Length; i++)
            {
                // Pick CPU hardware
                if (m_computer.Hardware[i].HardwareType != HardwareType.CPU)
                {
                    continue;
                }
                for (Int32 j = 0; j < m_computer.Hardware[i].Sensors.Length; j++)
                {
                    // Pick random CPU sensor. All cores should have a temp. 
                    if (m_computer.Hardware[i].Sensors[j].SensorType != SensorType.Temperature)
                    {
                        continue;
                    }
                    m_cpuCores += 1;
                }
            }

            m_computer.CPUEnabled = false;

            // Thread Loop
            while (m_running)
            {
                m_updateLock.WaitOne();
                UpdateValues();
                m_updateLock.ReleaseMutex();
                
                // Every 100ms check to make sure we aren't shut down. 
                // Every 5s update the values.
                for (UInt16 i = 0; i < 50; i ++)
                {
                    if (!m_running)
                    {
                        break;
                    }
                    Thread.Sleep(100);
                }
            }
        }

        /// <summary>
        /// Ends the monitoring process, This must be called or the program will not close.
        /// </summary>
        public override void End()
        {
            m_running = false;
            m_computer.Close();
            m_thread.Join();
        }

        /// <summary>
        /// To ensure that the system monitoring process has started.
        /// </summary>
        public void Existance() { }
    }
}
