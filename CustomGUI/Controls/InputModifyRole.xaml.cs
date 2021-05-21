using System;
using System.Collections.Generic;
using System.Windows;
using AdskConstructionCloudBreakdown;

namespace CustomGUI.Controls
{
    public partial class InputModifyRole : Window
    {
        public string RoleRet { set; get; }
        public AccessPermissionEnum AccessRet { set; get; }
        public InputModifyRole(string RoleName, AccessPermissionEnum access)
        {
            InitializeComponent();
            //set values to values that are currently set
            ComboBoxAccess.ItemsSource = Enum.GetValues(typeof(AdskConstructionCloudBreakdown.AccessPermissionEnum));
            RoleNameAnswer.Text = RoleName;
            ComboBoxAccess.SelectedItem = access;
            //in case that user canceled the input
            RoleRet = RoleNameAnswer.Text;
            AccessRet = (AccessPermissionEnum)ComboBoxAccess.SelectedItem;

        }

        private void btnDialogOk_Click(object sender, RoutedEventArgs e)
        {
            //set values for config
            RoleRet = RoleNameAnswer.Text;
            AccessRet = (AccessPermissionEnum)ComboBoxAccess.SelectedItem;

            this.DialogResult = true;
        }

        private void Window_ContentRendered(object sender, EventArgs e)
        {
            RoleNameAnswer.SelectAll();
            RoleNameAnswer.Focus();

        }


    }
}