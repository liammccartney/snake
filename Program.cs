using System.Numerics;
using Raylib_cs;

const int SNAKE_LENGTH = 256;
const int SQUARE_SIZE = 31;
const int screenWidth = 800;
const int screenHeight = 450;

var framesCounter = 0;
var gameOver = false;
var pause = false;

var food = new Food(false);
var snake = new SnakeBit[SNAKE_LENGTH];
var snakePosition = new Vector2[SNAKE_LENGTH];
var allowMove = false;
var offset = new Vector2();
var counterTail = 0;

void InitGame()
{
  framesCounter = 0;
  gameOver = false;
  pause = false;
  counterTail = 1;
  allowMove = false;

  offset.X = screenWidth % SQUARE_SIZE;
  offset.Y = screenHeight % SQUARE_SIZE;

  for (var i = 0; i < SNAKE_LENGTH; i++)
  {
    snake[i].position = new Vector2(offset.X / 2, offset.Y / 2);
    snake[i].size = new Vector2(SQUARE_SIZE, SQUARE_SIZE);
    snake[i].speed = new Vector2(SQUARE_SIZE, 0);

    if (i is 0)
    {
      snake[i].color = Color.DarkBlue;
    }
    else
    {
      snake[i].color = Color.Blue;
    }

    snakePosition[i] = new Vector2(0, 0);
  }

  food.Size = new Vector2(SQUARE_SIZE, SQUARE_SIZE);
  food.Color = Color.SkyBlue;
  food.Active = false;
}

void UpdateGame()
{
  if (!gameOver)
  {
    if (Raylib.IsKeyPressed(KeyboardKey.P)) pause = !pause;
    if (!pause)
    {
      if (Raylib.IsKeyPressed(KeyboardKey.Right) && CanMove(snake[0].speed.X, allowMove))
      {
        snake[0].speed = new Vector2(SQUARE_SIZE, 0);
        allowMove = false;
      }

      if (Raylib.IsKeyPressed(KeyboardKey.Left) && CanMove(snake[0].speed.X, allowMove))
      {
        snake[0].speed = new Vector2(-SQUARE_SIZE, 0);
        allowMove = false;
      }

      if (Raylib.IsKeyPressed(KeyboardKey.Up) && CanMove(snake[0].speed.Y, allowMove))
      {
        snake[0].speed = new Vector2(0, -SQUARE_SIZE);
        allowMove = false;
      }

      if (Raylib.IsKeyPressed(KeyboardKey.Down) && CanMove(snake[0].speed.Y, allowMove))
      {
        snake[0].speed = new Vector2(0, SQUARE_SIZE);
        allowMove = false;
      }

      for (var i = 0; i < counterTail; i++)
      {
        snakePosition[i] = snake[i].position;
      }

      if (framesCounter % 5 == 0)
      {
        for (var i = 0; i < counterTail; i++)
        {
          if (i is 0)
          {
            snake[0].position.X += snake[0].speed.X;
            snake[0].position.Y += snake[0].speed.Y;
            allowMove = true;
          }
          else
          {
            snake[i].position = snakePosition[i - 1];
          }
        }
      }

      if (snake[0].position.X > screenWidth - offset.X ||
          snake[0].position.Y > screenHeight - offset.Y ||
          snake[0].position.X < 0 || snake[0].position.Y < 0)
      {
        gameOver = true;
      }

      for (var i = 1; i < counterTail; i++)
      {
        if (snake[0].position.X == snake[i].position.X &&
            snake[0].position.Y == snake[i].position.Y)
        {
          gameOver = true;
        }
      }

      if (!food.Active)
      {
        food.Active = true;
        food.Position = new Vector2(
            RandomCoord(screenWidth, offset.X),
            RandomCoord(screenHeight, offset.Y)
            );
        for (var i = 0; i < counterTail; i++)
        {
          while (food.Position.X == snake[i].position.X && food.Position.Y == snake[i].position.Y)
          {
            food.Position = new Vector2(RandomCoord(screenWidth, offset.X), RandomCoord(screenHeight, offset.Y));
            i = 0;
          }
        }
      }

      if (snake[0].position.X < (food.Position.X + food.Size.X) &&
          (snake[0].position.X + snake[0].size.X) > food.Position.X &&
          snake[0].position.Y < (food.Position.Y + food.Size.Y) &&
          (snake[0].position.Y + snake[0].size.Y) > food.Position.Y
      )
      {
        snake[counterTail].position = snakePosition[counterTail - 1];
        counterTail += 1;
        food.Active = false;
      }

      framesCounter++;
    }
  }
  else
  {
    if (Raylib.IsKeyPressed(KeyboardKey.Enter))
    {
      InitGame();
      gameOver = false;
    }
  }
}

float RandomCoord(int limit, float offset)
=> Raylib.GetRandomValue(0, (limit / SQUARE_SIZE) - 1) * SQUARE_SIZE + offset / 2;

bool CanMove(float axisSpeed, bool allowMove)
  => allowMove && axisSpeed is 0;


void DrawGame()
{
  Raylib.BeginDrawing();
  Raylib.ClearBackground(Color.RayWhite);
  if (!gameOver)
  {
    for (var i = 0; i < screenWidth / SQUARE_SIZE + 1; i++)
    {
      Raylib.DrawLineV(new Vector2(SQUARE_SIZE * i + offset.X / 2, offset.Y / 2),
new Vector2(SQUARE_SIZE * i + offset.X / 2, screenHeight - offset.Y / 2), Color.LightGray);

      Raylib.DrawLineV(
          new Vector2(offset.X / 2, SQUARE_SIZE * i + offset.Y / 2),
          new Vector2(screenWidth - offset.X / 2, SQUARE_SIZE * i + offset.Y / 2),
          Color.LightGray
          );
    }

    for (var i = 0; i < counterTail; i++)
    {
      Raylib.DrawRectangleV(snake[i].position, snake[i].size, snake[i].color);
    }
    Raylib.DrawRectangleV(food.Position, food.Size, food.Color);
    if (pause)
    {
      Raylib.DrawText("Game Paused", screenWidth / 2 - Raylib.MeasureText("Game Paused", 20) / 2, Raylib.GetScreenHeight() / 2 - 40, 40, Color.Gray);
    }
  }
  else
  {
    Raylib.DrawText("Press [Enter] to Play Again", screenWidth / 2 - Raylib.MeasureText("Press [Enter] to Play Again", 20) / 2, Raylib.GetScreenHeight() / 2 - 50, 20, Color.Gray);
  }
  Raylib.EndDrawing();
}

Raylib.InitWindow(screenWidth, screenHeight, "Snake!");
InitGame();
Raylib.SetTargetFPS(60);
while (!Raylib.WindowShouldClose())
{
  UpdateGame();
  DrawGame();
}
Raylib.CloseWindow();

public struct SnakeBit
{
  public SnakeBit()
  {
  }
  public Vector2 position;
  public Vector2 size;
  public Vector2 speed;
  public Color color;
}

public struct Food
{
  public Food(bool active)
  {
    Active = active;
  }

  public Vector2 Position;
  public Vector2 Size;
  public bool Active;
  public Color Color;
}

