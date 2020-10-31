// ------------------------------------------------------------------------
// SunSh1n3 - Call of Duty: Black Ops III LVI Editor
// Copyright(C) 2020 Philip/Scobaluila
// ------------------------------------------------------------------------
// This program is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
// ------------------------------------------------------------------------
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.See the
// GNU General Public License for more details.
// ------------------------------------------------------------------------
// You should have received a copy of the GNU General Public License
// along with this program.If not, see<http://www.gnu.org/licenses/>.
// ------------------------------------------------------------------------
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
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
using SunSh1n3.IO;
using SunSh1n3.Utility;

namespace SunSh1n3
{
    public partial class MainWindow : Window
    {
        private static string GradesFolder
        {
            get
            {
                return Path.Combine(Environment.GetEnvironmentVariable("TA_TOOLS_PATH") ?? "", "share", "raw", "grades");
            }
        }

        public ObservableCollection<LVIValue> LVIBloomBlockValues { get; } = new ObservableCollection<LVIValue>()
        {
            new LVIValue("bloom_vEV"),
            new LVIValue("bloom_vPWR"),
            new LVIValue("bloom_vRGB1"),
            new LVIValue("bloom_vLUM1"),
            new LVIValue("bloom_vRGB2"),
            new LVIValue("bloom_vLUM2"),
            new LVIValue("bloom_vRGB3"),
            new LVIValue("bloom_vLUM3"),
            new LVIValue("bloom_vRGB4"),
            new LVIValue("bloom_vLUM4"),
            new LVIValue("bloom_vRGB5"),
            new LVIValue("bloom_vLUM5"),
            new LVIValue("bloom_vLIM"),
            new LVIValue("bloom_vLIA"),
            new LVIValue("bloom_vLIG"),
            new LVIValue("bloom_vMxR"),
            new LVIValue("bloom_vMxG"),
            new LVIValue("bloom_vMxB"),
        };

        public ObservableCollection<LVIValue> LVIColourBlockValues { get; } = new ObservableCollection<LVIValue>()
        {
            new LVIValue("color_vLumaM"),
            new LVIValue("color_vRangeM"),
            new LVIValue("color_vRangeA"),
            new LVIValue("color_vMtxSR"),
            new LVIValue("color_vMtxSG"),
            new LVIValue("color_vMtxSB"),
            new LVIValue("color_vMtxMR"),
            new LVIValue("color_vMtxMG"),
            new LVIValue("color_vMtxMB"),
            new LVIValue("color_vMtxHR"),
            new LVIValue("color_vMtxHG"),
            new LVIValue("color_vMtxHB"),
            new LVIValue("color_vMixR"),
            new LVIValue("color_vMixG"),
            new LVIValue("color_vMixB"),
        };

        public string FilePath { get; set; }

        public MainWindow()
        {
            InitializeComponent();
            DataContext = this;
        }

        private void WindowKeyDown(object sender, KeyEventArgs e)
        {
            if (Keyboard.Modifiers.HasFlag(ModifierKeys.Control))
            {
                if(e.Key == Key.S)
                {
                    if(Keyboard.Modifiers.HasFlag(ModifierKeys.Shift))
                    {
                        SaveAsClick(null, null);
                    }
                    else
                    {
                        SaveClick(null, null);
                    }
                }
                else if (e.Key == Key.O)
                {
                    OpenClick(null, null);
                }
                else if (e.Key == Key.N)
                {
                    NewClick(null, null);
                }
            }
        }

        private void SaveLVIFile(string filePath)
        {
            using (var writer = new BinaryWriter(File.Create(filePath)))
            {
                writer.Write(0x3032564C); // Magic (LV20)
                writer.Write((long)0x452); // File Size (Constant)
                writer.Write(Enumerable.Repeat((byte)0x0, 536).ToArray()); // PC Name, Project Name, etc. Not needed

                // Bloom GPU Block ID and Size
                writer.Write(new byte[] { 0x3C, 0x42, 0x4C, 0x4F, 0x4F, 0x4D, 0x47, 0x50, 0x55, 0x3E, 0x00, 0x20, 0x01, 0x00, 0x00 });

                foreach(var value in LVIBloomBlockValues)
                {
                    writer.Write(value.X);
                    writer.Write(value.Y);
                    writer.Write(value.Z);
                    writer.Write(value.W);
                }

                // Color GPU Block ID and Size
                writer.Write(new byte[] { 0x3C, 0x43, 0x4F, 0x4C, 0x4F, 0x52, 0x47, 0x50, 0x55, 0x3E, 0x00, 0xF0, 0x00, 0x00, 0x00 });

                foreach (var value in LVIColourBlockValues)
                {
                    writer.Write(value.X);
                    writer.Write(value.Y);
                    writer.Write(value.Z);
                    writer.Write(value.W);
                }
            }
        }

        private void LoadLVIFile(string filePath)
        {
            FilePath = filePath;

            using (var reader = new BinaryReader(File.OpenRead(filePath)))
            {
                if (reader.ReadUInt32() != 0x3032564C)
                    throw new ArgumentException("Invalid LVI Magic, expecting LV20", "magic");

                // Skip header, we don't need it
                reader.BaseStream.Position += 544;

                while(reader.BaseStream.Position < reader.BaseStream.Length)
                {
                    var blockMagic = reader.ReadNullTerminatedString();

                    if (string.IsNullOrWhiteSpace(blockMagic))
                        break;

                    var blockSize = reader.ReadInt32();

                    if(blockMagic == "<COLORGPU>")
                    {
                        if (blockSize != 240)
                            throw new ArgumentException("Invalid Colour GPU Block Size, expecting 240.", "blockSize");

                        foreach (var value in LVIColourBlockValues)
                        {
                            value.X = reader.ReadSingle();
                            value.Y = reader.ReadSingle();
                            value.Z = reader.ReadSingle();
                            value.W = reader.ReadSingle();
                        }
                    }
                    else if (blockMagic == "<BLOOMGPU>")
                    {
                        if (blockSize != 288)
                            throw new ArgumentException("Invalid Colour GPU Block Size, expecting 288.", "blockSize");

                        foreach (var value in LVIBloomBlockValues)
                        {
                            value.X = reader.ReadSingle();
                            value.Y = reader.ReadSingle();
                            value.Z = reader.ReadSingle();
                            value.W = reader.ReadSingle();
                        }
                    }
                    else
                    {
                        reader.BaseStream.Position += blockSize;
                    }
                }
            }
        }

        private void NewClick(object sender, RoutedEventArgs e)
        {
            var result = MessageBox.Show("Creating a new LVI file will clear all current values will clear all values. Are you sure?", "SunSh1n3", MessageBoxButton.YesNo, MessageBoxImage.Warning);

            if(result == MessageBoxResult.Yes)
            {
                foreach (var value in LVIBloomBlockValues)
                {
                    value.Reset();
                }

                foreach (var value in LVIColourBlockValues)
                {
                    value.Reset();
                }

                FilePath = "";
            }
        }

        private void OpenClick(object sender, RoutedEventArgs e)
        {
            try
            {
                var dialog = new OpenFileDialog()
                {
                    Title = "SunSh1n3 | Open File",
                    Filter = "LVI Grade file (*.lvi)|*.lvi",
                    InitialDirectory = string.IsNullOrWhiteSpace(FilePath) ? GradesFolder : Path.GetDirectoryName(FilePath)
                };

                if (dialog.ShowDialog() == true)
                {
                    LoadLVIFile(dialog.FileName);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(string.Format("An error has occured:\n\n{0}", ex), "SunSh1n3", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void SaveClick(object sender, RoutedEventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(FilePath))
                {
                    var dialog = new SaveFileDialog()
                    {
                        Title = "SunSh1n3 | Save File",
                        Filter = "LVI Grade file (*.lvi)|*.lvi",
                        InitialDirectory = string.IsNullOrWhiteSpace(FilePath) ? GradesFolder : Path.GetDirectoryName(FilePath)
                    };

                    if (dialog.ShowDialog() == true)
                    {
                        FilePath = dialog.FileName;
                    }
                }

                if (!string.IsNullOrWhiteSpace(FilePath))
                    SaveLVIFile(FilePath);
            }
            catch(Exception ex)
            {
                MessageBox.Show(string.Format("An error has occured:\n\n{0}", ex), "SunSh1n3", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void SaveAsClick(object sender, RoutedEventArgs e)
        {
            try
            {
                var dialog = new SaveFileDialog()
                {
                    Title = "SunSh1n3 | Save File As",
                    Filter = "LVI Grade file (*.lvi)|*.lvi",
                    InitialDirectory = string.IsNullOrWhiteSpace(FilePath) ? GradesFolder : Path.GetDirectoryName(FilePath)
                };

                if (dialog.ShowDialog() == true)
                {
                    FilePath = dialog.FileName;
                    SaveLVIFile(FilePath);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(string.Format("An error has occured:\n\n{0}", ex), "SunSh1n3", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void TextBoxGotFocus(object sender, RoutedEventArgs e)
        {
            ((TextBox)sender).SelectionStart = 0;
            ((TextBox)sender).SelectionLength = ((TextBox)sender).Text.Length;
        }

        private void ListViewDrop(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                var files = (string[])e.Data.GetData(DataFormats.FileDrop);

                foreach (var file in files)
                    if(Path.GetExtension(file).ToLower() == ".lvi")
                        LoadLVIFile(file);
            }
        }
    }
}
