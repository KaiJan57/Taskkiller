using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Diagnostics;

namespace Taskkiller
{
    public partial class FormConfig : Form
    {
        public bool showExitWarning = true;
        public List<int> TimeList = new List<int>();
        private int LastIndex = 0;

        public FormConfig()
        {
            InitializeComponent();
            //LanguageBox.SelectedIndex = 0;
            UpdateLanguage();
            if (TaskkillerMain.firstStart)
            {
                //Disable Abort actions when first start
                buttonAbort.Enabled = false;
                abortToolStripMenuItem.Enabled = false;
                this.LanguageBox.SelectedIndex = 0;
            }
            else
            {
                //else read configuration
                this.TimeList = Taskkiller.Program.MainContext.TimeList;
                foreach (string s in Taskkiller.Program.MainContext.ProcessNames)
                {
                    ProcessList.Items.Add(s);
                    if (Taskkiller.Program.MainContext.KillCompletely[ProcessList.Items.Count - 1])
                    {
                        this.ProcessList.SetItemChecked(this.ProcessList.Items.Count - 1, true);
                    }
                    else
                    {
                        this.ProcessList.SetItemChecked(this.ProcessList.Items.Count - 1, false);
                    }
                }
                this.TrollMode.Checked = Taskkiller.Program.MainContext.TrollMode;
                this.checkBoxHideIcon.Checked = Taskkiller.Program.MainContext.HideIcon;
                this.LanguageBox.SelectedIndex = Taskkiller.Program.MainContext.LanguageMode;
                if (ProcessList.Items.Count > 0)
                {
                    buttonSave.Enabled = true;
                    saveToolStripMenuItem.Enabled = true;
                }
            }
        }

        public void UpdateLanguage()
        {
            this.Program.Text = strings.String_Program;
            this.programToolStripMenuItem.Text = strings.String_Program;
            this.exitToolStripMenuItem.Text = strings.String_Exit;
            this.configurationToolStripMenuItem.Text = strings.String_Configuration;
            this.saveToolStripMenuItem.Text = strings.String_Save;
            this.abortToolStripMenuItem.Text = strings.String_Abort;
            this.buttonSave.Text = strings.String_Save;
            this.buttonAbort.Text = strings.String_Abort;
            this.groupBox1.Text = strings.String_AddEdit;
            this.label3.Text = strings.String_EndAfter;
            this.buttonAdd.Text = strings.String_AddEdit;
            this.KillProcess.Text = strings.String_KillCompletely;
            this.label1.Text = strings.String_Process;
            this.Remove.Text = strings.String_Remove;
            this.buttonRemove.Text = strings.String_Remove;
            this.label2.Text = strings.String_SelectRemove;
            this.groupBox2.Text = strings.String_AddedProcesses;
            this.groupBox3.Text = strings.String_Other;
            this.groupBox4.Text = strings.String_XMLOptions;
            this.buttonResetXML.Text = strings.String_DeleteData;
            this.buttonShow.Text = strings.String_ShowInExplorer;
            this.label5.Text = strings.String_ChooseLang;
            this.Text = strings.String_Title;
            this.checkBoxHideIcon.Text = strings.String_HideIcon;
            this.LanguageBox.Items[0] = strings.String_AutoLang;
        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            TaskkillerMain.firstStart = false;
            SaveConfig();
        }

        private void abortToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Abort();
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void buttonAbort_Click(object sender, EventArgs e)
        {
            Abort();
        }

        private void buttonSave_Click(object sender, EventArgs e)
        {
            SaveConfig();
        }

        private void SaveConfig()
        {
            showExitWarning = false;
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void Abort()
        {
            if (MessageBox.Show(strings.MsgBox_Question_Abort_Text, strings.MsgBox_Question_Abort_Title, MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.No)
            {
               return;
            }
            showExitWarning = false;
            this.DialogResult = DialogResult.Abort;
            this.Close();
        }

        private void ProcessName_TextChanged(object sender, EventArgs e)
        {
            if (ProcessName.Text == ".exe")
            {
                buttonAdd.Enabled = false;
            }
            else
            {
                buttonAdd.Enabled = true;
            }
        }

        private void buttonAdd_Click(object sender, EventArgs e)
        {
            AddProcess();
        }

        private void AddProcess()
        {
            if (ProcessList.Items.Contains(ProcessName.Text))
            {
                //Edit Item
                int index = ProcessList.Items.IndexOf(ProcessName.Text);
                if (KillProcess.Checked)
                {
                    this.ProcessList.SetItemChecked(index, true);
                }
                else
                {
                    this.ProcessList.SetItemChecked(index, false);
                }
                //(int)Time.Value never over 100. This Method works better.
                TimeList[index] = int.Parse(Time.Value.ToString());
                return;
            }
            ProcessList.Items.Add(ProcessName.Text);
            TimeList.Add(int.Parse(Time.Value.ToString()));
            if (KillProcess.Checked)
            {
                this.ProcessList.SetItemChecked(this.ProcessList.Items.Count - 1, true);
            }
            else
            {
                this.ProcessList.SetItemChecked(this.ProcessList.Items.Count - 1, false);
            }
            buttonSave.Enabled = true;
            saveToolStripMenuItem.Enabled = true;
            ProcessName.Text = ".exe";
            ProcessList.SelectedIndex = ProcessList.Items.Count - 1;
        }

        private void RemoveProcess()
        {
            this.TimeList.RemoveAt(ProcessList.SelectedIndex);
            this.ProcessList.Items.Remove(ProcessList.SelectedItem);
            if (ProcessList.Items.Count < 1)
            {
                buttonRemove.Enabled = false;
                buttonSave.Enabled = false;
                saveToolStripMenuItem.Enabled = false;
            }
            else
            {
                ProcessList.SelectedIndex = ProcessList.Items.Count - 1;
            }
        }

        private void ProcessList_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.ProcessList.SelectedIndex < 0)
            {
                buttonRemove.Enabled = false;
            }
            else
            {
                buttonRemove.Enabled = true;
                buttonAdd.Enabled = true;
                ProcessName.Text = ProcessList.Items[ProcessList.SelectedIndex].ToString();
                Time.Value = TimeList[ProcessList.SelectedIndex];
                KillProcess.Checked = ProcessList.GetItemChecked(ProcessList.SelectedIndex);
            }
        }

        private void buttonRemove_Click(object sender, EventArgs e)
        {
            RemoveProcess();
        }

        private void ProcessName_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                if (buttonAdd.Enabled)
                {
                    AddProcess();
                }
                else
                {
                    MessageBox.Show(strings.MsgBox_NoInput_Text, strings.MsgBox_NoInput_Title, MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void ProcessList_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Delete)
            {
                if (buttonRemove.Enabled)
                {
                    RemoveProcess();
                }
                else
                {
                    MessageBox.Show(strings.MsgBox_NoSelection_Text, strings.MsgBox_NoSelection_Title, MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void FormConfig_FormClosing(object sender, FormClosingEventArgs e)
        {
            //check the reason why the form is closing
            if (this.showExitWarning)
            {
                if (MessageBox.Show(strings.MsgBox_Question_Exit_Text, strings.MsgBox_Question_Exit_Title, MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.No)
                {
                    e.Cancel = true;
                }
            }
        }

        private void ProcessName_SelectionChanged(object sender, EventArgs e)
        {
            if (ProcessName.SelectionStart > ProcessName.Text.Length - 4)
            {
                ProcessName.SelectionStart = ProcessName.Text.Length - 4;
            }
            if (ProcessName.SelectionLength + ProcessName.SelectionStart > ProcessName.Text.Length - 4)
            {
                ProcessName.SelectionLength = ProcessName.Text.Length - 4 - ProcessName.SelectionStart;
            }
        }

        private void buttonShow_Click(object sender, EventArgs e)
        {
            try
            {
                Process.Start("explorer.exe", string.Format("/select,\"{0}\"", TaskkillerMain.configFile));
            }
            catch
            {
                MessageBox.Show(strings.MsgBox_Error_FileToShow_NotExisting, strings.MsgBox_Error_Title, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LanguageBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (LastIndex == this.LanguageBox.SelectedIndex)
            {
                return;
            }
            LastIndex = this.LanguageBox.SelectedIndex;
            switch (this.LanguageBox.SelectedIndex)
            {
                case 0:
                    {
                        //Language: Auto
                        Taskkiller.Program.MainContext.UpdateLanguage("auto");
                        break;
                    }
                case 1:
                    {
                        //English
                        Taskkiller.Program.MainContext.UpdateLanguage("en");
                        break;
                    }
                case 2:
                    {
                        //German
                        Taskkiller.Program.MainContext.UpdateLanguage("de");
                        break;
                    }
            }
        }

        private void buttonResetXML_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show(strings.MsgBox_ConfigDelete_Text, strings.MsgBox_ConfigDelete_Title, MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
            {
                try
                {
                    System.IO.File.Delete(TaskkillerMain.configFile);
                    System.IO.Directory.Delete(TaskkillerMain.dataPath);
                    if (MessageBox.Show(strings.MsgBox_ExecutableDelete_Text, strings.MsgBox_ExecutableDelete_Title, MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    {
                        showExitWarning = false;
                        //Just to prevent crashes on exit
                        this.Close();
                        this.Dispose();
                        var Process = new Process();
                        Process.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
                        Process.StartInfo.FileName = "cmd.exe";
                        //give cmd command: wait 1 second, delete noprompt, delete localization folder noprompt
                        Process.StartInfo.Arguments = "/C ping 1.1.1.1 -n 1 -w 1000 > Nul & del /F /Q \"" + Application.ExecutablePath + "\" & rmdir /S /Q \"" + Application.StartupPath + "\\de\"";
                        Process.Start();
                        Taskkiller.Program.MainContext.ExitProgram();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(strings.MsgBox_Error_FileDelete + ex.Message, strings.MsgBox_Error_Title, MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void checkBoxHideIcon_CheckedChanged(object sender, EventArgs e)
        {
            HideIcon(checkBoxHideIcon.Checked);
        }

        public static void HideIcon(bool hide)
        {
            if (hide)
            {
                //Hide Icon (Set Icon as transparent) and show message
                Taskkiller.Program.MainContext.TaskkillerIcon.Icon = Properties.Resources.Transparent;
                Taskkiller.Program.MainContext.TaskkillerIcon.Text = "";
                if(!Taskkiller.Program.MainContext.hiddenmsgshown)
                {
                    Taskkiller.Program.MainContext.TaskkillerIcon.ShowBalloonTip(2000, "Taskkiller", strings.String_Hello, ToolTipIcon.Info);
                }
                Taskkiller.Program.MainContext.hiddenmsgshown = true;
            }
            else
            {
                //Show Icon
                Taskkiller.Program.MainContext.hiddenmsgshown = false;
                Taskkiller.Program.MainContext.TaskkillerIcon.Text = "Taskkiller";
                ComponentResourceManager resources = new ComponentResourceManager(typeof(TaskkillerMain));
                Taskkiller.Program.MainContext.TaskkillerIcon.Icon = ((Icon)(resources.GetObject("TrayIco.Icon")));
            }
        }
    }
}
