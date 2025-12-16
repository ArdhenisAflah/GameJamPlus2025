// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;

// public class ParallaxHandleTiled : MonoBehaviour
// {
//     private Transform camera;

//     private int spriteWidth;
//     private int spriteHeight;
//     // Start is called before the first frame update
//     void Start()
//     {
//         camera = Camera.main.transform;
//         SpriteRenderer sr = GetComponentInChildren<SpriteRenderer>();
//         spriteWidth = (int)sr.bounds.size.x; //read tile mode size
//         spriteHeight = (int)sr.bounds.size.y; //read tile mode size

//         Debug.LogError(spriteWidth);
//     }

//     // Update is called once per frame
//     void LateUpdate()
//     {

//         float dx = camera.position.x - tile.position.x;



//     }
// }
