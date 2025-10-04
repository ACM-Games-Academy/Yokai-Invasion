using UnityEngine;

public class YokaiPathing : MonoBehaviour
{
    [SerializeField]
    private Transform playerPos;

    private void Awake()
    {
        playerPos = GameObject.FindGameObjectWithTag("Hero").transform;
    }

    private void FixedUpdate()
    {
        transform.position = Vector3.MoveTowards(transform.position, playerPos.position, 0.125f);
    }
}
