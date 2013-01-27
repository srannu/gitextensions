﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using GitCommands;

namespace GitUI.UserControls
{
    // "stashes"
    public partial class RepoObjectsTree
    {
        void ResetStashes(ICollection<GitStash> stashes)
        {//
            nodeStashes.Text = string.Format("stashes ({0})", stashes.Count);
        }

        TreeNode AddStash(TreeNodeCollection nodes, GitStash stash)
        {
            TreeNode treeNode = nodes.Add(stash.Index.ToString(), stash.Name);
            treeNode.Tag = stash;
            treeNode.ToolTipText = stash.Message;
            ApplyStashStyle(treeNode);
            return treeNode;
        }

        void ApplyStashStyle(TreeNode treeNode)
        {
            // style
        }
    }
}
