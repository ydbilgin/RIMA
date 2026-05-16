# PixelLab New Feature Analysis - Character States

## 1. Feature name + 2-3 sentence summary

Feature name: PixelLab Character States.

PixelLab added a state workflow inside the character creator: create alternate poses or variants of the same character first, then animate from that state instead of forcing every animation to begin from the default idle pose. The demonstrated value is stronger first frames for action-specific animations, cleaner transitions between poses, and faster directional animation assembly when combined with mirroring and small Pixelorama cleanup.

## 2. Workflow demonstrated

- Open PixelLab website and go to the Characters page.
- Generate or select a character in the newer V3 generation mode.
- Use a top-down camera setting for top-down game characters.
- In the demo, set a custom character size of 32 by 44 on a square canvas; this implies custom non-square sprite bounds are supported inside the character canvas.
- Click Create State on the character.
- Prompt the state with what the same character should be doing, such as fighting pose, mid-walk, eating from the floor, laying down, or a costume variant.
- Review the generated state; it preserves the base identity, proportions, and main design details while changing pose or costume.
- From a state, click Add your first animation.
- Choose Custom Animation V3.
- Enter the animation prompt, such as walk loop, fighting stance idle, or stand up.
- Optionally enhance the prompt.
- Define frame count.
- Keep the first frame enabled when the state is intended to be the animation starting frame.
- Generate the animation from that state.
- For transition-style motion, open advanced options and use interpolation: state as start frame, another frame/state as end frame, then generate in-between movement.
- Manually clean minor artifacts in Pixelorama instead of rerolling the whole animation when the body motion is usable.
- Generate selected directions and mirror symmetrical directions when the design allows it.

## 3. Inputs / outputs / formats

Inputs:

- Existing PixelLab character or newly generated character.
- State prompt describing pose, action, or variant.
- Optional built-in state examples such as fighting pose.
- Custom Animation V3 prompt.
- Frame count.
- Optional interpolation start and end frames.
- Direction selection/rotations for a complete directional set.

Outputs:

- Character state: a new pose or design variant of the same base character.
- Animation generated from that state, with the state acting as the first frame when first-frame retention is enabled.
- Directional animation outputs usable as sprite animation frames.
- Export/edit flow through Pixelorama for cleanup.

Formats found:

- Public video did not name export file formats. The visible workflow is PixelLab character/animation assets and Pixelorama-editable frames. Existing PixelLab positioning remains sprite-sheet/game-asset oriented, but exact export formats were not stated in this video.

## 4. Cost / credits per gen

Unknown from public sources checked. The video does not mention credits or cost for creating a state, and the public PixelLab pages checked only expose plan-level generation language, including a free trial with 40 fast generations and later daily slower generations. Treat every state and animation generation as consuming PixelLab generation budget until verified in the logged-in UI.

## 5. Pipeline fit - replace or augment existing RIMA pipeline?

Verdict: AUGMENT, not replace.

Character States should sit after base character creation and before animation generation. It does not replace Create Image Pro/reference-image work for designing the canonical 64x64 chibi class sprite, and it does not replace the existing need for strong 8-direction identity checks. It does, however, improve the animation starting-frame problem: attack, dash, hit, death, cast, crouch, or recovery can start from a pose state closer to the intended motion rather than from neutral idle.

Cross-check against RIMA flow:

- Create Image Pro / reference image: still needed for locked class identity and style. Character States helps derive pose variants after identity is accepted.
- 8-dir mirror: compatible. The demo explicitly says symmetrical designs can generate southeast/east/northeast, mirror west-facing directions, then add south/north for a full 8-direction walk set.
- Weapon-less production: strongly compatible. The demo warns side-specific details like a sword on one hip or one-sided shoulder pad reduce mirroring value; RIMA's weapon-less body sprites avoid that problem.
- Animation flow: compatible with Idle, Run, Attack, Dash, Hit, Death. Most useful for Attack/Dash/Hit/Death first-frame anchors and for Run if a mid-walk state can produce cleaner leg motion.
- Unity child weapon SpriteRenderer: unchanged. States should remain weapon-less; weapon timing still belongs in Unity child sprites or separate weapon animation layers.

## 6. Speed advantage

Known claim from the demo: a full 8-direction walking set can be built in under five minutes when the character is symmetrical enough to mirror directions and when only selected directions are generated directly. PixelLab's public site also markets asset creation as 10x faster, but that is a general site claim, not a measured Character States benchmark.

## 7. 64x64 chibi compatibility

Likely compatible, with testing required. The demo uses a custom 32 by 44 character size on a square canvas, which is smaller than RIMA's 64x64 native target and suggests the feature supports small sprite production. For RIMA, the risk is not canvas size but readability: chibi facial/hand details, attack silhouettes, and class-specific costume marks may drift during state generation and need Pixelorama or Unity-side cleanup.

## 8. Limitations

- Not every generation is final; the demo explicitly treats cleanup, rerolling, and per-direction judgment as part of the workflow.
- Some directions may be good while another needs cleanup or reroll.
- Symmetry matters. Side-specific costume or equipment details reduce the usefulness of mirroring.
- The workflow can introduce small artifacts, such as changing face/mouth details across frames.
- Animal and unusual pose states work, but may include unwanted contextual artifacts, such as extra grass in the cow example.
- A full polish pass across every direction is still manual.
- Public sources did not disclose per-state credit cost.
- Public Pixellab blog/changelog pages did not expose a dedicated Character States announcement via curl; the official YouTube video is the primary source found.
- Nitter mirrors checked were unavailable or returned no usable result; Reddit searches returned no recent matching posts.

## 9. Top 3 RIMA-actionable items

1. Integrate for animation anchors: create Character States for attack anticipation, dash lean, hit recoil, death start/fall, and run mid-stride before generating animation clips.
2. Pilot on one weapon-less class only: use one approved 64x64 class body, generate states for Run, Attack, Hit, Death, then compare against the current handcrafted prompt flow before changing production policy.
3. Defer full pipeline replacement: keep Create Image Pro/reference-image base production and existing 8-direction QC. Use Character States as a controlled augmentation for pose starts, variant testing, and faster mirrored directional animation.

## Source checks run

- YouTube oEmbed metadata for https://youtu.be/oCJWxfEwX-o.
- yt-dlp info JSON and English transcript for the video.
- pixellab.ai public pages via curl/Jina text extraction for animation, character, pricing, and generation language.
- Discord invite page via curl; invite is public but announcements are not accessible without Discord session/API access.
- Nitter mirror searches; no usable result due timeout/unavailable host/no matching public output.
- Reddit r/PixelLab, r/gamedev, and global Reddit JSON search; no recent matching posts found for "PixelLab Character States".
