using System.Numerics;
using Raylib_cs;
using static Raylib_cs.Raylib;

internal class Program
{
  // -- DIMENSIONS --
  static int cellSize = 30;
  static int cellCount = 25;
  static int screenWidth = cellSize * cellCount;
  static int screenHeight = cellSize * cellCount;

  // -- MOVEMENT --
  static Vector2 right = new Vector2(1, 0);
  static Vector2 left = new Vector2(-1, 0);
  static Vector2 up = new Vector2(0, -1);
  static Vector2 down = new Vector2(0, 1);

  static bool gameOver = false;

  private static void Main(string[] args)
  {
    var food = new Food();
    var snake = new Snake(right);


    InitWindow(screenWidth, screenHeight, "Snake!");
    SetTargetFPS(60);

    var direction = right;
    var frame = 1;
    while (!WindowShouldClose())
    {
      if (!gameOver)
      {
        // -- Handle Input --
        if (IsKeyPressed(KeyboardKey.Right) && (direction == up || direction == down)) direction = right;
        if (IsKeyPressed(KeyboardKey.Left) && (direction == up || direction == down)) direction = left;
        if (IsKeyPressed(KeyboardKey.Up) && (direction == right || direction == left)) direction = up;
        if (IsKeyPressed(KeyboardKey.Down) && (direction == right || direction == left)) direction = down;

        // -- Update Model --
        if (frame % 5 == 0)
        {
          snake.Update(direction);
          food.Place(snake.body);
          if (snake.IsSelfColliding() || IsOutOfBounds(snake))
          {
            gameOver = true;
          }

          if (snake.IsColliding(food.position))
          {
            snake.Eat();
            food.eaten = true;
          }
        }
      }
      else
      {
        if (IsKeyPressed(KeyboardKey.Enter))
        {
          snake = new Snake(right);
          food.eaten = false;
          food.Place(snake.body);
          gameOver = false;
        }
      }


      // -- Draw Frame --
      BeginDrawing();
      ClearBackground(Color.White);
      if (!gameOver)
      {
        snake.Draw();
        food.Draw();
      }
      else
      {
        DrawText($"Game Over", 12 * cellSize, 12 * cellSize, 20, Color.DarkPurple);
      }
      DrawText($"Score: {snake.body.Count - 1}", 0, 20, 20, Color.DarkPurple);
      // DrawFPS(0, 0);
      // DrawText($"Facing: {direction}", 0, 20, 20, Color.DarkPurple);
      // DrawText($"Head: {snake.body.FirstOrDefault()}", 0, 40, 20, Color.Black);
      // DrawText($"Food: {food.position}", 0, 60, 20, Color.Green);
      // DrawText($"Colliding: {snake.IsColliding(food.position)}", 0, 80, 20, Color.Pink);
      EndDrawing();
      frame++;
    }

    CloseWindow();

    bool IsOutOfBounds(Snake snake)
    {
      return snake.head.X < 0 || snake.head.Y < 0 || snake.head.X > cellCount - 1 || snake.head.Y > cellCount - 1;
    }

  }


  public class Food
  {
    public Vector2 position { get; set; }
    public bool eaten = true;

    public void Draw()
    {
      DrawRectangleV(position * cellSize, new Vector2(cellSize), Color.Green);
    }

    public void Place(List<Vector2> blackList)
    {
      if (eaten)
      {
        position = new Vector2(GetRandomValue(0, cellCount - 1), GetRandomValue(0, cellCount - 1));
        while (blackList.Contains(position))
        {
          position = new Vector2(GetRandomValue(0, cellCount - 1), GetRandomValue(0, cellCount - 1));
        }
        eaten = false;
      }
    }

  }

  public class Snake
  {
    public Vector2 direction = new();
    public List<Vector2> body;
    public Vector2 head => body.FirstOrDefault();
    public float size;

    public Snake(Vector2 startingDirection)
    {
      direction = startingDirection;
      body = new List<Vector2> { new Vector2(6, 6) };
    }

    public void Draw()
    {
      foreach (var bitPosition in body)
      {
        DrawRectangleV(bitPosition * cellSize, new Vector2(cellSize), Color.Black);
      }
    }

    public void Update(Vector2 newDirection)
    {
      var nextPosition = head + newDirection;
      direction = newDirection;
      body.RemoveAt(body.Count - 1);
      body.Insert(0, nextPosition);
    }

    public bool IsSelfColliding()
    {
      if (body.Count() is 1)
      {
        return false;
      }

      return body.Skip(1).Any(pos => pos == head);
    }

    public bool IsColliding(Vector2 collider)
    {
      return head == collider;
    }

    public void Eat()
    {
      body.Insert(0, head + direction);
    }
  }
}
