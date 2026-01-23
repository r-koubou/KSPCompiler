KSP Manual Tools
================

KSP リファレンスマニュアルから変数やコマンド情報を抽出するツール群です。
現行のドキュメント構造を解析して、KSP Compiler で利用するためのデータを生成します。

## 必要なソフトウェア

- [uv](https://github.com/astral-sh/uv)

## セットアップ

```bash
make setup
```

## 生成

```bash
make gen
```

`output/` ディレクトリに `variables.txt` と `*-commands.txt` が生成されます。

### git が差分があると認識した場合

 - `<プロジェクトルート>/Data/Symbols/` 以下に置いている xlsx ファイルに追記、追加分をTSV形式で出力してください。
 - `<プロジェクトルート>/Compiler/Features/SymbolManagement/Applications/SymbolDatabaseCliApp` の　import プログラムを実行してください

 ## TODO

 上記の手動操作は開発者である私が把握しているだけなため、将来的にシンプルな管理方法への改善、自動化したいと考えています
