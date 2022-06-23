namespace Spire;

public partial class StickyProjectileEntity : ProjectileEntity
{
	public Entity AttachedEntity { get; set; }

	public virtual float DestroyTimeOnCharacter { get; set; } = 1f;
	public virtual float NewDestroyTime { get; set; } = 60f;

	protected virtual float GetDestroyTime( Entity hitEntity )
	{
		if ( hitEntity is BaseCharacter )
			return DestroyTimeOnCharacter;

		return NewDestroyTime;
	}

	public override void Simulate()
	{
		if ( AttachedEntity.IsValid() )
		{
			if ( DestroyTime )
			{
				PlayHitEffects( Vector3.Zero );
				Delete();
			}
		}
		else
		{
			base.Simulate();
		}
	}

	protected override bool HasHitTarget( TraceResult trace )
	{
		if ( trace.Entity.IsValid() )
		{
			AttachedEntity = trace.Entity;
			SetParent( trace.Entity );
			DestroyTime = GetDestroyTime( trace.Entity );

			OnHitAction?.Invoke( this, AttachedEntity );
		}

		return false;
	}
}
