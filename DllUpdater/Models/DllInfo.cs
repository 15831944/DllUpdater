using Livet;

public class DllInfo : NotificationObject
{
    public DllInfo(bool iEnable, string iPath, string iVersion)
    {
        this.Enable = iEnable;
        this.Path = iPath;
        this.Version = iVersion;
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
    #region Version変更通知プロパティ
    private string _Version;
    public string Version
    {
        get
        { return _Version; }
        set
        {
            if (_Version == value)
                return;
            _Version = value;
            RaisePropertyChanged("Version");
        }
    }
    #endregion
}
