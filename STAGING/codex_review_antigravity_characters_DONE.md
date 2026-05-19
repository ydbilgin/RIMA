# Codex Review - Antigravity Character Folder

Scope: 32 named sprites in `STAGING/antigravity/Tum_Karakterler_Isimlendirilmis/`, checked against the canonical roster, Karar #100/#145/#155/#156, and the Opus assessment. I inspected the actual sprites via a generated contact sheet and file metadata. All 32 files are 64x64 RGBA, so scale compatibility is intact; the main risks are identity, tone, and classification.

## Q1 - Opus Assessment Verdict

Verdict: mostly AGREE.

1. Alt_Shadowblade_Mor_Kapsonlu misclassified: AGREE. The sprite reads as teal-haired, lavender/purple hooded caster, not a narrow dark assassin. Shadowblade canonical needs olive skin, near-black purple armor, and void purple accent glow. This asset has a cold mage silhouette and palette, so Frost Elementalist is a better use.

2. Alt_Ranger_Yasli_Avci misclassified: AGREE. It is an older bearded male in heavy dark leather/armor. It has no female Ranger identity, no bleached ivory hair, no forest green asymmetric armor, and no cold blue Ranger accent. Warblade Veteran/Aged is the strongest reclassification.

3. Kullanilmayan_Modern_Gri_Kapsonlu reject: AGREE. The grey hoodie and modern casual silhouette break Karar #79. Even if the palette is muted, the object language is contemporary streetwear, not fractured epic or ritual catastrophe.

4. NPC_Sari_SivriSacli_Genc tone risk: AGREE/PARTIAL. It does read too anime-young and spiky for a core hub NPC. I would not hard reject forever, but it should not be in Karar #156 v1. If kept, it needs a narrow future role like unstable apprentice, comic relief risk, or a deliberately irritating junior, not a default hub pillar.

5. Three NPC solid: AGREE. Demirci, GuildMaster, and YetenekHocasi are visually distinct, readable, and useful. They cover vendor/crafting, mentor/guild authority, and progression/training without overlapping too much.

6. Gunslinger_Ana2 canonical, Gunslinger_Ana archive: AGREE. The task states the user fix is dark short hair, not red ponytail. Ana2 has the darker sleek look and better matches the fixed lock; the red ponytail version should become old canonical archive or optional skin/NPC, not anchor.

7. Alt_Brawler_Kesis_Kel as Brawler Monk: AGREE. Bald, shirtless/robe-like, simple grounded silhouette. It preserves Brawler body language while changing class fantasy into monk/trainer direction.

8. Alt_Elementalist_Doga as Druid Elementalist: AGREE. The barefoot nature-coded look is a credible Elementalist skin. It is not the canonical honey-blonde low bun identity, but as a skin it expands the class cleanly.

9. Yedek duplicate archive: PARTIAL. They are likely image16_split duplicates. Archive is correct, but do not delete until the source sheet and final selected anchors are logged.

## Q2 - Other Use Case Findings

Verdict: Opus caught the major uses, with a few additions.

The strongest missed use is `Alternatif_Gunslinger_Kirmizi_Sackuyrugu.png`. It should not be canonical Gunslinger, but it is strong enough for a "Redline Gunslinger" skin or a Hub NPC role: retired duelist, pistol trainer, or rift courier.

`NPC_GorevVeren_KisaSac_Paltolu.png` is not just optional quest-giver. It could be the hub "contract broker" or expedition dispatcher, which is more useful than a generic quest giver in a roguelite.

`NPC_Sari_SivriSacli_Genc.png` is not v1-ready, but it may serve as a "reckless apprentice" in later narrative beats if the tone is sharpened. It needs less cheerful/anime presentation and more exhausted, ritual-burned styling.

Among yedek sprites, duplicate-looking Hexer, Brawler, Shadowblade, Ranger, Summoner, red Gunslinger, Warblade, Elementalist, Ronin, and Ravager forms are visible. No hidden superior canonical anchor appears there.

Additional misclassification risk: `08_Elementalist_Ana.png` hair reads more orange/auburn than honey-blonde in the enlarged sheet. This is not fatal, but it should be corrected or locked with a prompt before class identity hardens. `07_Ronin_Ana.png` has a topknot-like shape, but it is small and could be clearer.

## Q3 - State Workflow Fix List

Verdict: PASS with MODIFY on wording.

The six Opus fixes are directionally correct, but they need identity-preserving wording.

Shadowblade prompt: MODIFY. Use: "Keep the same male assassin identity, silhouette, face, and outfit. Add clear void-purple glow only on shoulder edges, belt seam, and dagger-side accents. Keep armor near-black purple, not blue, not teal."

Ronin prompt: PASS/MODIFY. Use: "Keep the same Asian male ronin and dark navy kimono/hakama. Tie the black hair into a visible samurai topknot. Do not add western armor or bright colors."

Elementalist prompt: MODIFY. Use: "Keep the same female Elementalist outfit: dusty indigo crop, cream sash, deep teal skirt. Change hair to honey-blonde low bun. Not red, not auburn."

Hexer prompt: PASS/MODIFY. Use: "Keep the same pale female hooded Hexer. Add subtle dark red hex-rune accents on collar, cuffs, and hem. Keep robe dark purple-black."

Summoner prompt: PASS/MODIFY. Use: "Keep the same female Summoner with long dark hair and indigo green-black robe. Raise one hand in a summoning gesture with faint cyan fingertip glow."

Warblade aging prompt: PASS/MODIFY. Use: "Keep the same male Warblade armor, beard shape, and broad stance. Make the face a late-40s weathered veteran with light wrinkles and grey streaks. Do not make him elderly or frail."

## Q4 - Skin Pilot Batch 1

Verdict: PARTIAL AGREE.

Opus proposal Frost Elementalist + Druid Elementalist + Warblade Veteran is coherent because all three candidates already exist and demonstrate Karar #155 value. It covers elemental reskin, nature/diversity skin, and age/veteran skin.

However, the batch has one product risk: two of the three skins are Elementalist-related. That is fine if the sprint goal is proving PixelLab skin workflow mechanics, but less ideal if the goal is proving class-wide skin coverage. My preferred batch depends on the validation question.

If the goal is "ship existing viable skins fast," keep Opus batch: Frost Elementalist, Druid Elementalist, Warblade Veteran.

If the goal is "prove breadth across classes," use Warblade Veteran, Brawler Monk, and Druid Elementalist. Park Frost Elementalist for batch 2 after the misclassification rename is documented.

My recommendation: Sprint 17 batch 1 should be Warblade Veteran + Brawler Monk + Druid Elementalist. Frost Elementalist remains high value, but two Elementalist variants in the first three can make the pilot look narrower than it is.

## Q5 - Critical Illogical Issues Missed

Verdict: Opus covered the major issues; Codex adds minor drift risks.

The biggest missed issue is canonical `08_Elementalist_Ana.png`: the hair appears warm orange/auburn, not clearly honey-blonde low bun. This deserves a state fix before lock.

Second, `10_Summoner_Ana.png` reads strongly as a dark teal/green robe caster, but the summoning gesture is passive. Without a hand cue, Summoner and Frost/Dark caster variants can blur. The Opus fix is correct and should be treated as required, not optional.

Third, `05_Shadowblade_Ana.png` has a good assassin silhouette, but the purple accents can read as ordinary suit pieces rather than void glow.

Fourth, `NPC_Demirci_Kahverengi_Yelek.png` is readable as vendor/blacksmith, but lacks a tool/anvil cue. It can ship for v1 if UI label supports it.

No proportion drift is severe. The chibi 3-4 head style is consistent across the set. Pose diversity is acceptable, though many anchors are front-facing neutral; production animation states will carry more identity than the static base sprites.

## Q6 - 5M/5F Gender Lock

Verdict: PASS with two watchpoints.

The canonical 10 anchors preserve the intended 5M/5F split if `06_Gunslinger_Ana2.png` replaces `06_Gunslinger_Ana.png` as canonical. Male anchors are Warblade, Brawler, Ravager, Shadowblade, and Ronin. Female anchors are Ranger, Gunslinger_Ana2, Elementalist, Hexer, and Summoner.

No anchor obviously violates the lock. The only ambiguity is not gender distribution but identity clarity. Shadowblade is male-coded enough in face/body shape, though narrow chibi styling can make it less explicit. Gunslinger_Ana2 reads female enough, but the short dark hair means documentation should explicitly lock it as female brown-skinned dark-short-hair Gunslinger to avoid future drift back to red ponytail.

The red ponytail `06_Gunslinger_Ana.png` should not be counted as the active canonical anchor, because that would preserve gender but violate the user-fix hair identity.

## Q7 - Karar #155/#156 Implications

Verdict: CONFIRMS both, with small adjustments.

For Karar #155, this folder strongly confirms the Class Skin Variant System. The alternatives are not random rejects; at least four have useful skin or NPC futures: Brawler Monk, Druid Elementalist, Warblade Veteran, and Redline/mentor Gunslinger. Frost Elementalist is also useful, but it proves the classification workflow must include human review because the filename can be wrong while the asset is still valuable.

Adjustment to Karar #155: the pilot should specify whether it optimizes for "fast viable skins" or "class spread." If fast viability, use Opus batch. If system proof, use three different classes.

For Karar #156, this folder validates a three-NPC v1 hub: GuildMaster/Mentor, Demirci/Vendor-Crafter, and YetenekHocasi/Skill Trainer. Quest-giver should wait until run contracts or quest UI exists. Sari spiky young NPC should be excluded from v1.

Final Codex verdict: Opus assessment is substantially correct. Required changes before production lock are: archive old Gunslinger as non-canonical, reclassify Alt Ranger as Warblade Veteran, reclassify Alt Shadowblade as Frost Elementalist, reject/archive modern hoodie, exclude Sari NPC from v1, and run state fixes on Shadowblade, Ronin, Elementalist, Hexer, Summoner, and optionally Warblade.
