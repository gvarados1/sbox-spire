namespace Spire.UI;

[UseTemplate]
public partial class HudActionsPanel : Panel
{
	// PLAYER ABILITIES 

	// @ref
	public PlayerAbilityPanel MovementPlayerAbility { get; set; }
	// @ref
	public PlayerAbilityPanel FirstPlayerAbility { get; set; }
	// @ref
	public PlayerAbilityPanel SecondPlayerAbility { get; set; }
	// @ref
	public PlayerAbilityPanel UltimatePlayerAbility { get; set; }

	/// WEAPON ABILITIES

	// @ref
	public WeaponAbilityPanel WeaponAttackAbility { get; set; }
	// @ref
	public WeaponAbilityPanel SpecialAttackAbility { get; set; }
	// @ref
	public WeaponAbilityPanel UltimateAttackAbility { get; set; }

	public override void Tick()
	{
		base.Tick();

		var character = Local.Pawn as PlayerCharacter;
		if ( !character.IsValid() )
			return;

		MovementPlayerAbility.Update( character );
		FirstPlayerAbility.Update( character );
		SecondPlayerAbility.Update( character );
		UltimatePlayerAbility.Update( character );

		var weapon = character.ActiveChild as BaseWeapon;

		Log.Info( weapon );

		WeaponAttackAbility.Update( weapon );
		SpecialAttackAbility.Update( weapon );
		UltimateAttackAbility.Update( weapon );
	}
}
