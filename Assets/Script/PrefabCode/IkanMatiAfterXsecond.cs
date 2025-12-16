using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IkanMatiAfterXsecond : MonoBehaviour
{
    private void OnTriggerExit2D(Collider2D other)
    {

        if (other.gameObject.tag == "obstacles")
        {
            Debug.Log("ikan mati");
            Destroy(other.gameObject);
        }

    }
}
