﻿using System;
using System.Drawing;
using System.Windows.Forms;
using GitCommands;
using Microsoft.Win32;
using System.IO;
using ResourceManager.Translation;
using System.Reflection;
using System.Diagnostics;

namespace GitUI.CommandsDialogs.SettingsDialog.Pages
{
    /// <summary>
    /// </summary>
    public partial class ChecklistSettingsPage : SettingsPageBase
    {
        private readonly TranslationString _wrongGitVersion =
            new TranslationString("Git found but version {0} is not supported. Upgrage to version {1} or later");

        private readonly TranslationString _gitVersionFound =
            new TranslationString("Git {0} is found on your computer.");

        private readonly TranslationString _unknownSshClient =
            new TranslationString("Unknown SSH client configured: {0}.");

        private readonly TranslationString _linuxToolsSshNotFound =
            new TranslationString("Linux tools (sh) not found. To solve this problem you can set the correct path in settings.");

        private readonly TranslationString _solveGitCommandFailedCaption =
            new TranslationString("Locate git");

        private readonly TranslationString _gitCanBeRun =
            new TranslationString("Git can be run using: {0}");

        private readonly TranslationString _gitCanBeRunCaption =
            new TranslationString("Locate git");

        private readonly TranslationString _solveGitCommandFailed =
            new TranslationString("The command to run git could not be determined automatically." + Environment.NewLine +
                "Please make sure git (msysgit or cygwin) is installed or set the correct command manually.");

        private readonly TranslationString _shellExtRegistered =
            new TranslationString("Shell extensions registered properly.");

        private readonly TranslationString _shellExtNoInstalled =
            new TranslationString("Shell extensions are not installed. Run the installer to install the shell extensions.");

        private readonly TranslationString _shellExtNeedsToBeRegistered =
            new TranslationString("{0} needs to be registered in order to use the shell extensions.");

        private readonly TranslationString _registryKeyGitExtensionsMissing =
            new TranslationString("Registry entry missing [Software\\GitExtensions\\GitExtensions\\InstallDir].");

        private readonly TranslationString _registryKeyGitExtensionsFaulty =
            new TranslationString("Invalid installation directory stored in [Software\\GitExtensions\\GitExtensions\\InstallDir].");

        private readonly TranslationString _registryKeyGitExtensionsCorrect =
            new TranslationString("GitExtensions is properly registered.");

        private readonly TranslationString _plinkputtyGenpageantNotFound =
            new TranslationString("PuTTY is configured as SSH client but cannot find plink.exe, puttygen.exe or pageant.exe.");

        private readonly TranslationString _puttyConfigured =
            new TranslationString("SSH client PuTTY is configured properly.");

        private readonly TranslationString _opensshUsed =
            new TranslationString("Default SSH client, OpenSSH, will be used. (commandline window will appear on pull, push and clone operations)");

        private readonly TranslationString _noMergeToolConfigured =
            new TranslationString("There is no mergetool configured. Do you want to configure kdiff3 as your mergetool?" +
                Environment.NewLine + "Select no if you want to configure a different mergetool yourself.");

        private readonly TranslationString _noMergeToolConfiguredCaption =
            new TranslationString("Mergetool");

        private readonly TranslationString _languageConfigured =
            new TranslationString("The configured language is {0}.");

        private readonly TranslationString _noLanguageConfigured =
            new TranslationString("There is no language configured for Git Extensions.");

        private readonly TranslationString _noEmailSet =
            new TranslationString("You need to configure a username and an email address.");

        private readonly TranslationString _emailSet =
            new TranslationString("A username and an email address are configured.");

        private readonly TranslationString _credentialHelperInstalled =
            new TranslationString("Git credential helper is installed.");

        private readonly TranslationString _noCredentialsHelperInstalled =
            new TranslationString("No credential helper installed.");

        private readonly TranslationString _gitCredentialWinStoreHelperInstalled =
            new TranslationString("Git Credential Win Store is installed as credential helper.");

        private readonly TranslationString _noCredentialsHelperInstalledTryGCS =
            new TranslationString("No credential helper could be installed. Try to install git-credential-winstore.exe.");

        private readonly TranslationString _mergeToolXConfiguredNeedsCmd =
            new TranslationString("{0} is configured as mergetool, this is a custom mergetool and needs a custom cmd to be configured.");

        private readonly TranslationString _customMergeToolXConfigured =
            new TranslationString("There is a custom mergetool configured: {0}");

        private readonly TranslationString _mergeToolXConfigured =
            new TranslationString("There is a custom mergetool configured.");

        private readonly TranslationString _linuxToolsSshFound =
            new TranslationString("Linux tools (sh) found on your computer.");

        private readonly TranslationString _gitNotFound =
            new TranslationString("Git not found. To solve this problem you can set the correct path in settings.");

        private readonly TranslationString _adviceDiffToolConfiguration =
            new TranslationString("You should configure a diff tool to show file diff in external program (kdiff3 for example).");

        private readonly TranslationString _kdiffAsDiffConfiguredButNotFound =
            new TranslationString("KDiff3 is configured as difftool, but the path to kdiff.exe is not configured.");

        private readonly TranslationString _kdiffAsDiffConfigured =
            new TranslationString("KDiff3 is configured as difftool.");

        private readonly TranslationString _diffToolXConfigured =
            new TranslationString("There is a difftool configured: {0}");

        private readonly TranslationString _configureMergeTool =
            new TranslationString("You need to configure merge tool in order to solve mergeconflicts (kdiff3 for example).");

        private readonly TranslationString _kdiffAsMergeConfiguredButNotFound =
            new TranslationString("KDiff3 is configured as mergetool, but the path to kdiff.exe is not configured.");

        private readonly TranslationString _kdiffAsMergeConfigured =
            new TranslationString("KDiff3 is configured as mergetool.");

        private readonly TranslationString _kdiff3NotFoundAuto =
            new TranslationString("Path to kdiff3 could not be found automatically." + Environment.NewLine +
                "Please make sure KDiff3 is installed or set path manually.");

        private readonly TranslationString _cantRegisterShellExtension =
            new TranslationString("Could not register the shell extension because '{0}' could not be found.");

        private readonly TranslationString _noDiffToolConfigured =
            new TranslationString("There is no difftool configured. Do you want to configure kdiff3 as your difftool?" +
                Environment.NewLine + "Select no if you want to configure a different difftool yourself.");

        private readonly TranslationString _noDiffToolConfiguredCaption =
            new TranslationString("Difftool");

        private readonly TranslationString _puttyFoundAuto =
            new TranslationString("All paths needed for PuTTY could be automatically found and are set.");

        private readonly TranslationString _linuxToolsShNotFound =
            new TranslationString("The path to linux tools (sh) could not be found automatically." + Environment.NewLine +
                "Please make sure there are linux tools installed (through msysgit or cygwin) or set the correct path manually.");

        private readonly TranslationString _linuxToolsShNotFoundCaption =
            new TranslationString("Locate linux tools");

        private readonly TranslationString _shCanBeRun =
            new TranslationString("Command sh can be run using: {0}sh");

        private readonly TranslationString _shCanBeRunCaption =
            new TranslationString("Locate linux tools");

        private const string _putty = "PuTTY";

        readonly CommonLogic _commonLogic;
        readonly CheckSettingsLogic _checkSettingsLogic;
        readonly GitModule _gitModule;
        readonly ISettingsPageHost _settingsPageHost;

        /// <summary>
        /// TODO: remove this direct dependency to another SettingsPage later when possible
        /// </summary>
        public SshSettingsPage SshSettingsPage { get; set; }

        private ChecklistSettingsPage()
        {
            InitializeComponent();
            Text = "Checklist";
            Translate();        
        }

        public ChecklistSettingsPage(CommonLogic commonLogic,
            CheckSettingsLogic checkSettingsLogic,
            GitModule gitModule,
            ISettingsPageHost settingsPageHost)
            : this()
        {
            _commonLogic = commonLogic;
            _checkSettingsLogic = checkSettingsLogic;
            _gitModule = gitModule;
            _settingsPageHost = settingsPageHost;
        }

        public static SettingsPageReference GetPageReference()
        {
            return new SettingsPageReferenceByType(typeof(ChecklistSettingsPage));
        }

        public override bool IsInstantSavePage
        {
            get
            {
                return true;
            }
        }

        public override void OnPageShown()
        {
            ////LoadSettings();
            CheckSettings();
        }

        private void gitCredentialWinStore_Fix_Click(object sender, EventArgs e)
        {
            if (_checkSettingsLogic.SolveGitCredentialStore())
            {
                MessageBox.Show(this, _gitCredentialWinStoreHelperInstalled.Text);
            }
            else
            {
                MessageBox.Show(this, _noCredentialsHelperInstalledTryGCS.Text);
            }

            CheckSettings();
        }

        private void translationConfig_Click(object sender, EventArgs e)
        {
            using (var frm = new FormChooseTranslation())
            {
                frm.ShowDialog(this); // will set Settings.Translation
            }

            _settingsPageHost.LoadAll();
            Translate();
            SaveAndRescan_Click(null, null);
        }

        private void SshConfig_Click(object sender, EventArgs e)
        {
            if (GitCommandHelpers.Plink())
            {
                if (SshSettingsPage.AutoFindPuttyPaths())
                {
                    MessageBox.Show(this, _puttyFoundAuto.Text, _putty);
                }
                else
                {
                    _settingsPageHost.GotoPage(SshSettingsPage.GetPageReference());   
                }
            }

            // original
////            if (Putty.Checked)
////            {
////                if (AutoFindPuttyPaths())
////                    MessageBox.Show(this, _puttyFoundAuto.Text, _puttyFoundAutoCaption.Text);
////                else
////                    tabControl1.SelectTab(tpSsh);
////            }
        }

        private void GitExtensionsInstall_Click(object sender, EventArgs e)
        {
            _checkSettingsLogic.SolveGitExtensionsDir();
            CheckSettings();
        }

        private void GitBinFound_Click(object sender, EventArgs e)
        {
            if (!_checkSettingsLogic.SolveLinuxToolsDir())
            {
                MessageBox.Show(this, _linuxToolsShNotFound.Text, _linuxToolsShNotFoundCaption.Text);
                _settingsPageHost.GotoPage(GitSettingsPage.GetPageReference());
                return;
            }

            MessageBox.Show(this, String.Format(_shCanBeRun.Text, Settings.GitBinDir), _shCanBeRunCaption.Text);
            ////GitBinPath.Text = Settings.GitBinDir;
            _settingsPageHost.LoadAll(); // apply settings to dialog controls (otherwise the later called SaveAndRescan_Click would overwrite settings again)
            SaveAndRescan_Click(null, null);
        }

        private void ShellExtensionsRegistered_Click(object sender, EventArgs e)
        {
            string path = Path.Combine(Settings.GetInstallDir(), CommonLogic.GitExtensionsShellEx32Name);
            if (!File.Exists(path))
            {
                path = Assembly.GetAssembly(GetType()).Location;
                path = Path.GetDirectoryName(path);
                path = Path.Combine(path, CommonLogic.GitExtensionsShellEx32Name);
            }
            if (File.Exists(path))
            {
                var pi = new ProcessStartInfo();
                pi.FileName = "regsvr32";
                pi.Arguments = string.Format("\"{0}\"", path);
                pi.Verb = "RunAs";
                pi.UseShellExecute = true;

                var process = Process.Start(pi);
                process.WaitForExit();

                if (IntPtr.Size == 8)
                {
                    path = path.Replace(CommonLogic.GitExtensionsShellEx32Name, CommonLogic.GitExtensionsShellEx64Name);
                    if (File.Exists(path))
                    {
                        pi.Arguments = string.Format("\"{0}\"", path);

                        var process64 = Process.Start(pi);
                        process64.WaitForExit();
                    }
                    else
                    {
                        MessageBox.Show(this, string.Format(_cantRegisterShellExtension.Text, CommonLogic.GitExtensionsShellEx64Name));
                    }
                }
            }
            else
            {
                MessageBox.Show(this, string.Format(_cantRegisterShellExtension.Text, CommonLogic.GitExtensionsShellEx32Name));
            }

            CheckSettings();
        }

        private void DiffToolFix_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(CheckSettingsLogic.GetGlobalDiffToolFromConfig()))
            {
                if (MessageBox.Show(this, _noDiffToolConfigured.Text, _noDiffToolConfiguredCaption.Text,
                        MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    _checkSettingsLogic.SolveDiffToolForKDiff();
                    _settingsPageHost.LoadAll(); // apply settings to dialog controls (otherwise the later called SaveAndRescan_Click would overwrite settings again)
                }
                else
                {
                    GotoPageGlobalSettings();
                    return;
                }
            }

            if (CheckSettingsLogic.GetGlobalDiffToolFromConfig().Equals("kdiff3", StringComparison.CurrentCultureIgnoreCase))
            {
                _checkSettingsLogic.SolveDiffToolPathForKDiff();
            }

            if (CheckSettingsLogic.GetGlobalDiffToolFromConfig().Equals("kdiff3", StringComparison.CurrentCultureIgnoreCase) &&
                string.IsNullOrEmpty(_gitModule.GetGlobalSetting("difftool.kdiff3.path")))
            {
                MessageBox.Show(this, _kdiff3NotFoundAuto.Text);
                GotoPageGlobalSettings();
                return;
            }

            SaveAndRescan_Click(null, null);
        }

        private void MergeToolFix_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(_commonLogic.GetGlobalMergeTool()))
            {
                if (
                    MessageBox.Show(this, _noMergeToolConfigured.Text,
                        _noMergeToolConfiguredCaption.Text, MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    _commonLogic.SetGlobalMergeTool("kdiff3");
                    _settingsPageHost.LoadAll(); // apply settings to dialog controls (otherwise the later called SaveAndRescan_Click would overwrite settings again)
                }
                else
                {
                    GotoPageGlobalSettings();
                    return;
                }
            }

            if (_commonLogic.IsMergeTool("kdiff3"))
            {
                _checkSettingsLogic.SolveMergeToolPathForKDiff();
            }
            else if (_commonLogic.IsMergeTool("p4merge") || _commonLogic.IsMergeTool("TortoiseMerge"))
            {
                _checkSettingsLogic.AutoConfigMergeToolCmd(true);

                _gitModule.SetGlobalPathSetting(
                    string.Format("mergetool.{0}.cmd", _commonLogic.GetGlobalMergeTool()), _checkSettingsLogic.GetMergeToolCmdText());
            }

            if (_commonLogic.IsMergeTool("kdiff3") &&
                string.IsNullOrEmpty(_gitModule.GetGlobalSetting("mergetool.kdiff3.path")))
            {
                MessageBox.Show(this, _kdiff3NotFoundAuto.Text);
                GotoPageGlobalSettings();
                return;
            }

            SaveAndRescan_Click(null, null);
        }

        // TODO: needed somewhere?
        ////private void SetGlobalMergeToolText(string text)
        ////{
        ////    throw new NotImplementedException("GlobalMergeTool.Text = ...");
        ////}

        private void GotoPageGlobalSettings()
        {
            _settingsPageHost.GotoPage(GlobalSettingsSettingsPage.GetPageReference());
        }

        private void UserNameSet_Click(object sender, EventArgs e)
        {
            GotoPageGlobalSettings();
            // nice-to-have: jump directly to correct text box
        }

        private void GitFound_Click(object sender, EventArgs e)
        {
            if (!_checkSettingsLogic.SolveGitCommand())
            {
                MessageBox.Show(this, _solveGitCommandFailed.Text, _solveGitCommandFailedCaption.Text);

                _settingsPageHost.GotoPage(GitSettingsPage.GetPageReference());
                return;
            }

            MessageBox.Show(this, String.Format(_gitCanBeRun.Text, Settings.GitCommand), _gitCanBeRunCaption.Text);

            _settingsPageHost.GotoPage(GitSettingsPage.GetPageReference());
            SaveAndRescan_Click(null, null);
        }

        private void SaveAndRescan_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            _settingsPageHost.SaveAll();
            _settingsPageHost.LoadAll();
            CheckSettings();
            Cursor.Current = Cursors.Default;
        }

        private void CheckAtStartup_CheckedChanged(object sender, EventArgs e)
        {
            Settings.SetBool("checksettings", CheckAtStartup.Checked);
        }

        public bool CheckSettings()
        {
            bool bValid = true;
            try
            {
                // once a check fails, we want bValid to stay false
                bValid = CheckGitCmdValid();
                bValid = CheckGlobalUserSettingsValid() && bValid;
                bValid = CheckMergeTool() && bValid;
                bValid = CheckDiffToolConfiguration() && bValid;
                bValid = CheckTranslationConfigSettings() && bValid;

                if (Settings.RunningOnWindows())
                {
                    bValid = CheckGitExtensionsInstall() && bValid;
                    bValid = CheckGitExtensionRegistrySettings() && bValid;
                    bValid = CheckGitExe() && bValid;
                    bValid = CheckSSHSettings() && bValid;
                    bValid = CheckGitCredentialStore() && bValid;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, ex.Message);
            }

            CheckAtStartup.Checked = getCheckAtStartupChecked(bValid);

            return bValid;
        }

        private static bool getCheckAtStartupChecked(bool bValid)
        {
            var retValue = Settings.GetValue<string>("checksettings", null) == null || Settings.GetValue<string>("checksettings", null) == "true";

            if (bValid && retValue)
            {
                Settings.SetValue("checksettings", false);
                retValue = false;
            }
            return retValue;
        }

        private bool CheckTranslationConfigSettings()
        {
            translationConfig.Visible = true;
            if (string.IsNullOrEmpty(Settings.Translation))
            {
                translationConfig.BackColor = Color.LightSalmon;
                translationConfig.Text = _noLanguageConfigured.Text;
                translationConfig_Fix.Visible = true;
                return false;
            }
            translationConfig.BackColor = Color.LightGreen;
            translationConfig_Fix.Visible = false;
            translationConfig.Text = String.Format(_languageConfigured.Text, Settings.Translation);
            return true;
        }

        private bool CheckSSHSettings()
        {
            SshConfig.Visible = true;
            if (GitCommandHelpers.Plink())
            {
                if (!File.Exists(Settings.Plink) || !File.Exists(Settings.Puttygen) || !File.Exists(Settings.Pageant))
                {
                    SshConfig.BackColor = Color.LightSalmon;
                    SshConfig.Text = _plinkputtyGenpageantNotFound.Text;
                    SshConfig_Fix.Visible = true;
                    return false;
                }
                SshConfig.BackColor = Color.LightGreen;
                SshConfig_Fix.Visible = false;
                SshConfig.Text = _puttyConfigured.Text;
                return true;
            }
            SshConfig.BackColor = Color.LightGreen;
            SshConfig_Fix.Visible = false;
            if (string.IsNullOrEmpty(GitCommandHelpers.GetSsh()))
                SshConfig.Text = _opensshUsed.Text;
            else
                SshConfig.Text = String.Format(_unknownSshClient.Text, GitCommandHelpers.GetSsh());
            return true;
        }

        private bool CheckGitExe()
        {
            GitBinFound.Visible = true;
            if (!File.Exists(Settings.GitBinDir + "sh.exe") && !File.Exists(Settings.GitBinDir + "sh") &&
                !CheckSettingsLogic.CheckIfFileIsInPath("sh.exe") && !CheckSettingsLogic.CheckIfFileIsInPath("sh"))
            {
                GitBinFound.BackColor = Color.LightSalmon;
                GitBinFound.Text = _linuxToolsSshNotFound.Text;
                GitBinFound_Fix.Visible = true;
                return false;
            }
            GitBinFound_Fix.Visible = false;
            GitBinFound.BackColor = Color.LightGreen;
            GitBinFound.Text = _linuxToolsSshFound.Text;
            return true;
        }

        private bool CheckGitCmdValid()
        {
            GitFound.Visible = true;
            if (!_checkSettingsLogic.CanFindGitCmd())
            {
                GitFound.BackColor = Color.LightSalmon;
                GitFound.Text = _gitNotFound.Text;
                GitFound_Fix.Visible = true;
                return false;
            }

            if (GitCommandHelpers.VersionInUse < GitVersion.LastSupportedVersion)
            {
                GitFound.BackColor = Color.LightSalmon;
                GitFound.Text = String.Format(_wrongGitVersion.Text, GitCommandHelpers.VersionInUse, GitVersion.LastSupportedVersion);
                GitFound_Fix.Visible = true;
                return false;
            }

            GitFound_Fix.Visible = false;
            GitFound.BackColor = Color.LightGreen;
            GitFound.Text = String.Format(_gitVersionFound.Text, GitCommandHelpers.VersionInUse);
            return true;
        }

        public bool CheckGitCredentialStore()
        {
            gitCredentialWinStore.Visible = true;

            bool isValid = _checkSettingsLogic.CheckGitCredentialStore();

            if (isValid)
            {
                gitCredentialWinStore.BackColor = Color.LightGreen;
                gitCredentialWinStore.Text = _credentialHelperInstalled.Text;
                gitCredentialWinStore_Fix.Visible = false;
            }
            else
            {
                gitCredentialWinStore.BackColor = Color.LightSalmon;
                gitCredentialWinStore.Text = _noCredentialsHelperInstalled.Text;
                gitCredentialWinStore_Fix.Visible = true;
            }

            return isValid;
        }

        private bool CheckDiffToolConfiguration()
        {
            DiffTool.Visible = true;
            if (string.IsNullOrEmpty(CheckSettingsLogic.GetGlobalDiffToolFromConfig()))
            {
                DiffTool.BackColor = Color.LightSalmon;
                DiffTool_Fix.Visible = true;
                DiffTool.Text = _adviceDiffToolConfiguration.Text;
                return false;
            }
            if (Settings.RunningOnWindows())
            {
                if (CheckSettingsLogic.GetGlobalDiffToolFromConfig().Equals("kdiff3", StringComparison.CurrentCultureIgnoreCase))
                {
                    string p = _gitModule.GetGlobalSetting("difftool.kdiff3.path");
                    if (string.IsNullOrEmpty(p) || !File.Exists(p))
                    {
                        DiffTool.BackColor = Color.LightSalmon;
                        DiffTool.Text = _kdiffAsDiffConfiguredButNotFound.Text;
                        DiffTool_Fix.Visible = true;
                        return false;
                    }
                    DiffTool.BackColor = Color.LightGreen;
                    DiffTool.Text = _kdiffAsDiffConfigured.Text;
                    DiffTool_Fix.Visible = false;
                    return true;
                }
            }
            string difftool = CheckSettingsLogic.GetGlobalDiffToolFromConfig();
            DiffTool.BackColor = Color.LightGreen;
            DiffTool.Text = String.Format(_diffToolXConfigured.Text, difftool);
            DiffTool_Fix.Visible = false;
            return true;
        }

        private bool CheckMergeTool()
        {
            MergeTool.Visible = true;
            if (string.IsNullOrEmpty(_commonLogic.GetGlobalMergeTool()))
            {
                MergeTool.BackColor = Color.LightSalmon;
                MergeTool.Text = _configureMergeTool.Text;
                MergeTool_Fix.Visible = true;
                return false;
            }

            if (Settings.RunningOnWindows())
            {
                if (_commonLogic.IsMergeTool("kdiff3"))
                {
                    string p = _gitModule.GetGlobalSetting("mergetool.kdiff3.path");
                    if (string.IsNullOrEmpty(p) || !File.Exists(p))
                    {
                        MergeTool.BackColor = Color.LightSalmon;
                        MergeTool.Text = _kdiffAsMergeConfiguredButNotFound.Text;
                        MergeTool_Fix.Visible = true;
                        return false;
                    }
                    MergeTool.BackColor = Color.LightGreen;
                    MergeTool.Text = _kdiffAsMergeConfigured.Text;
                    MergeTool_Fix.Visible = false;
                    return true;
                }
                string mergetool = _commonLogic.GetGlobalMergeTool().ToLowerInvariant();
                if (mergetool == "p4merge" || mergetool == "tmerge")
                {
                    string p = _gitModule.GetGlobalSetting(string.Format("mergetool.{0}.cmd", mergetool));
                    if (string.IsNullOrEmpty(p))
                    {
                        MergeTool.BackColor = Color.LightSalmon;
                        MergeTool.Text = String.Format(_mergeToolXConfiguredNeedsCmd.Text, mergetool);
                        MergeTool_Fix.Visible = true;
                        return false;
                    }
                    MergeTool.BackColor = Color.LightGreen;
                    MergeTool.Text = String.Format(_customMergeToolXConfigured.Text, mergetool);
                    MergeTool_Fix.Visible = false;
                    return true;
                }
            }
            MergeTool.BackColor = Color.LightGreen;
            MergeTool.Text = _mergeToolXConfigured.Text;
            MergeTool_Fix.Visible = false;
            return true;
        }

        private bool CheckGlobalUserSettingsValid()
        {
            UserNameSet.Visible = true;
            if (string.IsNullOrEmpty(_gitModule.GetGlobalSetting("user.name")) ||
                string.IsNullOrEmpty(_gitModule.GetGlobalSetting("user.email")))
            {
                UserNameSet.BackColor = Color.LightSalmon;
                UserNameSet.Text = _noEmailSet.Text;
                UserNameSet_Fix.Visible = true;
                return false;
            }
            UserNameSet.BackColor = Color.LightGreen;
            UserNameSet.Text = _emailSet.Text;
            UserNameSet_Fix.Visible = false;
            return true;
        }

        private bool CheckGitExtensionRegistrySettings()
        {
            if (!Settings.RunningOnWindows())
                return true;

            ShellExtensionsRegistered.Visible = true;

            if (string.IsNullOrEmpty(_commonLogic.GetRegistryValue(Registry.LocalMachine,
                                                      "Software\\Microsoft\\Windows\\CurrentVersion\\Shell Extensions\\Approved",
                                                      "{3C16B20A-BA16-4156-916F-0A375ECFFE24}")) ||
                string.IsNullOrEmpty(_commonLogic.GetRegistryValue(Registry.ClassesRoot,
                                                      "*\\shellex\\ContextMenuHandlers\\GitExtensions2", null)) ||
                string.IsNullOrEmpty(_commonLogic.GetRegistryValue(Registry.ClassesRoot,
                                                      "Directory\\shellex\\ContextMenuHandlers\\GitExtensions2", null)) ||
                string.IsNullOrEmpty(_commonLogic.GetRegistryValue(Registry.ClassesRoot,
                                                      "Directory\\Background\\shellex\\ContextMenuHandlers\\GitExtensions2",
                                                      null)))
            {
                //Check if shell extensions are installed
                string path32 = Path.Combine(Settings.GetInstallDir(), CommonLogic.GitExtensionsShellEx32Name);
                string path64 = Path.Combine(Settings.GetInstallDir(), CommonLogic.GitExtensionsShellEx64Name);
                if (!File.Exists(path32) || (IntPtr.Size == 8 && !File.Exists(path64)))
                {
                    ShellExtensionsRegistered.BackColor = Color.LightGreen;
                    ShellExtensionsRegistered.Text = String.Format(_shellExtNoInstalled.Text);
                    ShellExtensionsRegistered_Fix.Visible = false;
                    return true;
                }

                ShellExtensionsRegistered.BackColor = Color.LightSalmon;
                ShellExtensionsRegistered.Text = String.Format(_shellExtNeedsToBeRegistered.Text, CommonLogic.GitExtensionsShellEx32Name);
                ShellExtensionsRegistered_Fix.Visible = true;
                return false;
            }
            ShellExtensionsRegistered.BackColor = Color.LightGreen;
            ShellExtensionsRegistered.Text = _shellExtRegistered.Text;
            ShellExtensionsRegistered_Fix.Visible = false;
            return true;
        }

        private bool CheckGitExtensionsInstall()
        {
            if (!Settings.RunningOnWindows())
                return true;

            GitExtensionsInstall.Visible = true;
            if (string.IsNullOrEmpty(Settings.GetInstallDir()))
            {
                GitExtensionsInstall.BackColor = Color.LightSalmon;
                GitExtensionsInstall.Text = _registryKeyGitExtensionsMissing.Text;
                GitExtensionsInstall_Fix.Visible = true;
                return false;
            }
            if (Settings.GetInstallDir() != null && Settings.GetInstallDir().EndsWith(".exe"))
            {
                GitExtensionsInstall.BackColor = Color.LightSalmon;
                GitExtensionsInstall.Text = _registryKeyGitExtensionsFaulty.Text;
                GitExtensionsInstall_Fix.Visible = true;
                return false;
            }
            GitExtensionsInstall.BackColor = Color.LightGreen;
            GitExtensionsInstall.Text = _registryKeyGitExtensionsCorrect.Text;
            GitExtensionsInstall_Fix.Visible = false;
            return true;
        }
    }
}
