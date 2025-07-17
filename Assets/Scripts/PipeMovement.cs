using UnityEngine;

public class PipeMovement : MonoBehaviour
{
    [SerializeField] private float _pipeSpeed = 3f;

    // Update is called once per frame
    void Update()
    {
        transform.position += Vector3.left * Time.deltaTime * _pipeSpeed;
    }
}
