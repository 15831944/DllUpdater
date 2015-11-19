using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Livet;

namespace DllUpdater.Models
{
    public class Proxy : NotificationObject
    {

        #region Enable変更通知プロパティ
        private bool _Enable;
        public bool Enable
        {
            get
            { return _Enable; }
            set
            { 
                if (_Enable == value)
                    return;
                _Enable = value;
                RaisePropertyChanged("Enable");
            }
        }
        #endregion

        #region Server変更通知プロパティ
        private string _Server;
        public string Server
        {
            get
            { return _Server; }
            set
            { 
                if (_Server == value)
                    return;
                _Server = value;
                RaisePropertyChanged("Server");
            }
        }
        #endregion

        #region Port変更通知プロパティ
        private int _Port;
        public int Port
        {
            get
            { return _Port; }
            set
            { 
                if (_Port == value)
                    return;
                _Port = value;
                RaisePropertyChanged("Port");
            }
        }
        #endregion

    }
}
