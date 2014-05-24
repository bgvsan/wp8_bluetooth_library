using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Networking.Proximity;
using Windows.Networking.Sockets;
using Windows.Storage.Streams;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Collections.ObjectModel;
using Microsoft.Phone.Tasks;


namespace Bluetooth
{
    public class PairedDevice  
    {
        protected static StreamSocket _Socket;
        protected PairedDevice()
        { }

        protected static ObservableCollection<PairedDeviceInfo> _PairedDeviceList;
       
        public static ObservableCollection<PairedDeviceInfo> PairedDeviceList
        {
            get
            {
                if (_PairedDeviceList == null)
                {
                    _PairedDeviceList = new ObservableCollection<PairedDeviceInfo>();
                }
                return _PairedDeviceList;
            }
        }

        /// <summary>
        /// Add device to list of paired Devides
        /// </summary>
        /// <param name="device"></param>
        private static void Add(PairedDeviceInfo device)
        {
            PairedDeviceList.Add(device);
        }

        /// <summary>
        /// Clean Paired Device List
        /// </summary>
        private static void Clear()
        {
            PairedDeviceList.Clear();
        }

        #region Methods

     

        

        /// <summary>
        /// Update paired device list
        /// </summary>
        /// <returns></returns>
        public static async void RefreshList()
        {

            try
            {
                //cerca tutti i dispositivi pairati
                PeerFinder.AlternateIdentities["Bluetooth:Paired"] = "";
                var peers = await PeerFinder.FindAllPeersAsync();

                //Rimuovo tutti i device dalla lista
                PairedDeviceList.Clear();
                {
                    foreach (var peer in peers)
                    {
                        PairedDeviceList.Add(new PairedDeviceInfo(peer));
                    }
                }

            }
            catch (Exception e)
            {
                throw new BluetoothDeviceException("Error Retrive Paired Devices", e.InnerException);
            }

        }


        #endregion

    }
    public class BTConnection : PairedDevice
    {
        /// <summary>
        /// Send data to the connected device and wait for response
        /// </summary>
        /// <param name="package">Data to send to connected device</param>
        /// <param name="packet_lenght">Response lenght to await</param>
        /// <returns></returns>
        public async Task<byte[]> Send_Data(byte[] package, uint packet_lenght, string macaddress)
        {
            byte[] result_buff = new byte[packet_lenght];
            try
            {
                if (_Socket!= null)
                {
                    await ConnectToDevice(macaddress);
                }
                else
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
                throw new BluetoothDeviceException("Error sending Data to connected Device", ex.InnerException);
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
        /// Connect a Peer with specific macAddress
        /// </summary>
        /// <param name="macaddress"></param>
        /// <returns></returns>
        public async Task<bool> ConnectToDevice(string macaddress)
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
                    return true;
                }

                throw new BluetoothDeviceException("No device associated");
            }


            catch (Exception ex)
            {
                switch ((uint)ex.HResult)
                {
                    case 0x8007048F:
                        {

                            //Bluetooth spento
                            throw new BluetoothDeviceException("Bluetooth Off", ex.InnerException);
                        }
                    case 0x80072750:
                        {
                            //Host Down
                            throw new BluetoothDeviceException("Host Down", ex.InnerException);
                        }
                    default:
                        {
                            //all the other
                            throw new BluetoothDeviceException("Generic Exception", ex.InnerException);
                        }
                }

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
    }
}
    


