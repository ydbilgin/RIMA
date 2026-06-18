using System;
using System.Collections.Generic;
using UnityEngine;

namespace RIMA
{
    /// <summary>
    /// Minimal bilingual localisation service (TR default / EN).
    /// Usage:  Loc.T("menu.start")          → current-language string
    ///         Loc.T("draft.title_room", 3)  → string.Format with args
    /// Language stored in PlayerPrefs("rima.lang", default "tr").
    /// Subscribe to Loc.OnLanguageChanged to rebuild UI when the player
    /// switches language in Settings.
    /// </summary>
    public static class Loc
    {
        // ── Public API ──────────────────────────────────────────────────

        public static event Action OnLanguageChanged;

        public static string CurrentLanguage { get; private set; }

        static Loc()
        {
            CurrentLanguage = PlayerPrefs.GetString("rima.lang", "tr");
        }

        /// <summary>Return the localised string for <paramref name="key"/>.</summary>
        public static string T(string key)
        {
            Dictionary<string, string> table = CurrentLanguage == "en" ? _en : _tr;
            if (table.TryGetValue(key, out string val)) return val;

            // Fallback: try TR, then return key itself so nothing is invisible.
            if (_tr.TryGetValue(key, out string trFallback)) return trFallback;
            Debug.LogWarning($"[Loc] Missing key: '{key}'");
            return key;
        }

        /// <summary>Return the localised string with string.Format args applied.</summary>
        public static string T(string key, params object[] args)
        {
            string raw = T(key);
            try   { return string.Format(raw, args); }
            catch { return raw; }
        }

        /// <summary>Switch language and fire OnLanguageChanged so all subscribed UIs refresh.</summary>
        public static void SetLanguage(string lang)
        {
            string normalized = (lang ?? "tr").ToLowerInvariant();
            if (normalized != "tr" && normalized != "en") normalized = "tr";
            if (CurrentLanguage == normalized) return;

            CurrentLanguage = normalized;
            PlayerPrefs.SetString("rima.lang", normalized);
            PlayerPrefs.Save();
            OnLanguageChanged?.Invoke();
        }

        // ── String tables ───────────────────────────────────────────────

        private static readonly Dictionary<string, string> _tr = new Dictionary<string, string>
        {
            // Ana Menü
            { "menu.title",                 "RIMA" },
            { "menu.subtitle",              "RİFT AVCILARI" },
            { "menu.whisper",               "Yine geldin." },
            { "menu.btn.start",             "BAŞLA" },
            { "menu.btn.settings",          "AYARLAR" },
            { "menu.btn.quit",              "ÇIKIŞ" },

            // Ayarlar
            { "settings.title",             "AYARLAR" },
            { "settings.language",          "DİL" },
            { "settings.lang.tr",           "TÜRKÇE" },
            { "settings.lang.en",           "ENGLISH" },
            { "settings.gameplay",          "OYNANIŞ" },
            { "settings.aim_mode",          "Nişan Modu" },
            { "settings.mouse",             "FARE" },
            { "settings.facing",            "BAKIŞ YÖNÜ" },
            { "settings.dash_mode",         "Dash Modu" },
            { "settings.accessibility",     "ERİŞİLEBİLİRLİK" },
            { "settings.screen_shake",      "Ekran Sarsıntısı" },
            { "settings.hit_stop",          "Vuruş Duraklaması" },
            { "settings.low_hp_vignette",   "Düşük Can Efekti" },
            { "settings.damage_numbers",    "Hasar Sayıları" },
            { "settings.chromatic_aberration", "Kromatik Sapma" },
            { "settings.audio",             "SES" },
            { "settings.master",            "Ana Ses" },
            { "settings.music",             "Müzik" },
            { "settings.sfx",              "Ses Efektleri" },
            { "settings.controls",          "KONTROLLER" },
            { "settings.move",              "Hareket" },
            { "settings.dash",              "Dash" },
            { "settings.attack",            "Saldırı" },
            { "settings.alt_attack",        "Alt Saldırı" },
            { "settings.skill_1",           "Yetenek 1" },
            { "settings.skill_2",           "Yetenek 2" },
            { "settings.skill_3",           "Yetenek 3" },
            { "settings.skill_4",           "Yetenek 4" },
            { "settings.rift_break",        "Rift Kırılması" },
            { "settings.btn.reset",         "KONTROLLERİ SIFIRLA" },
            { "settings.btn.resume",        "DEVAM ET" },
            { "settings.btn.quit_to_menu",  "ANA MENÜYE DÖN" },
            { "settings.on",                "AÇIK" },
            { "settings.off",               "KAPALI" },

            // Karakter Seçim
            { "char_select.title",          "RIMA <color=#B0B3BC>- KARAKTER SEÇ</color>" },
            { "char_select.locked",         "KİLİTLİ" },
            { "char_select.in_development",  "GELİŞTİRME AŞAMASINDA" },
            { "char_select.skills",         "YETENEKLER" },
            { "char_select.skills_soon",    "Yetenekler yakında" },
            { "char_select.btn.select",     "SEÇ" },
            { "char_select.btn.back",       "GERİ" },
            { "char_select.btn.unlock",     "KİLİDİ AÇ — {0}" },
            { "char_select.not_enough_echo","YETERSİZ SHATTERED ECHO" },
            { "char_select.stats.damage",   "HASAR" },
            { "char_select.stats.durability","DAYANIKLILIK" },
            { "char_select.stats.speed",    "HIZ" },
            { "char_select.stats.control",  "KONTROL" },
            { "char_select.stats.difficulty","ZORLUK" },
            { "char_select.full_list",      "TAM LİSTE" },
            { "char_select.unlock_condition","{0} SHATTERED ECHO veya {1}" },

            // Karakter Sayfası
            { "char_sheet.active_kit",      "AKTİF KİT" },
            { "char_sheet.synergies",       "SİNERJİLER" },
            { "char_sheet.dungeon_route",   "KOŞU YOLU" },
            { "char_sheet.active_echoes",   "AKTİF ECHOLAR" },
            { "char_sheet.no_class",        "SINIF YOK" },

            // Bölüm Seçimi / Oda Geçişleri
            { "chamber_select.prompt.attune",    "[G] Bürün — {0}" },
            { "chamber_select.prompt.unlock",    "[G] Kilidi Aç — {0} SHATTERED ECHO" },
            { "chamber_select.prompt.enter_rift","[G] Rift'e Gir" },
            { "chamber_select.dummy_hp",         "KUKLA CANI {0}/{1}" },

            // Savaş
            { "combat.prompt.execute",      "[RMB] İnfaz" },

            // Ödül Toplama
            { "reward.prompt.take",         "[G] Ödülü Al" },

            // Harita
            { "map.title",                  "KOŞU YOLU" },
            { "map.subtitle",               "M ile kapat - cyan = bulunduğun oda" },

            // Yetenek Seçimi
            { "draft.title_room",           "ODA {0} — ÖDÜL SEÇ" },
            { "draft.title_generic",        "ÖDÜL SEÇ" },
            { "draft.subtitle",             "Birini seç — diğerleri kaybolur" },
            { "draft.slot_full",            "SLOT DOLU" },
            { "draft.replace_prompt",       "{0} almak için hangisini bırakmak istiyorsun?" },
            { "draft.btn.skip",             "ATLA — alma" },
            { "draft.gold_title",           "+{0} ALTIN" },
            { "draft.gold_desc",            "Hazinene ekle" },
            { "draft.heal_title",           "+%{0} CAN" },
            { "draft.heal_desc",            "Anında iyileş" },
            { "draft.echo_desc",            "Çağır bir yankısı (C)." },
            { "draft.pairs_with",           "⟂ {0} ile eşleşir" },
            { "draft.btn.select",           "SEÇ" },

            // Ölüm Ekranı
            { "death.quote_1",              "Rift hatırlar. Sen hatırlamayacaksın." },
            { "death.quote_2",              "Bu bir son değil. Sadece durduğun yer." },
            { "death.btn.retry",            "TEKRAR DENE [R]" },
            { "death.btn.main_menu",        "ANA MENÜ" },
            { "death.stats.total_echo",     "TOPLAM: +{0} SHATTERED ECHO" },

            // Zafer Ekranı
            { "victory.title",              "DEMO TAMAMLANDI" },
            { "victory.subtitle",           "Gerçek iniş seni bekliyor." },
            { "victory.teaser",             "Sıradaki echo bekliyor — yeni bir sınıf inişe katılıyor." },
            { "victory.btn.wishlist",       "WISHLIST ON STEAM" },
            { "victory.btn.main_menu",      "ANA MENÜ" },
            { "victory.btn.play_again",     "TEKRAR OYNA" },
            { "victory.stats.summary",      "ODA: {0}    LEŞ: {1}    SÜRE: {2}" },

            // Director Mode
            { "director.spawn.title",       "SPAWN" },
            { "director.spawn.hint",        "Dusman sec, dunya tikla koy, sag tik sil" },
            { "director.spawn.clear",       "TEMIZLE" },
            { "director.spawn.status.loaded", "{0} dusman hazir" },
            { "director.spawn.status.selected", "{0} secildi" },
            { "director.spawn.status.spawned", "{0} yerlestirildi - toplam {1}" },
            { "director.spawn.status.erased", "Silindi - toplam {0}" },
            { "director.spawn.status.erase_none", "Silinecek Director mob yok" },
            { "director.spawn.status.cleared", "Director spawnlari temizlendi" },
            { "director.spawn.status.no_wave", "Encounter wave bulunamadi" },
            { "director.class_skill.title", "CLASS & SKILL" },
            { "director.class_skill.hint", "Sinif degistir, draft skill sec, LMB/RMB basic ata" },
            { "director.class_skill.assign.q", "Q ATA" },
            { "director.class_skill.assign.e", "E ATA" },
            { "director.class_skill.assign.r", "R ATA" },
            { "director.class_skill.assign.f", "F ATA" },
            { "director.class_skill.assign.lmb", "LMB BASIC" },
            { "director.class_skill.assign.rmb", "RMB BASIC" },
            { "director.class_skill.status.class", "{0} aktif" },
            { "director.class_skill.status.selected", "{0} secildi" },
            { "director.class_skill.status.assigned", "{0} -> {1}" },
            { "director.class_skill.status.binding", "{0} basic binding uygulandi" },
            { "director.class_skill.status.no_manager", "PlayerClassManager yok" },
            { "director.class_skill.status.no_draft", "DraftManager yok" },
            { "director.class_skill.status.no_skill_selected", "Once skill sec" },
            { "director.class_skill.status.player_dead", "Oyuncu can kaybetti - sinif degistirilemez" },
            { "director.class_skill.status.skill_missing", "Skill yok: {0}" },
            { "director.class_skill.status.no_skills", "{0} icin aktif skill yok" },
            { "director.class_skill.status.assign_failed", "Skill atanamadi: {0}" },
            { "director.class_skill.status.binding_failed", "Binding olmadi: {0}" },
            { "director.stats.title",       "STATS" },
            { "director.stats.hint",        "Canlı ClassStatRuntime düzenleme" },
            { "director.stats.maxHP",       "MAKS CAN" },
            { "director.stats.physPower",   "FİZİKSEL GÜÇ" },
            { "director.stats.abilityPower","YETENEK GÜCÜ" },
            { "director.stats.attackSpeedMult", "SALDIRI HIZI" },
            { "director.stats.moveSpeed",   "HAREKET HIZI" },
            { "director.stats.debugGlobalDamageMult", "DEBUG HASAR ÇARPANI" },
            { "director.stats.reset",       "RESET" },
            { "director.stats.save",        "SAVE" },
            { "director.stats.export",      "EXPORT" },
            { "director.stats.status.live", "{0} runtime canlı" },
            { "director.stats.status.reset","{0} profil değerlerine döndü" },
            { "director.stats.status.saved","{0} preset kaydedildi" },
            { "director.stats.status.exported","{0} JSON panoya kopyalandı" },
            { "director.stats.status.no_manager","PlayerClassManager yok" },
            { "director.telemetry.title",   "TELEMETRI" },
            { "director.telemetry.hint",    "DPS, TTK, hasar kaynagi ve CSV export" },
            { "director.telemetry.dps",     "DPS 5S" },
            { "director.telemetry.ttk",     "SON TTK" },
            { "director.telemetry.events",  "EVENT" },
            { "director.telemetry.clear",   "TEMIZLE" },
            { "director.telemetry.export",  "CSV EXPORT" },
            { "director.telemetry.status.live", "+{0} hasar kaydedildi ({1})" },
            { "director.telemetry.status.cleared", "Telemetri temizlendi" },
            { "director.telemetry.status.exported", "{0} event CSV panoya kopyalandi" },

            // Kodeks
            { "codex.title",                "YETENEK KODEKSİ" },
            { "codex.empty",                "BU SINIF İÇİN KAYITLI YETENEK YOK" },
            { "codex.coming_soon",          "{0} (YAKINDA)" },
        };

        private static readonly Dictionary<string, string> _en = new Dictionary<string, string>
        {
            // Main Menu
            { "menu.title",                 "RIMA" },
            { "menu.subtitle",              "THE RIFT HUNTERS" },
            { "menu.whisper",               "You've Returned." },
            { "menu.btn.start",             "START" },
            { "menu.btn.settings",          "SETTINGS" },
            { "menu.btn.quit",              "QUIT" },

            // Settings
            { "settings.title",             "SETTINGS" },
            { "settings.language",          "LANGUAGE" },
            { "settings.lang.tr",           "TÜRKÇE" },
            { "settings.lang.en",           "ENGLISH" },
            { "settings.gameplay",          "GAMEPLAY" },
            { "settings.aim_mode",          "Aim Mode" },
            { "settings.mouse",             "MOUSE" },
            { "settings.facing",            "FACING" },
            { "settings.dash_mode",         "Dash Mode" },
            { "settings.accessibility",     "ACCESSIBILITY" },
            { "settings.screen_shake",      "Screen Shake" },
            { "settings.hit_stop",          "Hit Stop" },
            { "settings.low_hp_vignette",   "Low HP Vignette" },
            { "settings.damage_numbers",    "Damage Numbers" },
            { "settings.chromatic_aberration", "Chromatic Aberration" },
            { "settings.audio",             "AUDIO" },
            { "settings.master",            "Master Volume" },
            { "settings.music",             "Music" },
            { "settings.sfx",              "Sound Effects" },
            { "settings.controls",          "CONTROLS" },
            { "settings.move",              "Move" },
            { "settings.dash",              "Dash" },
            { "settings.attack",            "Attack" },
            { "settings.alt_attack",        "Alt Attack" },
            { "settings.skill_1",           "Skill 1" },
            { "settings.skill_2",           "Skill 2" },
            { "settings.skill_3",           "Skill 3" },
            { "settings.skill_4",           "Skill 4" },
            { "settings.rift_break",        "Rift Break" },
            { "settings.btn.reset",         "RESET CONTROLS" },
            { "settings.btn.resume",        "RESUME" },
            { "settings.btn.quit_to_menu",  "QUIT TO MENU" },
            { "settings.on",                "ON" },
            { "settings.off",               "OFF" },

            // Character Select
            { "char_select.title",          "RIMA <color=#B0B3BC>- SELECT CHARACTER</color>" },
            { "char_select.locked",         "LOCKED" },
            { "char_select.in_development",  "IN DEVELOPMENT" },
            { "char_select.skills",         "SKILLS" },
            { "char_select.skills_soon",    "Skills Coming Soon" },
            { "char_select.btn.select",     "SELECT" },
            { "char_select.btn.back",       "BACK" },
            { "char_select.btn.unlock",     "UNLOCK — {0}" },
            { "char_select.not_enough_echo","NOT ENOUGH SHATTERED ECHO" },
            { "char_select.stats.damage",   "DAMAGE" },
            { "char_select.stats.durability","DURABILITY" },
            { "char_select.stats.speed",    "SPEED" },
            { "char_select.stats.control",  "CONTROL" },
            { "char_select.stats.difficulty","DIFFICULTY" },
            { "char_select.full_list",      "FULL LIST" },
            { "char_select.unlock_condition","{0} SHATTERED ECHO or {1}" },

            // Character Sheet
            { "char_sheet.active_kit",      "ACTIVE KIT" },
            { "char_sheet.synergies",       "SYNERGIES" },
            { "char_sheet.dungeon_route",   "RUN PATH" },
            { "char_sheet.active_echoes",   "ACTIVE ECHOES" },
            { "char_sheet.no_class",        "NO CLASS" },

            // Chamber Select / Room Transitions
            { "chamber_select.prompt.attune",    "[G] Attune — {0}" },
            { "chamber_select.prompt.unlock",    "[G] Unlock — {0} SHATTERED ECHO" },
            { "chamber_select.prompt.enter_rift","[G] Enter Rift" },
            { "chamber_select.dummy_hp",         "DUMMY HP {0}/{1}" },

            // Combat
            { "combat.prompt.execute",      "[RMB] Execute" },

            // Reward Pickup
            { "reward.prompt.take",         "[G] Take Reward" },

            // Map
            { "map.title",                  "RUN PATH" },
            { "map.subtitle",               "Press M to close - cyan = current chamber" },

            // Skill Draft
            { "draft.title_room",           "CHAMBER {0} — CHOOSE REWARD" },
            { "draft.title_generic",        "CHOOSE REWARD" },
            { "draft.subtitle",             "Choose One — The Rest Will Vanish" },
            { "draft.slot_full",            "SLOT FULL" },
            { "draft.replace_prompt",       "Which one to replace to take {0}?" },
            { "draft.btn.skip",             "SKIP — Do Not Take" },
            { "draft.gold_title",           "+{0} GOLD" },
            { "draft.gold_desc",            "Add to hoard" },
            { "draft.heal_title",           "+%{0} HP" },
            { "draft.heal_desc",            "Heal instantly" },
            { "draft.echo_desc",            "Summon an echo (C)." },
            { "draft.pairs_with",           "⟂ Pairs With {0}" },
            { "draft.btn.select",           "SELECT" },

            // Death Screen
            { "death.quote_1",              "The Rift Remembers. You Won't." },
            { "death.quote_2",              "Not an Ending. Just a Place Where You Stopped." },
            { "death.btn.retry",            "RETRY [R]" },
            { "death.btn.main_menu",        "MAIN MENU" },
            { "death.stats.total_echo",     "TOTAL: +{0} SHATTERED ECHO" },

            // Victory Screen
            { "victory.title",              "DEMO COMPLETE" },
            { "victory.subtitle",           "The Full Descent Awaits." },
            { "victory.teaser",             "Next Echo Awaits — a New Class Joins the Descent." },
            { "victory.btn.wishlist",       "WISHLIST ON STEAM" },
            { "victory.btn.main_menu",      "MAIN MENU" },
            { "victory.btn.play_again",     "PLAY AGAIN" },
            { "victory.stats.summary",      "CHAMBER: {0}    KILLS: {1}    TIME: {2}" },

            // Director Mode
            { "director.spawn.title",       "SPAWN" },
            { "director.spawn.hint",        "Pick an enemy, click world to place, right-click to erase" },
            { "director.spawn.clear",       "CLEAR" },
            { "director.spawn.status.loaded", "{0} enemies ready" },
            { "director.spawn.status.selected", "{0} selected" },
            { "director.spawn.status.spawned", "{0} placed - total {1}" },
            { "director.spawn.status.erased", "Erased - total {0}" },
            { "director.spawn.status.erase_none", "No Director mob under cursor" },
            { "director.spawn.status.cleared", "Director spawns cleared" },
            { "director.spawn.status.no_wave", "Encounter wave missing" },
            { "director.class_skill.title", "CLASS & SKILL" },
            { "director.class_skill.hint", "Swap class, pick draft skill, assign LMB/RMB basic" },
            { "director.class_skill.assign.q", "ASSIGN Q" },
            { "director.class_skill.assign.e", "ASSIGN E" },
            { "director.class_skill.assign.r", "ASSIGN R" },
            { "director.class_skill.assign.f", "ASSIGN F" },
            { "director.class_skill.assign.lmb", "LMB BASIC" },
            { "director.class_skill.assign.rmb", "RMB BASIC" },
            { "director.class_skill.status.class", "{0} active" },
            { "director.class_skill.status.selected", "{0} selected" },
            { "director.class_skill.status.assigned", "{0} -> {1}" },
            { "director.class_skill.status.binding", "{0} basic binding applied" },
            { "director.class_skill.status.no_manager", "PlayerClassManager missing" },
            { "director.class_skill.status.no_draft", "DraftManager missing" },
            { "director.class_skill.status.no_skill_selected", "Select a skill first" },
            { "director.class_skill.status.player_dead", "Player is dead - class switch blocked" },
            { "director.class_skill.status.skill_missing", "Skill missing: {0}" },
            { "director.class_skill.status.no_skills", "No active skills for {0}" },
            { "director.class_skill.status.assign_failed", "Skill assign failed: {0}" },
            { "director.class_skill.status.binding_failed", "Binding failed: {0}" },
            { "director.stats.title",       "STATS" },
            { "director.stats.hint",        "Live ClassStatRuntime editing" },
            { "director.stats.maxHP",       "MAX HP" },
            { "director.stats.physPower",   "PHYS POWER" },
            { "director.stats.abilityPower","ABILITY POWER" },
            { "director.stats.attackSpeedMult", "ATTACK SPEED" },
            { "director.stats.moveSpeed",   "MOVE SPEED" },
            { "director.stats.debugGlobalDamageMult", "DEBUG DAMAGE MULT" },
            { "director.stats.reset",       "RESET" },
            { "director.stats.save",        "SAVE" },
            { "director.stats.export",      "EXPORT" },
            { "director.stats.status.live", "{0} runtime live" },
            { "director.stats.status.reset","{0} reset to profile" },
            { "director.stats.status.saved","{0} preset saved" },
            { "director.stats.status.exported","{0} JSON copied to clipboard" },
            { "director.stats.status.no_manager","PlayerClassManager missing" },
            { "director.telemetry.title",   "TELEMETRY" },
            { "director.telemetry.hint",    "DPS, TTK, damage source, and CSV export" },
            { "director.telemetry.dps",     "DPS 5S" },
            { "director.telemetry.ttk",     "LAST TTK" },
            { "director.telemetry.events",  "EVENTS" },
            { "director.telemetry.clear",   "CLEAR" },
            { "director.telemetry.export",  "CSV EXPORT" },
            { "director.telemetry.status.live", "+{0} damage recorded ({1})" },
            { "director.telemetry.status.cleared", "Telemetry cleared" },
            { "director.telemetry.status.exported", "{0} events copied as CSV" },

            // Codex
            { "codex.title",                "SKILL CODEX" },
            { "codex.empty",                "NO SKILLS RECORDED FOR THIS CLASS" },
            { "codex.coming_soon",          "{0} (COMING SOON)" },
        };
    }
}
