using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class AnimatronicFramework : MonoBehaviour
{
    NavMeshAgent Agent;
    GameObject Character;
    GameObject[] Points;
    private const float rotSpeed = 20f;

    void Start()
    {
        Agent = GetComponent<NavMeshAgent>();
        Character = GameObject.Find("Character");
    }

    // Update is called once per frame
    void Update()
    {
        Agent.SetDestination(Character.transform.position);

    }
}
