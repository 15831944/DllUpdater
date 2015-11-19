using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace DllUpdater.Models
{
    /// <summary>
    /// INIファイルハンドラー
    /// </summary>
    public class IniFileUtil
    {
        private const int MAX_BUFFER_SIZE = 1024;
        private const string DEFAULT_STRING = "";
        private const int DEFAULT_INT = 0;
        private const bool DEFAULT_BOOL = false;

        /// <summary>
        /// コンストラクター
        /// </summary>
        /// <param name="iIniFileName">INIファイル名</param>
        public IniFileUtil(string iIniFileName)
        {
            this.FileName = iIniFileName;
        }

        #region メンバー
        /// <summary>
        /// INIファイル名称
        /// </summary>
        public string FileName { get; private set; }
        #endregion

        #region メソッド
        public object GetObject(string iSection, string iKey, object iValue)
        {
            string def = (iValue == null) ? DEFAULT_STRING : iValue.ToString();
            return GetString(iSection, iKey, def);
        }
        public bool SetObject(string iSection, string iKey, object iValue)
        {
            return SetString(iSection, iKey, iValue.ToString());
        }
        /// <summary>
        /// 文字列を取得
        /// </summary>
        /// <param name="iSection">セクション名</param>
        /// <param name="iKey">キー名</param>
        /// <param name="iDefault">デフォルト文字列</param>
        /// <returns>INIファイルより取得した文字列</returns>
        public string GetString(string iSection, string iKey, string iDefault = DEFAULT_STRING)
        {
            if (string.IsNullOrEmpty(iSection) || string.IsNullOrEmpty(iSection)) return iDefault;
            StringBuilder sb = new StringBuilder(MAX_BUFFER_SIZE);
            IniFileUtil.GetPrivateProfileString(iSection, iKey, iDefault, sb, (uint)sb.Capacity, this.FileName);
            return sb.ToString();
        }
        /// <summary>
        /// 文字列を設定
        /// </summary>
        /// <param name="iSection">セクション名</param>
        /// <param name="iKey">キー名</param>
        /// <param name="iValue">値</param>
        /// <returns>成功：True</returns>
        public bool SetString(string iSection, string iKey, string iValue)
        {
            uint res = IniFileUtil.WritePrivateProfileString(iSection, iKey, iValue, this.FileName);
            return (res != 0);
        }
        /// <summary>
        /// 数値を取得
        /// </summary>
        /// <param name="iSection">セクション名</param>
        /// <param name="iKey">キー名</param>
        /// <param name="iDefault">デフォルト数値</param>
        /// <returns>INIファイルより取得した数値</returns>
        public int GetInt(string iSection, string iKey, int iDefault = DEFAULT_INT)
        {
            if (string.IsNullOrEmpty(iSection) || string.IsNullOrEmpty(iSection)) return iDefault;
            string defaultValue = iDefault.ToString();
            string iniRes = this.GetString(iSection, iKey, defaultValue);
            int ret = 0;
            bool parseRes = int.TryParse(iniRes,out ret);
            return parseRes ? ret : iDefault;
        }
        /// <summary>
        /// 数値を設定
        /// </summary>
        /// <param name="iSection">セクション名</param>
        /// <param name="iKey">キー名</param>
        /// <param name="iValue">値</param>
        /// <returns>成功：True</returns>
        public bool SetInt(string iSection, string iKey, int iValue)
        {
            if (string.IsNullOrEmpty(iSection) || string.IsNullOrEmpty(iSection)) return false;
            return this.SetString(iSection, iKey, iValue.ToString());
        }
        /// <summary>
        /// boolを取得
        /// </summary>
        /// <param name="iSection">セクション名</param>
        /// <param name="iKey">キー名</param>
        /// <param name="iDefault">デフォルト数値</param>
        /// <returns>INIファイルより取得した数値</returns>
        public bool GetBool(string iSection, string iKey, bool iDefault = DEFAULT_BOOL)
        {
            if (string.IsNullOrEmpty(iSection) || string.IsNullOrEmpty(iSection)) return iDefault;
            string defaultValue = iDefault ? "1" : "0";
            string iniRes = this.GetString(iSection, iKey, defaultValue);
            if (iniRes == "0" || iniRes == "1")
            {
                return (iniRes == "1") ? true : false;
            }
            else
            {
                return iDefault;
            }
        }
        /// <summary>
        /// boolを設定
        /// </summary>
        /// <param name="iSection">セクション名</param>
        /// <param name="iKey">キー名</param>
        /// <param name="iValue">値</param>
        /// <returns>成功：True</returns>
        public bool SetBool(string iSection, string iKey, bool iValue)
        {
            if (string.IsNullOrEmpty(iSection) || string.IsNullOrEmpty(iSection)) return false;
            string value = iValue ? "1" : "0";
            return this.SetString(iSection, iKey, value);
        }
        /// <summary>
        /// INIファイルのセクション名一覧を取得
        /// </summary>
        /// <param name="iDefault">デフォルトセクション名</param>
        /// <returns>INIファイルより取得したセクション名一覧</returns>
        public List<string> GetSectionList(string iDefault = DEFAULT_STRING)
        {
            byte[] b = new byte[1024];
            uint length = IniFileUtil.GetPrivateProfileStringByByteArray(null, null, iDefault, b, (uint)b.Length, this.FileName);
            string[] sections = Encoding.Default.GetString(b, 0, (int)length - 1).Split('\0');
            List<string> ret = new List<string>();
            foreach (string section in sections)
            {
                ret.Add(section);
            }
            return ret;
        }
        /// <summary>
        /// INIファイルのキー名一覧を取得
        /// </summary>
        /// <param name="iSection">セクション名</param>
        /// <param name="iDefault">デフォルト文字列</param>
        /// <returns>INIファイルのキー名一覧</returns>
        public List<string> GetKeyList(string iSection, string iDefault = DEFAULT_STRING)
        {
            if (string.IsNullOrEmpty(iSection)) return new List<string>();
            byte[] b = new byte[MAX_BUFFER_SIZE];
            uint length = IniFileUtil.GetPrivateProfileStringByByteArray(iSection, null, iDefault, b, (uint)b.Length, this.FileName);
            string[] keys = Encoding.Default.GetString(b, 0, (int)length - 1).Split('\0');
            List<string> ret = new List<string>();
            foreach (string key in keys)
            {
                ret.Add(key);
            }
            return ret;
        }
        /// <summary>
        /// セクションを削除
        /// </summary>
        /// <param name="iSection">セクション名</param>
        /// <returns>成功：True</returns>
        public bool RemoveSection(string iSection)
        {
            if (string.IsNullOrEmpty(iSection)) return false;
            IniFileUtil.WritePrivateProfileString(iSection, null, null, this.FileName);
            return true;
        }
        /// <summary>
        /// キーの削除
        /// </summary>
        /// <param name="iSection">セクション名</param>
        /// <param name="iKey">キー名</param>
        /// <returns>成功：True</returns>
        public bool RemoveKey(string iSection, string iKey)
        {
            if (string.IsNullOrEmpty(iSection) || string.IsNullOrEmpty(iSection)) return false;
            IniFileUtil.WritePrivateProfileString(iSection, iKey, null, this.FileName);
            return true;
        }
        #endregion

        #region DllImport
        [DllImport("KERNEL32.DLL")]
        public static extern uint GetPrivateProfileString(string lpAppName, string lpKeyName, string lpDefault, StringBuilder lpReturnedString, uint nSize, string lpFileName);

        [DllImport("KERNEL32.DLL", EntryPoint = "GetPrivateProfileStringA")]
        public static extern uint GetPrivateProfileStringByByteArray(string lpAppName, string lpKeyName, string lpDefault, byte[] lpReturnedString, uint nSize, string lpFileName);

        [DllImport("KERNEL32.DLL")]
        public static extern uint GetPrivateProfileInt(string lpAppName, string lpKeyName, int nDefault, string lpFileName);

        [DllImport("KERNEL32.DLL")]
        public static extern uint WritePrivateProfileString(string lpAppName, string lpKeyName, string lpString, string lpFileName);
        #endregion

    }
}
