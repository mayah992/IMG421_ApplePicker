using UnityEngine;

public class HelperCrow : MonoBehaviour
{
    public GameObject carriedApple;
    public float flySpeed = 6f;

    private bool exiting = false;
    private Vector3 exitDirection;

    void Start()
    {
        // random upward exit direction
        exitDirection = new Vector3(Random.Range(-1f, 1f), 1f, 0f).normalized;
    }

    void Update()
    {
        // handle bird exit after dropping apple
        if (exiting)
        {
            // fly away
            transform.position += exitDirection * flySpeed * Time.deltaTime;

            // destroy when off screen
            if (transform.position.y > 15f)
            {
                Destroy(gameObject);
            }

            return;
        }
        if (carriedApple == null)
        {
            exiting = true;
            return;
        }

        // find basket to track
        GameObject basket = GameObject.FindWithTag("Basket");
        if (basket == null) return;
        Vector3 target = basket.transform.position + new Vector3(0f, 2f, 0f);

        // move toward basket
        transform.position = Vector3.MoveTowards(
            transform.position,
            target,
            flySpeed * Time.deltaTime
        );

        // keep apple positioned
        carriedApple.transform.position =
            transform.position + new Vector3(0f, -1f, 0f);

        // drop apple when close
        if (Vector3.Distance(transform.position, target) < 0.3f)
        {
            carriedApple.GetComponent<Rigidbody2D>().gravityScale = 1f;
            carriedApple = null;

            // start exit phase
            exiting = true;
        }
    }
}


