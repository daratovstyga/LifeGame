using UnityEngine;

public class CellClick : MonoBehaviour
{
    void OnMouseDown()
    {
        Game game = Object.FindAnyObjectByType<Game>();       
        if (game.isRunning) return;

        string name = gameObject.name;
        if (name.StartsWith("Cell_"))
        {
            string[] parts = name.Split('_');
            int x = int.Parse(parts[1]);
            int y = int.Parse(parts[2]);
            game.grid[x, y] = !game.grid[x, y];
            GetComponent<SpriteRenderer>().color = game.grid[x, y] ? game.lifeColor : game.baseColor;
        }
    }
}