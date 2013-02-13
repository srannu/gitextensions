﻿using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace WindowsFormsApplication1
{
    partial class Form1
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private IContainer components = null;

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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.statusStrip = new System.Windows.Forms.StatusStrip();
            this.statusFiller = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatusLabel1 = new System.Windows.Forms.ToolStripStatusLabel();
            this.statusNumNotifications = new System.Windows.Forms.ToolStripDropDownButton();
            this.warningToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.failureToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.statusStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // statusStrip
            // 
            this.statusStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.statusNumNotifications,
            this.statusFiller,
            this.toolStripStatusLabel1});
            this.statusStrip.Location = new System.Drawing.Point(0, 70);
            this.statusStrip.Name = "statusStrip";
            this.statusStrip.Size = new System.Drawing.Size(706, 22);
            this.statusStrip.TabIndex = 5;
            // 
            // statusFiller
            // 
            this.statusFiller.Name = "statusFiller";
            this.statusFiller.Size = new System.Drawing.Size(604, 17);
            this.statusFiller.Spring = true;
            // 
            // toolStripStatusLabel1
            // 
            this.toolStripStatusLabel1.Name = "toolStripStatusLabel1";
            this.toolStripStatusLabel1.Size = new System.Drawing.Size(14, 17);
            this.toolStripStatusLabel1.Text = "X";
            this.toolStripStatusLabel1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // statusNumNotifications
            // 
            this.statusNumNotifications.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.failureToolStripMenuItem,
            this.warningToolStripMenuItem});
            this.statusNumNotifications.Image = ((System.Drawing.Image)(resources.GetObject("statusNumNotifications.Image")));
            this.statusNumNotifications.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.statusNumNotifications.Name = "statusNumNotifications";
            this.statusNumNotifications.Size = new System.Drawing.Size(42, 20);
            this.statusNumNotifications.Text = "5";
            // 
            // warningToolStripMenuItem
            // 
            this.warningToolStripMenuItem.Name = "warningToolStripMenuItem";
            this.warningToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.warningToolStripMenuItem.Text = "Warning";
            // 
            // failureToolStripMenuItem
            // 
            this.failureToolStripMenuItem.Name = "failureToolStripMenuItem";
            this.failureToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.failureToolStripMenuItem.Text = "Failure";
            // 
            // Form1
            // 
            this.ClientSize = new System.Drawing.Size(706, 92);
            this.Controls.Add(this.statusStrip);
            this.Name = "Form1";
            this.Text = "Form1";
            this.statusStrip.ResumeLayout(false);
            this.statusStrip.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private StatusStrip statusStrip;
        private ToolStripStatusLabel statusFiller;
        private ToolStripStatusLabel toolStripStatusLabel1;
        private ToolStripDropDownButton statusNumNotifications;
        private ToolStripMenuItem failureToolStripMenuItem;
        private ToolStripMenuItem warningToolStripMenuItem;
    }
}

