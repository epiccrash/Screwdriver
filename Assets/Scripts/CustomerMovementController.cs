using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class CustomerMovementController : MonoBehaviour
{
    private const float DestTolerance = 0.5f;
    private const float WanderRadius = 6f;
    private const float MaxTimeToWanderPos = 5f;

    [Header("Wander Wait Timing")]

    [SerializeField]
    private float _minWanderPosWaitTime;

    [SerializeField]
    private float _maxWanderPosWaitTime;

    // Wander Variables
    private bool _isWandering;
    private Transform _currentWanderLocation;
    private float _wanderPauseTimer;
    private float _currWanderTimer;

    // MoveTo Variables
    private Transform _currentDest;
    private bool _didArriveAtDest;
    public delegate void Del();
    private Del _onArrived;
    private bool _isRotating;
    private float _rotateTimeCount;

    private NavMeshAgent _agent;

    void Start()
    {
        _onArrived = null;
        _didArriveAtDest = false;

        _agent = GetComponent<NavMeshAgent>();
        RandomizeWanderWaitTimer();
    }

    // Update is called once per frame
    void Update()
    {
        if (_isWandering)
        {
            if (_agent.remainingDistance <= DestTolerance)
            {
                _wanderPauseTimer -= Time.deltaTime;

                // If its time, lets pick a new destination.
                if (_wanderPauseTimer <= 0)
                {
                    MoveToRandomWanderPosition();
                }
            }
            else
            {
                _currWanderTimer += Time.deltaTime;

                // Have we spent too long trying to wander to our current position?
                // If yes, we might be stuck and should pick another.
                if (_currWanderTimer > _maxWanderPosWaitTime)
                {
                    MoveToRandomWanderPosition();
                }
            }
        }
        else
        {
            if (!_didArriveAtDest && _agent.remainingDistance <= DestTolerance)
            {
                _onArrived?.Invoke();

                _onArrived = null;
                _didArriveAtDest = true;
                _isRotating = true;
            }

            if (_isRotating && _currentDest != null)
            {
                _rotateTimeCount += Time.deltaTime;

                transform.rotation = Quaternion.Slerp(transform.rotation, _currentDest.rotation, _rotateTimeCount);

                // Are we rotated the right way yet?
                _isRotating = !Mathf.Approximately(0, Quaternion.Angle(transform.rotation, _currentDest.rotation));
            }
        }
    }

    public void MoveTo(Transform newDest, Del onArrived)
    {
        _onArrived = onArrived;
        _currentDest = newDest;

        StopRandomWanderBehavior();
        _didArriveAtDest = false;
        _rotateTimeCount = 0;
        _agent.destination = newDest.position;
    }

    private void RandomizeWanderWaitTimer()
    {
        _wanderPauseTimer = UnityEngine.Random.Range(_minWanderPosWaitTime, _maxWanderPosWaitTime);
    }

    public void StartRandomWanderBehavior()
    {
        _isWandering = true;
    }

    public void StopRandomWanderBehavior()
    {
        _isWandering = false;
        _currentWanderLocation = null;
        _wanderPauseTimer = 0;
        _currWanderTimer = 0;
    }

    private void MoveToRandomWanderPosition()
    {
        Vector3 randomDirection = UnityEngine.Random.insideUnitSphere * WanderRadius;
        randomDirection += transform.position;

        NavMeshHit navHit;
        NavMesh.SamplePosition(randomDirection, out navHit, WanderRadius, -1);

        Vector3 newWanderPos = navHit.position;

        newWanderPos.z = Mathf.Max(newWanderPos.z, BarManager.Instance.CustomerZWanderLimit);

        RandomizeWanderWaitTimer();
        _currWanderTimer = 0;
        _agent.destination = newWanderPos;
    }
}
