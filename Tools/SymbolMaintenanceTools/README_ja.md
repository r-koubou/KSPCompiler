Symbol Maintenance Tools
========================

KSP リファレンスマニュアルから以下の情報を抽出するツール群です。

- 変数
- コマンド
- コールバック
- UI

差分が出たシンボルをYAMLへ反映をマニュアルを見ながら手動で行います。（将来的には自動化したい）

## 必要なソフトウェア

- [uv](https://github.com/astral-sh/uv)

## セットアップ

```bash
make setup
```

## YAMLファイルのバリデーション

`<プロジェクトルート>/Data/Symbols/` 以下の YAML ファイルのバリデーションを行います。

```bash
make validate_yaml
```

## YAMLファイルのUUID重複チェック

`<プロジェクトルート>/Data/Symbols/` 以下の YAML ファイル内のUUIDの重複が無いかを検査します。

```bash
make validate_yaml_uuid
```


## スクレイピング、結果ファイルの生成

```bash
make gen
```

`output/` に生成されます。

### git が `output/**` に差分があると認識した場合

 - `<プロジェクトルート>/Data/Symbols/` 以下に置いている YAML ファイルを更新してください。
     - https://ksp-symbol-editor.pages.dev/ を利用して編集
