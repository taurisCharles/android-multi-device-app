# Co-Op MVP Brief

## Goal

Let 2-3 kids play together on Android phones in a private co-op match versus bots.

## Recommended Build Path

1. Build solo movement and combat.
2. Add bots and wave survival.
3. Add local progression and unlocks.
4. Add private co-op rooms.
5. Test through Firebase App Distribution.

## Networking Recommendation

Start with private room codes, not public matchmaking.

Evaluate networking after the solo prototype feels good:

- Photon Fusion: strongest candidate for quick private-room multiplayer testing.
- Unity Netcode for GameObjects: good Unity-native option, but hosting/relay decisions matter.
- Fish-Networking: strong open-source option, more setup choices.

Recommended MVP choice when we reach multiplayer: Photon Fusion or Unity Netcode plus Unity Relay, depending on account/setup preference.

## First Co-Op Mode

Mode: Schoolyard Wave Survival.

Rules:

- 2-3 players join the same room.
- Players are on one team.
- Bots spawn in waves.
- The team wins by clearing all waves.
- The team loses when all players are knocked out.
- Bot count and health scale by player count.

## Controls

- Left on-screen joystick for movement.
- Right primary attack button.
- Right special power button.
- Optional revive/interact button in a later pass.

## Character Scope For First Playable

Implement and tune three characters first:

- Helena: starter healer, forgiving for younger players.
- JP: fast dash character.
- Steve: tank with a shield.

The other nine roster entries can exist in data before they are fully tuned.

## Store/Test Path

Use Firebase App Distribution for early phone testing. Move to Google Play Internal Testing once the game is stable enough to manage test releases through Play Console.
