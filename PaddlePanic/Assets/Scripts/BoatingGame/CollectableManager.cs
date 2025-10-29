using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectableManager : MonoBehaviour
{
    public Animator animator;

    public void Start()
    {
        StartCoroutine(RandomizeAnimation());
    }

    public IEnumerator RandomizeAnimation()
    {
        float randVal = Random.Range(0f, 10f);
        yield return new WaitForSeconds(randVal);
        StartAnimation();
    }

    public void StartAnimation()
    {
        animator.Play("JumpAnimation");
    }

    public void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Boat"))
        {
            Movement.instance.UpdateFishCount();
            gameObject.SetActive(false);
        }
    }
}
