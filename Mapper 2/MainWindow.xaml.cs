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
using DmxControlLib.Hardware;
using DmxControlLib.Utility;
using DmxUserControlLib;
using Microsoft.Win32;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

namespace Mapper_2
{
    /// <summary>
    /// Logique d'interaction pour MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        LPMMap map;
        OpenFileDialog opendial;
        SaveFileDialog savedial;
        

        public MainWindow()
        {
            InitializeComponent();

            try
            {
                LaunchPadControl.Connect();
            }
            catch(Exception)
            {
                MessageBox.Show("Impossible de connecter un LaunchPad");
            }
            map = new LPMMap("newmap");
            LaunchPadControl.LinkMapping(map);
            MapName_TextBox.Text = map.name;

            opendial = new OpenFileDialog();
            opendial.Filter = "Mapping Setting (.map)|*.map";
            

            savedial = new SaveFileDialog();
            savedial.Filter = "Mapping Setting (.map)|*.map";
            savedial.DefaultExt = ".map";
            savedial.AddExtension = true;

        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            LaunchPadControl.Reset();
            LaunchPadControl.close();
        }

        private void LPBproperty_BT_Valid_Click(object sender, EventArgs e)
        {
            if(sender is LaunchpadBTProperty)
            {
                LaunchpadBTProperty bt = sender as LaunchpadBTProperty;
                foreach(int sel in launchpadmap.selectedBT)
                {
                    int ind = map.BT.FindIndex(x => x.ID == sel);
                    map.BT[ind].Type = bt.BTType;
                    map.BT[ind].onColor = bt.ONBTColor;
                    map.BT[ind].offColor = bt.OFFBTColor;
                    map.BT[ind].onFlashing = bt.ONFlash;
                    map.BT[ind].offFlashing = bt.OFFFlash;
                }

                foreach (int sel in launchpadmap.selectedSystemBT)
                {
                    int ind = map.SysBT.FindIndex(x => x.ID == sel);
                    map.SysBT[ind].Type = bt.BTType;
                    map.SysBT[ind].onColor = bt.ONBTColor;
                    map.SysBT[ind].offColor = bt.OFFBTColor;
                    map.SysBT[ind].onFlashing = bt.ONFlash;
                    map.SysBT[ind].offFlashing = bt.OFFFlash;
                }

                LaunchPadControl.LinkMapping(map);
            }
        }

        private void New_Button_Click(object sender, RoutedEventArgs e)
        {
            map = new LPMMap("newMap");
            LaunchPadControl.LinkMapping(map);

            MapName_TextBox.Text = map.name;
        }

        private void Open_button_Click(object sender, RoutedEventArgs e)
        {
            Nullable<bool> result = opendial.ShowDialog();

            if(result == true)
            {
                BinaryFormatter format = new BinaryFormatter();
                Stream STRE = opendial.OpenFile();

                map = (LPMMap)format.Deserialize(STRE);

                LaunchPadControl.LinkMapping(map);
                MapName_TextBox.Text = map.name;

                STRE.Close();
            }
        }

        private void Save_Button_Click(object sender, RoutedEventArgs e)
        {
            savedial.FileName = MapName_TextBox.Text;
            Nullable<bool> result = savedial.ShowDialog();

            if(result == true)
            {
                BinaryFormatter format = new BinaryFormatter();
                Stream STRE = savedial.OpenFile();

                format.Serialize(STRE, map);

                STRE.Close();
            }
        }
    }
}
