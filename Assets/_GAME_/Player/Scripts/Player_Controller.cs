using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[SelectionBase]
public class Player_Controller : MonoBehaviour
{

    #region Editor Data

    [Header("Player Inventory")]
    [SerializeField] int victoryPoints = 0;
    [SerializeField] int developmentPoints = 0;

    [Header("Movement Attributes")]
    [SerializeField] float _moveSpeed = 5f;
    
    [Header("Dependencies")]
    [SerializeField] Rigidbody2D _rb;
    [SerializeField] Transform movePoint;

    #endregion

    private void Start() {

        // Moving movePoint reference outside of Player GameObject (was nested for organizational purposes).
        movePoint.parent = null;

    }

    #region Internal Data

    // Prefixed "_" denotes private/protected field to this class.
    private Vector2 _moveDir = Vector2.zero;

    #endregion
    
    #region Movement Logic

    // Returns a boolean as to whether the movePoint's position is a valid position.
    // NOTE: THIS IS A TEMPORARY, NON-DYNAMIC SOLUTION (issues were encountered
    // configuring the RigidBody2D and colliders to interact properly with the map.)
    private bool IsValidPosition() {

        if(movePoint.position.x < -10.5f) {

            movePoint.position = new Vector3 (-10.5f, movePoint.position.y, 0f);

        } else if(movePoint.position.x > 10.5f) {

            movePoint.position = new Vector3 (10.5f, movePoint.position.y, 0f);

        } else if(movePoint.position.y < -3.5f) {

            movePoint.position = new Vector3 (movePoint.position.x, -3.5f, 0f);

        } else if(movePoint.position.y > 3.5f) {

            movePoint.position = new Vector3 (movePoint.position.x, 3.5f, 0f);

        } else {

            return true;

        }

        return false;
        
    }

    // Called every frame.
    private void Update() {

        // Move player character position to position of movePoint.
        transform.position = Vector3.MoveTowards(transform.position, movePoint.position, _moveSpeed * Time.deltaTime);

        // Once character has successfully moved to movePoint (and movePoint is at valid position), accept further input.
        if(Vector3.Distance(transform.position, movePoint.position) == 0f && IsValidPosition()) {

            // If horizontal movement is detected.
            if(Mathf.Abs(Input.GetAxisRaw("Horizontal")) == 1f) {

                movePoint.position += new Vector3(Input.GetAxisRaw("Horizontal"), 0f, 0f);

            }

            // If vertical movement is detected.
            if(Mathf.Abs(Input.GetAxisRaw("Vertical")) == 1f) {

                movePoint.position += new Vector3(0f, Input.GetAxisRaw("Vertical"), 0f);

            }

        }

    }

    #endregion 

}
