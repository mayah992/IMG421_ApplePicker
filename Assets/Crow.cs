using UnityEngine;

public class Crow : MonoBehaviour
{   
    // poop variables
    public float poopChance = 0.85f;
    private float poopTimer;
    private float poopDelay;
    private bool hasDroppedPoop = false;
    public AudioSource crowPoop;

    // movement variables
    private Transform targetApple;
    public float verticalAdjustSpeed = 5f;
    public float speed = 10f;

    void Start()
    {
        // find first apple to target
        targetApple = FindClosestApple();

        // randomize poop 
        poopDelay = Random.Range(1.5f, 3f);
        poopTimer = 0f;
    }

    void Update()
    {
        // always move right
        Vector3 pos = transform.position;
        pos.x += speed * Time.deltaTime;

        // if target apple is caught or out of screen, retarget closest
        if (targetApple == null)
        {
            targetApple = FindClosestApple();
        }

        // if there is a target apple, adjust Y toward it
        else if (targetApple != null)
        {
            float targetY = targetApple.position.y;
            pos.y = Mathf.Lerp(pos.y, targetY, verticalAdjustSpeed * Time.deltaTime);
        }

        transform.position = pos;

        // destroy crow when off screen
        if (transform.position.x > 25f)
        {
            Destroy(gameObject);
        }

        // handle poop timing
        if (!hasDroppedPoop)
        {
            poopTimer += Time.deltaTime;

            if (poopTimer >= poopDelay)
            {
                TryDropPoop();
                hasDroppedPoop = true;
            }
        }
    }

    void OnCollisionEnter(Collision collision)
    {
         // find out what hit the crow
        GameObject collidedWith = collision.gameObject;
        if (collidedWith.CompareTag("Apple"))
        {
            // steal (destroy) apple
            Destroy(collidedWith.gameObject);
        }
    }

    // poop function
    void TryDropPoop()
    {
        if (Random.value < poopChance)
        {   
            // apply poop to dirt layer
            Basket basket = FindObjectOfType<Basket>();

            if (basket != null)
            {
                crowPoop.Play();
                basket.IncreaseDirtBig();
            }

            Debug.Log("POOP DROP");
        }
    }


    Transform FindClosestApple()
    {
        GameObject[] apples = GameObject.FindGameObjectsWithTag("Apple");

        if (apples.Length == 0)
            return null;

        GameObject closest = apples[0];
        float minDistance = Vector3.Distance(transform.position, closest.transform.position);

        foreach (GameObject apple in apples)
        {
            float dist = Vector3.Distance(transform.position, apple.transform.position);

            if (dist < minDistance)
            {
                minDistance = dist;
                closest = apple;
            }
        }

        return closest.transform;
    }
}
