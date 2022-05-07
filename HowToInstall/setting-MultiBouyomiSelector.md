# MultiBouyomiSelector側

## 設定
[MultiBouyomiSelector](https://github.com/kure3rd/MultiBouyomiSelector/releases)を解凍してsettings.xmlを開きます。
以下のような部分があります。

```xml
...
    <BouyomiChanLocations>
        <BouyomiChanA>C:\Users\test\Downloads\BouyomiChan_0_1_11_0_Beta21</BouyomiChanA>
        <BouyomiChanB>C:\Users\test\Downloads\BouyomiChan_0_1_11_0_Beta21_(1)</BouyomiChanB>
    </BouyomiChanLocations>
...
```
\<BouyomiChanLocations\>と\</BouyomiChanLocations\>の間に、先程設定した各棒読みちゃんの情報を記入していきます。

（
間の2行は設定例として書いてあります。
先に消しておきましょう。
一応設定例の行があっても動きはしますが、見栄えは悪いです。
）


それぞれの棒読みちゃんの名前と場所を書いていきます。
書き方は
```xml
<棒読みちゃんの名前>棒読みちゃんのフォルダ</棒読みちゃんの名前>
```
です。

- 棒読みちゃんの名前は、棒読みちゃんの設定ファイルに書いた名前です。
- 棒読みちゃんのフォルダは、棒読みちゃんを展開したフォルダ名です。
    - BouyomiChan.exeを右クリック->プロパティからコピーできます。

(棒読みちゃん.exeプロパティの画像)

## 動作確認

1. MultiBouyomiSelector.exeを起動します。
    - 設定されているBouyomiChan.exeが一斉に起動します。
    - エラーが出るときはHelpページを参照してください。
1. 下図のような状態になれば、設定がうまくいっています。

（MultiBouyomiSelector待機画像）
