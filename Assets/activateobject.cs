using UnityEngine;

public class EnableObject : MonoBehaviour
{
    public GameObject target;

    public void Activate()
    {
        if (target != null)
            target.SetActive(true);
    }
}
