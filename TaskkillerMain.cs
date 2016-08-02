using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Threading;
using System.Windows.Forms;
using System.Xml;

namespace Taskkiller
{
    public partial class TaskkillerMain : ApplicationContext
    {
        public static string dataPath = Application.StartupPath + Path.DirectorySeparatorChar + "Taskkiller_Data";
        public static string configFile = dataPath + Path.DirectorySeparatorChar + "Settings.xml";
        public static bool firstStart = false;
        public int LanguageMode = 0;
        public bool TrollMode = false;
        public bool HideIcon = false;
        public bool hiddenmsgshown = false;
        public List<string> ProcessNames = new List<string>();
        public List<int> TimeList = new List<int>();
        public List<bool> KillCompletely = new List<bool>();
        private List<KillThread> KillThreads = new List<KillThread>();
        private FormConfig configForm;
        private bool isInitialized = false;

        public TaskkillerMain()
        {
            InitializeComponent();
            UpdateLanguage(null);
            isInitialized = true;
            Program.MainContext = this;
            if (!File.Exists(configFile))
            {
                //Program is started first time
                //Check if data directory exists
                if (!Directory.Exists(dataPath))
                {
                    try
                    {
                        Directory.CreateDirectory(dataPath);
                    }
                    catch(Exception e)
                    {
                        MessageBox.Show(strings.MsgBox_Error_CD + e.Message, strings.MsgBox_Error_Title, MessageBoxButtons.OK, MessageBoxIcon.Error);
                        ExitProgram();
                    }
                }
                firstStart = true;
                MessageBox.Show(strings.MsgBox_FirstStart_Text, strings.MsgBox_FirstStart_Title, MessageBoxButtons.OK, MessageBoxIcon.Information);
                ShowConfigForm();
            }
            else
            {
                //File exists, read configuration
                ReadConfig();
            }
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (configForm != null && MessageBox.Show(strings.MsgBox_Question_Exit_Text, strings.MsgBox_Question_Exit_Title, MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.No)
            {
                return;
            }
            ExitProgram();
        }

        private void configToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ShowConfigForm();
        }

        private void ShowConfigForm()
        {
            //Check, if ConfigForm is open
            if (configForm == null)
            {
                //if not, make a new instance of FormConfig
                configForm = new FormConfig();
                //show it as a dialog, and check if the input was successful
                DialogResult result = configForm.ShowDialog();
                if (result == DialogResult.OK)
                {
                    //now if it was firststart, now it isnt anymore
                    firstStart = false;
                    //if it was, save everything
                    SaveConfig();
                    //and apply the settings
                    ApplyConfig();
                }
                //destroy the object to know later if the ConfigForm is open
                configForm = null;
                //Exit program after destroying configForm object
                if (result == DialogResult.Cancel)
                {
                    ExitProgram();
                }
            }
            else
            {
                //if the ConfigForm is open, bring it to front
                configForm.Activate();
            }
        }

        private void ReadConfig()
        {
            try
            {
                XmlDocument settings = new XmlDocument();
                settings.Load(configFile);
                XmlNodeList nodeList = settings.DocumentElement.SelectNodes("/Taskkiller_Settings/Process");
                foreach (XmlNode node in nodeList)
                {
                    ProcessNames.Add(node.SelectSingleNode("Name").InnerText);
                    KillCompletely.Add(bool.Parse(node.SelectSingleNode("KillCompletely").InnerText));
                    TimeList.Add(int.Parse(node.SelectSingleNode("Delay").InnerText));
                }
                nodeList = settings.DocumentElement.SelectNodes("/Taskkiller_Settings");
                foreach (XmlNode node in nodeList)
                {
                    TrollMode = bool.Parse(node.SelectSingleNode("TrollMode").InnerText);
                    HideIcon = bool.Parse(node.SelectSingleNode("HideIcon").InnerText);
                    LanguageMode = int.Parse(node.SelectSingleNode("Language").InnerText);
                }
                ApplyConfig();
            }
            catch (NullReferenceException)
            {
                MessageBox.Show(strings.MsgBox_Error_ReadConfigFile1, strings.MsgBox_Error_Title, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception e)
            {
                MessageBox.Show(strings.MsgBox_Error_ReadConfigFile + e.Message, strings.MsgBox_Error_Title, MessageBoxButtons.OK, MessageBoxIcon.Error);
                ExitProgram();
            }
        }

        private void SaveConfig()
        {
            //Write everything into local vars
            TimeList = configForm.TimeList;
            TrollMode = configForm.TrollMode.Checked;
            HideIcon = configForm.checkBoxHideIcon.Checked;
            LanguageMode = configForm.LanguageBox.SelectedIndex;
            //Clear everything up to prevent double items
            ProcessNames.Clear();
            KillCompletely.Clear();
            foreach (string s in configForm.ProcessList.Items)
            {
                ProcessNames.Add(s);
                KillCompletely.Add(configForm.ProcessList.GetItemChecked(configForm.ProcessList.Items.IndexOf(s)));
            }
            //Write everything into file
            try
            {
                XmlTextWriter writer = new XmlTextWriter(configFile, System.Text.Encoding.UTF8);
                writer.WriteStartDocument(true);
                writer.Formatting = Formatting.Indented;
                writer.Indentation = 2;
                writer.WriteStartElement("Taskkiller_Settings");
                foreach (string process in configForm.ProcessList.Items)
                {
                    bool killcompletely = configForm.ProcessList.GetItemChecked(configForm.ProcessList.Items.IndexOf(process));
                    int time = configForm.TimeList[configForm.ProcessList.Items.IndexOf(process)];
                    writer.WriteStartElement("Process");
                    writer.WriteStartElement("Name");
                    writer.WriteString(process);
                    writer.WriteEndElement();
                    writer.WriteStartElement("KillCompletely");
                    writer.WriteValue(killcompletely);
                    writer.WriteEndElement();
                    writer.WriteStartElement("Delay");
                    writer.WriteValue(time);
                    writer.WriteEndElement();
                    writer.WriteEndElement();
                }
                writer.WriteStartElement("TrollMode");
                writer.WriteValue(configForm.TrollMode.Checked);
                writer.WriteEndElement();
                writer.WriteStartElement("HideIcon");
                writer.WriteValue(configForm.checkBoxHideIcon.Checked);
                writer.WriteEndElement();
                writer.WriteStartElement("Language");
                writer.WriteValue(configForm.LanguageBox.SelectedIndex);
                writer.WriteEndElement();
                writer.WriteEndElement();
                writer.WriteEndDocument();
                writer.Close();
            }
            catch (Exception e)
            {
                MessageBox.Show(strings.MsgBox_Error_WriteConfigFile + e.Message, strings.MsgBox_Error_Title, MessageBoxButtons.OK, MessageBoxIcon.Error);
                ExitProgram();
            }
        }

        private void ApplyConfig()
        {
            FormConfig.HideIcon(HideIcon);
            switch (LanguageMode)
            {
                case 0:
                    {
                        UpdateLanguage("auto");
                        break;
                    }
                case 1:
                    {
                        UpdateLanguage("en");
                        break;
                    }
                case 2:
                    {
                        UpdateLanguage("de");
                        break;
                    }
            }
            KillAllThreads();
            foreach (string s in ProcessNames)
            {
                KillThreads.Add(new KillThread(KillCompletely[ProcessNames.IndexOf(s)], TrollMode, s.Substring(0, s.Length - 4), TimeList[ProcessNames.IndexOf(s)]));
            }
            foreach (KillThread k in KillThreads)
            {
                k.Start();
            }
        }

        //Updates the language of the program
        //Lang = null |Auto
        //Lang = de-De|German
        //...
        public void UpdateLanguage(string Lang)
        {
            if (Lang == null || Lang.ToLower() == "auto")
            {
                Thread.CurrentThread.CurrentUICulture = new CultureInfo(CultureInfo.InstalledUICulture.TwoLetterISOLanguageName);
            }
            else
            {
                Thread.CurrentThread.CurrentUICulture = CultureInfo.GetCultureInfo(Lang);
            }
            if (isInitialized)
            {
                exitToolStripMenuItem.Text = strings.String_Exit;
                configToolStripMenuItem.Text = strings.String_Config;
            }
            if (configForm != null)
            {
                configForm.UpdateLanguage();
            }
        }

        private void KillAllThreads()
        {
            //Kill all threads
            try
            {
                foreach (KillThread k in KillThreads)
                {
                    k.Stop();
                }
            }
            catch { }
            //Clear list
            KillThreads.Clear();
        }

        public void ExitProgram()
        {
            KillAllThreads();
            //dispose the icon to update the taskbar
            TaskkillerIcon.Dispose();
            //Close Form
            if (configForm != null)
            {
                //Don't show warning
                configForm.DialogResult = DialogResult.OK;
                configForm.Close();
                configForm.Dispose();
                configForm = null;
            }
            Environment.Exit(0);
        }
    }
}