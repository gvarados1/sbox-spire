namespace Spire.AI.Navigation;

public class Path 
{
	protected Vector3 CurrentPosition;
	protected Vector3 TargetPosition;

	public List<Vector3> Points { get; protected set; } = new();

	public bool IsEmpty => Points.Count <= 1;

	public const float DestinationLeeway = 5f;

	public void Update( Vector3 from, Vector3 to )
	{
		CurrentPosition = from;

		bool needsRebuild = false;
		if ( !TargetPosition.AlmostEqual( to, DestinationLeeway ) )
		{
			TargetPosition = to;
			needsRebuild = true;
		}

		if ( needsRebuild )
		{
			var fromFixed = NavMesh.GetClosestPoint( from );
			var toFixed = NavMesh.GetClosestPoint( to );

			Points.Clear();
			NavMesh.GetClosestPoint( from );
			NavMesh.BuildPath( fromFixed.Value, toFixed.Value, Points );
		}

		if ( Points.Count <= 1 )
			return;

		var deltaToNext = from - Points[1];
		var delta = Points[1] - Points[0];
		var deltaNormal = delta.Normal;

		if ( deltaToNext.WithZ( 0 ).Length.AlmostEqual( 0, DestinationLeeway ) )
		{
			Points.RemoveAt( 0 );
			return;
		}

		if ( deltaToNext.Normal.Dot( deltaNormal ) >= 1.0f )
		{
			Points.RemoveAt( 0 );
		}
	}

	public float GetDistance( int point, Vector3 from )
	{
		if ( Points.Count <= point ) return float.MaxValue;

		return Points[point].WithZ( from.z ).Distance( from );
	}

	public Vector3 GetDirection( Vector3 position )
	{
		if ( Points.Count == 1 )
			return (Points[0] - position).WithZ( 0 ).Normal;

		return (Points[1] - position).WithZ( 0 ).Normal;
	}

	private Color CurrentActionColor => Color.Yellow.WithAlpha( 1f );
	private Color QueuedActionColor => Color.White.WithAlpha( 0.35f );

	public void Debug()
	{
		var offset = Vector3.Up * 2;

		int i = 0;
		var cachedPoint = Vector3.Zero;
		foreach ( var point in Points )
		{
			var col = i <= 1 ? CurrentActionColor : QueuedActionColor;

			DebugOverlay.Circle( point, Rotation.LookAt( Vector3.Up ), 4f, col, 0f );

			if ( i > 0 )
				DebugOverlay.Line( cachedPoint + offset, point + offset, col );

			cachedPoint = point;
			i++;
		}
	}
}
