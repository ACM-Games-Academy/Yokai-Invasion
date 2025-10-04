using UnityEngine;

public class CameraMove : MonoBehaviour
{

    [SerializeField]
    private Vector3 offset = new Vector3(10, 8, 10);
    private void Update()
    {
        Camera.main.transform.position = transform.position + offset;
    }
}
