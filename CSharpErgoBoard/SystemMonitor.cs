// Using
using System.Collections.Generic;
using System.Threading;
using OpenHardwareMonitor.Hardware;


namespace CSharpErgoBoard
{
    /// <summary>
    /// A class which runs and records the properties of the PC. 
    /// </summary>
    /// <remarks>
    /// The class is a singleton class, this allows for it to exist only once and be universal in its use. 
    /// </remarks>
    public class SystemMonitor
    {
        // Private Encapsulated Variables
        private static bool m_AMD = false;
        private static int m_cpuCores = 0;  // This is expected to be greater than or equal to 1. 
        private static readonly List<float> m_cpuClock = new List<float>();
        private static readonly List<float> m_cpuLoad = new List<float>();
        private static readonly List<float> m_cpuTemp = new List<float>();
        private static readonly List<float> m_gpuClock = new List<float>();
        private static readonly List<float> m_gpuLoad = new List<float>();
        private static readonly List<float> m_gpuTemp = new List<float>();
        private static readonly List<float> m_ramLoad = new List<float>();
        private static readonly List<float> m_hddLoad = new List<float>();

        // Purely Private Variables
        private static bool m_running = false;
        private readonly Thread m_thread = new Thread(UpdateValues);
        private static readonly Computer m_computer = new Computer();

        /// <summary>
        /// Gets the total amount of CPU cores found in a system. 
        /// </summary>
        public static int CpuCores { get => m_cpuCores; }
        /// <summary>
        /// Get the Clock speed of each CPU core in a list, with the first value in the list being the total CPU clock speed
        /// </summary>
        public static List<float> CpuClock { get => m_cpuClock; }
        /// <summary>
        /// Get the load of each CPU core in a list
        /// </summary>
        public static List<float> CpuLoad { get => m_cpuLoad; }
        /// <summary>
        /// Get the temperture of each CPU core in a list
        /// </summary>
        public static List<float> CpuTemp { get => m_cpuTemp; }
        /// <summary>
        /// Get the clock speed of the graphic cards
        /// </summary>
        public static List<float> GpuClock { get => m_gpuClock; }
        /// <summary>
        /// Get the load of the graphics card
        /// </summary>
        public static List<float> GpuLoad { get => m_gpuLoad; }
        /// <summary>
        /// Get the temperture of the graphics card
        /// </summary>
        public static List<float> GpuTemp { get => m_gpuTemp; }
        /// <summary>
        /// Get the load of the ram
        /// </summary>
        public static List<float> RamLoad { get => m_ramLoad; }
        /// <summary>
        /// Get the load of the hard disk drive
        /// </summary>
        public static List<float> HddLoad { get => m_hddLoad; }

        /// <summary>
        /// Controls if the program uses a Nvidea or AMD gpu. 
        /// </summary>
        /// <value>
        ///     <c>true</c> if we are using a AMD GPU; <c>false</c> if we are using a Nvidea GPU.
        /// </value>
        public static bool AMD { get => m_AMD; set => m_AMD = value; }


        public SystemMonitor()
        {
            m_running = true;
            m_computer.Open();
            m_thread.Start();
        }

        /// <summary>
        /// Ends the monitoring process, This must be called or the program will not close.
        /// </summary>
        public void End()
        {
            m_running = false;
            m_computer.Close();
            m_thread.Join();
        }

        /// <summary>
        /// Updates the computer hardware values stored on our end. 
        /// </summary>
        private static void UpdateValues()
        {
            m_computer.CPUEnabled = true;
            // Gets the total amount of CPU cores. 
            for (int i = 0; i < m_computer.Hardware.Length; i++)
            {
                // Pick CPU hardware
                if (m_computer.Hardware[i].HardwareType != HardwareType.CPU)
                {
                    continue;
                }
                for (int j = 0; j < m_computer.Hardware[i].Sensors.Length; j++)
                {
                    // Pick random CPU sensor. All cores should have a temp. 
                    if (m_computer.Hardware[i].Sensors[j].SensorType != SensorType.Temperature)
                    {
                        continue;
                    }
                    m_cpuCores += 1;
                }
            }


            m_computer.GPUEnabled = true;
            m_computer.HDDEnabled = true;
            m_computer.RAMEnabled = true;

            while (m_running)
            {
                Thread.Sleep(1000); // Sleep for a whole second. We want information updates once a second. 

                for (int i = 0; i < m_computer.Hardware.Length; i++)
                {
                    // CPU
                    if (m_computer.Hardware[i].HardwareType == HardwareType.CPU)
                    {
                        m_cpuClock.Clear();
                        m_cpuLoad.Clear();
                        m_cpuTemp.Clear();
                        for (int j = 0; j < m_computer.Hardware[i].Sensors.Length; j++)
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
                    // AMD Gpu
                    else if ((m_computer.Hardware[i].HardwareType == HardwareType.GpuAti) && m_AMD)
                    {
                        m_gpuClock.Clear();
                        m_gpuLoad.Clear();
                        m_gpuTemp.Clear();
                        for (int j = 0; j < m_computer.Hardware[i].Sensors.Length; j++)
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
                    else if ((m_computer.Hardware[i].HardwareType == HardwareType.GpuNvidia) && m_AMD)
                    {
                        m_gpuClock.Clear();
                        m_gpuLoad.Clear();
                        m_gpuTemp.Clear();
                        for (int j = 0; j < m_computer.Hardware[i].Sensors.Length; j++)
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
                    else if (m_computer.Hardware[i].HardwareType == HardwareType.RAM)
                    {
                        m_ramLoad.Clear();
                        for (int j = 0; j < m_computer.Hardware[i].Sensors.Length; j++)
                        {
                            if (m_computer.Hardware[i].Sensors[j].SensorType == SensorType.Load)
                            {
                                //m_ramLoad.Add((float)(m_computer.Hardware[i].Sensors[j].Value));
                            }

                        }

                    }
                    else if (m_computer.Hardware[i].HardwareType == HardwareType.HDD)
                    {
                        m_hddLoad.Clear();
                        for (int j = 0; j < m_computer.Hardware[i].Sensors.Length; j++)
                        {
                            if (m_computer.Hardware[i].Sensors[j].SensorType == SensorType.Load)
                            {
                                //m_hddLoad.Add((float)(m_computer.Hardware[i].Sensors[j].Value));
                            }

                        }

                    }
                }

            }

            m_computer.CPUEnabled = false;
            m_computer.GPUEnabled = false;
            m_computer.HDDEnabled = false;
            m_computer.RAMEnabled = false;

        }

        private static void ThreadFunction()
        {

        }
    }
}
