using System.ComponentModel;
using GT2.SaveEditor.GTMode.License;

namespace GT2.SaveEditor.GUI
{
    public partial class LicenseTestViewModel : INotifyPropertyChanged
    {
        public string Name { get; set; }

        public LicenseTestData Data { get; set; }

        public LicenseTestResultEnum BestResult
        {
            get => Data.BestResult;
            set
            {
                Data.BestResult = value;
                OnPropertyChanged(nameof(BestResult));
            }
        }

        public LicenseTestRecord[] Records { get; set; }

        public event PropertyChangedEventHandler? PropertyChanged;

        public LicenseTestViewModel(string name, LicenseTestData data)
        {
            Name = name;
            Data = data;
            BestResult = Data.BestResult;
            Records = Data.Records;
        }

        protected virtual void OnPropertyChanged(string propertyName) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}