# Snakerito Graphics Tech Selection

Recommendation: keep Snakerito in Kotlin and Jetpack Compose Canvas for the next polish pass.

## Why

The current game is already a compact Compose app. A Snake.io-inspired refresh can be done by improving the Canvas rendering layer, adding themed vector assets, and animating between grid ticks. Moving to Unity or Godot would add migration cost without much benefit for this specific refresh.

## Direction

Theme: Mexican food arcade.

Visual upgrades:

- Snake head with sombrero and mustache.
- Brighter segmented snake body.
- Mexican food pickups: taco, burrito, salsa, chili, lime, nachos.
- Warmer tiled board or taco-stand tabletop arena.
- Arcade HUD with score, best score, and pickup feedback.

Technical upgrades:

- Keep game rules grid-based.
- Add visual interpolation between grid moves.
- Split Canvas drawing into focused renderer functions.
- Add pickup pulse, score pop, simple particles, and death animation.
- Use Android vector drawables for most themed assets.

Likely files:

- `app/src/main/java/com/example/snakegame/ui/GameScreen.kt`
- `app/src/main/java/com/example/snakegame/viewmodel/GameViewModel.kt`
- `app/src/main/java/com/example/snakegame/model/GameState.kt`
- `app/src/main/res/drawable/`
- `app/src/main/res/values/strings.xml`

## MVP Polish Pass

1. Add sombrero and mustache snake head.
2. Add rotating food pickup types.
3. Add smooth movement interpolation.
4. Add pickup/death effects.
5. Refresh launcher icon to match the new visual identity.
