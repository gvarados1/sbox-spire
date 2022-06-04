namespace Spire.Abilities;

public partial class BasicArrowAttack : BaseMeleeAttackAbility
{
	// Configuration
	public override float Cooldown => 1f;
	public override string AbilityName => "Slash";
	public override string AbilityDescription => "";
	public override string AbilityIcon => "ui/ability_icons/sword_slash.png";
	public override WeaponAbilityType Type => WeaponAbilityType.Attack;

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

		var velocity = (direction * ProjectileSpeed) + (Weapon.Owner.EyeRotation.Forward * ProjectileThrowStrength);
		projectile.Initialize( position, velocity, ProjectileRadius, OnProjectileHit );
	}

	protected void OnProjectileHit( ProjectileEntity projectile, Entity hitEntity )
	{
		hitEntity.TakeDamage( DamageInfo.FromBullet( hitEntity.Position, Vector3.Zero, 30f ) );
	}
}
