namespace Spire.Abilities;

public partial class ExplosiveArrowAttack : BaseMeleeAttackAbility
{
	// Configuration
	public override float Cooldown => 2f;
	public override string AbilityName => "Slash";
	public override string AbilityDescription => "";
	public override string AbilityIcon => "ui/ability_icons/explosive_arrow.png";
	public override WeaponAbilityType Type => WeaponAbilityType.Ultimate;

	public virtual float ProjectileSpeed => 800f;
	public virtual float ProjectileRadius => 10f;
	public virtual float ProjectileThrowStrength => 100f;

	public override void Execute()
	{
		base.Execute();

		if ( Host.IsClient )
			return;

		var projectile = new ProjectileEntity()
		{
			FaceDirection = true,
			IgnoreEntity = Weapon.Owner,
			Attacker = Weapon.Owner,
			LifeTime = 2.5f,
			Gravity = 0f,
			ModelPath = "weapons/rust_crossbow/rust_crossbow_bolt.vmdl"
		};

		var position = Weapon.Owner.EyePosition + Vector3.Down * 25f;
		var forward = Weapon.Owner.EyeRotation.Forward;
		var endPosition = position + forward * 100000f;
		var trace = Trace.Ray( position, endPosition )
			.Ignore( Weapon.Owner )
			.Run();

		var direction = (trace.EndPosition - position).Normal;
		direction = direction.Normal;

		var velocity = (direction * ProjectileSpeed) + (forward * ProjectileThrowStrength);
		projectile.Initialize( position, velocity, ProjectileRadius, OnProjectileHit );
	}

	protected void OnProjectileHit( ProjectileEntity projectile, Entity hitEntity )
	{
		new ExplosionEntity
		{
			Position = projectile.Position,
			Radius = 256f,
			Damage = 50f,
			ForceScale = 1f,
		}.Explode( projectile );
	}
}
