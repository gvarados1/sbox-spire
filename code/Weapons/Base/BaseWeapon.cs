using Spire.Abilities;

namespace Spire;

public partial class BaseWeapon : BaseCarriable
{
	[Net, Predicted]
	public TimeSince TimeSinceLastAbility { get; set; } = 1f;
	public virtual float GlobalAbilityCooldown => 0.5f;

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

	protected WeaponAbility CreateAbility( Type type )
	{
		var ability = TypeLibrary.Create<WeaponAbility>( type );
		ability.Entity = this;

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

		var button = ability.Type.GetButton();

		if ( cl.Pawn is PlayerCharacter character )
		{
			if ( button == InputButton.PrimaryAttack && character.TimeSinceInteract < 1f )
			{
				ability.Simulate( cl );
				return;
			}
		}


		if ( Input.Down( button ) )
		{
			if ( ability.CanRun() )
				ability.Interact();
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
}
