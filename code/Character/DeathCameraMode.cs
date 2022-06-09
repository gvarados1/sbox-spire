namespace Spire;

public partial class DeathCameraMode : CameraMode
{
	Vector3 FocusPoint;

	public override void Activated()
	{
		base.Activated();

		FocusPoint = CurrentView.Position;
		FieldOfView = CurrentView.FieldOfView;
	}

	public override void Update()
	{
		var player = Local.Client;
		if ( player == null )
			return;

		Position = FocusPoint + GetViewOffset();
		Rotation = Input.Rotation;
		FieldOfView = FieldOfView.LerpTo( 50, Time.Delta * 3.0f );

		Viewer = null;
	}

	public override void BuildInput( InputBuilder input )
	{
	}

	public virtual Vector3 GetViewOffset()
	{
		var player = Local.Client;
		if ( player == null ) return Vector3.Zero;

		return Input.Rotation.Forward * (-130 * 1) + Vector3.Up * (20 * 1);
	}
}
