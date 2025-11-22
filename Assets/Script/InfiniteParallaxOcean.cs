using UnityEngine;

public class ParallaxFixGrid : MonoBehaviour
{
    public Transform cam;

    [Header("Parallax Strength")]
    public float parallaxX = 0.5f;
    public float parallaxY = 0.3f;

    [Header("Infinite Scrolling")]
    public bool infiniteX = true;
    public bool infiniteY = true;

    [Header("Grid Size (Your Objects)")]
    public int rows = 3;
    public int cols = 3;

    private Transform[,] tiles;
    private Vector3 lastCamPos;

    private float tileWidth;
    private float tileHeight;

    void Start()
    {
        if (cam == null)
            cam = Camera.main.transform;

        lastCamPos = cam.position;

        SpriteRenderer sr = GetComponentInChildren<SpriteRenderer>();
        tileWidth = sr.bounds.size.x;
        tileHeight = sr.bounds.size.y;

        tiles = new Transform[rows, cols];

        // Read children and assign to grid
        int index = 0;
        foreach (Transform child in transform)
        {
            int r = index / cols;
            int c = index % cols;
            if (r < rows)
                tiles[r, c] = child;
            index++;
        }

        // 🔥 AUTO-FIX ALIGNMENT
        SnapChildrenToPerfectGrid();
    }

    private void SnapChildrenToPerfectGrid()
    {
        float x0 = -Mathf.Floor(cols / 2f) * tileWidth;
        float y0 = -Mathf.Floor(rows / 2f) * tileHeight;

        for (int r = 0; r < rows; r++)
        {
            for (int c = 0; c < cols; c++)
            {
                Transform tile = tiles[r, c];
                if (tile == null)
                    continue;

                float x = x0 + (c * tileWidth);
                float y = y0 + (r * tileHeight);

                tile.localPosition = new Vector3(x, y, 0f);
            }
        }
    }

    void LateUpdate()
    {
        Vector3 delta = cam.position - lastCamPos;

        // parallax motion
        transform.position += new Vector3(
            delta.x * parallaxX,
            delta.y * parallaxY,
            0f
        );

        // infinite tile recycling
        for (int r = 0; r < rows; r++)
        {
            for (int c = 0; c < cols; c++)
            {
                Transform tile = tiles[r, c];
                if (tile == null) continue;

                float dx = cam.position.x - tile.position.x;
                float dy = cam.position.y - tile.position.y;

                if (infiniteX && Mathf.Abs(dx) > tileWidth * 1.5f)
                {
                    float offsetX = Mathf.Sign(dx) * tileWidth * cols;
                    tile.position += new Vector3(offsetX, 0, 0);
                }

                if (infiniteY && Mathf.Abs(dy) > tileHeight * 1.5f)
                {
                    float offsetY = Mathf.Sign(dy) * tileHeight * rows;
                    tile.position += new Vector3(0, offsetY, 0);
                }
            }
        }

        lastCamPos = cam.position;
    }
}
