using System.Collections.Generic;
using System.Collections.ObjectModel;
using Livet;

namespace DllUpdater.Models
{
    public class Settings : NotificationObject
    {
        #region Constants
        private const string IniFileName = @".\DllUpdater.ini";
        // Globals
        private const string IniSectionGlobals = "Globals";
        // Globals Key
        private const string IniKeyCheckNewDllOnStartup = "CheckNewDllOnStartup";
        private const string IniKeyFindDllOnStartup = "FindDllOnStartup";
        private const string IniKeyTargetPath = "TargetPath";
        private const string IniKeyIgnorePath = "IgnorePath";
        // Globals Default
        private const bool IniDefaultCheckNewDllOnStartup = true;
        private const bool IniDefaultFindDllOnStartup = false;
        private const string IniDefaultTargetPath = "1,";
        private const string IniDefaultIgnorePath = "1,,1,1,1,1";

        // Proxy
        private const string IniSectionProxy = "Proxy";
        // Proxy Key
        private const string IniKeyProxyEnable = "Enable";
        private const string IniKeyProxyServer = "Server";
        private const string IniKeyProxyPort = "Port";
        // Proxy Default
        private const bool IniDefaultProxyEnable = false;
        private const string IniDefaultProxyServer = "localhost";
        private const int IniDefaultProxyPort = 8080;

        // DLL
        private const string IniSectionFFACE = "FFACE";
        private const string IniSectionFFACETools = "FFACETools";
        private const string IniSectionEliteAPI = "EliteAPI";
        private const string IniSectionEliteMMOAPI = "EliteMMOAPI";
        // DLL Key
        private const string IniKeyEnable = "Enable";
        private const string IniKeyCheckUrl = "CheckUrl";
        private const string IniKeyXPath = "XPath";
        private const string IniKeyXPathLastestData = "XPathLastestData";
        private const string IniKeyDownloadUrl = "DownloadUrl";
        // DLL Default FFACE
        private const bool IniDefaultEnableFFACE = true;
        private const string IniDefaultCheckUrlFFACE = "http://delvl.ffevo.net/Lolwutt/FFACE4-Public/blob/master/FFACE.dll";
        private const string IniDefaultXPathFFACE = ".//*[@id='tree-holder']/ul[2]/li/div[1]/div[1]/a";
        private const string IniDefaultXPathDataFFACE = "";
        private const string IniDefaultDownloadUrlFFACE = "http://delvl.ffevo.net/Lolwutt/FFACE4-Public/raw/master/FFACE.dll";
        // DLL Default FFACETools
        private const bool IniDefaultEnableFFACETools = true;
        private const string IniDefaultCheckUrlFFACETools = "https://github.com/h1pp0/FFACETools_ffevo.net/blob/master/Binary/FFACETools.dll";
        private const string IniDefaultXPathFFACETools = ".//*[@id='js-repo-pjax-container']/div[2]/span[1]/a";
        private const string IniDefaultXPathDataFFACETools = "";
        private const string IniDefaultDownloadUrlFFACETools = "https://github.com/h1pp0/FFACETools_ffevo.net/raw/master/Binary/FFACETools.dll";
        // DLL Default EliteAPI
        private const bool IniDefaultEnableEliteAPI = true;
        private const string IniDefaultCheckUrlEliteAPI = "http://ext.elitemmonetwork.com/downloads/eliteapi/index.php?v";
        private const string IniDefaultXPathEliteAPI = ".";
        private const string IniDefaultXPathDataEliteAPI = "";
        private const string IniDefaultDownloadUrlEliteAPI = "http://ext.elitemmonetwork.com/downloads/eliteapi/EliteAPI.dll";
        // DLL Default EliteMOAPI
        private const bool IniDefaultEnableEliteMMOAPI = true;
        private const string IniDefaultCheckUrlEliteMMOAPI = "http://ext.elitemmonetwork.com/downloads/elitemmo_api/index.php?v";
        private const string IniDefaultXPathEliteMMOAPI = ".";
        private const string IniDefaultXPathDataEliteMMOAPI = "";
        private const string IniDefaultDownloadUrlEliteMMOAPI = "http://ext.elitemmonetwork.com/downloads/elitemmo_api/EliteMMO.API.dll";
        #endregion

        private IniFileUtil ini;

        public Settings()
        {
            ini = new IniFileUtil(IniFileName);
            Load();
        }
        
        // Globals
        public bool CheckNewDllOnStartup { get; set; }
        public bool FindDllOnStartup { get; set; }
        public ObservableCollection<PathInfo> TargetPathList { get; set; }
        public ObservableCollection<PathInfo> IgnorePathList { get; set; }
        // Proxy
        public Proxy Proxy { get; set; }
        // DLL
        public DllTypeInfo FFACE { get; set; }
        public DllTypeInfo FFACETools { get; set; }
        public DllTypeInfo EliteAPI { get; set; }
        public DllTypeInfo EliteMMOAPI { get; set; }
        public Dictionary<DllType, DllTypeInfo> DllTypeInfoList = new Dictionary<DllType, DllTypeInfo>();

        /// <summary>
        /// 設定読み込み
        /// </summary>
        public void Load()
        {
            // Globals
            this.CheckNewDllOnStartup = ini.GetBool(IniSectionGlobals, IniKeyCheckNewDllOnStartup, IniDefaultCheckNewDllOnStartup);
            this.FindDllOnStartup = ini.GetBool(IniSectionGlobals, IniKeyFindDllOnStartup, IniDefaultFindDllOnStartup);
            // TargetPath
            this.TargetPathList = new ObservableCollection<PathInfo>();
            for (int i = 0; i < 100; i++)
            {
                string key = string.Format(IniKeyTargetPath + "{0:00}", i);
                string iniString = ini.GetString(IniSectionGlobals, key, IniDefaultTargetPath);
                string[] value = iniString.Split(',');
                if (value.Length == 2)
                {
                    if (value[1].Length > 0)
                    {
                        this.TargetPathList.Add(new PathInfo(
                            (value[0] == "1"),
                            value[1],
                            true,
                            true,
                            true,
                            true
                            ));
                    }
                }
            }
            if (this.TargetPathList.Count == 0)
            {
                this.TargetPathList.Add(new PathInfo(true, string.Empty, true, true, true, true));
            }
            // IgnorePath
            this.IgnorePathList = new ObservableCollection<PathInfo>();
            for (int i = 0; i < 100; i++)
            {
                string key = string.Format(IniKeyIgnorePath + "{0:00}", i);
                string iniString = ini.GetString(IniSectionGlobals, key, IniDefaultIgnorePath);
                string[] value = iniString.Split(',');
                if (value.Length == 6)
                {
                    if (value[1].Length > 0)
                    {
                        this.IgnorePathList.Add(new PathInfo(
                            (value[0] == "1"),
                            value[1],
                            (value[2] == "1"),
                            (value[3] == "1"),
                            (value[4] == "1"),
                            (value[5] == "1")
                            ));
                    }
                }
            }
            if (this.IgnorePathList.Count == 0)
            {
                this.IgnorePathList.Add(new PathInfo(true, string.Empty, true, true, true, true));
            }

            // Proxy
            this.Proxy = new Proxy()
            {
                Enable = ini.GetBool(IniSectionProxy,IniKeyProxyEnable,IniDefaultProxyEnable),
                Server = ini.GetString(IniSectionProxy, IniKeyProxyServer, IniDefaultProxyServer),
                Port = ini.GetInt(IniSectionProxy, IniKeyProxyPort, IniDefaultProxyPort)
            };

            // DLL FFACE
            this.FFACE = new DllTypeInfo()
            {
                DllType     = DllType.FFACE,
                Enable      = ini.GetBool(IniSectionFFACE, IniKeyEnable, IniDefaultEnableFFACE),
                Filename = Constants.FilenameFFACE,
                CheckUrl = ini.GetString(IniSectionFFACE, IniKeyCheckUrl, IniDefaultCheckUrlFFACE),
                XPath       = ini.GetString(IniSectionFFACE, IniKeyXPath, IniDefaultXPathFFACE),
                XPathData   = ini.GetString(IniSectionFFACE, IniKeyXPathLastestData, IniDefaultXPathDataFFACE),
                DownloadUrl = ini.GetString(IniSectionFFACE, IniKeyDownloadUrl, IniDefaultDownloadUrlFFACE)
            };
            this.DllTypeInfoList.Add(DllType.FFACE, this.FFACE);
            // DLL FFACETools
            this.FFACETools = new DllTypeInfo()
            {
                DllType = DllType.FFACETools,
                Enable = ini.GetBool(IniSectionFFACETools, IniKeyEnable, IniDefaultEnableFFACETools),
                Filename = Constants.FilenameFFACETools,
                CheckUrl = ini.GetString(IniSectionFFACETools, IniKeyCheckUrl, IniDefaultCheckUrlFFACETools),
                XPath = ini.GetString(IniSectionFFACETools, IniKeyXPath, IniDefaultXPathFFACETools),
                XPathData = ini.GetString(IniSectionFFACETools, IniKeyXPathLastestData, IniDefaultXPathDataFFACETools),
                DownloadUrl = ini.GetString(IniSectionFFACETools, IniKeyDownloadUrl, IniDefaultDownloadUrlFFACETools)
            };
            this.DllTypeInfoList.Add(DllType.FFACETools, this.FFACETools);
            // DLL EliteAPI
            this.EliteAPI = new DllTypeInfo()
            {
                DllType = DllType.EliteAPI,
                Enable = ini.GetBool(IniSectionEliteAPI, IniKeyEnable, IniDefaultEnableEliteAPI),
                Filename = Constants.FilenameEliteAPI,
                CheckUrl = ini.GetString(IniSectionEliteAPI, IniKeyCheckUrl, IniDefaultCheckUrlEliteAPI),
                XPath = ini.GetString(IniSectionEliteAPI, IniKeyXPath, IniDefaultXPathEliteAPI),
                XPathData = ini.GetString(IniSectionEliteAPI, IniKeyXPathLastestData, IniDefaultXPathDataEliteAPI),
                DownloadUrl = ini.GetString(IniSectionEliteAPI, IniKeyDownloadUrl, IniDefaultDownloadUrlEliteAPI)
            };
            this.DllTypeInfoList.Add(DllType.EliteAPI, this.EliteAPI);
            // DLL EliteMMOAPI
            this.EliteMMOAPI = new DllTypeInfo()
            {
                DllType = DllType.EliteMMOAPI,
                Enable = ini.GetBool(IniSectionEliteMMOAPI, IniKeyEnable, IniDefaultEnableEliteMMOAPI),
                Filename = Constants.FilenameEliteMMOAPI,
                CheckUrl = ini.GetString(IniSectionEliteMMOAPI, IniKeyCheckUrl, IniDefaultCheckUrlEliteMMOAPI),
                XPath = ini.GetString(IniSectionEliteMMOAPI, IniKeyXPath, IniDefaultXPathEliteMMOAPI),
                XPathData = ini.GetString(IniSectionEliteMMOAPI, IniKeyXPathLastestData, IniDefaultXPathDataEliteMMOAPI),
                DownloadUrl = ini.GetString(IniSectionEliteMMOAPI, IniKeyDownloadUrl, IniDefaultDownloadUrlEliteMMOAPI)
            };
            this.DllTypeInfoList.Add(DllType.EliteMMOAPI, this.EliteMMOAPI);
        }

        /// <summary>
        /// 設定保存
        /// </summary>
        public void Save()
        {
            // Globals
            ini.SetBool(IniSectionGlobals, IniKeyCheckNewDllOnStartup, this.CheckNewDllOnStartup);
            ini.SetBool(IniSectionGlobals, IniKeyFindDllOnStartup, this.FindDllOnStartup);
            // TargetPath
            for (int i = 0; i < 100; i++)
            {
                string key = string.Format(IniKeyTargetPath + "{0:00}", i);
                if (i < this.TargetPathList.Count)
                {
                    if (this.TargetPathList[i].Path.Length > 0)
                    {
                        string value = string.Join(",",
                            (this.TargetPathList[i].Enable) ? "1" : "0",
                            this.TargetPathList[i].Path);
                        ini.SetString(IniSectionGlobals, key, value);
                    }
                }
                else
                {
                    ini.RemoveKey(IniSectionGlobals, key);
                }
            }
            // IgnorePath
            for (int i = 0; i < 100; i++)
            {
                string key = string.Format(IniKeyIgnorePath + "{0:00}", i);
                if (i < this.IgnorePathList.Count)
                {
                    if (this.IgnorePathList[i].Path.Length > 0)
                    {
                        string value = string.Join(",",
                            (this.IgnorePathList[i].Enable) ? "1" : "0",
                            this.IgnorePathList[i].Path,
                            (this.IgnorePathList[i].FFACE) ? "1" : "0",
                            (this.IgnorePathList[i].FFACETools) ? "1" : "0",
                            (this.IgnorePathList[i].EliteAPI) ? "1" : "0",
                            (this.IgnorePathList[i].EliteMMOAPI) ? "1" : "0");
                        ini.SetString(IniSectionGlobals, key, value);
                    }
                }
                else
                {
                    ini.RemoveKey(IniSectionGlobals, key);
                }
            }

            // Proxy
            ini.SetBool(IniSectionProxy, IniKeyProxyEnable, this.Proxy.Enable);
            ini.SetString(IniSectionProxy, IniKeyProxyServer, this.Proxy.Server);
            ini.SetInt(IniSectionProxy, IniKeyProxyPort, this.Proxy.Port);

            // DLL FFACE
            ini.SetBool(IniSectionFFACE, IniKeyEnable, this.FFACE.Enable);
            ini.SetString(IniSectionFFACE, IniKeyCheckUrl, this.FFACE.CheckUrl);
            ini.SetString(IniSectionFFACE, IniKeyXPath, this.FFACE.XPath);
            ini.SetString(IniSectionFFACE, IniKeyXPathLastestData, this.FFACE.XPathData);
            ini.SetString(IniSectionFFACE, IniKeyDownloadUrl, this.FFACE.DownloadUrl);
            // DLL FFACETools
            ini.SetBool(IniSectionFFACETools, IniKeyEnable, this.FFACETools.Enable);
            ini.SetString(IniSectionFFACETools, IniKeyCheckUrl, this.FFACETools.CheckUrl);
            ini.SetString(IniSectionFFACETools, IniKeyXPath, this.FFACETools.XPath);
            ini.SetString(IniSectionFFACETools, IniKeyXPathLastestData, this.FFACETools.XPathData);
            ini.SetString(IniSectionFFACETools, IniKeyDownloadUrl, this.FFACETools.DownloadUrl);
            // DLL EliteAPI
            ini.SetBool(IniSectionEliteAPI, IniKeyEnable, this.EliteAPI.Enable);
            ini.SetString(IniSectionEliteAPI, IniKeyCheckUrl, this.EliteAPI.CheckUrl);
            ini.SetString(IniSectionEliteAPI, IniKeyXPath, this.EliteAPI.XPath);
            ini.SetString(IniSectionEliteAPI, IniKeyXPathLastestData, this.EliteAPI.XPathData);
            ini.SetString(IniSectionEliteAPI, IniKeyDownloadUrl, this.EliteAPI.DownloadUrl);
            // DLL EliteMMOAPI
            ini.SetBool(IniSectionEliteMMOAPI, IniKeyEnable, this.EliteMMOAPI.Enable);
            ini.SetString(IniSectionEliteMMOAPI, IniKeyCheckUrl, this.EliteMMOAPI.CheckUrl);
            ini.SetString(IniSectionEliteMMOAPI, IniKeyXPath, this.EliteMMOAPI.XPath);
            ini.SetString(IniSectionEliteMMOAPI, IniKeyXPathLastestData, this.EliteMMOAPI.XPathData);
            ini.SetString(IniSectionEliteMMOAPI, IniKeyDownloadUrl, this.EliteMMOAPI.DownloadUrl);
        }
    }

}
