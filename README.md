wp8_bluetooth_library
=====================

wp8_bluetooth_library has a GPL license because I believe in open development.


QUICK INFORMATION
=====================

This is a complete project that all can contribute, or can be used as is!
If you have any specifical request or need info please refer to my email address santinelli.diego[at]gmail.com.

IMPLEMENTED FUNCTION
=====================

```csharp
-ConnectToDevice(string macAddress);

-Send_Data(byte[] package, uint response_packet_lenght, string macAddress)
```

USAGE
=====================

```csharp
private async void Send_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Bluetooth.BTConnection btpd = new BTConnection();
                await btpd.ConnectToDevice(this.txtblkAddress.Text);
                byte[] data_to_send = new byte[] { 0X01, 0X02, 0X03 };
                await btpd.Send_Data(data_to_send, 1, this.txtblkAddress.Text);
            }
            catch (BluetoothDeviceException ex)
            {
                MessageBox.Show(ex.Message.ToString());
            }

        }
```
