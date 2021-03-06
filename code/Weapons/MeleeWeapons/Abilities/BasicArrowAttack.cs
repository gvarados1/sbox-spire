namespace Spire.Abilities;

public partial class BasicArrowAttack : WeaponAbility
{
	// Configuration
	public override float Cooldown => 2f;
	public override string AbilityName => "Slash";
	public override string AbilityDescription => "";
	public override string AbilityIcon => "ui/ability_icons/arrow_attack.png";
	public override WeaponAbilityType Type => WeaponAbilityType.Attack;
	public override float PlayerSpeedMultiplier => 0.1f;
	public override float AbilityDuration => 0.5f;

	public virtual float ProjectileSpeed => 800f;
	public virtual float ProjectileRadius => 10f;
	public virtual float ProjectileThrowStrength => 100f;
	public virtual bool ManualProjectile => false;

	protected override void PreAbilityExecute()
	{
		base.PreAbilityExecute();

		Entity.PlaySound( "bow_draw" );
	}

	protected virtual void CreateProjectile( float yawOffset = 0f )
	{
		if ( Host.IsClient ) return;

		Entity.PlaySound( "rust_crossbow.shoot" );

		var projectile = new ProjectileEntity()
		{
			FaceDirection = true,
			IgnoreEntity = Weapon.Owner,
			Attacker = Weapon.Owner,
			LifeTime = 2.5f,
			Gravity = 0f,
			ModelPath = "assets/projectiles/rust_crossbow_bolt_fixed.vmdl"
		};

		var position = Weapon.Owner.EyePosition + Vector3.Down * 20f + Weapon.Owner.EyeRotation.Forward * 40f;

		Angles spread = new Angles( 0f, yawOffset, 0f );
		Rotation rotation = Rotation.From( spread ) * Weapon.Owner.EyeRotation;

		var forward = rotation.Forward;
		var endPosition = position + forward * 100000f;
		var trace = Trace.Ray( position, endPosition )
			.Ignore( Weapon.Owner )
			.Run();

		var direction = (trace.EndPosition - position).Normal;
		direction = direction.Normal;

		var velocity = (direction * ProjectileSpeed) + (forward * ProjectileThrowStrength);
		projectile.Initialize( position, velocity, ProjectileRadius, OnProjectileHit );
	}

	protected override void PostAbilityExecute()
	{
		base.PostAbilityExecute();

		if ( Host.IsClient )
			return;

		if ( !ManualProjectile )
			CreateProjectile();
	}

	protected virtual void OnProjectileHit( ProjectileEntity projectile, Entity hitEntity )
	{
		if ( !hitEntity.IsValid() ) return;

		hitEntity.TakeDamage( DamageInfo.FromBullet( hitEntity.Position, Vector3.Zero, 30f ) );
	}
}
