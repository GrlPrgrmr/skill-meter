using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace SkillMeter.ViewModels
{
    public class ViewModelBase : DependencyObject, INotifyPropertyChanged, IDisposable
    {

        public Session CurrentSession
        {

            get
            {
                return Session.SessionObj;
            }
        }


        #region INotifyPropertyChanged Members

        public event PropertyChangedEventHandler PropertyChanged;

        protected internal void OnPropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
                handler(this, new PropertyChangedEventArgs(propertyName));
        }

        protected void OnPropertyChanged<T>(Expression<Func<T>> propertyExpression)
        {
            var propertyName = ExtractPropertyName(propertyExpression);
            OnPropertyChanged(propertyName.ToString());
        }

        static string ExtractPropertyName<T>(Expression<Func<T>> propertyExpression)
        {
            string propertyName = "";

            try
            {
                if (propertyExpression == null)
                    throw new ArgumentNullException("propertyExpression");

                var memberExpression = propertyExpression.Body as MemberExpression;
                if (memberExpression == null)
                    throw new Exception();

                var property = memberExpression.Member as PropertyInfo;
                if (property == null)
                    throw new Exception();

                propertyName = property.Name.ToString();

            }
            catch (Exception ex)
            {
                throw ex;
            }

            return propertyName;
        }

        #endregion // INotifyPropertyChanged Members

        private bool isClosed = false;
        public bool IsClosed
        {
            get { return isClosed; }
            set
            {
                isClosed = value;
                OnPropertyChanged("IsClosed");
            }
        }

        // public IMessageboxService MsgBox = new MessageboxService();




        #region "IDisposable methods"

        private bool disposed = false;

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    // Shouold be overridden in derived class
                }
            }

            this.disposed = true;
        }

        #endregion "IDisposable methods"

    }
}
