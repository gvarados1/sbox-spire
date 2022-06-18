namespace Spire.AI.Navigation;

public struct NavigatorOutput
{
	public bool Finished;
	public Vector3 Direction;
}

public partial class Navigator : BaseNetworkable
{
	/// <summary>
	/// Data that is assigned every frame
	/// </summary>
	public NavigatorOutput Output;

	public Path Path { get; set; } = new();

	public virtual float AvoidanceRadius => 500f;

	protected Vector3 TargetPosition;

	public virtual void Tick( Vector3 currentPosition )
	{
		Path.Update( currentPosition, TargetPosition );

		Output.Finished = Path.IsEmpty;

		if ( Output.Finished )
		{
			Output.Direction = Vector3.Zero;
			return;
		}

		Output.Direction = Path.GetDirection( currentPosition );

		var avoid = GetAvoidance( currentPosition, AvoidanceRadius );
		if ( !avoid.IsNearlyZero() )
		{
			Output.Direction = (Output.Direction + avoid).Normal;
		}

		Path.Debug();
	}

	public void SetTarget( Vector3 position )
	{
		TargetPosition = position;
	}

	// @TODO: Do this another way
	protected Vector3 GetAvoidance( Vector3 position, float radius )
	{
		var center = position + Output.Direction * radius * 0.5f;

		var objectRadius = 200.0f;
		Vector3 avoidance = default;

		foreach ( var ent in Entity.FindInSphere( center, radius ) )
		{
			if ( ent is not BaseCharacter ) continue;
			if ( ent.IsWorld ) continue;

			var delta = (position - ent.Position).WithZ( 0 );
			var closeness = delta.Length;
			if ( closeness < 0.001f ) continue;
			var thrust = ((objectRadius - closeness) / objectRadius).Clamp( 0, 1 );
			if ( thrust <= 0 ) continue;

			avoidance += delta.Normal * thrust * thrust;
		}

		return avoidance;
	}
}
