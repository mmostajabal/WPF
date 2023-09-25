using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Reflection.Emit;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Test.Classes;

namespace Test
{
    // -- WPF Part --
    // 1. Execute the loading command on clicking the "Load" button
    // 2. Execute the export command on clicking the "Export" button
    // 3. Change the text of the "Load" Button to "Loading" while the data is loaded
    // 4. Display the loaded data in the left ListBox.
    // 5. On Selecting an instance in this list, display its "Properties" in the second ListBox
    // 6. For the first ListBox, display the name and the template name of each entity. The name shall be in bold.
    // 7. For the second ListBox, display the name, the value and the type of each entry. The name shall be in bold.
    // 8. Disable the Export button while data is still loading

    // -- Data handling part --
    // Use the loaded data ("SmartObjects") and export them as a CSV:
    // 1. For each row, the data of a SmartObject shall be displayed.
    // 2. The first column shall be the "Name" of the SmartObject
    // 3. The second column shall be the "TemplateName" of the SmartObject
    // 4. The following rows shall be aggregated by the different released properties of the SmartObjects
    //    i.e. one column per unique property. Properties which do not exist for a SmartObject shall be left empty.
    // 5. Write the result to a file (can be hardcoded)
    // Example output:
    //    Name,Template,String length, Initial value,Min.set value,
    //    InstanceA,SOT1,5,Other value,,
    //    InstanceB,SOT2,,,5,
    //    InstanceC,SOT1,5,Value,,


    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        #region Test Data

        public int columnMinWidth { get; } = 100;

        private readonly SmartObjectInfo[] _testItems = new[]
        {
      new SmartObjectInfo
      {
        Name = "Valve1",
        TemplateName = "Valve",
        Properties =
        {
          new SmartObjectProperty
          {
            Name = "Initial Value",
            Value = 123,
            Type = typeof(int)
          },
        }
      },
      new SmartObjectInfo
      {
        Name = "Valve2",
        TemplateName = "Valve",
        Properties =
        {
          new SmartObjectProperty
          {
            Name = "Initial Value",


          },
          new SmartObjectProperty
          {
            Name = "StartUpFunction",
            Value = "NewWindowFunc",
            Type = typeof(string)
          }
        }
      },
      new SmartObjectInfo
      {
        Name = "Diesel",
        TemplateName = "Motor",
        Properties =
        {
          new SmartObjectProperty
          {
            Name = "FrameName",
            Value = "Frame1",
            Type = typeof(string)
          },
        }
      },
      new SmartObjectInfo
      {
        Name = "Diesel 2",
        TemplateName = "Motor 2",
      },
      new SmartObjectInfo
      {
        Name = "Diesel 3",
        TemplateName = "Motor 3",
        Properties =
        {
          new SmartObjectProperty
          {
            Value = "Frame1",
            Type = typeof(string)
          },
        }
      },
      new SmartObjectInfo
      {
        Name = "Diesel 4",
        TemplateName = "Motor 4",
        Properties =
        {
          new SmartObjectProperty
          {
            Name = "FrameName 4",
            Type = typeof(string)
          },
        }
      },
      new SmartObjectInfo
      {
        Name = "Diesel 5",
        TemplateName = "Motor 5",
        Properties =
        {
          new SmartObjectProperty
          {
            Name = "FrameName",
            Value = "Frame1"
          },
        }
      },

      new SmartObjectInfo
      {
        Name = "Diesel 6",
        Properties =
        {
          new SmartObjectProperty
          {
            Name = "FrameName",
            Value = "Frame1",
            Type = typeof(string)
          },
        }
      },


    };

        #endregion

        private readonly Logger _logger = Logger.GetInstance;

        public MainWindow()
        {
            _logger = Logger.GetInstance;
            LoadSmartObjectInfoCommand = new RelayCommand(DoLoad, CanLoad);
            ExportCommand = new RelayCommand(DoExport, CanExport);

            InitializeComponent();
        }

        #region Properties bound to the GUI
        private bool _isLoading;
        public bool IsLoading
        {
            get => _isLoading;
            set
            {
                if (value == _isLoading)
                {
                    return;
                }

                _isLoading = value;
                OnPropertyChanged();
            }
        }

        private ObservableCollection<SmartObjectInfo> _smartObjects = new ObservableCollection<SmartObjectInfo>();
        public ObservableCollection<SmartObjectInfo> SmartObjects
        {
            get => _smartObjects;
            set
            {
                if (_smartObjects == value)
                {
                    return;
                }

                _smartObjects = value;
                OnPropertyChanged();
            }
        }

        private SmartObjectInfo _selectedSmartObject;
        public SmartObjectInfo SelectedSmartObject
        {
            get => _selectedSmartObject;
            set
            {
                if (_selectedSmartObject == value)
                {
                    return;
                }

                _selectedSmartObject = value;
                OnPropertyChanged();
            }
        }
        #endregion


        #region Commands
        /**********************************************************
         *  Load Command
         ***********************************************************/
        public ICommand LoadSmartObjectInfoCommand { get; }
        private void DoLoad(object parameter)
        {
            new Thread(() =>
            {

                IsLoading = true;

                Dispatcher.Invoke(() => SmartObjects.Clear());

                foreach (var info in _testItems)
                {
                    Dispatcher.Invoke(() => SmartObjects.Add(info));
                }
                
                IsLoading = false;
            }).Start();
        }

        private bool CanLoad(object parameter) => !_isLoading;

        /**********************************************************
         *  End Load Command
         ***********************************************************/

        /**********************************************************
         *  Begin Export Command
         ***********************************************************/

        public ICommand ExportCommand { get; }

        private void DoExport(object parameter)
        {
            FolderOperation folder = new FolderOperation();
            FileOperation fileOperation = new FileOperation();
            string path;
            path = folder.GetFolder();

            if (path != "")
            {
                new Thread(
                  () =>
                  {
                      Task.Run(() =>
                      {
                          fileOperation.CrerateCSVFile<SmartObjectInfo>(LoadList.Items, path, "SmartObject", "csv");
                      });
                      // -- Data handling part --
                  }).Start();
            }
        }

        private bool CanExport(object parameter)
        {
            return true;
        }
        /**********************************************************
         *  End Export Command
         ***********************************************************/

        #endregion


        #region INotifyPropertyChanged implementation
        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged([CallerMemberName] string propertyName = "")
          => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        #endregion
        /**********************************************************************
         * LoadList_SelectionChanged
         * object sender
         * SelectionChangedEventArgs e
         * ********************************************************************/
        private void LoadList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                ListBox loadlist = (ListBox)sender;
                SelectedSmartObject = loadlist.SelectedItem as SmartObjectInfo;
            }
            catch (System.Exception ex)
            {
                _logger.Log(ex.ToString());
            }
        }
    }
}