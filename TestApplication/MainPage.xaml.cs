using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using TestApplication.Resources;
using Bluetooth;

namespace TestApplication
{
    public partial class MainPage : PhoneApplicationPage
    {
        // Costruttore
        public MainPage()
        {
            InitializeComponent();

            // Codice di esempio per localizzare la ApplicationBar
            //BuildLocalizedApplicationBar();
        }

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

        private void Connect_Click(object sender, RoutedEventArgs e)
        {
            
        }
        
        // Codice di esempio per la realizzazione di una ApplicationBar localizzata
        //private void BuildLocalizedApplicationBar()
        //{
        //    // Imposta la barra delle applicazioni della pagina su una nuova istanza di ApplicationBar
        //    ApplicationBar = nuova ApplicationBar();

        //    // Crea un nuovo pulsante e imposta il valore del testo sulla stringa localizzata da AppResources.
        //    ApplicationBarIconButton appBarButton = new ApplicationBarIconButton(new Uri("/Assets/AppBar/appbar.add.rest.png", UriKind.Relative));
        //    appBarButton.Text = AppResources.AppBarButtonText;
        //    ApplicationBar.Buttons.Add(appBarButton);

        //    // Crea una nuova voce di menu con la stringa localizzata da AppResources.
        //    ApplicationBarMenuItem appBarMenuItem = new ApplicationBarMenuItem(AppResources.AppBarMenuItemText);
        //    ApplicationBar.MenuItems.Add(appBarMenuItem);
        //}
    }
}