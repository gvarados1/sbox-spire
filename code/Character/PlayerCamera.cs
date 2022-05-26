
using System;

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
		var pawn = Local.Pawn as PlayerCharacter;
		if ( !pawn.IsValid() )
			return;

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
				OrbitAngles.pitch = OrbitAngles.pitch.Clamp( 40, 60 );
			}
		}
		else
		{
			var trace = Trace.Ray( input.Cursor.Origin, input.Cursor.Origin + input.Cursor.Direction * 100000f )
						.WithoutTags( "player" )
						.Radius( 5f )
						.Run();

			var targetDelta = (trace.HitPosition - pawn.Position);
			var targetDirection = targetDelta.Normal;

			DebugOverlay.Sphere( trace.HitPosition, 8f, Color.Green );

			input.ViewAngles = new(
				((float)Math.Asin( targetDirection.z )).RadianToDegree() * -1.0f,
				((float)Math.Atan2( targetDirection.y, targetDirection.x )).RadianToDegree(),
				0.0f );
		}

		// Let players move around at will
		input.InputDirection = input.AnalogMove;
	}
}
