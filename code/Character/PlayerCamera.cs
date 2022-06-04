
using System;

namespace Spire;

public partial class PlayerCamera : CameraMode
{
	protected Angles OrbitAngles;

	protected float OrbitDistance { get; set; } = 400f;
	protected float TargetOrbitDistance { get; set; } = 400f;
	protected float WheelSpeed => 10f;

	protected Range<int> CameraDistance { get; set; } = new( 125, 500 );
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

		if ( Input.UsingController )
		{
			OrbitAngles.yaw += input.AnalogLook.yaw;
			OrbitAngles.pitch += input.AnalogLook.pitch;
			OrbitAngles = OrbitAngles.Normal;

			input.ViewAngles = OrbitAngles.WithPitch( 0f );
		}
		else if ( input.Down( InputButton.SecondaryAttack ) )
		{
			OrbitAngles.yaw += input.AnalogLook.yaw;
			OrbitAngles.pitch += input.AnalogLook.pitch;
			OrbitAngles = OrbitAngles.Normal;

			input.ViewAngles = OrbitAngles.WithPitch( 0f );
		}
		else
		{
			var direction = Screen.GetDirection( Mouse.Position, FieldOfView, Rotation, Screen.Size );
			var hitPos = Utils.PlaneIntersectionWithZ( Position, direction, pawn.EyePosition.z );

			input.ViewAngles = (hitPos - pawn.EyePosition).EulerAngles;
		}

		OrbitAngles.pitch = OrbitAngles.pitch.Clamp( PitchClamp.Min, PitchClamp.Max );

		// Let players move around at will
		input.InputDirection = Rotation.From( OrbitAngles.WithPitch( 0f ) ) * input.AnalogMove;
	}
}
