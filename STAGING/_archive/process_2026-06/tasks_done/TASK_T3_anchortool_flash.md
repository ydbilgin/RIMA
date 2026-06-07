# Task — Lean weapon hand-anchor tuning tool (editor-only)

Amaç: Oturum B silah üretimi öncesi, 8 yön için el-anchor offset'lerini SceneView'da görsel olarak ayarlamayı kolaylaştırmak. Şu an offsetler Inspector'da çıplak array — hangi yön hangisi belli değil.

Kaynaklar (oku): `Assets/Scripts/Combat/OrientationSync.cs` (`handOffsets` 8-yön dizisi = SOURCE OF TRUTH; :64-66, :155 civarı) + `Assets/Scripts/Systems/Combat/HandAnchorAttach.cs` (runtime attach). `WeaponDatabaseSO` offset'i İKİNCİL — tool ona YAZMAZ, istersen read-only göster.

Yap (LEAN — over-engineering yok):
1. Yeni editor-only dosya: `Assets/Scripts/Editor/Combat/OrientationSyncAnchorEditor.cs` — `[CustomEditor(typeof(OrientationSync))]`.
2. SceneView'da seçili objenin 8 yön offset'i için: aktif yönün handle'ı (PositionHandle, 2D) sürüklenebilir; diğer 7 nokta küçük işaretle gösterilir + yön etiketi (S/SE/E/NE/N/NW/W/SW).
3. Inspector'a yön seçici (toolbar/popup) + per-yön Vector2 alanı.
4. `Undo.RecordObject` + `EditorUtility.SetDirty` ZORUNLU.
5. Runtime koduna dokunma (OrientationSync'e en fazla public read-accessor; mevcutsa onu kullan).

Doğrulama: compile-clean (console 0 error). Rapor: `STAGING/_done_T3_anchortool.md` (dosya + nasıl kullanılır 3 satır). Commit YAPMA.
