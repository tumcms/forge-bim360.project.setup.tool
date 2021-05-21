using System;
using System.Collections.Generic;
using System.Windows;
using AdskConstructionCloudBreakdown;

namespace CustomGUI.Controls
{
    public partial class InputModifyUser : Window
    {
        public string UserRet { set; get; }
        public string CompanyRet { set; get; }
        public AccessPermissionEnum AccessRet { set; get; }
        public InputModifyUser(string username, AccessPermissionEnum access, string company="")
        {
            InitializeComponent();
            //set values to values that are currently set
            ComboBoxAccess.ItemsSource = Enum.GetValues(typeof(AdskConstructionCloudBreakdown.AccessPermissionEnum));
            UserNameAnswer.Text = username;
            CompanyAnswer.Text = company;
            ComboBoxAccess.SelectedItem = access;
            //in case that user canceled the input
            UserRet = UserNameAnswer.Text;
            CompanyRet = CompanyAnswer.Text;
            AccessRet = (AccessPermissionEnum)ComboBoxAccess.SelectedItem;

        }

        private void btnDialogOk_Click(object sender, RoutedEventArgs e)
        {
            //set values for config
            UserRet=UserNameAnswer.Text;
            CompanyRet=CompanyAnswer.Text;
            AccessRet=(AccessPermissionEnum)ComboBoxAccess.SelectedItem;

            this.DialogResult = true;
        }

        private void Window_ContentRendered(object sender, EventArgs e)
        {
            UserNameAnswer.SelectAll();
            UserNameAnswer.Focus();

        }

        
    }
}