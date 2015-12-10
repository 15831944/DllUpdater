using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Media;
using System.Net;
using System.Text;
using System.Threading;
using System.Xml;
using DllUpdater.Properties;
using HtmlAgilityPack;
using Livet;

namespace DllUpdater.Models
{
    public class Dlls : NotificationObject
    {
        private Settings settings;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="iSettings">Settings</param>
        public Dlls(Settings iSettings)
        {
            settings = iSettings;

            this.IsRunningVersionCheck = false;
            this.IsRunningSearch = false;
            this.IsRunningReplace = false;
            this.IsRunningReplace = false;
            this.StatusMessage = string.Empty;
            RefreshDllVersion();
            this.DllList = new DispatcherCollection<SearchPathInfo>(DispatcherHelper.UIDispatcher);
        }

        #region プロパティ
        #region IsRunningVersionCheck変更通知プロパティ
        private bool _IsRunningVersionCheck;
        public bool IsRunningVersionCheck
        {
            get
            { return _IsRunningVersionCheck; }
            set
            { 
                if (_IsRunningVersionCheck == value)
                    return;
                _IsRunningVersionCheck = value;
                RaisePropertyChanged("IsRunningVersionCheck");
            }
        }
        #endregion
        #region IsRunningSearch変更通知プロパティ
        private bool _IsRunningSearch;
        public bool IsRunningSearch
        {
            get
            { return _IsRunningSearch; }
            private set
            {
                if (_IsRunningSearch == value)
                    return;
                _IsRunningSearch = value;
                RaisePropertyChanged("IsRunningSearch");
            }
        }
        #endregion
        #region IsRunningReplace変更通知プロパティ
        private bool _IsRunningReplace;
        public bool IsRunningReplace
        {
            get
            { return _IsRunningReplace; }
            private set
            { 
                if (_IsRunningReplace == value)
                    return;
                _IsRunningReplace = value;
                RaisePropertyChanged("IsRunningReplace");
            }
        }
        #endregion
        #region IsRunningDownload変更通知プロパティ
        private bool _IsRunningDownload;
        public bool IsRunningDownload
        {
            get
            { return _IsRunningDownload; }
            private set
            { 
                if (_IsRunningDownload == value)
                    return;
                _IsRunningDownload = value;
                RaisePropertyChanged("IsRunningDownload");
            }
        }
        #endregion

        #region StatusMessage変更通知プロパティ
        private string _StatusMessage;
        public string StatusMessage
        {
            get
            { return _StatusMessage; }
            set
            { 
                if (_StatusMessage == value)
                    return;
                _StatusMessage = value;
                RaisePropertyChanged("StatusMessage");
            }
        }
        #endregion

        #region VersionFFACE変更通知プロパティ
        private string _VersionFFACE;
        public string VersionFFACE
        {
            get
            { return _VersionFFACE; }
            set
            { 
                if (_VersionFFACE == value)
                    return;
                _VersionFFACE = value;
                RaisePropertyChanged("VersionFFACE");
            }
        }
        #endregion
        #region VersionFFACETools変更通知プロパティ
        private string _VersionFFACETools;
        public string VersionFFACETools
        {
            get
            { return _VersionFFACETools; }
            set
            { 
                if (_VersionFFACETools == value)
                    return;
                _VersionFFACETools = value;
                RaisePropertyChanged("VersionFFACETools");
            }
        }
        #endregion
        #region VersionEliteAPI変更通知プロパティ
        private string _VersionEliteAPI;
        public string VersionEliteAPI
        {
            get
            { return _VersionEliteAPI; }
            set
            { 
                if (_VersionEliteAPI == value)
                    return;
                _VersionEliteAPI = value;
                RaisePropertyChanged("VersionEliteAPI");
            }
        }
        #endregion
        #region VersionEliteMMOAPI変更通知プロパティ
        private string _VersionEliteMMOAPI;
        public string VersionEliteMMOAPI
        {
            get
            { return _VersionEliteMMOAPI; }
            set
            { 
                if (_VersionEliteMMOAPI == value)
                    return;
                _VersionEliteMMOAPI = value;
                RaisePropertyChanged("VersionEliteMMOAPI");
            }
        }
        #endregion
        #region DllList変更通知プロパティ
        private DispatcherCollection<SearchPathInfo> _DllList;
        public DispatcherCollection<SearchPathInfo> DllList
        {
            get
            { return _DllList; }
            set
            { 
                if (DllList == value)
                    return;
                _DllList = value;
                RaisePropertyChanged("DllList");
            }
        }
        #endregion
        #endregion

        #region メソッド
        /// <summary>
        /// バージョンチェック
        /// </summary>
        public void ExecuteVersionCheck()
        {
            if (!this.IsRunningVersionCheck)
            {
                Thread thread = new Thread(ExecuteVersionCheckThread) { IsBackground = true };
                thread.Start();
            }
            else
            {
                this.IsRunningVersionCheck = false;
                return;
            }
        }
        /// <summary>
        /// バージョンチェック メインスレッド
        /// </summary>
        private void ExecuteVersionCheckThread()
        {
            try
            {
                this.IsRunningVersionCheck = true;
                List<DllType> needVersionUp = new List<DllType>();
                foreach (KeyValuePair<DllType, DllTypeInfo> pair in settings.DllTypeInfoList)
                {
                    if (pair.Value.Enable)
                    {
                        this.StatusMessage = string.Format(Resources.MsgNowCheckingVersion, pair.Key);
                        string xPathData = string.Empty;
                        bool ret = MasterVersionCheck(pair.Key, out xPathData);
                        if (ret) needVersionUp.Add(pair.Key);
                    }
                }
                if (needVersionUp.Count > 0)
                {
                    this.StatusMessage = string.Format(Resources.MsgRequiredVersion, string.Join(",", needVersionUp));
                }
                else
                {
                    this.StatusMessage = Resources.MsgVersionupNotNecessary;
                }
                SystemSounds.Asterisk.Play();
            }
            catch (Exception e)
            {
                this.StatusMessage = string.Format(Resources.MsgErrorVersionCheck, e.Message);
                Console.WriteLine(e.InnerException.Message);
                Console.WriteLine(e.InnerException.StackTrace);
                SystemSounds.Hand.Play();
            }
            finally
            {
                this.IsRunningVersionCheck = false;
            }
        }

        /// <summary>
        /// DLLのダウンロード
        /// </summary>
        public void ExecuteDownload()
        {
            if (!this.IsRunningDownload)
            {
                Thread thread = new Thread(ExecuteDownloadThread) { IsBackground = true };
                thread.Start();
            }
            else
            {
                this.IsRunningDownload = false;
                return;
            }
        }
        /// <summary>
        /// DLLのダウンロードスレッド
        /// </summary>
        private void ExecuteDownloadThread()
        {
            try
            {
                this.IsRunningDownload = true;
                List<DllType> finishVersionUp = new List<DllType>();
                foreach (KeyValuePair<DllType, DllTypeInfo> pair in settings.DllTypeInfoList)
                {
                    if (pair.Value.Enable)
                    {
                        this.StatusMessage = string.Format(Resources.MsgDownloading, pair.Key);
                        string xPathData = string.Empty;
                        if (MasterVersionCheck(pair.Key, out xPathData))
                        {
                            if (DownloadDll(pair.Key))
                            {
                                pair.Value.XPathData = xPathData;
                                finishVersionUp.Add(pair.Key);
                            }
                        }
                    }
                }
                if (finishVersionUp.Count > 0)
                {
                    this.StatusMessage = string.Format(Resources.MsgDownloadCompleted, string.Join(",", finishVersionUp));
                }
                else
                {
                    this.StatusMessage = Resources.MsgDownloadNotNecessary;
                }
                SystemSounds.Asterisk.Play();
            }
            catch (Exception e)
            {
                this.StatusMessage = string.Format(Resources.MsgErrorDownload, e.Message);
                Console.WriteLine(e.InnerException.Message);
                Console.WriteLine(e.InnerException.StackTrace);
                SystemSounds.Hand.Play();
            }
            finally
            {
                this.IsRunningDownload = false;
            }
        }

        /// <summary>
        /// DLLの検索実行
        /// </summary>
        public void ExecuteSearch()
        {
            if (!this.IsRunningSearch)
            {
                Thread thread = new Thread(ExecuteSearchThread) { IsBackground = true };
                thread.Start();
            }
            else
            {
                this.IsRunningSearch = false;
                return;
            }
        }
        /// <summary>
        /// DLLの検索メインスレッド
        /// </summary>
        private void ExecuteSearchThread()
        {
            try
            {
                this.IsRunningSearch = true;

                this.DllList.Clear();
                foreach (var pathInfo in settings.TargetPathList)
                {
                    if (!this.IsRunningSearch) return;
                    if (pathInfo.Enable)
                    {
                        SearchDll(pathInfo.Path);
                    }
                }
            }
            catch (Exception e)
            {
                this.StatusMessage = string.Format(Resources.MsgErrorSearch, e.Message);
                string stackTrace, message;
                if (e.InnerException == null)
                {
                    message = e.Message;
                    stackTrace = e.StackTrace;
                }
                else
                {
                    message = e.InnerException.Message;
                    stackTrace = e.InnerException.StackTrace;
                }
                Console.WriteLine(message);
                Console.WriteLine(stackTrace);
                SystemSounds.Hand.Play();
            }
            finally
            {
                if (this.DllList.Count > 0)
                {
                    this.StatusMessage = string.Format(Resources.MsgFoundDll, this.DllList.Count);
                    SystemSounds.Asterisk.Play();
                }
                else
                {
                    this.StatusMessage = Resources.MsgDllNotFound;
                    SystemSounds.Beep.Play();
                }
                this.IsRunningSearch = false;
            }
        }

        /// <summary>
        /// DllListを全てチェック/解除
        /// </summary>
        public void AllCheck()
        {
            bool check = false;
            bool uncheck = false;
            foreach (var v in this.DllList)
            {
                if (!check && v.Enable) check = true;
                if (!uncheck && !v.Enable) uncheck = true;
            }

            bool enable = false;
            if (check && !uncheck) enable = false;
            else if (!check && uncheck) enable = true;
            else if (check && uncheck) enable = true;
            for (int i = 0; i < this.DllList.Count; i++)
            {
                this.DllList[i].Enable = enable;
            }
        }

        /// <summary>
        /// DLLを置換
        /// </summary>
        public void ExecuteReplace()
        {
            if (!this.IsRunningReplace)
            {
                Thread thread = new Thread(ExecuteReplaceThread) { IsBackground = true };
                thread.Start();
            }
            else
            {
                this.IsRunningReplace = false;
                return;
            }
        }
        /// <summary>
        /// DLLを置換メインスレッド
        /// </summary>
        private void ExecuteReplaceThread()
        {
            try
            {
                this.IsRunningReplace = true;
                if (!IsReplaceOK())
                {
                    this.StatusMessage = Resources.MsgExecuteAfterDownload;
                    SystemSounds.Hand.Play();
                    return;
                }

                int count = 0;
                foreach (var target in this.DllList)
                {
                    if (!this.IsRunningReplace) return;
                    if (!target.Enable) continue;
                    //コピー元のファイルパス決定
                    DllType dllType = DllTypeExt.GetDllType(target.Path);
                    if (dllType == DllType.Nothing) continue;
                    string sourcePath = GetSourcePath(dllType);
                    //ファイルコピー
                    if (target.Enable)
                    {
                        Console.WriteLine("処理中 {0} {1}", sourcePath, target.Path);
                        this.StatusMessage = string.Format(Resources.MsgReplace, target.Path);
                        if (!CopyDll(sourcePath, target.Path))
                        {
                            this.StatusMessage = string.Format(Resources.MsgErrorReplace, target.Path);
                            break;
                        }
                        count++;
                    }
                }
                this.StatusMessage = string.Format(Resources.MsgReplaced, count);
                SystemSounds.Asterisk.Play();
            }
            catch (Exception e)
            {
                this.StatusMessage = string.Format(Resources.MsgError, e.Message);
                Console.WriteLine(e.Message);
                Console.WriteLine(e.StackTrace);
                SystemSounds.Hand.Play();
            }
            finally
            {
                this.IsRunningReplace = false;
            }

        }

        /// <summary>
        /// バージョンチェック
        /// </summary>
        public void ExecuteStartup()
        {
            Thread thread = new Thread(ExecuteStartupThread) { IsBackground = true };
            thread.Start();
        }
        /// <summary>
        /// バージョンチェック メインスレッド
        /// </summary>
        private void ExecuteStartupThread()
        {
            if (settings.CheckNewDllOnStartup)
            {
                ExecuteDownloadThread();
            }
            if (settings.FindDllOnStartup)
            {
                ExecuteSearchThread();
            }
        }

        /// <summary>
        /// DLLの検索（再起呼び出し）
        /// </summary>
        /// <param name="iFullPath">検索パス</param>
        private void SearchDll(string iPath)
        {
            if (!Directory.Exists(iPath)) return;
            if (IsIgnorePath(iPath)) return;
            this.StatusMessage = string.Format("検索中 ： {0}", iPath);
            string[] files = Directory.GetFiles(iPath, "*.dll", SearchOption.TopDirectoryOnly);
            foreach (var fullpath in files)
            {
                if (!this.IsRunningSearch) return;
                if (IsIgnoreFile(fullpath)) continue;

                bool enable = IsVersionAbobe(GetSourcePath(DllTypeExt.GetDllType(fullpath)), fullpath);
                string version = GetDllVersion(fullpath);
                DllList.Add(new SearchPathInfo(
                    enable,
                    fullpath,
                    version));
            }
            string[] directries = Directory.GetDirectories(iPath, "*", SearchOption.TopDirectoryOnly);
            foreach (var directory in directries)
            {
                if (!this.IsRunningSearch) return;
                SearchDll(directory);
            }
        }
        /// <summary>
        /// 除外パスか判定
        /// </summary>
        /// <param name="iFullPath">パス</param>
        /// <returns>除外対象パスの場合True</returns>
        private bool IsIgnorePath(string iPath)
        {
            foreach (var v in settings.IgnorePathList)
            {
                if (v.Enable && v.Path.Length > 0 && v.FFACE && v.FFACETools && v.EliteAPI && v.EliteMMOAPI)
                {
                    if (iPath.ToLower().IndexOf(v.Path.ToLower()) >= 0) return true;
                }
            }
            return false;
        }
        /// <summary>
        /// 除外ファイルか判定
        /// </summary>
        /// <param name="iFullPath">フルパス</param>
        /// <returns>除外対象ファイルの場合True</returns>
        private bool IsIgnoreFile(string iFullPath)
        {
            string filename = Path.GetFileName(iFullPath).ToLower();
            string dirname = Path.GetDirectoryName(iFullPath).ToLower();
            // DllTypeでチェック
            DllType dllType = DllTypeExt.GetDllType(iFullPath);
            if (dllType == DllType.Nothing) return true;
            if (!settings.DllTypeInfoList[dllType].Enable) return true;

            // IgnorePathでチェック
            foreach (var ignorePath in settings.IgnorePathList)
            {
                if (!ignorePath.Enable || ignorePath.Path.Length == 0) continue;
                if (dirname.IndexOf(ignorePath.Path.ToLower()) >= 0)
                {
                    if ((dllType == DllType.FFACE && ignorePath.FFACE) ||
                        (dllType == DllType.FFACETools && ignorePath.FFACETools) ||
                        (dllType == DllType.EliteAPI && ignorePath.EliteAPI) ||
                        (dllType == DllType.EliteMMOAPI && ignorePath.EliteMMOAPI))
                    {
                        return true;
                    }
                }
            }
            return false;
        }
        /// <summary>
        /// DLL検索前チェック
        /// </summary>
        /// <returns>検索実行可能な場合True</returns>
        public bool SearchCheck()
        {
            foreach (var v in settings.TargetPathList)
            {
                if (v.Path.Length > 0) return true;
            }
            return false;
        }
        /// <summary>
        /// ファイルからバージョン取得
        /// </summary>
        /// <param name="iFullPath">ファイル名</param>
        /// <returns>バージョン</returns>
        private string GetDllVersion(string iFullPath)
        {
            if (!File.Exists(iFullPath)) return Constants.DefaultVersion;
            try
            {
                FileVersionInfo fvi = FileVersionInfo.GetVersionInfo(iFullPath);
                return fvi.FileVersion;
            }
            catch
            {
                return Constants.DefaultVersion;
            }
        }
        /// <summary>
        /// DLLをコピーする
        /// </summary>
        /// <param name="iSourceFullPath">コピー元</param>
        /// <param name="iDestFullPath">コピー先</param>
        /// <returns>成功した場合True</returns>
        public bool CopyDll(string iSourceFullPath, string iDestFullPath)
        {
            try
            {
                // コピー先のバージョンが低い場合だけ置換
                if (IsVersionAbobe(iSourceFullPath, iDestFullPath))
                {
                    Console.WriteLine("{0}を{1}にコピー", iSourceFullPath, iDestFullPath);
                    File.Copy(iSourceFullPath, iDestFullPath, true);
                }
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine("コピーエラー {0}→{1}", iSourceFullPath, iDestFullPath);
                Console.WriteLine(e.Message);
                Console.WriteLine(e.StackTrace);
                return false;
            }

        }
        /// <summary>
        /// 置換前チェック
        /// </summary>
        /// <returns>置換可能な場合True</returns>
        public bool IsReplaceOK()
        {
            if ((settings.FFACE.Enable && this.VersionFFACE == Constants.DefaultVersion) ||
                (settings.FFACETools.Enable && this.VersionFFACETools == Constants.DefaultVersion) ||
                (settings.EliteAPI.Enable && this.VersionEliteAPI == Constants.DefaultVersion) ||
                (settings.EliteMMOAPI.Enable && this.VersionEliteMMOAPI == Constants.DefaultVersion)) return false;
            return true;
        }
        /// <summary>
        /// コピー先のバージョンがコピー元より低いか
        /// </summary>
        /// <param name="iSourceFullPath">コピー元</param>
        /// <param name="iDestFullPath">コピー先</param>
        /// <returns>コピー先のバージョンが引く場合True</returns>
        private bool IsVersionAbobe(string iSourceFullPath, string iDestFullPath)
        {
            if (!File.Exists(iSourceFullPath) || !File.Exists(iDestFullPath)) return false;
            FileVersionInfo viS = FileVersionInfo.GetVersionInfo(iSourceFullPath);
            FileVersionInfo viT = FileVersionInfo.GetVersionInfo(iDestFullPath);
            if (viT.FileMajorPart < viS.FileMajorPart) return true;
            else
                if (viT.FileMajorPart < viS.FileMajorPart) return true;
                else
                    if (viT.FileBuildPart < viS.FileBuildPart) return true;
                    else
                        if (viT.FilePrivatePart < viS.FilePrivatePart) return true;
            return false;
        }
        /// <summary>
        /// DLLをインターネットよりダウンロード
        /// </summary>
        /// <param name="iDllType">DllType</param>
        /// <returns>成功した場合True</returns>
        private bool DownloadDll(DllType iDllType)
        {
            try
            {
                DllTypeInfo dll = settings.DllTypeInfoList[iDllType];
                if (!dll.Enable || dll.DownloadUrl.Length == 0 || dll.Filename.Length == 0) return false;
                // DLL保存ディレクトリ作成
                string path = GetSourcePath();
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }
                // ダウンロード
                string downloadFilename = Path.Combine(GetSourcePath(), Constants.FilenameTemp);
                if (File.Exists(downloadFilename)) File.Delete(downloadFilename);
                using (WebClient wc = new WebClient())
                {
                    if (settings.Proxy.Enable)
                    {
                        string proxy = string.Format("http://{0}:{1}", settings.Proxy.Server, settings.Proxy.Port);
                        wc.Proxy = new WebProxy(proxy);
                    }
                    wc.Encoding = Encoding.UTF8;
                    wc.DownloadFile(dll.DownloadUrl, downloadFilename);
                }
                // ファイル移動
                string destFilename = Path.Combine(GetSourcePath(), dll.Filename);
                if (File.Exists(destFilename)) File.Delete(destFilename);
                File.Move(downloadFilename, destFilename);
                // バージョン取得
                RefreshDllVersion();
                return true;
            }
            catch (Exception e)
            {
                string msg = string.Format(Resources.MsgVersionUpFor, iDllType, e.Message);
                throw new Exception(msg, e);
            }
        }
        /// <summary>
        /// DLLが保存されているパスを返す
        /// </summary>
        /// <param name="iDllFilename">保存パスに追加するファイル名</param>
        /// <returns>DLL保存パス</returns>
        private string GetSourcePath(DllType iDllType = DllType.Nothing)
        {
            if (iDllType == DllType.Nothing)
                return Path.Combine(".", Constants.PathSourceDll);
            else
                return Path.Combine(".", Constants.PathSourceDll, iDllType.GetFileName());
        }
        /// <summary>
        /// DLLのバージョン情報を更新
        /// </summary>
        private void RefreshDllVersion()
        {
            this.VersionFFACE = GetDllVersion(GetSourcePath(DllType.FFACE));
            this.VersionFFACETools = GetDllVersion(GetSourcePath(DllType.FFACETools));
            this.VersionEliteAPI = GetDllVersion(GetSourcePath(DllType.EliteAPI));
            this.VersionEliteMMOAPI = GetDllVersion(GetSourcePath(DllType.EliteMMOAPI));
        }
        /// <summary>
        /// DLL配布ページから、バージョンアップが必要か判定
        /// </summary>
        /// <param name="iDllType">DLL種別</param>
        /// <param name="oXPathData">判定用文字列</param>
        /// <returns>バージョンアップが必要な場合True</returns>
        private bool MasterVersionCheck(DllType iDllType, out string oXPathData)
        {
            try
            {
                oXPathData = string.Empty;
                DllTypeInfo dll = settings.DllTypeInfoList[iDllType];
                if (!dll.Enable || dll.XPath.Length == 0 || dll.CheckUrl.Length == 0) return false;

                using (WebClient wc = new WebClient())
                {
                    if (settings.Proxy.Enable)
                    {
                        string proxy = string.Format("http://{0}:{1}", settings.Proxy.Server, settings.Proxy.Port);
                        wc.Proxy = new WebProxy(proxy);
                    }
                    wc.Encoding = Encoding.UTF8;
                    string html = wc.DownloadString(dll.CheckUrl);

                    HtmlDocument htmlDoc = new HtmlDocument();
                    htmlDoc.LoadHtml(html);
                    HtmlNodeCollection nodes = htmlDoc.DocumentNode.SelectNodes(dll.XPath);
                    if (nodes != null && nodes.Count == 1)
                    {
                        string val = nodes[0].InnerText.Replace("\r", "").Replace("\n", "").Trim();
                        if (dll.XPathData != val)
                        {
                            oXPathData = val;
                            return true;
                        }
                    }
                    else
                    {
                        throw new Exception(Resources.MsgXPathNotExist);
                    }
                }
                return false;
            }
            catch (Exception e)
            {
                string msg = string.Format(Resources.MsgCheckingFor, iDllType, e.Message);
                throw new Exception(msg, e);
            }
        }
        #endregion
    }
}
