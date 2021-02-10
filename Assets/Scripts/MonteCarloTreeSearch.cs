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

    public Vector3 targetDestination;

    void Update()
    {
        MCTS();
    }

    void MCTS()
    {
        int counter = 0;
        
        MonteCarloNode rootNode = new MonteCarloNode(transform.position, targetDestination);
        do
        {
            leaf = rootNode.Selection();
            expandedLeaf = leaf.Expand();
            simulationResult = expandedLeaf.rollout();
            simulationResult.backPropogation(expandedLeaf);
            counter++;
        } while (counter < budget);

        rootNode.BestLeaf();
        
    }

    void draw(MonteCarloNode node)
    {
        List<Vector3> path;
        while (node.hasLeaf())
        {
            Gizmos.color = Color.red;
            Gizmos.DrawLine(node.currentPosition, node.getLeaf().currentPosition);
        }
    }
}
