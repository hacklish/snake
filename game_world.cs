using Godot;
using System;
using static Godot.GD;

using Snake;
using System.Drawing; // Point
using System.Collections.Generic; // List

public partial class game_world : Node2D
{
	private TileMap godot_board;
	private Label godot_score;
	private Vector2I tile_size;
	private Vector2I screen_size = new Vector2I(16, 32);

	private Snake.Direction movement = Direction.EAST;
	private Snake.GameBoard board;
	private Snake.Snake snake;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		base._Ready();

		godot_score = GetNode<Label>("%ScoreLabel");
		godot_score.Text = "Score: 0";
		
		godot_board = GetNode<TileMap>("%GameTiles");

		tile_size = godot_board.TileSet.TileSize;
		Print("TileSize: " + tile_size);
		Print("ScreenSize: " + screen_size);
		
		godot_board.SetCell(0, new Vector2I(0, 0), 0, new Vector2I(1, 0), 0);
		board = new Snake.GameBoard(screen_size.X, screen_size.Y);
		snake = new Snake.Snake(screen_size.X / 2, screen_size.Y / 2);
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		var headPosition = snake.GetHeadPosition();
		if (board.IsObstacleAt(headPosition))
		{
			// GameOver
		}

		if (board.IsConsumableAt(headPosition))
		{
			board.GenerateBerry();
			snake.Grow();
		}

		board.SetAt(headPosition, TailType.HEAD);
		List<Point> tail = snake.GetTail();
		foreach (Point tailPoint in tail)
		{
			board.SetAt(tailPoint, TailType.SNAKE);
		}

/*
				DateTime loopStart = DateTime.Now;
				while (true)
				{
					DateTime currentTime = DateTime.Now;
					if (currentTime.Subtract(loopStart).TotalMilliseconds > 500) { break; }

					if (Console.KeyAvailable)
					{
						var key = Console.ReadKey(true).Key;
						if (key.Equals(ConsoleKey.UpArrow) && movement != Direction.SOUTH)
						{
							movement = Direction.NORTH;
						}
						if (key.Equals(ConsoleKey.DownArrow) && movement != Direction.NORTH)
						{
							movement = Direction.SOUTH;
						}
						if (key.Equals(ConsoleKey.LeftArrow) && movement != Direction.EAST)
						{
							movement = Direction.WEST;
						}
						if (key.Equals(ConsoleKey.RightArrow) && movement != Direction.WEST)
						{
							movement = Direction.EAST;
						}
					}
				}
		*/

		var toClean = snake.Move(movement, 1);
		foreach (Point clearedPoint in toClean)
		{
			board.ClearAt(clearedPoint);
		}
		
		godot_score.Text = "Score: "; // + snake.GetScore());
	}
}
