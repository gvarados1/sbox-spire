namespace Spire;

[Library]
public partial class BouncyProjectileEntity : ProjectileEntity
{
	public float Bounciness { get; set; } = 1f;

	protected override bool HasHitTarget( TraceResult trace )
	{
		if ( LifeTime.HasValue )
		{
			if ( trace.Hit )
			{
				var reflectAmount = Vector3.Reflect( Velocity.Normal, trace.Normal );
				GravityModifier = 0f;
				Velocity = reflectAmount * Velocity.Length * Bounciness;
			}

			return false;
		}

		return base.HasHitTarget( trace );
	}
}
