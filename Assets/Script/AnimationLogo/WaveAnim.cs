using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using UnityEngine;

public class WaveAnim : MonoBehaviour
{
    private Vector3 LogoPos;
    public float amplitude = 3f; // how tall the wave
    public float frequency = 3f; //how fast
    // Start is called before the first frame update
    void Start()
    {
        LogoPos = this.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        float newY = LogoPos.y + Mathf.Sin(Time.time * frequency) * amplitude;
        transform.position = new Vector3(LogoPos.x, newY, LogoPos.z);
    }
}
