using DllUpdater.Models;
using DllUpdater.Properties;
using Livet;
using Livet.Commands;
using Livet.EventListeners;
using Livet.Messaging;
using Microsoft.WindowsAPICodePack.Dialogs;
using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Reflection;

namespace DllUpdater.ViewModels
{
    public class MainViewModel : ViewModel
    {
        public MainViewModel()
        {
            this.Settings = new Settings();
            this.Dlls = new Dlls(this.Settings);

            // バージョン情報の設定
            var ver = FileVersionInfo.GetVersionInfo(Assembly.GetExecutingAssembly().Location);
            this.ApplicationName = ver.ProductName;
            this.ApplicationVersion = string.Format("{0}.{1}.{2}", ver.ProductMajorPart, ver.ProductMinorPart, ver.ProductBuildPart);
            this.ApplicationCopyright = ver.LegalCopyright;
            this.UrlGitHub = Constants.UrlGitHub;
        }
        public void Initialize()
        {
            // リスナー設定
            var dllsListener = new PropertyChangedEventListener(this.Dlls, (sender, e) => {
                if (e.PropertyName == "IsRunningVersionCheck" ||
                    e.PropertyName == "IsRunningSearch" ||
                    e.PropertyName == "IsRunningReplace"||
                    e.PropertyName == "IsRunningDownload")
                {
                    VersionCheckCommand.RaiseCanExecuteChanged();
                    SearchDllCommand.RaiseCanExecuteChanged();
                    AllCheckCommand.RaiseCanExecuteChanged();
                    ReplaceDllCommand.RaiseCanExecuteChanged();
                    DownloadDllCommand.RaiseCanExecuteChanged();
                }
                RaisePropertyChanged(e.PropertyName); 
            });
            CompositeDisposable.Add(dllsListener);
            // スタートアップ
            this.Dlls.ExecuteStartup();
        }
        public void Shutdown()
        {
            this.Settings.Save();
        }

        #region プロパティ
        #region Settings変更通知プロパティ
        private Settings _Settings;
        public Settings Settings
        {
            get
            { return _Settings; }
            set
            { 
                if (_Settings == value)
                    return;
                _Settings = value;
                RaisePropertyChanged("Settings");
            }
        }
        #endregion
        #region TargetPath変更通知プロパティ
        private PathInfo _TargetPath;
        public PathInfo TargetPath
        {
            get
            { return _TargetPath; }
            set
            { 
                if (_TargetPath == value)
                    return;
                _TargetPath = value;
                RaisePropertyChanged("TargetPath");
            }
        }
        #endregion
        #region IgnorePath変更通知プロパティ
        private PathInfo _IgnorePath;
        public PathInfo IgnorePath
        {
            get
            { return _IgnorePath; }
            set
            { 
                if (_IgnorePath == value)
                    return;
                _IgnorePath = value;
                RaisePropertyChanged("IgnorePath");
            }
        }
        #endregion
        #region Dlls変更通知プロパティ
        private Dlls _Dlls;
        public Dlls Dlls
        {
            get
            { return _Dlls; }
            set
            { 
                if (_Dlls == value)
                    return;
                _Dlls = value;
                RaisePropertyChanged("Dlls");
            }
        }
        #endregion
        #region ApplicationName変更通知プロパティ
        private string _ApplicationName;
        public string ApplicationName
        {
            get
            { return _ApplicationName; }
            set
            {
                if (_ApplicationName == value)
                    return;
                _ApplicationName = value;
                RaisePropertyChanged("ApplicationName");
            }
        }
        #endregion
        #region ApplicationVersion変更通知プロパティ
        private string _ApplicationVersion;
        public string ApplicationVersion
        {
            get
            { return _ApplicationVersion; }
            set
            { 
                if (_ApplicationVersion == value)
                    return;
                _ApplicationVersion = value;
                RaisePropertyChanged("ApplicationVersion");
            }
        }
        #endregion
        #region ApplicationCopyright変更通知プロパティ
        private string _ApplicationCopyright;
        public string ApplicationCopyright
        {
            get
            { return _ApplicationCopyright; }
            set
            { 
                if (_ApplicationCopyright == value)
                    return;
                _ApplicationCopyright = value;
                RaisePropertyChanged("ApplicationCopyright");
            }
        }
        #endregion
        #region UrlGitHub変更通知プロパティ
        private string _UrlGitHub;
        public string UrlGitHub
        {
            get
            { return _UrlGitHub; }
            set
            { 
                if (_UrlGitHub == value)
                    return;
                _UrlGitHub = value;
                RaisePropertyChanged("UrlGitHub");
            }
        }
        #endregion
        #endregion

        #region コマンド
        #region VersionCheckCommand
        private ViewModelCommand _VersionCheckCommand;
        public ViewModelCommand VersionCheckCommand
        {
            get
            {
                if (_VersionCheckCommand == null)
                {
                    _VersionCheckCommand = new ViewModelCommand(VersionCheck, CanVersionCheck);
                }
                return _VersionCheckCommand;
            }
        }
        public bool CanVersionCheck()
        {
            return !this.Dlls.IsRunningSearch &&
                   !this.Dlls.IsRunningReplace &&
                   !this.Dlls.IsRunningDownload;
        }
        public void VersionCheck()
        {
            this.Dlls.ExecuteVersionCheck();
        }
        #endregion
        #region DownloadDllCommand
        private ViewModelCommand _DownloadDllCommand;
        public ViewModelCommand DownloadDllCommand
        {
            get
            {
                if (_DownloadDllCommand == null)
                {
                    _DownloadDllCommand = new ViewModelCommand(DownloadDll, CanDownloadDll);
                }
                return _DownloadDllCommand;
            }
        }
        public bool CanDownloadDll()
        {
            return !this.Dlls.IsRunningVersionCheck &&
                   !this.Dlls.IsRunningSearch &&
                   !this.Dlls.IsRunningReplace;
        }
        public void DownloadDll()
        {
            this.Dlls.ExecuteDownload();
        }
        #endregion
        #region SearchDllCommand
        private ViewModelCommand _SearchDllCommand;
        public ViewModelCommand SearchDllCommand
        {
            get
            {
                if (_SearchDllCommand == null)
                {
                    _SearchDllCommand = new ViewModelCommand(SearchDll, CanSearchDll);
                }
                return _SearchDllCommand;
            }
        }
        public bool CanSearchDll()
        {
            return !this.Dlls.IsRunningVersionCheck &&
                   !this.Dlls.IsRunningReplace &&
                   !this.Dlls.IsRunningDownload;
        }
        public void SearchDll()
        {
            if (!this.Dlls.SearchCheck())
            {
                Messenger.Raise(new InformationMessage(Resources.MsgSpecifyTargetPath, Resources.MsgWarning,System.Windows.MessageBoxImage.Warning, "Information"));
            }
            this.Dlls.ExecuteSearch();
        }
        #endregion
        #region AllCheckCommand
        private ViewModelCommand _AllCheckCommand;
        public ViewModelCommand AllCheckCommand
        {
            get
            {
                if (_AllCheckCommand == null)
                {
                    _AllCheckCommand = new ViewModelCommand(AllCheck, CanAllCheck);
                }
                return _AllCheckCommand;
            }
        }
        public bool CanAllCheck()
        {
            return !this.Dlls.IsRunningVersionCheck && 
                   !this.Dlls.IsRunningSearch &&
                   !this.Dlls.IsRunningReplace &&
                   !this.Dlls.IsRunningDownload;
        }
        public void AllCheck()
        {
            this.Dlls.AllCheck();
        }
        #endregion
        #region ReplaceDllCommand
        private ViewModelCommand _ReplaceDllCommand;
        public ViewModelCommand ReplaceDllCommand
        {
            get
            {
                if (_ReplaceDllCommand == null)
                {
                    _ReplaceDllCommand = new ViewModelCommand(ReplaceDll, CanReplaceDll);
                }
                return _ReplaceDllCommand;
            }
        }
        public bool CanReplaceDll()
        {
            return !this.Dlls.IsRunningVersionCheck && 
                   !this.Dlls.IsRunningSearch &&
                   !this.Dlls.IsRunningDownload;
        }
        public void ReplaceDll()
        {
            this.Dlls.ExecuteReplace();    
        }
        #endregion
        #region AddTargetPathCommand
        private ListenerCommand<object> _AddTargetPathCommand;
        public ListenerCommand<object> AddTargetPathCommand
        {
            get
            {
                if (_AddTargetPathCommand == null)
                {
                    _AddTargetPathCommand = new ListenerCommand<object>(AddTargetPath);
                }
                return _AddTargetPathCommand;
            }
        }
        public void AddTargetPath(object parameter)
        {
            int index = Settings.TargetPathList.IndexOf(parameter as PathInfo);
            AddPath(Settings.TargetPathList, index);
        }
        #endregion
        #region DeleteTargetPathCommand
        private ListenerCommand<object> _DeleteTargetPathCommand;
        public ListenerCommand<object> DeleteTargetPathCommand
        {
            get
            {
                if (_DeleteTargetPathCommand == null)
                {
                    _DeleteTargetPathCommand = new ListenerCommand<object>(DeleteTargetPath);
                }
                return _DeleteTargetPathCommand;
            }
        }
        public void DeleteTargetPath(object parameter)
        {
            if (Settings.TargetPathList.Count > 1)
            {
                int index = Settings.TargetPathList.IndexOf(parameter as PathInfo);
                DeletePath(Settings.TargetPathList, index);
            }
        }
        #endregion
        #region EditTargetPathCommand
        private ViewModelCommand _EditTargetPathCommand;
        public ViewModelCommand EditTargetPathCommand
        {
            get
            {
                if (_EditTargetPathCommand == null)
                {
                    _EditTargetPathCommand = new ViewModelCommand(EditTargetPath);
                }
                return _EditTargetPathCommand;
            }
        }
        public void EditTargetPath()
        {
            SelectPath(this.TargetPath);
        }
        #endregion
        #region AddIgnorePathCommand
        private ListenerCommand<object> _AddIgnorePathCommand;
        public ListenerCommand<object> AddIgnorePathCommand
        {
            get
            {
                if (_AddIgnorePathCommand == null)
                {
                    _AddIgnorePathCommand = new ListenerCommand<object>(AddIgnorePath);
                }
                return _AddIgnorePathCommand;
            }
        }
        public void AddIgnorePath(object parameter)
        {
            int index = Settings.IgnorePathList.IndexOf(parameter as PathInfo);
            AddPath(Settings.IgnorePathList, index);
        }
        #endregion
        #region DeleteIgnorePathCommand
        private ListenerCommand<object> _DeleteIgnorePathCommand;
        public ListenerCommand<object> DeleteIgnorePathCommand
        {
            get
            {
                if (_DeleteIgnorePathCommand == null)
                {
                    _DeleteIgnorePathCommand = new ListenerCommand<object>(DeleteIgnorePath);
                }
                return _DeleteIgnorePathCommand;
            }
        }
        public void DeleteIgnorePath(object parameter)
        {
            if (Settings.IgnorePathList.Count > 1)
            {
                int index = Settings.IgnorePathList.IndexOf(parameter as PathInfo);
                DeletePath(Settings.IgnorePathList, index);
            }
        }
        #endregion
        #region EditIgnorePathCommand
        private ViewModelCommand _EditIgnorePathCommand;
        public ViewModelCommand EditIgnorePathCommand
        {
            get
            {
                if (_EditIgnorePathCommand == null)
                {
                    _EditIgnorePathCommand = new ViewModelCommand(EditIgnorePath);
                }
                return _EditIgnorePathCommand;
            }
        }
        public void EditIgnorePath()
        {
            SelectPath(this.IgnorePath);
        }
        #endregion
        #region GitHubCommand
        private ViewModelCommand _GitHubCommand;
        public ViewModelCommand GitHubCommand
        {
            get
            {
                if (_GitHubCommand == null)
                {
                    _GitHubCommand = new ViewModelCommand(GitHub);
                }
                return _GitHubCommand;
            }
        }
        public void GitHub()
        {
            System.Diagnostics.Process.Start(Constants.UrlGitHub);
        }
        #endregion
        #endregion

        #region メソッド
        private void AddPath(ObservableCollection<PathInfo> iList,int iIndex)
        {
            if (iIndex > -1 && iIndex < iList.Count)
            {
                iList.Insert(iIndex + 1, new PathInfo(true, string.Empty, true, true));
            }
        }
        private void DeletePath(ObservableCollection<PathInfo> iList, int iIndex)
        {
            if (iIndex > -1 && iIndex < iList.Count)
            {
                iList.RemoveAt(iIndex);
            }
        }
        private void SelectPath(PathInfo iPathInfo)
        {
            if (iPathInfo == null) return;
            var dialog = new CommonOpenFileDialog();
            dialog.IsFolderPicker = true;
            dialog.EnsureReadOnly = false;
            dialog.AllowNonFileSystemItems = false;
            dialog.InitialDirectory = iPathInfo.Path;
            var Result = dialog.ShowDialog();
            if (Result == CommonFileDialogResult.Ok)
            {
                iPathInfo.Path = dialog.FileName;
            }
        }
        #endregion
    }
}
