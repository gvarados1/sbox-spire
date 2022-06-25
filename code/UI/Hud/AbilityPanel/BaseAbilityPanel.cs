using Spire.Abilities;

namespace Spire.UI;

public partial class BaseAbilityPanel : Panel
{
	public Ability AbilityRef { get; set; }

	// @ref
	public InputHint InputHint { get; set; }

	public string CooldownString
	{
		get
		{
			if ( AbilityRef is null || AbilityRef.InProgress || AbilityRef.TimeSinceLastUse < 1f )
				return "";

			var nextUse = (float)AbilityRef.TimeUntilNextUse;
			if ( nextUse.Floor() <= 0 )
				return "";

			return $"{nextUse.Floor()}";
		}
	}

	public BaseAbilityPanel()
	{
		AddClass( "abilitypanel" );
	}

	protected PlayerCharacter GetCharacter()
	{
		return Local.Pawn as PlayerCharacter;
	}

	protected BaseWeapon GetWeapon()
	{
		return GetCharacter()?.ActiveChild as BaseWeapon;
	}

	protected virtual void UpdateAbility()
	{
		if ( AbilityRef is not null )
		{
			SetClass( "in-use", AbilityRef.TimeSinceLastUse < 1f || AbilityRef.InProgress );
			Style.SetBackgroundImage( AbilityRef.GetIcon() );
		}
		else
		{
			SetClass( "in-use", false );
			Style.SetBackgroundImage( "" );
		}

		SetClass( "no-ability", AbilityRef is null );
		SetClass( "on-cooldown", AbilityRef?.TimeUntilNextUse > 0f );

		InputHint.SetButton( GetButton() );
	}

	protected virtual InputButton GetButton()
	{
		return InputButton.PrimaryAttack;
	}

	protected virtual Ability GetAbility()
	{
		return null;
	}

	public virtual void Update()
	{
		var ability = GetAbility();

		if ( AbilityRef == ability )
		{
			UpdateAbility();
			return;
		}

		if ( ability is not null )
		{
			UpdateAbility();
			AbilityRef = ability;
		}
	}
}
