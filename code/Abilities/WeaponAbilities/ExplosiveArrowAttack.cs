namespace Spire.Abilities;

public partial class ExplosiveArrowAttack : BasicArrowAttack
{

	// Configuration
	public override string Identifier => "explosive_arrow_attack";
	public override WeaponAbilityType Type => WeaponAbilityType.Ultimate;

	protected async Task DelayedExplosion( Entity hitEntity )
	{
		await GameTask.DelaySeconds( 0.5f );

		new ExplosionEntity
		{
			Position = hitEntity.Position,
			Radius = Data.AbilityEffectRadius,
			Damage = 50f,
			ForceScale = 1f,
		}.Explode( Owner );
	}

	protected override void OnProjectileHit( ProjectileEntity projectile, Entity hitEntity )
	{
		base.OnProjectileHit( projectile, hitEntity );

		_ = DelayedExplosion( hitEntity );
	}
}
