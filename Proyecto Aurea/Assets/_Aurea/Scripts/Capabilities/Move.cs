/* 
@Author: Christian Matos
@Date: 2023-06-27 15:39:17
@Last Modified by: Christian Matos
@Last Modified Date: 2023-06-28 20:47

* Functionality: Move a character.
* Approach: Use a Rigidbody2D to move the character with specific acceleration and speed.
* To Use: Attach to a character object.
* Dependencies: GroundCheck.cs, InputController.cs
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Move : MonoBehaviour
{
    // Movement settings
    [SerializeField] private InputController _inputController = null;
    [SerializeField, Range(0f, 100f)] private float maxSpeed = 4f;
    [SerializeField, Range(0f, 100f)] private float maxAcceleration = 35f;
    [SerializeField, Range(0f, 100f)] private float maxAirAcceleration = 20f;

    // Private variable
    private Vector2 direction;
    private Vector2 velocity;
    private Vector2 desiredVelocity;
    private float maxSpeedChange;
    private float acceleration;
    private bool onGround;

    // Onject components
    private Rigidbody2D _rigidbody2D;
    private CollisionCheck _collisionCheck;
    private WallClimb _wallClimb;

    void Awake()
    {
        // Get components
        _rigidbody2D = GetComponent<Rigidbody2D>();
        _collisionCheck = GetComponent<CollisionCheck>();
        _wallClimb = GetComponent<WallClimb>();
    }

    private void Update()
    {
        // Get input every frame
        direction.x = _inputController.RetrieveMoveInput(this.gameObject);
        desiredVelocity = new Vector2(direction.x, 0f) * Mathf.Max(maxSpeed - _collisionCheck.friction, 0f);
    }

    private void FixedUpdate() 
    {
        // Perform physics calculations every fixed frame
        onGround = _collisionCheck.onGround;
        velocity = _rigidbody2D.velocity;

        // Set acceleration and speed according to grounded state
        acceleration = onGround ? maxAcceleration : maxAirAcceleration;
        maxSpeedChange = acceleration * Time.deltaTime;
        velocity.x = Mathf.MoveTowards(velocity.x, desiredVelocity.x, maxSpeedChange);

        // Set velocity
        _rigidbody2D.velocity = velocity;
    }
}
