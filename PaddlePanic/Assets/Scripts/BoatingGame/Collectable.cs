using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Collectable : MonoBehaviour
{
    public List<Vector3> jumpPositions = new List<Vector3>();
    public Vector2 jumpRange;
    public Vector2 timeRange;
    Sequence sequence;

    public void Start()
    {
        RandomizeJumpPositions();
        Jump();
    }

    public void Jump()
    {
        sequence = DOTween.Sequence();

        for (int i = 1; i < jumpPositions.Count; i++)
        {
            transform.LookAt(jumpPositions[i]);
            sequence.Append(transform.DOJump(
                endValue: jumpPositions[i],
                jumpPower: Random.Range(jumpRange.x, jumpRange.y),
                numJumps: 1,
                duration: Random.Range(timeRange.x,timeRange.y)).SetEase(Ease.InOutSine));
        }
        sequence.SetLoops(-1, LoopType.Restart);
        
    }

    public void RandomizeJumpPositions()
    {
        for (int i = 0; i < jumpPositions.Count - 1; i++)
        {
            Vector3 tempPos = jumpPositions[i];
            int randIdx = Random.Range(i + 1, jumpPositions.Count);
            jumpPositions[i] = jumpPositions[randIdx];
            jumpPositions[randIdx] = tempPos;
        }
        transform.position = jumpPositions[0];
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Boat"))
        {
            CollectablesManager.instance.UpdateCollectablesCount();
        }
    }
}
