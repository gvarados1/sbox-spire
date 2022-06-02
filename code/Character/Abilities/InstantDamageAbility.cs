namespace Spire.Abilities;

public partial class InstantDamageAbility : PlayerAbility
{
	public override float Cooldown => 5f;
	public override string AbilityName => "Instant Damage";
	public override string AbilityDescription => "";
	public override string AbilityIcon => "ui/ability_icons/bomb.png";
	public override PlayerAbilityType Type => PlayerAbilityType.Standard;

	public virtual float ProjectileSpeed => 500f;
	public virtual float ProjectileRadius => 10f;
	public virtual float ProjectileThrowStrength => 100f;

	public override void Execute()
	{
		base.Execute();

		if ( Host.IsClient )
			return;

		var projectile = new BouncyProjectileEntity()
		{
			Bounciness = 0.7f,
			IgnoreEntity = Character,
			Attacker = Character,
			LifeTime = 2.5f,
			Gravity = 15f,
			ModelPath = "models/sbox_props/watermelon/watermelon.vmdl"
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
