using Spire.Buffs;

namespace Spire;

public partial class BaseCharacter
{
	[Net, Local]
	public IList<Buff> Buffs { get; set; }

	public Buff AddBuff<T>() where T : Buff, new()
	{
		var buff = new T();
		buff.OnBegin();

		Buffs.Add( buff );

		return buff;
	}

	protected void SimulateBuffs( Client cl )
	{
		for ( var i = Buffs.Count - 1; i >= 0; i-- )
		{
			var buff = Buffs[i];

			if ( buff.NextTick <= 0 )
			{
				buff.OnTick( this );
				buff.NextTick = buff.TickInterval;
			}

			// @TODO: This is a bit of a hack, Untildestroy gets assigned to some random garbo value.
			if ( buff.UntilDestroy <= 0 && buff.SinceCreation >= 1f )
			{
				buff.OnDestroy( this );
				Buffs.RemoveAt( i );
			}
		}
	}
}
