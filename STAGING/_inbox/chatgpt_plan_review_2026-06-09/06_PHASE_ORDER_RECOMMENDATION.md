# Phase Order Recommendation

Mevcut plan: A Combat → B İskelet → C Shop → D Boss → E Polish.

Benim önerim:

```txt
A0 — Demo hard lock
A1 — Softlock + lifecycle
B — Linear demo sequence + fixed camera + 2-class lock
D0 — Boss placeholder victory path
C — Shop
D1 — Real boss attacks
E — Polish
```

Boss placeholder shop'tan önce gelmeli, çünkü demo akışının sonu yoksa shop'u test etsen bile tam demo test edemezsin.

Uygulama sırası:
1. Softlock fix
2. DemoGraph / forced sequence
3. Fixed camera
4. 2-class lock
5. PauseMenu
6. Placeholder boss + victory/death
7. Shop 3 stand
8. Boss real telegraph attacks
9. Full playthrough tests
10. Visual polish
