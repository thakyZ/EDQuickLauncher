/* XIVQuickLauncher - Modified Code
 * Copyright (C) 2021  goatcorp
 */
using EDQuickLauncher.Addon;
using EDQuickLauncher.Windows.ViewModel;
using System.Windows;

namespace EDQuickLauncher.Windows {
  /// <summary>
  /// Interaction logic for FirstTimeSetup.xaml
  /// </summary>
  public partial class GenericAddonSetupWindow : Window {
    public GenericAddon Result { get; private set; }

    public GenericAddonSetupWindow(GenericAddon addon = null) {
      InitializeComponent();

      DataContext = new GenericAddonSetupWindowViewModel();

      if (addon != null) {
        PathEntry.Text = addon.Path;
        CommandLineTextBox.Text = addon.CommandLine;
        AdminCheckBox.IsChecked = addon.RunAsAdmin;
        RunOnCloseCheckBox.IsChecked = addon.RunOnClose;
        KillCheckBox.IsChecked = addon.KillAfterClose;
      }
    }

    private void NextButton_Click(object sender, RoutedEventArgs e) {
      if (System.String.IsNullOrWhiteSpace(PathEntry.Text))
        Close();

      Result = new GenericAddon {
        Path = PathEntry.Text,
        CommandLine = CommandLineTextBox.Text,
        RunAsAdmin = AdminCheckBox.IsChecked == true,
        RunOnClose = RunOnCloseCheckBox.IsChecked == true,
        KillAfterClose = KillCheckBox.IsChecked == true
      };

      Close();
    }

    private void AdminCheckBox_OnChecked(object sender, RoutedEventArgs e) {
      KillCheckBox.IsEnabled = false;
      KillCheckBox.IsChecked = false;
    }

    private void AdminCheckBox_OnUnchecked(object sender, RoutedEventArgs e) => KillCheckBox.IsEnabled = true;
  }
}