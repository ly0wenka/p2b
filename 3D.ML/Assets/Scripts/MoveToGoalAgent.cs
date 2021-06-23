using System;
using System.Collections;
using System.Collections.Generic;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;
using UnityEngine;

public class MoveToGoalAgent : Agent
{
    public override void OnEpisodeBegin()
    {
        transform.localPosition = new Vector3(8,2f, 1f);
    }

    [SerializeField] private Transform targetTransform;
    public override void OnActionReceived(ActionBuffers actions)
    {
        //Debug.Log(actions.DiscreteActions[0]);
        var moveX = actions.ContinuousActions[0];
        var moveZ = actions.ContinuousActions[1];

        var moveSpeed = 1f;
        transform.localPosition += new Vector3(moveX, 0, moveZ) * Time.deltaTime * moveSpeed;
    }

    public override void Heuristic(in ActionBuffers actionsOut)
    {
        ActionSegment<float> continuousAction = actionsOut.ContinuousActions;
        continuousAction[0] = Input.GetAxis("Horizontal");
        continuousAction[1] = Input.GetAxis("Vertical");
    }

    public override void CollectObservations(VectorSensor sensor)
    {
        sensor.AddObservation(transform.localPosition);
        sensor.AddObservation(targetTransform.localPosition);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<Goal>(out var goal))
        {
            SetReward(+1f);
            EndEpisode();
        }
        if (other.TryGetComponent<Wall>(out var wall))
        {
            SetReward(-1f);
            EndEpisode();
        }
    }
}