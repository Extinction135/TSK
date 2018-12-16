using System.Collections.Generic;

namespace DungeonRun
{
	public static class RoomData_SkullIsland_Square
	{
		//data is added to this list
		public static List<RoomXmlData> Data = new List<RoomXmlData>();
		//data uses this reference
		public static RoomXmlData dataRef;
		//data is added in the constructor below
		static RoomData_SkullIsland_Square()
		{

			#region ForestIsland_SquareRoom

			dataRef = new RoomXmlData();
			dataRef.type = RoomID.ForestIsland_SquareRoom;
			dataRef.windDirection = Direction.None;
			dataRef.windFrequency = 2;
			dataRef.windIntensity = 0;
			{IndObjXmlData obj = new IndObjXmlData(); obj.type = IndestructibleType.Dungeon_BlockDark; obj.direction = Direction.Down; obj.posX = 56; obj.posY = 24; dataRef.inds.Add(obj);}
			{IndObjXmlData obj = new IndObjXmlData(); obj.type = IndestructibleType.Dungeon_BlockDark; obj.direction = Direction.Down; obj.posX = 24; obj.posY = 56; dataRef.inds.Add(obj);}
			{IndObjXmlData obj = new IndObjXmlData(); obj.type = IndestructibleType.Dungeon_BlockDark; obj.direction = Direction.Down; obj.posX = 24; obj.posY = 120; dataRef.inds.Add(obj);}
			{IndObjXmlData obj = new IndObjXmlData(); obj.type = IndestructibleType.Dungeon_BlockDark; obj.direction = Direction.Down; obj.posX = 56; obj.posY = 152; dataRef.inds.Add(obj);}
			{IndObjXmlData obj = new IndObjXmlData(); obj.type = IndestructibleType.Dungeon_BlockDark; obj.direction = Direction.Down; obj.posX = 120; obj.posY = 152; dataRef.inds.Add(obj);}
			{IndObjXmlData obj = new IndObjXmlData(); obj.type = IndestructibleType.Dungeon_BlockDark; obj.direction = Direction.Down; obj.posX = 152; obj.posY = 120; dataRef.inds.Add(obj);}
			{IndObjXmlData obj = new IndObjXmlData(); obj.type = IndestructibleType.Dungeon_BlockDark; obj.direction = Direction.Down; obj.posX = 152; obj.posY = 56; dataRef.inds.Add(obj);}
			{IndObjXmlData obj = new IndObjXmlData(); obj.type = IndestructibleType.Dungeon_BlockDark; obj.direction = Direction.Down; obj.posX = 120; obj.posY = 24; dataRef.inds.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.Barrel; obj.direction = Direction.Down; obj.posX = 40; obj.posY = 120; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.Barrel; obj.direction = Direction.Down; obj.posX = 72; obj.posY = 24; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.Barrel; obj.direction = Direction.Down; obj.posX = 104; obj.posY = 24; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.Barrel; obj.direction = Direction.Down; obj.posX = 56; obj.posY = 40; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.Barrel; obj.direction = Direction.Down; obj.posX = 40; obj.posY = 56; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.Barrel; obj.direction = Direction.Down; obj.posX = 56; obj.posY = 56; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.Barrel; obj.direction = Direction.Down; obj.posX = 56; obj.posY = 72; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.Barrel; obj.direction = Direction.Down; obj.posX = 136; obj.posY = 56; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.Barrel; obj.direction = Direction.Down; obj.posX = 120; obj.posY = 56; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.Barrel; obj.direction = Direction.Down; obj.posX = 104; obj.posY = 56; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.Barrel; obj.direction = Direction.Down; obj.posX = 120; obj.posY = 40; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.Barrel; obj.direction = Direction.Down; obj.posX = 24; obj.posY = 72; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.Barrel; obj.direction = Direction.Down; obj.posX = 24; obj.posY = 104; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.Barrel; obj.direction = Direction.Down; obj.posX = 152; obj.posY = 72; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.Barrel; obj.direction = Direction.Down; obj.posX = 56; obj.posY = 120; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.Barrel; obj.direction = Direction.Down; obj.posX = 152; obj.posY = 104; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.Barrel; obj.direction = Direction.Down; obj.posX = 104; obj.posY = 152; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.Barrel; obj.direction = Direction.Down; obj.posX = 72; obj.posY = 152; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.Barrel; obj.direction = Direction.Down; obj.posX = 72; obj.posY = 120; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.Barrel; obj.direction = Direction.Down; obj.posX = 56; obj.posY = 136; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.Barrel; obj.direction = Direction.Down; obj.posX = 120; obj.posY = 136; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.Barrel; obj.direction = Direction.Down; obj.posX = 120; obj.posY = 120; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.Barrel; obj.direction = Direction.Down; obj.posX = 120; obj.posY = 104; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.Barrel; obj.direction = Direction.Down; obj.posX = 136; obj.posY = 120; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.Barrel; obj.direction = Direction.Down; obj.posX = 88; obj.posY = 72; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.Barrel; obj.direction = Direction.Down; obj.posX = 72; obj.posY = 88; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.Barrel; obj.direction = Direction.Down; obj.posX = 104; obj.posY = 88; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.Barrel; obj.direction = Direction.Down; obj.posX = 88; obj.posY = 104; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.Barrel; obj.direction = Direction.Down; obj.posX = 152; obj.posY = 88; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.Barrel; obj.direction = Direction.Down; obj.posX = 88; obj.posY = 24; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.Barrel; obj.direction = Direction.Down; obj.posX = 24; obj.posY = 88; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.Barrel; obj.direction = Direction.Down; obj.posX = 88; obj.posY = 152; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.Vendor_EnemyItems; obj.direction = Direction.Down; obj.posX = 88; obj.posY = 88; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.Dungeon_SpikesFloorOn; obj.direction = Direction.Down; obj.posX = 104; obj.posY = 72; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.Dungeon_SpikesFloorOn; obj.direction = Direction.Down; obj.posX = 72; obj.posY = 72; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.Dungeon_SpikesFloorOn; obj.direction = Direction.Down; obj.posX = 72; obj.posY = 104; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.Dungeon_SpikesFloorOn; obj.direction = Direction.Down; obj.posX = 104; obj.posY = 104; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.LeverOff; obj.direction = Direction.Down; obj.posX = 24; obj.posY = 152; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.Bush; obj.direction = Direction.Down; obj.posX = 24; obj.posY = 136; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.Bush; obj.direction = Direction.Down; obj.posX = 40; obj.posY = 136; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.Bush; obj.direction = Direction.Down; obj.posX = 40; obj.posY = 152; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.Bush; obj.direction = Direction.Down; obj.posX = 136; obj.posY = 152; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.Bush; obj.direction = Direction.Down; obj.posX = 136; obj.posY = 136; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.Bush; obj.direction = Direction.Down; obj.posX = 152; obj.posY = 136; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.Bush; obj.direction = Direction.Down; obj.posX = 152; obj.posY = 40; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.Bush; obj.direction = Direction.Down; obj.posX = 136; obj.posY = 40; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.Bush; obj.direction = Direction.Down; obj.posX = 136; obj.posY = 24; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.Bush; obj.direction = Direction.Down; obj.posX = 40; obj.posY = 24; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.Bush; obj.direction = Direction.Down; obj.posX = 40; obj.posY = 40; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.Bush; obj.direction = Direction.Down; obj.posX = 24; obj.posY = 40; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.FloorBlood; obj.direction = Direction.Down; obj.posX = 104; obj.posY = 40; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.FloorBlood; obj.direction = Direction.Down; obj.posX = 40; obj.posY = 88; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.FloorBlood; obj.direction = Direction.Down; obj.posX = 104; obj.posY = 136; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.FloorSkeleton; obj.direction = Direction.Down; obj.posX = 72; obj.posY = 56; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.FloorSkeleton; obj.direction = Direction.Down; obj.posX = 136; obj.posY = 104; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.Lava_PitTrap; obj.direction = Direction.Down; obj.posX = 56; obj.posY = 88; dataRef.ints.Add(obj);}
			//add the ref to the data list
			Data.Add(dataRef);
			#endregion

		}
	}
}