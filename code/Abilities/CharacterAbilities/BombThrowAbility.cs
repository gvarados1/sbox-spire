namespace Spire.Abilities;

public partial class BombThrowAbility : PlayerAbility
{
	// Configuration
	public override string Identifier => "bomb_throw";
	public override PlayerAbilityType Type => PlayerAbilityType.Ultimate;

	public virtual float ProjectileSpeed => 500f;
	public virtual float ProjectileRadius => 20f;
	public virtual float ProjectileThrowStrength => 100f;

	protected override void PreRun()
	{
		base.PreRun();

		if ( Host.IsClient )
			return;

		var projectile = new BouncyProjectileEntity()
		{
			Bounciness = 0.7f,
			ReflectionScale = 0.5f,
			IgnoreEntity = Character,
			Attacker = Character,
			LifeTime = 2.5f,
			Gravity = 30f,
			ModelPath = "assets/projectiles/small_bomb.vmdl",
			TrailEffect = "particles/weapons/crossbow/crossbow_trail.vpcf"
		};

		var position = Character.EyePosition + Vector3.Down * 25f;
		var forward = Character.EyeRotation.Forward;
		var endPosition = position + Vector3.Up * 256f + forward * 200f;
		var trace = Trace.Ray( position, endPosition )
			.Ignore( Character )
			.Run();

		var direction = (trace.EndPosition - position).Normal;
		direction = direction.Normal;

		var velocity = (direction * ProjectileSpeed) + (Character.EyeRotation.Forward * ProjectileThrowStrength);
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
