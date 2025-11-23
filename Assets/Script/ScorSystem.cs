using UnityEngine;

public class ScorSystem : MonoBehaviour
{
    public static int score = 0;       // skor global
    public float distanceStep = 100f;  // setiap 200 pixel = +1 skor

    private float lastX;               // posisi X terakhir dicatat

    void Start()
    {
        score = 0;                     // reset setiap mulai main
        lastX = transform.position.x;  // set posisi awal
    }

    void Update()
    {
        float traveled = transform.position.x - lastX;

        if (traveled >= distanceStep)
        {
            int gained = Mathf.FloorToInt(traveled / distanceStep);
            score += gained;

            lastX += gained * distanceStep;
            Debug.LogError(score);
        }
    }
}
