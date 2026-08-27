# GameJamPlus2025 - Project Guidelines & Architecture

This repository contains the Unity 2D game project for **GameJamPlus 2025**, targeting both PC and Mobile (Android/iOS) platforms.

---

## 1. Project Architecture & Key Systems

### A. Rocket Gameplay Mechanics
- **`RocketController.cs`**: Handles rocket thrust (boosting), fuel consumption/regeneration, rotation tilt, and ground collisions.
- **`RocketAutoLaunch.cs`**: Handles initial launch catapult / impulse force and delays player manual control.
- **`RocketStats.cs`**: Stores dynamic stats (upward/forward boost, max fuel, burn/regen rates, slow resistance) modified by upgrades.
- **`RocketAnimationController.cs`**: Manages driver animator, rocket skins based on upgrade level, and particle effects.

### B. Upgrade & Progression System
- **`UpgradeManager.cs`**: Singleton managing upgrade levels (`levelLaunch`, `levelBoost`, `levelFuel`, `levelWall`).
- **`UpgradeButton.cs` / `UpgradeEntry`**: UGUI-driven upgrade entries, cost scaling, and skin unlocks.
- **`ShellManager.cs`**: Manages shell currency for purchasing upgrades.
- **`SaveSystem.cs`**: Handles JSON serialization/deserialization to `Application.persistentDataPath` (`save.json`).

### C. Audio Management
- **`GameAudioManager.cs`**: Singleton audio manager with BGM/SFX channels and volume fading.

---

## 2. Mobile & Input Handling Standards

### A. Gameplay Input vs UI Input (Important)
- **Gameplay Touch (`Input.GetTouch`, `Input.GetMouseButton`)**:
  - Always check `TouchPhase.Began`, `TouchPhase.Moved`, or `TouchPhase.Stationary` for continuous actions (such as holding boost).
  - Use `TouchPhase.Began` for single-tap triggers (such as "Tap anywhere to start").
  - **UI Blocking**: Always protect gameplay input from firing when tapping UI elements using:
    ```csharp
    if (EventSystem.current != null && EventSystem.current.IsPointerOverGameObject(touch.fingerId))
        continue; // or return false
    ```
- **UI Input (UGUI Buttons, Sliders, ScrollViews)**:
  - Standard UGUI components (`Button.onClick`, `Slider.onValueChanged`) automatically handle mobile touch through Unity's `EventSystem` + `InputSystemUIInputModule` / `GraphicRaycaster`.
  - **Do NOT** rewrite UGUI buttons with manual `Input.GetTouch()` scripts. Keep using standard UGUI listeners.

### B. Cross-Platform Fallback
- Always retain fallback checks for Unity Editor and Standalone PC testing:
  - Mouse: `Input.GetMouseButton(0)` / `Input.GetMouseButtonDown(0)`
  - Keyboard: `Input.GetKey(KeyCode.Space)` or `Input.anyKeyDown`

---

## 3. C# & Unity Best Practices

1. **Null Safety with Singletons**:
   - Always use null-conditional operators or guard clauses when accessing singletons (`SaveSystem.Instance?.Save()`, `GameAudioManager.Instance?.PlaySFX(...)`).
2. **Lifecycle & Cleanups**:
   - When disabling gameplay controllers (e.g., in `PlayerGameOver.cs` or during cutscenes), implement `OnDisable()` to stop running animations, particles, and input states.
3. **Framerate Independence**:
   - Any continuous force, rotation lerp, or resource consumption must be multiplied by `Time.deltaTime` (or `Time.fixedDeltaTime` in `FixedUpdate`).
4. **Scene Management & Loading Guards**:
   - Prevent multi-tap race conditions when loading scenes by adding an `isLoading` boolean guard before calling `SceneManager.LoadScene()`.

