# インストール

[ここ](https://github.com/kure3rd/MultiBouyomiSelector/releases)から最新版をダウンロードして展開してください。
個人作成のプログラムなのでブラウザとかOSとかにとやかく言われると思いますが…。

## 設定方法

棒読みちゃんとの連携のために、いくつかの設定を書き直します。

### 棒読みちゃん側

#### インストール

**最新版**をダウンロードしてきます。
(画像)

必要な人数分、フォルダを展開します。
展開する場所はどこでも大丈夫です。
あとで必要なので、フォルダ名が被らないようにしてください。

例) C:\Users\name\Downloads\BouyomiChan_0_1_11_0_Beta21_A

#### 設定変更

展開したら、以下の作業をそれぞれの棒読みちゃんで行います。

1. 一度起動して、全ての確認メッセージに答えた後、棒読みちゃんをを終了する
    - BouyomiChan\(.setting\)が生成される
2. BouyomiChan\(.setting\)を開く
3. 棒読みちゃんの名前を決め、\<IpcChannelName\>BouyomiChan\</IpcChannelName\>に記入する
    - 例) BouyomiChan*A*
    - 必ずBouyomiChan**以外**に変更する
4. \<EnableSocket\>*true*\</EnableSocket\>,\<EnableHttpd\>*true*\</EnableHttpd\>の*true*を*false*にする
5. 保存する

（BouyomiChan.setting画像）

### MultiBouyomiChanSelector側

settings\(.xml\)を開きます。以下のような部分があります。

```xml
...
    <BouyomiChanLocations>
        <BouyomiChanA>C:\Users\test\Downloads\BouyomiChan_0_1_11_0_Beta21</BouyomiChanA>
        <BouyomiChanB>C:\Users\test\Downloads\BouyomiChan_0_1_11_0_Beta21_(1)</BouyomiChanB>
    </BouyomiChanLocations>
...
```

ここに自分の棒読みちゃんの情報を書きます。書き方は

```xml
<棒読みちゃんの名前>棒読みちゃんのフォルダ</棒読みちゃんの名前>
```

- 棒読みちゃんの名前は、棒読みちゃんの設定ファイルに書いた名前です
- 棒読みちゃんのフォルダは、棒読みちゃんを展開したフォルダ名です

棒読みちゃんを展開したフォルダ名は、棒読みちゃんのファイルを右クリック->プロパティからコピーできます。

(画像)

## 確認方法

- [MultiCommentViewer](https://ryu-s.github.io/app/multicommentviewer)とか[MultiCommentViewer](https://develop-kui.com/blog/multicommentviewer-download/)とかをダウンロードします。
    - 使い方は各ページを参照してください。
- 適当なライブ配信を設定します。
- 棒読みちゃん連携をonにします。
- MultiBouyomiSelectorを起動します。
    - 一緒に棒読みちゃんも起動します。
- ちょっと間をおいて、棒読みちゃんたちが喋り始めます。
