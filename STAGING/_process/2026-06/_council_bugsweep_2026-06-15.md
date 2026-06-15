# COUNCIL BUG-SWEEP: demo-kritik yüzey — olası bug/sorun listesi (her advisor AYRI)

ACTIVE RULES: (1) think (2) min/no-speculation (3) evidence file:line (4) BLOCKED if unclear.
GRAPHIFY: cross-file/mimari soruda önce graphify query (graph.json: STAGING/_process/2026-06/graphify_fullmap/graphify-out/graph.json, 6925 node AST), bulk-read'den ~71× ucuz. Sorgu: python ile node-link yükle, ilgili node + incoming/outgoing 'calls' edge'leri tara.

## AMAÇ
Sunum ~20 Haz. Kullanıcı tüm council'in demo-kritik yüzeyde olası bug/sorunları AYRI AYRI listelemesini istiyor. Orchestrator (Opus) sentezleyecek. Bağlam: F2 reward→kart GREEN, Build Mode centerpiece bug-free, stat→damage math kanıtlı, F1 safe. YENİ bulgu: Director Mode dev-direct'te Test state'inde overlay açık kalıyordu (FIX uygulandı — overlay Test'te gizleniyor).

## SCOPE (sadece demo-kritik yüzey — full-codebase DEĞİL, o post-demo)
1. **Golden-path:** RewardPickup.cs · DraftManager.cs · RoomRunDirector.cs (reward→draft→door)
2. **Build Mode:** BuildModeController.cs · BuildMode/BuildPlacementController.cs
3. **Director Mode:** DirectorMode.cs (trigger/state-machine/overlay/spawn/stat/telemetry) — ÖZELLİKLE state-desync, lifecycle, coupling
4. **stat→damage:** DamageCalculator.cs · DirectorMode stat-slider yolu
5. **Coupling:** Director ↔ BuildMode (EnterBuildMode→DirectorMode.SetState), auto-bootstrap pattern'leri

## İSTENEN ÇIKTI (KESİN FORMAT)
Numaralı liste, her madde:
`[SEVERITY] dosya:line — sorun (1 cümle) — neden demo'yu etkiler/etkilemez`
SEVERITY = DEMO-BLOCKING (golden-path/centerpiece'i bozar) / POLISH (görünür ama akışı bozmaz) / POST-DEMO (önemsiz/bilinen limitasyon).
- Spekülasyon değil — her madde file:line kanıtı. Emin değilsen "SUSPECTED" işaretle.
- Zaten bilinen/çözülmüş olanları TEKRARLAMA: F2 (GREEN), Build Mode (bug-free), F1 (safe), Director overlay bleed (FIX'lendi).
- DEMO-BLOCKING'leri ÖNE al. ≤15 madde (en önemliler). Over-engineering bulguları (mikro-optimizasyon) DEME.
