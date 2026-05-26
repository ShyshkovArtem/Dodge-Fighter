using UnityEngine;

public class Block : MonoBehaviour
{
    private const float DestroyBelowY = -6f;

    private void Update()
    {
        if (transform.position.y < DestroyBelowY)
        {
            Destroy(gameObject);
        }
    }
}
