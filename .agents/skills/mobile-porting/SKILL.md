---
name: mobile-porting
description: >-
  Best practices and reference guide for mobile input handling, touch interactions,
  UI scaling, and mobile platform optimizations in this Unity 2D game project.
---

# Mobile Porting & Touch Interaction Guide

This skill provides standard patterns and best practices for developing and maintaining mobile gameplay and UI features for GameJamPlus2025.

---

## 1. Gameplay Input Patterns

### Continuous Hold (e.g. Rocket Boost in `RocketController.cs`)
When checking for continuous screen press on mobile:
```csharp
public bool CheckBoostInput()
{
    // Mobile Touch
    if (Input.touchCount > 0)
    {
        for (int i = 0; i < Input.touchCount; i++)
        {
            Touch touch = Input.GetTouch(i);
            if (touch.phase == TouchPhase.Began ||
                touch.phase == TouchPhase.Moved ||
                touch.phase == TouchPhase.Stationary)
            {
                // Ignore touch if interacting with UI buttons
                if (EventSystem.current != null && EventSystem.current.IsPointerOverGameObject(touch.fingerId))
                    continue;

                return true;
            }
        }
    }

    // Editor / PC Fallback
    if (Input.GetMouseButton(0))
    {
        if (EventSystem.current != null && EventSystem.current.IsPointerOverGameObject())
            return false;
        return true;
    }

    // Keyboard Fallback
    if (Input.GetKey(KeyCode.Space))
        return true;

    return false;
}
```

### Single Tap Anywhere (e.g. Start Menu in `StartMenu.cs`)
When checking for a single tap/press to advance or start:
```csharp
public bool CheckTapAnywhere()
{
    // Mobile Touch (Only on Began phase)
    for (int i = 0; i < Input.touchCount; i++)
    {
        if (Input.GetTouch(i).phase == TouchPhase.Began)
            return true;
    }

    // Mouse / PC Fallback
    if (Input.GetMouseButtonDown(0) || Input.anyKeyDown)
        return true;

    return false;
}
```

---

## 2. UI Porting Rules (Do & Don't)

### What to Port for UI:
1. **Canvas Scaler**:
   - Set `UiScaleMode` to `Scale With Screen Size`.
   - Set `ReferenceResolution` to `(1920, 1080)`.
   - Set `ScreenMatchMode` to `MatchWidthOrHeight` with value `0.5` (or `0` for landscape).
2. **Touch Targets (Hitbox)**:
   - Ensure interactive UI buttons are at least `64x64px` in reference scale to be comfortable for fingers.
3. **Safe Area (Notched Phones)**:
   - For UI elements anchored to the extreme top or bottom edges, verify they do not collide with camera notches or home indicators.

### What NOT to Port:
- **Do NOT** replace `Button.onClick.AddListener()` or UGUI components with manual `Input.GetTouch()` scripts. Unity UGUI natively handles mobile touch via `EventSystem`.

---

## 3. Performance & Mobile Optimization Checklist

- **Target Frame Rate**: Set `Application.targetFrameRate = 60;` in game initialization.
- **Physics**: Avoid heavy physics calculations inside `Update()`. Synchronize with `FixedUpdate()` when manipulating rigidbodies.
- **Audio Cleanup**: Ensure audio resources fade or stop cleanly when pausing or switching scenes.
- **Save Persistence**: Use `Application.persistentDataPath` for save data files (`save.json`).

