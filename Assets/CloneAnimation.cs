using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloneAnimation : MonoBehaviour
{
    public Animator animator;
    private float minX = -0.425f;
    private float maxX = 0.425f;
    private float yPosition = -0.25f;
    private float zPosition = 0f;
    private Vector3 destination;
    private float walkSpeed = 0.4f; // Adjust the walking speed as necessary

    private void Walk()
    {

    }

    private IEnumerator WalkRoutine()
    {
        while (true)
        {
            // Choose a random destination within limits
            destination = new Vector3(Random.Range(minX, maxX), yPosition, zPosition);

            // Turn on walking animation
            animator.SetBool("Walk", true);

            // Determine the direction to face based on the destination
            if (destination.x > transform.localPosition.x)
            {
                // Face right
                transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
            }
            else if (destination.x < transform.localPosition.x)
            {
                // Face left
                transform.localScale = new Vector3(-Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
            }

            // Move towards the destination
            while (Vector3.Distance(transform.localPosition, destination) > 0.01f)
            {
                transform.localPosition = Vector3.MoveTowards(transform.localPosition, destination, walkSpeed * Time.deltaTime);
                yield return null;
            }

            // Arrive at destination, turn off walking animation
            animator.SetBool("Walk", false);

            // Wait for a random time between 4 and 20 seconds
            yield return new WaitForSeconds(Random.Range(6f, 18f));
        }
    }

    /*
    public void RandomSpawn() // Spawns clone in at random spot at start
    {
        // Instantiate the clone from the prefab
        //GameObject clone = Instantiate(clonePrefab, transform);

        // Find the InsideRoomArea Transform to position the clone as a child
        Transform insideRoomAreaTransform = transform.Find("InsideRoomArea");

        // If the InsideRoomArea exists, set the parent of the clone to be the InsideRoomArea
        if (insideRoomAreaTransform != null)
        {
            clone.transform.SetParent(insideRoomAreaTransform, false);
        }

        // Call the method to set the random position on the instantiated clone
        CloneAnimation cloneAnimationScript = clone.GetComponent<CloneAnimation>();
        cloneAnimationScript.RandomSpawn();
    } 
    */


    public void RandomSpawn()
    {
        float randomX = Random.Range(minX, maxX);

        // Set the position and log it to the console.
        Vector3 newPosition = new Vector3(randomX, yPosition, zPosition);
        transform.localPosition = newPosition;

        Debug.Log("SpawnCloneAtRandomPosition called: new position set to " + newPosition);
    }

    // Start is called before the first frame update
    void Awake()
    {
        //RandomSpawn();
        StartCoroutine(WalkRoutine());
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
