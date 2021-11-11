/* XIVQuickLauncher - Modified Code
 * Copyright (C) 2021  goatcorp
 */
using EDQuickLauncher.Windows.ViewModel;
using System.Windows;

namespace EDQuickLauncher.Windows {
  /// <summary>
  /// Interaction logic for OtpInputDialog.xaml
  /// </summary>
  public partial class UpdateLoadingDialog : Window {
    public UpdateLoadingDialog() {
      InitializeComponent();

      AutoLoginDisclaimer.Visibility = App.Settings.AutolaunchEnabled ? Visibility.Visible : Visibility.Collapsed;
      if (ResetUidCacheDisclaimer.Visibility == Visibility.Visible && AutoLoginDisclaimer.Visibility == Visibility.Visible) {
        UpdateLoadingCard.Height += 19;
      }

      DataContext = new UpdateLoadingDialogViewModel();
    }
  }
}