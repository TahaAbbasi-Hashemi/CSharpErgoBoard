// Using
using System;
using System.Windows.Forms;
using System.Configuration;
using System.Collections.Specialized;
using CSharpErgoBoard.Properties;

namespace CSharpErgoBoard.Design
{
    class KeyButton : Button
    {
        [Serializable()]
        private class KeyButtonError : Exception
        {
            // Functions
            /// <summary>
            /// Default constructor
            /// </summary>
            public KeyButtonError() : base() { }
            /// <summary>
            /// Constructor with a message.
            /// </summary>
            /// <param name="message"> The message being reported to the user.</param>
            public KeyButtonError(String message) : base(message) { }
            /// <summary>
            /// A Constructor with a inner error
            /// </summary>
            /// <param name="message"> The message being reported.</param>
            /// <param name="inner"> The error that progogated this error.</param>
            public KeyButtonError(String message, Exception inner) : base(message, inner) { }
        }

        private Boolean m_darkMode;
        private Boolean m_selected;
        private String m_side;
        private String m_size;
        private String m_type;
        private String m_value;
        private UInt32 m_row = 7;
        private UInt32 m_col;
        private UInt32 m_layer;
        private String m_setting;
        private String m_keyName;
        private String m_keyValue;

        public UInt32 Row { get => m_row; set => m_row = value; }
        public UInt32 Col { get => m_col; set => m_col = value; }
        public String KeyValue { get => m_keyValue; }
        public String KeyName { get => m_keyName; }
        public UInt32 Layer { get => m_layer; set => m_layer = value; }
        public String Value { get => m_value; set => m_value = value; }
        public String Type { get => m_type;}

        public KeyButton()
        {
            m_darkMode = false;
            m_selected = false;
            m_side = "Left";
            m_size = "Single";
            m_type = "Key";
            m_value = "None";
            m_row = 0;
            m_col = 0;
            m_layer = 1;
            m_setting = "L0R0C0";
            m_keyName = "R0C0";
            m_keyValue = "Row 0, Col 0";
        }
        public Boolean Setup(in String size = "Single")
        {
            m_size = size;
            UpdateKey();
            SaveKey();
            return true;
        }
        public Boolean ModeChange(in Boolean darkMode = false)
        {
            m_darkMode = darkMode;

            if (m_type == "Key")
            {
                if (!m_selected)
                {
                    if (m_darkMode)
                    {
                        if (m_size == "Tall")
                        {
                            Image = Resources.TallKeyDark;
                        }
                        else if (m_size == "Single")
                        {
                            Image = Resources.SingleKeyDark;
                        }
                        else if (m_size == "Wide")
                        {
                            Image = Resources.WideKeyDark;
                        }
                        else
                        {
                            throw new KeyButtonError("This is not the right type");
                        }
                    }
                    else
                    {
                        if (m_size == "Tall")
                        {
                            Image = Resources.TallKeyLight;
                        }
                        else if (m_size == "Single")
                        {
                            Image = Resources.SingleKeyLight;
                        }
                        else if (m_size == "Wide")
                        {
                            Image = Resources.WideKeyLight;
                        }
                        else
                        {
                            throw new KeyButtonError("This is not the right type");
                        }
                    }
                }
                else
                {
                    if (m_darkMode)
                    {
                        if (m_size == "Tall")
                        {
                            Image = Resources.TallKeyDarkSelected;
                        }
                        else if (m_size == "Single")
                        {
                            Image = Resources.SingleKeyDarkSelected;
                        }
                        else if (m_size == "Wide")
                        {
                            Image = Resources.WideKeyDarkSelected;
                        }
                        else
                        {
                            throw new KeyButtonError("This is not the right type");
                        }
                    }
                    else
                    {
                        if (m_size == "Tall")
                        {
                            Image = Resources.TallKeyLightSelected;
                        }
                        else if (m_size == "Single")
                        {
                            Image = Resources.SingleKeyLightSelected;
                        }
                        else if (m_size == "Wide")
                        {
                            Image = Resources.WideKeyLightSelected;
                        }
                        else
                        {
                            throw new KeyButtonError("This is not the right type");
                        }
                    }
                }
            }
            else
            {
                if (!m_selected)
                {
                    if (m_darkMode)
                    {
                        if (m_size == "Tall")
                        {
                            Image = Resources.TallKeyDarkLED;
                        }
                        else if (m_size == "Single")
                        {
                            Image = Resources.SingleKeyDarkLED;
                        }
                        else if (m_size == "Wide")
                        {
                            Image = Resources.WideKeyDarkLED;
                        }
                        else
                        {
                            throw new KeyButtonError("LED not selectged darkmode, This is not the right type " + m_size);
                        }
                    }
                    else
                    {
                        if (m_size == "Tall")
                        {
                            Image = Resources.TallKeyLightLED;
                        }
                        else if (m_size == "Single")
                        {
                            Image = Resources.SingleKeyLightLED;
                        }
                        else if (m_size == "Wide")
                        {
                            Image = Resources.WideKeyLightLED;
                        }
                        else
                        {
                            throw new KeyButtonError("This is not the right type");
                        }
                    }
                }
                else
                {
                    if (m_darkMode)
                    {
                        if (m_size == "Tall")
                        {
                            Image = Resources.TallKeyDarkLEDSelected;
                        }
                        else if (m_size == "Single")
                        {
                            Image = Resources.SingleKeyDarkLEDSelected;
                        }
                        else if (m_size == "Wide")
                        {
                            Image = Resources.WideKeyDarkLEDSelected;
                        }
                        else
                        {
                            throw new KeyButtonError("This is not the right type");
                        }
                    }
                    else
                    {
                        if (m_type == "Tall")
                        {
                            Image = Resources.TallKeyLightLEDSelected;
                        }
                        else if (m_size == "Single")
                        {
                            Image = Resources.SingleKeyLightLEDSelected;
                        }
                        else if (m_size == "Wide")
                        {
                            Image = Resources.WideKeyLightLEDSelected;
                        }
                        else
                        {
                            throw new KeyButtonError("This is not the right type");
                        }
                    }
                }
            }

            return true;
        }
        public Boolean SelectKey(in Boolean selected = true)
        {
            m_selected = selected;
            return ModeChange(m_darkMode);
        }
        public String SaveKey(in String value = null)
        {
            String outValue;

            if (value != null)
            {
                m_value = value;
                Text = value;
            }
            else
            {
                outValue = Text;
            }
            outValue = m_value;

            try
            {
                Configuration configFile = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
                KeyValueConfigurationCollection settings = configFile.AppSettings.Settings;
                if (settings[m_setting] == null)
                {
                    settings.Add(m_setting, m_value);
                }
                else
                {
                    settings[m_setting].Value = m_value;
                }
                configFile.Save(ConfigurationSaveMode.Modified);
                ConfigurationManager.RefreshSection(configFile.AppSettings.SectionInformation.Name);
            }
            catch (ConfigurationErrorsException)
            {
                return null;   // TODO I am not sure what to do here....
            }
            return outValue;
        }
        public Boolean UpdateKey()
        {
            m_row = (UInt32)(Name[Name.Length - 3]) - '0';
            m_col = (UInt32)(Name[Name.Length - 1]) - '0';
            if (Name.Length == 0)
            {
                throw new Exception("something is wrong here");
            }
            if (Name.Contains("id_buttonRight"))
            {
                m_side = "Right";
            }
            else if (Name.Contains("Id_buttonLeft"))
            {
                m_side = "Left";
            }
            if (Name.Contains("Key"))
            {
                m_type = "Key";
            }
            else if (Name.Contains("Led"))
            {
                m_type = "Led";
            }
            m_setting = m_side + "L" + m_layer + "R" + m_row + "C" + m_col;
            m_keyValue = "Row " + m_row + ", Column " + m_col;
            m_keyName = "L" + (m_layer - 1).ToString() + "R" + (m_row - 1) + "C" + (m_col - 1);
            //SaveKey();

            try
            {
                NameValueCollection appSettings = ConfigurationManager.AppSettings;
                if (appSettings[m_setting] != null)
                {
                    m_value = appSettings[m_setting];
                }
                else
                {
                    m_value = "None";
                }
            }
            catch (ConfigurationErrorsException)
            {
                return false;   // TODO figure out what to do here.
            }
            finally
            {
                if (m_value == "None")
                {
                    m_value = Text;
                }
                else
                {
                    Text = m_value;
                }
            }
            if (m_type == "Led")
            {
                Text = "";
                m_value = "None";   
            }
            else
            {
                m_value = Text;
            }

            return true;
        }
        public String GetText()
        {
            return Text;
        }
    }
}
