using System.Collections.Generic;

namespace DungeonRun
{
	public static class RoomData_SkullIsland_Key
	{
		//data is added to this list
		public static List<RoomXmlData> Data = new List<RoomXmlData>();
		//data uses this reference
		public static RoomXmlData dataRef;
		//data is added in the constructor below
		static RoomData_SkullIsland_Key()
		{

			#region SkullIsland_KeyRoom

			dataRef = new RoomXmlData();
			dataRef.type = RoomID.SkullIsland_KeyRoom;
			dataRef.windDirection = Direction.None;
			dataRef.windFrequency = 2;
			dataRef.windIntensity = 0;
			{IndObjXmlData obj = new IndObjXmlData(); obj.type = IndestructibleType.Dungeon_BlockDark; obj.direction = Direction.Down; obj.posX = 136; obj.posY = 56; dataRef.inds.Add(obj);}
			{IndObjXmlData obj = new IndObjXmlData(); obj.type = IndestructibleType.Dungeon_BlockDark; obj.direction = Direction.Down; obj.posX = 152; obj.posY = 56; dataRef.inds.Add(obj);}
			{IndObjXmlData obj = new IndObjXmlData(); obj.type = IndestructibleType.Dungeon_BlockDark; obj.direction = Direction.Down; obj.posX = 168; obj.posY = 56; dataRef.inds.Add(obj);}
			{IndObjXmlData obj = new IndObjXmlData(); obj.type = IndestructibleType.Dungeon_BlockDark; obj.direction = Direction.Down; obj.posX = 120; obj.posY = 88; dataRef.inds.Add(obj);}
			{IndObjXmlData obj = new IndObjXmlData(); obj.type = IndestructibleType.Dungeon_BlockDark; obj.direction = Direction.Down; obj.posX = 120; obj.posY = 104; dataRef.inds.Add(obj);}
			{IndObjXmlData obj = new IndObjXmlData(); obj.type = IndestructibleType.Dungeon_BlockDark; obj.direction = Direction.Down; obj.posX = 136; obj.posY = 120; dataRef.inds.Add(obj);}
			{IndObjXmlData obj = new IndObjXmlData(); obj.type = IndestructibleType.Dungeon_BlockDark; obj.direction = Direction.Down; obj.posX = 152; obj.posY = 120; dataRef.inds.Add(obj);}
			{IndObjXmlData obj = new IndObjXmlData(); obj.type = IndestructibleType.Dungeon_BlockDark; obj.direction = Direction.Down; obj.posX = 184; obj.posY = 56; dataRef.inds.Add(obj);}
			{IndObjXmlData obj = new IndObjXmlData(); obj.type = IndestructibleType.Dungeon_BlockDark; obj.direction = Direction.Down; obj.posX = 184; obj.posY = 88; dataRef.inds.Add(obj);}
			{IndObjXmlData obj = new IndObjXmlData(); obj.type = IndestructibleType.Dungeon_BlockDark; obj.direction = Direction.Down; obj.posX = 184; obj.posY = 104; dataRef.inds.Add(obj);}
			{IndObjXmlData obj = new IndObjXmlData(); obj.type = IndestructibleType.Dungeon_BlockDark; obj.direction = Direction.Down; obj.posX = 104; obj.posY = 56; dataRef.inds.Add(obj);}
			{IndObjXmlData obj = new IndObjXmlData(); obj.type = IndestructibleType.Dungeon_BlockDark; obj.direction = Direction.Down; obj.posX = 88; obj.posY = 56; dataRef.inds.Add(obj);}
			{IndObjXmlData obj = new IndObjXmlData(); obj.type = IndestructibleType.Dungeon_BlockDark; obj.direction = Direction.Down; obj.posX = 72; obj.posY = 56; dataRef.inds.Add(obj);}
			{IndObjXmlData obj = new IndObjXmlData(); obj.type = IndestructibleType.Dungeon_BlockDark; obj.direction = Direction.Down; obj.posX = 56; obj.posY = 56; dataRef.inds.Add(obj);}
			{IndObjXmlData obj = new IndObjXmlData(); obj.type = IndestructibleType.Dungeon_BlockDark; obj.direction = Direction.Down; obj.posX = 40; obj.posY = 56; dataRef.inds.Add(obj);}
			{IndObjXmlData obj = new IndObjXmlData(); obj.type = IndestructibleType.Dungeon_BlockDark; obj.direction = Direction.Down; obj.posX = 24; obj.posY = 56; dataRef.inds.Add(obj);}
			{IndObjXmlData obj = new IndObjXmlData(); obj.type = IndestructibleType.Dungeon_BlockDark; obj.direction = Direction.Down; obj.posX = 200; obj.posY = 56; dataRef.inds.Add(obj);}
			{IndObjXmlData obj = new IndObjXmlData(); obj.type = IndestructibleType.Dungeon_BlockDark; obj.direction = Direction.Down; obj.posX = 216; obj.posY = 56; dataRef.inds.Add(obj);}
			{IndObjXmlData obj = new IndObjXmlData(); obj.type = IndestructibleType.Dungeon_BlockDark; obj.direction = Direction.Down; obj.posX = 232; obj.posY = 56; dataRef.inds.Add(obj);}
			{IndObjXmlData obj = new IndObjXmlData(); obj.type = IndestructibleType.Dungeon_BlockDark; obj.direction = Direction.Down; obj.posX = 120; obj.posY = 56; dataRef.inds.Add(obj);}
			{IndObjXmlData obj = new IndObjXmlData(); obj.type = IndestructibleType.Dungeon_BlockDark; obj.direction = Direction.Down; obj.posX = 280; obj.posY = 40; dataRef.inds.Add(obj);}
			{IndObjXmlData obj = new IndObjXmlData(); obj.type = IndestructibleType.Dungeon_BlockDark; obj.direction = Direction.Down; obj.posX = 280; obj.posY = 56; dataRef.inds.Add(obj);}
			{IndObjXmlData obj = new IndObjXmlData(); obj.type = IndestructibleType.Dungeon_BlockDark; obj.direction = Direction.Down; obj.posX = 40; obj.posY = 24; dataRef.inds.Add(obj);}
			{IndObjXmlData obj = new IndObjXmlData(); obj.type = IndestructibleType.Dungeon_BlockDark; obj.direction = Direction.Down; obj.posX = 56; obj.posY = 120; dataRef.inds.Add(obj);}
			{IndObjXmlData obj = new IndObjXmlData(); obj.type = IndestructibleType.Dungeon_BlockDark; obj.direction = Direction.Down; obj.posX = 24; obj.posY = 40; dataRef.inds.Add(obj);}
			{IndObjXmlData obj = new IndObjXmlData(); obj.type = IndestructibleType.Dungeon_BlockDark; obj.direction = Direction.Down; obj.posX = 280; obj.posY = 72; dataRef.inds.Add(obj);}
			{IndObjXmlData obj = new IndObjXmlData(); obj.type = IndestructibleType.Dungeon_BlockDark; obj.direction = Direction.Down; obj.posX = 120; obj.posY = 120; dataRef.inds.Add(obj);}
			{IndObjXmlData obj = new IndObjXmlData(); obj.type = IndestructibleType.Dungeon_BlockDark; obj.direction = Direction.Down; obj.posX = 72; obj.posY = 24; dataRef.inds.Add(obj);}
			{IndObjXmlData obj = new IndObjXmlData(); obj.type = IndestructibleType.Dungeon_BlockDark; obj.direction = Direction.Down; obj.posX = 88; obj.posY = 24; dataRef.inds.Add(obj);}
			{IndObjXmlData obj = new IndObjXmlData(); obj.type = IndestructibleType.Dungeon_BlockDark; obj.direction = Direction.Down; obj.posX = 104; obj.posY = 24; dataRef.inds.Add(obj);}
			{IndObjXmlData obj = new IndObjXmlData(); obj.type = IndestructibleType.Dungeon_BlockDark; obj.direction = Direction.Down; obj.posX = 120; obj.posY = 24; dataRef.inds.Add(obj);}
			{IndObjXmlData obj = new IndObjXmlData(); obj.type = IndestructibleType.Dungeon_BlockDark; obj.direction = Direction.Down; obj.posX = 136; obj.posY = 24; dataRef.inds.Add(obj);}
			{IndObjXmlData obj = new IndObjXmlData(); obj.type = IndestructibleType.Dungeon_BlockDark; obj.direction = Direction.Down; obj.posX = 152; obj.posY = 24; dataRef.inds.Add(obj);}
			{IndObjXmlData obj = new IndObjXmlData(); obj.type = IndestructibleType.Dungeon_BlockDark; obj.direction = Direction.Down; obj.posX = 168; obj.posY = 24; dataRef.inds.Add(obj);}
			{IndObjXmlData obj = new IndObjXmlData(); obj.type = IndestructibleType.Dungeon_BlockDark; obj.direction = Direction.Down; obj.posX = 184; obj.posY = 24; dataRef.inds.Add(obj);}
			{IndObjXmlData obj = new IndObjXmlData(); obj.type = IndestructibleType.Dungeon_BlockDark; obj.direction = Direction.Down; obj.posX = 200; obj.posY = 24; dataRef.inds.Add(obj);}
			{IndObjXmlData obj = new IndObjXmlData(); obj.type = IndestructibleType.Dungeon_BlockDark; obj.direction = Direction.Down; obj.posX = 216; obj.posY = 24; dataRef.inds.Add(obj);}
			{IndObjXmlData obj = new IndObjXmlData(); obj.type = IndestructibleType.Dungeon_BlockDark; obj.direction = Direction.Down; obj.posX = 232; obj.posY = 24; dataRef.inds.Add(obj);}
			{IndObjXmlData obj = new IndObjXmlData(); obj.type = IndestructibleType.Dungeon_BlockDark; obj.direction = Direction.Down; obj.posX = 248; obj.posY = 24; dataRef.inds.Add(obj);}
			{IndObjXmlData obj = new IndObjXmlData(); obj.type = IndestructibleType.Dungeon_BlockDark; obj.direction = Direction.Down; obj.posX = 264; obj.posY = 24; dataRef.inds.Add(obj);}
			{IndObjXmlData obj = new IndObjXmlData(); obj.type = IndestructibleType.Dungeon_BlockDark; obj.direction = Direction.Down; obj.posX = 280; obj.posY = 88; dataRef.inds.Add(obj);}
			{IndObjXmlData obj = new IndObjXmlData(); obj.type = IndestructibleType.Dungeon_BlockDark; obj.direction = Direction.Down; obj.posX = 280; obj.posY = 104; dataRef.inds.Add(obj);}
			{IndObjXmlData obj = new IndObjXmlData(); obj.type = IndestructibleType.Dungeon_BlockDark; obj.direction = Direction.Down; obj.posX = 280; obj.posY = 120; dataRef.inds.Add(obj);}
			{IndObjXmlData obj = new IndObjXmlData(); obj.type = IndestructibleType.Dungeon_BlockDark; obj.direction = Direction.Down; obj.posX = 280; obj.posY = 136; dataRef.inds.Add(obj);}
			{IndObjXmlData obj = new IndObjXmlData(); obj.type = IndestructibleType.Dungeon_BlockDark; obj.direction = Direction.Down; obj.posX = 248; obj.posY = 56; dataRef.inds.Add(obj);}
			{IndObjXmlData obj = new IndObjXmlData(); obj.type = IndestructibleType.Dungeon_BlockDark; obj.direction = Direction.Down; obj.posX = 24; obj.posY = 72; dataRef.inds.Add(obj);}
			{IndObjXmlData obj = new IndObjXmlData(); obj.type = IndestructibleType.Dungeon_BlockDark; obj.direction = Direction.Down; obj.posX = 24; obj.posY = 88; dataRef.inds.Add(obj);}
			{IndObjXmlData obj = new IndObjXmlData(); obj.type = IndestructibleType.Dungeon_BlockDark; obj.direction = Direction.Down; obj.posX = 24; obj.posY = 104; dataRef.inds.Add(obj);}
			{IndObjXmlData obj = new IndObjXmlData(); obj.type = IndestructibleType.Dungeon_BlockDark; obj.direction = Direction.Down; obj.posX = 24; obj.posY = 120; dataRef.inds.Add(obj);}
			{IndObjXmlData obj = new IndObjXmlData(); obj.type = IndestructibleType.Dungeon_BlockDark; obj.direction = Direction.Down; obj.posX = 24; obj.posY = 136; dataRef.inds.Add(obj);}
			{IndObjXmlData obj = new IndObjXmlData(); obj.type = IndestructibleType.Dungeon_BlockDark; obj.direction = Direction.Down; obj.posX = 40; obj.posY = 152; dataRef.inds.Add(obj);}
			{IndObjXmlData obj = new IndObjXmlData(); obj.type = IndestructibleType.Dungeon_BlockDark; obj.direction = Direction.Down; obj.posX = 56; obj.posY = 152; dataRef.inds.Add(obj);}
			{IndObjXmlData obj = new IndObjXmlData(); obj.type = IndestructibleType.Dungeon_BlockDark; obj.direction = Direction.Down; obj.posX = 72; obj.posY = 152; dataRef.inds.Add(obj);}
			{IndObjXmlData obj = new IndObjXmlData(); obj.type = IndestructibleType.Dungeon_BlockDark; obj.direction = Direction.Down; obj.posX = 88; obj.posY = 152; dataRef.inds.Add(obj);}
			{IndObjXmlData obj = new IndObjXmlData(); obj.type = IndestructibleType.Dungeon_BlockDark; obj.direction = Direction.Down; obj.posX = 104; obj.posY = 152; dataRef.inds.Add(obj);}
			{IndObjXmlData obj = new IndObjXmlData(); obj.type = IndestructibleType.Dungeon_BlockDark; obj.direction = Direction.Down; obj.posX = 120; obj.posY = 152; dataRef.inds.Add(obj);}
			{IndObjXmlData obj = new IndObjXmlData(); obj.type = IndestructibleType.Dungeon_BlockDark; obj.direction = Direction.Down; obj.posX = 136; obj.posY = 152; dataRef.inds.Add(obj);}
			{IndObjXmlData obj = new IndObjXmlData(); obj.type = IndestructibleType.Dungeon_BlockDark; obj.direction = Direction.Down; obj.posX = 152; obj.posY = 152; dataRef.inds.Add(obj);}
			{IndObjXmlData obj = new IndObjXmlData(); obj.type = IndestructibleType.Dungeon_BlockDark; obj.direction = Direction.Down; obj.posX = 168; obj.posY = 152; dataRef.inds.Add(obj);}
			{IndObjXmlData obj = new IndObjXmlData(); obj.type = IndestructibleType.Dungeon_BlockDark; obj.direction = Direction.Down; obj.posX = 184; obj.posY = 152; dataRef.inds.Add(obj);}
			{IndObjXmlData obj = new IndObjXmlData(); obj.type = IndestructibleType.Dungeon_BlockDark; obj.direction = Direction.Down; obj.posX = 216; obj.posY = 152; dataRef.inds.Add(obj);}
			{IndObjXmlData obj = new IndObjXmlData(); obj.type = IndestructibleType.Dungeon_BlockDark; obj.direction = Direction.Down; obj.posX = 232; obj.posY = 152; dataRef.inds.Add(obj);}
			{IndObjXmlData obj = new IndObjXmlData(); obj.type = IndestructibleType.Dungeon_BlockDark; obj.direction = Direction.Down; obj.posX = 248; obj.posY = 152; dataRef.inds.Add(obj);}
			{IndObjXmlData obj = new IndObjXmlData(); obj.type = IndestructibleType.Dungeon_BlockDark; obj.direction = Direction.Down; obj.posX = 264; obj.posY = 152; dataRef.inds.Add(obj);}
			{IndObjXmlData obj = new IndObjXmlData(); obj.type = IndestructibleType.Dungeon_BlockDark; obj.direction = Direction.Down; obj.posX = 248; obj.posY = 72; dataRef.inds.Add(obj);}
			{IndObjXmlData obj = new IndObjXmlData(); obj.type = IndestructibleType.Dungeon_BlockDark; obj.direction = Direction.Down; obj.posX = 248; obj.posY = 88; dataRef.inds.Add(obj);}
			{IndObjXmlData obj = new IndObjXmlData(); obj.type = IndestructibleType.Dungeon_BlockDark; obj.direction = Direction.Down; obj.posX = 248; obj.posY = 104; dataRef.inds.Add(obj);}
			{IndObjXmlData obj = new IndObjXmlData(); obj.type = IndestructibleType.Dungeon_BlockDark; obj.direction = Direction.Down; obj.posX = 248; obj.posY = 120; dataRef.inds.Add(obj);}
			{IndObjXmlData obj = new IndObjXmlData(); obj.type = IndestructibleType.Dungeon_BlockDark; obj.direction = Direction.Down; obj.posX = 104; obj.posY = 88; dataRef.inds.Add(obj);}
			{IndObjXmlData obj = new IndObjXmlData(); obj.type = IndestructibleType.Dungeon_BlockDark; obj.direction = Direction.Down; obj.posX = 184; obj.posY = 120; dataRef.inds.Add(obj);}
			{IndObjXmlData obj = new IndObjXmlData(); obj.type = IndestructibleType.Dungeon_BlockDark; obj.direction = Direction.Down; obj.posX = 168; obj.posY = 120; dataRef.inds.Add(obj);}
			{IndObjXmlData obj = new IndObjXmlData(); obj.type = IndestructibleType.Dungeon_BlockDark; obj.direction = Direction.Down; obj.posX = 88; obj.posY = 88; dataRef.inds.Add(obj);}
			{IndObjXmlData obj = new IndObjXmlData(); obj.type = IndestructibleType.Dungeon_BlockDark; obj.direction = Direction.Down; obj.posX = 56; obj.posY = 104; dataRef.inds.Add(obj);}
			{IndObjXmlData obj = new IndObjXmlData(); obj.type = IndestructibleType.Dungeon_BlockDark; obj.direction = Direction.Down; obj.posX = 56; obj.posY = 88; dataRef.inds.Add(obj);}
			{IndObjXmlData obj = new IndObjXmlData(); obj.type = IndestructibleType.Dungeon_BlockDark; obj.direction = Direction.Down; obj.posX = 72; obj.posY = 120; dataRef.inds.Add(obj);}
			{IndObjXmlData obj = new IndObjXmlData(); obj.type = IndestructibleType.Dungeon_BlockDark; obj.direction = Direction.Down; obj.posX = 88; obj.posY = 120; dataRef.inds.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.FloorStain; obj.direction = Direction.Down; obj.posX = 5; obj.posY = 6; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.FloorStain; obj.direction = Direction.Down; obj.posX = 24; obj.posY = 22; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.FloorStain; obj.direction = Direction.Down; obj.posX = 22; obj.posY = 120; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.FloorStain; obj.direction = Direction.Down; obj.posX = 20; obj.posY = 171; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.FloorStain; obj.direction = Direction.Down; obj.posX = 37; obj.posY = 41; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.FloorStain; obj.direction = Direction.Down; obj.posX = 72; obj.posY = 87; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.FloorStain; obj.direction = Direction.Down; obj.posX = 84; obj.posY = 20; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.FloorStain; obj.direction = Direction.Down; obj.posX = 100; obj.posY = 153; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.FloorStain; obj.direction = Direction.Down; obj.posX = 132; obj.posY = 43; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.FloorStain; obj.direction = Direction.Down; obj.posX = 155; obj.posY = 139; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.FloorStain; obj.direction = Direction.Down; obj.posX = 166; obj.posY = 101; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.FloorStain; obj.direction = Direction.Down; obj.posX = 185; obj.posY = 86; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.FloorStain; obj.direction = Direction.Down; obj.posX = 183; obj.posY = 102; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.FloorStain; obj.direction = Direction.Down; obj.posX = 228; obj.posY = 151; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.FloorStain; obj.direction = Direction.Down; obj.posX = 249; obj.posY = 5; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.FloorStain; obj.direction = Direction.Down; obj.posX = 246; obj.posY = 54; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.FloorStain; obj.direction = Direction.Down; obj.posX = 247; obj.posY = 72; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.FloorStain; obj.direction = Direction.Down; obj.posX = 245; obj.posY = 88; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.FloorStain; obj.direction = Direction.Down; obj.posX = 264; obj.posY = 20; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.FloorStain; obj.direction = Direction.Down; obj.posX = 263; obj.posY = 37; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.FloorStain; obj.direction = Direction.Down; obj.posX = 261; obj.posY = 120; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.FloorStain; obj.direction = Direction.Down; obj.posX = 298; obj.posY = 7; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.FloorStain; obj.direction = Direction.Down; obj.posX = 298; obj.posY = 101; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.FloorStain; obj.direction = Direction.Down; obj.posX = 295; obj.posY = 133; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.FloorStain; obj.direction = Direction.Down; obj.posX = 295; obj.posY = 164; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.FloorStain; obj.direction = Direction.Down; obj.posX = -10; obj.posY = 90; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.ChestKey; obj.direction = Direction.Down; obj.posX = 152; obj.posY = 88; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.Dungeon_SpawnMob; obj.direction = Direction.Down; obj.posX = 152; obj.posY = 40; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.Dungeon_SwitchBlockUp; obj.direction = Direction.Down; obj.posX = 264; obj.posY = 120; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.Dungeon_SpawnMob; obj.direction = Direction.Down; obj.posX = 264; obj.posY = 88; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.Dungeon_SpawnMob; obj.direction = Direction.Down; obj.posX = 152; obj.posY = 136; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.Dungeon_SpawnMob; obj.direction = Direction.Down; obj.posX = 40; obj.posY = 120; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.Lava_Pit; obj.direction = Direction.Down; obj.posX = 200; obj.posY = 120; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.Dungeon_BlockLight; obj.direction = Direction.Down; obj.posX = 152; obj.posY = 104; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.Dungeon_SwitchBlockUp; obj.direction = Direction.Down; obj.posX = 56; obj.posY = 24; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.Dungeon_SwitchBlockDown; obj.direction = Direction.Down; obj.posX = 120; obj.posY = 40; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.Dungeon_SwitchBlockDown; obj.direction = Direction.Down; obj.posX = 264; obj.posY = 56; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.Lava_Pit; obj.direction = Direction.Down; obj.posX = 200; obj.posY = 88; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.Lava_Pit; obj.direction = Direction.Down; obj.posX = 216; obj.posY = 88; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.Lava_Pit; obj.direction = Direction.Down; obj.posX = 232; obj.posY = 88; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.Lava_Pit; obj.direction = Direction.Down; obj.posX = 200; obj.posY = 104; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.Lava_Pit; obj.direction = Direction.Down; obj.posX = 216; obj.posY = 104; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.Lava_Pit; obj.direction = Direction.Down; obj.posX = 232; obj.posY = 104; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.Lava_Pit; obj.direction = Direction.Down; obj.posX = 232; obj.posY = 120; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.Lava_Pit; obj.direction = Direction.Down; obj.posX = 216; obj.posY = 120; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.FloorBlood; obj.direction = Direction.Down; obj.posX = 104; obj.posY = 40; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.Dungeon_SwitchBlockUp; obj.direction = Direction.Down; obj.posX = 184; obj.posY = 40; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.Dungeon_SwitchBlockUp; obj.direction = Direction.Down; obj.posX = 120; obj.posY = 136; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.Dungeon_SwitchBlockDown; obj.direction = Direction.Down; obj.posX = 184; obj.posY = 136; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.Dungeon_SwitchBlockDown; obj.direction = Direction.Down; obj.posX = 40; obj.posY = 88; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.Dungeon_SwitchBlockDown; obj.direction = Direction.Down; obj.posX = 104; obj.posY = 120; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.Dungeon_SwitchBlockUp; obj.direction = Direction.Down; obj.posX = 72; obj.posY = 88; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.Dungeon_SwitchBlockUp; obj.direction = Direction.Down; obj.posX = 120; obj.posY = 72; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.Dungeon_SwitchBlockBtn; obj.direction = Direction.Down; obj.posX = 232; obj.posY = 72; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.Dungeon_BlockSpike; obj.direction = Direction.Right; obj.posX = 136; obj.posY = 72; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.Lava_PitTeethTop; obj.direction = Direction.Down; obj.posX = 200; obj.posY = 88; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.Lava_PitTeethTop; obj.direction = Direction.Down; obj.posX = 216; obj.posY = 88; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.Lava_PitTeethTop; obj.direction = Direction.Down; obj.posX = 232; obj.posY = 88; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.Lava_PitTeethBottom; obj.direction = Direction.Down; obj.posX = 200; obj.posY = 120; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.Lava_PitTeethBottom; obj.direction = Direction.Down; obj.posX = 216; obj.posY = 120; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.Lava_PitTeethBottom; obj.direction = Direction.Down; obj.posX = 232; obj.posY = 120; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.TorchUnlit; obj.direction = Direction.Down; obj.posX = 280; obj.posY = 24; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.TorchUnlit; obj.direction = Direction.Down; obj.posX = 280; obj.posY = 152; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.TorchUnlit; obj.direction = Direction.Down; obj.posX = 24; obj.posY = 152; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.TorchUnlit; obj.direction = Direction.Down; obj.posX = 24; obj.posY = 24; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.FloorBlood; obj.direction = Direction.Down; obj.posX = 40; obj.posY = 72; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.FloorBlood; obj.direction = Direction.Down; obj.posX = 88; obj.posY = 136; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.FloorSkeleton; obj.direction = Direction.Down; obj.posX = 248; obj.posY = 40; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.FloorSkeleton; obj.direction = Direction.Down; obj.posX = 216; obj.posY = 136; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.FloorSkeleton; obj.direction = Direction.Down; obj.posX = 168; obj.posY = 88; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.FloorSkeleton; obj.direction = Direction.Down; obj.posX = 104; obj.posY = 104; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.FloorSkeleton; obj.direction = Direction.Down; obj.posX = 40; obj.posY = 40; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.FloorSkeleton; obj.direction = Direction.Down; obj.posX = 40; obj.posY = 136; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.Debris; obj.direction = Direction.Down; obj.posX = 152; obj.posY = 72; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.Debris; obj.direction = Direction.Down; obj.posX = 72; obj.posY = 104; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.Debris; obj.direction = Direction.Down; obj.posX = 216; obj.posY = 72; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.Dungeon_Pot; obj.direction = Direction.Down; obj.posX = 136; obj.posY = 104; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.Dungeon_Pot; obj.direction = Direction.Down; obj.posX = 168; obj.posY = 104; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.Dungeon_Pot; obj.direction = Direction.Down; obj.posX = 248; obj.posY = 136; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.Dungeon_Pot; obj.direction = Direction.Down; obj.posX = 56; obj.posY = 136; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.Dungeon_Pot; obj.direction = Direction.Down; obj.posX = 200; obj.posY = 40; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.Post_HammerPost_Up; obj.direction = Direction.Down; obj.posX = 200; obj.posY = 152; dataRef.ints.Add(obj);}
			//add the ref to the data list
			Data.Add(dataRef);
			#endregion

		}
	}
}