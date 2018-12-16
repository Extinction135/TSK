using System.Collections.Generic;

namespace DungeonRun
{
	public static class RoomData_SkullIsland_Row
	{
		//data is added to this list
		public static List<RoomXmlData> Data = new List<RoomXmlData>();
		//data uses this reference
		public static RoomXmlData dataRef;
		//data is added in the constructor below
		static RoomData_SkullIsland_Row()
		{

			#region ForestIsland_RowRoom

			dataRef = new RoomXmlData();
			dataRef.type = RoomID.ForestIsland_RowRoom;
			dataRef.windDirection = Direction.None;
			dataRef.windFrequency = 2;
			dataRef.windIntensity = 0;
			{IndObjXmlData obj = new IndObjXmlData(); obj.type = IndestructibleType.Dungeon_BlockDark; obj.direction = Direction.Down; obj.posX = 72; obj.posY = 88; dataRef.inds.Add(obj);}
			{IndObjXmlData obj = new IndObjXmlData(); obj.type = IndestructibleType.Dungeon_BlockDark; obj.direction = Direction.Down; obj.posX = 216; obj.posY = 72; dataRef.inds.Add(obj);}
			{IndObjXmlData obj = new IndObjXmlData(); obj.type = IndestructibleType.Dungeon_BlockDark; obj.direction = Direction.Down; obj.posX = 232; obj.posY = 72; dataRef.inds.Add(obj);}
			{IndObjXmlData obj = new IndObjXmlData(); obj.type = IndestructibleType.Dungeon_BlockDark; obj.direction = Direction.Down; obj.posX = 232; obj.posY = 88; dataRef.inds.Add(obj);}
			{IndObjXmlData obj = new IndObjXmlData(); obj.type = IndestructibleType.Dungeon_BlockDark; obj.direction = Direction.Down; obj.posX = 232; obj.posY = 104; dataRef.inds.Add(obj);}
			{IndObjXmlData obj = new IndObjXmlData(); obj.type = IndestructibleType.Dungeon_BlockDark; obj.direction = Direction.Down; obj.posX = 216; obj.posY = 104; dataRef.inds.Add(obj);}
			{IndObjXmlData obj = new IndObjXmlData(); obj.type = IndestructibleType.Dungeon_BlockDark; obj.direction = Direction.Down; obj.posX = 88; obj.posY = 72; dataRef.inds.Add(obj);}
			{IndObjXmlData obj = new IndObjXmlData(); obj.type = IndestructibleType.Dungeon_BlockDark; obj.direction = Direction.Down; obj.posX = 72; obj.posY = 72; dataRef.inds.Add(obj);}
			{IndObjXmlData obj = new IndObjXmlData(); obj.type = IndestructibleType.Dungeon_BlockDark; obj.direction = Direction.Down; obj.posX = 72; obj.posY = 104; dataRef.inds.Add(obj);}
			{IndObjXmlData obj = new IndObjXmlData(); obj.type = IndestructibleType.Dungeon_BlockDark; obj.direction = Direction.Down; obj.posX = 88; obj.posY = 104; dataRef.inds.Add(obj);}
			{IndObjXmlData obj = new IndObjXmlData(); obj.type = IndestructibleType.Dungeon_BlockDark; obj.direction = Direction.Down; obj.posX = 40; obj.posY = 40; dataRef.inds.Add(obj);}
			{IndObjXmlData obj = new IndObjXmlData(); obj.type = IndestructibleType.Dungeon_BlockDark; obj.direction = Direction.Down; obj.posX = 40; obj.posY = 136; dataRef.inds.Add(obj);}
			{IndObjXmlData obj = new IndObjXmlData(); obj.type = IndestructibleType.Dungeon_BlockDark; obj.direction = Direction.Down; obj.posX = 264; obj.posY = 136; dataRef.inds.Add(obj);}
			{IndObjXmlData obj = new IndObjXmlData(); obj.type = IndestructibleType.Dungeon_BlockDark; obj.direction = Direction.Down; obj.posX = 264; obj.posY = 40; dataRef.inds.Add(obj);}
			{IndObjXmlData obj = new IndObjXmlData(); obj.type = IndestructibleType.Dungeon_BlockDark; obj.direction = Direction.Down; obj.posX = 168; obj.posY = 72; dataRef.inds.Add(obj);}
			{IndObjXmlData obj = new IndObjXmlData(); obj.type = IndestructibleType.Dungeon_BlockDark; obj.direction = Direction.Down; obj.posX = 136; obj.posY = 72; dataRef.inds.Add(obj);}
			{IndObjXmlData obj = new IndObjXmlData(); obj.type = IndestructibleType.Dungeon_BlockDark; obj.direction = Direction.Down; obj.posX = 136; obj.posY = 104; dataRef.inds.Add(obj);}
			{IndObjXmlData obj = new IndObjXmlData(); obj.type = IndestructibleType.Dungeon_BlockDark; obj.direction = Direction.Down; obj.posX = 168; obj.posY = 104; dataRef.inds.Add(obj);}
			{IndObjXmlData obj = new IndObjXmlData(); obj.type = IndestructibleType.Dungeon_BlockDark; obj.direction = Direction.Down; obj.posX = 200; obj.posY = 72; dataRef.inds.Add(obj);}
			{IndObjXmlData obj = new IndObjXmlData(); obj.type = IndestructibleType.Dungeon_BlockDark; obj.direction = Direction.Down; obj.posX = 200; obj.posY = 104; dataRef.inds.Add(obj);}
			{IndObjXmlData obj = new IndObjXmlData(); obj.type = IndestructibleType.Dungeon_BlockDark; obj.direction = Direction.Down; obj.posX = 104; obj.posY = 72; dataRef.inds.Add(obj);}
			{IndObjXmlData obj = new IndObjXmlData(); obj.type = IndestructibleType.Dungeon_BlockDark; obj.direction = Direction.Down; obj.posX = 104; obj.posY = 104; dataRef.inds.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.TorchLit; obj.direction = Direction.Down; obj.posX = 152; obj.posY = 72; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.TorchLit; obj.direction = Direction.Down; obj.posX = 152; obj.posY = 104; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.Lava_Pit; obj.direction = Direction.Down; obj.posX = 120; obj.posY = 72; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.Lava_Pit; obj.direction = Direction.Down; obj.posX = 120; obj.posY = 104; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.Lava_Pit; obj.direction = Direction.Down; obj.posX = 184; obj.posY = 72; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.Lava_Pit; obj.direction = Direction.Down; obj.posX = 184; obj.posY = 104; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.Lava_Pit; obj.direction = Direction.Down; obj.posX = 120; obj.posY = 88; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.Lava_Pit; obj.direction = Direction.Down; obj.posX = 184; obj.posY = 88; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.ConveyorBeltOn; obj.direction = Direction.Down; obj.posX = 248; obj.posY = 72; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.ConveyorBeltOn; obj.direction = Direction.Left; obj.posX = 216; obj.posY = 120; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.ConveyorBeltOn; obj.direction = Direction.Left; obj.posX = 232; obj.posY = 120; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.ConveyorBeltOff; obj.direction = Direction.Down; obj.posX = 248; obj.posY = 104; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.ConveyorBeltOff; obj.direction = Direction.Left; obj.posX = 248; obj.posY = 120; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.ConveyorBeltOn; obj.direction = Direction.Left; obj.posX = 168; obj.posY = 120; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.ConveyorBeltOff; obj.direction = Direction.Left; obj.posX = 200; obj.posY = 120; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.ConveyorBeltOff; obj.direction = Direction.Left; obj.posX = 184; obj.posY = 120; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.Dungeon_SwitchBlockBtn; obj.direction = Direction.Down; obj.posX = 216; obj.posY = 88; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.Dungeon_BlockSpike; obj.direction = Direction.Right; obj.posX = 146; obj.posY = 89; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.Barrel; obj.direction = Direction.Down; obj.posX = 56; obj.posY = 40; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.Dungeon_Switch; obj.direction = Direction.Down; obj.posX = 152; obj.posY = 88; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.LeverOn; obj.direction = Direction.Down; obj.posX = 88; obj.posY = 88; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.Lava_Pit; obj.direction = Direction.Down; obj.posX = 168; obj.posY = 88; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.Lava_Pit; obj.direction = Direction.Down; obj.posX = 136; obj.posY = 88; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.Lava_PitTeethTop; obj.direction = Direction.Down; obj.posX = 120; obj.posY = 72; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.ConveyorBeltOn; obj.direction = Direction.Left; obj.posX = 152; obj.posY = 120; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.ConveyorBeltOn; obj.direction = Direction.Left; obj.posX = 104; obj.posY = 120; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.Lava_PitTeethTop; obj.direction = Direction.Down; obj.posX = 184; obj.posY = 72; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.Lava_PitTeethTop; obj.direction = Direction.Down; obj.posX = 168; obj.posY = 88; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.ConveyorBeltOn; obj.direction = Direction.Left; obj.posX = 88; obj.posY = 120; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.Lava_PitTeethTop; obj.direction = Direction.Down; obj.posX = 136; obj.posY = 88; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.Lava_PitTeethBottom; obj.direction = Direction.Down; obj.posX = 184; obj.posY = 104; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.FloorStain; obj.direction = Direction.Down; obj.posX = 201; obj.posY = 121; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.ConveyorBeltOn; obj.direction = Direction.Up; obj.posX = 56; obj.posY = 104; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.ConveyorBeltOff; obj.direction = Direction.Left; obj.posX = 136; obj.posY = 120; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.ConveyorBeltOn; obj.direction = Direction.Down; obj.posX = 248; obj.posY = 88; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.ConveyorBeltOn; obj.direction = Direction.Up; obj.posX = 56; obj.posY = 88; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.Lava_PitTeethBottom; obj.direction = Direction.Down; obj.posX = 120; obj.posY = 104; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.Lava_PitTeethBottom; obj.direction = Direction.Down; obj.posX = 136; obj.posY = 88; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.Lava_PitTeethBottom; obj.direction = Direction.Down; obj.posX = 168; obj.posY = 88; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.ConveyorBeltOn; obj.direction = Direction.Right; obj.posX = 72; obj.posY = 56; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.ConveyorBeltOn; obj.direction = Direction.Right; obj.posX = 88; obj.posY = 56; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.Barrel; obj.direction = Direction.Down; obj.posX = 72; obj.posY = 40; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.Barrel; obj.direction = Direction.Down; obj.posX = 40; obj.posY = 56; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.Barrel; obj.direction = Direction.Down; obj.posX = 40; obj.posY = 72; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.Barrel; obj.direction = Direction.Down; obj.posX = 40; obj.posY = 88; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.Barrel; obj.direction = Direction.Down; obj.posX = 40; obj.posY = 104; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.Barrel; obj.direction = Direction.Down; obj.posX = 40; obj.posY = 120; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.ConveyorBeltOn; obj.direction = Direction.Right; obj.posX = 136; obj.posY = 56; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.ConveyorBeltOn; obj.direction = Direction.Right; obj.posX = 152; obj.posY = 56; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.ConveyorBeltOn; obj.direction = Direction.Right; obj.posX = 200; obj.posY = 56; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.ConveyorBeltOn; obj.direction = Direction.Right; obj.posX = 216; obj.posY = 56; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.ConveyorBeltOff; obj.direction = Direction.Right; obj.posX = 232; obj.posY = 56; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.ConveyorBeltOff; obj.direction = Direction.Down; obj.posX = 248; obj.posY = 56; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.ConveyorBeltOff; obj.direction = Direction.Left; obj.posX = 120; obj.posY = 120; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.ConveyorBeltOff; obj.direction = Direction.Left; obj.posX = 72; obj.posY = 120; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.ConveyorBeltOff; obj.direction = Direction.Up; obj.posX = 56; obj.posY = 120; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.ConveyorBeltOff; obj.direction = Direction.Up; obj.posX = 56; obj.posY = 72; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.ConveyorBeltOff; obj.direction = Direction.Right; obj.posX = 56; obj.posY = 56; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.ConveyorBeltOff; obj.direction = Direction.Right; obj.posX = 104; obj.posY = 56; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.ConveyorBeltOff; obj.direction = Direction.Right; obj.posX = 120; obj.posY = 56; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.ConveyorBeltOff; obj.direction = Direction.Right; obj.posX = 168; obj.posY = 56; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.ConveyorBeltOff; obj.direction = Direction.Right; obj.posX = 184; obj.posY = 56; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.Dungeon_Statue; obj.direction = Direction.Down; obj.posX = 56; obj.posY = 56; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.Dungeon_Statue; obj.direction = Direction.Down; obj.posX = 248; obj.posY = 56; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.Dungeon_Statue; obj.direction = Direction.Down; obj.posX = 248; obj.posY = 120; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.Dungeon_Statue; obj.direction = Direction.Down; obj.posX = 56; obj.posY = 120; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.Dungeon_Statue; obj.direction = Direction.Down; obj.posX = 152; obj.posY = 56; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.Dungeon_Statue; obj.direction = Direction.Down; obj.posX = 151; obj.posY = 120; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.Barrel; obj.direction = Direction.Down; obj.posX = 56; obj.posY = 136; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.Barrel; obj.direction = Direction.Down; obj.posX = 72; obj.posY = 136; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.Barrel; obj.direction = Direction.Down; obj.posX = 88; obj.posY = 136; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.Barrel; obj.direction = Direction.Down; obj.posX = 104; obj.posY = 136; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.Barrel; obj.direction = Direction.Down; obj.posX = 120; obj.posY = 136; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.Barrel; obj.direction = Direction.Down; obj.posX = 136; obj.posY = 136; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.Barrel; obj.direction = Direction.Down; obj.posX = 152; obj.posY = 136; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.Barrel; obj.direction = Direction.Down; obj.posX = 168; obj.posY = 136; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.Barrel; obj.direction = Direction.Down; obj.posX = 184; obj.posY = 136; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.Barrel; obj.direction = Direction.Down; obj.posX = 200; obj.posY = 136; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.Barrel; obj.direction = Direction.Down; obj.posX = 216; obj.posY = 136; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.Barrel; obj.direction = Direction.Down; obj.posX = 232; obj.posY = 136; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.Barrel; obj.direction = Direction.Down; obj.posX = 248; obj.posY = 136; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.Barrel; obj.direction = Direction.Down; obj.posX = 264; obj.posY = 120; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.Barrel; obj.direction = Direction.Down; obj.posX = 264; obj.posY = 104; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.Barrel; obj.direction = Direction.Down; obj.posX = 264; obj.posY = 88; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.Barrel; obj.direction = Direction.Down; obj.posX = 264; obj.posY = 72; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.Barrel; obj.direction = Direction.Down; obj.posX = 264; obj.posY = 56; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.Barrel; obj.direction = Direction.Down; obj.posX = 248; obj.posY = 40; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.Barrel; obj.direction = Direction.Down; obj.posX = 88; obj.posY = 40; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.Barrel; obj.direction = Direction.Down; obj.posX = 104; obj.posY = 40; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.Barrel; obj.direction = Direction.Down; obj.posX = 120; obj.posY = 40; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.Barrel; obj.direction = Direction.Down; obj.posX = 136; obj.posY = 40; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.Barrel; obj.direction = Direction.Down; obj.posX = 152; obj.posY = 40; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.Barrel; obj.direction = Direction.Down; obj.posX = 168; obj.posY = 40; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.Barrel; obj.direction = Direction.Down; obj.posX = 184; obj.posY = 40; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.Barrel; obj.direction = Direction.Down; obj.posX = 200; obj.posY = 40; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.Barrel; obj.direction = Direction.Down; obj.posX = 216; obj.posY = 40; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.Barrel; obj.direction = Direction.Down; obj.posX = 232; obj.posY = 40; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.Boat_Barrel; obj.direction = Direction.Down; obj.posX = 88; obj.posY = 24; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.Boat_Barrel; obj.direction = Direction.Down; obj.posX = 24; obj.posY = 24; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.Boat_Barrel; obj.direction = Direction.Down; obj.posX = 24; obj.posY = 152; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.Boat_Barrel; obj.direction = Direction.Down; obj.posX = 56; obj.posY = 152; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.Boat_Barrel; obj.direction = Direction.Down; obj.posX = 232; obj.posY = 152; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.Boat_Barrel; obj.direction = Direction.Down; obj.posX = 184; obj.posY = 24; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.Boat_Barrel; obj.direction = Direction.Down; obj.posX = 280; obj.posY = 72; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.Boat_Barrel; obj.direction = Direction.Down; obj.posX = 136; obj.posY = 152; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.Boat_Barrel; obj.direction = Direction.Down; obj.posX = 280; obj.posY = 24; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.Boat_Barrel; obj.direction = Direction.Down; obj.posX = 280; obj.posY = 152; dataRef.ints.Add(obj);}
			//add the ref to the data list
			Data.Add(dataRef);
			#endregion

		}
	}
}