using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;

public class MonteCarloTreeSearch : MonoBehaviour
{

    public int budget = 10;

    private MonteCarloNode leaf;
    private MonteCarloNode expandedLeaf;
    private MonteCarloNode simulationResult;
    
    private MonteCarloNode somethingUseful;

    public Vector3 targetDestination;

    //private MonteCarloNode node;

    private int i = 0;

    void Update()
    {
        while (i < budget)
        {
            MCTS();
            i++;
        }
    }

    void MCTS()
    {
        int counter = 0;
        
        MonteCarloNode rootNode = new MonteCarloNode(transform.position, targetDestination);
        do
        {
            leaf = rootNode.Selection();
            expandedLeaf = leaf.Expand();
            //simulationResult = expandedLeaf.rollout();
            //simulationResult.backPropogation(expandedLeaf);
            counter++;
        } while (counter < budget);
        
        Debug.Log("Leaf size: " + rootNode.leaf.Count);
        
        //rootNode.BestLeaf();
        
        //Debug.Log("Leaf size: " + rootNode.BestLeaf().leaf.Count);
        
        draw(rootNode);
        //node = rootNode;
    }

    void draw(MonteCarloNode node)
    {
        List<Vector3> path;
        /*
        while (node.hasLeaf())
        {
            Gizmos.color = Color.red;
            Gizmos.DrawLine(node.currentPosition, node.getLeaf().currentPosition);
        }
        */
        Debug.Log("Node: " + node.currentPosition);
        Debug.Log("Leaf: " + node.getLeaf().currentPosition);
    }
}
