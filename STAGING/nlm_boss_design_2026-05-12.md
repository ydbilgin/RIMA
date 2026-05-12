{
  "value": {
    "answer": "RIMA projesindeki güncel (LOCKED) tasarım belgelerine göre, oyunun boss tasarımları çok fazlı, taktiksel pozisyonlamaya dayalı ve oyuncunun yetenek (build) sınırlarını test eden sistemler üzerine kuruludur [1]. Son alınan mimari kararlarla (S59) birlikte boss boyutları, posture (duruş) sistemleri ve telegraph (saldırı ön uyarısı) kuralları kesinleşmiştir. \n\nİstediğiniz detaylara göre RIMA'nın güncel boss tasarım spesifikasyonları aşağıdadır:\n\n### 1. Global Boss Kuralları ve Sistemler\n\n**Telegraph ve Zamanlama Kuralı:**\nOyundaki tüm boss saldırıları için katı bir okunabilirlik kuralı vardır: **Maksimum 1.5 saniye telegraph (tell), minimum 0.4 saniye kaçınma/tepki penceresi (window)** [2]. Bu kural, ani ve haksız hasar almayı engeller, oyuncunun reflekslerini değil, doğru okuma ve pozisyon alma yeteneğini test eder [2].\n\n**Posture (Duruş/Stagger) Sistemi:**\nBoss'lar standart moblar gibi her vuruşta irkilmez (stagger olmaz), ancak özel bir \"Posture\" barına sahiptirler. Posture değerleri fazlara göre ölçeklenir:\n*   **Faz 1:** 700 Posture\n*   **Faz 2:** 850 Posture\n*   **Faz 3 (ve üzeri):** 1000 Posture [3]\nBu bar kırıldığında boss'lar mikro-stagger (0.3-0.4s) yaşar ve hasara açık \"Break Window\" penceresi yaratılır [4].\n\n**Fracture Echoes Sistemi (Boss Varyasyonları):**\nOyunun tekrar oynanabilirliğini (replayability) sağlayan \"Fracture Echoes\" sistemi, her boss'un 5 farklı zorluk veya varyasyon Echo'suna sahip olmasını sağlar [5, 6]. Bir boss her kesildiğinde bir sonraki run'da yeni bir Echo açılır (max 5). Bu 5 Echo şu kategorilerde boss'u değiştirir:\n1.  **Arena:** Arenanın düzenini veya zemin hazard'larını (tehlikelerini) değiştirir.\n2.  **Adaptif:** Oyuncunun kullandığı hasar tipine veya yeteneklerine direnç geliştirir.\n3.  **Minion (Summon):** Savaşa ekstra minyonlar veya yan tehditler ekler.\n4.  **Mekanik:** Boss'un mevcut yeteneklerine yeni özellikler (örn. daha geniş alan veya ek debuff) ekler.\n5.  **Yeni Faz (Echo 5):** 5. Echo açıldığında boss'a tamamen yepyeni bir faz (Phase) eklenir [6].\n\n---\n\n### 2. Act 1 Boss: The Penitent Sovereign\nBölgenin eski koruyucusu olan ve kendisini zincirlerle hapsetmiş bu boss, oyuncuya kaçınma disiplinini ve arena kontrolünü öğretir [7]. Savaş 3 fazdan oluşur [8].\n\n*   **Arena Etkileşimi:** Savaş, Act 1'in mimarisine uygun dairesel/sekizgen bir ritüel platformunda başlar. 2 adet net dash koridoru vardır [8]. Faz 2'ye geçildiğinde arenanın tam merkezinde bir \"Rift Tear\" hazard'ı belirir ve dash koridorlarını tek yönlü hale getirerek arenayı daraltır [8, 9].\n*   **Faz 1 (\"Zincirin Altında\" - %100 -> %66):**\n    *   *Chain Whip:* 6m düz çizgi (0.8s tell, 1.0s window).\n    *   *Penitent Surge:* Yere yumruk atarak 4m AoE itme + hasar (1.2s tell, 1.5s window).\n    *   *Shackle Cast:* 8m menzilli zincir, hedefi %50 yavaşlatır (1.0s tell, 1.2s window) [9].\n*   **Faz 2 (\"Kırılan Zincir\" - %66 -> %33):** Boss'un hızı +%30 artar. Zincirleri kopar [9].\n    *   *Fracture Strike:* 3 ardışık vuruş kombo (0.5s dash tell, 0.8s window).\n    *   *Chain Detonation:* Zemine saplanan zincir parçaları 2.5s sonra 2m yarıçapında patlar (1.0s tell) [10].\n*   **Faz 3 (\"Sovereign Awakened\" - %33 -> %0):** Merkezdeki Rift Tear tehlikesine rağmen güvenli alan daralır.\n    *   *Fracture Charge:* Arena boyunca yıkıcı düz çizgi dash (0.6s tell, 1.5s window).\n    *   *Sovereign's Wrath:* 2m'lik hareketli bir güvenli bölge hariç TÜM arenaya hasar verir (1.5s tell, 2.0s recovery) [11].\n\n---\n\n### 3. Act 2 Boss: The Echo Twin\nOyuncunun çift sınıf (dual-class) kimliğini kazanmasını yansıtan tematik bir savaştır. İki varlığın tek bir bedeni paylaştığı bu savaş 2 fazdır [12].\n\n*   **Arena ve Minion Mekaniği:** Run'da seçtiğiniz primary class neyse (Warblade, Elementalist vb.), Echo Twin ilk fazda o sınıfın agresif bir yansıması olarak savaşır [13].\n*   **Faz 1 (\"Birinci Kimlik\" - %100 -> %40):** Örneğin melee (Warblade) varyantıysa;\n    *   *Mirror Slash:* Oyuncunun son kullandığı yeteneğin ayna kopyasını atar.\n    *   *Identity Strike:* 5m dash ve anlık hasar.\n    *   *Echo Barrier:* 3s süren hasar azaltıcı kalkan.\n    *   *Twin Pulse:* 2m AoE patlaması ve itme [13, 14].\n*   **Faz 2 (\"İkinci Kimlik\" - %40 -> %0):** Boss ikiye yırtılır gibi olur ve ikinci kimlik öne çıkar. Düşman tamamen farklı bir yetenek setine geçer.\n    *   *Duality Surge:* İki kimliğin gücünü birleştiren devasa AoE. (Oyuncunun Cross-class ultimate kullanmasını teşvik eder).\n    *   *Phase Shift:* Birkaç saniye görünmez olup oyuncunun arkasından çıkar.\n    *   *Echo Cascade:* Faz 1'deki saldırıları 2x hızda tekrarlar.\n    *   *Twin Collapse:* 3s boyunca enerji toplayıp devasa patlama yaratır [14, 15].\n\n---\n\n### 4. Act 3 Boss: The Fracture Sovereign\nDünya kırılmasının (The Fracturing) merkezi olan ve bilince kavuşmuş bir \"alan\" anomalisidir. Bu boss stagger'lanamaz ve 3 fazlıdır [15, 16].\n\n*   **Arena Etkileşimi ve Summonlar:** Faz 2'de zemin çatlar ve 4 köşede \"Fracture Node\" adı verilen yapılar (summonlar) belirir. Oyuncu bu node'ları yok ederek boss'a %5 hasar verebilir [16, 17]. Faz 3'te ise arena tamamen değişir; fizik kuralları bozulur ve boşlukta (void) yüzen platform adacıklarına dönüşür [17].\n*   **Faz 1 (\"Yara Açıldı\" - %100 -> %60):**\n    *   *Fracture Beam:* Yavaş dönen lazer çizgisi.\n    *   *Void Shard:* 3 yöne fırlayan mermiler.\n    *   *Sovereign Step:* Rastgele konuma ışınlanma.\n    *   *Gravity Pull:* Oyuncuyu kenara çekip bırakma [16].\n*   **Faz 2 (\"Çevre Uyanıyor\" - %60 -> %30):** Arena küçülür, hazard zone genişler.\n    *   *Node Pulse:* Çağrılan node'lar her 15 saniyede bir patlar.\n    *   *Fracture Wave:* Zemin boyunca yayılan atlanabilir şok dalgası.\n    *   *Shatter Field:* Arenada dönen ve yavaşlatan orb'lar.\n    *   *Sovereign Gaze:* Bloklanabilen, 3s işaretli büyük hasar [17].\n*   **Faz 3 (\"Tam Kırılma\" - %30 -> %0):** Platform mekanikleri devreye girer.\n    *   *Fracture Surge:* Önceki saldırıların 2x hızda sırayla kullanımı.\n    *   *Void Collapse:* Arenanın bir bölümü çöker, platformlar kayar.\n    *   *Sovereign's Final Form:* Tüm alana hasar veren 5s hazırlık (doğru platforma kaçmak zorunludur).\n    *   *Echo of Pain:* Oyuncunun aldığı son büyük hasarı yansıtır [18].\n\n---\n\n### 5. Final Boss: The Architect (Canavar Form Kararı)\nThe Fracturing'i bilerek yaratan, her şeyin kaynağı olan bu varlık, oyuncunun oyun boyunca öğrendiği tüm dersleri sınayan 4 fazlı epik bir savaştır [19].\n\n**Boyut ve Form Revizyonu (LOCKED):** Eski tasarımda olan \"canavar formunda 512px canvas kullanımı\" veya boyutunun devasa değişmesi fikri **REVOKED (İptal)** edilmiştir. Yeni mimari kararlara göre Final Boss **256x256 native canvas'ta ve PPU=64** olarak üretilir [3, 20]. Boyut fiziksel olarak insan formuna küçülmez; devasa hissi PPU manipülasyonu ile (sahnede oyuncunun ~2.5 katı) sağlanır ve faz değişimleri görsel boyut küçülmesiyle değil, VFX, müzik değişimi ve mekanik yoğunluk ile verilir [3, 20].\n\n*   **Arena Etkileşimi:** Savaş boyunca arena geçmiş boss'ların mekaniklerine göre şekillenir. Faz 3'te karanlığa gömülen arena \"void mimarisi\" yaratır. Faz 4'te arena patlayarak karanlık dağılır [21, 22].\n*   **Faz 1 (\"Tanıdık\" - %100 -> %75):** Act 1 boss'unun hareketlerini kopyalar. Oyuncunun ne kadar güçlendiğini hissetmesi için tasarlanmıştır [19, 23].\n*   **Faz 2 (\"Bozulan\" - %75 -> %45):** Form glitch'lenmeye (bozulmaya) başlar.\n    *   *Fractured Mirror:* Oyuncunun son 3 skill'ini kopyalar.\n    *   *Glitch Step:* 4-5 hızlı teleport sonrası saldırı.\n    *   *Architect's Echo:* Rastgele geçmiş boss saldırıları.\n    *   *Identity Collapse:* Tüm arena 3s boyunca tersine (inverted) döner [23].\n*   **Faz 3 (\"Boşluk\" - %45 -> %20):** Müzik kesilir, karanlıkta platformlar belirir.\n    *   *Void Architecture:* Yeni tuzaklı veya güvenli platformlar yaratır.\n    *   *Gravity Inversion:* 5s boyunca hareket ve saldırı yönleri tersine döner.\n    *   *The Blueprint:* Zemine çizilen pattern 3s içinde hasar alanına dönüşür.\n    *   *Silence:* Oyuncunun tüm skill'lerini 4s boyunca bloklar, sadece hareket ederek hayatta kalması gerekir [21].\n*   **Faz 4 (\"Gerçek Form\" - %20 -> %0):** Architect tüm gücünü açığa çıkarır, müzik patlar ve önceki tüm Act mekanikleri aynı anda arenaya gelir [22, 24].\n    *   *Fracture Everything:* Rastgele 3 eski boss saldırısı arka arkaya atılır.\n    *   *The Original Sin:* Çok yavaş ama devasa hasarlı ölümcül bir saldırı.\n    *   *Build Breaker:* Oyuncunun o run'da en çok kullandığı skill'i 8s boyunca devre dışı bırakır.\n    *   *Final Architecture:* Büyük AoE ile yenilemez an yaratır, sürekli hareket ederek kaçınmak zorunludur [24].",
    "conversation_id": "a026df9f-7689-4086-baf2-fbe552b76536",
    "sources_used": [
      "f309a374-44bd-489f-aa88-fe0e9e1071b3",
      "8509dc23-8a3b-4b13-b32a-06cf539d7b7b",
      "073a0a1a-7bf5-4eda-bd03-10fd83b40828",
      "0474ead0-59c0-4bb7-a810-dc11e8a2bd01",
      "476ee859-fdfe-4c9e-bb0e-c9ca259b4cfc"
    ],
    "citations": {
      "1": "f309a374-44bd-489f-aa88-fe0e9e1071b3",
      "2": "8509dc23-8a3b-4b13-b32a-06cf539d7b7b",
      "3": "073a0a1a-7bf5-4eda-bd03-10fd83b40828",
      "4": "0474ead0-59c0-4bb7-a810-dc11e8a2bd01",
      "5": "476ee859-fdfe-4c9e-bb0e-c9ca259b4cfc",
      "6": "476ee859-fdfe-4c9e-bb0e-c9ca259b4cfc",
      "7": "f309a374-44bd-489f-aa88-fe0e9e1071b3",
      "8": "f309a374-44bd-489f-aa88-fe0e9e1071b3",
      "9": "f309a374-44bd-489f-aa88-fe0e9e1071b3",
      "10": "f309a374-44bd-489f-aa88-fe0e9e1071b3",
      "11": "f309a374-44bd-489f-aa88-fe0e9e1071b3",
      "12": "f309a374-44bd-489f-aa88-fe0e9e1071b3",
      "13": "f309a374-44bd-489f-aa88-fe0e9e1071b3",
      "14": "f309a374-44bd-489f-aa88-fe0e9e1071b3",
      "15": "f309a374-44bd-489f-aa88-fe0e9e1071b3",
      "16": "f309a374-44bd-489f-aa88-fe0e9e1071b3",
      "17": "f309a374-44bd-489f-aa88-fe0e9e1071b3",
      "18": "f309a374-44bd-489f-aa88-fe0e9e1071b3",
      "19": "f309a374-44bd-489f-aa88-fe0e9e1071b3",
      "20": "073a0a1a-7bf5-4eda-bd03-10fd83b40828",
      "21": "f309a374-44bd-489f-aa88-fe0e9e1071b3",
      "22": "f309a374-44bd-489f-aa88-fe0e9e1071b3",
      "23": "f309a374-44bd-489f-aa88-fe0e9e1071b3",
      "24": "f309a374-44bd-489f-aa88-fe0e9e1071b3"
    },
    "references": [
      {
        "source_id": "f309a374-44bd-489f-aa88-fe0e9e1071b3",
        "citation_number": 1,
        "cited_text": "RIMA — Boss Tasarımı: Çok Fazlı Savaşlar Son güncelleme: 2026-04-06 | Tasarım kararı: Claude -------------------------------------------------------------------------------- Tasarım Felsefesi Her boss, oyun akışındaki o anın duygusunu somutlaştırır. <cited_table>",
        "cited_table": {
          "num_columns": 3,
          "rows": [
            [
              "Boss",
              "An",
              "Duygu"
            ],
            [
              "Act 1 Boss",
              "Kırılma öncesi",
              "\"Bir şeyler bitiyor — ve değişiyor\""
            ],
            [
              "Act 2 Boss",
              "Dual-class ilk master",
              "\"Ben artık iki şeyim\""
            ],
            [
              "Act 3 Boss",
              "Full build doruk",
              "\"Bu build insane — ve bu boss bunu sınıyor\""
            ],
            [
              "Final Boss",
              "Her şeyin kapanışı",
              "\"Buraya kadar geldim. Ve şimdi her şey açık.\""
            ]
          ]
        }
      },
      {
        "source_id": "8509dc23-8a3b-4b13-b32a-06cf539d7b7b",
        "citation_number": 2,
        "cited_text": "COMBAT_ROSTER.md — RIMA Düşman Tasarımı [S43 GÜNCEL] Faz 1 mob listesi canonical. Act 2-3 bölümleri henüz tasarım bekliyor. Act bazlı, build-test odaklı düşman roster'ı. Mevcut MOB_TASARIMI.md ile çakışmaz — bu combat karar belgesi, o lore+prompt belgesi. -------------------------------------------------------------------------------- FAZ 1 — S42 ONAYLI MOB ROSTER (8 MOB) Aşağıdaki liste canonical. Act 1 bölümündeki eski tanımlar S43 öncesi versiyondur, kullanma. Tasarım felsefesi: Her mob bir oyuncu alışkanlığını kırar ve bunu spesifik bir skill ile yapar. Auto-attack ≤%25 DPS. Tell max 1.5s, window min 0.4s."
      },
      {
        "source_id": "073a0a1a-7bf5-4eda-bd03-10fd83b40828",
        "citation_number": 3,
        "cited_text": "<cited_table>",
        "cited_table": {
          "num_columns": 2,
          "rows": [
            [
              "Alan",
              "Karar"
            ],
            [
              "Mob infighting",
              "Hayir. Penitent Bruiser aura faction-blind (%50 heal azaltma, 3m) — 2026-05-11 LOCKED"
            ],
            [
              "Terrain hazard",
              "Var — F1 rift catlagi / F2 coken zemin / F3 lav+rift — hikayeye uygun — 2026-05-11 LOCKED"
            ],
            [
              "Room peek",
              "Sadece harita parcasiyla VEYA cleared oda — 2026-05-11 LOCKED"
            ],
            [
              "Hub practice",
              "Skill test + dummy, Hades'ten farkli — 2026-05-11 LOCKED"
            ],
            [
              "Karakter secimi",
              "Her run degistirilebilir, heat per-character (STS mantigi) — 2026-05-11 LOCKED"
            ],
            [
              "Wall tile variety",
              "Rule Tile hybrid (auto-connect + manual override) -- 2026-05-11 LOCKED"
            ],
            [
              "Floor tile variety",
              "Domain-warped Perlin 3-katman, edit-time bake, template-fixed -- 2026-05-11 LOCKED"
            ],
            [
              "Tile kenar invariance",
              "3px border = mortar #1A1C20 only, accent merkeze -- 2026-05-11 LOCKED"
            ],
            [
              "Room Designer vizyon",
              "MapForge -- genel arac, isometric P0, topdown/sidescroller template -- 2026-05-11"
            ],
            [
              "Pre-rendered 2D",
              "Blender/pre-render REJECTED v1; PixelLab pixel art LOCKED -- 2026-05-11"
            ],
            [
              "Render mimarisi",
              "2.5D: 3D cevre + 2D Billboard"
            ],
            [
              "Unity proje",
              "Yeni URP 3D proje, mevcut arsiv"
            ],
            [
              "Silah sistemi",
              "Ayrik 2D sprite child + WeaponAnchorMap + AnimationCurve senkron, body-only anim"
            ],
            [
              "Karakter silah entegrasyonu",
              "Silahli 1-piece — sinif silah sabit, karakter spriteinin parcasi, ayri attach yok. PixelLab create_character 8-yon native. — 2026-05-12 LOCKED"
            ],
            [
              "Anim yonler",
              "4 yon + yatay flip (8 yon REVOKE). N/S/E uretilir, W=flipX. — 2026-05-11 LOCKED (S59 KEEP)"
            ],
            [
              "Body-only anchor",
              "Yeni 16 anchor (4 class x 4 yon), 128px"
            ],
            [
              "Room Designer",
              "ITileWriter → 3D prefab output"
            ],
            [
              "FloorVariantPainter params",
              "warpFreq=0.05, zoneFreq=0.05, warpStrength=4.0; tiers base/accent/hero -- 2026-05-11 LOCKED"
            ],
            [
              "WallAutoConnect variants",
              "8 tip: straight_H/V, corner_NW/NE/SW/SE, end_L/R; 4-bit NSEW mask -- 2026-05-11 LOCKED"
            ],
            [
              "PIXELLAB klasor",
              "PIXELLAB_OUTPUTS/ (kalici) -- STAGING/PIXELLAB kaldirildi -- 2026-05-11"
            ],
            [
              "Map editor",
              "Custom Unity EditorWindow (RIMA Room Designer) -- 2026-05-10 LOCKED"
            ],
            [
              "Concept art stili",
              "Pixel art ZORUNLU, painterly YASAK -- anchor metadata.json referans"
            ],
            [
              "PixelLab MCP yonetimi",
              "Sonnet dogrudan cagri, Codex pre-review + post-QC -- 2026-05-10"
            ],
            [
              "Tile chromakey",
              "#00FF00, filter G>200 AND R<60 AND B<60, binary alpha snap"
            ],
            [
              "Duvar boyutu (PixelLab)",
              "64x128 isometric"
            ],
            [
              "Zemin boyutu",
              "64x64, 16 var"
            ],
            [
              "Anim view",
              "High top-down (~30-35 deg, Hades match) —"
            ],
            [
              "Anim yonler",
              "4 yon + flip (N/S/E uretilir, W=flipX) — 2026-05-11 (S59 KEEP)"
            ],
            [
              "v1 sprint siniflari",
              "Warblade / Ranger / Shadowblade / Elementalist (kalan 6 -> v2)"
            ],
            [
              "Dungeon mimari",
              "Prefab-per-room, 2.5D kare grid arena"
            ],
            [
              "Karakter sprite boyutu (S59)",
              "64x64 chibi top-down — 2026-05-12 LOCKED"
            ],
            [
              "Tile sprite boyutu (S59)",
              "32x32 top-down — 2026-05-12 LOCKED"
            ],
            [
              "VFX sprite boyutu (S59)",
              "64-128px mix (small 64-80, large 96-128) — 2026-05-12 LOCKED"
            ],
            [
              "Skill keybind",
              "LMB/RMB + Q/E/R/F + V(ult) + Space(dash)"
            ],
            [
              "Shadow Echo",
              "3 katman (aura+phantom+UI flash), cyan #00FFCC, 50 havuz"
            ],
            [
              "Act 1 map",
              "15 node procedural (topoloji sabit, icerik random)"
            ],
            [
              "Boss posture",
              "Faz1=700 / Faz2=850 / Faz3=1000"
            ],
            [
              "Final Boss canavar",
              "252-256px + PPU=32 (Faz 4 = 96px insan formu)"
            ]
          ]
        }
      },
      {
        "source_id": "0474ead0-59c0-4bb7-a810-dc11e8a2bd01",
        "citation_number": 4,
        "cited_text": "-------------------------------------------------------------------------------- Boss Rule Pattern (Universal) No execute on boss-tier (Broken/Sundered/Marked+Trapped/Tension MAX/Scar/Hex 10 all gated) Stack-only apply (no full stagger/freeze/lock) Damage scaling reduced (typical 1.5x not 2x) Micro-stagger (0.3-0.4s) replaces full stagger Phase 1 demo: Penitent Sovereign already follows this pattern Abuse Cap Pattern (Universal) ICD (internal cooldown) per enemy on state apply Decay timer (5-10s typical) Stack ceiling (3-10 depending on state) Refresh, not stack on reapply (most states) No CD reset loops (movement skill rule extends here: no infinite state apply chains)"
      },
      {
        "source_id": "476ee859-fdfe-4c9e-bb0e-c9ca259b4cfc",
        "citation_number": 5,
        "cited_text": "<cited_table>",
        "cited_table": {
          "num_columns": 4,
          "rows": [
            [
              "#",
              "Karar",
              "Sonuç",
              "Tarih"
            ],
            [
              "1",
              "Crusader →",
              "Lancer ile birlikte Ronin+Brawler ile değiştirildi.",
              "2026-04-11"
            ],
            [
              "2",
              "Lancer →",
              "Fantasy oturmadı. Ronin ile değiştirildi.",
              "2026-04-11"
            ],
            [
              "3",
              "Gunslinger",
              "İkili tabanca. BDO Archer awakening tarzı Rift Dash. ANA class — Faz 3",
              "2026-04-09"
            ],
            [
              "4",
              "Toplam class:",
              "Warblade/Elem/Shadow/Ranger/Ronin/Gunslinger/Ravager/Brawler/Summoner/Hexer",
              "2026-04-11"
            ],
            [
              "5",
              "Post-launch:",
              "DLC/Update olarak gelecek",
              "2026-04-09"
            ],
            [
              "6",
              "Rift Parry → KALDIRILDI",
              "Dash parry window silindi. Yerine: pasiflerle dodge/deflect seçeneği (şans bazlı veya %100, denge Faz 2'de).",
              "2026-04-16"
            ],
            [
              "7",
              "Fracture Echoes",
              "Boss varyasyon sistemi. Her boss 5 Echo. Kategori: Arena/Adaptif/Minion/Mekanik/Yeni Faz",
              "2026-04-09"
            ],
            [
              "8",
              "Mob kontrolü",
              "Antigravity (Claude) yönetir. 16 benzersiz mob.",
              "2026-04-09"
            ],
            [
              "9",
              "İsim teması",
              "Seçenek C (Hybrid) + Ronin, Gunslinger, Brawler",
              "2026-04-11"
            ],
            [
              "10",
              "Ses pipeline",
              "ChipTone (SFX) + AudioCraft/AudioGen (ortam, 5080 lokal) + Audacity + REAPER",
              "2026-04-09"
            ],
            [
              "11",
              "Ronin",
              "BDO Musa tarzı iaido katana. Draw Tension kaynağı. ANA class — Faz 3",
              "2026-04-11"
            ],
            [
              "12",
              "Brawler",
              "Kombo yumruk/tekme dövüşçüsü. Charge kaynağı. ANA class — Faz 3",
              "2026-04-11"
            ],
            [
              "13",
              "Tüm class'lar hızlı",
              "Hiçbir class yavaş hissettirmez. \"Ağır\" hissi görselden (animasyon/partikül) gelir, input response'dan değil. Zorunlu bekleme süresi yok.",
              "2026-04-11"
            ],
            [
              "14",
              "Skill anim: 3-segment workflow",
              "PEAK frame önce üretilir → START→PEAK interpolate → PEAK→END interpolate → birleştir. Detay: MASTER_KARAR_BELGESI #14 notu.",
              "2026-04-11"
            ],
            [
              "15",
              "Shadowblade tam redesign",
              "WoW Rogue kopyası kaldırıldı. Yeni: Sever (pozisyonel), Rift Scar/collapse geometrisi, void aesthetic. SINIF_VE_SKILL_KARAR_BELGESI v4.",
              "2026-04-14"
            ],
            [
              "16",
              "Elementalist Light elementi",
              "Arcane kaldırıldı. Light = synthesis state (Fire+Frost Resonance → Lightbreak). S43 canonical Light skill'leri: Prism Beam, Frost Wall, Solar Flare, Radiant Pillar. V Burst: Trinity Storm. Build axis: Radiant Break.",
              "2026-04-14"
            ],
            [
              "17",
              "V Meter ayrımı — tüm classlara",
              "Her class için ayrı [V] Dolum koşulu eklendi. Zorunlu kural: class resource'dan farklı aksiyon tipi. Dolum isimleri: Dominance/Convergence/Predation/Kill Zone/Carnage/Flow Cut/Showtime/Crowd Hype/Grave Chorus/Dread",
              "2026-04-14"
            ],
            [
              "18",
              "Item System D kilitleme",
              "Hybrid: Relic (2+1 garantili/run) + Skill Modifier (2+1 garantili/run). Ekipman slot yok, stat bloat yok. Kaynak: Treasure room / mini-boss / elite / shop.",
              "2026-04-14"
            ],
            [
              "19",
              "Ravager V Burst redesign",
              "BERSERK MODE = kan siklozu (2.5 birim yarıçap, pasif AoE + 0.5s single-target darbe). Kill → +0.8s (max +3s). V dolum: kill chain (Carnage), Fury'den ayrı.",
              "2026-04-14"
            ],
            [
              "20",
              "Brawler rotation fix",
              "Charged State (5 Charge) iki şekilde harcanabilir: (1) anında +%50 skill güçlendirme, (2) RMB ile \"Overdrive Fuel\" olarak bankala → Crowd Hype V'ye transfer.",
              "2026-04-14"
            ],
            [
              "21",
              "Hexer rotation fix",
              "Hexblast 7+ stack'te kullanılabilir. 7-9: %70/stack, CD sıfırlanmaz. 10: tam ödül. Early cashout branch — 10-stack rüyası korunuyor.",
              "2026-04-14"
            ],
            [
              "22",
              "Rift Break sistemi",
              "V Burst context-based interactive phase. Normal oda: hızlı otomatik. Boss/elite: slow-mo (0.08-0.20x), class'a özel input sekansı. Fail=base hasar, success=empowered. Hedef otomatik (context). Animasyonlar hazır olmadan detay tasarımı yapılmaz.",
              "2026-04-16"
            ],
            [
              "23",
              "Rift Break — class-model eşlemesi",
              "Warblade/Shadow/Ronin/Ranger=timing chain; Gunslinger/Ravager=charge-release; Elementalist/Hexer=sekans; Brawler=mash+timing; Summoner=minyon yönlendirme. Boss özel: Rift Duel (faz geçişi + execution window).",
              "2026-04-16"
            ],
            [
              "24",
              "Cross-class Tier Unlock",
              "Secondary class'tan 2 skill alınca Tier 2 cross-class açılır. Act 2 boss sonrası Tier 3 + Cross-class Ultimate açılır. Default temel sinerjiler Act 1 boss sonrası erişilebilir. Animasyonlar üretilmeden Tier 2-3 detayları yazılmaz.",
              "2026-04-16"
            ],
            [
              "25",
              "Meta progression",
              "Şimdilik yok. Hades-Darkness benzeri kalıcı hub upgrade → Faz 4-5 scope.",
              "2026-04-16"
            ],
            [
              "26",
              "Damage test → harita bağlantısı",
              "Combat damage testi ileride RuntimeRoomManager + oda tipi (elite/boss/corridor) bağlanacak. Şu an flat test yeterli. Faz 2 scope.",
              "2026-04-16"
            ],
            [
              "27",
              "Echo Imprint sistemi",
              "Her 3 combat odada 1 Echo Imprint sunusu (Skill Draft'a ek). 3 kategori: Strike Form (LMB), Outlet Form (RMB), Surge Form (Dash/Resource). Max 4/run (act başına 1). Faz 2'den itibaren.",
              "2026-04-17"
            ],
            [
              "28",
              "Tag Sinerji Bonusu",
              "6 aktif slot'tan 2 aynı tag → otomatik pasif bonus. Max 2 sinerji aktif. Tam tablo:",
              "2026-04-17"
            ],
            [
              "29",
              "Oda sayisi revizyonu",
              "Act 1: 8-9 oda",
              "2026-04-17"
            ],
            [
              "30",
              "Proje tonu: Fractured Epic",
              "\"Dark fantasy\" ifadesi kaldırıldı. Ton: Hades benzeri — dünya kırılmış ama görsel olarak DRAMATIK ve CANLI. Void karanlığına karşı parlak kontrastlar. Grimdark değil. Renkler canlı, karakterler ifadeli.",
              "2026-04-17"
            ],
            [
              "31",
              "Ghost Attack = Opsiyon C",
              "Her iki trigger noktasında: cross-class skill havuzu (80 skill, 2 slot) VE Z/X secondary skilleri. 12f animasyon, 2-segment, 4 yön. Sprite nötr üretilir, Unity'de MaterialPropertyBlock ile class tint. ~240g toplam 10 class. Tam spec:",
              "2026-04-17"
            ],
            [
              "32",
              "Mob Armor Variant sistemi",
              "Hades tarzı 3 tier: Normal (1x HP, kırmızı bar) / Armored (2x, altın→kırmızı) / Heavily Armored (3x, gümüş→altın→kırmızı). Sprite: base hazır → edit-images-v2 ile metalik zırh ekle, 1g/sprite. Act 1 varyantları: Shard Bulwark, Void Juggernaut, Iron Penitent, Chain Executioner, Relic Archon.",
              "2026-04-17"
            ],
            [
              "33",
              "PixelLab Faz Master Rehberi",
              "Tüm fazlar için tek üretim referansı. Faz 1 tam detay, Faz 2 tam, Faz 3-4 outline.",
              "2026-04-17"
            ],
            [
              "34",
              "Class cinsiyetleri — 5E/5K kilitlendi",
              "Erkek: Warblade, Brawler, Ravager, Ronin, Shadowblade. Kadın: Elementalist, Gunslinger, Hexer, Ranger, Summoner. Denge + özgünlük. Gunslinger kadın → trençkot+revolver arketipi klişeden kaçıyor. Hexer kadın → erkek dark wizard generic.",
              "2026-04-19"
            ],
            [
              "35",
              "PixelLab Sprite Pipeline — Session 17 kilitlendi",
              "#40+#41 ile override edildi.",
              "2026-04-19"
            ],
            [
              "36",
              "Kamera açısı: Hero Siege style — KİLİTLENDİ",
              "Tüm playable class sprite üretiminde",
              "2026-04-19"
            ],
            [
              "37",
              "Ranger identity — tactical rift hunter",
              "Dungeon/ruins avcısı. Forest archer DEĞİL. Asimetrik utility silüeti: trap canister + tether spool. Kite-control visual language. Rift hunter arka planı.",
              "2026-04-19"
            ],
            [
              "38",
              "Gunslinger identity — rift-tech dual-pistol duelist",
              "Western/kovboy arketipi YASAK. Rift-tech dual-pistol, kinetic run-and-gun okuma. Coat/hat silüeti altında kadın okuma korunmalı.",
              "2026-04-19"
            ],
            [
              "39",
              "Helmet scope ayrımı (Gemini vs PixelLab fazı)",
              "Gemini reference aşaması: helmet yok (yüz okunurluğu şart). PixelLab Warblade framework aşaması: helmet intentional (QC'de kontrol edilir). Bu faz ayrımıdır, çelişki değil.",
              "2026-04-19"
            ],
            [
              "40",
              "Kamera açısı REVİZYON — #36 override",
              "PixelLab \"low top-down\" = ~35 derece (ARPG açısı, Diablo 2/PoE). 75-80° PixelLab'de mevcut değil — TERK EDİLDİ. Aktif hedef: 35° ARPG. GDD \"75-80°\" ifadesi bu kararla override edilir.",
              "2026-04-20"
            ],
            [
              "41",
              "Sprite pipeline REVİZYON — #35 override",
              "Aktif pipeline: Gemini → concept PNG → PixelLab \"Create from Reference\", high top-down, female/male human preset, AI Freedom 0.",
              "2026-04-23"
            ],
            [
              "42",
              "Animasyon: Walk YOK, Run var",
              "Lokomotion animasyonu Walk değil Run. Idle = interpolate first+last frame aynı. Run = PixelLab Create Character built-in (simple loop). Attack/Dash = 3-segment interpolate workflow.",
              "2026-04-20"
            ],
            [
              "43",
              "Elementalist saç rengi — honey-blonde",
              "Koyu siyah saç terk edildi. Warm honey-blonde, arkaya topuz, birkaç tel yüzü çerçeveler, dramatik efekt yok. Siyah saç top-down'da koyu cüppe ile kaynıyor, okunmuyordu. Saç aynı zamanda sakin/kontrollü — büyü dalgalanması efekti YOK.",
              "2026-04-21"
            ],
            [
              "44",
              "Gunslinger saç rengi — deep auburn red (kızıl)",
              "Copper-orange terk edildi. Deep auburn red: koyu, zengin kızıl — orange/copper değil. Elementalist honey-blonde ile kontrast oluşturur, iki kadın class arasında palette ayrımı sağlanır.",
              "2026-04-21"
            ],
            [
              "45",
              "Kamera açısı — PixelLab Low Top-Down = ~35° ARPG",
              "PixelLab Create Character \"Low Top-Down\" modu ~35° Diablo 2/PoE açısı verir. South yönünde yüz görünür, gözler görünebilir — bu tool limiti, değiştirilemez. \"Gözler görünmez\" kriteri terk edildi. QC kriteri güncellemesi: yönler arası ölçek tutarlı + baş-gövde oranı insan (chibi değil) = PASS. warrior_idle_128.png referans olarak Style Image slotuna yüklenir.",
              "2026-04-24"
            ],
            [
              "46",
              "Run animasyonu: 6 frame, 8 yön, flip yok",
              "Her yön ayrı üretilir. Flip kullanmak yasak — simetri bozar, silah tarafı değişir. 8 yön × 6 frame = 48 clip per class.",
              "2026-04-23"
            ],
            [
              "47",
              "Animasyon üretim yöntemi",
              "Run = PixelLab Animate (8 gen direkt). Attack/Skill = KF+Interpolate (3-segment workflow). Single-phase = Animate direkt.",
              "2026-04-23"
            ],
            [
              "48",
              "Death/Hit reaction = 4 yön",
              "Lokomotion (#46) 8-yön, ama death ve hit reaction animasyonları 4-yön (ileri/geri/sol/sağ). Kısa süreli animasyonlar — köşe yönleri oyuncu okuma açısından kritik değil. Production cost yarıya iniyor.",
              "2026-04-24"
            ],
            [
              "49",
              "8-dir pipeline kilitli (Yol A)",
              "10 class tamamı 8-dir locomotion",
              "2026-04-24"
            ],
            [
              "53",
              "4 Cardinal Yön kilitlendi — S/E/N/W",
              "Animasyon üretimi S/E/N/W — 8 yön DEĞİL. Sebep: RIMA kamerası 30-35° top-down ARPG, Hades gibi izometrik değil. Hades'in 4 diagonal sprite sistemi izometrik kamera için — RIMA'ya uygulanamaz. Last Epoch/D2/Cursemark referansı = cardinal yön sistemi. Runtime: 8 hareket yönü → 4 sprite yönüne 45° threshold mapping + hysteresis (son kardinal yönü koru).",
              "2026-04-27"
            ],
            [
              "50",
              "Game Feel Toggles — Default ON, Settings Opt-Out",
              "Screen shake, hit stop, low HP vignette, damage numbers, chromatic aberration, motion blur, kill slowmo vb.",
              "2026-04-24"
            ],
            [
              "51",
              "Localization — Day 1 Modular, TR+EN Öncelik",
              "Tüm UI/dialog/tooltip metinleri",
              "2026-04-24"
            ],
            [
              "54",
              "R4 Ulti Toggle + Perfect Condition",
              "Per-skill Shift+key toggle. Lock ON default. \"Resource MAX = ulti\" TERK EDILDI → \"Perfect Condition triggers empowered cast.\" Gunslinger = Heat ZERO, Hexer = Stack 10 (target), diger class = Resource MAX. Relock rule: ulti cast sonrasi o skill auto Lock ON. Room start: tum lock'lar ON sifirlanir. Zorunlu HUD: lock icon + armed cue + confirmation cast VFX. Detay:",
              "2026-04-30"
            ],
            [
              "55",
              "R4 Brawler State Ownership — Shattered",
              "Brawler upgraded state =",
              "2026-04-30"
            ],
            [
              "56",
              "R4 Execute Gates — HP gate YASAK",
              "HP<30% execute tum class'larda YASAK. Her class sadece kendi state gate'ini kullanir (Broken/Sundered/Marked+Trapped/Tension/Scar/Heat ZERO/Hex10). Boss'ta execute yok — damage burst (50-70%) olarak indirgenir.",
              "2026-04-30"
            ],
            [
              "57",
              "R4 Counter Arketip Ayrimi",
              "3 counter arketipi keskin ayri tutulur: Warblade = absorb/break (timed block → Broken). Ronin = pre-draw timing (frame-perfect → Opened). Brawler = whiff/evade body movement (dodge into whiff → Off-Balance). Arasinda gecis ve cakisma YASAK.",
              "2026-04-30"
            ],
            [
              "58",
              "R4 Movement Option C",
              "Space = kisa dash, no state, no damage, resource-neutral. Build'de max 1 skill movement. Skill movement = state-interaction zorunlu. State apply → CD reset YASAK. Space + skill movement i-frame overlap YASAK.",
              "2026-04-30"
            ],
            [
              "59",
              "R4 Pixel Art Constraint — Skill VFX",
              "Skill kurban-taraf efekti: mevcut hit-react / freeze / slide / overlay / VFX / camera shake / hit-stop ONLY. Custom mob lift/throw/grapple/ragdoll YASAK. Wall-Slammed: fallback Ground-Slammed (wall yok ise Cracked refresh + dust VFX, slide yok). Boss/elite: micro-stagger only, slide yok. Detay:",
              "2026-04-30"
            ],
            [
              "60",
              "Skill System Taxonomy LOCKED",
              "4 aktif tip: STRIKE / ZONE / REACTIVE / STATE. 3 pasif tip: KEYSTONE (zorunlu 1) / MODIFIER (2) / RESONANCE (1). Upgrade: skill basina max 3, draft-only, REPLACE prompt. Identity Passive: her class 1 sabit, upgrade edilemez. Cross-Family Carrier: sadece Legendary upgrade, tag pip only, sigil yasak. Summoner/Hexer: alt-tag (summon/accumulation), yeni tip yok. Ghost Attack: sadece STRIKE, summon tag excluded. Detay:",
              "2026-05-06"
            ],
            [
              "61",
              "Dungeon Architecture KEEP — Hades-style",
              "Acik dunya / Diablo paradigm REJECTED. Hades-style discrete room flow + StS macro graph hibrit modeli LOCKED. Combat v1 commit-windowing + Cross-Class 3-tag Resonance v2 ayrik arena + kapi kilidi gerektiriyor. Detay:",
              "2026-05-09"
            ],
            [
              "62",
              "Act 1 Map LOCKED v1 (15 node) — #29 OVERRIDE",
              "Karar #29",
              "2026-05-09"
            ],
            [
              "63",
              "Map Fragment + Kirik Tas Tablet Sistemi LOCKED v1",
              "StS2-style harita parcasi reveal sistemi. Combat/Elite/Mystery oda temizlenince fragment duser (cyan glow + bobbing), G ile pickup zorunlu. Reveal: standart 1 node ileri %65 / 2 node %30 / 3 node %5. Acik node bonus: +1 hop ileri acar. Onun puslu = garanti drop, aciksa olasilikli drop. Boss kapisi 8 fragment (combat 6 + elite 2). Map UI metaforu: \"Kirik Tas Tablet\" — soyut grid + pasli altin cerceve + cyan rift catlaklari. Tablet 4 Act'te gorsel evrim gecirir (Act1: kale oymalari -> Act2: damarli et -> Act3: yuzen parcalar -> Act4: ayna). Hibrit UI: TAB MapPanel (StS-style) + sol-ust MiniMap (Hades-style 128x128). Detay:",
              "2026-05-09"
            ],
            [
              "64",
              "ActionCommitProfile 5 alan LOCKED v1",
              "Combat fluidity v1 sprint. BasicAttackProfile genisletilir 5 alanla: windupMs, recoveryMs, dashCancelStartFraction (per-class), hitstopMs, cancelOnWhiff. Alabaster Dawn animation commitment prensibi RIMA'ya tasindi. Detay: AD eval memory/project_alabaster_dawn_eval.md.",
              "2026-05-09"
            ],
            [
              "65",
              "3-Layer Feedback Hierarchy LOCKED v1",
              "Hit feedback 3 katman: Normal / Commit / Break. Posture break window'da (3s, +50% dmg) gorsel + ses katmani yukselir. Named outcome glyph v1'DE YOK (v2 aday).",
              "2026-05-09"
            ],
            [
              "66",
              "Boolean hasInterruptArmor v1",
              "Mob/boss posture v1: boolean hasInterruptArmor flag. Sayisal poise meter v2'ye ertelendi. Boss posture kalibrasyon: 450 light boss / 850 heavy boss. showPostureMeter Settings toggle (default ON, AD v2 eklemesi).",
              "2026-05-09"
            ],
            [
              "67",
              "Dash-Cancel Windows Per-Class",
              "BasicAttackProfile.cancelWindowFraction class-spesifik: Ravager/Shadowblade: 15-25% (early cancel). Ranger/Gunslinger: 30-55% (genisletildi v2 AD eklemesiyle). Warblade/Brawler: 60-75% (commit agirlikli). Casters (Elementalist/Hexer/Summoner): windup not cancellable.",
              "2026-05-09"
            ],
            [
              "68",
              "OnDash Passive Proc (4. pasif tipi)",
              "Cross-Class Proc system'e OnDash eklendi. CrossClassEffectType.OnDash enum + CrossClassSkillManager.OnDash() method. Shadowblade/Ronin primary kullanicilar.",
              "2026-05-09"
            ],
            [
              "69",
              "Cross-Class Proc Text Feedback",
              "3-tag Rift / 2-tag Resonance proc tetiklenince sigil glyph ustune 1 satir 12px text (\"Tremor!\" / \"Severance!\" / \"Bloodveil!\" vb.) + SFX. Death recap + next-run hint UX layer (boss yenilgisi sonrasi, opsiyonel).",
              "2026-05-09"
            ],
            [
              "70",
              "Resonance 2-tag Named Outcomes v2 ADAY",
              "[!] v2 aday, henuz LOCKED degil. 10 pair: Tremor/Bloodveil/Severance/Crushblood/Resonant Hold/Lockedge/Splinter/Phantom Pulse/Hammerwound/Hemorrhage. Rift 3-tag kurali KORUNUR. v2 onayi sonrasi LOCKED isaretlenecek.",
              "2026-05-09"
            ],
            [
              "71",
              "Silah Gorunurluk: Single-State LOCKED",
              "Tum siniflarda silah hep elde / hazir pozisyonda — combat-out ayri state YOK, \"puf\" mekanigi YOK. Istisna sadece",
              "2026-05-09"
            ],
            [
              "52",
              "Skill VFX + Projectile Mimarisi — Tüm Classlara Geçerli",
              "Projectile prefab yapısı: SpriteRenderer + PointLight2D (elemental renge göre) + CircleCollider2D + Rigidbody2D +",
              "2026-04-27"
            ],
            [
              "72",
              "S59 Pivot — Pure 2D Top-Down LOCKED",
              "2.5D mimari (3D env + 2D billboard) REVOKED. Pure 2D top-down (Hammerwatch/HLD ref) LOCKED. Karakter 64x64 chibi (eski 128px native + chibi YASAK kararlari REVOKED), tile 32x32 top-down (eski 64x64 floor + 64x128 wall iso REVOKED), VFX 64-128px mix. URP 2D Renderer + Pixel Perfect Camera + 2D Lights. Anim view: high top-down ~30-35° (Hades match KEEP). Mevcut RIMA projesi RESTORE (RIMA_2.5D nested arsivlenecek). 4 yon + flip KEEP. PPU=64. Detay:",
              "2026-05-12"
            ],
            [
              "73",
              "Karakter Silah Entegrasyonu — Silahlı 1-piece LOCKED",
              "Karakter 64px chibi sınıf-spesifik silahla TEK SPRITE üretilir. Body-only + WeaponAnchorMap + AnimationCurve senkron sistemi REVOKED. Sebep: 64px'te body-only AI variance yuksek, pixel-precise anchor imkansiz; PixelLab Create Character native silahli 1-piece uretiyor; sınıf↔silah sabit (Warblade=kılıç, Ranger=yay, Shadowblade=hançer, Elementalist=asa); referans oyunlar (Hammerwatch/HLD/Tunic) hepsi 1-piece. Karar #71 (silah hep elde single-state) ile uyumlu. Detay:",
              "2026-05-12"
            ],
            [
              "74",
              "Boss/Mob Boyut Hiyerarşisi 2^n + PPU=64 Standardize",
              "Tum sprite PPU=64 (boyut farki sprite canvas ile gelir, PPU manipulasyonuyla DEGIL). 2^n hierarchy: Karakter 64x64, kucuk/orta mob 64x64, elite mob 128x128, miniboss 128x128, Act Boss 256x256, Final Boss 256x256 (sahnede ~2.5x oyuncu = Hades benchmark). Eski Final Boss PPU=32 + Faz 4 = 96px insan formu (boyut degisimi) REVOKED. Boss kimligi mekanik + faz degisimleri + VFX ile gelir, boyut DEGIL. 4 faz mekanik kurali NOT_LOCKED (design sprint bekliyor). Detay:",
              "2026-05-12"
            ],
            [
              "75",
              "PixelLab Map Tools KULLANILMAYACAK",
              "NLM 2026-05-11 LOCKED kararı:",
              "2026-05-12"
            ],
            [
              "76",
              "Asset Prompt Format — TYPE/HEAD/BODY/LIMBS",
              "Karakter + mob + boss asset prompt formati: TYPE/STYLE/HEAD/BODY/LIMBS/EXTRA/CLOTHING/HANDS/SILHOUETTE/COLOR/POSE blok formati. Image #2 (ranger_walk_v1.txt referans) inspired. PixelLab Create Character / Create Image Pro / vary_object icin standart template. 8-direction sheet layout: 3x3 grid, center empty (Image #1 referans). LAYOUT + RULES bloklari sonda (size lock + footprint lock + anchor + anti-aliasing yasak). Detay:",
              "2026-05-12"
            ]
          ]
        }
      },
      {
        "source_id": "476ee859-fdfe-4c9e-bb0e-c9ca259b4cfc",
        "citation_number": 6,
        "cited_text": "Universal dash-parry sistemi kaldırıldı. Karar tarihi: 2026-04-16. Yerine: class'a özgü parry/deflect skill'leri (Iron Counter, Blade Veil, Predator's Fold, Counter Blow vb.) RiftParry.cs + ParryFeedback.cs + ClassParryBonus.cs → ARCHIVE/ 'e taşı. -------------------------------------------------------------------------------- FRACTURE ECHOES ÖZETİ Her boss ilk öldürmede Echo 1, her tekrarda +1 (max 5) Toggle ile açılıp kapatılabilir Echo 3+ = guaranteed Epic drop Echo 5 = cosmetic ödül + lore fragment Boss başına: Penitent (5 echo), Echo Twin (5), Fracture Sovereign (5), The Architect (5) Echo 5 = yeni faz eklenir bossa"
      },
      {
        "source_id": "f309a374-44bd-489f-aa88-fe0e9e1071b3",
        "citation_number": 7,
        "cited_text": "Multi-faz geçişleri sadece HP eşiği değil — her geçiş anlatı momenti . Kamera duraklıyor, efekt çalıyor, müzik kırılıyor. -------------------------------------------------------------------------------- ACT 1 BOSS — The Penitent Sovereign (3 Faz) [S42 — GÜNCEL] [S43 GÜNCEL] 2-fazlı eski tasarım geçersiz. Aşağıdaki 3-fazlı versiyon canonical. Tema: Boyun eğmiş ama kırılmamış. Zincirleriyle savaşır — ta ki onları kırana kadar. Lore: Bölgenin eski koruyucusu. Kendini cezalandırmak için rift enerjisini içinde hapsetti. Zincirleri artık fiziksel kilit değil, kendi öz disiplinin metaforu — kırıldıkları an yeni bir form ortaya çıkıyor."
      },
      {
        "source_id": "f309a374-44bd-489f-aa88-fe0e9e1071b3",
        "citation_number": 8,
        "cited_text": "Arena: 14×14 tile dairesel taş platform (Act 1 ritüel kalıntısı). 2 dash lane karşılıklı (kuzey-güney). Faz 2'de orta noktada Rift Tear hazard belirir, dash lane'ler tek-yönlü olur. Overlap-safety kuralı: Shackle Cast → Chain Detonation arasında minimum 0.8s gap zorunlu. Sovereign's Wrath güvenli daire: Center sabit OLMAZ — center / edge / boss-behind arc arasında döner (pattern okuma zorunlu). Faz 1 — \"Zincirin Altında\" (HP: 100% → 66%) Telegraphed kit, oyuncu öğrenir. Pattern: Whip → Surge → Whip → Shackle."
      },
      {
        "source_id": "f309a374-44bd-489f-aa88-fe0e9e1071b3",
        "citation_number": 9,
        "cited_text": "<cited_table> Faz geçiş eşiği: %66 HP Geçiş sahnesi (1.5s): Sovereign yere çöker. Göğsünden mor ışık fışkırır. \"...artık yetmez.\" → zincirleri kırılır, hız +%30. Arena merkezinde Rift Tear (3m radius hazard) belirir. Faz 2 — \"Kırılan Zincir\" (HP: 66% → 33%) Hız +%30 (Faz 1'e göre). Rift Tear orta hazard aktif.",
        "cited_table": {
          "num_columns": 4,
          "rows": [
            [
              "Skill",
              "Tell",
              "Etki",
              "Window"
            ],
            [
              "Chain Whip",
              "0.8s — kol geriye",
              "6m düz çizgi 30 hasar",
              "1.0s"
            ],
            [
              "Penitent Surge",
              "1.2s — yumruk yere",
              "4m radius itme + 35 hasar",
              "1.5s"
            ],
            [
              "Shackle Cast",
              "1.0s — zincir havalanır",
              "8m menzil tek hedef chain → 2s slow %50 + 15 hasar",
              "1.2s"
            ]
          ]
        }
      },
      {
        "source_id": "f309a374-44bd-489f-aa88-fe0e9e1071b3",
        "citation_number": 10,
        "cited_text": "<cited_table> Faz geçiş eşiği: %33 HP Geçiş sahnesi (2.0s): Sovereign havaya kalkar, kalan tüm zincirler erir, gövdesi yarısı rift enerjisiyle değişir. Müzik tema değişir. Hız toplam +%50 (Faz 1'e göre).",
        "cited_table": {
          "num_columns": 4,
          "rows": [
            [
              "Skill",
              "Tell",
              "Etki",
              "Window"
            ],
            [
              "Fracture Strike",
              "0.5s — dash forward",
              "3 ardışık swing (sol-sağ-orta), her biri 22 hasar",
              "0.8s son swing'den sonra"
            ],
            [
              "Chain Detonation",
              "1.0s — zincir parçaları zemine saplanır",
              "3 nokta marker, 2.5s sonra her biri 2m radius patlama (40 hasar)",
              "1.5s (parça yerleştirme sonrası)"
            ],
            [
              "Shackle Cast",
              "1.0s",
              "Faz 1 ile aynı",
              "1.2s"
            ]
          ]
        }
      },
      {
        "source_id": "f309a374-44bd-489f-aa88-fe0e9e1071b3",
        "citation_number": 11,
        "cited_text": "Faz 3 — \"Sovereign Awakened\" (HP: 33% → 0%) Desperation faz. Wrath'ın güvenli dairesi merkezdedir — ama Faz 2'den kalan Rift Tear orada (dış kenar = Charge riski, iç merkez = Tear hazard). 30-45s kill hedefi. <cited_table>",
        "cited_table": {
          "num_columns": 4,
          "rows": [
            [
              "Skill",
              "Tell",
              "Etki",
              "Window"
            ],
            [
              "Fracture Charge",
              "0.6s — başlangıç pozisyonu glow",
              "Arena boyunca dash + 50 hasar düz çizgi",
              "1.5s charge sonrası"
            ],
            [
              "Sovereign's Wrath",
              "1.5s — zemine kök salar, çevre kızarır",
              "Tüm arena hasar (60) HARİÇ orta 2m güvenli daire.",
              "2.0s recovery"
            ],
            [
              "Chain Detonation",
              "0.7s tell, 4 nokta, 1.5s patlama",
              "Faz 2 versiyonu hızlandı",
              "1.0s"
            ],
            [
              "Fracture Strike",
              "0.4s",
              "3-hit combo +%20 hasar",
              "0.6s"
            ]
          ]
        }
      },
      {
        "source_id": "f309a374-44bd-489f-aa88-fe0e9e1071b3",
        "citation_number": 12,
        "cited_text": "Ölüm sahnesi (2.5s): Sovereign çöker. \"...sonunda boş.\" Zemin çatlar → secondary class seçimi açılır (Faz 2 unlock cue; Faz 1'de placeholder credits). Özet tablosu güncelleme: <cited_table> -------------------------------------------------------------------------------- ACT 2 BOSS — The Echo Twin (2 Faz) Tema: Oyuncunun dual-class dönüşümünün yansıması. İki varlık — ama tek beden. Lore: Fracturing'de iki kimliği aynı anda taşıyan biri. İkisi de çıkmak istiyor. Oyuncu dual-class'ını ustalıkla kullanarak bu ikiliği bozabilir.",
        "cited_table": {
          "num_columns": 4,
          "rows": [
            [
              "Boss",
              "Faz Sayısı",
              "Geçiş HP",
              "Süre Hedefi"
            ],
            [
              "Act 1 — Penitent Sovereign",
              "3",
              "%66 / %33",
              "3-4 dk"
            ]
          ]
        }
      },
      {
        "source_id": "f309a374-44bd-489f-aa88-fe0e9e1071b3",
        "citation_number": 13,
        "cited_text": "Tasarım amacı: Cross-class Ultimate'in kullanılmasını mekanik olarak teşvik et. Faz 1 — \"Birinci Kimlik\" (HP: 100% → 40%) Boss, dominant kimliğiyle savaşır. Saldırıları okunabilir, pattern'ı nettir. Run'da hangi primary class seçildiğine bağlı olarak Echo Twin'in birinci kimliği değişir: Warblade aldıysan → Echo Twin melee agresif Elementalist aldıysan → Echo Twin ranged mage Shadowblade aldıysan → Echo Twin stealth/burst Ranger aldıysan → Echo Twin kiting saldırgan (Her varyant için 4 ayrı saldırı seti — implementation sonraya bırakılabilir, ilk build'de tek varyant yeterli)"
      },
      {
        "source_id": "f309a374-44bd-489f-aa88-fe0e9e1071b3",
        "citation_number": 14,
        "cited_text": "Örnek (Melee varyant): <cited_table> Faz geçiş eşiği: %40 HP Geçiş sahnesi (2 sn): Boss ikiye \"yırtılıyor\" gibi görünür — iki silüet belirir, sonra tekrar birleşir. İkinci kimlik dominant olur. Ses tonu değişir, renk paleti kayar. Faz 2 — \"İkinci Kimlik\" (HP: 40% → 0%) Tamamen farklı saldırı seti. Önceki faza hiç benzemiyor. <cited_table>",
        "cited_table": {
          "num_columns": 2,
          "rows": [
            [
              "Saldırı",
              "Mekanik"
            ],
            [
              "Mirror Slash",
              "Oyuncunun son kullandığı skill'in ayna kopyası"
            ],
            [
              "Identity Strike",
              "5m dash + hasar"
            ],
            [
              "Echo Barrier",
              "3s kalkan — hasar azaltır"
            ],
            [
              "Twin Pulse",
              "2m AoE patlama, oyuncuyu iter"
            ]
          ]
        }
      },
      {
        "source_id": "f309a374-44bd-489f-aa88-fe0e9e1071b3",
        "citation_number": 15,
        "cited_text": "Tasarım notu: Faz 2'ye girince \"Cross-class Ultimate kullan\" baskısı hissettirmeli ama zorunlu değil. İyi bir oyuncu Ultimate'siz de geçebilmeli — ama Ultimate kullanınca belirgin şekilde daha kolay. -------------------------------------------------------------------------------- ACT 3 BOSS — The Fracture Sovereign (3 Faz) Tema: Fracturing'in ta kendisi. Bu bir varlık değil — yaranın kendisi. Lore: Fracturing ilk burada açıldı. Bu \"alan\" zaman içinde bir bilinç kazandı. Oyuncu burada dünya yarığının merkezinde savaşıyor."
      },
      {
        "source_id": "f309a374-44bd-489f-aa88-fe0e9e1071b3",
        "citation_number": 16,
        "cited_text": "Tasarım amacı: Full dual-class build'in tüm bileşenlerini zorlasın. Her faz farklı bir yetenekler setini zorunlu kılar. Faz 1 — \"Yara Açıldı\" (HP: 100% → 60%) Saldırılar basit ama sert. Boss stagger'lanamaz. <cited_table> Faz 2 — \"Çevre Uyanıyor\" (HP: 60% → 30%) Geçiş sahnesi (2 sn): Zemin çatlar. Arena'nın 4 köşesinde \"Fracture Node\" aktive olur — bunlar hasar veriyor. Boss kendi etrafında bir koruyucu enerji döndürmeye başlar.",
        "cited_table": {
          "num_columns": 2,
          "rows": [
            [
              "Saldırı",
              "Mekanik"
            ],
            [
              "Fracture Beam",
              "Yavaş dönen lazer çizgisi"
            ],
            [
              "Void Shard",
              "3 yönde projectile"
            ],
            [
              "Sovereign Step",
              "Boss teleport — rastgele konuma"
            ],
            [
              "Gravity Pull",
              "Oyuncuyu kenara çeker, sonra bırakır"
            ]
          ]
        }
      },
      {
        "source_id": "f309a374-44bd-489f-aa88-fe0e9e1071b3",
        "citation_number": 17,
        "cited_text": "Faz 2'de arena küçülür (hazard zone genişler). Çalışma alanı daha dar. <cited_table> Ek mekanik: Fracture Node'ları vurarak öldürebilirsin → her öldürme boss'a %5 hasar. Boss'u daha hızlı öldürmenin alternatif yolu. Faz 3 — \"Tam Kırılma\" (HP: 30% → 0%) Geçiş sahnesi (2.5 sn): Boss havaya yükselir. Müzik sıfırlanır — sadece sessizlik, sonra yeni tema başlar. Arena tamamen değişir: zemin \"boşluk\"a dönüşür, sadece platform adacıkları kalır.",
        "cited_table": {
          "num_columns": 2,
          "rows": [
            [
              "Saldırı",
              "Mekanik"
            ],
            [
              "Node Pulse",
              "Her 15 saniyede nodlar patlıyor"
            ],
            [
              "Fracture Wave",
              "Zemin boyunca yayılan dalga — atlamak için pencere"
            ],
            [
              "Shatter Field",
              "Alanda dönen orbs, temas = slow"
            ],
            [
              "Sovereign Gaze",
              "3s işaret + büyük hasar — block edilebilir"
            ]
          ]
        }
      },
      {
        "source_id": "f309a374-44bd-489f-aa88-fe0e9e1071b3",
        "citation_number": 18,
        "cited_text": "<cited_table> Ölüm sahnesi: Arena yeniden birleşir. Zemin sağlamlaşır. Sessizlik. Final Boss kilidi açılır. -------------------------------------------------------------------------------- FINAL BOSS — The Architect (4 Faz)",
        "cited_table": {
          "num_columns": 2,
          "rows": [
            [
              "Saldırı",
              "Mekanik"
            ],
            [
              "Fracture Surge",
              "Boss'un tüm önceki saldırıları sırayla — ama 2x hızda"
            ],
            [
              "Void Collapse",
              "Arena'nın bir bölümü çöker, platform kayar"
            ],
            [
              "Sovereign's Final Form",
              "5s hazırlanır, sonra tüm alana hasar (hayatta kalmak için belirli platforma geçmek lazım)"
            ],
            [
              "Echo of Pain",
              "Oyuncunun aldığı son büyük hasarın kopyasını geri fırlatır"
            ]
          ]
        }
      },
      {
        "source_id": "f309a374-44bd-489f-aa88-fe0e9e1071b3",
        "citation_number": 19,
        "cited_text": "Tema: Fracturing'i yaratan varlık. Her şeyin kaynağı — ve belki, bir oyuncu kadar eskiyen biri. Lore: Fracturing bir kaza değildi. Architect, dünyayı kasıtlı kırdı — çünkü kırık dünya daha fazla \"potansiyel\" barındırır. Oyuncu tüm bu potansiyeli içinde topladı. Şimdi asıl savaş bu. Tasarım amacı: Her faz önceki boss'lardan bir ders alır. Oyuncu tüm run boyunca öğrendiklerini burada kullanmak zorunda kalır. Faz 1 — \"Tanıdık\" (HP: 100% → 75%) Act 1 boss'u mirror'lar. Neredeyse birebir aynı saldırılar — ama oyuncu artık çok daha güçlü."
      },
      {
        "source_id": "073a0a1a-7bf5-4eda-bd03-10fd83b40828",
        "citation_number": 20,
        "cited_text": "YENI LOCKED Kararlar (S59): | Alan | Karar | |---|---| | Mimari | Pure 2D Top-Down (Hammerwatch/HLD ref) — 2026-05-12 LOCKED | | Karakter sprite | 64x64 chibi, PixelLab Create Character (Pixen) — 2026-05-12 LOCKED | | Tile sprite | 32x32 top-down grid — 2026-05-12 LOCKED (eski 64x64 floor + 64x128 wall iso REVOKE) | | VFX boyut | 64-128px mix (kucuk vfx 64-80, ultimate 96-128) — 2026-05-12 LOCKED | | Anim yon (MVP) | 4 yon (N/S/E), W=flipX — 8 yon sonra — 2026-05-12 LOCKED (eski karar KEEP) | | Anim fps | 10-12 fps (pixel art ideal) — 2026-05-12 LOCKED | | Renderer | URP 2D Renderer + Pixel Perfect Camera + 2D Lights — 2026-05-12 LOCKED | | Texture filter | Point/Nearest, no compression, no mipmap — 2026-05-12 LOCKED | | PPU | 64 (sprite boyutu = PPU) — 2026-05-12 LOCKED | | Anim view | High top-down ~30-35deg Hades match — KEEP from S57 | | Pixel art only, Blender REJECTED | KEEP from S56 | | UI icon | 32x32 veya 64x64 — 2026-05-12 LOCKED | | Karakter silah entegrasyonu | Silahli 1-piece, sinif silah sabit, PixelLab Create Character native 8-yon — 2026-05-12 LOCKED (silah anchor REVOKED) | | Mob/boss boyut hiyerarsisi (2^n) | 64=char+kucuk/orta mob, 128=elite mob+miniboss, 256=act boss+final boss. Tum sprite PPU=64 standardize. — 2026-05-12 LOCKED | | Final Boss boyut | 256x256 canvas + PPU=64 (sahnede ~2.5x oyuncu, Hades benchmark) — 2026-05-12 LOCKED, eski PPU=32 REVOKE | | Boss mekanik (faz sayisi) | NOT_LOCKED yet — design sprint bekliyor (4 faz vs 3 faz vs adaptive) |"
      },
      {
        "source_id": "f309a374-44bd-489f-aa88-fe0e9e1071b3",
        "citation_number": 21,
        "cited_text": "Faz 3 — \"Boşluk\" (HP: 45% → 20%) Geçiş sahnesi (3 sn): Müzik tamamen kesilir. Arena yavaşça karanlığa gömülür — sadece Architect ve oyuncu kalır. Sessizlikte bir ses: \"Senden önce binlercesi buraya geldi.\" Act 3 boss'un arena mekaniği burada da var — ama bu sefer Architect'in kendisi de bir platform üzerinde, bazı saldırılar sadece doğru platformdan yapılabilir. <cited_table>",
        "cited_table": {
          "num_columns": 2,
          "rows": [
            [
              "Saldırı",
              "Mekanik"
            ],
            [
              "Void Architecture",
              "Architect yeni platformlar yaratır — bazıları tuzak"
            ],
            [
              "Gravity Inversion",
              "5s: yukarı yürünüyor, saldırı yönleri ters"
            ],
            [
              "The Blueprint",
              "Zemine pattern çizer, 3s içinde hasar alanı aktive"
            ],
            [
              "Silence",
              "Tüm skill'leri 4s bloklar — sadece hareket"
            ]
          ]
        }
      },
      {
        "source_id": "f309a374-44bd-489f-aa88-fe0e9e1071b3",
        "citation_number": 22,
        "cited_text": "\"Silence\" mekaniği önemi: Oyuncu 4 saniye hiçbir skill kullanamaz. İlk kez yaşayan oyuncu panikler. İkinci kez sakin kalır. Bu faz \"büyümüş oyuncu\" testi. Faz 4 — \"Gerçek Form\" (HP: 20% → 0%) Geçiş sahnesi (3.5 sn — en uzun geçiş): Arena patlar. Tüm karanlık dağılır. Architect'in gerçek hali görünür — dev değil, küçük. Bir insan. Müzik: yeni, beklenmedik, melodik. \"Tamam. Gerçekten görmek istiyorsan...\" Birkaç saniye sessizlik. Sonra: tema müziği başlar. Faz 4'te tüm önceki mekanikler aynı anda devrede:"
      },
      {
        "source_id": "f309a374-44bd-489f-aa88-fe0e9e1071b3",
        "citation_number": 23,
        "cited_text": "\"Beni tanıyor musun? İlk engelin bendim.\" Tasarım amacı: Oyuncuya ne kadar ilerlediğini hissettir. Act 1 boss'u dev gibi hissettirmiş olabilir — burada aynı saldırılar artık kolay geliyor. Faz 2 — \"Bozulan\" (HP: 75% → 45%) Geçiş sahnesi (2 sn): Architect'in görüntüsü \"glitch\"ler. Fiziksel formu bozulmaya başlar. Sesi kırık çıkar. Faz 2 saldırıları Act 2 boss'tan alınmış — ama daha hızlı ve unpredictable. <cited_table>",
        "cited_table": {
          "num_columns": 2,
          "rows": [
            [
              "Saldırı",
              "Özellik"
            ],
            [
              "Fractured Mirror",
              "Oyuncunun son 3 skill'ini sırayla kopyalar"
            ],
            [
              "Glitch Step",
              "4-5 rapid teleport, son konumda saldırı"
            ],
            [
              "Architect's Echo",
              "Geçmişte öldürülen tüm bossların bir saldırısı rastgele çıkar"
            ],
            [
              "Identity Collapse",
              "Tüm arena 3s \"inverted\" olur — hasar kaynaklarının yerleri değişir"
            ]
          ]
        }
      },
      {
        "source_id": "f309a374-44bd-489f-aa88-fe0e9e1071b3",
        "citation_number": 24,
        "cited_text": "Arena'nın köşeleri hasar veriyor (Act 3) Bazı saldırılar skill'leri kopyalıyor (Act 2) Hız Act 1'in 2 katı Ama oyuncu da en güçlü halinde. Full build, Cross-class Ultimate kullanılabilir. <cited_table>",
        "cited_table": {
          "num_columns": 2,
          "rows": [
            [
              "Saldırı",
              "Mekanik"
            ],
            [
              "Fracture Everything",
              "Tüm önceki boss saldırılarından rastgele 3 tanesi arka arkaya"
            ],
            [
              "The Original Sin",
              "Architect'in ilk saldırısı — basit, yavaş, ama gülünç derecede güçlü"
            ],
            [
              "Build Breaker",
              "Oyuncunun en çok kullandığı skill'i 8s devre dışı bırakır"
            ],
            [
              "Final Architecture",
              "Büyük AoE + kısa süreliğine yenilemez — oyuncunun"
            ]
          ]
        }
      }
    ]
  }
}
