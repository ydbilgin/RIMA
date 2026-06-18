# Pre-Commit Findings: Council AxPro (Adversarial Review)

## VERDICT: REJECT

### 1. Mimari Zafiyet / Tasarım Hatası (KRİTİK)
**Sorun:** `PlayerClassManager.SetPrimaryClass` metodundaki "Demo-Safety" koruması ölümcül bir yan etki barındırıyor.
**Detay:** Eğer bir şekilde (eski save verisi, hatalı atama vb. sonucu) `SelectedClass` olarak oynanamaz bir sınıf (örn. Ronin) kalmışsa ve sahne yüklenirken `Start()` metodu `applyPrimaryOnStart` üzerinden bu sınıfı başlatmayı denerse, `SetPrimaryClass` içindeki kontrol şu işlemi yapar:
`Debug.LogWarning("..."); return;`
Bu `early-return` yüzünden `ApplyPrimaryClassToPlayer(type)` **asla çağrılmaz**. Sonuç olarak oyuncu prefab'ına hiçbir sınıf sistemi (RageSystem, stat profilleri, temel saldırı mekanikleri, görsel Animator) yüklenmez. Oyuncu sahnede yeteneksiz, can değeri hatalı ve tamamen bozuk (husk) bir formda kalır.
**Çözüm:** Reddedilen sınıf durumunda sessizce `return` yapmak yerine, `type = ClassType.Warblade;` gibi güvenli bir fallback (yedek) sınıfa düşülmeli ve kurulum bu güvenli sınıfla tamamlanmalıdır.

### 2. Figür Kalitesi / Yalanlama (Şekil 9)
**Sorun:** `fig_weapon_mount.png` (Şekil 9) görseli büyük bir uyumsuzluk barındırıyor.
**Detay:** Görsel incelendiğinde, karakterin elleri iki yanında boş durmaktadır. Silah (beyaz-sarı büyük mızrak/kılıç) karakterin **sırtında/arkasında** çapraz bir şekilde asılı (sheathed/mounted) durmaktadır. Silah kesinlikle **"elde" (wielded) DEĞİLDİR**. Eğer rapor veya caption silahın elde tutulduğunu iddia ediyorsa, jüri önünde net bir yalanlama yaratacaktır.
**Çözüm:** Rapor metni/caption silahın "sırtta asılı (mounted)" olduğunu belirtecek şekilde değiştirilmeli ya da silahın gerçekten elde tutulduğu yeni bir render alınmalıdır.

### 3. Figür Kalitesi (Şekil 6)
**Durum:** Temiz.
**Detay:** `fig_rooms_island_grid.png` görselinde bahsedilen "void-leak" (sızıntı) veya mor dikdörtgen hataları gözlemlenmemiştir. İzometrik platform sınırları (root-like yapılarla) tutarlı ve temiz renderlanmıştır.

### 4. Diğer Eklemeler
- `DebugLogOverlay` (F3) ve `DraftManager` pasif bildirim (toast) entegrasyonları genel olarak güvenlidir. Ancak `DebugLogOverlay`, eğer arka plan thread'lerinden (UI dışı) log gelirse `Queue` üzerinde lock olmadığı için `InvalidOperationException` riski taşır. Demo için tolere edilebilir, ancak mimari olarak not edilmelidir.

## MUTLAKA DÜZELT:
1. `PlayerClassManager.SetPrimaryClass` içine fallback mekanizması (Warblade) ekle.
2. Şekil 9'un rapor caption'ını "silah sırtta asılı (mounted)" olarak düzelt veya görseli değiştir.
