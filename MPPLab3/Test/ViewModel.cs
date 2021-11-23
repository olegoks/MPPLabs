using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows.Input;
using Test;
using Microsoft.Win32;
using static System.String;
using AssemblyBrowserLibrary;

namespace Test {
    public class ViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public ICommand OpenFileCommand => new OpenFileCommand(OpenAssembly);
        public string OpenedFile { get; set; } = "File not opened";
        public IEnumerable<INode> namespaces { get; set; }

        private void OnPropertyChanged(string propertyName) {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        private void OpenAssembly()
        {
            var openFileDialog = new OpenFileDialog {
                Filter = "Assemblies|*.dll;*.exe",
                Title = "Select assembly",
                Multiselect = false
            };
            
            var isOpen = openFileDialog.ShowDialog();

            if (isOpen != null && isOpen.Value) {
                OpenedFile = openFileDialog.FileName;
                OnPropertyChanged(nameof(OpenedFile));

                namespaces = AssemblyBrowserLibrary.AssemblyBrowser.GetAssemblyInfo(OpenedFile);               
                AddSpaces(namespaces);
                OnPropertyChanged(nameof(namespaces));
            }
        }
        private static void AddSpaces(IEnumerable<INode> nodes)
        {
            foreach (var node in nodes)
            {
                var properties = node.GetType()
                    .GetProperties()
                    .Where(property => property.Name != "nodes")
                    .Where(property => !IsNullOrEmpty((string)property.GetValue(node)))
                    .ToList();

                foreach (var property in properties)
                {
                    property.SetValue(node, " " + property.GetValue(node));
                }
                AddSpaces(node.nodes);
            }
        }
    }
}