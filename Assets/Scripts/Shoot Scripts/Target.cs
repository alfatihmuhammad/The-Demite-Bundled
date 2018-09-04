using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MoveTowards))]
[RequireComponent (typeof(RotateTowards))]

public class Target : MonoBehaviour
{
    public TargetZone[] targetZones;
    public GameObject geometryContainer;
    public GameObject destructionParticlesContainer;
    public AudioSource hitClip;

    private TargetManager targetManager;
    private MoveTowards moveTowards;
    private RotateTowards rotateTowards;
    private GameObject player;
    private Vector3 startPosition;
    private ParticleSystem[] destructionParticles;

    private float pointsValueLoss;
    private float pitchVariance = 0.2f;
    private float originalPitch;
    private PointsDisplay pointsDisplay;

    public TargetManager TargetManager
    {
        set { targetManager = value; }
    }

    public GameObject Player
    {
        set { player = value; }
    }

    public void InitTarget()
    {
        //get component
        moveTowards = GetComponent<MoveTowards>();
        rotateTowards = GetComponent<RotateTowards>();
        destructionParticles = destructionParticlesContainer.GetComponentsInChildren<ParticleSystem>();

        //set points value loss
        pointsValueLoss = targetManager.pointsValueLoss;

        //set target transform
        moveTowards.target = player.transform;
        rotateTowards.target = player.transform;

        //store original pitch
        originalPitch = hitClip.pitch;

        //set points display
        pointsDisplay = targetManager.pointsDisplay;

        //enable scripts
        moveTowards.enabled = true;
        rotateTowards.enabled = true;
    }

    public void Reset()
    {
        //set to inactive target list
        targetManager.InactiveTargets.Enqueue(this);
        geometryContainer.SetActive(true);

        //disable target
        gameObject.SetActive(false);
    }

    public void Activate()
    {
        //store starting position for points value loss calculations
        startPosition = transform.position;

        //enable target
        gameObject.SetActive(true);
    }

    public void Hit(RaycastHit hit)
    {
        //get points
        int points = GetPoints(hit.collider);
        pointsDisplay.SetText(points);
        GameManager.instance.AddPoints(points);

        StartCoroutine(Destroy());
    }

    private IEnumerator Destroy()
    {
        //set random pitch and play audio
        hitClip.pitch = Random.Range(originalPitch - pitchVariance, originalPitch + pitchVariance);
        hitClip.Play(); 

        //disable geometry
        geometryContainer.SetActive(false);

        //enable particles
        destructionParticlesContainer.SetActive(true);

        //total time particles to finish
        float maxParticlesDuration = 0;

        //play particles effect
        foreach(ParticleSystem particles in destructionParticles)
        {
            maxParticlesDuration = Mathf.Max(maxParticlesDuration, particles.duration);
            particles.Play();
        }

        //move points display to stopping position
        pointsDisplay.transform.position = moveTowards.StoppingPosition;
        pointsDisplay.transform.LookAt(player.transform);

        //show display points
        pointsDisplay.gameObject.SetActive(true);

        //wait until particles have finished
        yield return new WaitForSeconds(maxParticlesDuration);

        //reset gameobject and hide it
        Reset();

        yield return new WaitForEndOfFrame();
    }

    private int GetPoints(Collider hitTargetZone)
    {
        foreach(TargetZone targetZone in targetZones)
        {
            if(targetZone.collider != hitTargetZone)
            {
                continue;
            }

            return CalculatePointsLoss(targetZone.points);
        }
        return 0;
    }

    private int CalculatePointsLoss(int pointsBase)
    {
        //get distance to compare
        float startDistanceToTarget = Vector3.Distance(startPosition, moveTowards.StoppingPosition);
        float currentDistanceToTarget = Vector3.Distance(transform.position, moveTowards.StoppingPosition);

        //get distance persentage
        float distancePersentage = (startDistanceToTarget * currentDistanceToTarget) / 100;
        distancePersentage = Mathf.Max(0, distancePersentage);

        //hold min and max value
        float maxPoints = pointsBase;
        float minPoints = maxPoints - (pointsBase * pointsValueLoss);

        //calculate linear points loss
        float pointsValue = Mathf.Lerp(minPoints, maxPoints, distancePersentage);
        pointsValue = Mathf.Max(0, pointsValue);

        //round the whole number and return new points
        return Mathf.RoundToInt(pointsValue);
    }

    [System.Serializable]
    public struct TargetZone
    {
        public Collider collider;
        public int points;
    }
}
