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
