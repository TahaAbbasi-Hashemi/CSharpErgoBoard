// Using
using System;
using System.Threading;

namespace CSharpErgoBoard
{
    /// <summary>
    /// A Basic Singleton Class used for helping other singleton classes be made
    /// </summary>
    /// <remarks>
    /// The Class is thread safe and uses locks to ensure double existance does not happen.
    /// </remarks>
    class Singleton
    {
        // Class Atributes
        /// <summary>
        /// The name of the class
        /// </summary>
        public String Name { get; } = "Singleton";
        /// <summary>
        /// The purpose of the class
        /// </summary>
        public String Purpose { get; } = "To act as a architectural base for singleton classes";
        /// <summary>
        /// To convert the class to a string.
        /// </summary>
        public new String ToString { get; } = "A Singleton Class";

        // Private Variabels 
        /// <summary>
        /// The instance of the singleton
        /// </summary>
        protected static Singleton m_instance = null;
        /// <summary>
        /// The variable inside the while loop of ThreadFunction
        /// </summary>
        protected static Boolean m_running = false;

        // Readonly Private Variables 
        /// <summary>
        /// The Thread designed for the signleton 
        /// </summary>
        protected static readonly Thread m_thread = new Thread(ThreadFunction);
        /// <summary>
        /// A Thread Safe lockused for instance development
        /// </summary>
        protected static readonly Object m_padlock = new Object();

        // Functions
        /// <summary>
        /// This is the instance of the singleton class. Any commands must be called using  
        /// </summary>
        /// <remarks>
        /// A singleton class has one or no instances. In order to use the instance this must be called. 
        /// This also starts the threading and logging process of the class.
        /// </remarks>
        public static Singleton Instance
        {
            get
            {
                if (m_instance == null)
                {
                    lock (m_padlock)
                    {
                        if (m_instance == null)
                        {
                            m_instance = new Singleton();
                            m_running = true;
                            m_thread.Start();
                        }
                    }
                }
                return m_instance;
            }
        }
        static Singleton() { }

        /// <summary>
        /// This function is run by the thread. This is intended to be over written by another function.
        /// </summary>
        protected static void ThreadFunction()
        {
            while (m_running)
            {
                // Do something
            }
        }

        /// <summary>
        /// This stops the threading process, if this function is not called, then the program can not close. This is intended to be over written by another function.
        /// </summary>
        public virtual void End()
        {
            m_running = false;
            m_thread.Join();
        }
    }
}
