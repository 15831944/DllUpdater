using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Livet;

namespace DllUpdater.Models
{
    public class PathInfo : NotificationObject
    {
        public PathInfo(bool iEnable, string iPath, bool iFFACE, bool iFFACETools, bool iEliteAPI, bool iEliteMMOAPI)
        {
            this.Enable = iEnable;
            this.Path = iPath;
            this.FFACE = iFFACE;
            this.FFACETools = iFFACETools;
            this.EliteAPI = iEliteAPI;
            this.EliteMMOAPI = iEliteMMOAPI;
        }

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
        #region Path変更通知プロパティ
        private string _Path;
        public string Path
        {
            get
            { return _Path; }
            set
            { 
                if (_Path == value)
                    return;
                _Path = value;
                RaisePropertyChanged("Path");
            }
        }
        #endregion
        #region FFACE変更通知プロパティ
        private bool _FFACE;
        public bool FFACE
        {
            get
            { return _FFACE; }
            set
            { 
                if (_FFACE == value)
                    return;
                _FFACE = value;
                RaisePropertyChanged("FFACE");
            }
        }
        #endregion
        #region FFACETools変更通知プロパティ
        private bool _FFACETools;
        public bool FFACETools
        {
            get
            { return _FFACETools; }
            set
            { 
                if (_FFACETools == value)
                    return;
                _FFACETools = value;
                RaisePropertyChanged("FFACETools");
            }
        }
        #endregion
        #region EliteAPI変更通知プロパティ
        private bool _EliteAPI;
        public bool EliteAPI
        {
            get
            { return _EliteAPI; }
            set
            { 
                if (_EliteAPI == value)
                    return;
                _EliteAPI = value;
                RaisePropertyChanged("EliteAPI");
            }
        }
        #endregion
        #region EliteMMOAPI変更通知プロパティ
        private bool _EliteMMOAPI;
        public bool EliteMMOAPI
        {
            get
            { return _EliteMMOAPI; }
            set
            { 
                if (_EliteMMOAPI == value)
                    return;
                _EliteMMOAPI = value;
                RaisePropertyChanged("EliteMMOAPI");
            }
        }
        #endregion
    }
}
