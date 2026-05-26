# Dodge Fighter / Block Fighter

A small 2D Unity arcade game originally made as one of my first university course projects, then quickly polished later so it could be shown as part of my portfolio.

The project is intentionally simple: the player dodges falling blocks, collects coins, unlocks character skins, and tries to survive for as long as possible. The recent polish pass focused on making the game more presentable and easier to build, not on rebuilding it from scratch.

## Project Context

This is an early learning project. Some parts of the design and structure still reflect that, but the current version includes a cleanup pass over the main scripts, a basic skin shop, persistent coins, and a death summary panel.

I kept the scope modest because the goal was to preserve the original university project while improving the parts that matter for a portfolio demo.

## Features

- Endless arcade-style gameplay
- Tap/click movement
- Double-tap dash with cooldown UI
- Falling block hazards
- Coin collection during a run
- Persistent total coin balance using `PlayerPrefs`
- Character skin shop
- Buy/equip skins from the main menu
- Selected skin is used in gameplay
- Death panel with final score, earned coins, retry, and main menu buttons
- Music and SFX volume sliders

## Controls

- Hold/click on the left or right side of the player to move.
- Double-tap/click to dash.
- Avoid falling blocks.
- Collect coins before dying.

## Scenes

- `Assets/Scenes/MainMenu.unity`
  - Main menu
  - Options panel
  - Skin shop panel

- `Assets/Scenes/room1.unity`
  - Main gameplay scene
  - Endless block and coin spawning
  - Death summary panel

## Important Scripts

- `Assets/Scripts/Player.cs`
  - Player movement, dash, collisions, and skin application.

- `Assets/Scripts/GameManager.cs`
  - Gameplay loop, spawning, score, earned coins, death panel, retry, and main menu flow.

- `Assets/Scripts/CoinManager.cs`
  - Persistent total coins and coin spending.

- `Assets/Scripts/SkinShopUI.cs`
  - Runtime shop UI, buying/equipping skins, price display, and equipped highlight.

- `Assets/Scripts/SkinDatabase.cs`
  - ScriptableObject database for available skins.

- `Assets/Scripts/PlayerSkinService.cs`
  - Saves owned/equipped skin state with `PlayerPrefs`.

- `Assets/Scripts/PlayerSkinApplier.cs`
  - Applies the selected skin animation overrides to the player.

- `Assets/Scripts/MainMenu.cs`
  - Main menu, shop, options, pause, and navigation button actions.

## Skin Shop

The shop uses `Assets/Resources/SkinDatabase.asset`.

Current skins:

- Virtual Guy, owned by default
- Mask Dude
- Ninja Frog
- Pink Man

The extra skins use character sprites from `Pixel Adventure 1/Assets/Main Characters`.

The shop panel can be adjusted from the Inspector through `SkinShopUI`, including:

- Item size
- Preview size
- Name font size
- Action font size
- Text width
- Coin sprite
- Price icon size
- Equipped highlight colors

If the shop needs to be regenerated, use:

`Tools > Block Fighter > Create Or Update Skin Shop`

This editor tool creates/updates:

- `Assets/Resources/SkinDatabase.asset`
- Generated skin animation clips under `Assets/Generated/SkinAnimations`
- `ShopPanel` in `MainMenu.unity`
- `DeathPanel` in `room1.unity`

## Death Panel

When the player dies, the game no longer jumps immediately back to the main menu. It now shows:

- `Your score: X`
- `You earned: Y`
- `Try Again`
- `Main Menu`

If no death panel is assigned in the scene, `GameManager` creates a simple fallback panel at runtime. For a custom panel, assign these fields on `GameManager`:

- `Death Panel`
- `Death Score Text`
- `Death Earned Coins Text`

The expected child names are:

- `ScoreValue`
- `CoinsValue`

## Unity Version

Built and tested with:

`Unity 2022.3.45f1`

Main packages include:

- Universal Render Pipeline `14.0.11`
- TextMeshPro `3.0.6`
- Unity UI `1.0.0`
- 2D Feature package `2.0.1`

## How To Run

### Play The Build

The playable build is provided through the repository's GitHub Releases page. This keeps the source project in the repository while making the game easy to download and try.

For the Android build:

1. Open the GitHub repository.
2. Go to `Releases`.
3. Download the latest `.apk` file.
4. Transfer the APK to an Android device if needed.
5. Install the APK.
6. Start the game from the installed app.

### Run From Unity

If you want to inspect the source project or make changes:

1. Open the project folder in Unity Hub.
2. Use Unity `2022.3.45f1` or another compatible Unity 2022 LTS version.
3. Open `Assets/Scenes/MainMenu.unity`.
4. Press Play.

## Build Notes

The repository is intended to contain the Unity source project. Playable builds are distributed separately through GitHub Releases.

If you want to create a fresh build from the Unity project:

1. Open `MainMenu.unity`.
2. Enter Play Mode.
3. Test the shop:
   - Buy a skin.
   - Equip it.
   - Start the game and confirm it appears.
4. Test gameplay:
   - Move and dash.
   - Collect coins.
   - Die and confirm the death panel appears.
   - Test `Try Again`.
   - Test `Main Menu`.
5. Build from Unity's Build Settings.

Recommended build flow before uploading:

1. Make a clean Android build from Unity.
2. Install and run the APK on an Android device or emulator, not only Play Mode.
3. Confirm the main menu, shop, gameplay, death panel, retry, and main menu return all work in the build.
4. Commit the source changes.
5. Create a GitHub Release.
6. Upload the APK as a release asset.
7. Add short release notes describing the portfolio polish changes.

## Known Limitations

- This is a small student project, not a production-scale game.
- The UI is functional and portfolio-ready, but still simple.
- The project uses `PlayerPrefs` for coins and skin ownership instead of a full save system.
- Gameplay difficulty is currently based on continuous spawning, without advanced balancing.
- Some asset and scene structure reflects the original learning-project phase.

## Assets

This project uses third-party/free asset packs already included in the Unity project, including:

- Pixel Adventure 1 character assets
- Violet Theme UI assets
- Other imported 2D art/audio assets used during the original university project

When publishing the project publicly or using it in a portfolio, make sure the included third-party assets are allowed under their original licenses.

## Portfolio Note

This project is presented as an example of early Unity learning, later polished with cleaner scripts, a small economy/shop loop, persistent unlocks, and a more complete game-over flow. It is meant to show progress, iteration, and practical Unity fundamentals rather than serve as a large commercial game.
