using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Windows.Forms;

namespace Taskkiller
{
    public partial class FormConfig : Form
    {
        public List<int> TimeList = new List<int>();
        private int LastIndex = 0;

        public FormConfig()
        {
            InitializeComponent();
            //LanguageBox.SelectedIndex = 0;
            if (LanguageBox.Items.Count < 6)
            {
                LanguageBox.DropDownStyle = ComboBoxStyle.DropDownList;
            }
            UpdateLanguage();
            if (TaskkillerMain.firstStart)
            {
                //Disable Abort actions when first start
                buttonAbort.Enabled = false;
                abortToolStripMenuItem.Enabled = false;
                LanguageBox.SelectedIndex = 0;
            }
            else
            {
                //else read configuration
                TimeList = Taskkiller.Program.MainContext.TimeList;
                foreach (string s in Taskkiller.Program.MainContext.ProcessNames)
                {
                    ProcessList.Items.Add(s);
                    if (Taskkiller.Program.MainContext.KillCompletely[ProcessList.Items.Count - 1])
                    {
                        ProcessList.SetItemChecked(ProcessList.Items.Count - 1, true);
                    }
                    else
                    {
                        ProcessList.SetItemChecked(ProcessList.Items.Count - 1, false);
                    }
                }
                TrollMode.Checked = Taskkiller.Program.MainContext.TrollMode;
                checkBoxHideIcon.Checked = Taskkiller.Program.MainContext.HideIcon;
                LanguageBox.SelectedIndex = Taskkiller.Program.MainContext.LanguageMode;
                if (ProcessList.Items.Count > 0)
                {
                    buttonSave.Enabled = true;
                    saveToolStripMenuItem.Enabled = true;
                }
            }
        }

        public void UpdateLanguage()
        {
            Program.Text = strings.String_Program;
            programToolStripMenuItem.Text = strings.String_Program;
            exitToolStripMenuItem.Text = strings.String_Exit;
            configurationToolStripMenuItem.Text = strings.String_Configuration;
            saveToolStripMenuItem.Text = strings.String_Save;
            abortToolStripMenuItem.Text = strings.String_Abort;
            buttonSave.Text = strings.String_Save;
            buttonAbort.Text = strings.String_Abort;
            groupBox1.Text = strings.String_AddEdit;
            label3.Text = strings.String_EndAfter;
            buttonAdd.Text = strings.String_AddEdit;
            KillProcess.Text = strings.String_KillCompletely;
            label1.Text = strings.String_Process;
            Remove.Text = strings.String_Remove;
            buttonRemove.Text = strings.String_Remove;
            label2.Text = strings.String_SelectRemove;
            groupBox2.Text = strings.String_AddedProcesses;
            groupBox3.Text = strings.String_Other;
            groupBox4.Text = strings.String_XMLOptions;
            buttonResetXML.Text = strings.String_DeleteData;
            buttonShow.Text = strings.String_ShowInExplorer;
            label5.Text = strings.String_ChooseLang;
            Text = strings.String_Title;
            checkBoxHideIcon.Text = strings.String_HideIcon;
            LanguageBox.Items[0] = strings.String_AutoLang;
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
            DialogResult = DialogResult.Cancel;
            Close();
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
            DialogResult = DialogResult.OK;
            Close();
        }

        private void Abort()
        {
            if (MessageBox.Show(strings.MsgBox_Question_Abort_Text, strings.MsgBox_Question_Abort_Title, MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.No)
            {
               return;
            }
            DialogResult = DialogResult.Abort;
            Close();
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
                    ProcessList.SetItemChecked(index, true);
                }
                else
                {
                    ProcessList.SetItemChecked(index, false);
                }
                //(int)Time.Value never over 100. This Method works better.
                TimeList[index] = int.Parse(Time.Value.ToString());
                return;
            }
            ProcessList.Items.Add(ProcessName.Text);
            TimeList.Add(int.Parse(Time.Value.ToString()));
            if (KillProcess.Checked)
            {
                ProcessList.SetItemChecked(ProcessList.Items.Count - 1, true);
            }
            else
            {
                ProcessList.SetItemChecked(ProcessList.Items.Count - 1, false);
            }
            buttonSave.Enabled = true;
            saveToolStripMenuItem.Enabled = true;
            ProcessName.Text = ".exe";
            ProcessList.SelectedIndex = ProcessList.Items.Count - 1;
        }

        private void RemoveProcess()
        {
            TimeList.RemoveAt(ProcessList.SelectedIndex);
            ProcessList.Items.Remove(ProcessList.SelectedItem);
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
            if (ProcessList.SelectedIndex < 0)
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
            if (DialogResult != DialogResult.OK && DialogResult != DialogResult.Abort && MessageBox.Show(strings.MsgBox_Question_Exit_Text, strings.MsgBox_Question_Exit_Title, MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.No)
            {
                e.Cancel = true;
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
            if (LastIndex == LanguageBox.SelectedIndex)
            {
                return;
            }
            LastIndex = LanguageBox.SelectedIndex;
            switch (LanguageBox.SelectedIndex)
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
                        //Just to prevent crashes on exit
                        DialogResult = DialogResult.OK;
                        Close();
                        Dispose();
                        Process Process = new Process();
                        Process.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
                        Process.StartInfo.FileName = "cmd.exe";
                        //give cmd command: wait 1 second, delete noprompt, delete localization folder noprompt
                        //Process.StartInfo.Arguments = "/C ping 1.1.1.1 -n 1 -w 1000 > Nul & del /F /Q \"" + Application.ExecutablePath + "\" & rmdir /S /Q \"" + Application.StartupPath + "\\de\""; <- No need to delete "de" folder, since satellite assemblys are embedded now
                        Process.StartInfo.Arguments = "/C ping 1.1.1.1 -n 1 -w 1000 > Nul & del /F /Q \"" + Application.ExecutablePath + "\"";
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
                Taskkiller.Program.MainContext.TaskkillerIcon.Icon = Properties.Resources.Appico;
            }
        }
    }
}
