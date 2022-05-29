using System.Threading.Tasks;

namespace Spire;

public partial class BaseMeleeWeapon : BaseWeapon
{
	/// <summary>
	/// How often we can swing
	/// </summary>
	public override float PrimaryRate => 1f;

	/// <summary>
	/// How long until we try to inflict damage from the weapon after an attack.
	/// </summary>
	public virtual float AttackInflictDelay => 0.35f;

	public virtual float AttackRange => 90f;
	public virtual float BaseDamage => 30f;
	public virtual int MaxConeAngle => 75;

	protected virtual async Task DelayedAttack()
	{
		await GameTask.DelaySeconds( AttackInflictDelay );

		ExecuteConeAttack();
	}

	protected virtual float GetDamage()
	{
		return BaseDamage;
	}

	// Extract to a util later?
	public static bool IsPointInsideCone( Vector3 point, Vector3 coneOrigin, Vector3 coneDirection, int maxAngle, int maxDistance )
	{
		var distanceToConeOrigin = (point - coneOrigin).Length;
		if ( distanceToConeOrigin < maxDistance )
		{
			var pointDirection = point - coneOrigin;
			var angle = Vector3.GetAngle( coneDirection, pointDirection );

			if ( angle < maxAngle )
				return true;
		}
		return false;
	}

	protected void TestInCone( Entity ent )
	{
		var bIsInCone = IsPointInsideCone(
			ent.EyePosition,
			Owner.EyePosition,
			Owner.EyeRotation.Forward,
			MaxConeAngle,
			AttackRange.CeilToInt()
		);

		if ( bIsInCone )
		{
			RunDamageTrace( ent );
		}
	}

	protected virtual void ExecuteConeAttack()
	{
		var ents = FindInSphere( Position, AttackRange );

		ents.Where( x => x is BaseCharacter && x != Owner )
			.ToList()
			.ForEach( x => TestInCone( x ) );
	}

	protected virtual void RunDamageTrace( Entity entity )
	{
		// Trace for visual effects
		var tr = Trace.Ray( Position, entity.EyePosition ).Ignore( Owner ).UseHitboxes( true ).Run();

		// Damage builder
		var damageInfo = new DamageInfo()
				.WithPosition( tr.EndPosition )
				.WithFlag( DamageFlags.Bullet )
				.WithForce( Owner.EyeRotation.Forward * AttackForce )
				.UsingTraceResult( tr )
				.WithAttacker( Owner )
				.WithWeapon( this );

		damageInfo.Damage = GetDamage();

		// Handle surface impacts
		tr.Surface.DoBulletImpact( tr );

		// Finally, inflict damage on our target
		entity.TakeDamage( damageInfo );
	}

	public override void AttackPrimary()
	{
		base.AttackPrimary();

		(Owner as AnimatedEntity)?.SetAnimParameter( "b_attack", true );

		_ = DelayedAttack();
	}
}
