using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Diagnostics;
using System.Windows.Forms;
using System.Globalization;

namespace Taskkiller
{
    class KillThread
    {
        public Thread killThread;
        private bool Troll;
        private bool KillCompletely;
        private int Time;
        private string Name;
        //Thread vars for stopping
        private object Lock = new object();
        private bool _Stop = false;

        public KillThread(bool KillCompletely, bool Troll, string Name, int Time)
        {
            this.Troll = Troll;
            this.KillCompletely = KillCompletely;
            this.Time = Time;
            this.Name = Name;
            killThread = new Thread(new ParameterizedThreadStart(Kill));
        }

        public void Start()
        {
            lock (Lock)
            {
                _Stop = false;
            }
            killThread.Start(new KillThreadParams(this.Troll, this.KillCompletely, this.Time, this.Name));
        }

        public void Stop()
        {
            lock (Lock)
            {
                _Stop = true;
            }
        }

        private void Kill(Object Params)
        {
            KillThreadParams ktparams = ((KillThreadParams)Params);
            bool Troll = ktparams.Troll;
            bool KillCompletely = ktparams.KillCompletely;
            int Time = ktparams.Time;
            string Name = ktparams.Name;
            while (true)
            {
                foreach (Process process in Process.GetProcessesByName(Name))
                {
                    lock (Lock)
                    {
                        if (_Stop)
                        {
                            break;
                        }
                    }
                    try
                    {
                        Thread.Sleep(Time);
                        if (KillCompletely)
                        {
                            process.Kill();
                        }
                        else
                        {
                            process.CloseMainWindow();
                        }
                        if (Troll)
                        {
                            new Thread(TrollWindow).Start();
                        }
                    }
                    catch
                    {
                    }
                }
                lock (Lock)
                {
                    if (_Stop)
                    {
                        break;
                    }
                }
            }
        }

        private void TrollWindow()
        {
            Thread.CurrentThread.CurrentUICulture = new CultureInfo(CultureInfo.InstalledUICulture.TwoLetterISOLanguageName);
            WindowHandle taskbar = new WindowHandle("SysListView32", "FolderView");
            //To focus the app
            Form activationForm = new Form();
            activationForm.FormBorderStyle = FormBorderStyle.None;
            activationForm.ShowInTaskbar = false;
            activationForm.Size = new System.Drawing.Size(0, 0);
            activationForm.Show(taskbar);
            activationForm.Activate();
            activationForm.Focus();
            activationForm.Close();
            //troll :-P
            if (MessageBox.Show(taskbar, strings.MsgBox_Troll_0, strings.MsgBox_ConfigDelete_Title, MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
            {
                if (MessageBox.Show(taskbar, strings.MsgBox_Troll_0, strings.MsgBox_ConfigDelete_Title, MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
                {
                    if (MessageBox.Show(taskbar, strings.MsgBox_Troll_0, strings.MsgBox_ConfigDelete_Title, MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
                    {
                        if (MessageBox.Show(taskbar, strings.MsgBox_Troll_0, strings.MsgBox_ConfigDelete_Title, MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
                        {
                            if (MessageBox.Show(taskbar, strings.MsgBox_Troll_0, strings.MsgBox_ConfigDelete_Title, MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
                            {
                                if (MessageBox.Show(taskbar, strings.MsgBox_Troll_0, strings.MsgBox_ConfigDelete_Title, MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
                                {
                                    if (MessageBox.Show(taskbar, strings.MsgBox_Troll_0, strings.MsgBox_ConfigDelete_Title, MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
                                    {
                                        if (MessageBox.Show(taskbar, strings.MsgBox_Troll_0, strings.MsgBox_ConfigDelete_Title, MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
                                        {
                                            if (MessageBox.Show(taskbar, strings.MsgBox_Troll_0, strings.MsgBox_ConfigDelete_Title, MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
                                            {
                                                MessageBox.Show(taskbar, strings.MsgBox_Troll_DontFeelLike, strings.MsgBox_Error_Title, MessageBoxButtons.OK, MessageBoxIcon.Stop);
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }
    }
}
