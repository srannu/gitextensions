﻿using System;
using System.Collections.Generic;
using GitCommands.Config;

namespace GitCommands
{
    public class GitRef : IGitItem
    {
        public const string GitHeadsPrefix = "refs/heads/";

        private readonly string _mergeSettingName;
        private readonly string _remoteSettingName;
        private IList<IGitItem> _subItems;
        public GitModule Module { get; private set; }

        public GitRef(GitModule module, string guid, string completeName) : this(module, guid, completeName, string.Empty) { }

        public GitRef(GitModule module, string guid, string completeName, string remote)
        {
            Module = module;
            Guid = guid;
            Selected = false;
            CompleteName = completeName;
            Remote = remote;
            IsTag = CompleteName.Contains("refs/tags/");
            IsHead = CompleteName.Contains(GitHeadsPrefix);
            IsRemote = CompleteName.Contains("refs/remotes/");
            IsBisect = CompleteName.Contains("refs/bisect/");

            ParseName();

            _remoteSettingName = RemoteSettingName(Name);
            _mergeSettingName = String.Format("branch.{0}.merge", Name);
        }

        public static GitRef CreateBranchRef(GitModule module, string guid, string name)
        {
            return new GitRef(module, guid, GitHeadsPrefix + name);
        }

        public string CompleteName { get; private set; }
        public bool Selected { get; set; }
        public bool SelectedHeadMergeSource { get; set; }
        public bool IsTag { get; private set; }
        public bool IsHead { get; private set; }
        public bool IsRemote { get; private set; }
        public bool IsBisect { get; private set; }

        public bool IsOther
        {
            get { return !IsHead && !IsRemote && !IsTag; }
        }

        public string LocalName
        {
            get { return IsRemote ? Name.Substring(Remote.Length + 1) : Name; }
        }

        public string Remote { get; private set; }

        public string TrackingRemote
        {
            get
            {
                return GetTrackingRemote(Module.GetLocalConfig());
            }
            set
            {
                if (String.IsNullOrEmpty(value))
                    Module.UnsetSetting(_remoteSettingName);
                else
                {
                    Module.SetSetting(_remoteSettingName, value);

                    if (MergeWith == "")
                        MergeWith = Name;
                }
            }
        }

        /// <summary>Gets the setting name for a branch's remote.</summary>
        public static string RemoteSettingName(string branch)
        {
            return String.Format("branch.{0}.remote", branch);
        }

        /// <summary>
        /// This method is a faster than the property above. The property reads the config file
        /// every time it is accessed. This method accepts a configfile what makes it faster when loading
        /// the revisiongraph.
        /// </summary>
        public string GetTrackingRemote(ConfigFile configFile)
        {
            return configFile.GetValue(_remoteSettingName);
        }

        public string MergeWith
        {
            get
            {
                return GetMergeWith(Module.GetLocalConfig());
            }
            set
            {
                if (String.IsNullOrEmpty(value))
                    Module.UnsetSetting(_mergeSettingName);
                else
                    Module.SetSetting(_mergeSettingName, GitHeadsPrefix + value);
            }
        }

        /// <summary>
        /// This method is a faster than the property above. The property reads the config file
        /// every time it is accessed. This method accepts a configfile what makes it faster when loading
        /// the revisiongraph.
        /// </summary>
        public string GetMergeWith(ConfigFile configFile)
        {
            string merge = configFile.GetValue(_mergeSettingName);
            return merge.StartsWith(GitHeadsPrefix) ? merge.Substring(11) : merge;

        }


        public static GitRef NoHead(GitModule module)
        {
            return new GitRef(module, null, "");
        }

        public static GitRef AllHeads(GitModule module)
        {
            return new GitRef(module, null, "*");
        }

        #region IGitItem Members

        public string Guid { get; private set; }
        public string Name { get; private set; }

        public IEnumerable<IGitItem> SubItems
        {
            get { return _subItems ?? (_subItems = Module.GetTree(Guid, false)); }
        }

        #endregion

        public override string ToString()
        {
            return CompleteName;
        }

        private void ParseName()
        {
            if (IsRemote)
            {
                Name = CompleteName.Substring(CompleteName.LastIndexOf("remotes/") + 8);
            } 
            else if (IsTag)
            {
                // we need the one containing ^{}, because it contains the reference
                var temp =
                    CompleteName.Contains("^{}")
                        ? CompleteName.Substring(0, CompleteName.Length - 3)
                        : CompleteName;

                Name = temp.Substring(CompleteName.LastIndexOf("tags/") + 5);
            }
            else if (IsHead)
            {
                Name = CompleteName.Substring(CompleteName.LastIndexOf("heads/") + 6);
            }
            else
                //if we don't know ref type then we don't know if '/' is a valid ref character
                Name = CompleteName.SkipStr("refs/");
        }
    }
}