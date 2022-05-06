# うまく動かないときは

## Failed to load settings.xmlと出てくる

### 先頭と末尾の名前が違っている
`<棒読みちゃんの名前>棒読みちゃんAの場所</棒読みちゃんの名前>`にしてください。
うしろの名前の前の**'/'も重要**です。

### そもそもsetting.xmlがMultiBouyomiSelector.exeと同じフォルダに無い
ダウンロードし直してください。

それでも無ければ連絡ください。Issueとか。

## "場所かチャンネル名が同じ設定になっています"と出る
settings.xmlに書いている場所かチャンネル名が同じになっています。

異なる場所の棒読みちゃんに異なる名前をつけて、settings.xmlを書き直してください。

## 起動はするけど喋ってくれない

### Connectedにならない
MultiBouyomiChanSelector側の設定ファイルで、棒読みちゃんの名前が間違っているかも。
場所と名前を対応させてください。

## TCPサーバーが起動しないとか出てくる

(画像)

それぞれの棒読みちゃんの設定ファイルで、以下を*false*にしてください。
- \<EnableSocket\>*true*\</EnableSocket\>
- \<EnableHttpd\>*true*\</EnableHttpd\>