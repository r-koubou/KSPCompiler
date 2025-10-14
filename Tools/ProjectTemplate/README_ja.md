Project Template
================

csproj ファイルを生成するためのテンプレートプロジェクトです。

## 必要なソフトウェア

- [uv](https://github.com/astral-sh/uv)

## セットアップ

```bash
make setup
```

## 生成

### モジュール

```bash
./usecase.sh <モジュール名>
```

### コンソールアプリケーション

```bash
./cli.sh <アプリケーション名>
```

### ユースケース

```bash
./usecase.sh <ユースケース名>
```

`out/` ディレクトリに `csproj`、 `Tests.csproj` (ユニットテスト用プロジェクト) ファイルが生成されます。
