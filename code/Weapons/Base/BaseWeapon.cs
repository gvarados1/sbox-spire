using Spire.Abilities;

namespace Spire;

public partial class BaseWeapon : BaseCarriable
{
	[Net, Predicted]
	public TimeSince TimeSinceLastAbility { get; set; } = 0f;

	public virtual List<Type> Abilities => new();

	public virtual Type AttackAbilityType => null;
	public virtual Type SpecialAbilityType => null;
	public virtual Type UltimateAbilityType => null;

	[Net, Predicted]
	public WeaponAbility AttackAbility { get; set; }
	[Net, Predicted]
	public WeaponAbility SpecialAbility { get; set; }
	[Net, Predicted]
	public WeaponAbility UltimateAbility { get; set; }

	public virtual float TimeUntilRelaxed { get; set; } = 5f;
	public virtual HoldType RelaxedHoldType { get; set; } = HoldType.None;

	protected WeaponAbility CreateAbility( Type type )
	{
		var ability = TypeLibrary.Create<WeaponAbility>( type );
		ability.Entity = this;

		Log.Info( $"Creating ability of type: {type}" );

		return ability;
	}

	public IEnumerable<WeaponAbility> GetAbilities()
	{
		yield return AttackAbility;
		yield return SpecialAbility;
		yield return UltimateAbility;
	}

	public override void Spawn()
	{
		base.Spawn();

		CreateAbilities();
	}

	public override void ClientSpawn()
	{
		base.ClientSpawn();

		CreateAbilities();
	}

	protected void CreateAbilities()
	{
		if ( AttackAbilityType is not null )
			AttackAbility = CreateAbility( AttackAbilityType );

		if ( SpecialAbilityType is not null )
			SpecialAbility = CreateAbility( SpecialAbilityType );

		if ( UltimateAbilityType is not null )
			UltimateAbility = CreateAbility( UltimateAbilityType );
	}

	protected void SimulateAbility( Client cl, WeaponAbility ability )
	{
		if ( !ability.IsValid() )
			return;

		if ( Input.Down( ability.Type.GetButton() ) )
		{
			if ( ability.CanExecute() )
			{
				ability.TryExecute();
				TimeSinceLastAbility = 0;
			}
		}

		ability.Simulate( cl );
	}

	public override void Simulate( Client cl )
	{
		if ( !Owner.IsValid() )
			return;

		SimulateAbility( cl, AttackAbility );
		SimulateAbility( cl, SpecialAbility );
		SimulateAbility( cl, UltimateAbility );
	}

	public override void SimulateAnimator( PawnAnimator anim )
	{
		base.SimulateAnimator( anim );

		if ( TimeSinceLastAbility > TimeUntilRelaxed )
		{
			anim.SetAnimParameter( "holdtype", (int)RelaxedHoldType );
		}
	}
}
