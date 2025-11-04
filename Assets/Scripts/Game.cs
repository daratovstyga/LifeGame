using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Game : MonoBehaviour
{
    public int width = 16;
    public int height = 16;
    public float cellSize = 8f;
    public GameObject Cell;
    public GameObject gridContainer;
    public Color baseColor;
    public Color lifeColor;

    public Button startPauseButton;
    public Button randomButton;
    public Button clearButton;
    public Slider speedSlider;

    public bool[,] grid;
    GameObject[,] cells;
    public bool isRunning = false;
    float timer = 0f;
    float stepDelay = 0.2f;

    void Start()
    {
        gridContainer = GameObject.Find("GameGrid");

        grid = new bool[width, height];
        cells = new GameObject[width, height];

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                Vector3 pos = new Vector3(x * cellSize, y * cellSize, 0);
                GameObject cell = Instantiate(Cell, pos, Quaternion.identity);
                cell.transform.SetParent(gridContainer.transform);
                cells[x, y] = cell;
                cell.name = $"Cell_{x}_{y}";
            }
        }

        CenterGrid(gridContainer);
    }

    void CenterGrid(GameObject gridContainer)
    {
        float gridWidth = width * cellSize;
        float gridHeight = height * cellSize;

        Vector3 center = new Vector3(-gridWidth / 2, -gridHeight / 2, 0);
        gridContainer.transform.position = center;
    }

    void Update()
    {
        if (isRunning)
        {
            timer += Time.deltaTime;
            if (timer >= stepDelay)
            {
                Step();
                timer = 0;
            }
        }
    }

    void Step()
    {
        bool[,] newGrid = new bool[width, height];

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                int neighbors = CountNeighbors(x, y);
                bool alive = grid[x, y];

                if (alive && (neighbors == 2 || neighbors == 3))
                    newGrid[x, y] = true;
                else if (!alive && neighbors == 3)
                    newGrid[x, y] = true;
            }
        }

        grid = newGrid;
        UpdateVisuals();
    }

    int CountNeighbors(int x, int y)
    {
        int count = 0;
        for (int dx = -1; dx <= 1; dx++)
        {
            for (int dy = -1; dy <= 1; dy++)
            {
                if (dx == 0 && dy == 0) continue;
                int nx = (x + dx + width) % width;
                int ny = (y + dy + height) % height;
                if (grid[nx, ny]) count++;
            }
        }
        return count;
    }

    void UpdateVisuals()
    {
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                Color c = grid[x, y] ? lifeColor : baseColor;
                cells[x, y].GetComponent<SpriteRenderer>().color = c;
            }
        }
    }

    public void Runner()
    {
        isRunning = !isRunning;
        startPauseButton.GetComponentInChildren<TextMeshProUGUI>().text = isRunning ? "Пауза" : "Старт";
    }

    public void RandomFill()
    {
        if (isRunning) return;
        for (int x = 0; x < width; x++)
            for (int y = 0; y < height; y++)
                grid[x, y] = Random.Range(0, 2) == 1;
        UpdateVisuals();
    }

    public void Clear()
    {
        if (isRunning) return;
        bool[,] newGrid = new bool[width, height];
        grid = newGrid;
        UpdateVisuals();
    }

    public void SpeedChange()
    {
        stepDelay = 1 - speedSlider.value;
    }
}