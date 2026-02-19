using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Basket : MonoBehaviour
{
    public ScoreCounter scoreCounter;

    // dirt variables
    public Image dirtImage;
    private float dirtAmount = 0f;
    public float dirtIncrease = 0.7f;

    // shake to scrub basket clean variables
    private float previousX;
    private int lastDirection = 0;
    private float directionChangeTimer = 0f;
    public float scrubSpeedThreshold = 30f; 
    public float scrubTimeWindow = 0.3f;    
    public float scrubCleanAmount = 0.05f;

    // sound variables
    public AudioSource appleCaught;

    // Start is called before the first frame update
    void Start()
    {
        // initialize dirt opacity to 0
        SetDirtAlpha(0f);

        // find reference to score counter game object
        GameObject scoreGO = GameObject.Find("ScoreCounter");
        // get the text component
        scoreCounter = scoreGO.GetComponent<ScoreCounter>();

        // find dirt if applicable (medium and hard mode)
        if(GameObject.Find("Dirt"))
        {
            GameObject dirt = GameObject.Find("Dirt");
            dirtImage = dirt.GetComponent<Image>();
        }

        previousX = transform.position.x;
    }

    // Update is called once per frame
    void Update()
    {
        // get current screen position of the mouse
        Vector3 mousePos2D = Input.mousePosition;
        // camera's z position sets how far to push the mous into 3D
        mousePos2D.z = -Camera.main.transform.position.z;

        // convert the point from 2D screen space into 3D game world space
        Vector3 mousePos3D = Camera.main.ScreenToWorldPoint( mousePos2D );

        // move the basket x position to mouse x position
        Vector3 pos = this.transform.position;
        pos.x = mousePos3D.x;
        this.transform.position = pos;

        // handle scrubbing in Medium and Hard modes
        if (SceneManager.GetActiveScene().name == "Medium_Mode" || 
        SceneManager.GetActiveScene().name == "Hard_Mode")
        {
            float currentX = transform.position.x;
            float deltaX = currentX - previousX;
            float speed = Mathf.Abs(deltaX) / Time.deltaTime;

            int currentDirection = 0;

            if (deltaX > 0.001f)
                currentDirection = 1;
            else if (deltaX < -0.001f)
                currentDirection = -1;

            // count time since last direction flip
            directionChangeTimer += Time.deltaTime;

            if (currentDirection != 0 && currentDirection != lastDirection)
            {
                // direction flipped

                if (speed > scrubSpeedThreshold && directionChangeTimer < scrubTimeWindow)
                {
                    CleanDirt();
                }

                // reset timer on EVERY direction change
                directionChangeTimer = 0f;
            }

            if (currentDirection != 0)
                lastDirection = currentDirection;

            previousX = currentX;

        }
    }

    void OnCollisionEnter(Collision collision)
    {
        // find out what hit the basket
        GameObject collidedWith = collision.gameObject;
        if ( collidedWith.CompareTag("Apple"))
        {
            // make sound
            appleCaught.Play();
            
            // destroy apples caught
            Destroy(collidedWith);

            // add points
            scoreCounter.score += 100;

            // increase opacity of dirt if present (medium mode)
            if (SceneManager.GetActiveScene().name == "Medium_Mode"
            || SceneManager.GetActiveScene().name == "Hard_Mode")
            {
                IncreaseDirt();
            }
        }
    }

    // dirt functions
    void IncreaseDirt()
    {
        SetDirtAlpha(dirtAmount + dirtIncrease);
        Debug.Log("DIRT INCREASED BY APPLE");
    }

    public void IncreaseDirtBig()
    {
        SetDirtAlpha(dirtAmount + 1.0f);
        Debug.Log("DIRT INCREASED BY POOP");
    }

    void CleanDirt()
    {
        SetDirtAlpha(dirtAmount - scrubCleanAmount);
    }

    void SetDirtAlpha(float amount)
    {
        dirtAmount = Mathf.Clamp01(amount);

        Color c = dirtImage.color;
        c.a = dirtAmount;
        dirtImage.color = c;
    }

}
