namespace Spire.Gamemodes.Duel;

public partial class DuelSpectatorCameraMode : CameraMode
{
	public BasePawn Target => (Local.Pawn as DuelSpectatorPawn)?.Target;

	protected Angles OrbitAngles;

	protected float OrbitDistance { get; set; } = 400f;
	protected float TargetOrbitDistance { get; set; } = 400f;
	protected float WheelSpeed => 10f;

	protected Range<int> CameraDistance { get; set; } = new( 125, 500 );
	protected Range<int> PitchClamp { get; set; } = new( 40, 60 );

	public static float ZFarPreference { get; set; } = 2048f;

	public override void Update()
	{
		var pawn = Target;

		if ( Target.IsValid() )
		{
			Position = pawn.Position;
			Position += Vector3.Up * (pawn.CollisionBounds.Center.z * pawn.Scale);
			Rotation = Rotation.From( OrbitAngles );
			Position += Rotation.Backward * OrbitDistance;
		}

		FieldOfView = 70f;

		ZFar = ZFarPreference;
		Viewer = null;
	}

	public override void BuildInput( InputBuilder input )
	{
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
		}

		OrbitAngles.pitch = OrbitAngles.pitch.Clamp( PitchClamp.Min, PitchClamp.Max );

		if ( !Target.IsValid() )
			return;

		Sound.Listener = new()
		{
			Position = Target.Position,
			Rotation = Rotation
		};
	}
}
