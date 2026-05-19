## Path A (Pure Painted) - Gercek Calisir mi?

a) Visual: Evet, tek oda veya sinematik showcase icin v4 hedefine en hizli yaklasan yol pure painted. Whole-room kompozisyon, isik, kir, catlak, rituel odak, duvar silueti ve atmosfer tek image icinde daha tutarli olur. Ama bu bir production mimarisi degil; daha cok art-direction proof aracidir.

b) Gameplay: Tam uyumlu degil. RIMA live hedefi 3-5 sub-room sequence, 20-30 template, procedural variation ve 32x22 fake-iso dungeon. Pure painted odada pathing, collision, door socket, enemy spawn, prop readability ve variation ya ustune invisible data olarak bind edilir ya da her varyant yeniden boyanir. Bu calisir, ama roguelite tekrar oynanabilirligi icin rijit kalir.

c) Iteration cost: Riskli. Full image re-gen ile ufak tweak bile composition/style drift yaratir: kapinin yeri, collider okunurlugu, enemy silhouette boslugu, zemin kontrasti her defasinda degisebilir. RIMA gibi combat readability isteyen oyunda bu pratik degil.

d) Production cost: 30+ sub-room icin scalable degil. 1-3 hero oda yapilir; 30 oda + varyant + act farki + gameplay socket revizyonu pure painted ile pahali ve kirilgan olur.

## Path C (Hybrid) - RIMA'ya Uyar mi?

Evet, RIMA icin asil production yolu Path C olmali: buyuk painted floor/material texture + modular sprite wall/prop/door overlay + data-driven sockets/colliders. Hades/Pyre tarafi bunu dogrular: Supergiant hissi tek parca flat tilemapten degil, guclu painted art direction ile ayrilmis runtime/gameplay katmanlarinin birlikte calismasindan gelir. Hades karakter tarafinda 2D concept -> 3D/sculpt/texture -> in-game model/sprite benzeri pipeline kullandi; yani hedef kalite, tekrar uretilebilir pipeline ile gelir. Dead Cells de kaliteyi korumak icin elde her retake cizmek yerine 3D-to-2D/pixel workflow kurdu. Ortak ders: iyi indie roguelite, guzel tek image degil, hizli retake alabilen guzel sistemdir.

## Real-World Comparison

Octopath: RIMA'ya dogrudan model degil. HD-2D, 2D karakterleri neredeyse tamamen 3D background ve modern lighting/post-process ile kaynastiriyor. RIMA fake-iso 2D sprite pipeline ve Unity 2D oda sekanslari icin fazla 3D/engine-heavy bir referans.

Hades: En yakin kalite referansi. RIMA icin ders: painted look korunur, ama gameplay katmanlari modular ve tekrar kullanilabilir kalir. RIMA Hades'i full kopyalamamali; Hades seviyesinde production disiplini hedeflemeli.

Dead Cells: En yakin workflow dersi. Visual stil farkli ama mantik ayni: retake maliyetini dusuren pipeline combat feel'i kurtarir. RIMA icin pure painted yerine hybrid asset system bunu saglar.

Recommended for RIMA: Hades hedef estetik + Dead Cells workflow mantigi + RIMA'nin mevcut 2D/fake-iso constraintleri. Yani Path C.

## Test Demo Strategy

1 saatlik 1 oda painted test mantikli ve risk-managed. Zaman israfi degil, cunku sadece su soruyu cevaplar: v4 painted hedefi Unity icinde player, camera, enemy, hit feedback ve readability ile hala guzel mi? Ama test sonucunu production mimarisi sanmamak gerekir. Demo Path A ile feel proof; production default Path C.

## TEK NET VERDICT

Test demo ile karar ver: 1 saatlik pure painted oda ile duyguyu ve readability'yi kanitla, fakat demo gecerse bile 30+ template production icin Hybrid sec. Pure painted yolu sadece hero/reference oda icin kullan; asil oyun Hybrid olursa hem guzel kalir hem roguelite olarak calisir.

## Hybrid Implementation (eger Hybrid secilirse)

- Tile size: 512x512 painted floor chunks for room-scale readability; 256x256 only for tight decals/transition patches. Unity PPU must match current 32x22 room scale before import lock.
- Variant count: Start 4 materials x 4 variants = 16 base floor chunks: stone, cracked stone, dirt/rubble, ritual/accent. Add 8 transition/edge decals after first playable proof, not before.
- Tilemap vs SpriteRenderer: Use Tilemap for repeated floor chunks and collision-free material layout. Use SpriteRenderer overlays for large hero floor stains, cracks, ritual circles, wall facades, doors, pillars, props. Never bake gameplay colliders into painted floor texture.
- Image_gen prompt template: "RIMA Act 1 shattered keep floor texture, fake-isometric 35 degree readable top surface, hand-painted dark fantasy roguelite, worn stone slabs with subtle dirt and cracks, no walls, no props, no characters, no text, no perspective camera, seamless-ish edges, gameplay readable center, muted contrast, transparent/clean boundary if possible, 512x512." For variants, lock material and palette, change only crack density, moss/dirt amount, ritual tint, edge wear.
- Sprite overlay rules: Walls/arches/doors define room boundary and z-depth; floor stays lower contrast than actors; props cannot hide enemy silhouettes; doors must align to data sockets; collision objects are separate authored shapes; decorative attachments stay on walls, not baked into floor; keep player/enemies outside any visual squash parent.

Sources consulted: Unreal Engine Octopath HD-2D interviews; GameDeveloper Hades art pipeline note; GameDeveloper Dead Cells 3D-to-2D workflow article. NLM query attempted but blocked by expired authentication.
