using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PushRigidBody : MonoBehaviour
{
    public float pushPower = 2.0f;
    public Animator animator;
    private bool isPushing = false;
    private Rigidbody currentBody = null;
    private void Update()
    {
        if ((Input.GetAxis("Horizontal") == 0 && Input.GetAxis("Vertical") == 0) )
        {
            StopPushAnimation();
            
        }
    }
    private void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.CompareTag("Pushable"))
        {
            Rigidbody body = collision.collider.attachedRigidbody;
            if (body == null || body.isKinematic)
            {
                return;
            }

            Vector3 pushDirection = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
            if (pushDirection.magnitude > 0.1f)
            {
                body.linearVelocity = pushDirection * pushPower;
                StartPushAnimation(body);
            }
            else
            {
                StopPushAnimation();
            }
        }
    }
    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Pushable") && collision.collider.attachedRigidbody == currentBody)
        {
            StopPushAnimation();
        }
    }

    private void StartPushAnimation(Rigidbody body)
    {
        if (!isPushing)
        {
            isPushing = true;
            currentBody = body;
            animator.SetBool("isPushing",isPushing);
            Debug.Log("StartPush Trigger Set");
        }
    }
    private void StopPushAnimation()
    {
        if (isPushing)
        {
            isPushing = false;
            currentBody = null;
            animator.SetBool("isPushing",isPushing);
            Debug.Log("Para esta animación esta cosa?");
        }
    }
}