namespace Rising;

public partial class PlayerCamera : CameraMode
{
	public BaseCharacter Character => Entity as BaseCharacter;

	protected float UpAmount => 128f;
	protected float BackAmount => 64f;

	public override void Update()
	{
		if ( !Character.IsValid() )
			return;

		Position = Character.Position + Vector3.Up * UpAmount + Character.Rotation.Backward * BackAmount;
		Rotation = Character.Rotation * Rotation.FromPitch( 45 );
	}

	public override void Build( ref CameraSetup camSetup )
	{
		base.Build( ref camSetup );

		camSetup.Position = Position;
		camSetup.Rotation = Rotation;
	}
}
