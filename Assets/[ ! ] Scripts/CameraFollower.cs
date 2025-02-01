using UnityEngine;

public class CameraFollower : MonoBehaviour
{
    private GameObject Player;
    private Vector3 TempPosition;

    [SerializeField] private float X_MinPosition;
    [SerializeField] private float X_MaxPosition;
    void Start()
    {
        
    }

    void Update()
    {
        Player = GameObject.FindGameObjectWithTag("Player");
    }
    private void LateUpdate()
    {
        if (Player != null) 
        {
            TempPosition = this.gameObject.transform.position;
            TempPosition.x = Player.transform.position.x;       

            if (TempPosition.x < X_MinPosition) 
            { 
                TempPosition.x = X_MinPosition;
            }

            if (TempPosition.x > X_MaxPosition)
            {
                TempPosition.x = X_MaxPosition;
            }

            this.gameObject.transform.position = TempPosition;
        }
    }
}
