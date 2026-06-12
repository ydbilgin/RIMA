# RIMA Damage Type Taksonomisi — DEEP ARCHITECTURE / DESIGN lens (Gemini 3.1 Pro)

Sen RIMA (2D top-down ARPG roguelite, 10 oynanabilir class) için derin sistem tasarımcısısın. Damage type taksonomisi kararı veriyoruz. DERİN MİMARİ/DESIGN lens — class kimlikleriyle tutarlılık, uzun-vade ölçeklenebilirlik, ARPG/LoL global konvansiyon doğruluğu.

## Sabit kararlar (değiştirme, üstüne tasarla)
- Ana stat ikilisi KİLİT: **Physical Damage** + **Ability Power (AP)**. (NLM canon)
- Mevcut enum başlangıç: `DamageType { Physical, Ability, True }`.
- 10 class var; **Elementalist** çalışma-anında element-switch yapıyor (fire/frost/lightning).
- Renk paleti adayları (global konvansiyon): True=beyaz #F4F0E6, Physical=ember #E89020, Magic/Ability=cyan #00FFCC, Fire #FF6A1F, Frost #7FE0FF, Lightning #FFD24A, Void/Shadow #7B3FA8, Light/Holy #FFF0B0.

## Cevapla (numaralı, gerekçeli)
1. **Taksonomi mimarisi:** Elemental alt-türler (Fire/Frost/Lightning/Void/Light) AP'nin alt-etiketi mi olmalı, yoksa bağımsız damage type mı? İki-eksen model mantıklı mı: `DamageType {Physical, Ability, True}` × `ElementTag {None,Fire,Frost,Lightning,Void,Light}` — Ability + ElementTag kombinasyonu? Bu, "AP elemental'a bölünmez ama elemental flavor/resist taşır" canon'unu korur mu? Tek-eksen flat enum'a karşı avantaj/dezavantaj.
2. **Light/Holy ve True ayrımı:** Light ayrı bir damage type mı yoksa True'nun reskin'i mi? Hangi class'lar Light kullanır (Templar/Cleric benzeri)? True'yu sadece "delici/resist-yok" rolüne mi sakla?
3. **Class tutarlılığı:** 10 class kimliğine göre her class'ın baskın damage type'ını/element'ini eşle. Physical-only class'lar (warrior/rogue), AP-pure (mage), elemental-switch (Elementalist), hybrid (paladin?) — taksonomi tüm kimlikleri temiz ifade ediyor mu? Eksik/fazla tür var mı?
4. **Resist/armor mimarisi:** ARPG-standart hangisi — Physical→Armor (flat veya %), Elemental→per-type resist mi yoksa tek "Magic Resist" mi? Diablo (per-element resist) vs LoL (Armor+MR iki eksen) — RIMA roguelite ölçeği için hangisi? True deler. Elementalist switch'i resist'e karşı "doğru elementi seç" oynanışı yaratır mı (istenir mi)?
5. **Renk konvansiyonu doğrula:** Önerilen renkler gerçekten LoL/Diablo/PoE global konvansiyonuyla uyumlu mu? Renk körü erişilebilirlik (Frost #7FE0FF vs Lightning #FFD24A vs cyan ayırt edilebilir mi)? Düzeltme öner.
6. **2-yıl ölçek:** Yeni element/class eklenince taksonomi nasıl büyür? Enum versiyonlama, resist tablosu data-driven mı (ScriptableObject) olmalı?

Türkçe yanıt ver, tam Türkçe karakter ZORUNLU. Kısa-net, madde madde, net tavsiye (survey değil karar).
