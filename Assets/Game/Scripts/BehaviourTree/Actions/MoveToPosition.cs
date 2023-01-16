using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TheKiwiCoder;
using MoreMountains.Tools;
using UnityEngine.AI;
using Game.Actors;

[System.Serializable]
[RequireComponent(typeof(MovementController))]
public class MoveToPosition : ActionNode
{
    public float speed = 5;
    public float stopDistance = 1f;
    public bool updateRotation = true;
    public float updatePathInterval = 0.2f;
    public float acceleration = 40.0f;
    public float tolerance = 1.0f;

    private Vector3 _movementDirection;
    private Vector3 _lookDirection;

    private float _elapsedToCalculatePath;
    private int _pathIndex;

    private NavMeshHit _hit;

    private bool IsPathIndexValid => _pathIndex >= context.navMeshPath.corners.Length;

    protected override void OnStart()
    {
        _elapsedToCalculatePath += Time.deltaTime;
        if (_elapsedToCalculatePath >= updatePathInterval)
        {
            _elapsedToCalculatePath = 0;
            CalculatePath();
        }

        UpdateDirection();
        Move();

#if UNITY_EDITOR
        DrawDebugNavigationPath();
#endif
    }

    protected override void OnStop()
    {
    }

    protected override State OnUpdate()
    {
        if (Vector3.Distance(context.transform.position, GetValidTargetPosition()) > tolerance)
        {
            return State.Running;
        }

        if (Vector3.Distance(context.transform.position, GetValidTargetPosition()) < tolerance)
        {
            return State.Success;
        }

        /*if (context.agent.pathStatus == UnityEngine.AI.NavMeshPathStatus.PathInvalid)
        {
            return State.Failure;
        }*/

        return State.Running;
    }

    private void UpdateDirection()
    {
        _movementDirection = Vector3.zero;
        _lookDirection = Vector3.zero;

        if (IsPathIndexValid)
        {
            return;
        }

        float sqrDistance = (context.transform.position - context.navMeshPath.corners[_pathIndex]).sqrMagnitude;
        float sqrStopDistance = stopDistance * stopDistance;
        float sqrRadius = context.movement.CapsuleRadius * context.movement.CapsuleRadius;
        if (sqrDistance <= sqrRadius + sqrStopDistance)
        {
            _pathIndex++;
            if (_pathIndex >= context.navMeshPath.corners.Length)
            {
                return;
            }
        }

        _movementDirection = (context.navMeshPath.corners[_pathIndex] - context.transform.position).normalized;
        _lookDirection = _movementDirection;
    }

    private void Move()
    {
        context.movement.UpdateInputs(_movementDirection, _lookDirection);
    }

    private void CalculatePath()
    {
        Vector3 targetPosition = GetValidTargetPosition();
        NavMesh.CalculatePath(context.transform.position, targetPosition, NavMesh.AllAreas, context.navMeshPath);
    }

    private Vector3 GetValidTargetPosition()
    {
        Vector3 targetPosition = blackboard.moveToPosition;
        if (NavMesh.SamplePosition(targetPosition, out _hit, 5f, NavMesh.AllAreas))
        {
            targetPosition = _hit.position;
        }

        return targetPosition;
    }

    private void DrawDebugNavigationPath()
    {
        Debug.DrawLine(context.transform.position, _hit.position, Color.red);
        for (int i = 0; i < context.navMeshPath.corners.Length - 1; i++)
        {
            Debug.DrawLine(context.navMeshPath.corners[i], context.navMeshPath.corners[i + 1], Color.blue);
        }
    }
}
