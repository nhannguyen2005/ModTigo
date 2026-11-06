public class Teleport
{
	public static MyVector vTeleport = new MyVector();

	public int x;

	public int y;

	public int headId;

	public int type;

	public bool isMe;

	public int y2;

	public int id;

	public int dir;

	public int planet;

	public static Image[] maybay = new Image[5];

	public static Image hole;

	public bool isUp;

	public bool isDown;

	private bool createShip;

	public bool paintFire;

	private bool painHead;

	private int tPrepare;

	private int vy = 1;

	private int tFire;

	private int tDelayHole;

	private bool tHole;

	private bool isShock;

	public Teleport(int x, int y, int headId, int dir, int type, bool isMe, int planet)
	{
		this.x = x;
		this.y = 5;
		y2 = y;
		this.headId = headId;
		this.type = type;
		this.isMe = isMe;
		this.dir = dir;
		this.planet = planet;
		tPrepare = 0;
		int num = 0;
		while (num < 100)
		{
			num++;
			y2 += 12;
			if (TileMap.tileTypeAt(x, y2, 2))
			{
				if (y2 % 24 != 0)
				{
					y2 -= y2 % 24;
				}
				break;
			}
		}
		if (this.planet > 2)
		{
			y2 += 4;
		}
		this.y = y2;
		isDown = false;
		isUp = false;
		paintFire = false;
		painHead = false;
		createShip = false;
	}

	public void InstantComplete()
	{
		if (type == 0)
		{
			if (isMe)
			{
				Char.myCharz().isTeleport = false;
				Controller.isStopReadMessage = false;
				GameScr.lockTick = 0;
				Char.ischangingMap = true;
			}
			else
			{
				Char c = GameScr.findCharInMap(id);
				if (c != null)
				{
					c.isTeleport = false;
					GameScr.vCharInMap.removeElement(c);
				}
			}
		}
		else if (type == 1)
		{
			if (isMe)
			{
				Char.myCharz().cx = x;
				Char.myCharz().cy = y2;
				Char.myCharz().statusMe = 1;
				Char.myCharz().isTeleport = false;
				GameScr.cmtoX = x - GameScr.gW2;
				GameScr.cmtoY = y2 - GameScr.gH23 - 1;
				GameScr.info1.isUpdate = true;
			}
			else
			{
				Char c2 = GameScr.findCharInMap(id);
				if (c2 != null)
				{
					c2.cx = x;
					c2.cy = y2;
					c2.statusMe = 1;
					c2.isTeleport = false;
				}
			}
		}
		vTeleport.removeElement(this);
	}

	public static void addTeleport(Teleport p)
	{
		if (p != null)
		{
			p.InstantComplete();
		}
	}

	public void paintHole(mGraphics g)
	{
	}

	public void paint(mGraphics g)
	{
	}

	public void update()
	{
		InstantComplete();
	}
}
