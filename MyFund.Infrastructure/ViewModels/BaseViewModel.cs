using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using Prism.Events;
using Prism.Mvvm;

namespace MyFund.Infrastructure.ViewModels
{
    public abstract class BaseViewModel : BindableBase, INotifyDataErrorInfo
    {
        private Dictionary<String, List<String>> _errors = new Dictionary<string, List<string>>();
        
        protected BaseViewModel(IEventAggregator eventAggregator)
        {
            EventAggregator = eventAggregator;
        }

        public event EventHandler<DataErrorsChangedEventArgs> ErrorsChanged;
        public IEventAggregator EventAggregator { get; }

        public bool HasErrors
        {
            get { return _errors.Count > 0; }
        }

        public IEnumerable<string> Errors
        {
            get
            {
                if (!HasErrors)
                {
                    return null;
                }

                return _errors.Values.SelectMany(l => l);
            }
        }

        public IEnumerable GetErrors(string propertyName)
        {
            if (String.IsNullOrEmpty(propertyName) ||
                !_errors.ContainsKey(propertyName)) return null;
            return _errors[propertyName];
        }

        public void AddError<T>(Expression<Func<T>> propertyExpression, string error, bool isWarning = true)
        {
            AddError(GetPropertyName(propertyExpression), error, isWarning);
        }

        public void RemoveError<T>(Expression<Func<T>> propertyExpression, string error)
        {
            RemoveError(GetPropertyName(propertyExpression), error);
        }

        private string GetPropertyName<T>(Expression<Func<T>> propertyExpression)
        {
            var memberExpression = (MemberExpression)propertyExpression.Body;
            return memberExpression.Member.Name;
        }

        private void AddError(string propertyName, string error, bool isWarning)
        {
            if (!_errors.ContainsKey(propertyName))
                _errors[propertyName] = new List<string>();

            if (!_errors[propertyName].Contains(error))
            {
                if (isWarning) _errors[propertyName].Add(error);
                else _errors[propertyName].Insert(0, error);
                RaiseErrorsChanged(propertyName);

                OnPropertyChanged(() => Errors);
            }
        }

        private void RemoveError(string propertyName, string error)
        {
            if (_errors.ContainsKey(propertyName) &&
                _errors[propertyName].Contains(error))
            {
                _errors[propertyName].Remove(error);
                if (_errors[propertyName].Count == 0) _errors.Remove(propertyName);
                RaiseErrorsChanged(propertyName);

                OnPropertyChanged(() => Errors);
            }
        }

        private void RaiseErrorsChanged(string propertyName)
        {
            if (ErrorsChanged != null)
                ErrorsChanged(this, new DataErrorsChangedEventArgs(propertyName));
        }
    }
}
