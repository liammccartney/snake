using System.Numerics;
using Raylib_cs;
using static Raylib_cs.Raylib;

const int size = 30;
const float speed = size / 5;

const int cellSize = 30;
const int cellCount = 25;
const int screenWidth = cellSize * cellCount;
const int screenHeight = cellSize * cellCount;

Vector2[] initPosition = { 
  new Vector2((screenWidth / 2) - size - size - size, screenHeight / 2) ,
  new Vector2((screenWidth / 2) - size - size, screenHeight / 2) ,
  new Vector2((screenWidth / 2) - size, screenHeight / 2) ,
  new Vector2(screenWidth / 2, screenHeight / 2), 
};
var snake = new List<Vector2>(initPosition);
var right = new Vector2(size, 0);
var left = new Vector2(-size, 0);
var up = new Vector2(0, -size);
var down = new Vector2(0, size);
var direction = right;


InitWindow(screenWidth, screenHeight, "Snake!");
SetTargetFPS(60);

var frame = 1;
while (!WindowShouldClose())
{
  // -- Handle Input --
  if (IsKeyDown(KeyboardKey.Right) && (direction == up || direction == down) ) direction = right;
  if (IsKeyDown(KeyboardKey.Left) && (direction == up || direction == down) ) direction = left;
  if (IsKeyDown(KeyboardKey.Up) && (direction == right || direction == left) ) direction = up;
  if (IsKeyDown(KeyboardKey.Down) && (direction == right || direction == left) ) direction = down;

  // -- Update Model --
  if (frame % 5 == 0)
  {
    snake.RemoveAt(snake.Count - 1);
    var head = snake.First();
    snake.Insert(0, head + direction);
  }

  // -- Draw Frame --
  BeginDrawing();
  ClearBackground(Color.White);
  var i = 0;
  foreach (var snakebit in snake)
  {
    var color = Color.Black;
    DrawRectangleV(snakebit, new Vector2(30, 30), color);
    i++;
  }
  DrawFPS(0, 0);
  DrawText($"{direction}", 0, 20, 20, Color.DarkGreen);
  DrawText($"{snake.FirstOrDefault()}", 0, 40, 20, Color.Red);
  EndDrawing();
  frame++;
}

CloseWindow();
