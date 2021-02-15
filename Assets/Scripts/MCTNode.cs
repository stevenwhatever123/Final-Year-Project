using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = System.Random;

public class MCTNode
{
    public int timeVisited;
    
    public Vector3 currentPosition;

    public Vector3 targetDestination;

    public MCTNode root;

    public List<MCTNode> child;

    public bool visited;

    public MCTNode(Vector3 currentPosition, Vector3 targetDestination)
    {
        timeVisited = 0;
        this.currentPosition = currentPosition;
        this.targetDestination = targetDestination;
        root = null;
        child = new List<MCTNode>();
        
        Debug.Log("Constructor working 1");
    }

    public MCTNode(MCTNode root, Vector3 currentPosition)
    {
        timeVisited = root.timeVisited + 1;
        this.currentPosition = currentPosition;
        this.targetDestination = root.targetDestination;
        this.root = root;
        //root.AddChild(this);
        child = new List<MCTNode>();
        
        Debug.Log("Constructor working 2");
    }

    public void AddChild(MCTNode node)
    {
        child.Add(node);
    }

    public bool HasChild()
    {
        if (child.Count < 1)
        {
            return false;
        }
        else
        {
            return true;
        }
    }

    public bool IsRoot()
    {
        if (root == null)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public int GetChildSize()
    {
        return child.Count;
    }

    public void SetVisited()
    {
        visited = true;
    }

    public bool HasVisited()
    {
        return visited;
    }

    public MCTNode Selection()
    {
        Debug.Log("Selection working");
        if (!HasChild() && IsRoot())
        {
            MCTNode childNode = Expansion();
            SetVisited();
            return childNode;
        }
        else if(!HasChild())
        {
            SetVisited();
            return this;
        }
        else
        {
            Random r = new Random();
            int index = r.Next(0, GetChildSize());
            //Debug.Log("Child Size: " + GetChildSize());
            //Debug.Log("Index: " + index);
            MCTNode childNode = child[index];
            SetVisited();
            return childNode.Selection();
        }
    }

    public MCTNode Expansion()
    {
        Debug.Log("Expansion Working");
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
                int z = r.Next(0, 2);
                //float z = 1;
                
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
                    currentPosition.y, currentPosition.z + z);
            }
            
            MCTNode childNode = new MCTNode(this, move);
            Debug.Log("Child Node: " + childNode.GetType());
            this.AddChild(childNode);
            childNode.SetVisited();
            return childNode;

        }

        return null;
    }
}
