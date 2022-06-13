namespace Spire.Abilities;

public partial class ExplosiveArrowAttack : BasicArrowAttack
{

	// Configuration
	public override string Identifier => "explosive_arrow_attack";
	public override WeaponAbilityType Type => WeaponAbilityType.Ultimate;

	protected override void OnProjectileHit( ProjectileEntity projectile, Entity hitEntity )
	{
		new ExplosionEntity
		{
			Position = projectile.Position,
			Radius = Data.AbilityEffectRadius,
			Damage = 20f,
			ForceScale = 1f,
		}.Explode( projectile );
	}
}
