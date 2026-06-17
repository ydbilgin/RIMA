# RESP_TODAY_axflash

## KES Listesi
- **Mob-anim ve Char-anim-wire:** Tamamen KES. Karakter sadece basic idle/run ile sunulsun; moblar statik kalsın.
- **Build-UI-polish:** Grid low/norm/high görsel detayları, hover-cell ve status-bar makyajı KES.
- **low-HP-vignette:** Ekran kenarı kızarma/kararma efekti KES.
- **Capture-otomasyonu:** 4 eksik ekranı elle çek geç, otomasyon yazmayı KES.

## Kredi-Yak Ama-Ucuz Önerisi
- **Statik Asset Üretimi (Sıfır Animasyon Yükü):** Kredileri animasyon pipeline yerine tek kare statik sprite/decallere harca.
- **Eksik Proplar:** Build Mode'daki hayalet-asset'leri çözmek için varil, sandık vb. statik proplar üret.
- **Statik Düşman/Mob Yönlü Sprite'ları:** Blob okunurluğunu çözmek için 4-yönlü statik mob sprite'ları.
- **Statik Decal & VFX Shape'leri:** Zemin çatlakları, moss ve basit skill mermileri/patlama spriteları (koddan scale/fade edilecek).

## Bugün Kesin 4-5 İş Sıralı
1. **GATE bootstrap-fix:** Sahneler arası geçiş ve bootstrap kilidini aç (En kritik demo-blocker).
2. **PixelLab Batch Queue:** Prop, VFX ve mob statik spritelarını üretmek için promptları kuyruğa yolla.
3. **Boss P0 & Shop-Residue Cleanup:** Boss dövüşü sonrasındaki artık nesneleri temizle, can barını düzelt.
4. **Okunabilirlik (Black-Blob):** Yeni statik mob spritelarını bağla veya rim-light ekle.
5. **Director UI Kod-Skin:** Prefab refactor yapmadan sadece koddan font/layout düzeltmesi yap.

## En Büyük Zaman-Tuzağı
- **Animasyon Entegrasyon Zinciri (Animator & Pivot Cleanup):** PixelLab çıktılarının dilimlenmesi, pivot hizalaması, aynalama temizliği ve Unity Animator'a bağlanması 1.5 günde demoyu patlatır.
