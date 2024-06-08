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
	private ColorRect godot_endgame;
	private Vector2I screen_size = new Vector2I(22, 16);
	private double total_delta = 0.0;

	private Snake.Direction movement = Direction.EAST;
	private Snake.GameBoard board;
	private Snake.Snake snake;


	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		base._Ready();

		godot_endgame = GetNode<ColorRect>("%GameOverScreen");
		godot_endgame.Hide();
		GetTree().Paused = false;

		godot_board = GetNode<TileMap>("%GameTiles");
		godot_score = GetNode<Label>("%ScoreLabel");
		godot_score.Text = "Score: 0";

		var render = new Snake.GameRender(godot_board);
		board = new Snake.GameBoard(screen_size.X, screen_size.Y, render);
		snake = new Snake.Snake(screen_size.X / 2, screen_size.Y / 2);
	}

	private void _GameOverDo()
	{
		GetTree().Paused = true;
		godot_endgame.Show();
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		godot_score.Text = "Score: " + snake.GetScore();

		var headPosition = snake.GetHeadPosition();
		if (board.IsObstacleAt(headPosition))
			_GameOverDo();

		total_delta += delta;
		if (total_delta < (1/6.0))
			return;
		total_delta = 0.0;

		if (board.IsConsumableAt(headPosition))
		{
			board.GenerateBerry();
			snake.Grow();
		}

		board.SetAt(headPosition, TailType.HEAD, toWhere:movement);
		List<Point> tail = snake.GetTail();
		foreach (Point tailPoint in tail)
		{
			board.SetAt(tailPoint, TailType.SNAKE);
		}

		if (Input.IsActionPressed("MOVE_UP") && movement != Direction.SOUTH)
			movement = Direction.NORTH;
		if (Input.IsActionPressed("MOVE_DOWN") && movement != Direction.NORTH)
			movement = Direction.SOUTH;
		if (Input.IsActionPressed("MOVE_LEFT") && movement != Direction.EAST)
			movement = Direction.WEST;
		if (Input.IsActionPressed("MOVE_RIGHT") && movement != Direction.WEST)
			movement = Direction.EAST;

		var toClean = snake.Move(movement, 1);
		foreach (Point clearedPoint in toClean)
		{
			board.ClearAt(clearedPoint);
		}
	}
}
