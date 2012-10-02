﻿using System;

namespace GitUI
{
    partial class FormCheckoutBranch
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
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.remoteOptionsPanel = new System.Windows.Forms.TableLayoutPanel();
            this.rbDontCreate = new System.Windows.Forms.RadioButton();
            this.rbCreateBranch = new System.Windows.Forms.RadioButton();
            this.rbResetBranch = new System.Windows.Forms.RadioButton();
            this.rbCreateBranchWithCustomName = new System.Windows.Forms.RadioButton();
            this.localChangesGB = new System.Windows.Forms.GroupBox();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.defaultActionChx = new System.Windows.Forms.CheckBox();
            this.rbMerge = new System.Windows.Forms.RadioButton();
            this.rbDontChange = new System.Windows.Forms.RadioButton();
            this.rbReset = new System.Windows.Forms.RadioButton();
            this.rbStash = new System.Windows.Forms.RadioButton();
            this.Ok = new System.Windows.Forms.Button();
            this.LocalBranch = new System.Windows.Forms.RadioButton();
            this.Remotebranch = new System.Windows.Forms.RadioButton();
            this.label1 = new System.Windows.Forms.Label();
            this.Branches = new System.Windows.Forms.ComboBox();
            this.txtCustomBranchName = new System.Windows.Forms.TextBox();
            this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
            this.tableLayoutPanel1.SuspendLayout();
            this.remoteOptionsPanel.SuspendLayout();
            this.localChangesGB.SuspendLayout();
            this.tableLayoutPanel2.SuspendLayout();
            this.flowLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.AutoSize = true;
            this.tableLayoutPanel1.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.tableLayoutPanel1.ColumnCount = 4;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel1.Controls.Add(this.remoteOptionsPanel, 0, 2);
            this.tableLayoutPanel1.Controls.Add(this.localChangesGB, 0, 3);
            this.tableLayoutPanel1.Controls.Add(this.Ok, 3, 3);
            this.tableLayoutPanel1.Controls.Add(this.LocalBranch, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.Remotebranch, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.label1, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.Branches, 1, 1);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.Padding = new System.Windows.Forms.Padding(7);
            this.tableLayoutPanel1.RowCount = 4;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(394, 262);
            this.tableLayoutPanel1.TabIndex = 0;
            this.tableLayoutPanel1.TabStop = true;
            // 
            // remoteOptionsPanel
            // 
            this.remoteOptionsPanel.AutoSize = true;
            this.remoteOptionsPanel.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.remoteOptionsPanel.ColumnCount = 1;
            this.tableLayoutPanel1.SetColumnSpan(this.remoteOptionsPanel, 4);
            this.remoteOptionsPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.remoteOptionsPanel.Controls.Add(this.flowLayoutPanel1, 0, 2);
            this.remoteOptionsPanel.Controls.Add(this.rbDontCreate, 0, 4);
            this.remoteOptionsPanel.Controls.Add(this.rbCreateBranch, 0, 3);
            this.remoteOptionsPanel.Controls.Add(this.rbResetBranch, 0, 0);
            this.remoteOptionsPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.remoteOptionsPanel.Location = new System.Drawing.Point(7, 67);
            this.remoteOptionsPanel.Margin = new System.Windows.Forms.Padding(0, 2, 0, 0);
            this.remoteOptionsPanel.Name = "remoteOptionsPanel";
            this.remoteOptionsPanel.RowCount = 5;
            this.remoteOptionsPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.remoteOptionsPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.remoteOptionsPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.remoteOptionsPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.remoteOptionsPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.remoteOptionsPanel.Size = new System.Drawing.Size(387, 102);
            this.remoteOptionsPanel.TabIndex = 2;
            this.remoteOptionsPanel.Visible = false;
            // 
            // rbDontCreate
            // 
            this.rbDontCreate.AutoSize = true;
            this.rbDontCreate.Location = new System.Drawing.Point(3, 80);
            this.rbDontCreate.Name = "rbDontCreate";
            this.rbDontCreate.Size = new System.Drawing.Size(157, 19);
            this.rbDontCreate.TabIndex = 3;
            this.rbDontCreate.Text = "Checkout remote branch";
            this.rbDontCreate.UseVisualStyleBackColor = true;
            // 
            // rbCreateBranch
            // 
            this.rbCreateBranch.AutoSize = true;
            this.rbCreateBranch.Location = new System.Drawing.Point(3, 55);
            this.rbCreateBranch.Margin = new System.Windows.Forms.Padding(3, 1, 3, 3);
            this.rbCreateBranch.Name = "rbCreateBranch";
            this.rbCreateBranch.Size = new System.Drawing.Size(229, 19);
            this.rbCreateBranch.TabIndex = 2;
            this.rbCreateBranch.Text = "Create local branch with the name \'{0}\'";
            this.rbCreateBranch.UseVisualStyleBackColor = true;
            // 
            // rbResetBranch
            // 
            this.rbResetBranch.AutoSize = true;
            this.rbResetBranch.Checked = true;
            this.rbResetBranch.Location = new System.Drawing.Point(3, 3);
            this.rbResetBranch.Name = "rbResetBranch";
            this.rbResetBranch.Size = new System.Drawing.Size(223, 19);
            this.rbResetBranch.TabIndex = 0;
            this.rbResetBranch.TabStop = true;
            this.rbResetBranch.Text = "Reset local branch with the name \'{0}\'";
            this.rbResetBranch.UseVisualStyleBackColor = true;
            // 
            // rbCreateBranchWithCustomName
            // 
            this.rbCreateBranchWithCustomName.AutoSize = true;
            this.rbCreateBranchWithCustomName.Location = new System.Drawing.Point(3, 3);
            this.rbCreateBranchWithCustomName.Margin = new System.Windows.Forms.Padding(3, 3, 3, 4);
            this.rbCreateBranchWithCustomName.Name = "rbCreateBranchWithCustomName";
            this.rbCreateBranchWithCustomName.Size = new System.Drawing.Size(232, 19);
            this.rbCreateBranchWithCustomName.TabIndex = 1;
            this.rbCreateBranchWithCustomName.Text = "Create local branch with custom name:";
            this.rbCreateBranchWithCustomName.UseVisualStyleBackColor = true;
            this.rbCreateBranchWithCustomName.CheckedChanged += new System.EventHandler(this.rbCreateBranchWithCustomName_CheckedChanged);
            // 
            // localChangesGB
            // 
            this.localChangesGB.AutoSize = true;
            this.tableLayoutPanel1.SetColumnSpan(this.localChangesGB, 2);
            this.localChangesGB.Controls.Add(this.tableLayoutPanel2);
            this.localChangesGB.Location = new System.Drawing.Point(9, 179);
            this.localChangesGB.Margin = new System.Windows.Forms.Padding(2, 10, 2, 2);
            this.localChangesGB.Name = "localChangesGB";
            this.localChangesGB.Padding = new System.Windows.Forms.Padding(6);
            this.localChangesGB.Size = new System.Drawing.Size(289, 79);
            this.localChangesGB.TabIndex = 26;
            this.localChangesGB.TabStop = false;
            this.localChangesGB.Text = "Local changes";
            // 
            // tableLayoutPanel2
            // 
            this.tableLayoutPanel2.AutoSize = true;
            this.tableLayoutPanel2.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.tableLayoutPanel2.ColumnCount = 4;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel2.Controls.Add(this.defaultActionChx, 0, 1);
            this.tableLayoutPanel2.Controls.Add(this.rbMerge, 0, 0);
            this.tableLayoutPanel2.Controls.Add(this.rbDontChange, 1, 0);
            this.tableLayoutPanel2.Controls.Add(this.rbReset, 3, 0);
            this.tableLayoutPanel2.Controls.Add(this.rbStash, 2, 0);
            this.tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel2.Location = new System.Drawing.Point(6, 22);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.RowCount = 2;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel2.Size = new System.Drawing.Size(277, 51);
            this.tableLayoutPanel2.TabIndex = 20;
            // 
            // defaultActionChx
            // 
            this.defaultActionChx.AutoSize = true;
            this.tableLayoutPanel2.SetColumnSpan(this.defaultActionChx, 4);
            this.defaultActionChx.Location = new System.Drawing.Point(3, 29);
            this.defaultActionChx.Margin = new System.Windows.Forms.Padding(3, 6, 3, 3);
            this.defaultActionChx.Name = "defaultActionChx";
            this.defaultActionChx.Size = new System.Drawing.Size(174, 19);
            this.defaultActionChx.TabIndex = 5;
            this.defaultActionChx.Text = "Remember as default action";
            this.defaultActionChx.UseVisualStyleBackColor = true;
            // 
            // rbMerge
            // 
            this.rbMerge.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.rbMerge.AutoSize = true;
            this.rbMerge.Location = new System.Drawing.Point(2, 2);
            this.rbMerge.Margin = new System.Windows.Forms.Padding(2);
            this.rbMerge.Name = "rbMerge";
            this.rbMerge.Size = new System.Drawing.Size(59, 19);
            this.rbMerge.TabIndex = 1;
            this.rbMerge.Text = "Merge";
            this.rbMerge.UseVisualStyleBackColor = true;
            // 
            // rbDontChange
            // 
            this.rbDontChange.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.rbDontChange.AutoSize = true;
            this.rbDontChange.Location = new System.Drawing.Point(65, 2);
            this.rbDontChange.Margin = new System.Windows.Forms.Padding(2);
            this.rbDontChange.Name = "rbDontChange";
            this.rbDontChange.Size = new System.Drawing.Size(96, 19);
            this.rbDontChange.TabIndex = 2;
            this.rbDontChange.Text = "Don\'t change";
            this.rbDontChange.UseVisualStyleBackColor = true;
            // 
            // rbReset
            // 
            this.rbReset.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.rbReset.AutoSize = true;
            this.rbReset.Location = new System.Drawing.Point(222, 2);
            this.rbReset.Margin = new System.Windows.Forms.Padding(2);
            this.rbReset.Name = "rbReset";
            this.rbReset.Size = new System.Drawing.Size(53, 19);
            this.rbReset.TabIndex = 4;
            this.rbReset.Text = "Reset";
            this.rbReset.UseVisualStyleBackColor = true;
            // 
            // rbStash
            // 
            this.rbStash.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.rbStash.AutoSize = true;
            this.rbStash.Location = new System.Drawing.Point(165, 2);
            this.rbStash.Margin = new System.Windows.Forms.Padding(2);
            this.rbStash.Name = "rbStash";
            this.rbStash.Size = new System.Drawing.Size(53, 19);
            this.rbStash.TabIndex = 3;
            this.rbStash.Text = "Stash";
            this.rbStash.UseVisualStyleBackColor = true;
            // 
            // Ok
            // 
            this.Ok.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.Ok.AutoSize = true;
            this.Ok.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.Ok.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.Ok.Location = new System.Drawing.Point(320, 229);
            this.Ok.Margin = new System.Windows.Forms.Padding(0, 10, 6, 6);
            this.Ok.Name = "Ok";
            this.Ok.Size = new System.Drawing.Size(68, 25);
            this.Ok.TabIndex = 23;
            this.Ok.Text = "Checkout";
            this.Ok.UseVisualStyleBackColor = true;
            this.Ok.Click += new System.EventHandler(this.OkClick);
            // 
            // LocalBranch
            // 
            this.LocalBranch.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.LocalBranch.AutoSize = true;
            this.LocalBranch.Checked = true;
            this.LocalBranch.Location = new System.Drawing.Point(9, 9);
            this.LocalBranch.Margin = new System.Windows.Forms.Padding(2);
            this.LocalBranch.Name = "LocalBranch";
            this.LocalBranch.Size = new System.Drawing.Size(93, 19);
            this.LocalBranch.TabIndex = 0;
            this.LocalBranch.TabStop = true;
            this.LocalBranch.Text = "Local branch";
            this.LocalBranch.UseVisualStyleBackColor = true;
            this.LocalBranch.CheckedChanged += new System.EventHandler(this.LocalBranchCheckedChanged);
            // 
            // Remotebranch
            // 
            this.Remotebranch.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.Remotebranch.AutoSize = true;
            this.Remotebranch.Location = new System.Drawing.Point(106, 9);
            this.Remotebranch.Margin = new System.Windows.Forms.Padding(2);
            this.Remotebranch.Name = "Remotebranch";
            this.Remotebranch.Padding = new System.Windows.Forms.Padding(6, 0, 0, 0);
            this.Remotebranch.Size = new System.Drawing.Size(112, 19);
            this.Remotebranch.TabIndex = 0;
            this.Remotebranch.Text = "Remote branch";
            this.Remotebranch.UseVisualStyleBackColor = true;
            this.Remotebranch.CheckedChanged += new System.EventHandler(this.RemoteBranchCheckedChanged);
            // 
            // label1
            // 
            this.label1.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(23, 45);
            this.label1.Margin = new System.Windows.Forms.Padding(2, 10, 3, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(78, 15);
            this.label1.TabIndex = 7;
            this.label1.Text = "Select branch";
            // 
            // Branches
            // 
            this.Branches.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.Branches.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.Branches.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.tableLayoutPanel1.SetColumnSpan(this.Branches, 3);
            this.Branches.FormattingEnabled = true;
            this.Branches.Location = new System.Drawing.Point(106, 40);
            this.Branches.Margin = new System.Windows.Forms.Padding(2, 10, 6, 2);
            this.Branches.Name = "Branches";
            this.Branches.Size = new System.Drawing.Size(282, 23);
            this.Branches.TabIndex = 1;
            this.Branches.SelectedIndexChanged += new System.EventHandler(this.Branches_SelectedIndexChanged);
            // 
            // txtCustomBranchName
            // 
            this.txtCustomBranchName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtCustomBranchName.Enabled = false;
            this.txtCustomBranchName.Location = new System.Drawing.Point(240, 2);
            this.txtCustomBranchName.Margin = new System.Windows.Forms.Padding(2);
            this.txtCustomBranchName.Name = "txtCustomBranchName";
            this.txtCustomBranchName.Size = new System.Drawing.Size(141, 23);
            this.txtCustomBranchName.TabIndex = 23;
            // 
            // flowLayoutPanel1
            // 
            this.flowLayoutPanel1.AutoScroll = true;
            this.flowLayoutPanel1.Controls.Add(this.rbCreateBranchWithCustomName);
            this.flowLayoutPanel1.Controls.Add(this.txtCustomBranchName);
            this.flowLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.flowLayoutPanel1.Location = new System.Drawing.Point(0, 25);
            this.flowLayoutPanel1.Margin = new System.Windows.Forms.Padding(0);
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            this.flowLayoutPanel1.Size = new System.Drawing.Size(387, 29);
            this.flowLayoutPanel1.TabIndex = 1;
            this.flowLayoutPanel1.WrapContents = false;
            // 
            // FormCheckoutBranch
            // 
            this.AcceptButton = this.Ok;
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoSize = true;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.ClientSize = new System.Drawing.Size(394, 262);
            this.Controls.Add(this.tableLayoutPanel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormCheckoutBranch";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Checkout branch";
            this.Activated += new System.EventHandler(this.FormCheckoutBranch_Activated);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.remoteOptionsPanel.ResumeLayout(false);
            this.remoteOptionsPanel.PerformLayout();
            this.localChangesGB.ResumeLayout(false);
            this.localChangesGB.PerformLayout();
            this.tableLayoutPanel2.ResumeLayout(false);
            this.tableLayoutPanel2.PerformLayout();
            this.flowLayoutPanel1.ResumeLayout(false);
            this.flowLayoutPanel1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.RadioButton LocalBranch;
        private System.Windows.Forms.RadioButton Remotebranch;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox Branches;
        private System.Windows.Forms.Button Ok;
        private System.Windows.Forms.GroupBox localChangesGB;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private System.Windows.Forms.RadioButton rbDontChange;
        private System.Windows.Forms.RadioButton rbStash;
        private System.Windows.Forms.RadioButton rbReset;
        private System.Windows.Forms.RadioButton rbMerge;
        private System.Windows.Forms.CheckBox defaultActionChx;
        private System.Windows.Forms.TableLayoutPanel remoteOptionsPanel;
        private System.Windows.Forms.RadioButton rbDontCreate;
        private System.Windows.Forms.RadioButton rbCreateBranch;
        private System.Windows.Forms.RadioButton rbResetBranch;
        private System.Windows.Forms.RadioButton rbCreateBranchWithCustomName;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
        private System.Windows.Forms.TextBox txtCustomBranchName;
    }
}
