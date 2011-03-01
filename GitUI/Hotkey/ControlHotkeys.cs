﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace GitUI.Hotkey
{
  /// <summary>
  /// ControlHotkeys enables editing of HotkeySettings
  /// </summary>
  public partial class ControlHotkeys : UserControl
  {
    #region Properties

    #region Settings
    private HotkeySettings[] _Settings;
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public HotkeySettings[] Settings 
    {
      get { return _Settings; }
      set 
      { 
        _Settings = value;
        UpdateCombobox(value);
      } 
    }
    #endregion

    #region SelectedHotkeySettings
    private HotkeySettings _SelectedHotkeySettings;
    public HotkeySettings SelectedHotkeySettings 
    { 
      get { return _SelectedHotkeySettings; } 
      set
      {
        _SelectedHotkeySettings = value;
        UpdateListViewItems(value);
      } 
    }

    #endregion

    #region SelectedHotkeyCommand
    private HotkeyCommand _SelectedHotkeyCommand;
    public HotkeyCommand SelectedHotkeyCommand 
    {
      get { return _SelectedHotkeyCommand; }
      set 
      { 
        _SelectedHotkeyCommand = value;

      }
    }
    #endregion

    private HotkeySettingsManager HotkeySettingsManager = new HotkeySettingsManager();

    #endregion

    public ControlHotkeys()
    {
      InitializeComponent();
    }

    #region Methods

    private void UpdateCombobox(HotkeySettings[] settings)
    {
      this.cmbSettings.Items.Clear();
      if (settings != null)
        foreach (var setting in settings)
          cmbSettings.Items.Add(setting);
    }

    private void UpdateListViewItems(HotkeySettings setting)
    {
      this.listMappings.Clear();
      if (setting != null)
        foreach (var cmd in setting.Commands)
          this.listMappings.Items.Add(new ListViewItem(cmd.Name, cmd.KeyData.ToText()) { Tag = setting });
    }

    private void ControlHotkeys_Load(object sender, EventArgs e)
    {
      this.Settings = this.HotkeySettingsManager.LoadSettings();
    }

    private void bClear_Click(object sender, EventArgs e)
    {
      this.txtHotkey.KeyData = Keys.None;
    }

    private void cmbSettings_SelectedIndexChanged(object sender, EventArgs e)
    {
      this.SelectedHotkeySettings = this.cmbSettings.SelectedItem as HotkeySettings;
    }

    private void listMappings_SelectedIndexChanged(object sender, EventArgs e)
    {
      var lvi = this.listMappings.SelectedItems.Count > 0 ? this.listMappings.SelectedItems[0] : null;
      if (lvi != null)
      {
        var hotkey = lvi.Tag as HotkeyCommand;
      }
    }

    private void bApply_Click(object sender, EventArgs e)
    {

    }

    #endregion

   
  }
}
