﻿using System;
using System.IO;
using System.Text;
using System.Windows.Forms;
using GitCommands;

namespace GitUI
{
    public partial class FormAddToGitIgnore : GitExtensionsForm
    {
        public FormAddToGitIgnore(string filePattern)
        {
            InitializeComponent();
            Translate();
            FilePattern.Text = filePattern;
            UpdatePreviewPanel();
        }

        private void AddToIngoreClick(object sender, EventArgs e)
        {
            try
            {
                FileInfoExtensions
                    .MakeFileTemporaryWritable(Settings.WorkingDir + ".gitignore",
                                       x =>
                                       {
                                           var gitIgnoreFile = new StringBuilder();
                                           gitIgnoreFile.Append(Environment.NewLine);
                                           gitIgnoreFile.Append(FilePattern.Text);

                                           using (TextWriter tw = new StreamWriter(x, true, Settings.Encoding))
                                           {
                                               tw.Write(gitIgnoreFile);
                                           }
                                       });
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }

            Close();
        }

        private void UpdatePreviewPanel()
        {
            Preview.DataSource = GitCommandHelpers.GetFiles(FilePattern.Text);
            NumMatches.Text = Preview.Items.Count.ToString();
            NumMatches.Left = filesWillBeIgnored.Left - NumMatches.Width + 3;
            noMatchPanel.Visible = (Preview.Items.Count == 0);
        }

        private void FilePattern_TextChanged(object sender, EventArgs e)
        {
            UpdatePreviewPanel();
        }
    }
}