# ax Gemini 3.5 Flash (High) — LEAN / minimal-proof lens

READ: Assets/Scripts/UI/DirectorMode.cs (telemetry+stat *ForValidation hooks) · Assets/Scripts/Core/RewardPickup.cs

## DURUM
Kullanıcı VIDEO KAYDI ALAMIYOR. O yüzden golden-path "film-proof" item'ları (stat→damage canlı, telemetry CSV, G-collect) runtime execute_code data-proof ile kanıtlanmalı. stat→damage MATH zaten unit-test ile doğrulandı (LMB lineer). Build Mode + F2 reward→kart zaten runtime kanıtlı.

## SENİN LENS'İN: minimal proof / over-engineering eleştirisi
**Q1:** stat→damage MATH (DamageCalculator) zaten unit-test ile kanıtlandıysa, CANLI integration testi (düşman spawn + gerçek LMB + hasar oku) gerçekten gerekli mi, yoksa over-engineering mi? Hangi item için canlı test ŞART, hangisi için unit/code-confirm YETER?
**Q2:** G-collect: math+render kanıtlı, ForceCollect zincir kanıtlı. Gerçek G-trigger testi marjinal değer mi, yoksa tek gerçek belirsizlik (kullanıcı G'ye basamayacağına göre demo nasıl olacak?) bu mu?
**Q3:** Video yoksa demo NASIL sunulacak (canlı mı, data-proof rapor mu, screenshot mu)? Bu, neyin kanıtlanması gerektiğini değiştirir mi?

Çıktı: ≤ çok kısa. Her item: "canlı test ŞART / unit yeter / skip" + tek cümle gerekçe.
