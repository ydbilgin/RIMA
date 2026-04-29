"""
RIMA — SFX Generator
====================
Tüm Faz 1 ses efektlerini retro chiptune stiliyle üretir.
Square wave / sawtooth / sine / noise + ADSR envelope + frekans sweep.

Kurulum (1 kez):
    pip install numpy scipy

Kullanım:
    cd "F:\\Antigravity Projeler\\2d roguelite"
    python tools/sfx_generator.py

Çıktı:
    RIMA/Assets/Audio/SFX/*.wav   ← Unity'e direkt import edilebilir
"""

import numpy as np
from scipy.io import wavfile
import os
import sys

# ── Gereksinim kontrolü ──────────────────────────────────────────────────────
try:
    import numpy as np
    from scipy.io import wavfile
except ImportError:
    print("HATA: numpy ve scipy gerekli.")
    print("  pip install numpy scipy")
    sys.exit(1)

# ── Sabitler ─────────────────────────────────────────────────────────────────
SR = 44100   # sample rate
OUT_DIR = os.path.join(os.path.dirname(__file__),
                       "..", "RIMA", "Assets", "Audio", "SFX")

# ── Temel dalga üreticiler ───────────────────────────────────────────────────

def t_arr(duration):
    return np.linspace(0, duration, int(SR * duration), endpoint=False)

def square(freq, duration, duty=0.5):
    t = t_arr(duration)
    phase = (t * freq) % 1.0
    return np.where(phase < duty, 1.0, -1.0).astype(np.float32)

def sawtooth(freq, duration):
    t = t_arr(duration)
    return (2 * ((t * freq) % 1.0) - 1).astype(np.float32)

def triangle(freq, duration):
    t = t_arr(duration)
    phase = (t * freq) % 1.0
    return (2 * np.abs(2 * phase - 1) - 1).astype(np.float32)

def sine(freq, duration):
    t = t_arr(duration)
    return np.sin(2 * np.pi * freq * t).astype(np.float32)

def noise(duration):
    return np.random.uniform(-1, 1, int(SR * duration)).astype(np.float32)

def sweep(f_start, f_end, duration, wave='square', duty=0.5):
    """Frekans glide — pitch up/down efekti"""
    n = int(SR * duration)
    freqs = np.linspace(f_start, f_end, n)
    phase = np.cumsum(freqs / SR)
    if wave == 'square':
        raw = np.sign(np.sin(2 * np.pi * phase))
    elif wave == 'sine':
        raw = np.sin(2 * np.pi * phase)
    elif wave == 'sawtooth':
        raw = 2 * (phase % 1.0) - 1
    elif wave == 'triangle':
        raw = 2 * np.abs(2 * (phase % 1.0) - 1) - 1
    else:
        raw = np.sign(np.sin(2 * np.pi * phase))
    return raw.astype(np.float32)

# ── ADSR envelope ─────────────────────────────────────────────────────────────

def adsr(wave, attack, decay, sustain_level, release):
    """
    wave          : numpy array
    attack/decay/release : saniye cinsinden
    sustain_level : 0.0 – 1.0
    """
    n = len(wave)
    a = int(attack  * SR)
    d = int(decay   * SR)
    r = int(release * SR)
    s = max(0, n - a - d - r)

    env = np.concatenate([
        np.linspace(0.0, 1.0, a)             if a > 0 else np.array([]),
        np.linspace(1.0, sustain_level, d)   if d > 0 else np.array([]),
        np.full(s, sustain_level)            if s > 0 else np.array([]),
        np.linspace(sustain_level, 0.0, r)   if r > 0 else np.array([]),
    ]).astype(np.float32)

    # Uzunluk eşitle
    if len(env) < n:
        env = np.pad(env, (0, n - len(env)))
    env = env[:n]
    return wave * env

# ── Kaydet ───────────────────────────────────────────────────────────────────

def save(name, wave, volume=0.75):
    wave = wave * volume
    wave = np.clip(wave, -1.0, 1.0)
    data = (wave * 32767).astype(np.int16)
    path = os.path.join(OUT_DIR, name)
    wavfile.write(path, SR, data)
    print(f"  [OK] {name}")

def mix(*waves):
    """Birden fazla dalgayı karıştır, normalize et"""
    max_len = max(len(w) for w in waves)
    result = np.zeros(max_len, dtype=np.float32)
    for w in waves:
        result[:len(w)] += w
    peak = np.max(np.abs(result))
    if peak > 0:
        result /= peak
    return result

def concat(*waves):
    """Dalgaları arka arkaya ekle"""
    return np.concatenate(waves).astype(np.float32)

# ═══════════════════════════════════════════════════════════════════════════════
# SOUND DEFINITIONS
# ═══════════════════════════════════════════════════════════════════════════════

def make_all():
    os.makedirs(OUT_DIR, exist_ok=True)
    print(f"\nÇıktı klasörü: {os.path.abspath(OUT_DIR)}\n")

    # ── SALDIRI / VURUŞ ───────────────────────────────────────────────────────

    # sfx_hit_light — Warblade temel LMB saldırısı
    # Kısa, keskin darbe: noise burst + aşağı sweep
    w = mix(
        adsr(sweep(350, 80, 0.10, 'square'), 0.002, 0.03, 0.2, 0.07),
        adsr(noise(0.10) * 0.4,              0.001, 0.02, 0.1, 0.07),
    )
    save("sfx_hit_light.wav", w)

    # sfx_cleave — Yatay kılıç savurma (Cleave skill)
    # Sawtooth sweep + geniş impact
    part1 = adsr(sweep(180, 60, 0.12, 'sawtooth'), 0.005, 0.05, 0.3, 0.06)
    part2 = adsr(mix(noise(0.08) * 0.6, square(110, 0.08) * 0.5), 0.001, 0.04, 0.2, 0.04)
    w = mix(part1, np.pad(part2, (int(0.06*SR), 0))[:len(part1)])
    save("sfx_cleave.wav", w)

    # sfx_ground_slam — İki elle zemine vurma (Attack 2)
    # Alçak frekans thump + geniş noise burst
    w = mix(
        adsr(sweep(120, 30, 0.20, 'square'),  0.003, 0.08, 0.1, 0.11),
        adsr(noise(0.20) * 0.7,               0.001, 0.04, 0.2, 0.15),
        adsr(sine(60, 0.20),                  0.001, 0.05, 0.1, 0.14),
    )
    save("sfx_ground_slam.wav", w)

    # sfx_iron_charge — Öne doğru hamle (Iron Charge skill)
    # Whoosh (sweep up) + darbe sonu
    whoosh = adsr(sweep(80, 280, 0.15, 'square'), 0.01, 0.06, 0.4, 0.08)
    impact = adsr(mix(noise(0.08) * 0.8, sweep(300, 60, 0.08, 'square')), 0.001, 0.03, 0.2, 0.05)
    w = concat(whoosh, impact)
    save("sfx_iron_charge.wav", w)

    # ── DASH ──────────────────────────────────────────────────────────────────

    # sfx_dash — Hızlı öne atılma
    # Yukarı sine sweep, çok kısa
    w = adsr(sweep(160, 500, 0.12, 'sine'), 0.005, 0.04, 0.3, 0.075)
    w += adsr(sweep(200, 600, 0.12, 'square') * 0.3, 0.005, 0.04, 0.2, 0.075)
    peak = np.max(np.abs(w)); w = w / peak if peak > 0 else w
    save("sfx_dash.wav", w)

    # ── OYUNCU HASAR / ÖLÜM ───────────────────────────────────────────────────

    # sfx_player_hurt — Oyuncu hasar aldı
    # Orta frekanslı darbe, kısa ve keskin
    w = mix(
        adsr(sweep(400, 150, 0.14, 'square'), 0.001, 0.03, 0.3, 0.11),
        adsr(noise(0.14) * 0.5,              0.001, 0.02, 0.1, 0.12),
    )
    save("sfx_player_hurt.wav", w)

    # sfx_player_death — Oyuncu öldü
    # Aşağı uzun sweep + dağılan noise
    w = mix(
        adsr(sweep(300, 40, 0.60, 'square'),    0.01, 0.15, 0.4, 0.44),
        adsr(noise(0.60) * 0.5,                  0.01, 0.10, 0.2, 0.49),
        adsr(sweep(200, 30, 0.60, 'sawtooth'),   0.01, 0.15, 0.3, 0.44),
    )
    save("sfx_player_death.wav", w)

    # ── DÜŞMAN HASAR / ÖLÜM ───────────────────────────────────────────────────

    # sfx_enemy_hit — Düşman hasar aldı (genel)
    # Biraz daha yüksek pitch, player_hurt'tan ayrışsın
    w = mix(
        adsr(sweep(500, 200, 0.10, 'square'), 0.001, 0.025, 0.2, 0.074),
        adsr(noise(0.10) * 0.4,              0.001, 0.015, 0.1, 0.084),
    )
    save("sfx_enemy_hit.wav", w)

    # sfx_enemy_death_shard — Shard Walker ölümü (taşlar patlıyor)
    # Noise burst → çınlayan parçacıklar
    burst = adsr(noise(0.08) * 1.0, 0.001, 0.02, 0.5, 0.059)
    tinkles = sum(
        adsr(sine(f, 0.25) * 0.3, 0.001, 0.05, 0.2, 0.2)
        for f in [880, 1100, 1320, 660]
    )
    tinkles_padded = np.pad(tinkles, (int(0.05*SR), 0))
    w = mix(burst, tinkles_padded[:max(len(burst), len(tinkles_padded))])
    save("sfx_enemy_death_shard.wav", w)

    # sfx_enemy_death_void — Void Thrall ölümü (void parçalanıyor)
    # Alçak, titreşimli — void purple hissi
    w = mix(
        adsr(sweep(220, 40, 0.35, 'square'),   0.005, 0.10, 0.3, 0.245),
        adsr(sweep(330, 60, 0.35, 'sawtooth'), 0.005, 0.10, 0.2, 0.245),
        adsr(noise(0.35) * 0.4,                0.005, 0.05, 0.3, 0.295),
    )
    save("sfx_enemy_death_void.wav", w)

    # ── COLD BLUE ENERGY ──────────────────────────────────────────────────────

    # sfx_cold_pulse — Soğuk mavi enerji nabzı
    # Yüksek, kristal sine — Iron Warden idle, hitflash
    freqs = [1200, 1500, 1800]
    waves = [adsr(sine(f, 0.30) * 0.4, 0.02, 0.05, 0.5, 0.23) for f in freqs]
    w = mix(*waves)
    save("sfx_cold_pulse.wav", w)

    # ── BOSS ──────────────────────────────────────────────────────────────────

    # sfx_boss_intro — Boss karşılaşması başlıyor
    # Alçak gümbürtü büyüyor + impact
    rumble  = adsr(sweep(40, 80, 0.60, 'sine') * 0.8,    0.05, 0.20, 0.7, 0.35)
    build   = adsr(sweep(60, 120, 0.60, 'square') * 0.4, 0.05, 0.20, 0.5, 0.35)
    impact  = adsr(mix(noise(0.20)*0.9, sweep(200,30,0.20,'square')*0.7), 0.001, 0.04, 0.3, 0.16)
    silence = np.zeros(int(0.05*SR), dtype=np.float32)
    w = mix(
        np.pad(rumble, (0, int(0.25*SR))),
        np.pad(build,  (0, int(0.25*SR))),
        np.pad(concat(silence, impact), (int(0.60*SR), 0))[:int(0.85*SR)],
    )
    save("sfx_boss_intro.wav", w)

    # sfx_boss_death — Boss öldü
    # Uzun, yavaş çöküş → patlama → yankılanan sessizlik
    collapse = adsr(sweep(150, 25, 0.80, 'square'), 0.01, 0.20, 0.4, 0.59)
    explosion = adsr(mix(noise(0.30)*0.9, sweep(300,20,0.30,'sawtooth')*0.6), 0.001, 0.05, 0.4, 0.249)
    ring = adsr(sum(sine(f, 0.50)*0.2 for f in [440, 660, 880]), 0.01, 0.10, 0.4, 0.39)
    exp_padded = np.pad(explosion, (int(0.70*SR), 0))
    ring_padded = np.pad(ring, (int(0.90*SR), 0))
    total_len = int(1.40*SR)
    def fit(w):
        if len(w) < total_len: return np.pad(w, (0, total_len-len(w)))
        return w[:total_len]
    w = mix(fit(collapse), fit(exp_padded), fit(ring_padded))
    save("sfx_boss_death.wav", w)

    # ── ODA SİSTEMİ ───────────────────────────────────────────────────────────

    # sfx_room_clear — Oda temizlendi, kapı açılıyor
    # Yükselen arpeggio (C minör: C E♭ G)
    notes = [261, 311, 392, 523]  # C4 E♭4 G4 C5
    parts = []
    for i, freq in enumerate(notes):
        w_note = adsr(square(freq, 0.18, duty=0.4), 0.005, 0.06, 0.4, 0.115)
        parts.append(np.pad(w_note, (int(i * 0.12 * SR), 0)))
    max_len = max(len(p) for p in parts)
    result = np.zeros(max_len, dtype=np.float32)
    for p in parts:
        result[:len(p)] += p
    peak = np.max(np.abs(result)); result = result / peak if peak > 0 else result
    save("sfx_room_clear.wav", result)

    # ── SKILL / UI ────────────────────────────────────────────────────────────

    # sfx_skill_offer — Skill draft paneli açıldı
    # Parlak, ilgi çekici kısa arpeggio (E♭ → G → B♭)
    notes = [311, 392, 466]  # E♭4 G4 B♭4
    parts = []
    for i, freq in enumerate(notes):
        w_note = adsr(square(freq, 0.14, duty=0.45), 0.003, 0.05, 0.4, 0.087)
        parts.append(np.pad(w_note, (int(i * 0.09 * SR), 0)))
    max_len = max(len(p) for p in parts)
    result = np.zeros(max_len, dtype=np.float32)
    for p in parts:
        result[:len(p)] += p
    peak = np.max(np.abs(result)); result = result / peak if peak > 0 else result
    save("sfx_skill_offer.wav", result)

    # sfx_skill_select — Skill seçildi (onay)
    # İki nota: alçak + yüksek, tat bırakır
    lo = adsr(square(392, 0.12, 0.4), 0.003, 0.04, 0.5, 0.077)
    hi = adsr(square(784, 0.18, 0.4), 0.003, 0.04, 0.5, 0.137)
    hi_padded = np.pad(hi, (int(0.10*SR), 0))
    w = mix(
        np.pad(lo, (0, len(hi_padded) - len(lo))),
        hi_padded
    )
    save("sfx_skill_select.wav", w)

    # sfx_ui_click — Genel UI tıklama
    w = adsr(square(440, 0.06, 0.45), 0.001, 0.02, 0.3, 0.039)
    save("sfx_ui_click.wav", w)

    # sfx_ui_hover — UI üzerine gelince
    w = adsr(square(330, 0.04, 0.45), 0.001, 0.015, 0.2, 0.024)
    save("sfx_ui_hover.wav", w)

    # sfx_ui_back — Geri / kapat
    hi = adsr(square(440, 0.07, 0.45), 0.001, 0.02, 0.3, 0.049)
    lo = adsr(square(330, 0.07, 0.45), 0.001, 0.02, 0.2, 0.049)
    lo_padded = np.pad(lo, (int(0.06*SR), 0))
    w = mix(
        np.pad(hi, (0, len(lo_padded) - len(hi))),
        lo_padded
    )
    save("sfx_ui_back.wav", w)

    # ── RESOURCE SİSTEMLERİ ───────────────────────────────────────────────────

    # sfx_rage_full — Rage bar doldu
    # Tiz, uyarıcı ping + kısa rezonans
    w = mix(
        adsr(sine(1200, 0.25),          0.01, 0.03, 0.6, 0.21),
        adsr(square(600, 0.25, 0.45),   0.01, 0.04, 0.4, 0.21),
    )
    save("sfx_rage_full.wav", w)

    # sfx_hp_low — HP düşük uyarı tik (loop olarak çalacak)
    # Kısa, karanlık, huzursuz edici
    w = adsr(square(220, 0.08, 0.5), 0.001, 0.01, 0.6, 0.069)
    w += adsr(noise(0.08)*0.2,       0.001, 0.01, 0.2, 0.069)
    peak = np.max(np.abs(w)); w = w / peak if peak > 0 else w
    save("sfx_hp_low.wav", w)

    # ── ÖZET ─────────────────────────────────────────────────────────────────
    files = [f for f in os.listdir(OUT_DIR) if f.endswith('.wav')]
    print(f"\n[DONE] {len(files)} ses dosyasi uretildi -> {os.path.abspath(OUT_DIR)}")
    print("\nUnity'e import icin:")
    print("  Project penceresi -> Assets/Audio/SFX klasorune surukle & birak")
    print("  VEYA Unity otomatik algilar (klasor zaten Assets altinda)")


if __name__ == "__main__":
    make_all()
