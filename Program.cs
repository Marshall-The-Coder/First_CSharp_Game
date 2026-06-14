using Raylib_cs;
using System.Numerics;

class Program
{
    static void Main()
    {
        Raylib.InitWindow(800, 600, "My Game");
        Raylib.SetTargetFPS(60);

        // Create the player once, outside the main loop
        Player player = new Player(new Vector2(400, 300), new Vector2(10, 10), Color.Green);
        Block block = new Block(new Vector2(200, 200), new Vector2(100, 100), Color.Red);
        Block block2 = new Block(new Vector2(200, 500), new Vector2(50, 50), Color.Orange);

        while (!Raylib.WindowShouldClose())
        {
            player.Update();
            Block.GetAll().ForEach(b => Collisions.Collides(player, b));

            Raylib.BeginDrawing();
            Raylib.ClearBackground(Color.Blue);

            player.Draw();
            Block.GetAll().ForEach(b => b.Draw());
            

            Raylib.EndDrawing();
        }

        Raylib.CloseWindow();
    }
}

public class Player
{
    public Vector2 Position { get; set; }
    public Vector2 Velocity { get; set; }
    public Vector2 Size { get; set; }

    public Color Color { get; set; }

    public Player(Vector2 position, Vector2 size, Color color)
    {
        Position = position;
        Velocity = Vector2.Zero;
        Size = size;
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
            float speed = 250f; // units per second
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
        Raylib.DrawRectangleV(Position, Size, Color);
    }
}
public class Block
{
    static List<Block> all = [];
    public Vector2 Position { get; set; }
    public Vector2 Size { get; set; }
    public Color Color { get; set; }


    public Block(Vector2 position, Vector2 size, Color color)
    {
        Position = position;
        Color = color;
        Size = size;
        Block.all.Add(this);
    }
    public void Draw()
    {
        Raylib.DrawRectangleV(Position, Size, Color);
    }
    public static List<Block> GetAll()
    {
        return Block.all;
    }
}
public static class Collisions
{
    public static void Collides(Player player, Block block)
    {
        if (player.Position.X < block.Position.X + block.Size.X &&
            player.Position.X + player.Size.X > block.Position.X &&
            player.Position.Y < block.Position.Y + block.Size.Y &&
            player.Position.Y + player.Size.Y > block.Position.Y)
        {
            double OverLapTop = player.Position.Y + player.Size.Y - block.Position.Y;
            double OverLapBottom = block.Position.Y + block.Size.Y - player.Position.Y;
            double OverLapLeft = player.Position.X + player.Size.X - block.Position.X;
            double OverLapRight = block.Position.X + block.Size.X - player.Position.X;

            double MinOverLap = Math.Min(Math.Min(OverLapTop, OverLapBottom), Math.Min(OverLapLeft, OverLapRight));

            if (MinOverLap == OverLapTop)
            {
                Vector2 playerPosition = player.Position;
                playerPosition.Y = block.Position.Y - player.Size.Y;
                player.Position = playerPosition;
            }
            else if (MinOverLap == OverLapBottom)
            {
                Vector2 playerPosition = player.Position;
                playerPosition.Y = block.Position.Y + block.Size.Y;
                player.Position = playerPosition;
            }
            else if (MinOverLap == OverLapLeft)
            {
                Vector2 playerPosition = player.Position;
                playerPosition.X = block.Position.X - player.Size.X;
                player.Position = playerPosition;
            }
            else if (MinOverLap == OverLapRight)
            {
                Vector2 playerPosition = player.Position;
                playerPosition.X = block.Position.X + block.Size.X;
                player.Position = playerPosition;
            }
        }
    }
}
