
namespace Rising;

public partial class PlayerCamera : CameraMode
{
	protected Angles OrbitAngles;

	protected float OrbitDistance { get; set; } = 400f;
	protected float WheelSpeed => 10f;

	public override void Update()
	{
		var pawn = Local.Pawn as AnimatedEntity;

		if ( pawn == null )
			return;

		Position = pawn.Position;
		Vector3 targetPos;

		Position += Vector3.Up * (pawn.CollisionBounds.Center.z * pawn.Scale);
		Rotation = Rotation.From( OrbitAngles );

		targetPos = Position + Rotation.Backward * OrbitDistance;

		Position = targetPos;

		FieldOfView = 70;

		Viewer = null;
	}

	public override void BuildInput( InputBuilder input )
	{
		if ( input.Down( InputButton.SecondaryAttack ) )
		{
			var wheel = input.MouseWheel;

			if ( wheel != 0 )
			{
				OrbitDistance -= wheel * WheelSpeed;
				OrbitDistance = OrbitDistance.Clamp( 300, 500 );
			}
			else
			{
				OrbitAngles.yaw += input.AnalogLook.yaw;
				OrbitAngles.pitch += input.AnalogLook.pitch;
				OrbitAngles = OrbitAngles.Normal;
				OrbitAngles.pitch = OrbitAngles.pitch.Clamp( 50, 80 );
			}

			input.AnalogLook = Angles.Zero;
		}

		// Let players move around at will
		input.InputDirection = input.AnalogMove;
	}
}
