    using UnityEngine;
    using UnityEngine.SceneManagement;


    public class AppleTree : MonoBehaviour
    {
        [Header("Inscribed")]
        
        // movement variables
        public GameObject applePrefab;
        public float speed = 1f;
        public float leftAndRightEdge = 10f;
        public float changeDirChance = 0.1f;


        // apple drop variables
        public float secondsBetweenAppleDrops = 1f;
        public float speedIncreaseRate = 0.05f;
        public float dropAccelerationRate = 0.005f;
        public float minimumDropDelay = 0.75f;
        public float timeToMaxDifficulty = 30f; // seconds until fully intense
        float difficultyTimer;

        // helper crow for easy mode
        public GameObject helperCrowPrefab;

        // called before first frame
        void Start()
        {
            float progress = Mathf.Clamp01(difficultyTimer / timeToMaxDifficulty);

            // Early game range: 0.5x - 1.3x
            // Late game range: 0.25x - 0.7x

            float minMultiplier = Mathf.Lerp(0.5f, 0.25f, progress);
            float maxMultiplier = Mathf.Lerp(2f, 0.6f, progress);

            float randomDelay = Random.Range(
                secondsBetweenAppleDrops * minMultiplier,
                secondsBetweenAppleDrops * maxMultiplier
            );

            Invoke("DropApple", randomDelay);

        } 
        
        void DropApple()
        {
            // create apple
            GameObject apple = Instantiate<GameObject>( applePrefab);
            apple.transform.position = transform.position;

            // if in east mode, spawn helper crow to drop off apple
            if (SceneManager.GetActiveScene().name == "Easy_Mode")
            {
                GameObject crow = Instantiate(helperCrowPrefab);
                crow.transform.position = apple.transform.position + new Vector3(-1.5f, 1f, 0f);

                HelperCrow helper = crow.GetComponent<HelperCrow>();
                Debug.Log("NICE CROW SPAWNED");

                if (helper != null)
                {
                    helper.carriedApple = apple;
                }
                else
                {
                    Debug.LogError("HelperCrow component missing on prefab!");
                }

            }

            // increase difficulty
           float progress = Mathf.Clamp01(difficultyTimer / timeToMaxDifficulty);

            // Early game range: 0.7x - 1.3x
            // Late game range: 0.5x - 0.9x

            float minMultiplier = Mathf.Lerp(0.5f, 0.15f, progress);
            float maxMultiplier = Mathf.Lerp(2f, 0.6f, progress);

            float randomDelay = Random.Range(
                secondsBetweenAppleDrops * minMultiplier,
                secondsBetweenAppleDrops * maxMultiplier
            );

            Invoke("DropApple", randomDelay);

        }

        // called once per frame
        void Update()
        {
            // basic movement
            Vector3 pos = transform.position;
            pos.x += speed * Time.deltaTime;
            transform.position = pos;

            // change direction
            if (pos.x < -leftAndRightEdge)
            {
                // move right
                speed = Mathf.Abs(speed);
            }
            else if(pos.x > leftAndRightEdge)
            {
                // move left
                speed = -Mathf.Abs(speed);
            }

            if (SceneManager.GetActiveScene().name == "Medium_Mode"
            || SceneManager.GetActiveScene().name == "Hard_Mode")
            {
                difficultyTimer += Time.deltaTime;

                // Increase movement speed over time
                float direction = Mathf.Sign(speed);
                speed += direction * speedIncreaseRate * Time.deltaTime;

                // Decrease apple drop delay over time
                secondsBetweenAppleDrops -= dropAccelerationRate * Time.deltaTime;
                secondsBetweenAppleDrops = Mathf.Max(secondsBetweenAppleDrops, minimumDropDelay);
            }
        }

        void FixedUpdate()
        {
            if(Random.value < changeDirChance)
            {
                speed *= -1;
            }
        }
    }
