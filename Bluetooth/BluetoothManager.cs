using Microsoft.Phone.Tasks;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Net;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Windows.Networking.Proximity;
using Windows.Networking.Sockets;
using System.Linq;
using Windows.Storage.Streams;
using System.Runtime.InteropServices.WindowsRuntime;

namespace Bluetooth
{
    public  class  BluetoothManager
    {
        ///<summary>
        ///Show to window the bluethooth control to switch on bluetooth
        ///</summary>
        public static void ShowBluetoothControlPanel()
        {
            ConnectionSettingsTask connectionSettingTask = new ConnectionSettingsTask();
            connectionSettingTask.ConnectionSettingsType = ConnectionSettingsType.Bluetooth;
            connectionSettingTask.Show();
        }
    }

    public class PairedDeviceInfo
    {

        internal PairedDeviceInfo(PeerInformation peerInformation)
        {
            this.PeerInfo = peerInformation;
            this.DisplayName = this.PeerInfo.DisplayName;
            this.HostName = this.PeerInfo.HostName.DisplayName;
            this.ServiceName = this.PeerInfo.ServiceName;
        }
        public string DisplayName { get; private set; }
        public string HostName { get; private set; }
        public string ServiceName { get; private set; }
        public PeerInformation PeerInfo { get; private set; } 
    }

    public class BluetoothDeviceException : Exception
    {

        public BluetoothDeviceException() : base() { }
        public BluetoothDeviceException(string message) : base(message) { }
        public BluetoothDeviceException(string message, System.Exception inner) : base(message, inner) { }

       }
      
    }



