using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Chapter_1_2_2 {

    public class MovingSphere : MonoBehaviour {

        [SerializeField, Range(0f, 100f)]
        float maxSpeed = 10f;

        [SerializeField, Range(0f, 100f)]
        float maxAcceleration = 10f, maxAirAcceleration = 1f;

        [SerializeField, Range(0f, 10f)]
        float jumpHeight = 2f;

        [SerializeField, Range(0, 5)]
        int maxAirJumps = 2;

        Rigidbody body;
        Vector3 velocity;
        Vector3 desiredVelocity;
        bool desiredJump;
        bool onGround;
        int jumpPhase;

        private void Awake() {
            body = GetComponent<Rigidbody>();
        }

        private void Update() {
            Vector2 playerInput;
            playerInput.x = Input.GetAxis("Horizontal");
            playerInput.y = Input.GetAxis("Vertical");
            playerInput = Vector2.ClampMagnitude(playerInput, 1f);

            desiredVelocity = new Vector3(playerInput.x, 0f, playerInput.y) * maxSpeed;

            desiredJump |= Input.GetButtonDown("Jump");
        }

        private void FixedUpdate() {
            UpdateState();

            float acceletaion = onGround ? maxAcceleration : maxAirAcceleration;
            float maxSpeedChange = acceletaion * Time.deltaTime;
            velocity.x = Mathf.MoveTowards(velocity.x, desiredVelocity.x, maxSpeedChange);
            velocity.z = Mathf.MoveTowards(velocity.z, desiredVelocity.z, maxSpeedChange);

            if (desiredJump) {
                desiredJump = false;
                Jump();
            }

            body.velocity = velocity;

            onGround = false;
        }

        void UpdateState() {
            velocity = body.velocity;
            if (onGround) {
                jumpPhase = 0;
            }
        }

        void Jump() {
            if (onGround || jumpPhase < maxAirJumps) {
                jumpPhase += 1;
                float jumpSpeed = Mathf.Sqrt(-2f * Physics.gravity.y * jumpHeight);
                if (velocity.y > 0f) {
                    jumpSpeed = Mathf.Max(jumpSpeed - velocity.y, 0f);
                }
                velocity.y += jumpSpeed;
            }
        }

        private void OnCollisionEnter(Collision collision) {
            //onGround = true;
            EvaluateCollision(collision);
        }

        //private void OnCollisionExit(Collision collision) {
        //    onGround = false;
        //}

        private void OnCollisionStay(Collision collision) {
            //onGround = true;
            EvaluateCollision(collision);
        }

        void EvaluateCollision(Collision collision) {
            for (int i = 0; i < collision.contactCount; i++) {
                Vector3 normal = collision.GetContact(i).normal;
                onGround |= normal.y >= 0.9f;
            }
        }

    }
}
