using Prism.Mvvm;
using System.ComponentModel;

namespace IgOutlook.Core
{
    public class ViewModelBase : BindableBase
    {
        private string _title;
        public string Title
        {
            get { return _title; }
            set { SetProperty(ref _title, value); }
        }

        private string _iconSource;
        public string IconSource
        {
            get { return _iconSource; }
            set { SetProperty(ref _iconSource, value); }
        }

        public void UpdateTitleOnPropertyChanged(INotifyPropertyChanged source, string propertyName, string concatedString = "", string nullValue = "")
        {
            source.PropertyChanged += (s, a) =>
            {
                if (a.PropertyName == propertyName)
                {
                    var value = (string)source.GetType().GetProperty(propertyName).GetValue(source);

                    if (string.IsNullOrEmpty(value))
                        Title = nullValue + concatedString;
                    else
                        Title = value + concatedString;
                }
            };
        }

        public void HookOnPropertyChanged(INotifyPropertyChanged source, string propertyName, System.Action action)
        {
            source.PropertyChanged += (s, a) =>
            {
                if (a.PropertyName == propertyName)
                {
                    action.Invoke();
                }
            };
        }

        //public void HookOnPropertyChanged(INotifyPropertyChanged source, System.Action action)
        //{
        //    source.PropertyChanged += (s, a) =>
        //    {
        //        action.Invoke();
        //    };
        //}

        public void RefreshTitle()
        {
            RaisePropertyChanged("Title");
        }
    }
}
