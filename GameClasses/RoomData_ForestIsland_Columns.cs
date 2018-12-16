using System.Collections.Generic;

namespace DungeonRun
{
	public static class RoomData_ForestIsland_Columns
	{
		//data is added to this list
		public static List<RoomXmlData> Data = new List<RoomXmlData>();
		//data uses this reference
		public static RoomXmlData dataRef;
		//data is added in the constructor below
		static RoomData_ForestIsland_Columns()
		{

			#region ForestIsland_ColumnRoom

			dataRef = new RoomXmlData();
			dataRef.type = RoomID.ForestIsland_ColumnRoom;
			dataRef.windDirection = Direction.None;
			dataRef.windFrequency = 2;
			dataRef.windIntensity = 0;
			{IndObjXmlData obj = new IndObjXmlData(); obj.type = IndestructibleType.Dungeon_BlockDark; obj.direction = Direction.Down; obj.posX = 72; obj.posY = 200; dataRef.inds.Add(obj);}
			{IndObjXmlData obj = new IndObjXmlData(); obj.type = IndestructibleType.Dungeon_BlockDark; obj.direction = Direction.Down; obj.posX = 104; obj.posY = 200; dataRef.inds.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.Dungeon_BlockLight; obj.direction = Direction.Down; obj.posX = 88; obj.posY = 168; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.Dungeon_SpawnMob; obj.direction = Direction.Down; obj.posX = 88; obj.posY = 152; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.Dungeon_BlockLight; obj.direction = Direction.Down; obj.posX = 88; obj.posY = 88; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.Dungeon_Switch; obj.direction = Direction.Down; obj.posX = 88; obj.posY = 136; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.Dungeon_SwitchBlockUp; obj.direction = Direction.Down; obj.posX = 72; obj.posY = 136; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.Dungeon_SwitchBlockUp; obj.direction = Direction.Down; obj.posX = 72; obj.posY = 120; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.Dungeon_SwitchBlockUp; obj.direction = Direction.Down; obj.posX = 88; obj.posY = 120; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.Dungeon_SwitchBlockUp; obj.direction = Direction.Down; obj.posX = 104; obj.posY = 120; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.FloorStain; obj.direction = Direction.Down; obj.posX = 10; obj.posY = 27; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.FloorStain; obj.direction = Direction.Down; obj.posX = 7; obj.posY = 88; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.FloorStain; obj.direction = Direction.Down; obj.posX = 7; obj.posY = 181; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.FloorStain; obj.direction = Direction.Down; obj.posX = 26; obj.posY = 9; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.FloorStain; obj.direction = Direction.Down; obj.posX = 26; obj.posY = 107; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.FloorStain; obj.direction = Direction.Down; obj.posX = 23; obj.posY = 260; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.FloorStain; obj.direction = Direction.Down; obj.posX = 26; obj.posY = 298; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.FloorStain; obj.direction = Direction.Down; obj.posX = 54; obj.posY = 7; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.FloorStain; obj.direction = Direction.Down; obj.posX = 56; obj.posY = 148; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.FloorStain; obj.direction = Direction.Down; obj.posX = 53; obj.posY = 215; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.Coliseum_Shadow_Stairs_Right; obj.direction = Direction.Down; obj.posX = 120; obj.posY = 248; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.Lava_Pit; obj.direction = Direction.Down; obj.posX = 40; obj.posY = 56; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.Lava_PitBridge; obj.direction = Direction.Down; obj.posX = 88; obj.posY = 40; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.Coliseum_Shadow_Stairs_Middle; obj.direction = Direction.Down; obj.posX = 72; obj.posY = 248; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.Lava_PitBridge; obj.direction = Direction.Down; obj.posX = 88; obj.posY = 264; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.FloorStain; obj.direction = Direction.Down; obj.posX = 117; obj.posY = 89; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.FloorStain; obj.direction = Direction.Down; obj.posX = 119; obj.posY = 102; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.FloorStain; obj.direction = Direction.Down; obj.posX = 120; obj.posY = 203; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.Coliseum_Shadow_Stairs_Middle; obj.direction = Direction.Down; obj.posX = 88; obj.posY = 248; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.FloorStain; obj.direction = Direction.Down; obj.posX = 137; obj.posY = 298; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.FloorStain; obj.direction = Direction.Down; obj.posX = 148; obj.posY = 26; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.FloorStain; obj.direction = Direction.Down; obj.posX = 150; obj.posY = 54; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.FloorStain; obj.direction = Direction.Down; obj.posX = 155; obj.posY = 232; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.FloorStain; obj.direction = Direction.Down; obj.posX = 170; obj.posY = 219; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.Dungeon_SwitchBlockUp; obj.direction = Direction.Down; obj.posX = 72; obj.posY = 152; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.Dungeon_SwitchBlockUp; obj.direction = Direction.Down; obj.posX = 104; obj.posY = 136; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.Dungeon_SwitchBlockUp; obj.direction = Direction.Down; obj.posX = 104; obj.posY = 152; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.Dungeon_SwitchBlockUp; obj.direction = Direction.Down; obj.posX = 72; obj.posY = 168; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.Dungeon_SwitchBlockUp; obj.direction = Direction.Down; obj.posX = 104; obj.posY = 168; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.Dungeon_SwitchBlockUp; obj.direction = Direction.Down; obj.posX = 104; obj.posY = 184; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.Dungeon_SwitchBlockUp; obj.direction = Direction.Down; obj.posX = 88; obj.posY = 184; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.Dungeon_SwitchBlockUp; obj.direction = Direction.Down; obj.posX = 72; obj.posY = 184; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.Lava_PitTrap; obj.direction = Direction.Down; obj.posX = 72; obj.posY = 216; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.Dungeon_BlockLight; obj.direction = Direction.Down; obj.posX = 88; obj.posY = 216; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.Dungeon_SwitchBlockBtn; obj.direction = Direction.Down; obj.posX = 88; obj.posY = 200; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.Post_HammerPost_Up; obj.direction = Direction.Down; obj.posX = 104; obj.posY = 216; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.Dungeon_SwitchBlockBtn; obj.direction = Direction.Down; obj.posX = 88; obj.posY = 104; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.Post_VerticalLeft; obj.direction = Direction.Down; obj.posX = 72; obj.posY = 88; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.Post_CornerLeft; obj.direction = Direction.Down; obj.posX = 72; obj.posY = 104; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.Post_CornerRight; obj.direction = Direction.Down; obj.posX = 104; obj.posY = 104; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.Post_VerticalRight; obj.direction = Direction.Down; obj.posX = 104; obj.posY = 88; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.Coliseum_Shadow_Stairs_Middle; obj.direction = Direction.Down; obj.posX = 104; obj.posY = 248; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.Lava_PitTeethTop; obj.direction = Direction.Down; obj.posX = 56; obj.posY = 264; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.Lava_PitBridge; obj.direction = Direction.Down; obj.posX = 88; obj.posY = 56; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.Lava_PitTeethTop; obj.direction = Direction.Down; obj.posX = 72; obj.posY = 264; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.Lava_PitTeethTop; obj.direction = Direction.Down; obj.posX = 104; obj.posY = 264; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.Lava_Pit; obj.direction = Direction.Down; obj.posX = 40; obj.posY = 200; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.Lava_Pit; obj.direction = Direction.Down; obj.posX = 40; obj.posY = 216; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.Lava_Pit; obj.direction = Direction.Down; obj.posX = 40; obj.posY = 232; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.Lava_Pit; obj.direction = Direction.Down; obj.posX = 40; obj.posY = 248; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.Lava_Pit; obj.direction = Direction.Down; obj.posX = 136; obj.posY = 248; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.Lava_Pit; obj.direction = Direction.Down; obj.posX = 136; obj.posY = 232; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.Lava_Pit; obj.direction = Direction.Down; obj.posX = 136; obj.posY = 216; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.Lava_Pit; obj.direction = Direction.Down; obj.posX = 136; obj.posY = 200; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.Lava_Pit; obj.direction = Direction.Down; obj.posX = 40; obj.posY = 184; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.Lava_Pit; obj.direction = Direction.Down; obj.posX = 40; obj.posY = 168; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.Lava_Pit; obj.direction = Direction.Down; obj.posX = 40; obj.posY = 152; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.Lava_Pit; obj.direction = Direction.Down; obj.posX = 40; obj.posY = 136; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.Lava_Pit; obj.direction = Direction.Down; obj.posX = 40; obj.posY = 120; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.Lava_Pit; obj.direction = Direction.Down; obj.posX = 40; obj.posY = 104; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.Lava_Pit; obj.direction = Direction.Down; obj.posX = 40; obj.posY = 88; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.Lava_Pit; obj.direction = Direction.Down; obj.posX = 40; obj.posY = 72; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.Lava_Pit; obj.direction = Direction.Down; obj.posX = 56; obj.posY = 56; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.Lava_Pit; obj.direction = Direction.Down; obj.posX = 72; obj.posY = 56; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.Lava_Pit; obj.direction = Direction.Down; obj.posX = 104; obj.posY = 56; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.Lava_Pit; obj.direction = Direction.Down; obj.posX = 120; obj.posY = 56; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.Lava_Pit; obj.direction = Direction.Down; obj.posX = 136; obj.posY = 56; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.Dungeon_Pot; obj.direction = Direction.Down; obj.posX = 104; obj.posY = 72; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.Lava_Pit; obj.direction = Direction.Down; obj.posX = 40; obj.posY = 40; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.Lava_Pit; obj.direction = Direction.Down; obj.posX = 136; obj.posY = 72; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.Lava_Pit; obj.direction = Direction.Down; obj.posX = 136; obj.posY = 88; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.Lava_Pit; obj.direction = Direction.Down; obj.posX = 136; obj.posY = 104; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.Lava_Pit; obj.direction = Direction.Down; obj.posX = 136; obj.posY = 120; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.Lava_Pit; obj.direction = Direction.Down; obj.posX = 136; obj.posY = 136; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.Lava_Pit; obj.direction = Direction.Down; obj.posX = 136; obj.posY = 152; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.Lava_Pit; obj.direction = Direction.Down; obj.posX = 136; obj.posY = 168; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.Lava_Pit; obj.direction = Direction.Down; obj.posX = 136; obj.posY = 184; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.Lava_Pit; obj.direction = Direction.Down; obj.posX = 56; obj.posY = 40; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.Lava_Pit; obj.direction = Direction.Down; obj.posX = 72; obj.posY = 40; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.Dungeon_Pot; obj.direction = Direction.Down; obj.posX = 88; obj.posY = 72; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.Lava_Pit; obj.direction = Direction.Down; obj.posX = 104; obj.posY = 40; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.Lava_Pit; obj.direction = Direction.Down; obj.posX = 120; obj.posY = 40; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.Lava_Pit; obj.direction = Direction.Down; obj.posX = 136; obj.posY = 40; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.Lava_Pit; obj.direction = Direction.Down; obj.posX = 40; obj.posY = 264; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.Lava_Pit; obj.direction = Direction.Down; obj.posX = 56; obj.posY = 264; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.Lava_Pit; obj.direction = Direction.Down; obj.posX = 72; obj.posY = 264; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.Dungeon_Pot; obj.direction = Direction.Down; obj.posX = 72; obj.posY = 72; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.Lava_Pit; obj.direction = Direction.Down; obj.posX = 104; obj.posY = 264; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.Lava_Pit; obj.direction = Direction.Down; obj.posX = 120; obj.posY = 264; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.Lava_Pit; obj.direction = Direction.Down; obj.posX = 136; obj.posY = 264; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.Lava_PitTeethTop; obj.direction = Direction.Down; obj.posX = 40; obj.posY = 40; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.Lava_PitTeethTop; obj.direction = Direction.Down; obj.posX = 56; obj.posY = 40; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.Lava_PitTeethTop; obj.direction = Direction.Down; obj.posX = 72; obj.posY = 40; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.Lava_PitTeethTop; obj.direction = Direction.Down; obj.posX = 88; obj.posY = 40; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.Lava_PitTeethTop; obj.direction = Direction.Down; obj.posX = 104; obj.posY = 40; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.Lava_PitTeethTop; obj.direction = Direction.Down; obj.posX = 120; obj.posY = 40; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.Lava_PitTeethTop; obj.direction = Direction.Down; obj.posX = 136; obj.posY = 40; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.Lava_PitTeethTop; obj.direction = Direction.Down; obj.posX = 120; obj.posY = 264; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.Lava_PitTeethTop; obj.direction = Direction.Down; obj.posX = 88; obj.posY = 264; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.Pot; obj.direction = Direction.Down; obj.posX = 40; obj.posY = 24; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.Pot; obj.direction = Direction.Down; obj.posX = 56; obj.posY = 24; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.Pot; obj.direction = Direction.Down; obj.posX = 72; obj.posY = 24; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.Lava_PitTeethBottom; obj.direction = Direction.Down; obj.posX = 40; obj.posY = 264; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.Lava_PitTeethBottom; obj.direction = Direction.Down; obj.posX = 56; obj.posY = 264; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.Lava_PitTeethBottom; obj.direction = Direction.Down; obj.posX = 72; obj.posY = 264; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.Lava_PitTeethBottom; obj.direction = Direction.Down; obj.posX = 88; obj.posY = 264; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.Lava_PitTeethBottom; obj.direction = Direction.Down; obj.posX = 104; obj.posY = 264; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.Lava_PitTeethBottom; obj.direction = Direction.Down; obj.posX = 120; obj.posY = 264; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.Lava_PitTeethBottom; obj.direction = Direction.Down; obj.posX = 136; obj.posY = 264; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.Lava_PitTeethBottom; obj.direction = Direction.Down; obj.posX = 56; obj.posY = 56; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.Lava_PitTeethBottom; obj.direction = Direction.Down; obj.posX = 72; obj.posY = 56; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.Lava_PitTeethBottom; obj.direction = Direction.Down; obj.posX = 88; obj.posY = 56; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.Lava_PitTeethBottom; obj.direction = Direction.Down; obj.posX = 104; obj.posY = 56; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.Lava_PitTeethBottom; obj.direction = Direction.Down; obj.posX = 120; obj.posY = 56; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.Coliseum_Shadow_Stairs_Left; obj.direction = Direction.Down; obj.posX = 56; obj.posY = 248; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.Dungeon_Statue; obj.direction = Direction.Down; obj.posX = 56; obj.posY = 232; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.Dungeon_Statue; obj.direction = Direction.Down; obj.posX = 120; obj.posY = 232; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.Dungeon_Pot; obj.direction = Direction.Down; obj.posX = 56; obj.posY = 72; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.Dungeon_Pot; obj.direction = Direction.Down; obj.posX = 120; obj.posY = 72; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.Pot; obj.direction = Direction.Down; obj.posX = 88; obj.posY = 24; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.Pot; obj.direction = Direction.Down; obj.posX = 104; obj.posY = 24; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.Pot; obj.direction = Direction.Down; obj.posX = 120; obj.posY = 24; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.Pot; obj.direction = Direction.Down; obj.posX = 136; obj.posY = 24; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.Dungeon_BlockLight; obj.direction = Direction.Down; obj.posX = 56; obj.posY = 120; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.Dungeon_BlockLight; obj.direction = Direction.Down; obj.posX = 120; obj.posY = 120; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.Dungeon_BlockLight; obj.direction = Direction.Down; obj.posX = 56; obj.posY = 184; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.Dungeon_BlockLight; obj.direction = Direction.Down; obj.posX = 120; obj.posY = 184; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.FloorBlood; obj.direction = Direction.Down; obj.posX = 8; obj.posY = 280; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.FloorBlood; obj.direction = Direction.Down; obj.posX = 168; obj.posY = 296; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.FloorBlood; obj.direction = Direction.Down; obj.posX = 152; obj.posY = 104; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.FloorBlood; obj.direction = Direction.Down; obj.posX = 168; obj.posY = 8; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.FloorBlood; obj.direction = Direction.Down; obj.posX = 8; obj.posY = 40; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.FloorSkeleton; obj.direction = Direction.Down; obj.posX = 24; obj.posY = 8; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.FloorSkeleton; obj.direction = Direction.Down; obj.posX = 24; obj.posY = 200; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.FloorSkeleton; obj.direction = Direction.Down; obj.posX = 152; obj.posY = 296; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.FloorSkeleton; obj.direction = Direction.Down; obj.posX = 152; obj.posY = 152; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.FloorSkeleton; obj.direction = Direction.Down; obj.posX = 40; obj.posY = 280; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.FloorBlood; obj.direction = Direction.Down; obj.posX = 24; obj.posY = 136; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.FloorStain; obj.direction = Direction.Down; obj.posX = 9; obj.posY = 116; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.FloorStain; obj.direction = Direction.Down; obj.posX = 6; obj.posY = 139; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.FloorStain; obj.direction = Direction.Down; obj.posX = 11; obj.posY = 214; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.FloorStain; obj.direction = Direction.Down; obj.posX = 20; obj.posY = 132; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.FloorStain; obj.direction = Direction.Down; obj.posX = 58; obj.posY = 104; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.FloorStain; obj.direction = Direction.Down; obj.posX = 55; obj.posY = 202; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.FloorStain; obj.direction = Direction.Down; obj.posX = 68; obj.posY = 280; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.FloorStain; obj.direction = Direction.Down; obj.posX = 89; obj.posY = 134; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.FloorStain; obj.direction = Direction.Down; obj.posX = 87; obj.posY = 246; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.FloorStain; obj.direction = Direction.Down; obj.posX = 107; obj.posY = 214; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.FloorStain; obj.direction = Direction.Down; obj.posX = 104; obj.posY = 232; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.FloorStain; obj.direction = Direction.Down; obj.posX = 105; obj.posY = 247; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.FloorStain; obj.direction = Direction.Down; obj.posX = 106; obj.posY = 295; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.FloorStain; obj.direction = Direction.Down; obj.posX = 116; obj.posY = 6; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.FloorStain; obj.direction = Direction.Down; obj.posX = 121; obj.posY = 87; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.FloorStain; obj.direction = Direction.Down; obj.posX = 119; obj.posY = 154; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.FloorStain; obj.direction = Direction.Down; obj.posX = 119; obj.posY = 216; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.FloorStain; obj.direction = Direction.Down; obj.posX = 123; obj.posY = 283; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.FloorStain; obj.direction = Direction.Down; obj.posX = 133; obj.posY = 6; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.FloorStain; obj.direction = Direction.Down; obj.posX = 150; obj.posY = 88; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.FloorStain; obj.direction = Direction.Down; obj.posX = 155; obj.posY = 298; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.FloorStain; obj.direction = Direction.Down; obj.posX = 166; obj.posY = 230; dataRef.ints.Add(obj);}
			//add the ref to the data list
			Data.Add(dataRef);
			#endregion

		}
	}
}