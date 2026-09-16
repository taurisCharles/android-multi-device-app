# SchoolYardArena Art Direction

## Goal

Move SchoolYardArena from prototype shape art to a modern, kid-friendly 2D arcade look that still builds quickly in Unity and reads clearly on Android phones.

The target is not to clone Brawl Stars. The target is a bright schoolyard arena game with enough art, animation, VFX, and UI polish that it no longer feels like debug geometry.

## Visual Pillars

1. **Readable First**
   - Characters must be understandable at phone scale.
   - The player, enemy, pickups, hazards, and controls should be identifiable without labels.
   - Combat feedback should be visible even during movement.

2. **Toy-Like Arcade**
   - Chunky silhouettes.
   - Slightly exaggerated heads/backpacks/shoes.
   - Soft shadows and simple highlights.
   - Friendly proportions, no realistic violence.

3. **Schoolyard Specific**
   - Basketball paint, chalk marks, cones, benches, lunch tables, hopscotch, backpacks, apples, dodgeballs.
   - No real school names, logos, uniforms, or identifying details.

4. **Asset-Driven**
   - Stop relying on generated circles/squares for final visuals.
   - Generated shapes can stay only as fallback/debug.
   - Visual slice should use imported sprites, sprite sheets, or purpose-made 2D assets.

## Target Style

Recommended style: **2D/2.5D cartoon mobile arena**.

Characteristics:

- Top-down / slightly tilted 2D perspective.
- Painted schoolyard floor with texture, scuffs, chalk, and props.
- Characters as animated sprite sheets or layered 2D puppet sprites.
- Soft circular shadows under characters.
- Thick readable outlines.
- Bright but not neon-heavy palette.
- Simple effects: punch arc, shockwave, hit starburst, heal sparkle.

## Palette

Primary world colors:

- Asphalt/court tan: warm desaturated orange-brown.
- Chalk/paint lines: cream/yellow-white.
- Grass/edges: muted green.
- UI dark panels: charcoal/navy.

Character colors:

- Helena: teal/aqua shirt, yellow backpack accent.
- Owen: red shirt, dark backpack accent.
- Future roster: each character needs one dominant color plus one accent.

Avoid:

- Large flat brown fields.
- Overly dark blue/slate scenes.
- One-color UIs.
- Tiny low-contrast details.

## Asset Checklist For Visual Slice

### Arena

- One painted schoolyard map sprite or tilemap.
- Court lines integrated into the art.
- Edge props that frame the arena without blocking controls.
- Optional decorative props: bench, cone, hoop, lunch table, hopscotch, chalk doodles.

Acceptance:

- Looks intentional in a full-screen phone screenshot.
- Does not feel like a floating rectangle.
- Has enough texture to feel alive but stays readable behind characters.

### Characters

Required for the slice:

- Helena idle.
- Helena run/walk.
- Helena punch.
- Helena hit/flash.
- Owen idle.
- Owen run/chase.
- Owen punch/contact threat.
- Owen hit/knockout.

Minimum fallback:

- Four-direction or single-direction walk cycles are acceptable.
- Punch can be a one-shot frame plus VFX if full animation is not ready.

Acceptance:

- Helena and Owen can be recognized without reading labels.
- Movement has personality.
- Hit state is obvious.

### Combat VFX

Required:

- Punch arc.
- Special shockwave.
- Hit spark/starburst.
- Damage numbers.
- Heal numbers.
- Pickup sparkle.

Acceptance:

- User can tell exactly when Punch connects.
- Special feels different from Punch.
- Feedback does not hide the characters.

### UI

Required:

- Mobile-safe HUD.
- Punch button with icon.
- Special button with icon and cooldown ring/state.
- Cleaner health/nameplates.
- Round result message.

Acceptance:

- Buttons feel like game controls, not default debug GUI.
- Cooldowns are readable.
- UI avoids phone cutouts and navigation edges.

### Audio

Required later in the visual slice:

- Punch sound.
- Special sound.
- Hit sound.
- Apple/heal sound.
- Win/loss sting.

Acceptance:

- Sounds confirm player actions.
- Volumes are kid-friendly and not harsh.

## Production Strategy

### Phase 1: Replace The Arena

Start with the background because it sets the whole visual bar.

Deliverables:

- `Assets/Art/Arenas/schoolyard_arena.png`
- Unity scene uses the arena sprite instead of generated floor/line rectangles.
- Existing colliders can remain code-generated.

### Phase 2: Replace Helena And Owen

Deliverables:

- `Assets/Art/Characters/Helena/`
- `Assets/Art/Characters/Owen/`
- Sprite sheets or layered body-part sprites.
- Basic animation controller or scripted frame animation.

### Phase 3: Replace Combat Feedback

Deliverables:

- Punch arc sprite.
- Special ring/shockwave sprite.
- Hit spark sprites.
- Pickup sparkle sprite.

### Phase 4: Replace HUD

Deliverables:

- Icon buttons.
- Cooldown treatment.
- Cleaner nameplates/health bars.
- Result banner.

## Asset Sources

Allowed sources:

- Custom generated assets.
- Unity Asset Store packs with compatible licensing.
- Kenney/open licensed game assets.
- Original hand-edited sprites.

Avoid:

- Copying Brawl Stars assets or closely imitating their characters.
- Real photos of kids.
- Real school logos/names.
- Any asset with unclear commercial/test distribution rights.

## Definition Of Done For The Visual Slice

The visual slice is done when a phone screenshot shows:

- A polished schoolyard arena.
- Helena and Owen as actual characters.
- Apples/food as appealing pickups.
- Punch/special feedback visible in action.
- Modern-looking mobile controls.
- No obvious debug rectangles or shape placeholders in the main play area.

## Immediate Next Step

Create or source the first arena image and wire Unity to use it while preserving current gameplay:

1. Add a real arena sprite under `Assets/Art/Arenas`.
2. Update `PrototypeSceneBuilder` to use the arena sprite when available.
3. Keep generated art only as fallback.
4. Build and compare phone screenshot against the current generated-art version.
