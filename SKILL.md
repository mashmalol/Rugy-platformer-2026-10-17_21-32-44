---
name: rugy-unity-2d-platformer
description: Build a fuzzy Persian-rug-themed 2.5D platformer in a Unity 3D Universal Render Pipeline project, one reusable script or visual asset at a time. Use when the user wants to start or expand a Rugy-style game from an empty Unity project, create soft rug physics, generate game concept art, or establish incremental Unity gameplay systems and assets.
---

# Rugy Unity 2.5D Platformer

Build the game as a side-scrolling 2.5D platformer inside a magical Persian rug. Assume the user is working in a Unity **3D Universal Render Pipeline** project unless they explicitly choose another setup.

## Core design language

Preserve these visual and gameplay anchors:

- Thick, fuzzy crimson wool carpet as the primary ground material.
- Persian-inspired geometric motifs in turquoise, saffron gold, cream, navy, emerald, and violet.
- Floating carpet fragments, tassels, loose golden threads, glowing medallions, and a deep indigo magical void.
- Cozy, handcrafted fantasy mood: soft, warm, tactile, whimsical, and slightly mysterious.
- Use a 2.5D coordinate model: X is horizontal movement, Y is jumping, Z is depth and remains constrained for the player.
- Prefer readable silhouettes and practical Unity implementation over purely cinematic decoration.

Suggested palette:

```text
Persian Red: #9E3040
Deep Burgundy: #531D35
Turquoise: #1FA6A0
Saffron Gold: #E6A83B
Night Navy: #172447
Emerald: #277C62
Wool Cream: #F1D9A6
Magic Violet: #8C64C7
```

## Interaction style

Work incrementally. When the user asks to start from zero or says “one by one”:

1. Deliver only the next useful script or asset unless they request a batch.
2. State what the item does.
3. Give the exact Unity path where it belongs.
4. Give concise Unity setup steps and inspector assignments.
5. Verify any created file exists before stopping and attach it using its absolute path.
6. Do not assume a project path from a Unity deep link; that link opens a local Unity Hub project and does not grant filesystem access to the project.
7. Keep a short progress sequence so future steps can continue consistently.

Ask one focused clarification only when a missing choice changes implementation materially, such as 2D versus 3D physics, CharacterController versus Rigidbody, or the actual project directory. Otherwise use the defaults below.

## Default technical architecture

For a Unity 3D URP project, use:

- `CharacterController` for the player unless the user requests physics-driven Rigidbody gameplay.
- Side-scrolling movement with X/Y motion and Z locked to a configurable plane.
- A child `VisualRoot` for model sway, squash, bob, and other fuzzy visual effects.
- A separate camera controller for smooth horizontal follow and optional look-ahead.
- Layered prefabs and ScriptableObjects only when they reduce repeated setup.
- Legacy Input Manager for the first prototype when compatibility is more important than a new input package. Mention how to switch to the Input System when needed.

For a true 2D Unity project, switch to `Rigidbody2D`, `Collider2D`, Tilemap, and 2D physics; do not mix the 2D and 3D controller implementations.

## Recommended build order

Follow this order for an empty 3D URP project:

1. **Project folders and scene:** create `Scenes`, `Scripts/Player`, `Scripts/World`, `Scripts/Enemies`, `Scripts/Camera`, `Prefabs`, `Materials`, `Models`, `Textures`, and `Audio`; save the first scene as `CrimsonCourtyard`.
2. **Player controller:** create a 3D `CharacterController` player with soft acceleration, jump buffering, coyote time, floating while holding Jump, air control, facing, and subtle visual sway.
3. **Temporary player visual:** use a capsule or simple woven placeholder under `VisualRoot` until a character asset exists.
4. **Rug ground prototype:** create a thick carpet platform with collision, rounded/fuzzy visual edges, and Persian pattern color blocks.
5. **Camera:** implement a side-scrolling camera that follows the player while keeping Z fixed.
6. **Collectible:** add Golden Thread pickup and a minimal counter.
7. **Checkpoint:** add a golden tassel checkpoint and respawn behavior.
8. **Enemy base:** implement one predictable Wool Knot patrol enemy before adding moths or Shadow Scissors.
9. **Level blockout:** lay out The Crimson Courtyard with safe tutorial space, small gaps, upper platforms, a checkpoint, a moving-platform section, and a fountain goal.
10. **Polish assets:** generate and integrate rug materials, fiber particles, tassels, lighting, medallion effects, sound, and UI.

Do not generate all systems at once if the user requested one-by-one progress.

## Player controller requirements

When creating the first 3D player controller, include:

- `CharacterController` requirement and null-safe setup.
- Horizontal acceleration and deceleration.
- Configurable movement speed.
- Gravity, stronger fall gravity, and jump height.
- Coyote time and jump buffering.
- Floating only while airborne and holding Jump; cap downward float speed.
- X/Y movement with Z locked through the movement vector.
- Optional facing direction and `VisualRoot` sway.
- Soft landing squash or bounce kept subtle and optional.
- Clear serialized fields with useful defaults.
- XML comments explaining the controller’s purpose.

Place it at:

```text
Assets/Scripts/Player/Rugy3DPlayerController.cs
```

Use a descriptive filename and class name that match exactly. Do not overwrite a previously created 2D controller unless the user explicitly asks to replace it.

## Concept-art and visual-asset workflow

When the user asks for a game image, concept art, or visual asset:

1. Read the image-generation skill before generating.
2. Define the asset’s job: gameplay visual target, key art, background, tile, prop, character, or UI mockup.
3. Use detailed English prompts even if the user writes in another language.
4. Include camera/view, platform readability, materials, palette, lighting, and what must not appear.
5. Generate one image per requested asset; do not silently combine unrelated deliverables into a contact sheet.
6. Save generated assets under a descriptive project path such as `Art/Concepts/` or `Art/Environment/`.
7. For gameplay concept art, prefer 16:9. For wide key art, prefer 21:9. Use transparent PNG only for isolated assets.
8. Deliver every requested final image exactly once with an absolute Markdown file link.

Default gameplay concept prompt ingredients:

```text
3D URP 2.5D side-scrolling gameplay view, thick fuzzy crimson wool rug, Persian geometric patterns, turquoise and saffron accents, readable raised platforms, dangling tassels, floating golden threads, deep indigo void, warm magical lighting, handcrafted stylized game art, no UI, no text, no logo.
```

## Level-design defaults: The Crimson Courtyard

Use a left-to-right route of roughly 160 world units for the first level:

- Waking Pattern: safe tutorial and first Wool Knot.
- Frayed Crossing: small gaps and a flying Thread Moth.
- First Courtyard: broad medallion area with upper routes.
- Golden Tassel: first checkpoint around the middle.
- Loom Bridge: slow moving wool platforms.
- Red Garden: mixed enemy and vertical-route section.
- Crimson Fountain: final goal and calm completion space.

Keep the first enemy slow, make the first gap forgiving, provide recovery platforms below harder jumps, and avoid trapping the player between enemies. Place optional Golden Threads above safe routes to reward exploration.

## Output format

For each incremental deliverable, use this compact structure:

```markdown
## Step N: [item name]

**Purpose:** [what it does]

**File:** [absolute Markdown link]

### Unity setup
1. [create or select object]
2. [add component]
3. [assign fields/layers]

### Test
[one short play-mode test]

### Next recommended step
[item]
```

Do not claim that a Unity deep link was edited or that a local Unity Editor was launched. Say clearly when the user must copy a file into their local project or open the link in Unity Hub.
