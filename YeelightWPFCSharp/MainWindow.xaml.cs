using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using YeelightAPI;

namespace YeelightWPFCSharp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        Device device;
        List<Device> devices;

        public MainWindow()
        {
            InitializeComponent();
            statusLabel.Content = "Disconnected";
            statusLabel.Foreground = Brushes.Red;
            RefreshDevices();
        }

        private void ConnectBtn_Click(object sender, RoutedEventArgs e)
        {
            ConnectAsync(deviceTxt.Text);
        }

        private void OnBtn_Click(object sender, RoutedEventArgs e)
        {
            if(device != null)
                device.SetPower();
        }

        private void OffBtn_Click(object sender, RoutedEventArgs e)
        {
            if (device != null)
                device.SetPower(false);
        }

        private async void RefreshDevices()
        {
            devices = null;
            devices = await DeviceLocator.Discover();

            if (devicesListView.HasItems)
            {
                devicesListView.Items.Clear();
            }

            if (devices.Count > 0)
            {
                foreach (Device device in devices)
                {
                    devicesListView.Items.Add(device);
                }
            }
        }

        private void RefreshBtn_Click(object sender, RoutedEventArgs e)
        {
            RefreshDevices();
        }
        
        private void devicesListView_PreviewMouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            var item = sender as ListView;
            var dev = item.SelectedItem as Device;
            if (dev != null)
            {
                ConnectAsync(dev.Hostname);
            }
        }

        private async void ConnectAsync(string ip)
        {
            if (ip != "")
            {
                device = new Device(ip);
                bool isConnected = await device.Connect();
                if (isConnected)
                {
                    statusLabel.Content = "Connected";
                    statusLabel.Foreground = Brushes.Green;
                }
            }
            else
            {
                MessageBox.Show("Please specify a valid IP");
            }
        }
    }
}
