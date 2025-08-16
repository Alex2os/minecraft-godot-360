using Godot;
using System;

public class GridMap : Godot.GridMap
{
	// Declare member variables here. Examples:
	// private int a = 2;
	// private string b = "text";

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		
	}
	
	public void DestroyBlock(Vector3 world_coordinate){
		
		Vector3 grid_map_coordinate = WorldToMap(world_coordinate);
		SetCellItem((int)grid_map_coordinate.x, (int)grid_map_coordinate.y, (int)grid_map_coordinate.z, -1);
		
	}
	
	public void PlaceBlock(Vector3 world_coordinate, int block_id){
		
		Vector3 grid_map_coordinate = WorldToMap(world_coordinate);
		SetCellItem((int)grid_map_coordinate.x, (int)grid_map_coordinate.y, (int)grid_map_coordinate.z, block_id);
	}
}
