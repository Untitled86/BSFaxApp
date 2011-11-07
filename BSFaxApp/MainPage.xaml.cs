using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using System.IO;
using System.Windows.Media.Imaging;
using System.Collections;
using Microsoft.Phone.Tasks;

namespace BSFaxApp
{
    public partial class MainPage : PhoneApplicationPage
    {
        // Constructor
        public MainPage()
        {
            InitializeComponent();

            ApplicationBar.Buttons.Clear();
            setupButtonIcons();

            // Set the data context of the listbox control to the sample data
            DataContext = App.ViewModel;
            this.Loaded += new RoutedEventHandler(MainPage_Loaded);            
        }

        // Load data for the ViewModel Items
        private void MainPage_Loaded(object sender, RoutedEventArgs e)
        {
            //ApplicationBar.IsVisible = false;
            /*if (!App.ViewModel.IsDataLoaded)
            {
                App.ViewModel.LoadData();
            }*/
        }

        private void ListBox2_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //((BSFaxApp.ItemViewModel)e.AddedItems[0]).LineOne
            Console.Write(sender.ToString());
            Microsoft.Phone.Tasks.PhotoChooserTask task = new Microsoft.Phone.Tasks.PhotoChooserTask();
            task.Completed += new EventHandler<Microsoft.Phone.Tasks.PhotoResult>(task_Completed);
            task.Show();
        }

        void task_Completed(object sender, Microsoft.Phone.Tasks.PhotoResult e)
        {
            if (e.TaskResult == Microsoft.Phone.Tasks.TaskResult.OK)
            {
                MessageBox.Show("" + e.ChosenPhoto.ToString());                
            }
        }

        private void ListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Console.Write(sender.ToString());
            
            Microsoft.Phone.Tasks.PhoneNumberChooserTask task = new Microsoft.Phone.Tasks.PhoneNumberChooserTask();
            
            task.Completed += new EventHandler<Microsoft.Phone.Tasks.PhoneNumberResult>(task_Completed);
            task.Show();
        }

        void task_Completed(object sender, Microsoft.Phone.Tasks.PhoneNumberResult e)
        {
            if (e.TaskResult == Microsoft.Phone.Tasks.TaskResult.OK)
            {
                MessageBox.Show(e.PhoneNumber);
            }
        }

        ApplicationBarIconButton syncIcon = null;
        ApplicationBarIconButton addIcon = null;
        ApplicationBarIconButton uploadIcon = null;        

        private void setupButtonIcons()
        {
            syncIcon = getIconFromName("sync", "/icons/appbar.sync.rest.png");
            addIcon = getIconFromName("add pages", "/icons/appbar.add.rest.png");
            uploadIcon = getIconFromName("send", "/icons/appbar.upload.rest.png");
        }

        private void Panorama_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {/*
            string header = ((PanoramaItem)((Panorama)sender).SelectedItem).Header.ToString();
            
            ApplicationBar.Buttons.Clear();

            if (header.Equals("quick fax", StringComparison.InvariantCultureIgnoreCase))
            {
                if (ApplicationBar.Buttons.IndexOf(addIcon) == -1)
                {
                    ApplicationBar.Buttons.Add(addIcon);
                }
                ApplicationBarIconButton sendButton = uploadIcon;
                sendButton.Click += new EventHandler(sendButton_Click);

                if (ApplicationBar.Buttons.IndexOf(sendButton) == -1)
                {
                    ApplicationBar.Buttons.Add(sendButton);                
                }

                ApplicationBar.IsVisible = true;
            }
            else if ((header.Equals("how it works", StringComparison.InvariantCultureIgnoreCase)) ||
                     (header.Equals("templates", StringComparison.InvariantCultureIgnoreCase)))
            {
                ApplicationBar.IsVisible = true;
            }
            else if (header.Equals("sent faxes", StringComparison.InvariantCultureIgnoreCase))
            {
                if (ApplicationBar.Buttons.IndexOf(syncIcon) == -1)
                {
                    ApplicationBar.Buttons.Add(syncIcon);
                }
                ApplicationBar.IsVisible = true;
            }
            else if (header.Equals("received faxes", StringComparison.InvariantCultureIgnoreCase))
            {

                if (ApplicationBar.Buttons.IndexOf(syncIcon) == -1)
                {
                    ApplicationBar.Buttons.Add(syncIcon);
                }
                ApplicationBar.IsVisible = true;
            }
            else
            {
                ApplicationBar.IsVisible = true;
            }*/

        }

        void sendButton_Click(object sender, EventArgs e)
        {
            //System.Text.UTF8Encoding encoding = new System.Text.UTF8Encoding();
            //System.IO.MemoryStream memoryStream = new MemoryStream(encoding.GetBytes("this is a test"));
            //UploadUtil.StartUpload("http://brian_18_or@suite304.com:byeG00d@www.suite304.com/test.txt", memoryStream);
            //UploadUtil.Uploads.Add(
            FaxService.Version1Client wsClient = new FaxService.Version1Client();
            wsClient.PingCompleted += new EventHandler<FaxService.PingCompletedEventArgs>(wsClient_PingCompleted);
            wsClient.PingAsync();

            FaxService.Fax fax = new FaxService.Fax();

            System.Collections.ObjectModel.ObservableCollection<string> recipients = new System.Collections.ObjectModel.ObservableCollection<string>();
            recipients.Add("Brien Schultz");
            fax.Recipients = recipients;
            wsClient.SendFaxCompleted += new EventHandler<FaxService.SendFaxCompletedEventArgs>(wsClient_SendCompleted);            
            wsClient.SendFaxAsync(fax);
            MessageBox.Show("Fax started...");

            
            WriteableBitmap wb = new WriteableBitmap(300, 300);
            byte[] data = WriteableBitmapExtensions.ToByteArray(wb);
            
            
        }

        void wsClient_SendCompleted(object sender, FaxService.SendFaxCompletedEventArgs e)
        {
            MessageBox.Show("Fax completed.");
        }

        void wsClient_PingCompleted(object sender, FaxService.PingCompletedEventArgs e)
        {
            MessageBox.Show("Ping Result: " + e.Result.ToString());
        }

        private ApplicationBarIconButton getIconFromName(string aTitle, string aIconUri)
        {
            ApplicationBarIconButton result = new ApplicationBarIconButton();
            result.Text = aTitle;
            result.IsEnabled = true;
            result.IconUri = new Uri(aIconUri, UriKind.Relative);
            return result;
        }

        private void button1_Click(object sender, RoutedEventArgs e)
        {
            
            
            WebBrowserTask task = new WebBrowserTask();
            task.URL = Uri.EscapeUriString("https://www.metrofax.com/soho/signupform.aspx?referringparty=Brien Schultz&referral=Yes");
            task.Show();
        }
    }
}