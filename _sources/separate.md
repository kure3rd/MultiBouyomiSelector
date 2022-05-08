# 各棒読みちゃんをOBS音源上で左右に振りたい

本プログラムの最終目的です。
**各棒読みちゃんを別々の仮想オーディオデバイスに通すことで、別のステレオ音源としてOBSに入力します**。

自分は配信者ではないのでわかりませんが、もっと一般的な方法があるかも知れません。
項目として追加したいので、是非Issueなどでご教示ください。


必要なもの
- 仮想オーディオデバイス、例えば
    - [SYNCROOM（シンクルーム）](https://syncroom.yamaha.com/?utm_source=jpy_foot_br&utm_medium=owned&utm_campaign=allyear)
        - …についてくる、Yamaha SYNCROOM driver
    - [VB\-Cable](https://vb-audio.com/Cable/index.htm)
    - 仮想オーディオデバイスであれば何でも良いはずです。
- （必要な方は）仮想ミキサー


ざっくり手順
- それぞれの棒読みちゃんの出力先を別々の仮想オーディオデバイスに設定
    - 本手順書ではSYNCROOM driverとVB-Cableを使っています
    - 同等の環境が用意できるなら何でも良いです
- OBSの音声出力キャプチャで各仮想オーディオデバイスを指定する
    - ステレオになっているので詳細プロパティから左右に振ることができる
- （必要な方は）仮想ミキサーから仮想オーディオデバイスの音を流す

## まず疑問に思うであろう点

### それぞれの棒読みちゃんをwin-capture-audioで取り込めばよいのでは？

[win\-capture\-audio](https://obsproject.com/forum/resources/win-capture-audio.1338/)では、同じプロセス名のプログラムは同一音源として扱われます。
そして棒読みちゃんは全て同じプロセス名BouyomiChan.exeとして動いています。
試してみましたが、複数の棒読みちゃんを起動していると、しっかりPIDを複数表示してくれます。仕様です。
このプロセス名はプログラム内に組み込まれており、外部から手を加えるのは難しいです。

可能ならもっとスマートな方法
……MultiBouyomiSelector自体に仮想デバイス機能を持たせたりして、MultiBouyomiSelectorで棒読みちゃんが出力している音声を取り込んで左右まで設定して、win-capture-audioから一発で入力できるようにする的な……
を取りたいですが、デバイスやプロセス関係は専門外で全く知識がありません。

求む有識者！


# 設定方法

## 仮想オーディオデバイスのインストール
- [VB\-Cable](https://vb-audio.com/Cable/index.htm)
- [SYNCROOM（シンクルーム）](https://syncroom.yamaha.com/?utm_source=jpy_foot_br&utm_medium=owned&utm_campaign=allyear)

指示通りインストールしてください。

## 棒読みちゃんの音声出力デバイス設定

![bouyomichan](image/bouyomichan.png)

棒読みちゃんの設定を開きます。

![bouyomichan-devicesetting](image/bouyomichan-devicesetting.png)

システム->音声出力デバイスから、使いたい仮想オーディオデバイスを選びます。
他の方向に振り分けたい棒読みちゃんとは別の仮想オーディオデバイスを選んでください。

（同じオーディオデバイスを選んでも問題なく動作はします。）


## OBS
ソース+ -> 音声出力キャプチャ -> 新規作成 -> 棒読みちゃんで選んだ仮想オーディオデバイス

追加したソースの音声ミキサーが動いたら成功です。オーディオの詳細プロパティから左右に振ってください。

詳細はOBSの使い方で調べてください。

## 仮想ミキサー

配信上には棒読みちゃんの声が乗りますが、自分のPC上では何も鳴りません。
必要であれば仮想ミキサーを使って、仮想オーディオデバイスからの出力をPC上で流してください。

やり方は使うミキサーによって様々です。
自分は[VB\-Audio VoiceMeeter Banana](https://vb-audio.com/Voicemeeter/banana.htm)を[配信や動画にVC音を入れない方法、Voicemeeter Bananaの使い方と設定の解説。USBヘッドセットもOK \| ARUTORA](https://arutora.com/15444)を参考にしながら使いました。

だいたいこんな感じになっていれば聞こえるはずです。

![banana](image/banana.png)