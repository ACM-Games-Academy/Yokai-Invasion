using UnityEngine;

public class CameraMove : MonoBehaviour
{

    [SerializeField]
    private Vector3 offset;
    private void Update()
    {
        Camera.main.transform.position = transform.position + offset;
    }
}
