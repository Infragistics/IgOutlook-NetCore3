using IgOutlook.Business.Core;
using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace IgOutlook.Business.Contacts
{
    public class Contact : BusinessBase, IDataErrorInfo
    {
        public Contact()
        {
            _errors = new Dictionary<string, string>();
        }

        private int _id;
        public int Id
        {
            get { return _id; }
            set { SetProperty(ref _id, value); }
        }

        private Uri _photoUri;
        public Uri PhotoUri
        {
            get { return _photoUri; }
            set { SetProperty(ref _photoUri, value); }
        }

        private string _firstName;
        public string FirstName
        {
            get { return _firstName; }
            set { SetProperty(ref _firstName, value); }
        }

        private string _middleName;
        public string MiddleName
        {
            get { return _middleName; }
            set { SetProperty(ref _middleName, value); }
        }

        private string _lastName;
        public string LastName
        {
            get { return _lastName; }
            set { SetProperty(ref _lastName, value); }
        }

        private string _email;
        public string Email
        {
            get { return _email; }
            set { SetProperty(ref _email, value); }
        }

        private string _jobTitle;
        public string JobTitle
        {
            get { return _jobTitle; }
            set { SetProperty(ref _jobTitle, value); }
        }

        private string _company;
        public string Company
        {
            get { return _company; }
            set { SetProperty(ref _company, value); }
        }

        private string _department;
        public string Department
        {
            get { return _department; }
            set { SetProperty(ref _department, value); }
        }

        private string _notes;
        public string Notes
        {
            get { return _notes; }
            set { SetProperty(ref _notes, value); }
        }

        private string _fileAs;
        public string FileAs
        {
            get { return _fileAs; }
            set { SetProperty(ref _fileAs, value); }
        }

        private string _displayAs;
        public string DisplayAs
        {
            get { return _displayAs; }
            set { SetProperty(ref _displayAs, value); }
        }

        private string _phoneBusiness;
        public string PhoneBusiness
        {
            get { return _phoneBusiness; }
            set { SetProperty(ref _phoneBusiness, value); }
        }

        private string _phoneHome;
        public string PhoneHome
        {
            get { return _phoneHome; }
            set { SetProperty(ref _phoneHome, value); }
        }

        private string _phoneMobile;
        public string PhoneMobile
        {
            get { return _phoneMobile; }
            set { SetProperty(ref _phoneMobile, value); }
        }

        private string _adressBusiness;
        public string AdressBusiness
        {
            get { return _adressBusiness; }
            set { SetProperty(ref _adressBusiness, value); }
        }

        #region IDataErrorInfo

        public string Error
        {
            get;
            private set;
        }

        public string this[string propertyName]
        {
            get
            {
                Error = string.Empty;

                switch (propertyName)
                {
                    //TODO: The rest of the properties need to be validated as well 
              //      case "Id": Error = string.IsNullOrEmpty(id) ? "Id is not valid" : null; break;
                    case "FirstName": Error = string.IsNullOrEmpty(_firstName) ? propertyName + "is not valid" : null; break;
                    case "MiddleName": Error = string.IsNullOrEmpty(_middleName) ? propertyName + "is not valid" : null; break;
                    case "LastName": Error = string.IsNullOrEmpty(_lastName) ? propertyName + "is not valid" : null; break;
                    case "Email": Error = string.IsNullOrEmpty(_email) ? propertyName + "is not valid" : null; break;
                    case "Company": Error = string.IsNullOrEmpty(_company) ? propertyName + "is not valid" : null; break;
                    case "FileAs": Error = string.IsNullOrEmpty(_fileAs) ? propertyName + "is not valid" : null; break;
                    default: break;
                }

                UpdateErrors(propertyName, Error);

                return Error;

            }
        }

        private Dictionary<string, string> _errors;

        private void UpdateErrors(string propertyName, string errorMsg)
        {
            if (_errors.ContainsKey(propertyName))
            {
                if (string.IsNullOrEmpty(errorMsg))
                    _errors.Remove(propertyName);
                else
                    _errors[propertyName] = errorMsg;
            }
            else
            {
                if (!string.IsNullOrEmpty(errorMsg))
                    _errors.Add(propertyName, errorMsg);
            }
        }

        public bool HasErrors()
        {
            return _errors.Count > 0;
        }

        #endregion //IDataErrorInfo
    }
}
