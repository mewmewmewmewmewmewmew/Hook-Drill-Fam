using UnityEngine;

public class CameraFollows : MonoBehaviour
{
    public GameObject player;
    void LateUpdate()
    {
        transform.position = new Vector3(player.transform.position.x, player.transform.position.y, -10);
    }
}
