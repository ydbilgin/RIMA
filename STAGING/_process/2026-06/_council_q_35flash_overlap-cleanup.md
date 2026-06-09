# RIMA Council — LEAN / SHIP-FAST LENS (Gemini 3.5 Flash High)

RIMA = 2D Unity ARPG, DEMO yetiştiriyoruz. Sen EN YALIN YOL + over-engineering eleştirisi lensisin. Demo'yu bozmadan en az dokunuşla overlap riskini kapat. Kanıt GREP-DOĞRULANMIŞ — file:line uydurma, verilene dayan. Amacın: "en az iş, demo'yu ısırmayanı ertele" demek.

## Gerçekler (grep)
- Canlı demo sahnesi _Arena = SADECE RoomRunDirector + IsoRoomBuilder. Legacy hiçbir şey canlı sahnede yok.
- Softlock zaten fix'li (76/76 test). Demo akışı (Combat,Combat,Merchant,Combat,Boss) zaten kodlu.

### 3 hedef
1. İki DungeonGraph aynı isim (canlı Runtime.DungeonGraph vs legacy Core/DungeonGraph MonoBehaviour). Legacy'ye 5 legacy dosya + 3 test bağlı. Risk: CS0104 + Inspector kafa karışıklığı. AMA hiçbiri _Arena'da değil.
2. MainMenuScreen self-create ama guard'ı canlı sahneleri (MainMenu/_Arena/CharacterSelect) ZATEN koruyor; ghost sadece guard-dışı test sahnesinde. OnPlayClicked chamber-bypass ikinci yol açıyor. 4 test ona bağlı.
3. DoorTrigger/GateBehavior legacy ama RoomClearVictoryTrigger (RewardPickup'tan static çağrılıyor) onları kullanıyor → silmek derlemeyi kırar.

## Sorular
1. Her hedef için EN YALIN güvenli aksiyon: (A) [Obsolete]+gerekirse rename, fiziksel sil ertele / (B) hepsini şimdi sil / (C) dokunma. Tek satır.
2. Hangi "temizlik" demo'yu HİÇ ısırmaz → ERTELE/SKIP demeli? Hangi tek şey gerçekten ŞİMDİ değer? (over-engineering uyarısı)
3. Demo için minimum: rename+[Obsolete] yeter mi, yoksa silmeye hiç girmeyelim mi?
4. RewardPickup→RoomClearVictoryTrigger→DoorTrigger zinciri: en ucuz "kırmadan" yol?
5. En küçük commit seti + doğrulama.

ÇIKTI: Her hedef tek-satır karar + "şimdi değmez, ertele" listesi + minimum commit. Acımasız yalınlık.
