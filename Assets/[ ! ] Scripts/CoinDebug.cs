using UnityEngine;

public class CoinDebug : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Enemy")) 
        {
            Debug.Log(collision.gameObject.name);
        }
    }
}
