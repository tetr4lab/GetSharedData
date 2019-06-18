# GetSharedData / Googleスプレッドシートを使ったUnityコラボ開発
Import Google spreadsheet data into Unity editor.  
tags: Unity C# gas spreadsheet

# 前提
- unity 2018.4.1f1
- C#スクリプティング
- Googleスプレッドシート (ブラウザで使うグーグル製エクセル ※世間一般の理解です。)
- Googleアカウント
<details><summary>このシステムのセキュリティについて</summary><div>
- このシステムのセキュリティ
    - スプレッドシート
        - 特別な共有設定は不要で、共同編集者のGoogleアカウントに対する共有のみを設定します。
        - ドキュメントIDが、平文でエディタの設定内([EditorPrefs](https://docs.unity3d.com/ja/current/ScriptReference/EditorPrefs.html))に保存されます。
    - GAS (Google Apps Script)
        - URLを知っていれば誰でも使用できるように設定します。
        - 承認したGoogleアカウントの権限で実行され、承認者のGoogleドライブに保存された全てのスプレッドシートにアクセス可能です。
        - URLが、平文でエディタの設定内([EditorPrefs](https://docs.unity3d.com/ja/current/ScriptReference/EditorPrefs.html))に保存されます。
</div></details>

# できること
- テキストや数値などをGoogleスプレッドシートで共同編集できます。
- 編集結果は、Unityエディタへ任意のタイミングで取り込むことができます。
- 「シナリオやユーザ向けメッセージなどの編集者」や「各種の初期値を設定する担当者」がプログラマでない場合に共同制作が容易になります。
- 多言語対応のテキストと、言語に依存しない固定値 (int、float、string、bool)を扱えます。
- スクリプトに触らずに、スプレッドシートの式を編集するだけで改造できます。

# 特徴
- スプレッドシートの情報を、jsonやcsvのようなデータにせず、コードに変換してコンパイルします。
    - スプレッドシート上でC#のスクリプトを生成します。
    - スクリプトは、他のソースとともにコンパイルされます。
    - アプリ実行時には、単なるクラスあるいは定数になるので軽量です。

# 導入

### Google Apps Script の作成

- [The Apps Script Dashboard](https://script.google.com/home)へアクセスします。
- 「新規スクリプト」を作成し、`getspreadseet`と名付けます。
- 「コード.gs」の中身を、以下のコードで置き換えます。

```gas:getspreadseet/コード.gs
// getspreadseet?d=document&s=seet&a=area

function doGet(e) {
  if (e.parameter.k == '[Access Key]') {
    var spreadsheet = SpreadsheetApp.openById(e.parameter.d);
    var value = spreadsheet.getSheetByName(e.parameter.s).getRange(e.parameter.a).getValue();
    Logger.log (value);
    return ContentService.createTextOutput(value).setMimeType(ContentService.MimeType.TEXT);
  }
}
```
- **スクリプト中の「[Access Key]」を、キーワードで書き換えてください。**
    - これは秘密のURLの一部になるので、ランダムで十分長い文字列にしてください。
    - このキーワードは、後で使用しますので、何処か安全な場所に控えておいてください。
- スクリプトを保存します。
- 「公開」メニューから、「ウェブ アプリケーションとして導入」を選び、以下を選択します。
    - プロジェクト バージョン: `New`
    - 次のユーザーとしてアプリケーションを実行: `自分（～）`
    - アプリケーションにアクセスできるユーザー: `全員（匿名ユーザーを含む）`
- 「導入」ボタンを押します。
<details><summary>続いて、スクリプトを「承認」する必要があります。</summary><div>
- 承認の進め方
    - 「承認が必要です」と言われたら「許可を確認」するボタンを押します。
    - 「このアプリは確認されていません」と表示されたら「詳細」をクリックします。
    - 「getspreadsheet（安全ではないページ）に移動」と表示されるので、クリックします。
    - 「getspreadsheet が Google アカウントへのアクセスをリクエストしています」と表示されるので、「許可」します。
    - 「現在のウェブ アプリケーションの URL」が表示されます。
        - このURLは、後で使用しますので、何処か安全な場所に控えておいてください。
    - これによって、URLを知る誰でも、承認したアカウントの権限で、スクリプトが実行可能になります。
        - スクリプトは、アカウントにアクセス権がありドキュメントIDが分かる任意のスプレッドシートを読み取ることができます。
        - スクリプトと権限は、いつでも[The Apps Script Dashboard](https://script.google.com/home)から削除可能です。
</div></details>

# プロジェクトの準備

### スプレッドシートの用意
- [スプレッドシートの雛形](https://drive.google.com/open?id=1kRJ4CpSyD5lMNh_a37FlhArUmV8-x5Mh6FkdR_Kzjwk)を開き、ファイルメニューから「コピーを作成…」を選びます。
- フォルダを選んで保存します。
    - Googleドライブに保存されます。(ドライブの容量は消費しません。)
- マイドライブから保存されたフォルダを辿り、コピーされたスプレッドシートを開きます。
- 開いたスプレッドシートのURLで、`https://docs.google.com/spreadsheets/d/～～～/edit#gid=～`の「/d/～～～/edit#」の部分に注目してください。
    - この「～～～」の部分が、このスプレッドシートの`Document ID`です。
    - このIDは、後で使用しますので、何処か安全な場所に控えておいてください。

### アセットの導入と設定

- プロジェクトにアセット「GetSharedData.unitypackage」を導入してください。
- 「Window」メニューの「GetSharedData」を選択しウインドウを開きます。
- 「Application URL*」に、控えておいた「ウェブ アプリケーションの URL」を記入します。
- 「Access Key*」に、控えておいたキーワードを記入します。
- 「Document ID」に、控えておいたスプレッドシートのIDを記入します。
- 「Assets/Scripts/ Folder」は、プロジェクトのフォルダ構造に合わせて書き換えてください。
- 設定はエディタを終了しても保存されます。
    - 「*」の付く設定は、プロジェクトを跨いで共有されます。
    - その他の設定はプロジェクト毎に保持されます。
        - プロジェクトは、`Player Settings`の`Company Name`と`Product Name`で識別されますので、適切に設定してください。
    - 全ての設定は、[EditorPrefs](https://docs.unity3d.com/ja/current/ScriptReference/EditorPrefs.html)に平文で保存されます。

# 使い方

### スプレッドシート
- 最初から入っているシート名は変更しないでください。<details><summary>参考</summary>シート名(「テキスト」、「数値」)は`Assets/GetSharedData/Editor/GetSharedData.cs`に記述されています。</details>
- シートの追加や並び替えは任意に行うことができます。
- 「テキスト」シートに記載したものはclass、「数値」シートに記載したものはconstに変換されます。
- 「テキスト」は、7行目から記入していきます。
    - セル内改行は、改行文字`\n`に変換されます。
    - 7行目以降に記入例が書かれている可能性がありますが、書き替えて構いません。
    - 6行目までは、基本的に書き換えないでください。
    - 設定言語を変更する場合は、5行目(キー=Culture)に、[SystemLanguage](https://docs.unity3d.com/ja/current/ScriptReference/SystemLanguage.html)で定められた言語名でを記載してください。
- 「数値」は、4行目から記入していきます。
    - 4行目以降に記入例が書かれている可能性がありますが、書き替えて構いません。
- 「テキスト」の対応言語数を増減する場合は、スプレッドシートのO～R列およびA2セルの計算式を書き換える必要があります。(ご自身での対応が困難な場合はご相談ください。)
- 不用意に式が書き換えられないように、セルに対して適切に保護を施すことをお勧めします。

### Unityエディタ
- GetSharedDataウィンドウの「Get Shared Data」ボタンを押すか、Toolsメニューの「GetSharedData」を選ぶか、そのメニュー項目に表示されるキーボードショートカットを使うことで、スプレッドシートの情報が取り込まれ、コンパイルされます。

### スクリプトでの使用
#### テキスト
- `int Cns.Locale` 言語番号 (プロパティ)
    - 整数を代入することで、言語を選択します。(初期値`0`)
    - 番号は列の順(C=`0`,D=`1`,E=`2`)です。
    - 通常は、アプリの初期化で`Cns.Locale = -1;`と記述して、システムの言語設定(`Application.systemLanguage`)に従うように設定します。
        - システム言語に相当するテキストがシートにない場合は`0`が使われます。
- `string Cns.S (Cns.Nam key[, ...])` テキスト (メソッド)
    - キーを指定して現在設定されている言語のテキストを返します。
    - キーを列挙すると該当するテキストを連結して返します。
    - 使用するスクリプトファイルの冒頭で`using Nam = Cns.Nam;`と記述することで、`Cns.S (Nam.Language)`などと書けます。
- `string Cns.S (int locale, Cns.Nam key[, ...])` テキスト (メソッド)
    - 示された言語番号のテキストを返します。
- `int Cns.L (string str)` 言語番号の逆引き (メソッド)
    - [SystemLanguage](https://docs.unity3d.com/ja/current/ScriptReference/SystemLanguage.html)に定められた言語名を渡すと、言語番号を返します。
    - 未定義なら`-1`を返します。
- `Cns.Nam Cns.N (string str)` キーの逆引き (メソッド)
    - `Cns.S (～)`で得たテキストを渡すと、該当のキーを返します。
    - 未定義なら`-1`を返します。
- `string Cns.Language` 現在の言語名 (プロパティ)
    - 現在設定されている言語名を返します。
    - システムの設定に従うように`-1`を設定した場合は、`Application.systemLanguage`を返します。

#### 数値
- キーは`const`として定義されているので、そのままスクリプトで使用できます。

# 改造について
- このスプレッドシートではセルの式でC#ソースを生成し、アセットではセルから取得したソースをファイルに保存しています。
- 従って、スプレッドシートの式を書き換えれば、どのセルから何を集めてどういうクラスにするのかを自由に書き替えることが可能です。
- さらに、アセット側のエディタ拡張では、任意のシートの任意のセルを取得してファイルに保存できるようになっているので、生成するソースファイルを比較的容易に再構成できます。
- 上記のセルの式と保存ファイルの変更を組み合わせると、C#でない他の形式にすることも可能です。
