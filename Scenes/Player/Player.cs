using Godot;
using System;

public class Player : KinematicBody
{
	private const float _PlayerSpeed = 8.0f;
	private const float _PlayerJumpVelocity = 10.0f;
	private const float _Gravity = -30.0f;
	private const double _MouseSensitivity = 0.002;
	
	Vector3 velocity =  Vector3.Zero; // we have to make a velocity vector3 for ourselves and use it at the end of the physics process. the character here does not have a velocity property already defined.
	
	[Export] private NodePath _PlayerCameraPath;
	[Export] private NodePath _PlayerCameraRayCastPath;
	
	private Camera _PlayerCamera;
	private RayCast _PlayerCameraRayCast;
	
	public override void _Ready()
	{
		Input.SetMouseMode(Input.MouseModeEnum.Captured);
		
		_PlayerCamera = GetNode<Camera>(_PlayerCameraPath);
		_PlayerCameraRayCast = GetNode<RayCast>(_PlayerCameraRayCastPath);
	}
	
	public override void _UnhandledInput(InputEvent @event){
		
		if(@event is InputEventMouseMotion mouse_motion){
			Vector3 rotation = Rotation;
			Vector3 camera_rotation = _PlayerCamera.Rotation;
			
			rotation.y = rotation.y - mouse_motion.Relative.x * (float)_MouseSensitivity;
			camera_rotation.x = camera_rotation.x - mouse_motion.Relative.y * (float)_MouseSensitivity;
			
			camera_rotation.x = Mathf.Clamp(camera_rotation.x, Mathf.Deg2Rad(-90), Mathf.Deg2Rad(90));
			
			Rotation = rotation;
			_PlayerCamera.Rotation = camera_rotation;
			
		}
	}
	
	public override void _PhysicsProcess(float delta){
		
		if (!IsOnFloor()){
			velocity += new Vector3(0, _Gravity, 0) * (float)delta;
		}
		
		
		if (Input.IsActionPressed("jump") && IsOnFloor()){
			velocity.y = _PlayerJumpVelocity;
		}
		
		Vector2 inputDir = Input.GetVector("left", "right", "up", "down");
		
		Vector3 direction = (Transform.basis * new Vector3(inputDir.x, 0, inputDir.y)).Normalized();
		
		if(direction!= Vector3.Zero){
			velocity.x = direction.x * _PlayerSpeed;
			velocity.z = direction.z * _PlayerSpeed;
		}
		else{
			velocity.x = Mathf.MoveToward(velocity.x, 0, _PlayerSpeed);
			velocity.z = Mathf.MoveToward(velocity.z, 0, _PlayerSpeed);
		}
		
		
		if(Input.IsActionJustPressed("destroy")){
			
			if(_PlayerCameraRayCast.IsColliding()){
				if(_PlayerCameraRayCast.GetCollider().HasMethod("DestroyBlock")){
					_PlayerCameraRayCast.GetCollider().Call("DestroyBlock", _PlayerCameraRayCast.GetCollisionPoint() - _PlayerCameraRayCast.GetCollisionNormal());
				}
			}
		}
		
		if(Input.IsActionJustPressed("place")){
			
			if(_PlayerCameraRayCast.IsColliding()){
				if(_PlayerCameraRayCast.GetCollider().HasMethod("PlaceBlock")){
					_PlayerCameraRayCast.GetCollider().Call("PlaceBlock", _PlayerCameraRayCast.GetCollisionPoint() + _PlayerCameraRayCast.GetCollisionNormal(), 1);
				}
			}
		}
		
		velocity = MoveAndSlide(velocity, Vector3.Up);
	}
	
	
	
}
