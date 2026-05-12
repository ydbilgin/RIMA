{
  "value": {
    "answer": "RIMA projesi için düşman tasarımı, oda kompozisyon kuralları ve seviye sistemlerine dair kilitlenmiş (LOCKED) son kararlar ve detaylı mekanikler aşağıda sunulmuştur.\n\n### Mob Armor Variant Sistemi (Zırh Varyantları)\nOyun içerisinde düşmanların tekrar edilebilirliğini artırmak ve farklı zorluk seviyeleri yaratmak için Hades tarzı 3 aşamalı bir zırh sistemi kilitlenmiştir [1]. \n*   **Normal:** 1x HP'ye sahip standart varyanttır ve doğrudan kırmızı can barı kullanır [1].\n*   **Armored:** 2x HP'ye sahiptir; önce altın renkli zırh barı tüketilmeli, ardından kırmızı can barına geçilmelidir [1].\n*   **Heavily Armored:** 3x HP'ye sahiptir; gümüş zırh barı, altın bar ve son olarak kırmızı bar şeklinde üç katmanlı savunma sunar [1].\nPixelLab'de sıfırdan üretmek yerine mevcut temel spritelar üzerine \"edit-images-v2\" ile metalik zırhlar eklenerek maliyet düşürülmüştür [1]. Act 1 için belirlenen zırhlı varyantlar; **Shard Bulwark**, **Void Juggernaut**, **Iron Penitent**, **Chain Executioner** ve **Relic Archon** olarak isimlendirilmiştir [1].\n\n### Infighting ve Terrain Hazard (Çevre Tehlikeleri) Kararları\n*   **Infighting (Kendi Arasında Savaşma):** Düşmanların birbirine hasar vurması (infighting) RIMA'da kesinlikle yasaktır [2]. Penitent Bruiser'ın 3 metre yarıçaplı iyileşme azaltan (%50) aurası \"faction-blind\" olarak tanımlansa da, bu durum düşmanların birbiriyle savaşacağı anlamına gelmez; yalnızca auraya giren her birimin (özellikle oyuncunun) can çalma mekaniklerini bloke etmesi için kurgulanmıştır [3], [2]. \n*   **Terrain Hazard (Çevre Tehlikeleri):** Çevre tehlikeleri hikayeye uygun olarak üç derinlik (DepthBand) seviyesine bölünmüştür [2]. F1'de (Act 1) **rift çatlakları**, F2'de (Act 2) **çöken zeminler** ve F3'te (Act 3) **lav + rift alanları** bulunur [2].\n\n### MOB_COMPOSITION_RULES (Oda Kompozisyon Kuralları)\nDüşmanların birlikte ortaya çıkma ihtimallerini dengeleyen katı kurallar `MOB_COMPOSITION_RULES` içerisinde kilitlenmiştir:\n*   **Kural 1 (M06 + M04 Yasağı):** Kalkan veren Relic Caster ile Anti-Heal aurası yayan Penitent Bruiser, F1 derinliğindeki erken odalarda KESİNLİKLE aynı anda çıkamaz [4]. Bu kombinasyon yeni oyuncularda kafa karıştırıcı bir \"hedef önceliği (priority) çatışması\" yaratır, ancak F2/F3 katmanlarında \"advanced room\" olarak izinlidir [4].\n*   **Kural 2 (M08 + M04 Yasağı):** Hollow Hulk ile Penitent Bruiser hiçbir katta aynı odada çıkamaz [5]. Hulk'un sismik darbeleri oyuncuyu dar alana sıkıştırırken Bruiser'ın iyileşme engellemesi kaçış yolunu kapatan haksız bir tuzak yaratır [5].\n*   **Kural 3:** Elit odalar zorlu olmak zorundadır ancak kesinlikle devasa düşmanlar (örn. Hollow Hulk) içermek zorunda değildir; farklı tehdit puanı kombinasyonlarıyla oluşturulabilir [5].\n*   **Kural 4 (Telegraph Eğitimi):** M07 Riftbound Augur'un saldırıları özellikle Act 1'de oyuncuya \"1.2 saniyelik okuma ve reaktif dodge\" yeteneğini öğretmek amacıyla tasarlanmıştır [6].\n\n### Tam 16 Mob Listesi, Rolleri ve Mekanikleri\nOyunun genel düşman havuzu (Roster), 16 benzersiz varlıktan oluşur [7].\n\n**1. Shard Walker (Grunt / Ranged Caster)**\nOrta mesafeden savaşan menzilli birimdir [8]. Oyuncuya 3'lü kristal mermiler fırlatır (Triple Shard) ve öldüğünde 2 metrelik bir \"Fracture Burst\" patlamasıyla hasar verir [9]. Menzili korumayı öğretir [8].\n\n**2. Void Thrall (Grunt / Splitter)**\nZayıf ve yavaş yakın dövüş birimidir [10]. Hedef önceliğini test eder; çünkü öldürüldüğünde daha hızlı ve agresif olan iki adet \"HalfThrall\" birimine bölünür (Death Split) [11], [10].\n\n**3. Penitent Bruiser (Grunt / Bruiser)**\nGövdesi mor ışık yayan kambur bir savaşçıdır [3]. Sürekli olarak 3 metre yarıçaplı, can çalmayı %50 azaltan bir Anti-Heal Aura yayar [3]. Ağır \"Penitent Surge\" alan itmesiyle savaşır [3]. \n\n**4. Chain Warden Echo (Grunt / Controller)**\nOyuncunun hareket kabiliyetini sınayan ve \"dash her şeyi çözer\" ezberini bozan zırhlı bir gardiyandır [12], [13]. Üçlü zinciriyle vurarak fiziksel yavaşlatma uygular ve \"Chain Pull\" yeteneği ile dash yeteneğine bağışık biçimde oyuncuyu kendine çeker [12].\n\n**5. Relic Caster (Grunt / Support)**\nElinde kırık bir mühür tutan kırılgan destek büyücüsüdür [14]. Etrafındaki düşmanlara %50 hasar azaltan kalkan (Aegis Mark) verir ve küçük \"Shardling\" minyonlar çağırır [15]. Her odada öncelikli infaz hedefidir [15], [14].\n\n**6. Fracture Imp (Swarm / Trash)**\nKüçük ve çok hızlı sürü yaratıklarıdır [16]. Oyuncunun etrafını sararak 0.5m mesafeden atılırlar (Rift Lunge) ve öldüklerinde %20 yavaşlatan yapışkan bir zemin sıvısı (Death Splatter) bırakırlar [17]. \n\n**7. Seam Crawler (Grunt / Skirmisher)**\nSabit duran büyücü/menzilli oyuncuları cezalandıran taktiksel bir düşmandır [18]. Zemin çatlaklarında yer altına gizlenerek (Submerge) hasar almaz hale gelir ve gölgesi takip edilmezse altınızdan \"Burst Strike\" yeteneği ile fırlayarak ağır hasar verir [19]. \n\n**8. Hollow Mite (Swarm)**\nSürü halinde hızlıca zikzak çizerek yaklaşan minik düşmanlardır [20], [7].\n\n**9. Echo Hound (Grunt)**\nHızlı ışınlanma (blink) mekaniklerine sahip bir tazıdır [21]. Çıkardığı yanılsama kopyalarıyla (echo afterimage) oyuncuyu farklı noktalardan aynı anda tehdit eder ve ses odaklı aggro (tehdit) çeker [22]. \n\n**10. Twice-Born (Elite Pair)**\nAynı bedenin iki farklı yüzünü yansıtan elit bir ikilidir [23]. Hasarı %50 oranında paylaşırlar; strateji ikisine aynı anda hasar vurmaktır, çünkü birisi öldüğünde hayatta kalan anında durdurulamaz bir berserk moduna girer [23]. \n\n**11. Fracture-Born (Elite)**\nArena zeminindeki bir çatlaktan yavaşça 4 aşamada doğar [22]. Erken hasar verilirse doğum (spawn) aşamasındayken yok edilebilir, yok edilemezse arenadaki en büyük tehdide dönüşür [22].\n\n**12. Spore Hollow (Elite)**\nYavaş hareket eden ancak oyuncuyu sürekli izole etmeye çalışan elit birimdir [24]. Zemin üzerine zehirli spor bulutları bırakır ve öldüğünde geniş bir zehir alanı bırakarak odayı kaplar [24], [7].\n\n**13. Rift Maw (Elite)**\nSabit duran ve arena kontrolü yapan devasa bir elit objedir [25]. Kendi etrafında yavaş ama sürekli bir çekim alanı yaratırken arenaya sürekli olarak daha küçük düşmanlar spawn eder [25], [7].\n\n**14. Class Mimic (Special)**\nFracturing olayının oyuncunun varlığından kopyaladığı bir yansımadır [26], [25]. Oyuncunun kullandığı ana sınıf yeteneklerini birebir taklit eder ve kendi build'inizi size karşı kullanır [27], [25].\n\n**15. Remnant Host (Special)**\nOda içerisinde her 15 saniyede bir 3 farklı ruh formu arasında değişim yapan özel birimdir [25]. Hangi ruha bürünürse ona uygun bir direnç ve saldırı seti kazanır [25].\n\n**16. The Wound (Special)**\nOdadaki en zayıf ama en tehlikeli özel düşmandır [20]. Oyuncuya saldırmaz, ancak etrafındaki tüm düşmanlara sürekli olarak güçlü bir can yenileme (heal) sağlar [20], [24]. Eğer onu infaz etmeyi başarırsanız patlayarak odadaki tüm düşmanlara %20 HP hasarı vurur [20].\n\n### Düşmanların Fazlara (Act 1-5) Göre Dağılımı\nGeliştirme süreci ve dikey kesit (Vertical Slice) mantığıyla düşmanların oyuna eklenme fazları şu şekilde kilitlenmiştir:\n*   **Faz 1 (Combat Prototipi):** Savaşın çekirdeğini test etmek için temel Act 1 düşmanları üretilir [28]. **ShardWalker, VoidThrall, Penitent, ChainWarden, RelicCaster, FractureImp, SeamCrawler ve Hollow Mite** bu fazda aktifleşir [29], [20].\n*   **Faz 2 (İlk Oynanabilir Loop):** Act 1 tamamen inşa edilirken ilk elit tehdidi olan **Twice-Born** oyuna eklenir [30].\n*   **Faz 3 (Secondary Class ve Act 2):** İkinci Act'in çürüyen atmosferini desteklemek üzere **Echo Hound** ve **Fracture-Born** oyuna girer [21], [22].\n*   **Faz 4 (Tam Demo):** Act 2'nin elit gücünü test etmek için **Spore Hollow** ve özel hedef **The Wound** eklenir [31], [24].\n*   **Faz 5 (Tam Oyun/Early Access):** Oyunun Act 3 ve nihai safhalarını desteklemek üzere **Rift Maw**, **Class Mimic** ve **Remnant Host** oyuna katılır [32], [25].",
    "conversation_id": "a026df9f-7689-4086-baf2-fbe552b76536",
    "sources_used": [
      "476ee859-fdfe-4c9e-bb0e-c9ca259b4cfc",
      "073a0a1a-7bf5-4eda-bd03-10fd83b40828",
      "8509dc23-8a3b-4b13-b32a-06cf539d7b7b",
      "55bba95f-6c9d-4c0b-b1d2-e4d1962efcb6",
      "99401014-de6f-417d-89bd-e8ddc03b3ffa",
      "b342785f-351c-48db-a85e-b4a951efae39",
      "b2b48d38-e9b5-49e0-9412-1a54fbcec059",
      "cebbb0b5-1d67-4310-a407-6b87bdd117fe",
      "4d875caf-b606-46c7-8452-0ec324c56045",
      "dd900e61-2c7b-4519-aa26-79526075ad8e",
      "38ddcede-8fab-42e8-8bab-cc72d3f0dcb8"
    ],
    "citations": {
      "1": "476ee859-fdfe-4c9e-bb0e-c9ca259b4cfc",
      "2": "073a0a1a-7bf5-4eda-bd03-10fd83b40828",
      "3": "8509dc23-8a3b-4b13-b32a-06cf539d7b7b",
      "4": "55bba95f-6c9d-4c0b-b1d2-e4d1962efcb6",
      "5": "55bba95f-6c9d-4c0b-b1d2-e4d1962efcb6",
      "6": "55bba95f-6c9d-4c0b-b1d2-e4d1962efcb6",
      "7": "99401014-de6f-417d-89bd-e8ddc03b3ffa",
      "8": "8509dc23-8a3b-4b13-b32a-06cf539d7b7b",
      "9": "8509dc23-8a3b-4b13-b32a-06cf539d7b7b",
      "10": "8509dc23-8a3b-4b13-b32a-06cf539d7b7b",
      "11": "b342785f-351c-48db-a85e-b4a951efae39",
      "12": "8509dc23-8a3b-4b13-b32a-06cf539d7b7b",
      "13": "8509dc23-8a3b-4b13-b32a-06cf539d7b7b",
      "14": "8509dc23-8a3b-4b13-b32a-06cf539d7b7b",
      "15": "8509dc23-8a3b-4b13-b32a-06cf539d7b7b",
      "16": "8509dc23-8a3b-4b13-b32a-06cf539d7b7b",
      "17": "8509dc23-8a3b-4b13-b32a-06cf539d7b7b",
      "18": "8509dc23-8a3b-4b13-b32a-06cf539d7b7b",
      "19": "8509dc23-8a3b-4b13-b32a-06cf539d7b7b",
      "20": "b2b48d38-e9b5-49e0-9412-1a54fbcec059",
      "21": "cebbb0b5-1d67-4310-a407-6b87bdd117fe",
      "22": "cebbb0b5-1d67-4310-a407-6b87bdd117fe",
      "23": "8509dc23-8a3b-4b13-b32a-06cf539d7b7b",
      "24": "4d875caf-b606-46c7-8452-0ec324c56045",
      "25": "dd900e61-2c7b-4519-aa26-79526075ad8e",
      "26": "8509dc23-8a3b-4b13-b32a-06cf539d7b7b",
      "27": "8509dc23-8a3b-4b13-b32a-06cf539d7b7b",
      "28": "b2b48d38-e9b5-49e0-9412-1a54fbcec059",
      "29": "b2b48d38-e9b5-49e0-9412-1a54fbcec059",
      "30": "38ddcede-8fab-42e8-8bab-cc72d3f0dcb8",
      "31": "4d875caf-b606-46c7-8452-0ec324c56045",
      "32": "dd900e61-2c7b-4519-aa26-79526075ad8e"
    },
    "references": [
      {
        "source_id": "476ee859-fdfe-4c9e-bb0e-c9ca259b4cfc",
        "citation_number": 1,
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
        "source_id": "073a0a1a-7bf5-4eda-bd03-10fd83b40828",
        "citation_number": 2,
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
        "source_id": "8509dc23-8a3b-4b13-b32a-06cf539d7b7b",
        "citation_number": 3,
        "cited_text": "-------------------------------------------------------------------------------- M04 — Penitent Bruiser Role: Bruiser (telegraphed heavy) Fracture Trait: Fracturing enerjisini içine kapattı; göğsünden mor ışık dalgaları yayılıyor. Silüet: 128×128, omuzları aşağı çökmüş, kambur gövde, baş eğik. Auto-attack: Slow melee swing (1.0s telegraph, 30 hasar) — pozisyonel pressure, ana tehdit değil. Skill 1 — Anti-Heal Aura (pasif): 3m radius, içindeyken lifesteal/heal %50 azalır. Skill 2 — Penitent Surge: 1.2s tell, 3m radius AOE itme + 35 hasar + 0.5s stagger. Window 1.5s. Warblade Counter: Aura'yı zorla → 3m içine gir, Surge'den dodge, LMB sweep. Synergy: Imp ile → aura içinde Imp goo + Bruiser swing = sandwich."
      },
      {
        "source_id": "55bba95f-6c9d-4c0b-b1d2-e4d1962efcb6",
        "citation_number": 4,
        "cited_text": "RIMA — Mob Kompozisyon Kurallari Status: v1 LOCKED — 2026-05-09 Kural 1 — M06 + M04 Birliktelik Yasagi Relic Caster (M06) + Penitent Bruiser (M04) ayni odada YASAK — F1 katinda. Sebep: M06'nin Aegis'i + M04'un Anti-Heal aurasi birlesince priority conflict yaratir. Oyuncu \"hangi threat once?\" sorusunu F1'de henuz bilmiyor. F2/F3 istisnasi: Bu kombinasyon \"advanced room\" olarak F2/F3'te izinli. Kural: Composition tag → advanced_priority_conflict , sadece depth 3+ odalarda secilir. Kural 2 — M08 + M04 Birliktelik Yasagi"
      },
      {
        "source_id": "55bba95f-6c9d-4c0b-b1d2-e4d1962efcb6",
        "citation_number": 5,
        "cited_text": "Hollow Hulk (M08) + Penitent Bruiser (M04) ayni odada YASAK — tum katlarda. Sebep: M08 charge/deprem AoE'si oyuncuyu merkeze sikistirir. M04 aurasi iyilesme engeller. Arena dar + sustain yok = oyuncu kacis yolu bulamiyor. Damage + sustain denial birlesimi her katman icin too hard. Istisna: Boss odasi degerlendirmesi ayri — arena buyukse (30x30+) degerlendirilebilir. Kural 3 — Elite Oda M08 Zorunlulugu Elite odalar M08 icermek ZORUNDA DEGIL. Alternatif elite kombo ornegi: 2xM06(4pt) + 2xM04(4pt) + 2xM01(1pt) = 18pt — YASAK (Kural 1 ihlali, F1) 2xM07(3pt) + 2xM05(4pt) + 2xM02(3pt) = 20pt — gecerli elite (M08 yok)"
      },
      {
        "source_id": "55bba95f-6c9d-4c0b-b1d2-e4d1962efcb6",
        "citation_number": 6,
        "cited_text": "Kural: Elite oda tanimi \"zorlu kompozisyon\", sadece M08 degil. Kural 4 — M07 Telegraph Trainer M07 Riftbound Augur: Augur saldirilari 1.2s belirgin alan telegraph birakir. Oyuncu bu pencerede dash etmezse +%50 hasar alir. Tasarim amaci: Act 1'de \"telegraph okuma + reaktif dodge\" kalibini ogretmek. Bu, Act 1 Boss (Penitent Sovereign) Faz 2 mekanikleri icin hazirlik. Mevcut tasarim (sadece debuff) bu rolu karsilamiyor — guncelleme bekliyor. Status: TODO v1 — Augur tanimi guncellenmeli. Versiyonlama <cited_table>",
        "cited_table": {
          "num_columns": 3,
          "rows": [
            [
              "Versiyon",
              "Tarih",
              "Degisim"
            ],
            [
              "v1.0",
              "2026-05-09",
              "Ilk kompozisyon kurallari (Opus denge review)"
            ]
          ]
        }
      },
      {
        "source_id": "99401014-de6f-417d-89bd-e8ddc03b3ffa",
        "citation_number": 7,
        "cited_text": "MOB DAĞILIM HARİTASI (Tüm Aktlar) Set A — Mevcut Prefablar (MEKANIK_KARARLARI) <cited_table> Set B — Planlanan (MOB_TASARIMI) <cited_table>",
        "cited_table": {
          "num_columns": 6,
          "rows": [
            [
              "Mob",
              "Act 1",
              "Act 2",
              "Act 3",
              "Boyut",
              "Tier"
            ],
            [
              "ShardWalker",
              "✅",
              "✅",
              "✅",
              "128px (S43)",
              "Grunt"
            ],
            [
              "VoidThrall (+HalfThrall)",
              "✅",
              "✅",
              "—",
              "128px (S43)",
              "Grunt"
            ],
            [
              "Penitent",
              "✅",
              "✅",
              "—",
              "128px (S43)",
              "Grunt"
            ],
            [
              "ChainWarden",
              "✅",
              "✅",
              "✅",
              "128px (S43)",
              "Grunt"
            ],
            [
              "RelicCaster",
              "—",
              "✅",
              "✅",
              "128px (S43)",
              "Grunt"
            ],
            [
              "FractureImp",
              "✅",
              "✅",
              "—",
              "128px (S43)",
              "Grunt"
            ],
            [
              "SeamCrawler",
              "✅",
              "✅",
              "—",
              "256×128px (S43)",
              "Grunt"
            ]
          ]
        }
      },
      {
        "source_id": "8509dc23-8a3b-4b13-b32a-06cf539d7b7b",
        "citation_number": 8,
        "cited_text": "-------------------------------------------------------------------------------- NORMAL DÜŞMANLAR (Act 1) 1. Shard Walker (mevcut) Tür: Grunt / Fractured Combat Rolü: Ranged pressure + death AoE Oyuncuya Sorduğu Soru: \"Mesafeyi nasıl yönetirsin?\" Ana Davranış: Orta mesafeden shard fırlatır (3'lü yayılım), ölünce patlama Zorladığı Build: Close-range melee — yaklaşmak zor, uzak durmak da değil Zayıf Yönü: Hareketli oyuncuya isabet ettirmesi zor; dash build'leri kolayca geçer Siluet: Çok parçalı, dağınık insansı; gap'lerden ışık sızıyor Boyut: 64px → 112px (player boyutuna yakın grunt — tehdit hissettirmeli)"
      },
      {
        "source_id": "8509dc23-8a3b-4b13-b32a-06cf539d7b7b",
        "citation_number": 9,
        "cited_text": "-------------------------------------------------------------------------------- M02 — Shard Walker Role: Ranged Caster Fracture Trait: Vücudu yarısından kopmuş kristal levhalardan oluşuyor; her atışta bir levha kopup fırlatılıyor. Silüet: 112×112, dik dar gövde, omuzdan 3-4 keskin shard çıkıntısı. Auto-attack: Yok. Skill 1 — Triple Shard: 0.8s tell (3 shard parlar), 3 ardışık projectile (15° yelpaze), her biri 18 hasar. Skill 2 — Fracture Burst: Death triggered, 0.5s sonra 2m radius patlama (25 hasar). Codex Adjustment: Triple Shard → minimum range deadzone var (Warblade dash-in geçerli yanıt). Warblade Counter: Dash forward (yelpazeden geç), LMB-LMB-Ram. Ram knockback ile death burst'ten kaç. Synergy: Bruiser ile → Bruiser'ı kite ederken Walker arkadan triple shard."
      },
      {
        "source_id": "8509dc23-8a3b-4b13-b32a-06cf539d7b7b",
        "citation_number": 10,
        "cited_text": "-------------------------------------------------------------------------------- 3. Void Thrall (mevcut) Tür: Splitter / Fractured Combat Rolü: Priority target — öldür ama dikkatli öldür Oyuncuya Sorduğu Soru: \"Hedef önceliğin var mı?\" Ana Davranış: Ölünce iki HalfThrall'a bölünür; yarılar daha hızlı Zorladığı Build: AoE — thrall'ı AoE ile öldürünce iki sorun olur Zayıf Yönü: Single target burst ile temiz öldürülünce split kontrollü Siluet: Uzun, ince, void tendrilleri; soluk mor aura Boyut: 96px → 128px (player boyutunda — split olmadan da tehdit hissettirmeli)"
      },
      {
        "source_id": "b342785f-351c-48db-a85e-b4a951efae39",
        "citation_number": 11,
        "cited_text": "VoidThrall — \"Void Pulse\" Kimlik: Void enerjisiyle şişirilmiş ağır düşman. Patlama riski barındırıyor. Attack: Kollarını açıp göğsünden void enerji dalgası yayıyor. AoE yakın mesafe. Frame 1-2: Kollar geriye açılıyor Frame 3-4: Göğüs şişiyor, void enerji birikimi Frame 5-6: Enerji patlar, kollar öne geliyor Mekanik: AoE melee. VoidThrall_DeathSplit ile sinerji (öldürünce HalfThrall spawn). Animasyon sayısı: 4 yön × 6 frame + death 4 yön × 6 frame --------------------------------------------------------------------------------"
      },
      {
        "source_id": "8509dc23-8a3b-4b13-b32a-06cf539d7b7b",
        "citation_number": 12,
        "cited_text": "-------------------------------------------------------------------------------- M05 — Chain Warden Echo Role: Charger / Mobility Punisher Fracture Trait: Eski hapishane muhafızının yankısı; zincirleri rift enerjisinden örülmüş. Silüet: 128×128, ağır zırh göğsü, omuzlardan 2 zincir uçuşan tendril. Auto-attack: Yok. Skill 1 — Triple Chain: 0.7s tell, 3 zincir 45° yelpazede (6m menzil). Hit = 1.5s slow %50. Skill 2 — Chain Pull: 1.0s tell, Warb 4m boss yönüne çekilir + 20 hasar. Dash-immune. Pull sonrası 1.2s window. Codex Adjustment: Chain Pull → dash-immune ama Iron Counter/parry ile kırılabilir. Warblade Counter: Triple Chain → dash side. Chain Pull → erken parry timing veya pull sonrası burst. Faz 2 Note: Pull dash-immune → Ranger/Gunslinger için gerçek tehdit. Synergy: Seam Crawler + Warden = en yüksek gerilim. ERKEN ODALARDA BERABER SPAWN YASAK."
      },
      {
        "source_id": "8509dc23-8a3b-4b13-b32a-06cf539d7b7b",
        "citation_number": 13,
        "cited_text": "-------------------------------------------------------------------------------- 4. Chain Warden Tür: Controller / Fractured Combat Rolü: Mobility Check — dash ve kite'ı cezalandırır Lore: Hapisane muhafızının kalıntısı. Zincirleri hâlâ uçuşuyor, ama artık onu kontrol eden yok. Oyuncuya Sorduğu Soru: \"Dash her şeyi çözer mi?\" Ana Davranış: 3 zincir fırlatır — her biri ayrı yöne, birbirinden 45° açıyla İsabet → 1.5s yavaşlama (Chill değil, fiziksel slow) Dash ile kaçınılır ama \"dash = tanklama değil\" öğretir Zorladığı Build: Melee sustain (yerinde duruyorsa zincirleri birden yer) Zayıf Yönü: Zincir fırlatma sonrası 2s açık pencere — telegraph açık Oda Kompozisyonu: Seam Crawler ile → oyuncu hem kaçamaz hem flankan yiyor Siluet: Ağır zırhlı ama zincirler silueti karmaşıklaştırıyor — zincirler belirgin Boyut: 80px → 128px (player boyutunda controller — görsel ağırlık zorunlu) Varyant: \"Rusted Warden\" — daha yavaş ama zincir hasarı daha yüksek (Act 1 geç)"
      },
      {
        "source_id": "8509dc23-8a3b-4b13-b32a-06cf539d7b7b",
        "citation_number": 14,
        "cited_text": "-------------------------------------------------------------------------------- 6. Relic Caster Tür: Support / Fractured Combat Rolü: Execution Target — önce bu öldürülmeli Lore: Bir zamanlar mühür tutan büyücü. Mührü kırıldı ama büyüyü bilmeye devam ediyor. Oyuncuya Sorduğu Soru: \"Önce kimi öldürürsün?\" Ana Davranış: Yakın düşmana kalkan verir (2s, kırılabilir ama zaman alır) Kendisi zayıf — öldürmek kolay ama diğerleri kalkan alıyor Kalkan verme cooldown: 4s — window var Zorladığı Build: Burst — önce caster öldürülmezse burst'ün tüketir Zayıf Yönü: En kırılgan düşman — sadece öncelik sorunu Oda Kompozisyonu: Chain Warden + Relic Caster → zincirlenirken caster'ı öldürmeye çalışmak Siluet: İnce, yüksek; elinde kırık relikvar; siluet net ve tek Boyut: 80px (kasıtlı fragile support — oda içindeki en küçük figür, kolay hedef okunması zorunlu) Varyant: \"Heretic Caster\" (Act 2) — kalkan yerine anti-heal debuff"
      },
      {
        "source_id": "8509dc23-8a3b-4b13-b32a-06cf539d7b7b",
        "citation_number": 15,
        "cited_text": "-------------------------------------------------------------------------------- M06 — Relic Caster Role: Summoner / Spawner + buff support Fracture Trait: Elinde tuttuğu kırık relikvarın içinden minik rift fragmanları çağırıyor. Silüet: 80×80 (KÜÇÜK — execution priority cue), ince uzun gövde, dik kristal kırığı. Auto-attack: Yok. Skill 1 — Summon Shardling: 1.5s channel (relikvar parlar), 1.5m radius'ta 1 Shardling spawn (15 HP). CD 6s. Kanaldayken savunmasız. Skill 2 — Aegis Mark: 0.5s tell, en yakın allied mob'a 3s damage shield (%50 reduction). CD 5s. Mark'lı mob altın aura. Codex Adjustment: Aegis Mark shield break reward — hem target hem caster 0.5s stagger. Warblade Counter: EXECUTION TARGET. Channeling sırasında dash-in + LMB combo. Synergy: Chain Warden + Caster → Warden Mark'lı, Pull immune, Warb kapana sıkışır."
      },
      {
        "source_id": "8509dc23-8a3b-4b13-b32a-06cf539d7b7b",
        "citation_number": 16,
        "cited_text": "-------------------------------------------------------------------------------- 7. Fracture Imp Tür: Swarm Skirmisher / Rift-Born Combat Rolü: AoE Bait — AoE olmayanı boğar, AoE olanı kolayca temizlenir Lore: Çatlaktan fışkırdı. Küçük, ahmak ama hızlı ve çok. Oyuncuya Sorduğu Soru: \"Yavaş build'inle sürüyü nasıl yönetirsin?\" Ana Davranış: 3-4 aynı anda spawn — her biri zayıf Hızlı melee, önce çevreliyor sonra saldırıyor Tek başına tehlike yok, kalabalıkta overwhelm eder Zorladığı Build: Single-target burst — teke tek temizler ama yorulur Zayıf Yönü: AoE veya cleave ile tek hareket Oda Kompozisyonu: Shard Walker ile — Walker odağı çekerken Imp'ler çevreliyor Siluet: Küçük, sivri; net ve farklı — diğer düşmanlarla karışmıyor Boyut: 48px (true swarm tipi — kasıtlı küçük, ama 32px altı olmaz)"
      },
      {
        "source_id": "8509dc23-8a3b-4b13-b32a-06cf539d7b7b",
        "citation_number": 17,
        "cited_text": "Önerilen encounter'lar: \"Triple Threat\" (8pt): Walker + Imp×3 + Crawler \"Lockdown\" (10pt): Warden + Crawler + Imp×3 \"Execution Test\" (9pt): Caster + Warden + Imp×2 \"Aura Trap\" (10pt): Bruiser + Augur + Walker \"Mini-Boss Solo\" (8pt, Elite oda): Hollow Hulk solo -------------------------------------------------------------------------------- M01 — Fracture Imp Role: Trash / Swarm Fracture Trait: Çatlaktan fışkırmış, gövdesinde ışık sızdıran açık deliklerden rift enerjisi damlıyor. Silüet: 48×48, sivri uzun kollar, gövde küçük üçgen, başı silüetin %40'ı. Auto-attack: Yok. Temas hasarı 5 (negligible). Skill 1 — Rift Lunge: 0.4s cup-back tell, 0.5m ileri sıçrayış + 12 hasar. Recovery 0.6s (savunmasız window). Skill 2 — Death Splatter: Ölünce 0.3s sonra 1m radius rift goo (3s slow %20). Codex Adjustment: Death Splatter slow → player başına max 1 aktif debuff. Warblade Counter: LMB sweep tek vuruşta öldürür. Dash attack 3-4 imp grubunu temizler. Synergy: Shard Walker'la → Walker odağı çekerken Imp'ler arkadan lunge. Splatter goo'lar dash lane'ini kapatır."
      },
      {
        "source_id": "8509dc23-8a3b-4b13-b32a-06cf539d7b7b",
        "citation_number": 18,
        "cited_text": "-------------------------------------------------------------------------------- 2. Seam Crawler (mevcut) Tür: Skirmisher / Rift-Born Combat Rolü: Fast flanker, anti-kiting Oyuncuya Sorduğu Soru: \"Çevrenin farkında mısın?\" Ana Davranış: Zemin çatlaklarında kayar, çıkış animasyonu öncesinde görünmez Zorladığı Build: Turret-style caster; sabit duran oyuncuyu punish eder Zayıf Yönü: Yavaş ama öngörülü — çıkış noktası okunabilir Siluet: Yassı, zemine yapışık; sadece pençe ve omurga görünür — ama GENİŞ (yatay tehdit) Boyut: 48px → 96px (zemine yapışık ama geniş — yatay kaplama alanı büyük)"
      },
      {
        "source_id": "8509dc23-8a3b-4b13-b32a-06cf539d7b7b",
        "citation_number": 19,
        "cited_text": "-------------------------------------------------------------------------------- M03 — Seam Crawler Role: Skirmisher (hit-and-run) Fracture Trait: Zemindeki çatlaklarda yaşıyor; üst silüeti bir parça ışık ve gölge. Silüet: 96×96 ama yatay (geniş, 30px yükseklik), zemine yapışık, 6 pençe çıkıntısı. Auto-attack: Yok. Skill 1 — Submerge (pasif): %50 zaman zemin altında. Underground iken hasar almaz. 1.5m radius dim distortion shader. Skill 2 — Burst Strike: 1.0s yer altı approach (gölge telegraph), zeminden fırlayış 1m radius bite (28 hasar + 0.5s knockback). Burst sonrası 1.4s exposed. Codex Adjustment: Submerge strict max duration 2s + visible distortion zorunlu. Chain-submerge yasak. Warblade Counter: Gölgeyi takip et, fırladığı an dash-back + LMB combo. Synergy: Chain Warden ile → Warden zincirleri kilitterken Crawler burst strike. ERKEN ODALARDA BİRLİKTE SPAWN YASAK."
      },
      {
        "source_id": "b2b48d38-e9b5-49e0-9412-1a54fbcec059",
        "citation_number": 20,
        "cited_text": "Swarm Tier (48px) <cited_table> Elite Varyantlar (Bu fazda) Her grunt mob'a 1 affix uygulanabilir: <cited_table> Özel Mob (Nadir) <cited_table>",
        "cited_table": {
          "num_columns": 3,
          "rows": [
            [
              "Mob",
              "Mekanik",
              "Sprite Durumu"
            ],
            [
              "Hollow Mite",
              "Çok hızlı, zigzag, sürü taktik (4-8 birden spawn)",
              "❌ Üretilecek"
            ]
          ]
        }
      },
      {
        "source_id": "cebbb0b5-1d67-4310-a407-6b87bdd117fe",
        "citation_number": 21,
        "cited_text": "\"Kaçış İzi\": Dash CD -%20 \"Gölge Hız\": 8s hareket hızı +%30 \"Yankı Adımı\": Dash sırasında dodging frame +0.5s Blood Oracle 🩸 Tema: HP trade, lifesteal Koşul: HP %60 altındaysa en güçlü seçenek görünür Offer örnekleri: \"Kan Paktı\": HP -%15, tüm hasar +%25 (kalıcı, run süresince) \"Sızıntı\": Her kill'den +3 HP (HP %60 altı) \"Son Damlam\": HP -%30, tüm skill'ler 1 tier atladı -------------------------------------------------------------------------------- MOB'LAR (Faz 2'ye ek — Act 2) Yeni Grunt (96px)"
      },
      {
        "source_id": "cebbb0b5-1d67-4310-a407-6b87bdd117fe",
        "citation_number": 22,
        "cited_text": "<cited_table> Yeni Elite (160px) <cited_table> Act 2 Mob Dağılımı <cited_table> --------------------------------------------------------------------------------",
        "cited_table": {
          "num_columns": 4,
          "rows": [
            [
              "Mob",
              "Act",
              "Mekanik",
              "Sprite"
            ],
            [
              "Echo Hound",
              "2, 3",
              "Hızlı blink + echo afterimage (iki noktadan hasar), ses çeker",
              "❌ Üretilecek"
            ]
          ]
        }
      },
      {
        "source_id": "8509dc23-8a3b-4b13-b32a-06cf539d7b7b",
        "citation_number": 23,
        "cited_text": "-------------------------------------------------------------------------------- E2. The Twice-Born (mevcut) Tür: Elite Pair / Fractured Combat Rolü: Resource Pressure — ikisini aynı anda yönet Oyuncuya Sorduğu Soru: \"AoE mi yoksa focus fire mı?\" Ana Davranış: Hasar birbirine %50 bölünür; biri ölünce diğeri berserk Zorladığı Build: AoE — her ikisini zayıflatır ama berserk'i önlemez Siluet: İkisi birbirinin aynası — ama renk ton farkı var (birincil/ikincil) Boyut: 80px → 128px + 128px (elite pair — her biri player boyutunda, ikisi birden baskılayıcı)"
      },
      {
        "source_id": "4d875caf-b606-46c7-8452-0ec324c56045",
        "citation_number": 24,
        "cited_text": "-------------------------------------------------------------------------------- MOB'LAR (Faz 3'e ek) <cited_table> -------------------------------------------------------------------------------- BOSS: FRACTURE SOVEREİGN (Act 3 — Sadece F1 test) Tam boss Faz 5'te. Bu fazda sadece Faz 1 prototip. Faz 1 — \"Yara Açıldı\" (HP: 100% → 60%)",
        "cited_table": {
          "num_columns": 5,
          "rows": [
            [
              "Mob",
              "Act",
              "Mekanik",
              "Tier",
              "Sprite"
            ],
            [
              "Spore Hollow",
              "2",
              "Yavaş yaklaşım, zemine spor bulutu, ölünce büyük poison AoE",
              "Elite (160px)",
              "❌"
            ],
            [
              "The Wound",
              "1,2,3",
              "Pasif iyileştirici, önce öldürülmesi gereken hedef",
              "Special (128px)",
              "❌"
            ]
          ]
        }
      },
      {
        "source_id": "dd900e61-2c7b-4519-aa26-79526075ad8e",
        "citation_number": 25,
        "cited_text": "YENİ MOB'LAR <cited_table> -------------------------------------------------------------------------------- BOSS: FRACTURE SOVEREİGN (Tam — 3 Faz) Faz 1 — \"Yara Açıldı\" (100% → 60%) Faz 4'ten aynen. Faz 2 — \"Çevre Uyanıyor\" (60% → 30%) Arena köşelerinde Fracture Node aktive olur. Arena küçülür. <cited_table>",
        "cited_table": {
          "num_columns": 4,
          "rows": [
            [
              "Mob",
              "Act",
              "Tier",
              "Mekanik"
            ],
            [
              "Rift Maw",
              "3",
              "Elite (160px)",
              "Sabit, çekim alanı, düşman spawn'lar"
            ],
            [
              "Class Mimic",
              "2,3",
              "Special (128px)",
              "Oyuncunun class skill'lerini kopyalar"
            ],
            [
              "Remnant Host",
              "3",
              "Special (160px)",
              "3 ruh, her 15s ruh değişir, farklı direnç/saldırı"
            ]
          ]
        }
      },
      {
        "source_id": "8509dc23-8a3b-4b13-b32a-06cf539d7b7b",
        "citation_number": 26,
        "cited_text": "-------------------------------------------------------------------------------- E3. Fracture Knight Tür: Elite Skirmisher / Emergent Combat Rolü: Mirror Threat — oyuncunun dash'ini taklit eder Lore: The Fracturing anında bir şampiyonun hareket örüntüsünü emdi. Artık onun gibi hareket ediyor. Oyuncuya Sorduğu Soru: \"Dash'ini ne zaman ve neden kullanıyorsun?\" Ana Davranış: Oyuncu dash kullandıktan 0.5s sonra Fracture Knight de dash yapar — aynı yönde \"Mirror dash\" görsel cue ile belirtilir — öğrenilebilir Yüksek hasar ama savunmasız hemen sonrası pencere var Zorladığı Build: Dash-heavy build — kendi silahıyla vurulur Zayıf Yönü: Mirror dash sonrası 1s stun window Oda Kompozisyonu: Chain Warden ile → dash'ini Warden için kullanırsan Knight sırtında Grudge Bağlantısı: ✅ — özellikle dash ile öldürülünce sonraki Fracture Knight daha çok mirror eder Boyut: 80px → 160px (elite skirmisher — oyuncudan açık büyük, ama hızlı — \"büyük ama hızlı\" paradoksu tehdit hissini artırır)"
      },
      {
        "source_id": "8509dc23-8a3b-4b13-b32a-06cf539d7b7b",
        "citation_number": 27,
        "cited_text": "-------------------------------------------------------------------------------- E2. Mirror Paladin Tür: Elite Mirror / Fractured Combat Rolü: Skill Reflection + Anti-Build Lore: Bir şampiyonun gölgesi. Ama gölge şimdi kendi ışığını yapıyor. Oyuncuya Sorduğu Soru: \"Build'ine karşı kendi build'in.\" Ana Davranış: Açılışta oyuncunun equipped skill'lerini \"tarar\" (2s) Her equipped skill için bir \"echo\" saldırısı oluşturur Oyuncu skill değiştirirse tarama yenilenir Zorladığı Build: Spesifik combo build → kendi combo'su döner Zayıf Yönü: Erken burst (2s tarama bitince aktif) → tarama tamamlanamaz Boyut: 96px, oyuncunun renk şemasında ama bozuk"
      },
      {
        "source_id": "b2b48d38-e9b5-49e0-9412-1a54fbcec059",
        "citation_number": 28,
        "cited_text": "FAZ 1 — CORE LOOP (Combat Prototype) Claude: Sadece bu dosyayı oku. Bu fazın scope'u dışındaki hiçbir şeyi implemente etme. -------------------------------------------------------------------------------- SCOPE Hedef: Combat hissi çalışıyor mu? 1 class ile oda temizleme loop'u test edilebilir. Ne VAR: 1 oynanabilir class (Warblade) Act 1 odaları (8-9 oda, sadece Combat + Elite tipleri; opsiyonel 1 Shop) Temel düşman AI (7 mob) Skill draft UI (Common tier) Rage sistemi Ölüm + restart Map fragment (kısmi görünür harita) Act 1 boss (Penitent Sovereign — Faz 1 sadece)"
      },
      {
        "source_id": "b2b48d38-e9b5-49e0-9412-1a54fbcec059",
        "citation_number": 29,
        "cited_text": "Resource Bar UI: Rage bar: çatlak, sert, agresif form — kenarlarda kırmızı enerji Dolunca: Bladestorm kullanılabilir göstergesi (parıldama) -------------------------------------------------------------------------------- MOB'LAR (7 Mob — Act 1) Grunt Tier (96px) <cited_table>",
        "cited_table": {
          "num_columns": 3,
          "rows": [
            [
              "Mob",
              "Mekanik",
              "Sprite Durumu"
            ],
            [
              "ShardWalker",
              "Orta hız, parça fırlatma, ölünce parça AoE",
              "✅ Prefab hazır"
            ],
            [
              "VoidThrall",
              "Yakın dövüş, ölünce İKİYE BÖLÜNÜR (2× HalfThrall)",
              "✅ Prefab hazır"
            ],
            [
              "Penitent",
              "3-hit combo, son vuruş armor break",
              "✅ Prefab hazır"
            ],
            [
              "ChainWarden",
              "Oyuncuyu zincirle çeker",
              "✅ Prefab hazır"
            ],
            [
              "FractureImp",
              "Melee + shard scatter ikincil hasar",
              "✅ Prefab hazır"
            ],
            [
              "SeamCrawler",
              "Zemin çatlaklarında kayar, ambush",
              "✅ Anim hazır"
            ]
          ]
        }
      },
      {
        "source_id": "38ddcede-8fab-42e8-8bab-cc72d3f0dcb8",
        "citation_number": 30,
        "cited_text": "Build Eksenleri: Sniper / Trap Master / Kite Lord Resource Bar UI: Menzile göre dolan ince halka — uzaktan dolu, yakından boşalır -------------------------------------------------------------------------------- MOB'LAR (9 Mob — Act 1) Faz 1'deki 7 mob'a ek olarak: Yeni Grunt <cited_table> Yeni Elite (160px) <cited_table> --------------------------------------------------------------------------------",
        "cited_table": {
          "num_columns": 3,
          "rows": [
            [
              "Mob",
              "Mekanik",
              "Sprite"
            ],
            [
              "SeamCrawler",
              "Zemin çatlaklarında kayar, pençe saldırısı, ambush",
              "✅ Anim hazır"
            ]
          ]
        }
      },
      {
        "source_id": "4d875caf-b606-46c7-8452-0ec324c56045",
        "citation_number": 31,
        "cited_text": "FAZ 4 — FULL DEMO Claude: Sadece bu dosyayı oku. Faz 1-3 tamamlanmış varsay. -------------------------------------------------------------------------------- SCOPE Hedef: 30-45 dakikalık tam run. Act 1-2 tam, cross-class Ultimate, Curse Gate, Event odası, Fracture Echoes, temel meta-progression. Steam Demo'ya hazır. Ne VAR (Faz 3'e ek): +2 class: Summoner, Hexer (toplam 10 class tamamlanır) Cross-class Ultimate (Act 2 boss sonrası açılır) Legendary skill tier Curse Gate odası (HP harca → büyük bonus) Event odası (ikili seçenekli hikaye kararı) Curse sistemi (5 basit efekt) Fracture Echoes (ilk 2 boss: Penitent Sovereign + Echo Twin) Temel meta-progression (Echoes harcanabilir: class unlock, hub) Hub NPC etkileşimi (Ferryman, Vrel, Sister Mourne) +2 mob: Spore Hollow, The Wound (tam implementasyon) Fracture Sovereign boss (Act 3 başlangıcı — sadece F1) Codex + item description lore"
      },
      {
        "source_id": "dd900e61-2c7b-4519-aa26-79526075ad8e",
        "citation_number": 32,
        "cited_text": "FAZ 5 — FULL GAME (Early Access) Updated: 2026-04-29 — S43 canonical class/skill list applied Claude: Sadece bu dosyayı oku. Faz 1-4 tamamlanmış varsay. -------------------------------------------------------------------------------- SCOPE Hedef: Act 1-2-3 + Final Boss tam, Fracture Echoes (tüm bosslar), Grudge sistemi, zorluk modları, tam meta-progression. Early Access'e hazır. Ne VAR (Faz 4'e ek): Act 3 tam (10-11 oda) Final Boss — The Architect (4 faz) Fracture Sovereign tam (3 faz) Fracture Echoes tüm bosslar (5 echo/boss × 4 boss = 20 echo toplam) 90 cross-class pasif + 90 cross-class ultimate (10×9 tam set) Grudge / Nemesis sistemi Zorluk modları (Echo / Rift / Fracture / Void) +3 mob: Rift Maw, Class Mimic, Remnant Host 2 ek Spirit: Void Seer, Fallen Champion Ancient Relic spirit (güçlü ama bedelli) Lokalizasyon altyapısı (TR/EN)"
      }
    ]
  }
}
