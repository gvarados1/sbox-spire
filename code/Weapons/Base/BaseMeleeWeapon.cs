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
	public virtual float AttackRadius => 20f;

	protected virtual async Task DelayedAttack()
	{
		await GameTask.DelaySeconds( AttackInflictDelay );

		InflictDamage();
	}

	protected virtual void InflictDamage()
	{
		var startPos = Owner.EyePosition;
		var direction = Owner.EyeRotation.Forward;
		var range = AttackRange;

		var trace = Trace.Ray( startPos, startPos + direction * range )
			.Ignore( Owner )
			.Radius( AttackRadius )
			.Run();

		DebugOverlay.TraceResultWithRealm( trace );
	}

	public override void AttackPrimary()
	{
		base.AttackPrimary();

		(Owner as AnimatedEntity)?.SetAnimParameter( "b_attack", true );

		_ = DelayedAttack();
	}
}
