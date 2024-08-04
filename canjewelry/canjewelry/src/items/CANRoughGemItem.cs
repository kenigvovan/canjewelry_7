﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vintagestory.API.Client;
using Vintagestory.API.Common;
using Vintagestory.API.Config;

namespace canjewelry.src.items
{
    public class CANRoughGemItem: Item
    {
        public override void GetHeldItemInfo(ItemSlot inSlot, StringBuilder dsc, IWorldAccessor world, bool withDebugInfo)
        {
            base.GetHeldItemInfo(inSlot, dsc, world, withDebugInfo);
            if (inSlot.Itemstack.Collectible.Attributes.KeyExists("canGemTypeToAttribute"))
            {
                string buffName = inSlot.Itemstack.Collectible.Attributes["canGemTypeToAttribute"].ToString();
                if (buffName.Equals("maxhealthExtraPoints"))
                {
                    if (canjewelry.config.gems_buffs.TryGetValue(buffName, out var buffValuesDict))
                    {
                        dsc.Append(Lang.Get("canjewelry:buff-name-" + buffName)).Append(" +" + buffValuesDict[inSlot.Itemstack.Collectible.Attributes["canGemType"].AsInt().ToString()]);
                    }
                }
                else
                {
                    if (canjewelry.config.gems_buffs.TryGetValue(buffName, out var buffValuesDict))
                    {
                        float buffValue = buffValuesDict[inSlot.Itemstack.Collectible.Attributes["canGemType"].AsInt().ToString()] * 100;
                        dsc.Append(Lang.Get("canjewelry:buff-name-" + buffName));
                        dsc.Append(buffValue > 0 ? " +" + Math.Round(buffValue) + "%" : " " + Math.Round(buffValue) + "%");
                    }
                }
            }
            dsc.AppendLine();
            dsc.Append(Lang.Get("canjewelry:need_to_be_processed"));
        }
    }

}
