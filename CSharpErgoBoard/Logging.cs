// Using
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;


namespace CSharpErgoBoard
{
    /// <summary>
    /// A basic data class used to store all of the information that can be used in the logging process
    /// </summary>
    public class LogData
    {
        /// <summary>
        /// This is the default constructor. This only exists to ensure that the program can run as expected.
        /// </summary>
        public LogData() { }
        // Private Encapsulated Variables
        private String m_message = "Empty";
        private String m_severity = "";
        private String m_time = "";
        private String m_date = "";
        private String m_threadName = "";
        private String m_memberName = "";
        private String m_fileName = "";
        private String m_lineNumber = "";

        // Encapsulation Functions
        /// <summary>
        /// Each log must have a message, the message describes the purpose of the logs existance or importance
        /// </summary>
        public String Message { get => m_message; set => m_message = value; }
        /// <summary>
        /// The date is the date at which the log was made. 
        /// </summary>
        public String Date { get => m_date; set => m_date = value; }
        /// <summary>
        /// The thread name is the name of the thread that made the log, 
        /// </summary>
        public String ThreadName { get => m_threadName; set => m_threadName = value; }
        /// <summary>
        /// The name of the file that the log was made in. 
        /// </summary>
        public String FileName { get => m_fileName; set => m_fileName = value; }
        /// <summary>
        /// The exact line number that the log was made on. 
        /// </summary>
        public String LineNumber { get => m_lineNumber; set => m_lineNumber = value; }
        /// <summary>
        /// The exact time that the log was made
        /// </summary>
        public String Time { get => m_time; set => m_time = value; }
        /// <summary>
        /// The name of the member that made the log. 
        /// </summary>
        public String MemberName { get => m_memberName; set => m_memberName = value; }
        /// <summary>
        /// The severity/importance of the log.
        /// </summary>
        public string Severity { get => m_severity; set => m_severity = value; }
    }

    /// <summary>
    /// The logging class is used to record what happens during runtime of the program. 
    /// </summary>
    /// <remarks>
    /// This is a singleton class used to record actions done during runtime of the the program. 
    /// Because the logging system is singleton based any class can call it as long as it is in the scope.
    /// This allows for benifits of not having multiple logging sections in the program. 
    /// The logging class is thread safe, and uses locks. 
    /// </remarks>
    class Logging
    {
        // Private Encapsulated Variables
        private static String m_logFormat = "%D (%T), \"%F\" (%m) <%L> : %M";
        private static String m_directory = "Logs.log";

        // Purely Private Variables
        private static Boolean m_running = false;
        private static Logging m_instance = null;
        private static Boolean m_flush = false;

        // Readonly Private Variables
        private static readonly Queue<LogData> m_output = new Queue<LogData>();
        private static readonly Mutex m_outputLock = new Mutex();
        private static readonly Thread m_thread = new Thread(ThreadFunction);
        private static readonly Object m_padlock = new Object();

        // Encapsulation Functions
        /// <summary>
        /// This is the format that the log will be saved as. 
        /// </summary>
        /// <remarks>
        /// The default method is "%D (%T), \"%F\" <%L> : %M" as it contains all the nessary criteria. 
        /// If you wish to create your own format you can use these variables to indicate properties of the log. \n
        /// %D would represent the date the log was made \n
        /// %T would represent the time that the log was made. \n
        /// %F would represent the file that the log was made in. \n
        /// %L would represent the line that the log was written on. \n
        /// %M would represent the message the log contains. \n
        /// %m would represent the member of the class that called the log.
        /// </remarks>
        public static string LogFormat { get => m_logFormat; set => m_logFormat = value; }
        /// <summary>
        /// Logs must be saved in a file somewhere to be read, Directory is the name of the file where the logs are saved.
        /// </summary>
        public static string Directory { get => m_directory; set => m_directory = value; }
        /// <summary>
        /// This is the instance of the singleton class. Any commands must be called using this. 
        /// </summary>
        /// <remarks>
        /// A singleton class has one or no instances. In order to use the instance this must be called. 
        /// This also starts the threading and logging process of the class.
        /// </remarks>
        public static Logging Instance
        {
            get
            {
                if(m_instance == null)
                {
                    lock(m_padlock)
                    {
                        if(m_instance == null)
                        {
                            m_instance = new Logging();
                            m_running = true;
                            m_thread.Start();
                        }
                    }
                }
                return m_instance;
            }
        }

        // Functions
        /// <summary>
        /// The default static constructor. This is neither public or private intentionally to allow for singleton class
        /// </summary>
        static Logging() {}

        /// <summary>
        /// Removes all previously made logs. 
        /// </summary>
        public static void Flush()
        {
            m_flush = true;
        }

        /// <summary>
        /// This function creates a log to be saved.
        /// </summary>
        /// <param name="message"> This is the message you want saved to log. This is mandatory to have</param>
        /// <param name="memberName"> Using macros finds the member name that made the log</param>
        /// <param name="filePath"> Using macros finds the current file name that the log was made on</param>
        /// <param name="lineNumber"> Using macros finds the line number that the log was made on</param>
        public void Log(String message,
            String severity = "",
            [System.Runtime.CompilerServices.CallerMemberName] String memberName = "",
            [System.Runtime.CompilerServices.CallerFilePath] String filePath = "",
            [System.Runtime.CompilerServices.CallerLineNumber] int lineNumber = 0)
        {
            LogData newLog = new LogData
            {
                Message = message,
                Date = DateTime.Today.ToShortDateString(),
                //ThreadName = Thread.Get,
                FileName = filePath,
                LineNumber = lineNumber.ToString(),
                Time = DateTime.Now.ToString("h:mm:ss tt"),
                MemberName = memberName
            };
            m_outputLock.WaitOne();
            m_output.Enqueue(newLog);
            m_outputLock.ReleaseMutex();
        }

        /// <summary>
        /// This is the function that saves all logging process
        /// </summary>
        private static void ThreadFunction()
        {
            LogData writeLog;
            String message;
            Char messageParameter;
            System.IO.StreamWriter file;

            String path = @"C:\Users\mieuser\Source\Repos\CSharpErgoBoard\CSharpErgoBoard\Logs\Logs.log";
            file = new System.IO.StreamWriter(path);
            while (m_running)
            {
                // There are no logs to be saved
                if (m_output.Count() == 0)
                {
                    // Sleep the thread for 1ms as to not use more than nesscary system resources. 
                    Thread.Sleep(1);
                    continue;
                }

                // Flushing the file 
                if (m_flush)
                {
                    file.Close();
                    file = new System.IO.StreamWriter(path);
                    m_flush = false;
                }
                
                // Formatting the log message
                message = "";
                writeLog = m_output.Dequeue();
                for (int i = 0; i < LogFormat.Count(); i++)
                {
                    if (LogFormat.ElementAt(i) == '%')
                    {
                        messageParameter = LogFormat.ElementAt(i + 1);
                        if (messageParameter == 'D')
                        {
                            message += writeLog.Date;
                        }
                        else if (messageParameter == 'T')
                        {
                            message += writeLog.Time;
                        }
                        else if (messageParameter == 't')
                        {
                            message += writeLog.ThreadName;
                        }
                        else if (messageParameter == 'm')
                        {
                            message += writeLog.MemberName;
                        }
                        else if (messageParameter == 'F')
                        {
                            message += writeLog.FileName;
                        }
                        else if (messageParameter == 'L')
                        {
                            message += writeLog.LineNumber;
                        }
                        else if (messageParameter == 'M')
                        {
                            message += writeLog.Message;
                        }
                        else
                        {
                            message += '%';
                            message += messageParameter;
                        }
                        i++;
                    }
                    else
                    {
                        message += LogFormat.ElementAt(i);
                    }
                }
                file.WriteLine(message);
            }
            file.Close();
        }

        /// <summary>
        /// This stops the logging process. If this isn't called somewhere then its possible for no logs to be saved.  
        /// </summary>
        public void End()
        {
            m_running = false;
            m_thread.Join();
        }
    }

}
