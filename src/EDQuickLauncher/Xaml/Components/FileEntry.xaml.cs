﻿/* XIVQuickLauncher - Modified Code
 * Copyright (C) 2021  goatcorp
 */
using Microsoft.WindowsAPICodePack.Dialogs;
using System.Windows;
using System.Windows.Data;

namespace EDQuickLauncher.Xaml.Components {
  /// <summary>
  /// Interaction logic for FolderEntry.xaml
  /// </summary>
  public partial class FileEntry {
    public static DependencyProperty TextProperty = DependencyProperty.Register("Text", typeof(string),
      typeof(FileEntry),
      new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

    public static DependencyProperty DescriptionProperty = DependencyProperty.Register("Description",
      typeof(string), typeof(FileEntry), new PropertyMetadata(null));

    public static DependencyProperty FiltersProperty = DependencyProperty.Register("Filters",
      typeof(string), typeof(FileEntry), new PropertyMetadata(null));

    public string Text {
      get => GetValue(TextProperty) as string;

      set => SetValue(TextProperty, value);
    }

    public string Description {
      get => GetValue(DescriptionProperty) as string;

      set => SetValue(DescriptionProperty, value);
    }

    public string Filters {
      get => GetValue(FiltersProperty) as string;

      set => SetValue(FiltersProperty, value);
    }

    public FileEntry() => InitializeComponent();

    private void BrowseFolder(object sender, RoutedEventArgs e) {
      using (var dlg = new CommonOpenFileDialog()) {
        var parent = Window.GetWindow(this);

        dlg.Multiselect = false;
        dlg.IsFolderPicker = false;
        dlg.EnsurePathExists = true;
        dlg.Title = Description;

        var filterSets = Filters.Split(';');

        foreach (var filterSet in filterSets) {
          var filterOptions = filterSet.Split(',');
          dlg.Filters.Add(new CommonFileDialogFilter(filterOptions[0], filterOptions[1]));
        }

        CommonFileDialogResult result = dlg.ShowDialog(parent);

        if (result == CommonFileDialogResult.Ok) {
          Text = dlg.FileName;
          BindingExpression be = GetBindingExpression(TextProperty);
          if (be != null)
            be.UpdateSource();
        }
      }
    }
  }
}