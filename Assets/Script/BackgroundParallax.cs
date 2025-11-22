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

//     private Transform[,] tiles;
//     private float tileWidth;
//     private float tileHeight;

//     void Start()
//     {
//         if (cam == null)
//             cam = Camera.main.transform;

//         tiles = new Transform[rows, cols];

//         // Read children into grid
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
//     }

//     void LateUpdate()
//     {
//         Vector3 camPos = cam.position;

//         // ===== Smooth parallax using absolute camera position =====
//         transform.position = new Vector3(
//             camPos.x * parallaxX,
//             camPos.y * parallaxY,
//             transform.position.z
//         );

//         // (Optional but recommended) pixel snap to stop micro-stutter
//         transform.position = new Vector3(
//             Mathf.Round(transform.position.x * 100f) / 100f,
//             Mathf.Round(transform.position.y * 100f) / 100f,
//             transform.position.z
//         );

//         // ===== Tile recycling =====
//         for (int r = 0; r < rows; r++)
//         {
//             for (int c = 0; c < cols; c++)
//             {
//                 Transform tile = tiles[r, c];
//                 if (tile == null) continue;

//                 // Horizontal infinite
//                 if (infiniteX)
//                 {
//                     float dx = camPos.x - tile.position.x;
//                     if (Mathf.Abs(dx) > tileWidth * 1.5f)
//                     {
//                         float offset = Mathf.Sign(dx) * tileWidth * cols;
//                         tile.position += new Vector3(offset, 0, 0);
//                     }
//                 }

//                 // Vertical infinite
//                 if (infiniteY)
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
using UnityEngine;

public class ParallaxGrid : MonoBehaviour
{
    public Transform cam;

    [Header("Parallax Strength")]
    public float parallaxX = 0.5f;
    public float parallaxY = 0.2f;

    [Header("Infinite Tiling")]
    public bool infiniteX = true;
    public bool infiniteY = false;  // applies only to top row now

    [Header("Grid Size")]
    public int rows = 2;
    public int cols = 3;

    private Transform[,] tiles;
    private float tileWidth;
    private float tileHeight;

    void Start()
    {
        if (cam == null)
            cam = Camera.main.transform;

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

        SpriteRenderer sr = GetComponentInChildren<SpriteRenderer>();
        tileWidth = sr.bounds.size.x;
        tileHeight = sr.bounds.size.y;
    }

    void LateUpdate()
    {
        Vector3 camPos = cam.position;

        // Absolute parallax (smoothest)
        transform.position = new Vector3(
            camPos.x * parallaxX,
            camPos.y * parallaxY,
            transform.position.z
        );

        // Pixel snap (avoid jitter)
        transform.position = new Vector3(
            Mathf.Round(transform.position.x * 100f) / 100f,
            Mathf.Round(transform.position.y * 100f) / 100f,
            transform.position.z
        );

        // Tile recycling
        for (int r = 0; r < rows; r++)
        {
            for (int c = 0; c < cols; c++)
            {
                Transform tile = tiles[r, c];
                if (tile == null) continue;

                // Horizontal infinite scroll (both rows)
                if (infiniteX)
                {
                    float dx = camPos.x - tile.position.x;
                    if (Mathf.Abs(dx) > tileWidth * 1.5f)
                    {
                        float offset = Mathf.Sign(dx) * tileWidth * cols;
                        tile.position += new Vector3(offset, 0, 0);
                    }
                }

                // Vertical infinite scroll (ONLY TOP ROW)
                if (infiniteY && r == 0)
                {
                    float dy = camPos.y - tile.position.y;
                    if (Mathf.Abs(dy) > tileHeight * 0.75f)
                    {
                        float offset = Mathf.Sign(dy) * tileHeight * rows;
                        tile.position += new Vector3(0, offset, 0);
                    }
                }
            }
        }
    }
}
