using System.Numerics;
using Raylib_cs;
using static Raylib_cs.Raylib;

const int size = 32;
const int screenWidth = 800;
const int screenHeight = 450;
const float speed = size / 5;

var square = (
      position: new Vector2(screenWidth / 2, screenHeight / 2),
      size: new Vector2(size, size),
      speed: new Vector2(speed, 0),
      color: Color.Black,
      alive: true
      );

InitWindow(screenWidth, screenHeight, "Snake!");
SetTargetFPS(60);

while (!WindowShouldClose())
{
  Update();

  BeginDrawing();
  Draw();
  EndDrawing();
}

CloseWindow();

void Draw()
{
  ClearBackground(Color.RayWhite);
  DrawRectangleV(square.position, square.size, square.color);
  DrawFPS(screenWidth - MeasureText("XX FPS", 20), 0);
  DrawText($"{square.position}", screenWidth / 2, screenHeight / 2, 20, Color.Blue);
  if (!square.alive) DrawText("Dead", 0, 0, 40, Color.Red);
}

void Update()
{
  if (square.alive)
  {

    if (IsKeyDown(KeyboardKey.Right)) square.speed = new Vector2(speed, 0);
    if (IsKeyDown(KeyboardKey.Left)) square.speed = new Vector2(-speed, 0);
    if (IsKeyDown(KeyboardKey.Up)) square.speed = new Vector2(0, -speed);
    if (IsKeyDown(KeyboardKey.Down)) square.speed = new Vector2(0, speed);

    square.position += square.speed;

    if (square.position.X < 0 || square.position.Y < 0 || square.position.X > screenWidth || square.position.Y > screenHeight) square.alive = false;
  }
  else
  {
    square.speed = new Vector2(0);
    if (IsKeyPressed(KeyboardKey.Enter))
    {
      square.position = new Vector2(0);
      square.speed = new Vector2(speed, 0);
      square.alive = true;
    }
  }
}
