using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClickParticleEffect2D : MonoBehaviour
{
    [SerializeField] private ParticleSystem clickEffect;

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mousePos.z = 0f;

            PlayEffect(mousePos);
        }
    }

    private void PlayEffect(Vector3 position)
    {
        clickEffect.transform.position = position;
        clickEffect.Play();
    }
}