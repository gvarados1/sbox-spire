namespace Spire.Abilities;

public partial class ExplosiveArrowAttack : BasicArrowAttack
{
	// Configuration
	public override float Cooldown => 20f;
	public override string AbilityName => "Explosive Arrow";
	public override string AbilityDescription => "";
	public override string AbilityIcon => "ui/ability_icons/explosive_arrow.png";
	public override WeaponAbilityType Type => WeaponAbilityType.Ultimate;

	public override float AbilityDuration => 0.8f;
	public override bool ManualProjectile => true;

	public float AttackDelay => 0.2f;
	public float AmountOfAttacks => 3;

	protected float StoredAmountOfAttacks = 0;

	protected override void PreAbilityExecute()
	{
		base.PreAbilityExecute();

		StoredAmountOfAttacks = 0;
	}

	protected override async Task AsyncExecute()
	{
		PreAbilityExecute();

		InProgress = true;

		while ( StoredAmountOfAttacks < AmountOfAttacks )
		{
			await GameTask.DelaySeconds( AttackDelay );
			StoredAmountOfAttacks++;

			CreateProjectile();
		}

		await GameTask.DelaySeconds( AbilityDuration );

		InProgress = false;
		NextUse = Cooldown;
		LastUsed = 0;

		PostAbilityExecute();
	}


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
