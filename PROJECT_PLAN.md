# SchoolYardArea MVP Plan

Working codename: SchoolYardArea.

Planned GitHub repository name: android-multi-device-app.

Tech selections:

- Engine: Unity 2D.
- Target platform: Android.
- First testing path: Firebase App Distribution.
- Later testing path: Google Play Internal Testing.
- First multiplayer strategy: offline MVP first, then local/private multiplayer prototype.

## Product Direction

SchoolYardArea is a simple top-down Android arena game inspired by kid-friendly arcade brawlers. Players pick a character, enter a small schoolyard-themed arena, battle bots, earn wins, and unlock more characters.

The first MVP should be fun offline before we add multiplayer. Multiplayer is important, but it will be much easier to build well after movement, combat, powers, match flow, and unlocks feel good.

## Engine Recommendation

Recommended engine: Unity 2D.

Why Unity:

- Strong Android build pipeline for APK/AAB testing.
- Good 2D physics, animation, particles, input, and UI tools.
- Fast iteration for arcade combat feel.
- Better path to future multiplayer than native Android UI frameworks.
- Easy to integrate with Firebase App Distribution or Google Play testing later.

Alternatives considered:

- Godot: good open-source 2D engine, lighter than Unity, but multiplayer and Android testing workflows may require more custom setup.
- Native Android/Kotlin: good for apps, but an action arena game would require us to build more game-loop, collision, animation, and input systems ourselves.
- LibGDX: capable and Android-friendly, but less visual tooling than Unity.

Decision: use Unity 2D unless there is a strong reason to avoid it.

## MVP Scope

The MVP should include:

- Android build target.
- Landscape orientation.
- Schoolyard arcade arena.
- One local player.
- On-screen movement joystick.
- On-screen attack button.
- On-screen special power button.
- Bot opponents.
- Health bars.
- Match timer.
- Win/loss result screen.
- Persistent win count.
- Character unlocks based on wins.
- Character select screen.
- 12-character roster defined in data.
- At least 3 playable/tuned characters for the first build.

Nice-to-have for MVP, only if time allows:

- Basic music and sound effects.
- Simple particle effects for attacks.
- Basic character portraits.
- Difficulty selection.
- Local same-device demo mode.

Out of MVP:

- Real-money purchases.
- Public matchmaking.
- Accounts/login.
- Chat.
- Loot boxes.
- School-identifying details.

## Testing Path

Recommended first testing path: Firebase App Distribution.

Why:

- Easier than Play Store setup for early testers.
- Lets us invite specific testers by email.
- Good fit for private family/kids phone testing.
- Supports APK/AAB distribution.

Later testing path:

- Google Play Internal Testing once the app is stable enough for a Play Console workflow.
- Closed testing before production release if required by Google Play account rules.

## Game Theme

Theme: schoolyard arcade.

Visual tone:

- Bright, playful, readable.
- Schoolyard-inspired arenas without referencing a real school.
- Chalk lines, cones, hopscotch patterns, playground markings, backpacks, dodgeballs, lunch tables, and gym mats are good environmental motifs.
- Avoid real school names, logos, uniforms, or identifying details.

## Core Loop

1. Player chooses an unlocked character.
2. Player enters a short arena match.
3. Player defeats bots or survives the objective.
4. Player earns a win.
5. Win total unlocks new characters.
6. Player tries the new character and repeats.

## First Game Mode

Recommended first mode: solo battle versus bots.

Rules:

- 1 player versus 3 bots.
- 2-minute match timer.
- Player wins by being the last one standing or having the most knockouts when time expires.
- Player loses if eliminated and no respawn remains.

We can add team battles, gem/token grab, and online multiplayer after the core arena works.

## Controls

Recommended controls:

- Left virtual joystick for movement.
- Right attack button for primary attack.
- Right special button for special power.
- Optional smaller dash button later.

Design notes:

- Buttons should be large enough for kids.
- Attack direction can initially follow movement direction.
- Later, we can add aim-drag for more advanced characters.

## Character Roster

Unlock pacing should be simple and predictable. No loot boxes.

| Character | Role | Primary Attack | Special Power | Unlock |
| --- | --- | --- | --- | --- |
| Helena | Healer | Spark burst | Healing circle | 0 wins |
| Vivien | Trickster | Quick stars | Short invisibility | 2 wins |
| Owen | Bruiser | Heavy toss | Ground slam | 4 wins |
| JP | Speedster | Fast taps | Dash strike | 6 wins |
| Steve | Tank | Slow heavy hit | Shield bubble | 8 wins |
| Beth | Controller | Chalk splash | Slow zone | 10 wins |
| Zoe | Trapper | Pop shot | Sticky trap | 12 wins |
| Sophia | Support | Ribbon shot | Team/self boost | 14 wins |
| Zane | Blaster | Bounce ball | Big burst | 16 wins |
| Dylan | Sniper | Long throw | Charged shot | 18 wins |
| Alex | Balanced | Straight shot | Power combo | 20 wins |
| Tomasso | Wildcard | Curve shot | Arena whirlwind | 25 wins |

First fully playable characters:

- Helena: forgiving starter with small self-heal.
- JP: fast movement, simple dash special.
- Steve: tanky character with a shield special.

## Privacy And Safety

Use first names only.

Avoid:

- Photos of real kids.
- Voice recordings.
- Real school names.
- Class names.
- Addresses or locations.
- Any text implying the characters represent real children exactly.

Treat each character as a fictional arcade character inspired by a first name.

## Multiplayer Roadmap

Phase 1: Offline MVP

- Player versus bots.
- Local unlock progression.
- Android test builds.

Phase 2: Local Multiplayer Prototype

- Same Wi-Fi private room or local network test.
- 1v1 or 2v2.
- No accounts.

Phase 3: Online Multiplayer

- Private room codes.
- Friend/family testing.
- Evaluate Unity Netcode, Photon, or a simple authoritative WebSocket server.

Phase 4: Store-Ready Multiplayer

- Account/privacy review.
- Abuse prevention.
- Matchmaking only if needed.
- Parent-friendly settings.

## Milestones

### Milestone 1: Prototype

- Unity project created.
- Android build succeeds.
- Player can move in an arena.
- One attack works.
- One bot can chase and attack.

### Milestone 2: MVP Match

- 3 bots.
- Health/damage/death.
- Win/loss screen.
- Timer and score.
- Basic sound effects.

### Milestone 3: Characters And Unlocks

- 12 roster entries.
- 3 tuned playable characters.
- Win count saved locally.
- Unlock flow implemented.

### Milestone 4: Phone Testing

- Firebase project configured.
- Android build uploaded.
- Testers invited.
- Feedback list started.

### Milestone 5: Polish Pass

- Better sprites.
- Particles.
- Improved arena art.
- Juicier hit effects.
- Balance pass.

## Immediate Next Steps

1. Confirm Unity 2D as the engine.
2. Install Unity Hub and a current Unity LTS editor with Android Build Support.
3. Create a new Unity project in this folder or a sibling folder.
4. Commit the initial project to a separate GitHub repo.
5. Build Milestone 1.
