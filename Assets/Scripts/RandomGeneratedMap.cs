using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class RandomGeneratedMap : MonoBehaviour
{
    public GameObject wallPrefab;
    public int width = 40;
    public int height = 40;
    private int[,] maze;
    private List<Vector2> stack = new List<Vector2>();

    void Start()
    {
        maze = new int[width, height];
        GenerateMaze();
    }

    void GenerateMaze()
    {
        // Start from the center
        int x = width / 2;
        int y = height / 2;
        maze[x, y] = 1;

        stack.Add(new Vector2(x, y));

        while (stack.Count > 0)
        {
            Vector2 current = stack[stack.Count - 1];
            stack.RemoveAt(stack.Count - 1);

            List<Vector2> unvisitedNeighbours = new List<Vector2>();

            // Check all four directions
            for (int dx = -4; dx <= 4; dx += 8)
            {
                if (current.x + dx >= 0 && current.x + dx < width && maze[(int)current.x + dx, (int)current.y] == 0)
                {
                    unvisitedNeighbours.Add(new Vector2(current.x + dx, current.y));
                }
            }
            for (int dy = -4; dy <= 4; dy += 8)
            {
                if (current.y + dy >= 0 && current.y + dy < height && maze[(int)current.x, (int)current.y + dy] == 0)
                {
                    unvisitedNeighbours.Add(new Vector2(current.x, current.y + dy));
                }
            }

            if (unvisitedNeighbours.Count > 0)
            {
                Vector2 chosen = unvisitedNeighbours[Random.Range(0, unvisitedNeighbours.Count)];
                maze[(int)chosen.x, (int)chosen.y] = 1;
                maze[(int)current.x + ((int)chosen.x - (int)current.x) / 2, (int)current.y + ((int)chosen.y - (int)current.y) / 2] = 1;
                maze[(int)current.x + ((int)chosen.x - (int)current.x) / 4, (int)current.y + ((int)chosen.y - (int)current.y) / 4] = 1;
                maze[(int)current.x + ((int)chosen.x - (int)current.x) * 3 / 4, (int)current.y + ((int)chosen.y - (int)current.y) * 3 / 4] = 1;

                stack.Add(current);
                stack.Add(chosen);
            }
        }

        // Instantiate walls for each point in the maze that is still a wall
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                if (maze[i, j] == 1)
                {
                    Vector3 position = new Vector3(i, 0, j);
                    Instantiate(wallPrefab, position, Quaternion.identity, transform);
                }
            }
        }
    }
}
