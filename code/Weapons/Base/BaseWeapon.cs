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
