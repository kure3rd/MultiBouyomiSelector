# 棒読みちゃんの設定

## 棒読みちゃんの解凍

**最新版**をダウンロードしてきます。

![bouyomichan-beta](../image/bouyomichan-beta.png)

必要な人数分、フォルダを展開します。
展開する場所はどこでも大丈夫です。
あとで必要なので、フォルダ名が被らないようにしてください。

例) C:\Users\name\Downloads\BouyomiChan_0_1_11_0_Beta21_A

## 棒読みちゃんの設定

展開したら、以下の作業をそれぞれの棒読みちゃんで行います。

1. 棒読みちゃんの設定を開く
    - "このプラグインを有効にしますか？"みたいなのは全部"いいえ"でOK
1. システム->アプリケーション連携を設定する
    - IpcClientChannelでの接続を受けるを*True*にする
    - チャンネル名を設定する
        - 例) BouyomiChan*A*
        - チャンネル名は必ずBouyomiChan**以外**に変更する
    - 以下の項目の*True*を*False*に変更する
        - ローカルTCPサーバ機能を使う
        - ローカルHTTPサーバ機能を使う
1. 棒読みちゃんを閉じる
    - MultiBouyomiSelectorの設定に移る前に、必ず全て閉じておく

![BouyomiChan Setting](../image/bouyomichan-setting.png)

棒読みちゃんの声の設定はそれぞれで管理されています。
BouyomiChanAは普通の声、BouyomiChanBは高い声のように設定しておくと聞き分けやすいでしょう。
一度設定しておくと、次に起動したときにも同じ設定で喋ってくれます。