using System;
using System.Collections;
using UnityEngine;

public class UnitMover : MonoBehaviour
{
    [SerializeField] private float _speed;

    private YieldInstruction _wait;
    private Coroutine _coroutine;

    public event Action ReachedToTarget;

    private void Awake()
    {
        _wait = new WaitForFixedUpdate();
    }

    public void MoveToTarget(Vector3 targetPosition)
    {
        if ( _coroutine != null )
            StopCoroutine(_coroutine);

        _coroutine = StartCoroutine(Moving(targetPosition));
    }

    private IEnumerator Moving(Vector3 targetPosition)
    {
        while (transform.position != targetPosition)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, _speed * Time.deltaTime);
            transform.LookAt(targetPosition);

            yield return _wait;
        }

        ReachedToTarget?.Invoke();
    }
}
