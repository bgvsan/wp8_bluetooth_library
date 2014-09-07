QUICK INFORMATION
=====================
Is a wp8 - wp8.1 Library for hc-06/hc-05 bluetooth module.
You can connect , send and receive data packet to your module.
wp8_bluetooth_library has a GPL license.

This is a complete project that all can contribute, or can be used as is!
If you have any specifical request or need info please refer to my email address santinelli.diego[at]gmail.com.

![Alt HC-06 Module](https://www.github.com/bgvsan/wp8_bluetooth_library/bluetooth-HC05-01.jpg "HC-06 Module")

IMPLEMENTED FUNCTION
=====================

```csharp
-ConnectToDevice(string macAddress);

-Send_Data(byte[] package, uint response_packet_lenght, string macAddress)
```

USAGE
=====================

Connect to a device (You need the mac address)

```csharp

```

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
