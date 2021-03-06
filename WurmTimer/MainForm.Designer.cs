﻿namespace WurmTimer
{
    partial class MainForm
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
                trayIcon.Dispose();
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
            this.components = new System.ComponentModel.Container();
            this.btnPrayer = new System.Windows.Forms.Button();
            this.btnShortMed = new System.Windows.Forms.Button();
            this.btnLongMed = new System.Windows.Forms.Button();
            this.btnSleep = new System.Windows.Forms.Button();
            this.btnCustom = new System.Windows.Forms.Button();
            this.dtCustom = new System.Windows.Forms.DateTimePicker();
            this.layoutPanel = new System.Windows.Forms.FlowLayoutPanel();
            this.timerContextMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.deleteToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.btnSermon = new System.Windows.Forms.Button();
            this.btnAlignment = new System.Windows.Forms.Button();
            this.checkLog = new System.Windows.Forms.CheckBox();
            this.timerContextMenu.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnPrayer
            // 
            this.btnPrayer.Location = new System.Drawing.Point(146, 12);
            this.btnPrayer.Name = "btnPrayer";
            this.btnPrayer.Size = new System.Drawing.Size(128, 23);
            this.btnPrayer.TabIndex = 1;
            this.btnPrayer.Text = "Prayer";
            this.btnPrayer.UseVisualStyleBackColor = true;
            this.btnPrayer.Click += new System.EventHandler(this.btnPrayer_Click);
            // 
            // btnShortMed
            // 
            this.btnShortMed.Location = new System.Drawing.Point(12, 12);
            this.btnShortMed.Name = "btnShortMed";
            this.btnShortMed.Size = new System.Drawing.Size(128, 23);
            this.btnShortMed.TabIndex = 0;
            this.btnShortMed.Text = "Short meditation";
            this.btnShortMed.UseVisualStyleBackColor = true;
            this.btnShortMed.Click += new System.EventHandler(this.btnShortMed_Click);
            // 
            // btnLongMed
            // 
            this.btnLongMed.Location = new System.Drawing.Point(12, 41);
            this.btnLongMed.Name = "btnLongMed";
            this.btnLongMed.Size = new System.Drawing.Size(128, 23);
            this.btnLongMed.TabIndex = 2;
            this.btnLongMed.Text = "Long meditation";
            this.btnLongMed.UseVisualStyleBackColor = true;
            this.btnLongMed.Click += new System.EventHandler(this.btnLongMed_Click);
            // 
            // btnSleep
            // 
            this.btnSleep.Location = new System.Drawing.Point(146, 41);
            this.btnSleep.Name = "btnSleep";
            this.btnSleep.Size = new System.Drawing.Size(128, 23);
            this.btnSleep.TabIndex = 3;
            this.btnSleep.Text = "Sleep bonus";
            this.btnSleep.UseVisualStyleBackColor = true;
            this.btnSleep.Click += new System.EventHandler(this.btnSleep_Click);
            // 
            // btnCustom
            // 
            this.btnCustom.Location = new System.Drawing.Point(12, 99);
            this.btnCustom.Name = "btnCustom";
            this.btnCustom.Size = new System.Drawing.Size(128, 23);
            this.btnCustom.TabIndex = 4;
            this.btnCustom.Text = "Custom";
            this.btnCustom.UseVisualStyleBackColor = true;
            this.btnCustom.Click += new System.EventHandler(this.btnCustom_Click);
            // 
            // dtCustom
            // 
            this.dtCustom.Format = System.Windows.Forms.DateTimePickerFormat.Time;
            this.dtCustom.Location = new System.Drawing.Point(146, 99);
            this.dtCustom.Name = "dtCustom";
            this.dtCustom.Size = new System.Drawing.Size(129, 20);
            this.dtCustom.TabIndex = 5;
            this.dtCustom.Value = new System.DateTime(2011, 7, 7, 0, 10, 0, 0);
            // 
            // layoutPanel
            // 
            this.layoutPanel.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.layoutPanel.AutoScroll = true;
            this.layoutPanel.Location = new System.Drawing.Point(12, 152);
            this.layoutPanel.Name = "layoutPanel";
            this.layoutPanel.Size = new System.Drawing.Size(263, 192);
            this.layoutPanel.TabIndex = 6;
            this.layoutPanel.ClientSizeChanged += new System.EventHandler(this.layoutPanel_ClientSizeChanged);
            this.layoutPanel.ControlAdded += new System.Windows.Forms.ControlEventHandler(this.layoutPanel_ControlAdded);
            // 
            // timerContextMenu
            // 
            this.timerContextMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.deleteToolStripMenuItem});
            this.timerContextMenu.Name = "timerContextMenu";
            this.timerContextMenu.Size = new System.Drawing.Size(106, 26);
            // 
            // deleteToolStripMenuItem
            // 
            this.deleteToolStripMenuItem.Name = "deleteToolStripMenuItem";
            this.deleteToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.deleteToolStripMenuItem.Text = "Delete";
            this.deleteToolStripMenuItem.Click += new System.EventHandler(this.deleteToolStripMenuItem_Click);
            // 
            // btnSermon
            // 
            this.btnSermon.Location = new System.Drawing.Point(146, 70);
            this.btnSermon.Name = "btnSermon";
            this.btnSermon.Size = new System.Drawing.Size(128, 23);
            this.btnSermon.TabIndex = 8;
            this.btnSermon.Text = "Sermon";
            this.btnSermon.UseVisualStyleBackColor = true;
            this.btnSermon.Click += new System.EventHandler(this.btnSermon_Click);
            // 
            // btnAlignment
            // 
            this.btnAlignment.Location = new System.Drawing.Point(12, 70);
            this.btnAlignment.Name = "btnAlignment";
            this.btnAlignment.Size = new System.Drawing.Size(128, 23);
            this.btnAlignment.TabIndex = 7;
            this.btnAlignment.Text = "Alignment gain";
            this.btnAlignment.UseVisualStyleBackColor = true;
            this.btnAlignment.Click += new System.EventHandler(this.btnAlignment_Click);
            // 
            // checkLog
            // 
            this.checkLog.AutoSize = true;
            this.checkLog.Location = new System.Drawing.Point(12, 129);
            this.checkLog.Name = "checkLog";
            this.checkLog.Size = new System.Drawing.Size(95, 17);
            this.checkLog.TabIndex = 9;
            this.checkLog.Text = "Check log files";
            this.checkLog.UseVisualStyleBackColor = true;
            this.checkLog.CheckedChanged += new System.EventHandler(this.checkLog_CheckedChanged);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(285, 356);
            this.Controls.Add(this.checkLog);
            this.Controls.Add(this.btnSermon);
            this.Controls.Add(this.btnAlignment);
            this.Controls.Add(this.layoutPanel);
            this.Controls.Add(this.btnSleep);
            this.Controls.Add(this.dtCustom);
            this.Controls.Add(this.btnCustom);
            this.Controls.Add(this.btnLongMed);
            this.Controls.Add(this.btnShortMed);
            this.Controls.Add(this.btnPrayer);
            this.Name = "MainForm";
            this.Text = "Wurm Timer";
            this.timerContextMenu.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnPrayer;
        private System.Windows.Forms.Button btnShortMed;
        private System.Windows.Forms.Button btnLongMed;
        private System.Windows.Forms.Button btnSleep;
        private System.Windows.Forms.Button btnCustom;
        private System.Windows.Forms.DateTimePicker dtCustom;
        private System.Windows.Forms.FlowLayoutPanel layoutPanel;
        private System.Windows.Forms.ContextMenuStrip timerContextMenu;
        private System.Windows.Forms.ToolStripMenuItem deleteToolStripMenuItem;
        private System.Windows.Forms.Button btnSermon;
        private System.Windows.Forms.Button btnAlignment;
        private System.Windows.Forms.CheckBox checkLog;
    }
}

