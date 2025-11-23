// using UnityEngine;

// public class ParallaxGrid : MonoBehaviour
// {
//     public Transform cam;

//     [Header("Parallax Strength")]
//     public float parallaxX = 0.5f;
//     public float parallaxY = 0.2f;

//     [Header("Infinite Tiling")]
//     public bool infiniteX = true;
//     public bool infiniteY = false;

//     [Header("Grid Size")]
//     public int rows = 2;
//     public int cols = 3;

//     [Header("Manual Y Offset per Row")]
//     public float[] rowYOffset; // <- your manual Y for each row

//     private Transform[,] tiles;
//     private float tileWidth;
//     private float tileHeight;

//     void Start()
//     {
//         if (cam == null)
//             cam = Camera.main.transform;

//         if (rowYOffset == null || rowYOffset.Length != rows)
//         {
//             rowYOffset = new float[rows];
//         }

//         tiles = new Transform[rows, cols];

//         int index = 0;
//         foreach (Transform child in transform)
//         {
//             int r = index / cols;
//             int c = index % cols;

//             if (r < rows)
//                 tiles[r, c] = child;

//             index++;
//         }

//         SpriteRenderer sr = GetComponentInChildren<SpriteRenderer>();
//         tileWidth = sr.bounds.size.x;
//         tileHeight = sr.bounds.size.y;

//         ApplyManualRowPositions();
//     }

//     void ApplyManualRowPositions()
//     {
//         for (int r = 0; r < rows; r++)
//         {
//             for (int c = 0; c < cols; c++)
//             {
//                 Transform t = tiles[r, c];
//                 if (t == null) continue;

//                 float x = c * tileWidth;
//                 float y = rowYOffset[r];  // <- manual row height

//                 t.localPosition = new Vector3(x, y, 0);
//             }
//         }
//     }

//     void LateUpdate()
//     {
//         Vector3 camPos = cam.position;

//         // Parallax
//         transform.position = new Vector3(
//             camPos.x * parallaxX,
//             camPos.y * parallaxY,
//             transform.position.z
//         );

//         // Pixel snap to reduce jitter
//         transform.position = new Vector3(
//             Mathf.Round(transform.position.x * 100f) / 100f,
//             Mathf.Round(transform.position.y * 100f) / 100f,
//             transform.position.z
//         );

//         // Tile recycling (infinite scroll)
//         for (int r = 0; r < rows; r++)
//         {
//             for (int c = 0; c < cols; c++)
//             {
//                 Transform tile = tiles[r, c];
//                 if (tile == null) continue;

//                 // Horizontal infinite scroll
//                 if (infiniteX)
//                 {
//                     float dx = camPos.x - tile.position.x;
//                     if (Mathf.Abs(dx) > tileWidth * 1.5f)
//                     {
//                         float offset = Mathf.Sign(dx) * tileWidth * cols;
//                         tile.position += new Vector3(offset, 0, 0);
//                     }
//                 }

//                 // Vertical only for selected rows
//                 if (infiniteY && r == 0) // or whichever row you want
//                 {
//                     float dy = camPos.y - tile.position.y;
//                     if (Mathf.Abs(dy) > tileHeight * 0.75f)
//                     {
//                         float offset = Mathf.Sign(dy) * tileHeight * rows;
//                         tile.position += new Vector3(0, offset, 0);
//                     }
//                 }
//             }
//         }
//     }
// }

// using UnityEngine;

// public class ParallaxGrid : MonoBehaviour
// {
//     public Transform cam;

//     [Header("Parallax Strength")]
//     public float parallaxX = 0.5f;
//     public float parallaxY = 0.2f;

//     [Header("Infinite Tiling")]
//     public bool infiniteX = true;
//     public bool infiniteY = false;

//     [Header("Grid Size")]
//     public int rows = 2;
//     public int cols = 3;

//     [Header("Manual Y Offset per Row")]
//     public float[] rowYOffset;

//     [Header("Smoothing")]
//     public bool pixelPerfect = false;  // Only enable if you really need it
//     public float pixelsPerUnit = 100f; // Set to match your sprite PPU

//     private Transform[,] tiles;
//     private float tileWidth;
//     private float tileHeight;
//     private Vector3 lastCamPos;

//     void Start()
//     {
//         if (cam == null)
//             cam = Camera.main.transform;

//         if (rowYOffset == null || rowYOffset.Length != rows)
//         {
//             rowYOffset = new float[rows];
//         }

//         tiles = new Transform[rows, cols];

//         int index = 0;
//         foreach (Transform child in transform)
//         {
//             int r = index / cols;
//             int c = index % cols;

//             if (r < rows)
//                 tiles[r, c] = child;

//             index++;
//         }

//         SpriteRenderer sr = GetComponentInChildren<SpriteRenderer>();
//         if (sr != null)
//         {
//             tileWidth = sr.bounds.size.x;
//             tileHeight = sr.bounds.size.y;
//         }

//         ApplyManualRowPositions();
//         lastCamPos = cam.position;
//     }

//     void ApplyManualRowPositions()
//     {
//         for (int r = 0; r < rows; r++)
//         {
//             for (int c = 0; c < cols; c++)
//             {
//                 Transform t = tiles[r, c];
//                 if (t == null) continue;

//                 float x = c * tileWidth;
//                 float y = rowYOffset[r];

//                 t.localPosition = new Vector3(x, y, 0);
//             }
//         }
//     }

//     void LateUpdate()
//     {
//         Vector3 camPos = cam.position;

//         // Calculate parallax offset
//         Vector3 targetPos = new Vector3(
//             camPos.x * parallaxX,
//             camPos.y * parallaxY,
//             transform.position.z
//         );

//         // Optional pixel snapping (only if you need pixel-perfect)
//         if (pixelPerfect)
//         {
//             targetPos.x = Mathf.Round(targetPos.x * pixelsPerUnit) / pixelsPerUnit;
//             targetPos.y = Mathf.Round(targetPos.y * pixelsPerUnit) / pixelsPerUnit;
//         }

//         transform.position = targetPos;

//         // Check for tile recycling (with hysteresis to prevent flickering)
//         HandleTileRecycling(camPos);

//         lastCamPos = camPos;
//     }

//     void HandleTileRecycling(Vector3 camPos)
//     {
//         // Calculate world space center positions for recycling checks
//         float recycleThresholdX = tileWidth * 0.75f; // Increased from 1.5 to prevent visible popping
//         float recycleThresholdY = tileHeight * 0.75f;

//         for (int r = 0; r < rows; r++)
//         {
//             for (int c = 0; c < cols; c++)
//             {
//                 Transform tile = tiles[r, c];
//                 if (tile == null) continue;

//                 Vector3 tileWorldPos = tile.position;

//                 // Horizontal infinite scroll
//                 if (infiniteX)
//                 {
//                     float dx = camPos.x - tileWorldPos.x;

//                     // Recycle if tile is far enough off screen
//                     if (dx > recycleThresholdX)
//                     {
//                         // Tile is too far left, move to right
//                         float offset = tileWidth * cols;
//                         tile.position = new Vector3(
//                             tileWorldPos.x + offset,
//                             tileWorldPos.y,
//                             tileWorldPos.z
//                         );
//                     }
//                     else if (dx < -recycleThresholdX)
//                     {
//                         // Tile is too far right, move to left
//                         float offset = tileWidth * cols;
//                         tile.position = new Vector3(
//                             tileWorldPos.x - offset,
//                             tileWorldPos.y,
//                             tileWorldPos.z
//                         );
//                     }
//                 }

//                 // Vertical infinite scroll (typically only for specific rows)
//                 if (infiniteY)
//                 {
//                     float dy = camPos.y - tileWorldPos.y;

//                     if (dy > recycleThresholdY)
//                     {
//                         float offset = tileHeight * rows;
//                         tile.position = new Vector3(
//                             tileWorldPos.x,
//                             tileWorldPos.y + offset,
//                             tileWorldPos.z
//                         );
//                     }
//                     else if (dy < -recycleThresholdY)
//                     {
//                         float offset = tileHeight * rows;
//                         tile.position = new Vector3(
//                             tileWorldPos.x,
//                             tileWorldPos.y - offset,
//                             tileWorldPos.z
//                         );
//                     }
//                 }
//             }
//         }
//     }
// }

using UnityEngine;

public class ParallaxGrid : MonoBehaviour
{
    public Transform cam;

    [Header("Parallax Strength")]
    public float parallaxX = 0.5f;
    public float parallaxY = 0.2f;

    [Header("Infinite Tiling")]
    public bool infiniteX = true;
    public bool infiniteY = false;

    [Header("Grid Size")]
    public int rows = 3;   // You can set 2 or 3 depending on your layer
    public int cols = 3;

    private Transform[,] tiles;
    private float tileWidth;
    private float tileHeight;

    // For absolute parallax
    private Vector3 startPos;

    // CHANGE THIS BASED ON YOUR PIXELS PER UNIT (PPU)
    private float snapUnit = 0.01f; // 1/100 = good for PPU 100, change to 1/32 if PPU=32, etc.

    void Start()
    {
        if (cam == null)
            cam = Camera.main.transform;

        startPos = transform.position;

        tiles = new Transform[rows, cols];

        int index = 0;
        foreach (Transform child in transform)
        {
            int r = index / cols;
            int c = index % cols;

            if (r < rows)
                tiles[r, c] = child;

            index++;
        }

        // Get size of a tile
        SpriteRenderer sr = GetComponentInChildren<SpriteRenderer>();
        tileWidth = sr.bounds.size.x;
        tileHeight = sr.bounds.size.y;

        // Snap all tiles at start (prevents initial jitter/drift)
        SnapAllTilesInitial();
    }

    void LateUpdate()
    {
        Vector3 camPos = cam.position;

        // --- ABSOLUTE PARALLAX (smooth, no jitter) ---
        float px = Mathf.Round((camPos.x * parallaxX) / snapUnit) * snapUnit;
        float py = Mathf.Round((camPos.y * parallaxY) / snapUnit) * snapUnit;

        transform.position = new Vector3(
            startPos.x + px,
            startPos.y + py,
            transform.position.z
        );

        // --- TILE RECYCLING ---
        for (int r = 0; r < rows; r++)
        {
            for (int c = 0; c < cols; c++)
            {
                Transform tile = tiles[r, c];
                if (tile == null) continue;

                // === INFINITE X ===
                if (infiniteX)
                {
                    float dx = camPos.x - tile.position.x;

                    if (Mathf.Abs(dx) > tileWidth * 1.5f)
                    {
                        float offset = Mathf.Sign(dx) * tileWidth * cols;

                        tile.position += new Vector3(offset, 0, 0);

                        SnapTile(tile); // <-- Prevent floating drift
                    }
                }

                // === INFINITE Y ===
                if (infiniteY)
                {
                    float dy = camPos.y - tile.position.y;

                    if (Mathf.Abs(dy) > tileHeight * 1.5f)
                    {
                        float offset = Mathf.Sign(dy) * tileHeight * rows;

                        tile.position += new Vector3(0, offset, 0);

                        SnapTile(tile); // <-- Prevent drifting
                    }
                }
            }
        }
    }

    // Snap tile to grid to avoid floating precision drift
    private void SnapTile(Transform t)
    {
        t.position = new Vector3(
            Mathf.Round(t.position.x / snapUnit) * snapUnit,
            Mathf.Round(t.position.y / snapUnit) * snapUnit,
            t.position.z
        );
    }

    // Snap every tile at start
    private void SnapAllTilesInitial()
    {
        foreach (Transform t in transform)
            SnapTile(t);
    }
}
