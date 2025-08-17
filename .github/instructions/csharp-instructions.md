---
# applyTo: "**/*.cs"
---

# C# コーディング方針（要点）

- C# のコードは **プロジェクトの LangVersion に合わせつつ、可能な限り最新の C# 13 の記法・API を優先**すること。
  - ただし **ビルドが通らないプレビュー機能は使わない**。必要なら C# 12 相当へ安全にフォールバックする。
- **file-scoped namespace**、`using` の重複回避／並び順の最適化。
- **nullable 有効** 前提（`#nullable enable` / プロジェクト設定）。
- 非同期は `async`/`await`、**`ConfigureAwait(false)` はライブラリ内のみ**。
- **`required` メンバー／`init` アクセサ**を活用（対応環境のみ）。
- **`switch` 式／パターンマッチング**で条件分岐を簡潔に。
- **`Span<T>`/`Memory<T>`** や `ReadOnlySpan<char>` はホットパスでのみ使用、先に可読性を優先。
- 例外は**ガード節**で早期 return、`try/catch` の粒度は最小限。
