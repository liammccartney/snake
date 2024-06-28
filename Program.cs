using Raylib_cs;

Raylib.InitWindow(800, 400, "Hello World");

while(!Raylib.WindowShouldClose())
{
  Raylib.BeginDrawing();
  Raylib.ClearBackground(Color.White);

  Raylib.DrawText("Hello, World!", 12, 12, 20, Color.Black);

  Raylib.EndDrawing();
}

Raylib.CloseWindow();
