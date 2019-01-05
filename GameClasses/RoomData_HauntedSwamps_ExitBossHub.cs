using System.Collections.Generic;

namespace DungeonRun
{
	public static class RoomData_HauntedSwamps_ExitBossHub
	{
		//data is added to this list
		public static List<RoomXmlData> Data = new List<RoomXmlData>();
		//data uses this reference
		public static RoomXmlData dataRef;
		//data is added in the constructor below
		static RoomData_HauntedSwamps_ExitBossHub()
		{

			#region HauntedSwamps_BossRoom

			dataRef = new RoomXmlData();
			dataRef.type = RoomID.HauntedSwamps_BossRoom;
			dataRef.windDirection = Direction.None;
			dataRef.windFrequency = 2;
			dataRef.windIntensity = 0;
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.Water_2x2; obj.direction = Direction.Down; obj.posX = 24; obj.posY = 24; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.Water_2x2; obj.direction = Direction.Down; obj.posX = 24; obj.posY = 56; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.Water_2x2; obj.direction = Direction.Down; obj.posX = 56; obj.posY = 24; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.Water_LillyPad; obj.direction = Direction.Down; obj.posX = 56; obj.posY = 56; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.Water_2x2; obj.direction = Direction.Down; obj.posX = 24; obj.posY = 88; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.Water_2x2; obj.direction = Direction.Down; obj.posX = 24; obj.posY = 120; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.Water_2x2; obj.direction = Direction.Down; obj.posX = 56; obj.posY = 88; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.Water_2x2; obj.direction = Direction.Down; obj.posX = 56; obj.posY = 120; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.Water_2x2; obj.direction = Direction.Down; obj.posX = 88; obj.posY = 24; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.Water_2x2; obj.direction = Direction.Down; obj.posX = 88; obj.posY = 56; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.Water_2x2; obj.direction = Direction.Down; obj.posX = 88; obj.posY = 88; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.Water_2x2; obj.direction = Direction.Down; obj.posX = 88; obj.posY = 120; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.Water_2x2; obj.direction = Direction.Down; obj.posX = 24; obj.posY = 152; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.Water_2x2; obj.direction = Direction.Down; obj.posX = 56; obj.posY = 152; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.Water_LillyPad; obj.direction = Direction.Down; obj.posX = 184; obj.posY = 24; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.Water_2x2; obj.direction = Direction.Down; obj.posX = 120; obj.posY = 152; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.Water_2x2; obj.direction = Direction.Down; obj.posX = 152; obj.posY = 152; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.Water_2x2; obj.direction = Direction.Down; obj.posX = 120; obj.posY = 24; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.Water_2x2; obj.direction = Direction.Down; obj.posX = 152; obj.posY = 24; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.Water_LillyPad; obj.direction = Direction.Down; obj.posX = 152; obj.posY = 88; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.Water_2x2; obj.direction = Direction.Down; obj.posX = 216; obj.posY = 24; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.Water_2x2; obj.direction = Direction.Down; obj.posX = 120; obj.posY = 56; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.Water_2x2; obj.direction = Direction.Down; obj.posX = 248; obj.posY = 24; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.Water_2x2; obj.direction = Direction.Down; obj.posX = 152; obj.posY = 56; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.Water_2x2; obj.direction = Direction.Down; obj.posX = 120; obj.posY = 88; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.Water_2x2; obj.direction = Direction.Down; obj.posX = 184; obj.posY = 56; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.Water_LillyPad; obj.direction = Direction.Down; obj.posX = 248; obj.posY = 120; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.Water_2x2; obj.direction = Direction.Down; obj.posX = 120; obj.posY = 120; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.Water_2x2; obj.direction = Direction.Down; obj.posX = 216; obj.posY = 56; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.Water_2x2; obj.direction = Direction.Down; obj.posX = 184; obj.posY = 88; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.Water_2x2; obj.direction = Direction.Down; obj.posX = 152; obj.posY = 120; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.Water_2x2; obj.direction = Direction.Down; obj.posX = 248; obj.posY = 56; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.Water_2x2; obj.direction = Direction.Down; obj.posX = 216; obj.posY = 88; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.Water_2x2; obj.direction = Direction.Down; obj.posX = 184; obj.posY = 120; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.Water_2x2; obj.direction = Direction.Down; obj.posX = 248; obj.posY = 88; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.Water_2x2; obj.direction = Direction.Down; obj.posX = 216; obj.posY = 120; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.Water_LillyPad; obj.direction = Direction.Down; obj.posX = 88; obj.posY = 152; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.Water_2x2; obj.direction = Direction.Down; obj.posX = 184; obj.posY = 152; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.Water_2x2; obj.direction = Direction.Down; obj.posX = 216; obj.posY = 152; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.Water_2x2; obj.direction = Direction.Down; obj.posX = 248; obj.posY = 152; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.Water_1x1; obj.direction = Direction.Down; obj.posX = 280; obj.posY = 24; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.Water_1x1; obj.direction = Direction.Down; obj.posX = 280; obj.posY = 40; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.Water_1x1; obj.direction = Direction.Down; obj.posX = 280; obj.posY = 56; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.Water_1x1; obj.direction = Direction.Down; obj.posX = 280; obj.posY = 72; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.Water_1x1; obj.direction = Direction.Down; obj.posX = 280; obj.posY = 88; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.Water_1x1; obj.direction = Direction.Down; obj.posX = 280; obj.posY = 104; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.Water_1x1; obj.direction = Direction.Down; obj.posX = 280; obj.posY = 120; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.Water_1x1; obj.direction = Direction.Down; obj.posX = 280; obj.posY = 136; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.Water_1x1; obj.direction = Direction.Down; obj.posX = 280; obj.posY = 152; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.Water_1x1; obj.direction = Direction.Down; obj.posX = 280; obj.posY = 168; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.Coastline_Straight; obj.direction = Direction.Down; obj.posX = 24; obj.posY = 8; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.Coastline_Straight; obj.direction = Direction.Down; obj.posX = 40; obj.posY = 8; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.Coastline_Straight; obj.direction = Direction.Down; obj.posX = 56; obj.posY = 8; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.Coastline_Straight; obj.direction = Direction.Down; obj.posX = 72; obj.posY = 8; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.Coastline_Straight; obj.direction = Direction.Down; obj.posX = 88; obj.posY = 8; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.Coastline_Straight; obj.direction = Direction.Down; obj.posX = 104; obj.posY = 8; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.Coastline_Straight; obj.direction = Direction.Down; obj.posX = 120; obj.posY = 8; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.Coastline_Straight; obj.direction = Direction.Down; obj.posX = 136; obj.posY = 8; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.Coastline_Straight; obj.direction = Direction.Down; obj.posX = 152; obj.posY = 8; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.Coastline_Straight; obj.direction = Direction.Down; obj.posX = 168; obj.posY = 8; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.Coastline_Straight; obj.direction = Direction.Down; obj.posX = 184; obj.posY = 8; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.Coastline_Straight; obj.direction = Direction.Down; obj.posX = 200; obj.posY = 8; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.Coastline_Straight; obj.direction = Direction.Down; obj.posX = 216; obj.posY = 8; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.Coastline_Straight; obj.direction = Direction.Down; obj.posX = 232; obj.posY = 8; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.Coastline_Straight; obj.direction = Direction.Down; obj.posX = 248; obj.posY = 8; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.Coastline_Straight; obj.direction = Direction.Down; obj.posX = 264; obj.posY = 8; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.Coastline_Straight; obj.direction = Direction.Down; obj.posX = 280; obj.posY = 8; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.Coastline_Straight; obj.direction = Direction.Left; obj.posX = 296; obj.posY = 24; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.Coastline_Straight; obj.direction = Direction.Left; obj.posX = 296; obj.posY = 40; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.Coastline_Straight; obj.direction = Direction.Left; obj.posX = 296; obj.posY = 56; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.Coastline_Straight; obj.direction = Direction.Left; obj.posX = 296; obj.posY = 72; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.Coastline_Straight; obj.direction = Direction.Left; obj.posX = 296; obj.posY = 88; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.Coastline_Straight; obj.direction = Direction.Left; obj.posX = 296; obj.posY = 104; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.Coastline_Straight; obj.direction = Direction.Left; obj.posX = 296; obj.posY = 120; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.Coastline_Straight; obj.direction = Direction.Left; obj.posX = 296; obj.posY = 136; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.Coastline_Straight; obj.direction = Direction.Left; obj.posX = 296; obj.posY = 152; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.Coastline_Straight; obj.direction = Direction.Left; obj.posX = 296; obj.posY = 168; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.Coastline_Straight; obj.direction = Direction.Right; obj.posX = 8; obj.posY = 168; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.Coastline_Straight; obj.direction = Direction.Right; obj.posX = 8; obj.posY = 152; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.Coastline_Straight; obj.direction = Direction.Right; obj.posX = 8; obj.posY = 136; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.Coastline_Straight; obj.direction = Direction.Right; obj.posX = 8; obj.posY = 120; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.Coastline_Straight; obj.direction = Direction.Right; obj.posX = 8; obj.posY = 104; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.Coastline_Straight; obj.direction = Direction.Right; obj.posX = 8; obj.posY = 88; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.Coastline_Straight; obj.direction = Direction.Right; obj.posX = 8; obj.posY = 72; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.Coastline_Straight; obj.direction = Direction.Right; obj.posX = 8; obj.posY = 56; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.Coastline_Straight; obj.direction = Direction.Right; obj.posX = 8; obj.posY = 40; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.Coastline_Straight; obj.direction = Direction.Right; obj.posX = 8; obj.posY = 24; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.Coastline_Corner_Exterior; obj.direction = Direction.Down; obj.posX = 8; obj.posY = 8; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.Coastline_Corner_Exterior; obj.direction = Direction.Left; obj.posX = 296; obj.posY = 8; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.Water_SmPlant; obj.direction = Direction.Down; obj.posX = 40; obj.posY = 120; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.Water_SmPlant; obj.direction = Direction.Down; obj.posX = 264; obj.posY = 40; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.Water_SmPlant; obj.direction = Direction.Down; obj.posX = 200; obj.posY = 168; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.Water_Bulb; obj.direction = Direction.Down; obj.posX = 8; obj.posY = 56; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.Water_Bulb; obj.direction = Direction.Down; obj.posX = 8; obj.posY = 136; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.Water_Bulb; obj.direction = Direction.Down; obj.posX = 296; obj.posY = 24; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.Water_Bulb; obj.direction = Direction.Down; obj.posX = 296; obj.posY = 120; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.Water_Bulb; obj.direction = Direction.Down; obj.posX = 232; obj.posY = 8; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.Water_Bulb; obj.direction = Direction.Down; obj.posX = 56; obj.posY = 8; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.Water_Bulb; obj.direction = Direction.Down; obj.posX = 152; obj.posY = 8; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.Water_Bulb; obj.direction = Direction.Down; obj.posX = 56; obj.posY = 168; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.Water_Bulb; obj.direction = Direction.Down; obj.posX = 232; obj.posY = 168; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.Water_RockSm; obj.direction = Direction.Down; obj.posX = 88; obj.posY = 40; dataRef.ints.Add(obj);}
			//add the ref to the data list
			Data.Add(dataRef);
			#endregion


			#region HauntedSwamps_ExitRoom

			dataRef = new RoomXmlData();
			dataRef.type = RoomID.HauntedSwamps_ExitRoom;
			dataRef.windDirection = Direction.None;
			dataRef.windFrequency = 2;
			dataRef.windIntensity = 0;
			{IndObjXmlData obj = new IndObjXmlData(); obj.type = IndestructibleType.Dungeon_ExitPillarLeft; obj.direction = Direction.Down; obj.posX = 72; obj.posY = 152; dataRef.inds.Add(obj);}
			{IndObjXmlData obj = new IndObjXmlData(); obj.type = IndestructibleType.Dungeon_ExitPillarRight; obj.direction = Direction.Down; obj.posX = 104; obj.posY = 152; dataRef.inds.Add(obj);}
			{IndObjXmlData obj = new IndObjXmlData(); obj.type = IndestructibleType.Dungeon_ExitLight; obj.direction = Direction.Down; obj.posX = 88; obj.posY = 160; dataRef.inds.Add(obj);}
			{IndObjXmlData obj = new IndObjXmlData(); obj.type = IndestructibleType.Dungeon_Exit; obj.direction = Direction.Down; obj.posX = 88; obj.posY = 152; dataRef.inds.Add(obj);}
			{IndObjXmlData obj = new IndObjXmlData(); obj.type = IndestructibleType.Dungeon_ExitPillarLeft; obj.direction = Direction.Down; obj.posX = 72; obj.posY = 152; dataRef.inds.Add(obj);}
			{IndObjXmlData obj = new IndObjXmlData(); obj.type = IndestructibleType.Dungeon_ExitPillarRight; obj.direction = Direction.Down; obj.posX = 104; obj.posY = 152; dataRef.inds.Add(obj);}
			{IndObjXmlData obj = new IndObjXmlData(); obj.type = IndestructibleType.Dungeon_ExitLight; obj.direction = Direction.Down; obj.posX = 88; obj.posY = 160; dataRef.inds.Add(obj);}
			{IndObjXmlData obj = new IndObjXmlData(); obj.type = IndestructibleType.Dungeon_Exit; obj.direction = Direction.Down; obj.posX = 88; obj.posY = 152; dataRef.inds.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.Water_LillyPad; obj.direction = Direction.Down; obj.posX = 88; obj.posY = 120; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.Water_LillyPad; obj.direction = Direction.Down; obj.posX = 72; obj.posY = 88; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.Water_LillyPad; obj.direction = Direction.Down; obj.posX = 88; obj.posY = 56; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.Water_LillyPad; obj.direction = Direction.Down; obj.posX = 72; obj.posY = 24; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.Water_1x1; obj.direction = Direction.Down; obj.posX = 72; obj.posY = 72; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.Water_2x2; obj.direction = Direction.Down; obj.posX = 104; obj.posY = 24; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.Water_2x2; obj.direction = Direction.Down; obj.posX = 136; obj.posY = 24; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.Water_2x2; obj.direction = Direction.Down; obj.posX = 40; obj.posY = 24; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.Water_2x2; obj.direction = Direction.Down; obj.posX = 40; obj.posY = 56; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.Water_2x2; obj.direction = Direction.Down; obj.posX = 120; obj.posY = 56; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.Water_2x2; obj.direction = Direction.Down; obj.posX = 104; obj.posY = 88; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.Water_2x2; obj.direction = Direction.Down; obj.posX = 40; obj.posY = 88; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.Water_2x2; obj.direction = Direction.Down; obj.posX = 136; obj.posY = 88; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.Water_2x2; obj.direction = Direction.Down; obj.posX = 120; obj.posY = 120; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.Water_2x2; obj.direction = Direction.Down; obj.posX = 56; obj.posY = 120; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.Water_2x2; obj.direction = Direction.Down; obj.posX = 24; obj.posY = 120; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.Water_2x2; obj.direction = Direction.Down; obj.posX = 40; obj.posY = 152; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.Water_2x2; obj.direction = Direction.Down; obj.posX = 120; obj.posY = 152; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.Water_1x1; obj.direction = Direction.Down; obj.posX = 24; obj.posY = 24; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.Water_1x1; obj.direction = Direction.Down; obj.posX = 24; obj.posY = 40; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.Water_1x1; obj.direction = Direction.Down; obj.posX = 24; obj.posY = 56; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.Water_1x1; obj.direction = Direction.Down; obj.posX = 24; obj.posY = 72; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.Water_1x1; obj.direction = Direction.Down; obj.posX = 24; obj.posY = 88; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.Water_1x1; obj.direction = Direction.Down; obj.posX = 24; obj.posY = 104; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.Water_1x1; obj.direction = Direction.Down; obj.posX = 24; obj.posY = 152; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.Water_1x1; obj.direction = Direction.Down; obj.posX = 24; obj.posY = 168; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.Water_1x1; obj.direction = Direction.Down; obj.posX = 152; obj.posY = 56; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.Water_1x1; obj.direction = Direction.Down; obj.posX = 152; obj.posY = 72; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.Water_1x1; obj.direction = Direction.Down; obj.posX = 152; obj.posY = 120; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.Water_1x1; obj.direction = Direction.Down; obj.posX = 152; obj.posY = 136; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.Water_1x1; obj.direction = Direction.Down; obj.posX = 152; obj.posY = 152; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.Water_1x1; obj.direction = Direction.Down; obj.posX = 152; obj.posY = 168; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.Water_1x1; obj.direction = Direction.Down; obj.posX = 72; obj.posY = 56; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.Coastline_Straight; obj.direction = Direction.Down; obj.posX = 24; obj.posY = 8; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.Coastline_Straight; obj.direction = Direction.Down; obj.posX = 40; obj.posY = 8; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.Coastline_Straight; obj.direction = Direction.Down; obj.posX = 56; obj.posY = 8; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.Coastline_Straight; obj.direction = Direction.Down; obj.posX = 72; obj.posY = 8; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.Coastline_Straight; obj.direction = Direction.Down; obj.posX = 88; obj.posY = 8; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.Coastline_Straight; obj.direction = Direction.Down; obj.posX = 104; obj.posY = 8; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.Coastline_Straight; obj.direction = Direction.Down; obj.posX = 120; obj.posY = 8; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.Coastline_Straight; obj.direction = Direction.Down; obj.posX = 136; obj.posY = 8; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.Coastline_Straight; obj.direction = Direction.Down; obj.posX = 152; obj.posY = 8; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.Coastline_Straight; obj.direction = Direction.Right; obj.posX = 8; obj.posY = 24; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.Coastline_Straight; obj.direction = Direction.Left; obj.posX = 168; obj.posY = 24; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.Coastline_Straight; obj.direction = Direction.Right; obj.posX = 8; obj.posY = 40; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.Coastline_Straight; obj.direction = Direction.Right; obj.posX = 8; obj.posY = 56; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.Signpost; obj.direction = Direction.Down; obj.posX = 72; obj.posY = 56; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.Coastline_Straight; obj.direction = Direction.Right; obj.posX = 8; obj.posY = 72; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.Coastline_Straight; obj.direction = Direction.Right; obj.posX = 8; obj.posY = 88; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.Coastline_Straight; obj.direction = Direction.Right; obj.posX = 8; obj.posY = 104; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.Coastline_Straight; obj.direction = Direction.Right; obj.posX = 8; obj.posY = 120; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.Coastline_Straight; obj.direction = Direction.Right; obj.posX = 8; obj.posY = 136; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.Coastline_Straight; obj.direction = Direction.Right; obj.posX = 8; obj.posY = 152; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.Coastline_Straight; obj.direction = Direction.Right; obj.posX = 8; obj.posY = 168; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.Coastline_Straight; obj.direction = Direction.Left; obj.posX = 168; obj.posY = 40; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.Coastline_Straight; obj.direction = Direction.Left; obj.posX = 168; obj.posY = 56; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.Coastline_Straight; obj.direction = Direction.Left; obj.posX = 168; obj.posY = 72; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.Coastline_Straight; obj.direction = Direction.Left; obj.posX = 168; obj.posY = 88; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.Coastline_Straight; obj.direction = Direction.Left; obj.posX = 168; obj.posY = 104; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.Coastline_Straight; obj.direction = Direction.Left; obj.posX = 168; obj.posY = 120; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.Coastline_Straight; obj.direction = Direction.Left; obj.posX = 168; obj.posY = 136; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.Coastline_Straight; obj.direction = Direction.Left; obj.posX = 168; obj.posY = 152; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.Coastline_Straight; obj.direction = Direction.Left; obj.posX = 168; obj.posY = 168; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.Coastline_Corner_Exterior; obj.direction = Direction.Down; obj.posX = 8; obj.posY = 8; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.Coastline_Corner_Exterior; obj.direction = Direction.Left; obj.posX = 168; obj.posY = 8; dataRef.ints.Add(obj);}
			//add the ref to the data list
			Data.Add(dataRef);
			#endregion


			#region HauntedSwamps_HubRoom

			dataRef = new RoomXmlData();
			dataRef.type = RoomID.HauntedSwamps_HubRoom;
			dataRef.windDirection = Direction.None;
			dataRef.windFrequency = 2;
			dataRef.windIntensity = 0;
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.Water_2x2; obj.direction = Direction.Down; obj.posX = 24; obj.posY = 24; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.Water_2x2; obj.direction = Direction.Down; obj.posX = 56; obj.posY = 24; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.Water_2x2; obj.direction = Direction.Down; obj.posX = 24; obj.posY = 56; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.Water_2x2; obj.direction = Direction.Down; obj.posX = 88; obj.posY = 24; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.Water_LillyPad; obj.direction = Direction.Down; obj.posX = 56; obj.posY = 216; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.Water_2x2; obj.direction = Direction.Down; obj.posX = 24; obj.posY = 88; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.Water_2x2; obj.direction = Direction.Down; obj.posX = 120; obj.posY = 24; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.Water_2x2; obj.direction = Direction.Down; obj.posX = 88; obj.posY = 56; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.Water_2x2; obj.direction = Direction.Down; obj.posX = 56; obj.posY = 88; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.Water_2x2; obj.direction = Direction.Down; obj.posX = 24; obj.posY = 120; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.Water_2x2; obj.direction = Direction.Down; obj.posX = 152; obj.posY = 24; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.Water_2x2; obj.direction = Direction.Down; obj.posX = 120; obj.posY = 56; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.Water_2x2; obj.direction = Direction.Down; obj.posX = 184; obj.posY = 24; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.Water_2x2; obj.direction = Direction.Down; obj.posX = 152; obj.posY = 56; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.Water_2x2; obj.direction = Direction.Down; obj.posX = 1144; obj.posY = 392; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.Water_2x2; obj.direction = Direction.Down; obj.posX = 1080; obj.posY = 344; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.Water_2x2; obj.direction = Direction.Down; obj.posX = 1208; obj.posY = -88; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.Water_2x2; obj.direction = Direction.Down; obj.posX = 1720; obj.posY = -120; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.Water_2x2; obj.direction = Direction.Down; obj.posX = 88; obj.posY = 88; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.Water_2x2; obj.direction = Direction.Down; obj.posX = 120; obj.posY = 88; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.Water_2x2; obj.direction = Direction.Down; obj.posX = 56; obj.posY = 120; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.Water_2x2; obj.direction = Direction.Down; obj.posX = 88; obj.posY = 120; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.Water_2x2; obj.direction = Direction.Down; obj.posX = 120; obj.posY = 120; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.Water_2x2; obj.direction = Direction.Down; obj.posX = 152; obj.posY = 88; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.Water_2x2; obj.direction = Direction.Down; obj.posX = 184; obj.posY = 56; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.Water_2x2; obj.direction = Direction.Down; obj.posX = 216; obj.posY = 24; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.Water_2x2; obj.direction = Direction.Down; obj.posX = 248; obj.posY = 24; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.Water_2x2; obj.direction = Direction.Down; obj.posX = 248; obj.posY = 88; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.Water_LillyPad; obj.direction = Direction.Down; obj.posX = 232; obj.posY = 56; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.Water_2x2; obj.direction = Direction.Down; obj.posX = 184; obj.posY = 88; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.Water_BigPlant; obj.direction = Direction.Down; obj.posX = 72; obj.posY = 184; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.Water_LillyPad; obj.direction = Direction.Down; obj.posX = 232; obj.posY = 216; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.Water_2x2; obj.direction = Direction.Down; obj.posX = 152; obj.posY = 120; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.Water_2x2; obj.direction = Direction.Down; obj.posX = 24; obj.posY = 152; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.Water_2x2; obj.direction = Direction.Down; obj.posX = 24; obj.posY = 184; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.Water_2x2; obj.direction = Direction.Down; obj.posX = 24; obj.posY = 216; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.Water_2x2; obj.direction = Direction.Down; obj.posX = 24; obj.posY = 248; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.Water_2x2; obj.direction = Direction.Down; obj.posX = 264; obj.posY = 216; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.Water_2x2; obj.direction = Direction.Down; obj.posX = 88; obj.posY = 152; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.Water_BigPlant; obj.direction = Direction.Down; obj.posX = 216; obj.posY = 88; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.Water_2x2; obj.direction = Direction.Down; obj.posX = 24; obj.posY = 280; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.Water_1x1; obj.direction = Direction.Down; obj.posX = 216; obj.posY = 216; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.Water_1x1; obj.direction = Direction.Down; obj.posX = 104; obj.posY = 184; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.Water_2x2; obj.direction = Direction.Down; obj.posX = 120; obj.posY = 152; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.Water_2x2; obj.direction = Direction.Down; obj.posX = 184; obj.posY = 120; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.Water_2x2; obj.direction = Direction.Down; obj.posX = 152; obj.posY = 152; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.Water_2x2; obj.direction = Direction.Down; obj.posX = 184; obj.posY = 152; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.Water_2x2; obj.direction = Direction.Down; obj.posX = 216; obj.posY = 120; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.Water_2x2; obj.direction = Direction.Down; obj.posX = 248; obj.posY = 120; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.Water_2x2; obj.direction = Direction.Down; obj.posX = 216; obj.posY = 152; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.Water_2x2; obj.direction = Direction.Down; obj.posX = 248; obj.posY = 152; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.Water_2x2; obj.direction = Direction.Down; obj.posX = 120; obj.posY = 184; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.Water_2x2; obj.direction = Direction.Down; obj.posX = 88; obj.posY = 216; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.Water_2x2; obj.direction = Direction.Down; obj.posX = 56; obj.posY = 248; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.Water_2x2; obj.direction = Direction.Down; obj.posX = 88; obj.posY = 248; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.Water_2x2; obj.direction = Direction.Down; obj.posX = 120; obj.posY = 216; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.Water_2x2; obj.direction = Direction.Down; obj.posX = 152; obj.posY = 184; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.Water_2x2; obj.direction = Direction.Down; obj.posX = 152; obj.posY = 216; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.Water_2x2; obj.direction = Direction.Down; obj.posX = 184; obj.posY = 184; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.Water_2x2; obj.direction = Direction.Down; obj.posX = 184; obj.posY = 216; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.Water_2x2; obj.direction = Direction.Down; obj.posX = 216; obj.posY = 184; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.Water_1x1; obj.direction = Direction.Down; obj.posX = 216; obj.posY = 232; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.Water_2x2; obj.direction = Direction.Down; obj.posX = 248; obj.posY = 184; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.Water_LillyPad; obj.direction = Direction.Down; obj.posX = 56; obj.posY = 56; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.Water_2x2; obj.direction = Direction.Down; obj.posX = 120; obj.posY = 248; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.Water_2x2; obj.direction = Direction.Down; obj.posX = 56; obj.posY = 280; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.Water_2x2; obj.direction = Direction.Down; obj.posX = 88; obj.posY = 280; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.Water_2x2; obj.direction = Direction.Down; obj.posX = 120; obj.posY = 280; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.Water_2x2; obj.direction = Direction.Down; obj.posX = 152; obj.posY = 248; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.Water_2x2; obj.direction = Direction.Down; obj.posX = 152; obj.posY = 280; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.Water_2x2; obj.direction = Direction.Down; obj.posX = 184; obj.posY = 248; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.Water_2x2; obj.direction = Direction.Down; obj.posX = 216; obj.posY = 248; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.Water_2x2; obj.direction = Direction.Down; obj.posX = 184; obj.posY = 280; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.Water_2x2; obj.direction = Direction.Down; obj.posX = 216; obj.posY = 280; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.Water_2x2; obj.direction = Direction.Down; obj.posX = 248; obj.posY = 248; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.Water_2x2; obj.direction = Direction.Down; obj.posX = 248; obj.posY = 280; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.Water_1x1; obj.direction = Direction.Down; obj.posX = 280; obj.posY = 24; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.Water_1x1; obj.direction = Direction.Down; obj.posX = 280; obj.posY = 40; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.Water_1x1; obj.direction = Direction.Down; obj.posX = 216; obj.posY = 56; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.Water_1x1; obj.direction = Direction.Down; obj.posX = 216; obj.posY = 72; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.Water_1x1; obj.direction = Direction.Down; obj.posX = 280; obj.posY = 88; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.Water_1x1; obj.direction = Direction.Down; obj.posX = 280; obj.posY = 104; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.Water_1x1; obj.direction = Direction.Down; obj.posX = 280; obj.posY = 120; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.Water_1x1; obj.direction = Direction.Down; obj.posX = 280; obj.posY = 136; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.Water_1x1; obj.direction = Direction.Down; obj.posX = 280; obj.posY = 152; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.Water_1x1; obj.direction = Direction.Down; obj.posX = 280; obj.posY = 168; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.Water_1x1; obj.direction = Direction.Down; obj.posX = 280; obj.posY = 184; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.Water_1x1; obj.direction = Direction.Down; obj.posX = 280; obj.posY = 200; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.Water_2x2; obj.direction = Direction.Down; obj.posX = 264; obj.posY = 56; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.Water_2x2; obj.direction = Direction.Down; obj.posX = 56; obj.posY = 152; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.Water_1x1; obj.direction = Direction.Down; obj.posX = 280; obj.posY = 248; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.Water_1x1; obj.direction = Direction.Down; obj.posX = 280; obj.posY = 264; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.Water_1x1; obj.direction = Direction.Down; obj.posX = 280; obj.posY = 280; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.Water_1x1; obj.direction = Direction.Down; obj.posX = 280; obj.posY = 296; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.Coastline_Straight; obj.direction = Direction.Down; obj.posX = 24; obj.posY = 8; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.Coastline_Straight; obj.direction = Direction.Down; obj.posX = 40; obj.posY = 8; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.Coastline_Straight; obj.direction = Direction.Down; obj.posX = 56; obj.posY = 8; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.Coastline_Straight; obj.direction = Direction.Down; obj.posX = 72; obj.posY = 8; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.Coastline_Straight; obj.direction = Direction.Down; obj.posX = 88; obj.posY = 8; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.Coastline_Straight; obj.direction = Direction.Down; obj.posX = 104; obj.posY = 8; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.Coastline_Straight; obj.direction = Direction.Down; obj.posX = 120; obj.posY = 8; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.Coastline_Straight; obj.direction = Direction.Down; obj.posX = 136; obj.posY = 8; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.Coastline_Straight; obj.direction = Direction.Down; obj.posX = 152; obj.posY = 8; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.Coastline_Straight; obj.direction = Direction.Down; obj.posX = 168; obj.posY = 8; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.Coastline_Straight; obj.direction = Direction.Down; obj.posX = 184; obj.posY = 8; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.Coastline_Straight; obj.direction = Direction.Down; obj.posX = 200; obj.posY = 8; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.Coastline_Straight; obj.direction = Direction.Down; obj.posX = 216; obj.posY = 8; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.Coastline_Straight; obj.direction = Direction.Down; obj.posX = 232; obj.posY = 8; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.Coastline_Straight; obj.direction = Direction.Down; obj.posX = 248; obj.posY = 8; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.Coastline_Straight; obj.direction = Direction.Down; obj.posX = 264; obj.posY = 8; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.Coastline_Straight; obj.direction = Direction.Down; obj.posX = 280; obj.posY = 8; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.Coastline_Straight; obj.direction = Direction.Left; obj.posX = 296; obj.posY = 24; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.Coastline_Straight; obj.direction = Direction.Left; obj.posX = 296; obj.posY = 40; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.Coastline_Straight; obj.direction = Direction.Left; obj.posX = 296; obj.posY = 56; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.Coastline_Straight; obj.direction = Direction.Left; obj.posX = 296; obj.posY = 88; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.Coastline_Straight; obj.direction = Direction.Left; obj.posX = 296; obj.posY = 104; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.Coastline_Straight; obj.direction = Direction.Left; obj.posX = 296; obj.posY = 120; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.Coastline_Straight; obj.direction = Direction.Left; obj.posX = 296; obj.posY = 72; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.Coastline_Straight; obj.direction = Direction.Left; obj.posX = 296; obj.posY = 136; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.Coastline_Straight; obj.direction = Direction.Left; obj.posX = 296; obj.posY = 152; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.Coastline_Straight; obj.direction = Direction.Left; obj.posX = 296; obj.posY = 168; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.Coastline_Straight; obj.direction = Direction.Left; obj.posX = 296; obj.posY = 184; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.Coastline_Straight; obj.direction = Direction.Left; obj.posX = 296; obj.posY = 200; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.Coastline_Straight; obj.direction = Direction.Left; obj.posX = 296; obj.posY = 216; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.Coastline_Straight; obj.direction = Direction.Left; obj.posX = 296; obj.posY = 232; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.Coastline_Straight; obj.direction = Direction.Left; obj.posX = 296; obj.posY = 248; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.Coastline_Straight; obj.direction = Direction.Left; obj.posX = 296; obj.posY = 264; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.Coastline_Straight; obj.direction = Direction.Left; obj.posX = 296; obj.posY = 280; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.Coastline_Straight; obj.direction = Direction.Left; obj.posX = 296; obj.posY = 296; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.Coastline_Straight; obj.direction = Direction.Right; obj.posX = 8; obj.posY = 216; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.Coastline_Straight; obj.direction = Direction.Right; obj.posX = 8; obj.posY = 232; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.Coastline_Straight; obj.direction = Direction.Right; obj.posX = 8; obj.posY = 248; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.Coastline_Straight; obj.direction = Direction.Right; obj.posX = 8; obj.posY = 264; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.Coastline_Straight; obj.direction = Direction.Right; obj.posX = 8; obj.posY = 280; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.Coastline_Straight; obj.direction = Direction.Right; obj.posX = 8; obj.posY = 296; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.Coastline_Straight; obj.direction = Direction.Right; obj.posX = 8; obj.posY = 200; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.Coastline_Straight; obj.direction = Direction.Right; obj.posX = 8; obj.posY = 184; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.Coastline_Straight; obj.direction = Direction.Right; obj.posX = 8; obj.posY = 168; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.Coastline_Straight; obj.direction = Direction.Right; obj.posX = 8; obj.posY = 152; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.Coastline_Straight; obj.direction = Direction.Right; obj.posX = 8; obj.posY = 136; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.Coastline_Straight; obj.direction = Direction.Right; obj.posX = 8; obj.posY = 120; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.Coastline_Straight; obj.direction = Direction.Right; obj.posX = 8; obj.posY = 104; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.Coastline_Straight; obj.direction = Direction.Right; obj.posX = 8; obj.posY = 88; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.Coastline_Straight; obj.direction = Direction.Right; obj.posX = 8; obj.posY = 72; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.Coastline_Straight; obj.direction = Direction.Right; obj.posX = 8; obj.posY = 56; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.Coastline_Straight; obj.direction = Direction.Right; obj.posX = 8; obj.posY = 40; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.Coastline_Straight; obj.direction = Direction.Right; obj.posX = 8; obj.posY = 24; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.Coastline_Corner_Exterior; obj.direction = Direction.Down; obj.posX = 8; obj.posY = 8; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.Coastline_Corner_Exterior; obj.direction = Direction.Left; obj.posX = 296; obj.posY = 8; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.Bush; obj.direction = Direction.Down; obj.posX = 40; obj.posY = 40; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.Bush; obj.direction = Direction.Down; obj.posX = 40; obj.posY = 56; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.Bush; obj.direction = Direction.Down; obj.posX = 40; obj.posY = 72; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.Bush; obj.direction = Direction.Down; obj.posX = 40; obj.posY = 88; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.Bush; obj.direction = Direction.Down; obj.posX = 40; obj.posY = 104; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.Bush; obj.direction = Direction.Down; obj.posX = 40; obj.posY = 120; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.Bush; obj.direction = Direction.Down; obj.posX = 264; obj.posY = 56; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.Bush; obj.direction = Direction.Down; obj.posX = 264; obj.posY = 72; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.Bush; obj.direction = Direction.Down; obj.posX = 264; obj.posY = 88; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.Bush; obj.direction = Direction.Down; obj.posX = 264; obj.posY = 120; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.Bush; obj.direction = Direction.Down; obj.posX = 264; obj.posY = 136; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.Bush; obj.direction = Direction.Down; obj.posX = 264; obj.posY = 104; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.Water_1x1; obj.direction = Direction.Down; obj.posX = 104; obj.posY = 200; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.Water_1x1; obj.direction = Direction.Down; obj.posX = 56; obj.posY = 184; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.Water_1x1; obj.direction = Direction.Down; obj.posX = 56; obj.posY = 200; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.Bush; obj.direction = Direction.Down; obj.posX = 40; obj.posY = 136; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.Bush; obj.direction = Direction.Down; obj.posX = 40; obj.posY = 152; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.Bush; obj.direction = Direction.Down; obj.posX = 40; obj.posY = 168; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.Bush; obj.direction = Direction.Down; obj.posX = 40; obj.posY = 184; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.Bush; obj.direction = Direction.Down; obj.posX = 40; obj.posY = 200; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.Bush; obj.direction = Direction.Down; obj.posX = 40; obj.posY = 216; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.Bush; obj.direction = Direction.Down; obj.posX = 40; obj.posY = 232; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.Bush; obj.direction = Direction.Down; obj.posX = 40; obj.posY = 248; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.Bush; obj.direction = Direction.Down; obj.posX = 264; obj.posY = 248; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.Bush; obj.direction = Direction.Down; obj.posX = 264; obj.posY = 232; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.Bush; obj.direction = Direction.Down; obj.posX = 264; obj.posY = 216; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.Bush; obj.direction = Direction.Down; obj.posX = 264; obj.posY = 200; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.Bush; obj.direction = Direction.Down; obj.posX = 264; obj.posY = 184; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.Bush; obj.direction = Direction.Down; obj.posX = 264; obj.posY = 168; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.Bush; obj.direction = Direction.Down; obj.posX = 264; obj.posY = 152; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.Bush; obj.direction = Direction.Down; obj.posX = 264; obj.posY = 40; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.Bush; obj.direction = Direction.Down; obj.posX = 56; obj.posY = 248; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.Bush; obj.direction = Direction.Down; obj.posX = 72; obj.posY = 248; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.Bush; obj.direction = Direction.Down; obj.posX = 232; obj.posY = 248; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.Bush; obj.direction = Direction.Down; obj.posX = 248; obj.posY = 248; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.Bush; obj.direction = Direction.Down; obj.posX = 248; obj.posY = 40; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.Bush; obj.direction = Direction.Down; obj.posX = 232; obj.posY = 40; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.Bush; obj.direction = Direction.Down; obj.posX = 56; obj.posY = 40; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.Bush; obj.direction = Direction.Down; obj.posX = 72; obj.posY = 40; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.Water_SmPlant; obj.direction = Direction.Down; obj.posX = 56; obj.posY = 104; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.Water_SmPlant; obj.direction = Direction.Down; obj.posX = 248; obj.posY = 152; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.Post_Horizontal; obj.direction = Direction.Down; obj.posX = 40; obj.posY = 264; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.Post_Horizontal; obj.direction = Direction.Down; obj.posX = 56; obj.posY = 264; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.Post_Horizontal; obj.direction = Direction.Down; obj.posX = 72; obj.posY = 264; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.Post_Horizontal; obj.direction = Direction.Down; obj.posX = 88; obj.posY = 264; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.Post_Horizontal; obj.direction = Direction.Down; obj.posX = 264; obj.posY = 264; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.Post_Horizontal; obj.direction = Direction.Down; obj.posX = 248; obj.posY = 264; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.Post_Horizontal; obj.direction = Direction.Down; obj.posX = 232; obj.posY = 264; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.Post_Horizontal; obj.direction = Direction.Down; obj.posX = 216; obj.posY = 264; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.Post_Horizontal; obj.direction = Direction.Down; obj.posX = 104; obj.posY = 264; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.Post_Horizontal; obj.direction = Direction.Down; obj.posX = 200; obj.posY = 264; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.Post_CornerRight; obj.direction = Direction.Down; obj.posX = 184; obj.posY = 264; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.Post_CornerRight; obj.direction = Direction.Down; obj.posX = 120; obj.posY = 264; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.Post_VerticalRight; obj.direction = Direction.Down; obj.posX = 120; obj.posY = 248; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.Water_Vine; obj.direction = Direction.Down; obj.posX = 88; obj.posY = 40; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.Water_Vine; obj.direction = Direction.Down; obj.posX = 200; obj.posY = 40; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.Post_VerticalLeft; obj.direction = Direction.Down; obj.posX = 216; obj.posY = 56; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.Post_VerticalLeft; obj.direction = Direction.Down; obj.posX = 216; obj.posY = 72; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.Post_CornerRight; obj.direction = Direction.Down; obj.posX = 104; obj.posY = 72; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.Water_Bulb; obj.direction = Direction.Down; obj.posX = 40; obj.posY = 8; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.Water_Bulb; obj.direction = Direction.Down; obj.posX = 8; obj.posY = 72; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.Water_Bulb; obj.direction = Direction.Down; obj.posX = 24; obj.posY = 136; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.Water_Bulb; obj.direction = Direction.Down; obj.posX = 8; obj.posY = 184; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.Water_Bulb; obj.direction = Direction.Down; obj.posX = 24; obj.posY = 248; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.Water_Bulb; obj.direction = Direction.Down; obj.posX = 8; obj.posY = 296; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.Water_Bulb; obj.direction = Direction.Down; obj.posX = 104; obj.posY = 296; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.Water_Bulb; obj.direction = Direction.Down; obj.posX = 200; obj.posY = 296; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.Water_Bulb; obj.direction = Direction.Down; obj.posX = 296; obj.posY = 296; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.Water_Bulb; obj.direction = Direction.Down; obj.posX = 280; obj.posY = 216; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.Water_Bulb; obj.direction = Direction.Down; obj.posX = 296; obj.posY = 168; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.Water_Bulb; obj.direction = Direction.Down; obj.posX = 280; obj.posY = 88; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.Water_Bulb; obj.direction = Direction.Down; obj.posX = 296; obj.posY = 24; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.Water_Bulb; obj.direction = Direction.Down; obj.posX = 216; obj.posY = 8; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.Water_Bulb; obj.direction = Direction.Down; obj.posX = 120; obj.posY = 8; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.Water_SmPlant; obj.direction = Direction.Down; obj.posX = 264; obj.posY = 8; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.Water_SmPlant; obj.direction = Direction.Down; obj.posX = 184; obj.posY = 232; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.Water_RockSm; obj.direction = Direction.Down; obj.posX = 184; obj.posY = 40; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.Water_RockSm; obj.direction = Direction.Down; obj.posX = 56; obj.posY = 152; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.Water_RockMed; obj.direction = Direction.Down; obj.posX = 216; obj.posY = 152; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.Water_RockMed; obj.direction = Direction.Down; obj.posX = 216; obj.posY = 248; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.Boat_Anchor; obj.direction = Direction.Down; obj.posX = 104; obj.posY = 216; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.Bush; obj.direction = Direction.Down; obj.posX = 88; obj.posY = 216; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.Bush; obj.direction = Direction.Down; obj.posX = 88; obj.posY = 232; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.Bush; obj.direction = Direction.Down; obj.posX = 88; obj.posY = 248; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.Bush; obj.direction = Direction.Down; obj.posX = 104; obj.posY = 248; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.Bush; obj.direction = Direction.Down; obj.posX = 56; obj.posY = 136; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.Bush; obj.direction = Direction.Down; obj.posX = 200; obj.posY = 56; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.Bush; obj.direction = Direction.Down; obj.posX = 200; obj.posY = 72; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.Bush; obj.direction = Direction.Down; obj.posX = 88; obj.posY = 56; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.Bush; obj.direction = Direction.Down; obj.posX = 248; obj.posY = 136; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.Bush; obj.direction = Direction.Down; obj.posX = 248; obj.posY = 168; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.Water_LillyPad_Mini; obj.direction = Direction.Down; obj.posX = 152; obj.posY = 40; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.Water_LillyPad_Mini; obj.direction = Direction.Down; obj.posX = 104; obj.posY = 136; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.Water_LillyPad_Mini; obj.direction = Direction.Down; obj.posX = 216; obj.posY = 184; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.Water_LillyPad_Mini; obj.direction = Direction.Down; obj.posX = 184; obj.posY = 120; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.Water_LillyPad_Mini; obj.direction = Direction.Down; obj.posX = 136; obj.posY = 168; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.Water_LillyPad_Mini; obj.direction = Direction.Down; obj.posX = 232; obj.posY = 120; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.Water_LillyPad_Mini; obj.direction = Direction.Down; obj.posX = 296; obj.posY = 72; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.Water_LillyPad_Mini; obj.direction = Direction.Down; obj.posX = 168; obj.posY = 248; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.Water_LillyPad_Mini; obj.direction = Direction.Down; obj.posX = 72; obj.posY = 168; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.Water_LillyPad_Mini; obj.direction = Direction.Down; obj.posX = 24; obj.posY = 88; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.Water_LillyPad_Mini; obj.direction = Direction.Down; obj.posX = 120; obj.posY = 72; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.Water_LillyPad_Mini; obj.direction = Direction.Down; obj.posX = 264; obj.posY = 296; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.Water_LillyPad_Mini; obj.direction = Direction.Down; obj.posX = 72; obj.posY = 280; dataRef.ints.Add(obj);}
			{IntObjXmlData obj = new IntObjXmlData(); obj.type = InteractiveType.Water_LillyPad_Mini; obj.direction = Direction.Down; obj.posX = 8; obj.posY = 280; dataRef.ints.Add(obj);}
			//add the ref to the data list
			Data.Add(dataRef);
			#endregion

		}
	}
}