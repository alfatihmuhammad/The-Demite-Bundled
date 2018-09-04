using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PointsDisplay : MonoBehaviour
{
    public Text pointsText;
    public float moveSpeed = .5f;
    public float lifetime = 1f;

    void OnEnable()
    {
        StartCoroutine(AutoHide());
    }

    void Update()
    {
        transform.localPosition += Vector3.up * moveSpeed * Time.deltaTime;
    }

    public void SetText(int points)
    {
        pointsText.text = points.ToString();
    }

    private IEnumerator AutoHide()
    {
        yield return new WaitForSeconds(lifetime);

        this.gameObject.SetActive(false);
    }
}
