# MODEL_ROUTING.md
> **Ne zaman yükle:** Hangi AI aracını kullanacağına karar verirken.
> **Ne zaman yükleme:** İş yapılırken — bu bir karar belgesi.

---

## VARSAYILAN: SONNET

Aksini belirtmediğin sürece Claude Code Sonnet kullan.

---

## MODEL KARAR TABLOSU

| İş | Model | Araç | Etiket |
|---|---|---|---|
| Sprite/animasyon üretimi (PixelLab) | — | PixelLab MCP | SIMPLE |
| Unity sahne düzenleme, component atama | Sonnet | Antigravity / Unity MCP | SIMPLE |
| Script yazma (basit: getter/setter, UI bind) | Sonnet | Antigravity / Kiro | SIMPLE |
| Tilemap boyama, prefab kurulumu | Sonnet | Antigravity | SIMPLE |
| Animator Controller bağlama | Sonnet | Antigravity | SIMPLE |
| Script yazma (sistem: state machine, manager) | Sonnet | Claude Code | ADVANCED |
| Hata ayıklama (runtime exception, logic bug) | Sonnet | Claude Code | ADVANCED |
| Dokümantasyon yazma / güncelleme | Sonnet | Claude Code | ADVANCED |
| PixelLab prompt optimizasyonu | Sonnet | Claude Code | ADVANCED |
| Sistem mimarisi kararı | **Opus** | Claude Code | CRITICAL |
| Cross-class sinerji tasarımı | **Opus** | Claude Code | CRITICAL |
| Boss mekanik tasarımı | **Opus** | Claude Code | CRITICAL |
| Skill balans / progression eğrisi | **Opus** | Claude Code | CRITICAL |
| Yeni bir faza geçiş planı | **Opus** | Claude Code | CRITICAL |

---

## AJAN KARAR AĞACI

```
İş ne kadar öngörülebilir?
├── Tamamen öngörülebilir, mekanik → Antigravity veya Kiro
│   Örnekler: component ekle, sahneye nesne koy, script bağla
│
├── Kısmen öngörülebilir, ama bağlam lazım → Sonnet (Claude Code)
│   Örnekler: yeni sistem yaz, hata ayıkla, dokuman güncelle
│
└── Yaratıcı karar, trade-off, mimari → Opus (Claude Code)
    Örnekler: sistemi nasıl tasarlayalım, bu mekanik eğlenceli mi
```

---

## OPUS NE ZAMAN KULLANMA

- Rutin kod yazarken
- Bilinen bir sistemi genişletirken
- Sprite üretimi / asset pipeline'da
- Belgeler okunurken / özetlenirken
- Antigravity/Kiro'ya talimat yazarken

Opus'u "daha iyi sonuç için" otomatik olarak seçme. Zor ve kalite-kritik işler için sakla.

---

## KIRO vs ANTİGRAVİTY

| | Kiro | Antigravity |
|---|---|---|
| Bağlam | Sadece görev dosyasını okur | Proje genelini görebilir |
| Güçlü olduğu yer | Bağımsız, net tanımlı görevler | Unity Editor içi işler |
| Zayıf olduğu yer | Mimari karar, tasarım değişikliği | Uzun kodlama oturumları |
| Görev formatı | `AGENTS.md` → Kiro bölümü | `AGENTS.md` → Antigravity bölümü |
