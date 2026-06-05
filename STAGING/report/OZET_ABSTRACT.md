# Özet / Abstract

---

## Özet

Bu çalışma, tek geliştirici tarafından yapay zekâ destekli çok-ajanlı bir geliştirme süreci ve veri-güdümlü oyun mimarisi kullanılarak hayata geçirilen 2D izometrik bir aksiyon-roguelite oyunu olan RIMA'yı sunmaktadır. Projenin temel sorusu şudur: tek kişilik bir ekip, kapsamlı bir roguelite oyununu nasıl üretebilir?

Bunu mümkün kılmak için iki paralel yaklaşım izlenmiştir. İlki, içerik ve sistemin birbirinden ayrıldığı veri-güdümlü bir mimaridir; oyun odaları ScriptableObject veri dosyaları olarak tanımlanmış, zemin-uçurum-prop yerleşimi çalışma zamanında ve otomatik olarak inşa edilmiş, JSON tabanlı içe aktarma araçlarıyla dış kaynaklı oda tasarımları sisteme entegre edilmiştir. İkincisi, kod üretimi, tasarım danışmanlığı ve çıktı incelemesini farklı rollere dağıtan çok-ajanlı bir yazılım mühendisliği sürecidir. Bu süreçte yazar-reviewer ayrımı, karar dökümanı zorunluluğu ve doğrulama kanıtı gerekliliği temel ilkeler olarak uygulanmıştır.

Proje; on oynanabilir sınıf, 26 oda şablonu, yaklaşık 490 otomatik test ve MainMenu'den boss karşılaşmasına uzanan oynanabilir tam döngüyle sonuçlanmıştır. Test altyapısı EditMode ve PlayMode testlerine ek olarak görsel oda kalite güvencesi sürecini kapsamakta; bu süreç gerçek hataları sistematik biçimde tespit etmiş ve düzeltmiştir.

**Anahtar kelimeler:** roguelite, prosedürel içerik, veri-güdümlü tasarım, çok-ajanlı yapay zekâ, oyun geliştirme

---

## Abstract

This work presents RIMA, a 2D isometric action-roguelite game developed by a single developer using an AI-assisted multi-agent development process and a data-driven game architecture. The central question guiding the project is: how can a solo developer produce a roguelite game of meaningful scope and quality?

Two parallel approaches were adopted to address this challenge. The first is a data-driven architecture in which content and systems are decoupled. Game rooms are defined as ScriptableObject data files; floor, cliff, and prop placement are constructed at runtime through automated systems; and externally authored room designs are integrated via a JSON import pipeline. The second is a multi-agent software engineering process in which code generation, design consultation, and output review are distributed across specialized agents. Core principles include author-reviewer separation, mandatory decision documentation, and verification evidence requirements for every completed task.

The project delivers a playable end-to-end loop spanning MainMenu, a walkable diegetic character selection space (Attunement Chamber), room-by-room combat with 3-card skill drafts, branching door choices, and a boss encounter, supported by ten playable character classes, 26 room templates, and approximately 490 automated tests. The quality assurance infrastructure extends beyond unit testing to include a programmatic visual room QC process, which identified and resolved systematic errors in prop placement across multiple room templates.

**Keywords:** roguelite, procedural content generation, data-driven design, multi-agent artificial intelligence, game development
