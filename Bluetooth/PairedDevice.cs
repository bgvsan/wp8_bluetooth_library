using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Networking.Proximity;
using Windows.Networking.Sockets;
using System.Linq;
using Windows.Storage.Streams;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Collections.ObjectModel;
using Microsoft.Phone.Tasks;


namespace Bluetooth
{
    public class PairedDevice : BluetoothManager
    {
        private static ObservableCollection<PeerInformation> _PairedDeviceList;
        private PairedDevice() { }

        public static ObservableCollection<PeerInformation> PairedDeviceList
        {
            get
            {
                if (_PairedDeviceList == null)
                {
                    _PairedDeviceList = new ObservableCollection<PeerInformation>();
                }
                return _PairedDeviceList;
            }
        }


        private static StreamSocket _Socket;
        //private Socket(){}

        //public static StreamSocket GetSocket
        //{
        //    get
        //    {
        //        if (_Socket == null)
        //        {
        //            _Socket = new StreamSocket();
        //        }
        //        return _Socket;
        //    }
        //}

        /// <summary>
        /// Add device to list of paired Devides
        /// </summary>
        /// <param name="device"></param>
        public static void Add(PeerInformation device)
        {
            PairedDeviceList.Add(device);
        }

        /// <summary>
        /// Clean Paired Device List
        /// </summary>
        public static void Clear()
        {
            PairedDeviceList.Clear();
        }

        #region Methods

        /// <summary>
        /// Show to window the bluethooth control to switch on bluetooth
        /// </summary>
        public override void ShowBluetoothControlPanel()
        {
            ConnectionSettingsTask connectionSettingTask = new ConnectionSettingsTask();
            connectionSettingTask.ConnectionSettingsType = ConnectionSettingsType.Bluetooth;
            connectionSettingTask.Show();
        }


        /// <summary>
        /// Connect a Peer with specific macAddress
        /// </summary>
        /// <param name="macaddress"></param>
        /// <returns></returns>
        private async Task<bool> ConnectToDevice(string macaddress)
        {
            PeerFinder.AlternateIdentities["Bluetooth:Paired"] = "";
            //cerca tutti i dispositivi pairati
            var devices = await PeerFinder.FindAllPeersAsync();
            List<PeerInformation> devices_list = devices.ToList();
            try
            {
                PeerInformation peer = devices_list.Where(s => s.HostName.CanonicalName.Equals("(" + macaddress.ToUpper() + ")")).FirstOrDefault();
                if (peer != null)
                {
                    _Socket = new StreamSocket();
                    await _Socket.ConnectAsync(peer.HostName, "1");
                    //MessageBox.Show(AppResources.msgConnessioneAvvenuta);
                    return true;
                }

                return false;
            }

            catch (Exception ex)
            {
                switch ((uint)ex.HResult)
                {
                    case 0x8007048F:
                        {
                            //Bluetooth spento
                            break;
                        }
                    case 0x80072750:
                        {
                            //Host Down
                            break;
                        }
                    default:
                        {

                            //all the other
                            break;
                        }
                }
                close();
                return false;
            }
        }



        /// <summary>
        /// Send data to the connected device
        /// </summary>
        /// <param name="package"></param>
        /// <param name="packet_lenght"></param>
        /// <returns></returns>
        public async Task<byte[]> send_data(byte[] package, uint packet_lenght)
        {
            byte[] result_buff = new byte[packet_lenght];
            try
            {
                if (_Socket != null)
                {
                    var data = GetBufferFromByteArray(package);
                    //li sparo su buffer
                    var temp = await _Socket.OutputStream.WriteAsync(data);
                    //controllo lo stream in ingresso
                    var xx = await _Socket.InputStream.ReadAsync(package.AsBuffer(), packet_lenght, InputStreamOptions.None);
                    DataReader read = DataReader.FromBuffer(xx);
                    read.ReadBytes(result_buff);
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("errore: " + ex);
            }
            return result_buff;
        }

        /// <summary>
        /// Transform data from Array to buffer
        /// </summary>
        /// <param name="package"></param>
        /// <returns></returns>
        private IBuffer GetBufferFromByteArray(byte[] package)
        {
            using (DataWriter dw = new DataWriter())
            {
                dw.WriteBytes(package);
                return dw.DetachBuffer();
            }
        }

        /// <summary>
        /// Attempt to close connection with socket
        /// </summary>
        private void close()
        {
            if (_Socket != null)
            {
                try
                {
                    _Socket.InputStream.Dispose();
                    _Socket.OutputStream.Dispose();
                    _Socket.Dispose();
                    GC.Collect();
                }
                catch (Exception e)
                {

                    System.Diagnostics.Debug.WriteLine("_socket close dispose error " + e.ToString() + " " + e.StackTrace);
                    //Tenta la chiusura..
                }
            }



        }

        #endregion

    }

}
