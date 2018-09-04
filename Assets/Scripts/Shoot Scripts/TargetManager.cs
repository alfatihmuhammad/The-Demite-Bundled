using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetManager : MonoBehaviour
{
    public GameObject targetPrefab;
    public float spwanDelay = 2f;
    public float timeBetweenSpawnMin = 1f;
    public float timeBetweenSpawnMax = 5f;
    public float spwanRadius = 10f;
    public float maxSpawnHeight = 40f;
    public int maxNumTarget = 20;

    [Range(0, 1), Tooltip("How much % of points is removed when target is at stopping distance (0 = 0%, 1 = 100%")]
    public float pointsValueLoss;

    public PointsDisplay pointsDisplay;

    private List<Target> spawnedTargets = new List<Target>();
    private Queue<Target> inactiveTargets = new Queue<Target>();

    public Queue<Target> InactiveTargets
    {
        get { return inactiveTargets; }
    }

    void Awak()
    {
        //disable on game start because this is controlled by game manager
        this.enabled = false;
    }

    void OnEnable()
    {
        StartCoroutine(SpawnTarget());
    }

    void OnDisable()
    {
        StopCoroutine(SpawnTarget());
        ResetAllTargets();
    }

    public void InitTarget()
    {
        //create target parent game object (for a cleaner outline)
        GameObject targetParent = new GameObject();
        targetParent.name = "Targets";

        //instantiate all targets
        for (int i = 0; i < maxNumTarget; i++)
        {
            Target targetInstance = (Instantiate(targetPrefab) as GameObject).GetComponent<Target>();

            //register target ke manager
            targetInstance.TargetManager = this;

            //set parent
            targetInstance.transform.parent = targetParent.transform;

            //set player target
            targetInstance.Player = GameManager.instance.player;

            //initialize target
            targetInstance.InitTarget();

            //add to target list
            spawnedTargets.Add(targetInstance);
        }
        ResetAllTargets();
    }

    private IEnumerator SpawnTarget()
    {
        //wait before spawning
        yield return new WaitForSeconds(spwanDelay);

        //spawning loop
        while(this.isActiveAndEnabled)
        {
            if(inactiveTargets.Count > 0)
            {
                //get inactive target from queue
                Target target = inactiveTargets.Dequeue();

                //move target to the position and make sure it is possible for the player
                Vector3 position;

                do
                {
                    position = transform.position + Random.onUnitSphere * spwanRadius;
                } while (position.y < transform.position.y || position.y>maxSpawnHeight);

                target.transform.position = position;

                //activate target
                target.Activate();
            }

            //get random wait time
            float waitTime = Random.Range(timeBetweenSpawnMin, timeBetweenSpawnMax);

            yield return new WaitForSeconds(waitTime);
        }
    }

   private void ResetAllTargets()
    {
        //clear target queue
        inactiveTargets.Clear();

        //reset all target list
        foreach(Target target in spawnedTargets)
        {
            target.Reset();
        }
    }
}
