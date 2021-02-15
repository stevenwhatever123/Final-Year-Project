using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.AI;
using Random = System.Random;

public class MonteCarloNode
{

    public int score;
    public int timeVisited;

    public Vector3 currentPosition;

    public Vector3 targetDestination;

    private MonteCarloNode root;
    public List<MonteCarloNode> leaf;
    public List<MonteCarloNode> availableMoves;

    public MonteCarloNode(Vector3 currentPosition, Vector3 targetDestination)
    {
        score = 0;
        timeVisited = 0;
        this.currentPosition = currentPosition;
        this.targetDestination = targetDestination;
        root = null;
        leaf = new List<MonteCarloNode>();
        availableMoves = new List<MonteCarloNode>();

        //AddAvailableMoves();
        Debug.Log("Construction working 1");
    }

    public MonteCarloNode(MonteCarloNode root, Vector3 currentPosition)
    {
        score = 0;
        timeVisited = 0;
        this.currentPosition = currentPosition;
        this.targetDestination = root.targetDestination;
        this.root = root;
        leaf = new List<MonteCarloNode>();
        availableMoves = new List<MonteCarloNode>();
        this.root.leaf.Add(this);
        
        Debug.Log("Construction working 2");
    }

    public MonteCarloNode(MonteCarloNode node)
    {
        score = node.score;
        timeVisited = node.timeVisited;
        currentPosition = node.currentPosition;
        root = node.root;
        leaf = new List<MonteCarloNode>(node.leaf);
        availableMoves = new List<MonteCarloNode>(node.availableMoves);
        
        Debug.Log("Construction working 3");
    }

    public void Backup(int val)
    {
        score += val;
        timeVisited++;
        if (root != null)
        {
            root.Backup(val);
        }
    }

    public MonteCarloNode BestLeaf()
    {
        double bestVal = double.MinValue;
        MonteCarloNode bestLeaf = null;

        foreach (MonteCarloNode node in leaf)
        {
            //double utc = ((double) node.score / (double) node.timeVisited);
            double utc = uctValue(timeVisited, node.score, node.timeVisited);

            if (utc > bestVal)
            {
                bestLeaf = node;
                bestVal = utc;
            }
        }
        
        Debug.Log("Best Leaf working");

        return bestLeaf;
    }

    public void AddAvailableMoves(List<Vector3> Positions)
    {
        foreach (Vector3 v in Positions)
        {
            AddLeaf(new MonteCarloNode(this, v));
        }
    }

    public bool hasLeaf()
    {
        if (this.leaf == null)
        {
            return false;
        }
        else
        {
            return true;
        }
    }

    public MonteCarloNode getLeaf()
    {
        return this.leaf[0];
    }

    public void AddTimeVisit()
    {
        timeVisited++;
    }

    public void AddScore()
    {
        score++;
    }

    public void AddLeaf(MonteCarloNode Leaf)
    {
        leaf.Add(Leaf);
    }

    public double uctValue(int totalVisit, double nodeScore, int nodeVisit)
    {
        if (nodeVisit == 0)
        {
            return int.MaxValue;
        }
        return ((double) nodeScore / (double) nodeVisit) 
               + 1.41 * Math.Sqrt(Math.Log(totalVisit) / (double) nodeVisit);
    }

    public MonteCarloNode findBestNodeWithUCT(MonteCarloNode node)
    {
        int parentVisit = node.timeVisited;
        double bestNodeWithUCT = 0;
        MonteCarloNode bestNode = node;
        foreach (MonteCarloNode leaf in node.leaf)
        {
            double temp = uctValue(parentVisit, leaf.score, leaf.timeVisited);
            if (temp > bestNodeWithUCT)
            {
                bestNodeWithUCT = temp;
                bestNode = leaf;
            }
        }
        
        Debug.Log("Find Best Node with UCT working");

        return bestNode;
    }

    public MonteCarloNode Selection()
    {   
        /*
        MonteCarloNode node = root;
        while (node.leaf.Count != 0 || node.leaf != null)
        {
            node = findBestNodeWithUCT(node);
        }
        Debug.Log("Selection working");
        return node;
        */
        MonteCarloNode node = root;
        while (leaf.Count != 0)
        {
            node = findBestNodeWithUCT(this);
        }
        Debug.Log("Selection working");

        if (node == null)
        {
            return this;
        }
        else
        {
            return node;
        }
    }
    
    public MonteCarloNode Expand()
    {
        Debug.Log("Expansion Working");
        /*
        MonteCarloNode ret = availableMoves[0];
        AddLeaf(ret);
        //availableMoves.Remove(ret);
        Debug.Log("Really Really big problems");
        return ret;
        */
        Random r = new Random();
        if (currentPosition != targetDestination)
        {
            Vector3 move = new Vector3(0, 0, 0);
            float distance = Vector3.Distance(currentPosition, targetDestination);
            if (distance < 3)
            {
                move = new Vector3(targetDestination.x, 
                    targetDestination.y, targetDestination.z);
            }
            else
            {
                //float x = r.Next(-1, 0);
                float x = 0;
                float y = 0;
                float z = r.Next(0, 1);
                /*
                if (currentPosition.x - targetDestination.x > 0)
                {
                    x = r.Next(-1, 0);
                }
                else
                {
                    x = r.Next(0, 1);
                }
                
                if (currentPosition.z - targetDestination.z > 0)
                {
                    z = r.Next(-1, 0);
                }
                else
                {
                    z = r.Next(0, 1);
                }
                */
                move = new Vector3(currentPosition.x + x, 
                    currentPosition.y, currentPosition.z + z );
            }

            MonteCarloNode leaf = new MonteCarloNode(this, move);

            return leaf;
        }

        return null;
    }

    public void backPropogation(MonteCarloNode nodeToExplore)
    {
        Debug.Log("Back Propogation Working");
        MonteCarloNode tempNode = nodeToExplore;
        while (tempNode != null)
        {
            tempNode.AddTimeVisit();
            if (tempNode.targetDestination != targetDestination)
            {
                tempNode.AddScore();
            }

            tempNode = tempNode.root;
        }
    }

    public MonteCarloNode rollout()
    {
        Debug.Log("Rollout Working");
        
        Random r = new Random();
        
        while (currentPosition != targetDestination)
        {
            MonteCarloNode leaf = Expand();
            currentPosition = leaf.currentPosition;
        }
        
        if (currentPosition == targetDestination)
        {
            return this;
        }
        
        return null;
    }
}
