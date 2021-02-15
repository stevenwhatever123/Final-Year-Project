using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MCTS : MonoBehaviour
{
    public int budget = 1;

    private MCTNode leaf;
    private MCTNode expandedLeaf;
    private MCTNode simulationResult;
    
    private MCTNode somethingUseful;

    public Vector3 targetDestination;
    
    private int i = 0;
    
    void Update()
    {
        while (i < budget)
        {
            MCTSmethod();
            i++;
        }
    }
    
    void MCTSmethod()
    {

        MCTNode rootNode = new MCTNode(transform.position, targetDestination);
        leaf = rootNode.Selection(); 
        //expandedLeaf = leaf.Expansion(); 
        //simulationResult = expandedLeaf.rollout();
        //simulationResult.backPropogation(expandedLeaf);

        while (leaf.currentPosition != targetDestination)
        {
            expandedLeaf = leaf.Expansion();
            leaf = expandedLeaf;
            
        }

        Debug.Log("Leaf size: " + rootNode.child.Count);
        
        draw(rootNode, expandedLeaf);
    }
    
    void draw(MCTNode node, MCTNode result)
    {
        List<Vector3> path;
        
        Debug.Log("Node: " + node.currentPosition);
        //Debug.Log("Leaf: " + result.currentPosition);
        printNode(node);
    }

    void printNode(MCTNode node)
    {
        Debug.Log("Leaf: " + node.currentPosition);
        for (int i = 0; i < node.GetChildSize(); i++)
        {
            if (node.child[i].HasVisited())
            {
                printNode(node.child[i]);
            }
        }
    }
}
