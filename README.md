QUICK INFORMATION
=====================
Is a wp8 - wp8.1 Library for hc-06/hc-05 bluetooth module.
You can connect , send and receive data packet to your module.
wp8_bluetooth_library has a GPL license.

This is a complete project that all can contribute, or can be used as is!
If you have any specifical request or need info please refer to my email address santinelli.diego[at]gmail.com.

![Alt HC-06 Module](/image/bluetooth-HC05-01.jpg?raw=true "HC-06 Module")

IMPLEMENTED FUNCTION
=====================

```csharp
- ConnectToDevice(string macAddress);

- Send_Data(byte[] package, uint response_packet_lenght, string macAddress)
```

USAGE
=====================

Connect to a device & send data (You need the mac address)

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

Send data and receive response from bluetooth module

```csharp
	await btpd.Send_Data((Byte[]])data_to_send, (int)response_lenght, (string)macAddress);
```

mac Address format string : XXXXXXXXXXXX

EXCEPTION
=====================
Always use a try & catch between the connection

Open panel tho switch on bluetooth
```csharp
BluetoothDeviceException("Bluetooth Off",ex.InnerException);
```

No device found "pair first the bluetooth device""
```csharp
 BluetoothDeviceException("No device associated")
 ```  

No response from device 
```csharp
BluetoothDeviceException("Host Down", ex.InnerException);
```                      

Check ID_CAPTION permission to your project
```csharp
BluetoothDeviceException("An attempt was made to access a socket in a way forbidden by its access permissions check ID_CAPTION ", ex.InnerException);
```

Generic exception check to msdn
```csharp
BluetoothDeviceException("Generic Exception", ex.InnerException);
```
