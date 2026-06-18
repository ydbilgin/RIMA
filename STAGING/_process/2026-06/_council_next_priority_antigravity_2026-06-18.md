# COUNCIL DECISION — Antigravity Priority Plan (2026-06-18)

## A) Aksiyon Planı (Yarına Kadar Sıralı)

1. **Polish Commit + Exposure (0.35 -> 0.6)**
   - **Gerekçe:** Editör canlı sunumunda görünebilirlik moody atmosferden çok daha önemlidir. Uygulanan polish commit edilmeli ve exposure 0.6'ya yükseltilmelidir.
   - **Risk:** Sıfır (Sadece rendering ayarı).

2. **Failed-Cast No-Op Feedback (SFX/Toast/Flash)**
   - **Gerekçe:** Oyuncu skill basıp hiçbir şey olmadığında oyun bozuk veya tepkisiz sanılır. Çok kritik bir demo-killer hissiyatıdır. CanExecute sonucuna göre basit UI/Audio feedback eklenmelidir.
   - **Risk:** Düşük-Orta (Core combat loop'a dokunmadan sadece geri bildirim katmanında çözülmelidir).

3. **Director Mode Duplicate Slot Engelleyici**
   - **Gerekçe:** Director Mode sunumun centerpiece'idir. Aynı skill'in 2. slota yerleştirilip cooldown paylaşması sunumda direkt sırıtacaktır. UI/Loadout seviyesinde duplicate engeli getirilmelidir.
   - **Risk:** Düşük (Sadece UI/Loadout validation seviyesinde).

4. **HUD HP-bar Lerp + Toast Ease (Tier-2 Polish)**
   - **Gerekçe:** Canlı sunumda hasar akışının ve can barlarının yumuşak hareket etmesi seyirci için görsel algıyı ve oyun kalitesini doğrudan artırır.
   - **Risk:** Çok Düşük (UI katmanı).

5. **Low-HP/Rage Kırmızı Ekran De-stack**
   - **Gerekçe:** Boss veya low-health anlarında overlay glitch'i oluşması amatörce durur, sunum kalitesini düşürür.
   - **Risk:** Düşük (Arbitration/Alpha clamp).

## B) Ertelenenler Listesi (Post-Demo)

- **Merchant Persistent Echo Drain:** Demo tek run üzerinden sunulacağı için meta-currency persistence ihlali gözden kaçacaktır. Save/load sistemlerine demo öncesi dokunmak gereksiz risk oluşturur.
- **Dead-but-Acting (2.3s etkileşim penceresi):** Ölüyken kapı/ödül alma ihtimali çok dar bir penceredir. Geniş bir state machine refactoring'e girmek çok risklidir.
- **healMultiplier / AntiHealAura concurrency yarışı:** Core stat save/restore mantığına dokunmak demo öncesi en büyük risklerden biridir, kesinlikle ertelenmeli.
- **Combo/Damage Correctness (Glacial+Burn, Ice-Shatter, Severance):** Detaylı combat hesaplamaları canlı sunumda fark edilemeyecek kadar incedir. Hasar formüllerine dokunulmamalıdır.
- **Find-in-hot-path (9 Find çağrısı):** Guarded cache optimizasyonu post-demo aşamasına bırakılmalıdır. Demo sunum performansını bloke etmez.
