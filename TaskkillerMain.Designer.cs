using System;
using System.Collections.Generic;
using System.Text;

namespace Taskkiller
{
    partial class TaskkillerMain
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(TaskkillerMain));
            this.TaskkillerIcon = new System.Windows.Forms.NotifyIcon();
            this.TaskkillerIconMenu = new System.Windows.Forms.ContextMenuStrip();
            this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.configToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            // 
            // TaskkillerIcon
            // 
            this.TaskkillerIcon.Icon = Properties.Resources.Appico;
            this.TaskkillerIcon.ContextMenuStrip = this.TaskkillerIconMenu;
            this.TaskkillerIcon.Text = "Taskkiller";
            this.TaskkillerIcon.Visible = true;
            this.TaskkillerIcon.DoubleClick += new System.EventHandler(this.configToolStripMenuItem_Click);
            // 
            // TaskkillerIconMenu
            // 
            this.TaskkillerIconMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.configToolStripMenuItem,
            this.exitToolStripMenuItem});
            this.TaskkillerIconMenu.Name = "TaskkillerIconMenu";
            this.TaskkillerIconMenu.RenderMode = System.Windows.Forms.ToolStripRenderMode.System;
            this.TaskkillerIconMenu.Size = new System.Drawing.Size(153, 70);
            // 
            // exitToolStripMenuItem
            // 
            this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            this.exitToolStripMenuItem.Size = new System.Drawing.Size(94, 22);
            this.exitToolStripMenuItem.Text = strings.String_Exit;
            this.exitToolStripMenuItem.Click += new System.EventHandler(this.exitToolStripMenuItem_Click);
            // 
            // configToolStripMenuItem
            // 
            this.configToolStripMenuItem.Name = "configToolStripMenuItem";
            this.configToolStripMenuItem.Size = new System.Drawing.Size(94, 22);
            this.configToolStripMenuItem.Text = strings.String_Config;
            this.configToolStripMenuItem.Click += new System.EventHandler(this.configToolStripMenuItem_Click);
        }

        #endregion

        public System.Windows.Forms.NotifyIcon TaskkillerIcon;
        private System.Windows.Forms.ContextMenuStrip TaskkillerIconMenu;
        private System.Windows.Forms.ToolStripMenuItem configToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;

    }
}
