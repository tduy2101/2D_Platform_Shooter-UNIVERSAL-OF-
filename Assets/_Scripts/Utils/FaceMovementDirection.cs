using UnityEngine;

public class FaceMovementDirection : MonoBehaviour
{
    private Vector3 previousPosition;
    private Vector3 moveDirection;
    private Quaternion targetRoation;
    private float rotationSpeed = 200f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        previousPosition = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        previousPosition -= new Vector3(GameManager.Instance.adjustedWorldSpeed, 0f);
        moveDirection = transform.position - previousPosition;

        targetRoation = Quaternion.LookRotation(Vector3.forward, moveDirection);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRoation, rotationSpeed * Time.deltaTime);

        previousPosition = transform.position;
    }
}
