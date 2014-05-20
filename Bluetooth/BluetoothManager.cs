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
    public abstract class BluetoothManager : IBluetoothManager
    {

        public string DisplayName { get; set; }
        public string HostName { get; set; }
        public string ServiceName { get; set; }
        public PeerInformation PeerInfo { get; set; }

        public abstract void ShowBluetoothControlPanel();
    }

}