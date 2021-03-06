﻿using System.Collections.Generic;
using ElvenCurse.Core.Model.Creatures.Npcs;
using ElvenCurse.Core.Model.Items;

namespace ElvenCurse.Core.Interfaces
{
    public interface IItemsService
    {
        List<Item> GetItems();
        Item GetItem(int id);
        int SaveItem(Item modelItem);
        CharacterEquipment ReloadCharacterEquipment(CharacterEquipment deserializeObject);
    }
}
