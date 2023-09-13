using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class J_PawnMove : MonoBehaviour
{
    int currentX = 0;
    int currentY = 0;
    float currentRotation = 0f;
    bool isMoving = false;
    Animator anim;
    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponentInChildren<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        MovePiece(1, 1);

    }
    // Define variables to track the current position and rotation.


    // Function to move the chess piece forward by a specified number of steps.
    void MoveForward(int steps)
    {
        // Calculate the target position.
        int targetX = currentX;
        int targetY = currentY + steps;

        // Rotate 45 degrees if moving forward by 1 step and the rotation is required.
        if (steps == 1 && currentX + 1 == targetX)
        {
            StartCoroutine(RotatePiece(45f, 1f));
            currentRotation += 45f;
        }

        // Rotate -45 degrees if moving forward by 1 step and the rotation is required.
        if (steps == 1 && currentX - 1 == targetX)
        {
            StartCoroutine(RotatePiece(-45f, 1f));
            currentRotation -= 45f;
        }

        // Update the current position.
        currentX = targetX;
        currentY = targetY;

        // Move forward.
        MovePiece(steps, 1f);
    }

    // Function to rotate the chess piece over time.
    IEnumerator RotatePiece(float targetAngle, float duration)
    {
        float initialAngle = currentRotation;
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            float angle = Mathf.LerpAngle(initialAngle, targetAngle, elapsedTime / duration);
            transform.rotation = Quaternion.Euler(0f, angle, 0f);

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        transform.rotation = Quaternion.Euler(0f, targetAngle, 0f);
        currentRotation = targetAngle;
    }

    // Function to move the chess piece to a target position over time.
    void MovePiece(int steps, float moveSpeed)
    {
        // Calculate the target position as a Vector3.
        Vector3 targetPosition = new Vector3(currentX, transform.position.y, currentY + steps);

        // Move towards the target position.
        isMoving = true;
        anim.SetTrigger("Move");

        while (isMoving)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime / 5);

            // Stop moving when you arrive at the target position.
            if (transform.position == targetPosition)
            {
                isMoving = false;
                anim.SetTrigger("Idle");
            }
        }
    }


}
