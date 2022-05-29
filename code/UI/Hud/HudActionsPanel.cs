namespace Spire.UI;

[UseTemplate]
public partial class HudActionsPanel : Panel
{
	// @ref
	public WeaponAbilityPanel WeaponAttackAbility { get; set; }
	// @ref
	public WeaponAbilityPanel SpecialAttackAbility { get; set; }
	// @ref
	public WeaponAbilityPanel UltimateAttackAbility { get; set; }

	public HudActionsPanel()
	{
	}

	public override void Tick()
	{
		base.Tick();

		var player = Local.Pawn as PlayerCharacter;

		if ( !player.IsValid() )
			return;

		var weapon = player.ActiveChild as BaseWeapon;
		if ( !weapon.IsValid() )
			return;

		WeaponAttackAbility.Update( weapon );
		SpecialAttackAbility.Update( weapon );
		UltimateAttackAbility.Update( weapon );
	}
}
