using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class FindArea : MonoBehaviour
{
    public GameObject NavMeshSurfaceGameObject;

    public NavMeshSurface surface;

    public GameObject Player;
    
    // Start is called before the first frame update
    void Start()
    {
        NavMeshSurfaceGameObject = GameObject.FindWithTag("NavMesh");
        surface = NavMeshSurfaceGameObject.GetComponent<NavMeshSurface>();

        Player = GameObject.FindWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log(surface.navMeshData);
        //Debug.Log(Player.transform.position);
    }
}
