using System;
using System.ComponentModel;

namespace AsynchronyThreadingDemo.AsynchronousPatterns
{
    public  class EAPCompletedEventArgs : AsyncCompletedEventArgs
    {
        private string _result;

        public EAPCompletedEventArgs(string result, Exception ex, bool cancelled, object userState) 
            : base(ex, cancelled, userState)
        {
            _result = result;
        }

        public string Result
        {
            get
            {
                return _result;
            }
            set
            {
                _result = value;
            }
        }
    }
}
