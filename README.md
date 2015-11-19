# ![DllUpdater](http://i.imgur.com/Kr6rOIn.png)DllUpdater

DllUpdaterは、FFACE.dll、FFACETools.dll、EliteAPI.dll、EliteMMO.API.dllの最新バージョンをダウンロードし、指定されたフォルダのDLLを置換するユーティリティです。

![全体イメージ](http://i.imgur.com/bJTipVs.png)

## 使用方法
1. 初回起動時、自動的に最新のDLLをインターネットより取得するので、終わるまで待ちましょう。
2. 設定タブより検索対象とするディレクトリを登録する。
3. ![検索ボタン](http://i.imgur.com/Ge3019j.png)検索ボタンを押して、DLLを検索する。
4. 一覧に検索結果が表示されるので、更新が必要なDLLをチェックします。
5. ![置換ボタン](http://i.imgur.com/GFyLaix.png)置換ボタンを押して、DLLを更新します。

## 画面説明
### メイン画面
![メイン画面](http://i.imgur.com/bJTipVs.png)
* バージョン
![バージョン](http://i.imgur.com/p4oZrgm.png)
現在、DllUpdaterが取り込んでいるDLLのバージョンを表示しています。  
チェックをオフにする事で、オフにしたDLLの機能（バージョンチェック・検索・置換）を停止させる事ができます。必要の無いものはチェックを外しておきましょう。

* ![バージョンチェックボタン](http://i.imgur.com/STluor3.png)バージョンチェックボタン  
DLLの配布先のページをチェックし、変更があった場合ステータスバーにその旨が表示されます。

* ![ダウンロードボタン](http://i.imgur.com/6OdLK8K.png)ダウンロードボタン  
DLLの配布先のページをチェックし、変更があった場合DLLのダウンロードを行い、DllUpdaterに取り込みます。

* ![検索ボタン](http://i.imgur.com/Ge3019j.png)検索ボタン  
設定画面で指定された対象パス・除外パスを元に、DLLの検索を行い一覧に表示します。  
置換する必要のないDLLは、デフォルトでチェックが外れています。

* ![全てチェック/解除ボタン](http://i.imgur.com/EXm69Jw.png)全てチェック/解除ボタン  
一覧のチェックボックスを全てチェック/解除します。

* ![置換ボタン](http://i.imgur.com/GFyLaix.png)置換ボタン  
一覧でチェックの入ったDLLのみ、DllUpdaterに取り込まれた最新のDLLに置換します。

### 設定画面
![設定画面](http://i.imgur.com/gGDxkZb.png)
* 起動時に最新のDLLをダウンロードする  
起動時に、インターネットのDLL配布ページを見に行き、変更があった場合最新のDLLをダウンロードします。

* 起動時に置換対象のDLLを検索する  
起動時に、自動的にDLLの検索を行います。

* Proxy  
プロキシのサーバー名とポート番号を設定します。

* 対象パス  
DLLを検索する対象となるパスを指定します。（サブフォルダも含みます）  
Enableのチェックを外すと、一時的に対象外とする事ができます。

* 除外パス  
DLLを検索する対象としたくないパスを指定します。（サブフォルダも含みます）  
各DLLのチェックを外すと、除外対象から外れます。  
たとえば、EnjoyFishingは独自のFFACEToolsを使用しており、FFACETools以外を更新したい。  
その場合には以下のように、FFACEToolsのみ除外することができます。
![例](http://i.imgur.com/r1jR3Xv.png)

## インストール・アンインストール
1. [こちら](https://github.com/rohme/DllUpdater/releases)から、最新版のバイナリを取得する。
2. 適当なディレクトリに、バイナリを解凍する。
3. アンインストールは、解凍したディレクトリを削除するだけです。

## 開発環境
* Windows7 Ultimate 64bit
* [Microsoft Visual Studio Express 2013 C#](http://www.visualstudio.com/ja-jp/products/visual-studio-express-vs.aspx)
* [.NET Framework 4.0](http://www.microsoft.com/ja-jp/net/)以上

## ソース
DllUpdaterのソースは以下のサイトで、GPLv2ライセンスにて公開しています。  
https://github.com/rohme/  
障害報告・質問とかあればIssuesに登録して頂いて結構です。

## 免責事項
　本ソフトはフリーソフトです。自由にご使用ください。  
　このソフトウェアを使用したことによって生じたすべての障害・損害・不具合等に関しては、作者は一切の責任を負いません。各自の責任においてご使用ください。  

## 修正履歴
* **2015-11-19 Ver0.0.1 プレリリース**
	- とりあえずプレリリース
