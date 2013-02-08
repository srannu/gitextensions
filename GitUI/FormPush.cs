﻿using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using GitCommands;
using GitCommands.Repository;
using GitUI.RepoHosting;
using GitUI.Script;
using ResourceManager.Translation;
using GitCommands.Config;

namespace GitUI
{
    public partial class FormPush : GitModuleForm
    {
        private const string PuttyText = "PuTTY";
        private const string HeadText = "HEAD";
        private string _currentBranch;
        private string _currentBranchRemote;
        private bool candidateForRebasingMergeCommit;
        private string selectedBranch;
        private string selectedBranchRemote;
        private string selectedRemoteBranchName;

        /// <summary>Indicates whether an error occurred.</summary>
        public bool ErrorOccurred { get; private set; }

        /// <summary>Gets or sets the selected remote.</summary>
        string SelectedRemote
        {
            get { return _NO_TRANSLATE_Remotes.Text; }
            set { _NO_TRANSLATE_Remotes.Text = value; }
        }

        #region Translation
        private readonly TranslationString _branchNewForRemote =
            new TranslationString("The branch you are about to push seems to be a new branch for the remote." +
                                  Environment.NewLine + "Are you sure you want to push this branch?");

        private readonly TranslationString _cannotLoadPutty =
            new TranslationString("Cannot load SSH key. PuTTY is not configured properly.");

        private readonly TranslationString _pushCaption = new TranslationString("Push");

        private readonly TranslationString _pushToCaption = new TranslationString("Push to {0}");

        private readonly TranslationString _selectDestinationDirectory =
            new TranslationString("Please select a destination directory");

        private readonly TranslationString _selectRemote = new TranslationString("Please select a remote repository");

        /// <summary>need to select a tag</summary>
        private readonly TranslationString _selectTag =
            new TranslationString("You need to select a tag to push or select \"Push all tags\".");

        private readonly TranslationString _updateTrackingReference =
            new TranslationString("The branch {0} does not have a tracking reference. Do you want to add a tracking reference to {1}?");

        private readonly TranslationString _yes = new TranslationString("Yes");
        private readonly TranslationString _no = new TranslationString("No");
        #endregion

        private FormPush()
            : this(null as GitUICommands) { }

        public FormPush(GitUICommands aCommands)
            : base(aCommands)
        {
            InitializeComponent();
            Translate();

            //can't be set in OnLoad, because after PushAndShowDialogWhenFailed()
            //they are reset to false
            PushAllTags.Checked = Settings.PushAllTags;
            AutoPullOnRejected.Checked = Settings.AutoPullOnRejected;
            if (aCommands != null)
                Init();
        }

        public FormPush(GitUICommands uiCommands, GitPush push)
            : this(uiCommands)
        {

        }

        private void Init()
        {
            if (GitCommandHelpers.VersionInUse.SupportPushWithRecursiveSubmodulesCheck)
            {
                RecursiveSubmodules.Enabled = true;
                RecursiveSubmodules.SelectedIndex = Settings.RecursiveSubmodules;
                if (!GitCommandHelpers.VersionInUse.SupportPushWithRecursiveSubmodulesOnDemand)
                    RecursiveSubmodules.Items.RemoveAt(2);
            }
            else
            {
                RecursiveSubmodules.Enabled = false;
                RecursiveSubmodules.SelectedIndex = 0;
            }

            _currentBranch = Module.GetSelectedBranch();

            _NO_TRANSLATE_Remotes.DataSource = Module.GetRemotes();

            UpdateBranchDropDown();
            UpdateRemoteBranchDropDown();

            Push.Focus();

            _currentBranchRemote = Module.GetSetting(string.Format("branch.{0}.remote", _currentBranch));
            if (_currentBranchRemote.IsNullOrEmpty() && _NO_TRANSLATE_Remotes.Items.Count >= 2)
            {
                IList<string> remotes = (IList<string>)_NO_TRANSLATE_Remotes.DataSource;
                int i = remotes.IndexOf("origin");
                _NO_TRANSLATE_Remotes.SelectedIndex = i >= 0 ? i : 0;
            }
            else
                SelectedRemote = _currentBranchRemote;
            RemotesUpdated(null, null);
        }

        public DialogResult PushAndShowDialogWhenFailed(IWin32Window owner)
        {
            if (!PushChanges(owner))
                return ShowDialog(owner);
            return DialogResult.OK;
        }

        public DialogResult PushAndShowDialogWhenFailed()
        {
            return PushAndShowDialogWhenFailed(null);
        }

        private void PushClick(object sender, EventArgs e)
        {
            if (PushChanges(this))
                Close();
        }

        /// <summary>Default push config.</summary>
        class DefaultPushConfig
        {
            public string LocalBranch;
            public string RemoteBranch;
            DefaultPushConfig(string setting)
            {
                var values = setting.Split(':');
                if (values.Length >= 1)
                    LocalBranch = values[0];
                if (values.Length >= 2)// remote ref (branch)
                    RemoteBranch = values[1];
            }

            public static DefaultPushConfig TryParse(GitModule git, string remote)
            {
                //Get default push for this remote (if any). Local branch name is left of ":"
                var pushSettingValue = git.GetSetting(string.Format("remote.{0}.push", remote));
                return string.IsNullOrEmpty(pushSettingValue)
                    ? null
                    : new DefaultPushConfig(pushSettingValue);
            }
        }

        /// <summary>Gets the configured default remote ref (branch) to push to, or null if not config'd.</summary>
        DefaultPushConfig GetDefaultPush(string remote)
        {
            return DefaultPushConfig.TryParse(Module, remote);
        }

        /// <summary>Tries to push (given the selected options) and returns true if successful.
        /// <remarks>Returns false if the there are invalid options or the push fails.</remarks></summary>
        bool PushChanges(IWin32Window owner)
        {
            ErrorOccurred = false;
            if (PushToUrl.Checked && string.IsNullOrEmpty(PushDestination.Text))
            {
                MessageBox.Show(owner, _selectDestinationDirectory.Text);
                return false;
            }
            if (PushToRemote.Checked && string.IsNullOrEmpty(SelectedRemote))
            {
                MessageBox.Show(owner, _selectRemote.Text);
                return false;
            }
            if (TabControlTagBranch.SelectedTab == TagTab && string.IsNullOrEmpty(TagComboBox.Text) &&
                !PushAllTags.Checked)
            {// NO tag selected AND "Push all tags" not selected
                MessageBox.Show(owner, _selectTag.Text);
                return false;
            }

            //Extra check if the branch is already known to the remote, give a warning when not.
            //This is not possible when the remote is an URL, but this is ok since most users push to
            //known remotes anyway.
            if (TabControlTagBranch.SelectedTab == BranchTab && PushToRemote.Checked)
            {
                //If the current branch is not the default push, and not known by the remote 
                //(as far as we know since we are disconnected....)
                DefaultPushConfig defaultPush = GetDefaultPush(SelectedRemote);
                if (defaultPush == null || RemoteBranch.Text != defaultPush.RemoteBranch &&
                    !Module.GetHeads(true, true).Any(x => x.Remote == SelectedRemote && x.LocalName == RemoteBranch.Text))
                    //Ask if this is really what the user wants
                    if (!Settings.DontConfirmPushNewBranch)
                        if (MessageBox.Show(owner, _branchNewForRemote.Text, _pushCaption.Text, MessageBoxButtons.YesNo) ==
                            DialogResult.No)
                        {
                            return false;
                        }
            }

            if (PushToUrl.Checked)
                Repositories.AddMostRecentRepository(PushDestination.Text);
            Settings.PushAllTags = PushAllTags.Checked;
            Settings.AutoPullOnRejected = AutoPullOnRejected.Checked;
            Settings.RecursiveSubmodules = RecursiveSubmodules.SelectedIndex;

            string remote = "";
            string destination;
            if (PushToUrl.Checked)
            {
                destination = PushDestination.Text;
            }
            else
            {
                if (GitCommandHelpers.Plink())
                {
                    if (!File.Exists(Settings.Pageant))
                        MessageBox.Show(owner, _cannotLoadPutty.Text, PuttyText);
                    else
                        Module.StartPageantForRemote(SelectedRemote);
                }

                destination = SelectedRemote;
                remote = SelectedRemote.Trim();
            }

            string pushCmd;
            if (TabControlTagBranch.SelectedTab == BranchTab)
            {
                bool track = ReplaceTrackingReference.Checked;
                if (!track && !string.IsNullOrWhiteSpace(RemoteBranch.Text))
                {
                    GitHead selectedLocalBranch = _NO_TRANSLATE_Branch.SelectedItem as GitHead;
                    track = selectedLocalBranch != null && string.IsNullOrEmpty(selectedLocalBranch.TrackingRemote);

                    string[] remotes = _NO_TRANSLATE_Remotes.DataSource as string[];
                    if (remotes != null)
                        foreach (string remoteBranch in remotes)
                            if (!string.IsNullOrEmpty(remoteBranch) && _NO_TRANSLATE_Branch.Text.StartsWith(remoteBranch))
                                track = false;

                    if (track && !Settings.DontConfirmAddTrackingRef)
                    {
                        DialogResult result = MessageBox.Show(String.Format(_updateTrackingReference.Text, selectedLocalBranch.Name, RemoteBranch.Text), _pushCaption.Text, MessageBoxButtons.YesNoCancel);

                        if (result == DialogResult.Cancel)
                            return false;

                        track = result == DialogResult.Yes;
                    }
                }

                pushCmd = GitCommandHelpers.PushCmd(destination, _NO_TRANSLATE_Branch.Text, RemoteBranch.Text,
                    PushAllBranches.Checked, ForcePushBranches.Checked, track, RecursiveSubmodules.SelectedIndex);
            }
            else if (TabControlTagBranch.SelectedTab == TagTab)
                pushCmd = GitCommandHelpers.PushTagCmd(destination, TagComboBox.Text, PushAllTags.Checked,
                                                             ForcePushBranches.Checked);
            else
            {// Push Multiple Branches Tab selected
                var pushActions = new List<GitPushAction>();
                foreach (DataRow row in _branchTable.Rows)
                {
                    var push = Convert.ToBoolean(row["Push"]);
                    var force = Convert.ToBoolean(row["Force"]);
                    var delete = Convert.ToBoolean(row["Delete"]);

                    if (push || force)
                        pushActions.Add(new GitPushAction(row["Local"].ToString(), row["Remote"].ToString(), force));
                    else if (delete)
                        pushActions.Add(GitPushAction.DeleteRemoteBranch(row["Remote"].ToString()));
                }
                pushCmd = GitCommandHelpers.PushMultipleCmd(destination, pushActions);
            }

            ScriptManager.RunEventScripts(Module, ScriptEvent.BeforePush);

            //controls can be accessed only from UI thread
            candidateForRebasingMergeCommit = Settings.PullMerge == Settings.PullAction.Rebase && PushToRemote.Checked && !PushAllBranches.Checked && TabControlTagBranch.SelectedTab == BranchTab;
            selectedBranch = _NO_TRANSLATE_Branch.Text;
            selectedBranchRemote = SelectedRemote;
            selectedRemoteBranchName = RemoteBranch.Text;

            using (var form = new FormRemoteProcess(Module, pushCmd)
                       {
                           Remote = remote,
                           Text = string.Format(_pushToCaption.Text, destination),
                           HandleOnExitCallback = HandlePushOnExit
                       })
            {

                form.ShowDialog(owner);
                ErrorOccurred = form.ErrorOccurred();

                if (!Module.InTheMiddleOfConflictedMerge() &&
                    !Module.InTheMiddleOfRebase() && !form.ErrorOccurred())
                {
                    ScriptManager.RunEventScripts(Module, ScriptEvent.AfterPush);
                    if (_createPullRequestCB.Checked)
                        UICommands.StartCreatePullRequest(owner);
                    return true;
                }
            }

            return false;
        }


        private bool IsRebasingMergeCommit()
        {
            if (candidateForRebasingMergeCommit)
            {
                if (selectedBranch == _currentBranch && selectedBranchRemote == _currentBranchRemote)
                {
                    string remoteBranchName = selectedBranchRemote + "/" + selectedRemoteBranchName;
                    return Module.ExistsMergeCommit(remoteBranchName, selectedBranch);
                }
                else
                    return false;
            }
            else
                return false;
        }

        private bool HandlePushOnExit(ref bool isError, FormProcess form)
        {
            if (!isError)
                return false;

            //auto pull only if current branch was rejected
            Regex IsRejected = new Regex(Regex.Escape("! [rejected] ") + ".*" + Regex.Escape(_currentBranch) + ".*" + Regex.Escape(" (non-fast-forward)"), RegexOptions.Compiled);

            if (Settings.AutoPullOnRejected && IsRejected.IsMatch(form.GetOutputString()))
            {
                if (Settings.PullMerge == Settings.PullAction.Fetch)
                    form.AppendOutputLine(Environment.NewLine + "Can not perform auto pull, when merge option is set to fetch.");
                else if (IsRebasingMergeCommit())
                    form.AppendOutputLine(Environment.NewLine + "Can not perform auto pull, when merge option is set to rebase " + Environment.NewLine
                                        + "and one of the commits that are about to be rebased is a merge.");
                else
                {
                    bool pullCompleted;
                    UICommands.StartPullDialog(form.Owner ?? form, true, out pullCompleted);
                    if (pullCompleted)
                    {
                        form.Retry();
                        return true;
                    }
                }
            }

            return false;
        }

        private void FillPushDestinationDropDown()
        {
            PushDestination.DataSource = Repositories.RemoteRepositoryHistory.Repositories;
            PushDestination.DisplayMember = "Path";
        }

        private void UpdateBranchDropDown()
        {
            var curBranch = _NO_TRANSLATE_Branch.Text;

            _NO_TRANSLATE_Branch.DisplayMember = "Name";
            _NO_TRANSLATE_Branch.Items.Clear();
            _NO_TRANSLATE_Branch.Items.Add(HeadText);

            if (string.IsNullOrEmpty(curBranch))
            {
                curBranch = _currentBranch;
                if (curBranch.IndexOfAny("() ".ToCharArray()) != -1)
                    curBranch = HeadText;
            }

            foreach (var head in Module.GetHeads(false, true))
                _NO_TRANSLATE_Branch.Items.Add(head);

            _NO_TRANSLATE_Branch.Text = curBranch;
        }

        private void PullClick(object sender, EventArgs e)
        {
            UICommands.StartPullDialog(this);
        }

        private void UpdateRemoteBranchDropDown()
        {
            RemoteBranch.DisplayMember = "Name";
            RemoteBranch.Items.Clear();

            if (!string.IsNullOrEmpty(_NO_TRANSLATE_Branch.Text))
                RemoteBranch.Items.Add(_NO_TRANSLATE_Branch.Text);

            foreach (var head in Module.GetHeads(false, true))
                if (!RemoteBranch.Items.Contains(head))
                    RemoteBranch.Items.Add(head);
        }

        private void BranchSelectedValueChanged(object sender, EventArgs e)
        {
            if (_NO_TRANSLATE_Branch.Text != HeadText)
            {
                if (PushToRemote.Checked)
                {
                    var branch = _NO_TRANSLATE_Branch.SelectedItem as GitHead;
                    if (branch != null && branch.TrackingRemote.Equals(SelectedRemote.Trim()))
                    {
                        RemoteBranch.Text = branch.MergeWith;
                        if (!string.IsNullOrEmpty(RemoteBranch.Text))
                            return;
                    }
                }

                RemoteBranch.Text = _NO_TRANSLATE_Branch.Text;
            }
        }

        private void FormPushLoad(object sender, EventArgs e)
        {
            _NO_TRANSLATE_Remotes.Select();

            Text = string.Concat(_pushCaption.Text, " (", Module.WorkingDir, ")");

            var gitHoster = RepoHosts.TryGetGitHosterForModule(Module);
            _createPullRequestCB.Enabled = gitHoster != null;
        }

        private void AddRemoteClick(object sender, EventArgs e)
        {
            UICommands.StartRemotesDialog(this, SelectedRemote);
            string origText = SelectedRemote;
            _NO_TRANSLATE_Remotes.DataSource = Module.GetRemotes();
            if (_NO_TRANSLATE_Remotes.Items.Contains(origText)) // else first item gets selected
            {
                SelectedRemote = origText;
            }
        }

        private void PushToRemoteCheckedChanged(object sender, EventArgs e)
        {
            BranchSelectedValueChanged(null, null);
            if (!PushToRemote.Checked)
                return;

            PushDestination.Enabled = false;
            folderBrowserButton1.Enabled = false;
            _NO_TRANSLATE_Remotes.Enabled = true;
            AddRemote.Enabled = true;
        }

        private void PushToUrlCheckedChanged(object sender, EventArgs e)
        {
            if (!PushToUrl.Checked)
                return;

            PushDestination.Enabled = true;
            folderBrowserButton1.Enabled = true;
            _NO_TRANSLATE_Remotes.Enabled = false;
            AddRemote.Enabled = false;

            FillPushDestinationDropDown();
        }

        private void RemotesUpdated(object sender, EventArgs e)
        {
            if (TabControlTagBranch.SelectedTab == MultipleBranchTab)
                UpdateMultiBranchView();

            EnableLoadSshButton();

            // update the text box of the Remote Url combobox to show the URL of selected remote
            {
                string pushUrl = Module.GetPathSetting(string.Format(SettingKeyString.RemotePushUrl, SelectedRemote));
                if (pushUrl.IsNullOrEmpty())
                {
                    pushUrl = Module.GetPathSetting(string.Format(SettingKeyString.RemoteUrl, SelectedRemote));
                }
                PushDestination.Text = pushUrl;
            }

            var pushSettingValue = Module.GetSetting(string.Format("remote.{0}.push", SelectedRemote));

            if (PushToRemote.Checked && !string.IsNullOrEmpty(pushSettingValue))
            {
                DefaultPushConfig defaultPush = GetDefaultPush(SelectedRemote);

                RemoteBranch.Text = "";
                if (!string.IsNullOrEmpty(defaultPush.LocalBranch))
                {
                    var currentBranch = new GitHead(Module, null, defaultPush.LocalBranch, SelectedRemote);
                    _NO_TRANSLATE_Branch.Items.Add(currentBranch);
                    _NO_TRANSLATE_Branch.SelectedItem = currentBranch;
                }
                if (!string.IsNullOrEmpty(defaultPush.RemoteBranch))
                    RemoteBranch.Text = defaultPush.RemoteBranch;
                return;
            }

            if (string.IsNullOrEmpty(_NO_TRANSLATE_Branch.Text))
            {
                // Doing this makes it pretty easy to accidentally create a branch on the remote.
                // But leaving it blank will do the 'default' thing, meaning all branches are pushed.
                // Solution: when pushing a branch that doesn't exist on the remote, ask what to do
                var currentBranch = new GitHead(Module, null, _currentBranch, SelectedRemote);
                _NO_TRANSLATE_Branch.Items.Add(currentBranch);
                _NO_TRANSLATE_Branch.SelectedItem = currentBranch;
                return;
            }

            BranchSelectedValueChanged(null, null);
        }

        private void EnableLoadSshButton()
        {
            LoadSSHKey.Visible = !string.IsNullOrEmpty(Module.GetPuttyKeyFileForRemote(SelectedRemote));
        }

        private void LoadSshKeyClick(object sender, EventArgs e)
        {
            if (!File.Exists(Settings.Pageant))
                MessageBox.Show(this, _cannotLoadPutty.Text, PuttyText);
            else
                Module.StartPageantForRemote(SelectedRemote);
        }

        private void RemotesValidated(object sender, EventArgs e)
        {
            EnableLoadSshButton();
        }

        private void FillTagDropDown()
        {
            TagComboBox.DisplayMember = "Name";
            /// var tags = Module.GetTagHeads(GitModule.GetTagHeadsOption.OrderByCommitDateDescending); // comment out to sort by commit date
            var tags = Module.GetTagHeads(GitModule.GetTagHeadsSortOrder.ByName);
            TagComboBox.DataSource = tags;
        }

        private void ForcePushBranchesCheckedChanged(object sender, EventArgs e)
        {
            ForcePushTags.Checked = ForcePushBranches.Checked;
        }

        private void ForcePushTagsCheckedChanged(object sender, EventArgs e)
        {
            ForcePushBranches.Checked = ForcePushTags.Checked;
        }

        private void PushAllBranchesCheckedChanged(object sender, EventArgs e)
        {
            _NO_TRANSLATE_Branch.Enabled = !PushAllBranches.Checked;
            RemoteBranch.Enabled = !PushAllBranches.Checked;
        }

        #region Multi-Branch Methods

        private DataTable _branchTable;

        private void UpdateMultiBranchView()
        {
            _branchTable = new DataTable();
            _branchTable.Columns.Add("Local", typeof(string));
            _branchTable.Columns.Add("Remote", typeof(string));
            _branchTable.Columns.Add("New", typeof(string));
            _branchTable.Columns.Add("Push", typeof(bool));
            _branchTable.Columns.Add("Force", typeof(bool));
            _branchTable.Columns.Add("Delete", typeof(bool));
            _branchTable.ColumnChanged += BranchTable_ColumnChanged;
            var bs = new BindingSource { DataSource = _branchTable };
            BranchGrid.DataSource = bs;

            string remote = SelectedRemote.Trim();
            if (remote == "")
                return;

            var localHeads = Module.GetHeads(false, true);
            var remoteHeads = Module.GetRemoteHeads(remote, false, true);

            // Add all the local branches.
            foreach (var head in localHeads)
            {
                DataRow row = _branchTable.NewRow();
                row["Force"] = false;
                row["Delete"] = false;
                row["Local"] = head.Name;

                string remoteName;
                if (head.Remote == remote)
                    remoteName = head.MergeWith ?? head.Name;
                else
                    remoteName = head.Name;

                row["Remote"] = remoteName;
                bool newAtRemote = remoteHeads.Any(h => h.Name == remoteName);
                row["New"] = newAtRemote ? _no.Text : _yes.Text;
                row["Push"] = newAtRemote;

                _branchTable.Rows.Add(row);
            }

            // Offer to delete all the left over remote branches.
            foreach (var remoteHead in remoteHeads)
            {
                GitHead head = remoteHead;
                if (!localHeads.Any(h => h.Name == head.Name))
                {
                    DataRow row = _branchTable.NewRow();
                    row["Local"] = null;
                    row["Remote"] = remoteHead.Name;
                    row["New"] = _no.Text;
                    row["Push"] = false;
                    row["Force"] = false;
                    row["Delete"] = false;
                    _branchTable.Rows.Add(row);
                }
            }
        }

        static void BranchTable_ColumnChanged(object sender, DataColumnChangeEventArgs e)
        {
            if (e.Column.ColumnName == "Push" && (bool)e.ProposedValue)
            {
                e.Row["Force"] = false;
                e.Row["Delete"] = false;
            }
            if (e.Column.ColumnName == "Force" && (bool)e.ProposedValue)
            {
                e.Row["Push"] = false;
                e.Row["Delete"] = false;
            }
            if (e.Column.ColumnName == "Delete" && (bool)e.ProposedValue)
            {
                e.Row["Push"] = false;
                e.Row["Force"] = false;
            }
        }

        private void TabControlTagBranch_Selected(object sender, TabControlEventArgs e)
        {
            if (TabControlTagBranch.SelectedTab == MultipleBranchTab)
                UpdateMultiBranchView();
            else if (TabControlTagBranch.SelectedTab == TagTab)
                FillTagDropDown();
            else
            {
                UpdateBranchDropDown();
                UpdateRemoteBranchDropDown();
            }
        }

        private void BranchGrid_CurrentCellDirtyStateChanged(object sender, EventArgs e)
        {
            // Push grid checkbox changes immediately into the underlying data table.
            if (BranchGrid.CurrentCell is DataGridViewCheckBoxCell)
            {
                BranchGrid.EndEdit();
                ((BindingSource)BranchGrid.DataSource).EndEdit();
            }
        }

        #endregion


        private void ShowOptions_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            PushOptionsPanel.Visible = true;
            ShowOptions.Visible = false;
            SetFormSizeToFitAllItems();
        }

        private void ShowTagOptions_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            TagOptionsPanel.Visible = true;
            ShowTagOptions.Visible = false;
            SetFormSizeToFitAllItems();
        }

        private void SetFormSizeToFitAllItems()
        {
            this.Size = new System.Drawing.Size(this.MinimumSize.Width, this.MinimumSize.Height + 70);
        }
    }
}