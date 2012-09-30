﻿using System;
using System.Linq;
using System.Windows.Forms;
using GitCommands;
using ResourceManager.Translation;

namespace GitUI
{
    public partial class FormSubmodules : GitExtensionsForm
    {
        private readonly TranslationString _removeSelectedSubmodule =
             new TranslationString("Are you sure you want remove the selected submodule?");

        private readonly TranslationString _removeSelectedSubmoduleCaption = new TranslationString("Remove");

        public FormSubmodules()
        {
            InitializeComponent();
            Translate();
        }

        private void AddSubmoduleClick(object sender, EventArgs e)
        {
            using (var formAddSubmodule = new FormAddSubmodule())
                formAddSubmodule.ShowDialog(this);
            Initialize();
        }

        private void FormSubmodulesShown(object sender, EventArgs e)
        {
            Initialize();
        }

        private void Initialize()
        {
            Cursor.Current = Cursors.WaitCursor;
            var submodule = Submodules.SelectedRows.Count == 1 ? Submodules.SelectedRows[0].DataBoundItem as GitSubmodule : null;
            Submodules.DataSource = GitModule.Current.GetSubmodules();
            if (submodule != null)
            {
                DataGridViewRow row = Submodules.Rows
                    .Cast<DataGridViewRow>()
                    .FirstOrDefault(r => r.DataBoundItem as GitSubmodule == submodule);

                if (row != null)
                    row.Selected = true;
            }
            Cursor.Current = Cursors.Default;
        }

        private void SubmodulesSelectionChanged(object sender, EventArgs e)
        {
            if (Submodules.SelectedRows.Count != 1)
                return;

            var submodule = Submodules.SelectedRows[0].DataBoundItem as GitSubmodule;
            if (submodule == null)
                return;

            Cursor.Current = Cursors.WaitCursor;
            SubModuleName.Text = submodule.Name;
            SubModuleRemotePath.Text = submodule.RemotePath;
            SubModuleLocalPath.Text = submodule.LocalPath;
            SubModuleCommit.Text = submodule.CurrentCommitGuid;
            SubModuleBranch.Text = submodule.Branch;
            SubModuleStatus.Text = submodule.Status;
            Cursor.Current = Cursors.Default;
        }

        private void SynchronizeSubmoduleClick(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            FormProcess.ShowDialog(this, GitCommandHelpers.SubmoduleSyncCmd(SubModuleName.Text));
            Initialize();
            Cursor.Current = Cursors.Default;
        }

        private void UpdateSubmoduleClick(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            FormProcess.ShowDialog(this, GitCommandHelpers.SubmoduleUpdateCmd(SubModuleName.Text));
            Initialize();
            Cursor.Current = Cursors.Default;
        }

        private void RemoveSubmoduleClick(object sender, EventArgs e)
        {
            if (Submodules.SelectedRows.Count != 1 ||
                MessageBox.Show(this, _removeSelectedSubmodule.Text, _removeSelectedSubmoduleCaption.Text, MessageBoxButtons.YesNo) !=
                DialogResult.Yes)
                return;

            Cursor.Current = Cursors.WaitCursor;
            GitModule.Current.RunGitCmd("rm --cached \"" + SubModuleName.Text + "\"");

            var modules = GitModule.Current.GetSubmoduleConfigFile();
            modules.RemoveConfigSection("submodule \"" + SubModuleName.Text + "\"");
            if (modules.GetConfigSections().Count > 0)
                modules.Save();
            else
                GitModule.Current.RunGitCmd("rm --cached \".gitmodules\"");

            var configFile = GitModule.Current.GetLocalConfig();
            configFile.RemoveConfigSection("submodule \"" + SubModuleName.Text + "\"");
            configFile.Save();

            Initialize();
            Cursor.Current = Cursors.Default;
        }
    }
}