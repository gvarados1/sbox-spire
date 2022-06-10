namespace Spire.Abilities;

public partial class ExplosiveArrowAttack : BasicArrowAttack
{
	// Configuration
	public override float Cooldown => 20f;
	public override string Name => "Explosive Arrow";
	public override string Description => "";
	public override string Icon => "ui/ability_icons/explosive_arrow.png";
	public override WeaponAbilityType Type => WeaponAbilityType.Ultimate;
	public override float Duration => 0.8f;

	protected override void OnProjectileHit( ProjectileEntity projectile, Entity hitEntity )
	{
		new ExplosionEntity
		{
			Position = projectile.Position,
			Radius = 256f,
			Damage = 20f,
			ForceScale = 1f,
		}.Explode( projectile );
	}
}
