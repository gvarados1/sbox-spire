namespace Spire.Abilities;

public partial class ConeArrowAttack : BaseMeleeAttackAbility
{
	// Configuration
	public override float Cooldown => 10f;
	public override string AbilityName => "Cone Attack";
	public override string AbilityDescription => "";
	public override string AbilityIcon => "ui/ability_icons/arrow_cone_attack.png";
	public override WeaponAbilityType Type => WeaponAbilityType.Special;

	public virtual float ProjectileSpeed => 800f;
	public virtual float ProjectileRadius => 10f;
	public virtual float ProjectileThrowStrength => 100f;
	public virtual float ConeSize => 15f;

	protected void CreateProjectile( float yawOffset )
	{

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

	public override void Execute()
	{
		base.Execute();

		if ( Host.IsClient )
			return;

		CreateProjectile( -ConeSize );
		CreateProjectile( 0f );
		CreateProjectile( ConeSize );
	}

	protected void OnProjectileHit( ProjectileEntity projectile, Entity hitEntity )
	{
		if ( !hitEntity.IsValid() ) return;

		hitEntity.TakeDamage( DamageInfo.FromBullet( hitEntity.Position, Vector3.Zero, 20f ) );
	}
}
