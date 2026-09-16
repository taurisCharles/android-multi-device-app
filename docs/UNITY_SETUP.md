# Unity Setup

## Recommended Editor

Use Unity 2D with Android Build Support.

Recommended install through Unity Hub:

- Unity 2022 LTS or Unity 6 LTS
- Android Build Support
- Android SDK and NDK Tools
- OpenJDK

## Opening The Project

1. Open Unity Hub.
2. Add the folder `/mnt/c/Dev/SchoolYardArena`.
3. Open the project with a supported LTS editor.
4. Set platform to Android.
5. Create the first scene at `Assets/Scenes/ArenaPrototype.unity`.

## First Scene Checklist

Create:

- Main Camera in orthographic mode.
- Arena background.
- Player prefab with:
  - `Rigidbody2D`
  - `Health`
  - `PlayerController2D`
  - `VirtualMoveInput`
- Bot prefab with:
  - `Rigidbody2D`
  - `Health`
  - `BotChaseController`
- Canvas for mobile controls.

## Android Build Target

Recommended first package id:

`com.tauris.schoolyardarena`

Recommended orientation:

Landscape.

## Multiplayer MVP Direction

Do not start with public matchmaking. Start with private co-op rooms where 2-3 players team up versus bots.

Recommended networking candidates to evaluate after the solo prototype:

- Unity Netcode for GameObjects
- Photon Fusion or Photon PUN
- Fish-Networking

Pick networking after the local game loop feels good.
