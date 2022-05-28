namespace Spire;

public partial class BaseWeapon : BaseCarriable
{
	[Net, Predicted]
	public TimeSince TimeSincePrimaryAttack { get; set; }

	public virtual float PrimaryRate => 15.0f;

	protected virtual bool CanPrimaryAttack()
	{
		if ( !Owner.IsValid() || !Input.Down( InputButton.PrimaryAttack ) ) return false;

		var rate = PrimaryRate;
		if ( rate <= 0 ) return true;

		return TimeSincePrimaryAttack > (1 / rate);
	}

	public virtual void AttackPrimary()
	{
		TimeSincePrimaryAttack = 0;

		RpcPrimaryAttack();
	}

	[ClientRpc]
	protected void RpcPrimaryAttack()
	{
		ClientPrimaryAttack();
	}

	protected virtual void ClientPrimaryAttack()
	{

	}

	protected virtual void InflictDamage( float damage, TraceResult tr, DamageFlags addedFlags = DamageFlags.Slash )
	{
		if ( !tr.Hit )
			return;

		var damageInfo = new DamageInfo()
				.WithPosition( tr.EndPosition )
				.WithFlag( addedFlags )
				.WithForce( tr.Direction * tr.Distance )
				.UsingTraceResult( tr )
				.WithAttacker( Owner )
				.WithWeapon( this );

		damageInfo.Damage = damage;

		tr.Entity.TakeDamage( damageInfo );
	}

	public override void Simulate( Client player )
	{
		if ( !Owner.IsValid() )
			return;

		if ( CanPrimaryAttack() )
		{
			using ( LagCompensation() )
			{
				TimeSincePrimaryAttack = 0;
				AttackPrimary();
			}
		}
	}
}
