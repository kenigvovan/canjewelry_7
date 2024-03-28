﻿using canjewelry.src.utils;
using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Runtime.Intrinsics.X86;
using System.Text;
using System.Threading.Channels;
using System.Threading.Tasks;
using Vintagestory.API.Common;

namespace canjewelry.src
{
    public class Config
    {
        public float grindTimeOneTick = 3;

        public Dictionary<string, HashSet<string>> buffNameToPossibleItem = new Dictionary<string, HashSet<string>>
        (new Dictionary<string, HashSet<string>> {
            {"diamond", new HashSet<string>{ "brigandine", "plate", "chain", "scale", "cansimplenecklace", "cantiara" } },
            {"corundum", new HashSet<string>{ "pickaxe", "shovel", "cansimplenecklace", "cantiara", "tunneler" } },
            {"emerald", new HashSet<string>{ "brigandine", "plate", "chain", "scale", "cansimplenecklace", "-antique" , "cantiara" } },
            {"fluorite", new HashSet<string>{ "halberd", "mace", "spear", "rapier", "longsword", "zweihander", "messer", "falx",
                "cansimplenecklace", "cantiara", "ihammer", "tshammer", "biaxe", "tssword", "shammer", "hamb", "atgeir" } },

            {"lapislazuli", new HashSet<string>{ "brigandine", "plate", "chain", "scale", "cansimplenecklace", "-antique", "cantiara" } },
            {"malachite", new HashSet<string>{ "brigandine", "plate", "chain", "scale", "knife", "cansimplenecklace", "-antique" , "cantiara", "scythe" } },
            {"olivine", new HashSet<string>{ "brigandine", "plate", "chain", "scale", "cansimplenecklace", "-antique" , "cantiara" } },
            {"uranium", new HashSet<string>{ "brigandine", "plate", "chain", "scale", "cansimplenecklace" , "-antique" , "cantiara" } },
            {"quartz", new HashSet<string>{ "pickaxe", "cansimplenecklace" , "cantiara", "tspaxel", "tunneler" } },
            {"ruby",  new HashSet<string>{ "bow", "cansimplenecklace", "cantiara", "tbow-compound"  }},
            {"citrine",  new HashSet<string>{ "knife", "cansimplenecklace" , "cantiara" }},

            {"berylaquamarine", new HashSet<string>{ "brigandine", "plate", "chain", "scale", "cansimplenecklace", "cantiara" } },
            {"berylbixbite", new HashSet<string>{ "pickaxe", "shovel", "cansimplenecklace", "cantiara", "tunneler" } },
            {"corundumruby",  new HashSet<string>{ "bow", "cansimplenecklace", "cantiara", "tbow-compound"  }},
            {"corundumsapphire", new HashSet<string>{ "pickaxe", "cansimplenecklace", "cantiara", "tunneler" } },
            {"garnetalmandine",  new HashSet<string>{ "bow", "cansimplenecklace", "cantiara", "tspaxel", "tbow-compound"  }},
            {"garnetandradite", new HashSet<string>{ "brigandine", "plate", "chain", "scale", "cansimplenecklace" , "-antique" , "cantiara" } },
            {"garnetgrossular", new HashSet<string>{ "halberd", "mace", "spear", "rapier", "longsword", "zweihander", "messer", "falx", "cansimplenecklace",
                "cantiara", "ihammer", "tshammer", "biaxe", "tssword", "shammer", "hamb", "atgeir"} },
            {"garnetpyrope",  new HashSet<string>{ "knife", "cansimplenecklace" , "cantiara" }},
            {"garnetspessartine",  new HashSet<string>{ "knife", "cansimplenecklace" , "cantiara" }},
            {"garnetuvarovite",  new HashSet<string>{ "bow", "cansimplenecklace", "cantiara"  }},
            {"spinelred", new HashSet<string>{ "brigandine", "plate", "chain", "scale", "cansimplenecklace", "-antique" , "cantiara" } },
            {"topazamber", new HashSet<string>{ "brigandine", "plate", "chain", "scale", "knife", "cansimplenecklace", "-antique" , "cantiara" } },
            {"topazblue", new HashSet<string>{ "brigandine", "plate", "chain", "scale", "cansimplenecklace", "-antique" , "cantiara" } },
            {"topazpink",  new HashSet<string>{ "knife", "cansimplenecklace" , "cantiara" }},
            {"tourmalinerubellite", new HashSet<string>{ "brigandine", "plate", "chain", "scale", "cansimplenecklace", "-antique" , "cantiara" } },
            {"tourmalineschorl", new HashSet<string>{ "halberd", "mace", "spear", "rapier", "longsword", "zweihander", "messer", "falx",
                "cansimplenecklace", "cantiara", "ihammer", "tshammer", "biaxe", "tssword", "shammer", "hamb", "atgeir" } },
            {"tourmalineverdelite", new HashSet<string>{ "brigandine", "plate", "chain", "scale", "cansimplenecklace" , "-antique" , "cantiara" } },
            {"tourmalinewatermelon",  new HashSet<string>{ "bow", "cansimplenecklace", "cantiara", "tbow-compound" }},


        });
        public Dictionary<string, Dictionary<string, float>> gems_buffs = new Dictionary<string, Dictionary<string, float>>
            (new Dictionary<string, Dictionary<string, float>>  {
                { "walkspeed", new Dictionary<string, float>{
                    { "1", 0.02f },
                    { "2", 0.04f },
                    { "3", 0.08f }
                    }
                },
                { "miningSpeedMul", new Dictionary<string, float>{
                    { "1", 0.03f },
                    { "2", 0.06f },
                    { "3", 0.09f }
                    }
                },
                { "maxhealthExtraPoints", new Dictionary<string, float>{
                    { "1", 1 },
                    { "2", 2 },
                    { "3", 4 }
                    }
                },
                { "meleeWeaponsDamage", new Dictionary<string, float>{
                    { "1", 0.03f },
                    { "2", 0.05f },
                    { "3", 0.08f }
                    }
                },
                { "hungerrate", new Dictionary<string, float>{
                    { "1", -0.03f },
                    { "2", -0.06f },
                    { "3", -0.1f }
                    }
                },
                { "wildCropDropRate", new Dictionary<string, float>{
                    { "1", 0.02f },
                    { "2", 0.05f },
                    { "3", 0.09f }
                    }
                },
                { "armorDurabilityLoss", new Dictionary<string, float>{
                    { "1", 0.05f },
                    { "2", 0.1f },
                    { "3", 0.15f }
                    }
                },
                { "oreDropRate", new Dictionary<string, float>{
                    { "1", 0.05f },
                    { "2", 0.08f },
                    { "3", 0.11f }
                    }
                },
                { "healingeffectivness", new Dictionary<string, float>{
                    { "1", 0.05f },
                    { "2", 0.1f },
                    { "3", 0.12f }
                    }
                },
                { "rangedWeaponsDamage", new Dictionary<string, float>{
                    { "1", 0.02f },
                    { "2", 0.04f },
                    { "3", 0.09f }
                    }
                },
                { "animalLootDropRate", new Dictionary<string, float>{
                    { "1", 0.02f },
                    { "2", 0.04f },
                    { "3", 0.09f }
                    }
                },

                { "vesselContentsDropRate", new Dictionary<string, float>{
                    { "1", 0.02f },
                    { "2", 0.04f },
                    { "3", 0.09f }
                    }
                },

                { "animalseekingrange", new Dictionary<string, float>{
                    { "1", -0.03f },
                    { "2", -0.05f },
                    { "3", -0.010f }
                    }
                },
                { "rangedWeaponsSpeed", new Dictionary<string, float>{
                    { "1", 0.02f },
                    { "2", 0.05f },
                    { "3", 0.08f }
                    }
                },
                { "animalharvestingtime", new Dictionary<string, float>{
                    { "1", -0.02f },
                    { "2", -0.05f },
                    { "3", -0.08f }
                    }
                },
                { "mechanicalsDamage", new Dictionary<string, float>{
                    { "1", 0.02f },
                    { "2", 0.04f },
                    { "3", 0.08f }
                    }
                },
                { "rangedWeaponsAcc", new Dictionary<string, float>{
                    { "1", 0.02f },
                    { "2", 0.04f },
                    { "3", 0.07f }
                    }
                },
                { "armorWalkSpeedAffectedness", new Dictionary<string, float>{
                    { "1", -0.02f },
                    { "2", -0.04f },
                    { "3", -0.07f }
                    }
                },
                { "bowDrawingStrength", new Dictionary<string, float>{
                    { "1", 0.01f },
                    { "2", 0.03f },
                    { "3", 0.06f }
                    }
                }
            });

        public Dictionary<string, int> items_codes_with_socket_count = new Dictionary<string, int>(new Dictionary<string, int>
        {

        });
        public Dictionary<string, int[]> items_codes_with_socket_count_and_tiers = new Dictionary<string, int[]>()
        {
             { "canjewelry:cansimplenecklace-*", new int[1] {1} },
            { "canjewelry:cantiara-*", new int[3] {1, 3, 1} },
            { "*knife-generic-gold", new int[1] {3} },
            {  "*knife-generic-silver", new int[1] {3} },
            { "*knife-generic-iron",  new int[1] {3} },
            { "*knife-generic-meteoriciron", new int[2] {3, 3} },
            {  "*knife-generic-steel", new int[3] {3, 3, 3} },

            {  "*pickaxe-blackbronze", new int[1] {3} },
            {  "*pickaxe-tinbronze", new int[1] {3} },
            {  "*pickaxe-bismuthbronze", new int[1] {3} },
            {  "*pickaxe-gold", new int[1] {3} },
            {  "*pickaxe-silver", new int[1] {3} },
            {  "*pickaxe-iron", new int[2] {3, 3} },
            {  "*pickaxe-meteoriciron", new int[2] {3, 3} },
            {  "*pickaxe-steel", new int[3] {3, 3, 3} },

            {  "*scythe-blackbronze", new int[1] {3} },
            {  "*scythe-tinbronze", new int[1] {3} },
            {  "*scythe-bismuthbronze", new int[1] {3} },
            {  "*scythe-gold", new int[1] {3} },
            {  "*scythe-iron", new int[2] {3, 3} },
            {  "*scythe-meteoriciron", new int[2] {3, 3} },
            {  "*scythe-steel", new int[3] {3, 3, 3} },

            {  "*shovel-blackbronze", new int[1] {3} },
            {  "*shovel-tinbronze", new int[1] {3} },
            {  "*shovel-bismuthbronze", new int[1] {3} },
            {  "*shovel-gold", new int[1] {3} },
            {  "*shovel-iron", new int[2] {3, 3} },
            {  "*shovel-meteoriciron", new int[2] {3, 3} },
            {  "*shovel-steel", new int[3] {3, 3, 3} },

            {  "armor-head-brigandine-*", new int[1] {3} },
            {  "armor-legs-brigandine-*", new int[1] {3} },
            {  "armor-body-brigandine-*", new int[2] {3, 3} },


            {  "armor-head-plate-*", new int[1] {3} },
            {  "armor-legs-plate-*", new int[1] {3} },
            {  "armor-body-plate-*", new int[2] {3, 3} },

            {  "armor-head-scale-*", new int[1] {3} },
            {  "armor-legs-scale-*", new int[1] {3} },
            {  "armor-body-scale-*", new int[2] {3, 3} },


            {  "armor-head-chain-*", new int[1] {3} },
            {  "armor-legs-chain-*", new int[1] {3} },
            {  "armor-body-chain-*", new int[2] {3, 3} },

            {  "armor-head-antique-*", new int[1] {3} },
            {  "armor-legs-antique-*", new int[1] {3} },
            {  "armor-body-antique-*", new int[2] {3, 3} },

            {  "xmelee:xzweihander-meteoriciron", new int[1] {3} },
            {  "xmelee:xzweihander-steel", new int[2] {3, 3} },

            {  "xmelee:xlongsword-meteoriciron", new int[1] {3} },
            {  "xmelee:xlongsword-steel", new int[2] {3, 3} },


            {  "xmelee:xhalberd-meteoriciron", new int[1] {3} },
            {  "xmelee:xhalberd-steel", new int[2] {3, 3} },


            {  "xmelee:xmesser-meteoriciron", new int[1] {3} },
            {  "xmelee:xmesser-steel", new int[2] {3, 3} },

            {  "xmelee:xmace-meteoriciron", new int[1] {3} },
            {  "xmelee:xmace-steel", new int[2] {3, 3} },

            {  "xmelee:xpike-meteoriciron", new int[1] {3} },
            {  "xmelee:xpike-steel", new int[2] {3, 3} },

            {  "xmelee:xrapier-meteoriciron", new int[1] {3} },
            {  "xmelee:xrapier-steel", new int[2] {3, 3} },


            {  "xmelee:xspear-meteoriciron", new int[1] {3} },
            {  "xmelee:xspear-steel", new int[2] {3, 3} },

            {  "*blade-falx-gold", new int[1] {3} },
            {  "*blade-falx-silver", new int[1] {3} },
            {  "*blade-falx-iron", new int[1] {3} },
            {  "*blade-falx-meteoriciron", new int[2] {3, 3} },
            {  "*blade-falx-steel", new int[2] {3, 3} },
            {  "*blade-falx-blackguard-iron", new int[2] {3, 3} },
            {  "*blade-forlorn-iron", new int[2] {3, 3} },
            {  "*blade-longsword-admin", new int[3] {3, 3, 3} },

            {  "bow-simple", new int[1] {3} },
            {  "bow-recurve", new int[2] {3, 3} },
            {  "bow-long", new int[2] {3, 3} },

            { "tstools:ihammer",  new int[1] { 3 } },
            { "tstools:tshammer",  new int[2] {3, 3}  },
            { "tstools:tspaxel",  new int[2] {3, 3}  },
            { "tstools:tspickaxe",  new int[2] {3, 3}  },
            { "tstools:biaxe",  new int[2] {3, 3}  },
            { "tstools:tssword",  new int[2] {3, 3}  },
            { "tstools:shammer",  new int[2] {3, 3}  },
            { "tstools:hamb",  new int[2] {3, 3}  },
            { "tstools:tbow-compound",  new int[2] {3, 3}  },

            { "swordz:zweihander-iron-*",  new int[2] {3, 3}  },
            { "swordz:zweihander-meteoriciron-*",  new int[2] {3, 3}  },
            { "swordz:zweihander-steel-*",  new int[3] {3, 3, 3}  },

            { "swordz:atgeir-iron",  new int[2] {3, 3}  },
            { "swordz:atgeir-meteoriciron",  new int[2] {3, 3}  },
            { "swordz:atgeir-steel",  new int[3] {3, 3, 3}  },

            {  "swordz:armor-head-*", new int[1] {3} },
            {  "swordz:armor-legs-*", new int[1] {3} },
            {  "swordz:armor-body-*", new int[2] {3, 3} },

            { "swordz:tunneler-steel-*",  new int[2] {3, 3}  },
            { "swordz:tunneler-stainlesssteel-*",  new int[2] {3, 3}  },
            { "swordz:tunneler-titanium-*",  new int[3] {3, 3, 3}  },
            { "swordz:tunneler-mithril-*",  new int[3] {3, 3, 3}  },
            { "swordz:tunneler-adamant-*",  new int[3] {3, 3, 3}  },
            { "swordz:tunneler-orichalcum-*",  new int[3] {3, 3, 3}  },
            { "swordz:tunneler-aithril-*",  new int[4] {3, 3, 3, 3}  },

            { "swordz:pernach-iron-*",  new int[2] {3, 3}  },
            { "swordz:pernach-meteoriciron-*",  new int[2] {3, 3}  },
            { "swordz:pernach-steel-*",  new int[3] {3, 3, 3}  },

            { "swordz:warhammer-iron",  new int[2] {3, 3}  },
            { "swordz:warhammer-meteoriciron",  new int[2] {3, 3}  },
            { "swordz:warhammer-steel",  new int[3] {3, 3, 3}  },

            { "swordz:stiletto-iron",  new int[2] {3, 3}  },
            { "swordz:stiletto-meteoriciron",  new int[2] {3, 3}  },
            { "swordz:stiletto-steel",  new int[3] {3, 3, 3}  },


            { "swordz:knife-generic-stainelesssteel", new int[1] {3} },
             { "swordz:knife-generic-titanium", new int[2] {3, 3} },
             { "swordz:knife-generic-mithril", new int[2] {3, 3} },
             { "swordz:knife-generic-adamant", new int[2] {3, 3} },
             { "swordz:knife-generic-orichalcum", new int[2] {3, 3} },
             { "swordz:knife-generic-aithril", new int[3] {3, 3, 3} },

              { "swordz:sord-iron-*",  new int[2] {3, 3}  },
            { "swordz:sord-meteoriciron-*",  new int[2] {3, 3}  },
            { "swordz:sord-steel-*",  new int[3] {3, 3, 3}  },


            { "swordz:gladius-iron-*",  new int[2] {3, 3}  },


            { "swordz:kilij-iron-*",  new int[2] {3, 3}  },
            { "swordz:kilij-meteoriciron-*",  new int[2] {3, 3}  },
            { "swordz:kilij-steel-*",  new int[3] {3, 3, 3}  },

             { "swordz:longsword-iron-*",  new int[2] {3, 3}  },
            { "swordz:longsword-meteoriciron-*",  new int[2] {3, 3}  },
            { "swordz:longsword-steel-*",  new int[3] {3, 3, 3}  },

            { "diamondpick-steel",  new int[3] {3, 3, 3}  },
        };
        public int pan_take_per_use = 8;
        public Dictionary<string, string> gem_type_to_buff = new Dictionary<string, string>()
        {
            { "diamond", "walkspeed"},
            { "corundum", "miningSpeedMul"},
            { "emerald", "maxhealthExtraPoints"},
            { "fluorite", "meleeWeaponsDamage"},
            { "lapislazuli", "hungerrate" },
            { "malachite", "wildCropDropRate" },
            { "olivine_peridot", "armorDurabilityLoss"},
             { "olivine", "armorDurabilityLoss"},
            { "quartz", "oreDropRate"},
            { "uranium", "healingeffectivness"},
            { "ruby", "rangedWeaponsDamage"},
            { "citrine", "animalLootDropRate"},

            { "berylaquamarine",  "walkspeed"},
            { "berylbixbite",  "miningSpeedMul"},
            { "corundumruby",  "rangedWeaponsDamage"},
            { "corundumsapphire",  "vesselContentsDropRate"},
            { "garnetalmandine",  "bowDrawingStrength"},
            { "garnetandradite",  "animalseekingrange"},
            { "garnetgrossular",  "meleeWeaponsDamage"},
            { "garnetpyrope",  "animalLootDropRate"},
            { "garnetspessartine",  "animalLootDropRate"},
            { "garnetuvarovite",  "rangedWeaponsSpeed"},
            { "spinelred",  "armorWalkSpeedAffectedness"},
            { "topazamber",  "wildCropDropRate"},
            { "topazblue",  "maxhealthExtraPoints"},
            { "topazpink",  "animalharvestingtime"},
            { "tourmalinerubellite",  "armorDurabilityLoss"},
            { "tourmalineschorl",  "mechanicalsDamage"},
            { "tourmalineverdelite",  "healingeffectivness"},
            { "tourmalinewatermelon",  "rangedWeaponsAcc"}
        };

        public Dictionary<string, float> max_buff_values = new Dictionary<string, float>()
        {
            { "walkspeed", 0.5f},
            { "maxhealthExtraPoints", 25}
        };

        public Dictionary<string, DropInfo[]> gems_drops_table = new Dictionary<string, DropInfo[]>()
        {
             //malachite
            { "ore-*-malachite-*", new DropInfo[]{
               new DropInfo(EnumItemClass.Item, "canjewelry:gem-rough-chipped-malachite", 0.005f, 0, true, "canjewelrygemsdroprate"),
               new DropInfo(EnumItemClass.Item, "canjewelry:gem-rough-flawed-malachite", 0.0005f, 0, true, "canjewelrygemsdroprate"),
               new DropInfo(EnumItemClass.Item, "canjewelry:gem-rough-normal-malachite", 0.00005f, 0, true, "canjewelrygemsdroprate")
               }
            },            
             //fluorite
            { "ore-*-pentlandite-*", new DropInfo[]{
               new DropInfo(EnumItemClass.Item, "canjewelry:gem-rough-chipped-fluorite", 0.005f, 0, true, "canjewelrygemsdroprate"),
               new DropInfo(EnumItemClass.Item, "canjewelry:gem-rough-flawed-fluorite", 0.0005f, 0, true, "canjewelrygemsdroprate"),
               new DropInfo(EnumItemClass.Item, "canjewelry:gem-rough-normal-fluorite", 0.00005f, 0, true, "canjewelrygemsdroprate")
               }
            },
            //corundum
            { "ore-*-rhodochrosite-*", new DropInfo[]{
               new DropInfo(EnumItemClass.Item, "canjewelry:gem-rough-chipped-corundum", 0.005f, 0, true, "canjewelrygemsdroprate"),
               new DropInfo(EnumItemClass.Item, "canjewelry:gem-rough-flawed-corundum", 0.0005f, 0, true, "canjewelrygemsdroprate"),
               new DropInfo(EnumItemClass.Item, "canjewelry:gem-rough-normal-corundum", 0.00005f, 0, true, "canjewelrygemsdroprate")
               }
            },
            //quartz
            { "ore-*-quartz-*", new DropInfo[]{
               new DropInfo(EnumItemClass.Item, "canjewelry:gem-rough-chipped-quartz", 0.001f, 0, true, "canjewelrygemsdroprate"),
               new DropInfo(EnumItemClass.Item, "canjewelry:gem-rough-flawed-quartz", 0.005f, 0, true, "canjewelrygemsdroprate"),
               new DropInfo(EnumItemClass.Item, "canjewelry:gem-rough-normal-quartz", 0.0005f, 0, true, "canjewelrygemsdroprate")
               }
            },
            //rocks
            { "rock-limestone", new DropInfo[]{
               new DropInfo(EnumItemClass.Item, "canjewelry:gem-rough-chipped-malachite", 0.001f, 0, true, "canjewelrygemsdroprate"),
               new DropInfo(EnumItemClass.Item, "canjewelry:gem-rough-flawed-malachite", 0.0005f, 0, true, "canjewelrygemsdroprate"),
               new DropInfo(EnumItemClass.Item, "canjewelry:gem-rough-normal-malachite", 0.00005f, 0, true, "canjewelrygemsdroprate")
               }
            },
            { "rock-granite", new DropInfo[]{
               new DropInfo(EnumItemClass.Item, "canjewelry:gem-rough-chipped-quartz", 0.001f, 0, true, "canjewelrygemsdroprate"),
               new DropInfo(EnumItemClass.Item, "canjewelry:gem-rough-flawed-quartz", 0.0005f, 0, true, "canjewelrygemsdroprate"),
               new DropInfo(EnumItemClass.Item, "canjewelry:gem-rough-normal-quartz", 0.00005f, 0, true, "canjewelrygemsdroprate"),
                new DropInfo(EnumItemClass.Item, "canjewelry:gem-rough-chipped-ruby", 0.001f, 0, true, "canjewelrygemsdroprate"),
               new DropInfo(EnumItemClass.Item, "canjewelry:gem-rough-flawed-ruby", 0.0005f, 0, true, "canjewelrygemsdroprate"),
               new DropInfo(EnumItemClass.Item, "canjewelry:gem-rough-normal-ruby", 0.00005f, 0, true, "canjewelrygemsdroprate"),
               new DropInfo(EnumItemClass.Item, "canjewelry:gem-rough-chipped-citrine", 0.001f, 0, true, "canjewelrygemsdroprate"),
               new DropInfo(EnumItemClass.Item, "canjewelry:gem-rough-flawed-citrine", 0.0005f, 0, true, "canjewelrygemsdroprate"),
               new DropInfo(EnumItemClass.Item, "canjewelry:gem-rough-normal-citrine", 0.00005f, 0, true, "canjewelrygemsdroprate")
               }
            },
            { "rock-whitemarble", new DropInfo[]{
               new DropInfo(EnumItemClass.Item, "canjewelry:gem-rough-chipped-malachite", 0.001f, 0, true, "canjewelrygemsdroprate"),
               new DropInfo(EnumItemClass.Item, "canjewelry:gem-rough-flawed-malachite", 0.0005f, 0, true, "canjewelrygemsdroprate"),
               new DropInfo(EnumItemClass.Item, "canjewelry:gem-rough-normal-malachite", 0.00005f, 0, true, "canjewelrygemsdroprate")
               }
            },
            { "rock-chalk", new DropInfo[]{
               new DropInfo(EnumItemClass.Item, "canjewelry:gem-rough-chipped-malachite", 0.001f, 0, true, "canjewelrygemsdroprate"),
               new DropInfo(EnumItemClass.Item, "canjewelry:gem-rough-flawed-malachite", 0.0005f, 0, true, "canjewelrygemsdroprate"),
               new DropInfo(EnumItemClass.Item, "canjewelry:gem-rough-normal-malachite", 0.00005f, 0, true, "canjewelrygemsdroprate")
               }
            },
            { "rock-greenmarble", new DropInfo[]{
               new DropInfo(EnumItemClass.Item, "canjewelry:gem-rough-chipped-malachite", 0.001f, 0, true, "canjewelrygemsdroprate"),
               new DropInfo(EnumItemClass.Item, "canjewelry:gem-rough-flawed-malachite", 0.0005f, 0, true, "canjewelrygemsdroprate"),
               new DropInfo(EnumItemClass.Item, "canjewelry:gem-rough-normal-malachite", 0.00005f, 0, true, "canjewelrygemsdroprate")
               }
            },
            { "rock-kimberlite", new DropInfo[]{
               new DropInfo(EnumItemClass.Item, "canjewelry:gem-rough-chipped-diamond", 0.001f, 0, true, "canjewelrygemsdroprate"),
               new DropInfo(EnumItemClass.Item, "canjewelry:gem-rough-flawed-diamond", 0.0005f, 0, true, "canjewelrygemsdroprate"),
               new DropInfo(EnumItemClass.Item, "canjewelry:gem-rough-normal-diamond", 0.00005f, 0, true, "canjewelrygemsdroprate"),
               new DropInfo(EnumItemClass.Item, "canjewelry:gem-rough-chipped-ruby", 0.001f, 0, true, "canjewelrygemsdroprate"),
               new DropInfo(EnumItemClass.Item, "canjewelry:gem-rough-flawed-ruby", 0.0005f, 0, true, "canjewelrygemsdroprate"),
               new DropInfo(EnumItemClass.Item, "canjewelry:gem-rough-normal-ruby", 0.00005f, 0, true, "canjewelrygemsdroprate")
               }
            },
            { "rock-suevite", new DropInfo[]{
               new DropInfo(EnumItemClass.Item, "canjewelry:gem-rough-chipped-diamond", 0.001f, 0, true, "canjewelrygemsdroprate"),
               new DropInfo(EnumItemClass.Item, "canjewelry:gem-rough-flawed-diamond", 0.0005f, 0, true, "canjewelrygemsdroprate"),
               new DropInfo(EnumItemClass.Item, "canjewelry:gem-rough-normal-diamond", 0.00005f, 0, true, "canjewelrygemsdroprate")
               }
            },
            { "rock-phyllite", new DropInfo[]{
               new DropInfo(EnumItemClass.Item, "canjewelry:gem-rough-chipped-corundum", 0.001f, 0, true, "canjewelrygemsdroprate"),
               new DropInfo(EnumItemClass.Item, "canjewelry:gem-rough-flawed-corundum", 0.0005f, 0, true, "canjewelrygemsdroprate"),
               new DropInfo(EnumItemClass.Item, "canjewelry:gem-rough-normal-corundum", 0.00005f, 0, true, "canjewelrygemsdroprate")
               }
            },
            { "rock-shale", new DropInfo[]{
               new DropInfo(EnumItemClass.Item, "canjewelry:gem-rough-chipped-emerald", 0.001f, 0, true, "canjewelrygemsdroprate"),
               new DropInfo(EnumItemClass.Item, "canjewelry:gem-rough-flawed-emerald", 0.0005f, 0, true, "canjewelrygemsdroprate"),
               new DropInfo(EnumItemClass.Item, "canjewelry:gem-rough-normal-emerald", 0.00005f, 0, true, "canjewelrygemsdroprate")
               }
            },
            { "rock-slate", new DropInfo[]{
               new DropInfo(EnumItemClass.Item, "canjewelry:gem-rough-chipped-fluorite", 0.001f, 0, true, "canjewelrygemsdroprate"),
               new DropInfo(EnumItemClass.Item, "canjewelry:gem-rough-flawed-fluorite", 0.0005f, 0, true, "canjewelrygemsdroprate"),
               new DropInfo(EnumItemClass.Item, "canjewelry:gem-rough-normal-fluorite", 0.00005f, 0, true, "canjewelrygemsdroprate")
               }
            },
            { "rock-claystone", new DropInfo[]{
               new DropInfo(EnumItemClass.Item, "canjewelry:gem-rough-chipped-quartz", 0.001f, 0, true, "canjewelrygemsdroprate"),
               new DropInfo(EnumItemClass.Item, "canjewelry:gem-rough-flawed-quartz", 0.0005f, 0, true, "canjewelrygemsdroprate"),
               new DropInfo(EnumItemClass.Item, "canjewelry:gem-rough-normal-quartz", 0.00005f, 0, true, "canjewelrygemsdroprate")
               }
            },
            { "rock-andesite", new DropInfo[]{
               new DropInfo(EnumItemClass.Item, "canjewelry:gem-rough-chipped-uranium", 0.001f, 0, true, "canjewelrygemsdroprate"),
               new DropInfo(EnumItemClass.Item, "canjewelry:gem-rough-flawed-uranium", 0.0005f, 0, true, "canjewelrygemsdroprate"),
               new DropInfo(EnumItemClass.Item, "canjewelry:gem-rough-normal-uranium", 0.00005f, 0, true, "canjewelrygemsdroprate")
               }
            },
            { "rock-sandstone", new DropInfo[]{
               new DropInfo(EnumItemClass.Item, "canjewelry:gem-rough-chipped-olivine", 0.001f, 0, true, "canjewelrygemsdroprate"),
               new DropInfo(EnumItemClass.Item, "canjewelry:gem-rough-flawed-olivine", 0.0005f, 0, true, "canjewelrygemsdroprate"),
               new DropInfo(EnumItemClass.Item, "canjewelry:gem-rough-normal-olivine", 0.00005f, 0, true, "canjewelrygemsdroprate")
               }
            },
            { "rock-conglomerate", new DropInfo[]{
               new DropInfo(EnumItemClass.Item, "canjewelry:gem-rough-chipped-fluorite", 0.001f, 0, true, "canjewelrygemsdroprate"),
               new DropInfo(EnumItemClass.Item, "canjewelry:gem-rough-flawed-fluorite", 0.0005f, 0, true, "canjewelrygemsdroprate"),
               new DropInfo(EnumItemClass.Item, "canjewelry:gem-rough-normal-fluorite", 0.00005f, 0, true, "canjewelrygemsdroprate")
               }
            },
            { "rock-chert", new DropInfo[]{
               new DropInfo(EnumItemClass.Item, "canjewelry:gem-rough-chipped-uranium", 0.001f, 0, true, "canjewelrygemsdroprate"),
               new DropInfo(EnumItemClass.Item, "canjewelry:gem-rough-flawed-uranium", 0.0005f, 0, true, "canjewelrygemsdroprate"),
               new DropInfo(EnumItemClass.Item, "canjewelry:gem-rough-normal-uranium", 0.00005f, 0, true, "canjewelrygemsdroprate"),
                new DropInfo(EnumItemClass.Item, "canjewelry:gem-rough-chipped-citrine", 0.001f, 0, true, "canjewelrygemsdroprate"),
               new DropInfo(EnumItemClass.Item, "canjewelry:gem-rough-flawed-citrine", 0.0005f, 0, true, "canjewelrygemsdroprate"),
               new DropInfo(EnumItemClass.Item, "canjewelry:gem-rough-normal-citrine", 0.00005f, 0, true, "canjewelrygemsdroprate")
               }
            },
            { "rock-basalt", new DropInfo[]{
               new DropInfo(EnumItemClass.Item, "canjewelry:gem-rough-chipped-quartz", 0.001f, 0, true, "canjewelrygemsdroprate"),
               new DropInfo(EnumItemClass.Item, "canjewelry:gem-rough-flawed-quartz", 0.0005f, 0, true, "canjewelrygemsdroprate"),
               new DropInfo(EnumItemClass.Item, "canjewelry:gem-rough-normal-quartz", 0.00005f, 0, true, "canjewelrygemsdroprate")
               }
            },
            { "rock-peridotite", new DropInfo[]{
               new DropInfo(EnumItemClass.Item, "canjewelry:gem-rough-chipped-olivine", 0.001f, 0, true, "canjewelrygemsdroprate"),
               new DropInfo(EnumItemClass.Item, "canjewelry:gem-rough-flawed-olivine", 0.0005f, 0, true, "canjewelrygemsdroprate"),
               new DropInfo(EnumItemClass.Item, "canjewelry:gem-rough-normal-olivine", 0.00005f, 0, true, "canjewelrygemsdroprate")
               }
            },
             { "rock-bauxite", new DropInfo[]{
               new DropInfo(EnumItemClass.Item, "canjewelry:gem-rough-chipped-lapislazuli", 0.001f, 0, true, "canjewelrygemsdroprate"),
               new DropInfo(EnumItemClass.Item, "canjewelry:gem-rough-flawed-lapislazuli", 0.0005f, 0, true, "canjewelrygemsdroprate"),
               new DropInfo(EnumItemClass.Item, "canjewelry:gem-rough-normal-lapislazuli", 0.00005f, 0, true, "canjewelrygemsdroprate")
               }
            },
           { "ore-lapislazuli-*", new DropInfo[]{
               new DropInfo(EnumItemClass.Item, "canjewelry:gem-rough-chipped-lapislazuli", 0.01f, 0, true, "canjewelrygemsdroprate"),
               new DropInfo(EnumItemClass.Item, "canjewelry:gem-rough-flawed-lapislazuli", 0.005f, 0, true, "canjewelrygemsdroprate"),
               new DropInfo(EnumItemClass.Item, "canjewelry:gem-rough-normal-lapislazuli", 0.0005f, 0, true, "canjewelrygemsdroprate")
               }
            },
           { "ore-quartz-*", new DropInfo[]{
               new DropInfo(EnumItemClass.Item, "canjewelry:gem-rough-chipped-quartz", 0.01f, 0, true, "canjewelrygemsdroprate"),
               new DropInfo(EnumItemClass.Item, "canjewelry:gem-rough-flawed-quartz", 0.005f, 0, true, "canjewelrygemsdroprate"),
               new DropInfo(EnumItemClass.Item, "canjewelry:gem-rough-normal-quartz", 0.0005f, 0, true, "canjewelrygemsdroprate")
               }
            },
           { "ore-fluorite-*", new DropInfo[]{
               new DropInfo(EnumItemClass.Item, "canjewelry:gem-rough-chipped-fluorite", 0.01f, 0, true, "canjewelrygemsdroprate"),
               new DropInfo(EnumItemClass.Item, "canjewelry:gem-rough-flawed-fluorite", 0.005f, 0, true, "canjewelrygemsdroprate"),
               new DropInfo(EnumItemClass.Item, "canjewelry:gem-rough-normal-fluorite", 0.0005f, 0, true, "canjewelrygemsdroprate")
               }
            },
        };
        
        public bool debugMode = false;

        public class DropInfo
        {
            public EnumItemClass TypeCollectable;
            public string NameCollectable;
            public float avg;
            public float var;
            public bool LastDrop;
            public string DropModbyStat;
            public string attributes;

            public DropInfo(EnumItemClass TypeCollectable, string NameCollectable, float avg, float var, bool LastDrop, string DropModbyStat = "", string attributes ="")
            {
                this.TypeCollectable = TypeCollectable;
                this.NameCollectable = NameCollectable;
                this.avg = avg;
                this.var = var;
                this.LastDrop = LastDrop;
                this.DropModbyStat = DropModbyStat;
                this.attributes = attributes;
            }
        }
    }
}
