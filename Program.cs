using Raylib_cs;
using System.Numerics;

class Program
{
    static void Main()
    {
        Raylib.InitWindow(800, 600, "My Game");
        Raylib.SetTargetFPS(60);

        // Create the player once, outside the main loop
        Player player = new Player(new Vector2(400, 300), Color.Green);
        Block block = new Block(new Vector2(200, 200), Color.Red, new Vector2(100, 100));

        while (!Raylib.WindowShouldClose())
        {
            player.Update();

            Raylib.BeginDrawing();
            Raylib.ClearBackground(Color.Blue);

            player.Draw();
            block.Draw();

            Raylib.EndDrawing();
        }

        Raylib.CloseWindow();
    }
}

class Player
{
    public Vector2 Position { get; set; }
    public Vector2 Velocity { get; set; }

    public Color Color { get; set; }

    public Player(Vector2 position, Color color)
    {
        Position = position;
        Velocity = Vector2.Zero;
        Color = color;
    }
    public void Update()
    {
        // Simple WASD movement using Raylib input
        Vector2 input = new Vector2(0, 0);
        if (Raylib.IsKeyDown(KeyboardKey.W)) input.Y -= 1;
        if (Raylib.IsKeyDown(KeyboardKey.S)) input.Y += 1;
        if (Raylib.IsKeyDown(KeyboardKey.A)) input.X -= 1;
        if (Raylib.IsKeyDown(KeyboardKey.D)) input.X += 1;

        if (input != Vector2.Zero)
        {
            // Convert input to an angle (theta)
            float theta = MathF.Atan2(input.Y, input.X);
            float speed = 200f; // units per second
            float dt = Raylib.GetFrameTime();

            // Because Position is a struct property, modify a local copy and assign it back
            Vector2 position = Position;
            position.X += MathF.Cos(theta) * speed * dt;
            position.Y += MathF.Sin(theta) * speed * dt;
            Position = position;
        }
    }

    public void Draw()
    {
        Raylib.DrawCircleV(Position, 10, Color);
    }
}
class Block
{
    public Vector2 Position { get; set; }
    public Vector2 Size { get; set; }
    public Color Color { get; set; }

    public Block(Vector2 position, Color color, Vector2 size)
    {
        Position = position;
        Color = color;
        Size = size;
    }
    public void Draw()
    {
        Raylib.DrawRectangleV(Position, Size, Color);
    }
}