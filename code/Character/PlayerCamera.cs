
using System;

namespace Rising;

public partial class PlayerCamera : CameraMode
{
	protected Angles OrbitAngles;

	protected float OrbitDistance { get; set; } = 400f;
	protected float TargetOrbitDistance { get; set; } = 400f;
	protected float WheelSpeed => 10f;

	protected Range<int> CameraDistance { get; set; } = new( 300, 500 );
	protected Range<int> PitchClamp { get; set; } = new( 40, 60 );

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

		var wheel = input.MouseWheel;
		if ( wheel != 0 )
		{
			TargetOrbitDistance -= wheel * WheelSpeed;
			TargetOrbitDistance = TargetOrbitDistance.Clamp( CameraDistance.Min, CameraDistance.Max );
		}

		OrbitDistance = OrbitDistance.LerpTo( TargetOrbitDistance, Time.Delta * 10f );

		if ( input.Down( InputButton.SecondaryAttack ) )
		{
			OrbitAngles.yaw += input.AnalogLook.yaw;
			OrbitAngles.pitch += input.AnalogLook.pitch;
			OrbitAngles = OrbitAngles.Normal;
			OrbitAngles.pitch = OrbitAngles.pitch.Clamp( PitchClamp.Min, PitchClamp.Max );
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
