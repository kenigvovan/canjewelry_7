﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Intrinsics.Arm;
using System.Text;
using System.Threading.Tasks;
using canjewelry.src.inventories;
using Vintagestory.API.Client;
using Vintagestory.API.Common;
using Vintagestory.API.Config;
using Vintagestory.API.Datastructures;
using Vintagestory.API.MathTools;
using Vintagestory.API.Server;
using Vintagestory.API.Util;
using Vintagestory.Client.NoObf;
using Vintagestory.GameContent;
using Vintagestory.GameContent.Mechanics;
using static canjewelry.src.Config;

namespace canjewelry.src.jewelry
{
    public class BEJewelGrinder : BlockEntityContainer, ITexPositionSource
    {
        private static SimpleParticleProperties FlourParticles = new SimpleParticleProperties(1f, 3f, ColorUtil.ToRgba(40, 220, 220, 220), new Vec3d(), new Vec3d(), new Vec3f(-0.25f, -0.25f, -0.25f), new Vec3f(0.25f, 0.25f, 0.25f), minSize: 0.1f, maxSize: 0.3f, model: EnumParticleModel.Quad);
        private static SimpleParticleProperties FlourDustParticles;
        public static Dictionary<string, int> gemTypeToColor = new Dictionary<string, int>();
        private Dictionary<string, AssetLocation> tmpTextures = new Dictionary<string, AssetLocation>();
        private ILoadedSound ambientSound;
        internal InventoryJewelGrinder inventory;
        private JewelGrinderTopRenderer renderer;
        private bool automated;
        private BEBehaviorMPConsumer mpc;
        private float prevSpeed = float.NaN;
        private Dictionary<string, float> playersGrinding = new Dictionary<string, float>();
        private int quantityPlayersGrinding;
        private int nowOutputFace;
        private bool beforeGrinding;
        private ITexPositionSource blockTexSource;
        public string Material => this.Block.LastCodePart();
        public float GrindSpeed
        {
            get
            {
                /* if (this.quantityPlayersGrinding > 0)
                     return 1f;*/
                return this.automated && this.mpc.Network != null ? this.mpc.TrueSpeed : 0.0f;
            }
        }
        private MeshData quernBaseMesh
        {
            get
            {
                object quernBaseMesh;
                this.Api.ObjectCache.TryGetValue("jewerlgrinder-" + this.Material, out quernBaseMesh);
                return (MeshData)quernBaseMesh;
            }
            set => this.Api.ObjectCache["jewerlgrinder-" + this.Material] = (object)value;
        }
        private MeshData quernTopMesh
        {
            get
            {
                object quernTopMesh = (object)null;
                this.Api.ObjectCache.TryGetValue("jewerlgrinder-top" + this.Material, out quernTopMesh);
                return (MeshData)quernTopMesh;
            }
            set => this.Api.ObjectCache["jewerlgrinder-top" + this.Material] = (object)value;
        }
        public virtual float maxGrindingTime() => 4f;
        public override string InventoryClassName => "jewelgrinder";
        public virtual string DialogTitle => Lang.Get("Jewel Grinder");
        public override InventoryBase Inventory => (InventoryBase)this.inventory;
        public ItemSlot InputSlot => this.inventory[0];

        public ItemSlot OutputSlot => this.inventory[1];

        public ItemStack InputStack
        {
            get => this.inventory[0].Itemstack;
            set
            {
                this.inventory[0].Itemstack = value;
                this.inventory[0].MarkDirty();
            }
        }

        public ItemStack OutputStack
        {
            get => this.inventory[1].Itemstack;
            set
            {
                this.inventory[1].Itemstack = value;
                this.inventory[1].MarkDirty();
            }
        }

        public GrindingProperties InputGrindProps
        {
            get
            {
                ItemSlot itemSlot = this.inventory[0];
                return itemSlot.Itemstack == null ? (GrindingProperties)null : itemSlot.Itemstack.Collectible.GrindingProps;
            }
        }

        public Size2i AtlasSize => (this.Api as ICoreClientAPI).BlockTextureAtlas.Size;

        public TextureAtlasPosition this[string textureCode]
        {
            get
            {
                CompositeTexture compositeTexture;
                if (textureCode == "layer" && this.inventory[0].Itemstack != null)
                {
                    return this.getOrCreateTexPos(this.tmpTextures[textureCode]);
                }
                //var a = this.inventory[0].Itemstack.Item.Textures.TryGetValue(textureCode, out compositeTexture);
                //var f = (this.Api as ICoreClientAPI).BlockTextureAtlas[compositeTexture.Base];
                //var c = this.blockTexSource[textureCode];
                //if(this.inventory[0].Itemstack != null && this.inventory[0].Itemstack.Item != null)
                //textureCode = "metal";


                return textureCode == "steel" && this.inventory[0].Itemstack != null && this.inventory[0].Itemstack.Item != null && this.inventory[0].Itemstack.Item.Textures.TryGetValue("metal", out compositeTexture)
                    ? (this.Api as ICoreClientAPI).ItemTextureAtlas[compositeTexture.Base]
                    : this.blockTexSource[textureCode];
            }
        }
        static BEJewelGrinder()
        {
            BEJewelGrinder.FlourParticles.AddPos.Set(17.0 / 16.0, 0.0, 17.0 / 16.0);
            BEJewelGrinder.FlourParticles.AddQuantity = 20f;
            BEJewelGrinder.FlourParticles.MinVelocity.Set(-0.25f, 0.0f, -0.25f);
            BEJewelGrinder.FlourParticles.AddVelocity.Set(0.5f, 1f, 0.5f);
            BEJewelGrinder.FlourParticles.WithTerrainCollision = true;
            BEJewelGrinder.FlourParticles.ParticleModel = EnumParticleModel.Cube;
            BEJewelGrinder.FlourParticles.LifeLength = 1.5f;
            BEJewelGrinder.FlourParticles.SizeEvolve = EvolvingNatFloat.create(EnumTransformFunction.QUADRATIC, -0.4f);
            BEJewelGrinder.FlourDustParticles = new SimpleParticleProperties(1f, 3f, ColorUtil.ToRgba(40, 220, 220, 220), new Vec3d(), new Vec3d(), new Vec3f(-0.25f, -0.25f, -0.25f), new Vec3f(0.25f, 0.25f, 0.25f), minSize: 0.1f, maxSize: 0.3f, model: EnumParticleModel.Quad);
            BEJewelGrinder.FlourDustParticles.AddPos.Set(17.0 / 16.0, 0.0, 17.0 / 16.0);
            BEJewelGrinder.FlourDustParticles.AddQuantity = 5f;
            BEJewelGrinder.FlourDustParticles.MinVelocity.Set(-0.05f, 0.0f, -0.05f);
            BEJewelGrinder.FlourDustParticles.AddVelocity.Set(0.1f, 0.2f, 0.1f);
            BEJewelGrinder.FlourDustParticles.WithTerrainCollision = false;
            BEJewelGrinder.FlourDustParticles.ParticleModel = EnumParticleModel.Quad;
            BEJewelGrinder.FlourDustParticles.LifeLength = 1.5f;
            BEJewelGrinder.FlourDustParticles.SelfPropelled = true;
            BEJewelGrinder.FlourDustParticles.GravityEffect = 0.0f;
            BEJewelGrinder.FlourDustParticles.SizeEvolve = EvolvingNatFloat.create(EnumTransformFunction.QUADRATIC, 0.4f);
            BEJewelGrinder.FlourDustParticles.OpacityEvolve = EvolvingNatFloat.create(EnumTransformFunction.QUADRATIC, -16f);
        }
        public BEJewelGrinder()
        {
            this.inventory = new InventoryJewelGrinder((string)null, (ICoreAPI)null);
            this.inventory.SlotModified += new Action<int>(this.OnSlotModifid);
            this.inventory[0].MaxSlotStackSize = 1;
        }
        public override void Initialize(ICoreAPI api)
        {
            base.Initialize(api);
            this.inventory.LateInitialize("jewelgrinder-" + this.Pos.X.ToString() + "/" + this.Pos.Y.ToString() + "/" + this.Pos.Z.ToString(), api);
            
            this.RegisterGameTickListener(new Action<float>(this.Every100ms), 100);
            this.RegisterGameTickListener(new Action<float>(this.Every500ms), 500);
            if (this.ambientSound == null && api.Side == EnumAppSide.Client)
                this.ambientSound = ((IClientWorldAccessor)api.World).LoadSound(new SoundParams()
                {
                    Location = new AssetLocation("sounds/block/quern.ogg"),
                    ShouldLoop = true,
                    Position = this.Pos.ToVec3f().Add(0.5f, 0.25f, 0.5f),
                    DisposeOnFinish = false,
                    Volume = 0.75f
                });
            if (api.Side != EnumAppSide.Client)
                return;
            if ((api as ICoreClientAPI) != null)
                this.blockTexSource = (api as ICoreClientAPI).Tesselator.GetTexSource(this.Block);
           this.renderer = new JewelGrinderTopRenderer(api as ICoreClientAPI, this.Pos, this.GenMesh("top"));
           this.renderer.mechPowerPart = this.mpc;
           if (this.automated)
            {
                this.renderer.ShouldRender = true;
                this.renderer.ShouldRotateAutomated = true;
            }
          (api as ICoreClientAPI).Event.RegisterRenderer((IRenderer)this.renderer, EnumRenderStage.Opaque, "jewelgrinder");
            if (this.quernBaseMesh == null)
                this.quernBaseMesh = this.GenMesh();
            //this.inventory.SlotModified += (int sl) => { this.setRenderer(); };
            this.setRenderer();

            
        }
        public override void CreateBehaviors(Block block, IWorldAccessor worldForResolve)
        {
            base.CreateBehaviors(block, worldForResolve);
            this.mpc = this.GetBehavior<BEBehaviorMPConsumer>();
            if (this.mpc == null)
                return;
            this.mpc.OnConnected = (Action)(() =>
            {
                this.automated = true;
                this.quantityPlayersGrinding = 0;
                if (this.renderer == null)
                    return;
                this.renderer.ShouldRender = true;
                this.renderer.ShouldRotateAutomated = true;
            });
            this.mpc.OnDisconnected = (Action)(() =>
            {
                this.automated = false;
                if (this.renderer == null)
                    return;
                this.renderer.ShouldRender = false;
                this.renderer.ShouldRotateAutomated = false;
            });
        }
        public override void FromTreeAttributes(ITreeAttribute tree, IWorldAccessor worldForResolving)
        {
            base.FromTreeAttributes(tree, worldForResolving);
            this.Inventory.FromTreeAttributes(tree.GetTreeAttribute("inventory"));
            if (this.Api != null)
                this.Inventory.AfterBlocksLoaded(this.Api.World);
            //this.inputGrindTime = tree.GetFloat("inputGrindTime");
            // this.nowOutputFace = tree.GetInt("nowOutputFace");
            /* if (worldForResolving.Side == EnumAppSide.Client)
             {
                 List<int> clientIds = new List<int>((IEnumerable<int>)(tree["clientIdsGrinding"] as IntArrayAttribute).value);
                 this.quantityPlayersGrinding = clientIds.Count;
                 foreach (string str in this.playersGrinding.Keys.ToArray<string>())
                 {
                     IPlayer player = this.Api.World.PlayerByUid(str);
                     if (!clientIds.Contains(player.ClientId))
                         this.playersGrinding.Remove(str);
                     else
                         clientIds.Remove(player.ClientId);
                 }
                 for (int i = 0; i < clientIds.Count; i++)
                 {
                     IPlayer player = ((IEnumerable<IPlayer>)worldForResolving.AllPlayers).FirstOrDefault<IPlayer>((System.Func<IPlayer, bool>)(p => p.ClientId == clientIds[i]));
                     if (player != null)
                         this.playersGrinding.Add(player.PlayerUID, worldForResolving.ElapsedMilliseconds);
                 }
                 this.updateGrindingState();
             }*/
            ICoreAPI api = this.Api;
            if (api != null ? (api.Side == EnumAppSide.Client) : false)
            {
                if (this.renderer != null)
                {
                    this.renderer.meshref = (api as ICoreClientAPI).Render.UploadMesh(this.GenMesh("top"));
                }
                //this.GenMesh("top");
            }
            return;
            //this.clientDialog.Update(this.inputGrindTime, this.maxGrindingTime());
        }
        public override void ToTreeAttributes(ITreeAttribute tree)
        {
            base.ToTreeAttributes(tree);
            ITreeAttribute tree1 = (ITreeAttribute)new TreeAttribute();
            this.Inventory.ToTreeAttributes(tree1);
            tree["inventory"] = (IAttribute)tree1;
            // tree.SetFloat("inputGrindTime", this.inputGrindTime);
            // tree.SetInt("nowOutputFace", this.nowOutputFace);
            List<int> intList = new List<int>();
            foreach (KeyValuePair<string, float> keyValuePair in this.playersGrinding)
            {
                IPlayer player = this.Api.World.PlayerByUid(keyValuePair.Key);
                if (player != null)
                    intList.Add(player.ClientId);
            }
            tree["clientIdsGrinding"] = (IAttribute)new IntArrayAttribute(intList.ToArray());
        }
        public override void OnBlockRemoved()
        {
            base.OnBlockRemoved();
            if (this.ambientSound != null)
            {
                this.ambientSound.Stop();
                this.ambientSound.Dispose();
            }
            this.renderer?.Dispose();
            this.renderer = null;
        }
        public override void OnBlockBroken(IPlayer byPlayer = null) => base.OnBlockBroken(byPlayer);
        public override void OnReceivedClientPacket(IPlayer player, int packetid, byte[] data)
        {
            if (packetid < 1000)
            {
                this.Inventory.InvNetworkUtil.HandleClientPacket(player, packetid, data);
                this.Api.World.BlockAccessor.GetChunkAtBlockPos(this.Pos.X, this.Pos.Y, this.Pos.Z).MarkModified();
            }
            else
            {
                if (packetid != 1001 || player.InventoryManager == null)
                    return;
                player.InventoryManager.CloseInventory((IInventory)this.Inventory);
            }
        }
        public override bool OnTesselation(ITerrainMeshPool mesher, ITesselatorAPI tesselator)
        {
            if (this.Block == null)
                return false;
            mesher.AddMeshData(this.quernBaseMesh);
            //if (this.quantityPlayersGrinding == 0 && !this.automated)
            //{
            // mesher.AddMeshData(this.quernTopMesh.Clone().Rotate(new Vec3f(0.5f, 0.5f, 0.5f), 0.0f, this.renderer.AngleRad, 0.0f).Translate(0.0f, 11f / 16f, 0.0f));
            // }
            return true;
        }
        public override void OnStoreCollectibleMappings(
          Dictionary<int, AssetLocation> blockIdMapping,
          Dictionary<int, AssetLocation> itemIdMapping)
        {
            foreach (ItemSlot itemSlot in this.Inventory)
            {
                if (itemSlot.Itemstack != null)
                {
                    if (itemSlot.Itemstack.Class == EnumItemClass.Item)
                        itemIdMapping[itemSlot.Itemstack.Item.Id] = itemSlot.Itemstack.Item.Code;
                    else
                        blockIdMapping[itemSlot.Itemstack.Block.BlockId] = itemSlot.Itemstack.Block.Code;
                }
            }
        }

        public override void OnLoadCollectibleMappings(
          IWorldAccessor worldForResolve,
          Dictionary<int, AssetLocation> oldBlockIdMapping,
          Dictionary<int, AssetLocation> oldItemIdMapping,
          int schematicSeed)
        {
            foreach (ItemSlot itemSlot in this.Inventory)
            {
                if (itemSlot.Itemstack != null && !itemSlot.Itemstack.FixMapping(oldBlockIdMapping, oldItemIdMapping, worldForResolve))
                    itemSlot.Itemstack = (ItemStack)null;
            }
        }
        public override void OnBlockUnloaded()
        {
            base.OnBlockUnloaded();
            this.renderer?.Dispose();
        }
        public void IsGrinding(IPlayer byPlayer) => this.SetPlayerGrinding(byPlayer, true);
        private void Every100ms(float dt)
        {
            float grindSpeed = this.GrindSpeed;
            if (this.Api.Side == EnumAppSide.Client)
            {
                if (this.InputStack != null)
                {
                  
                }
                if(mpc == null)
                {
                    return;
                }
                if (this.ambientSound != null && (double)this.mpc.TrueSpeed != (double)this.prevSpeed)
                {
                    this.prevSpeed = this.mpc.TrueSpeed;
                    this.ambientSound.SetPitch((float)((0.5 + (double)this.prevSpeed) * 0.899999976158142));
                    this.ambientSound.SetVolume(Math.Min(1f, this.prevSpeed * 3f));
                }
                else
                    this.prevSpeed = float.NaN;
            }
            else
            {
                if (!this.CanGrind() || (double)grindSpeed <= 0.0)
                    return;
               /* this.inputGrindTime += dt * grindSpeed;
                if ((double)this.inputGrindTime >= (double)this.maxGrindingTime())
                {
                    this.grindInput();
                    this.inputGrindTime = 0.0f;
                }*/
                //this.MarkDirty();
            }
        }
        private void grindInput()
        {
            ItemStack itemStack = this.InputGrindProps.GroundStack.ResolvedItemstack.Clone();
            if (this.OutputSlot.Itemstack == null)
                this.OutputSlot.Itemstack = itemStack;
            else if (this.OutputSlot.Itemstack.Collectible.GetMergableQuantity(this.OutputSlot.Itemstack, itemStack, EnumMergePriority.AutoMerge) > 0)
            {
                this.OutputSlot.Itemstack.StackSize += itemStack.StackSize;
            }
            else
            {
                BlockFacing facing = BlockFacing.HORIZONTALS[this.nowOutputFace];
                this.nowOutputFace = (this.nowOutputFace + 1) % 4;
                if (this.Api.World.BlockAccessor.GetBlock(this.Pos.AddCopy(facing)).Replaceable < 6000)
                    return;
                this.Api.World.SpawnItemEntity(itemStack, this.Pos.ToVec3d().Add(0.5 + (double)facing.Normalf.X * 0.7, 0.75, 0.5 + (double)facing.Normalf.Z * 0.7), new Vec3d((double)facing.Normalf.X * 0.0199999995529652, 0.0, (double)facing.Normalf.Z * 0.0199999995529652));
            }
            this.InputSlot.TakeOut(1);
            this.InputSlot.MarkDirty();
            this.OutputSlot.MarkDirty();
        }
        private void Every500ms(float dt)
        {
           // if (this.Api.Side == EnumAppSide.Server && ((double)this.GrindSpeed > 0.0 || (double)this.prevInputGrindTime != (double)this.inputGrindTime) && this.inventory[0].Itemstack?.Collectible.GrindingProps != null)
             //   this.MarkDirty();
           // this.prevInputGrindTime = this.inputGrindTime;
            /*foreach (KeyValuePair<string, float> keyValuePair in this.playersGrinding)
            {
                if (this.Api.World.ElapsedMilliseconds - keyValuePair.Value > 1000L)
                {
                    this.playersGrinding.Remove(keyValuePair.Key);
                    break;
                }
            }*/
        }
        public void SetPlayerGrinding(IPlayer player, bool playerGrinding)
        {
           // if (!this.automated)
           // {
           if(player.Entity.Api.Side == EnumAppSide.Client)
            {
                return;
            }
            if (playerGrinding)
                this.playersGrinding[player.PlayerUID] = 0;
            else
                this.playersGrinding.Remove(player.PlayerUID);
            this.quantityPlayersGrinding = this.playersGrinding.Count;
           // }
            //this.updateGrindingState();
        }
        private void updateGrindingState()
        {
            if (this.Api?.World == null)
                return;
            /*bool flag = this.quantityPlayersGrinding > 0 || this.automated && (double)this.mpc.TrueSpeed > 0.0;
            if (flag != this.beforeGrinding)
            {
                if (this.renderer != null)
                    this.renderer.ShouldRotateManual = this.quantityPlayersGrinding > 0;
                this.Api.World.BlockAccessor.MarkBlockDirty(this.Pos, new Action(this.OnRetesselated));
                if (flag)
                    this.ambientSound?.Start();
                else
                    this.ambientSound?.Stop();
                if (this.Api.Side == EnumAppSide.Server)
                    this.MarkDirty();
            }
            this.beforeGrinding = flag;*/
        }
        private void OnSlotModifid(int slotid)
        {
            if(Api is ICoreClientAPI)
            {
                this.GenMesh("top");
            }
            if (slotid != 0)
                return;
        }
        private void OnRetesselated()
        {
           if (this.renderer == null)
                return;
            this.renderer.ShouldRender = this.quantityPlayersGrinding > 0 || this.automated;
        }
        internal MeshData GenMesh(string type = "base")
        {
            //this.targetAtlas = targetAtlas;
            this.tmpTextures.Clear();
            if (!InputSlot.Empty)
            {
                string layerType = InputStack.Collectible.LastCodePart();
                tmpTextures["layer"] = new AssetLocation("canjewelry:item/grindlayer/" + layerType + ".png");
            }
            Block block = this.Api.World.BlockAccessor.GetBlock(this.Pos);
            if (block.BlockId == 0)
                return (MeshData)null;
            MeshData modeldata = null;
            if (type.Equals("base")){
                ((ICoreClientAPI)this.Api).Tesselator.TesselateShape((CollectibleObject)block, Shape.TryGet(this.Api, "canjewelry:shapes/block/jewelgrinder.json"), out modeldata);
            }
            else if (type.Equals("top"))
            {
                //((ICoreClientAPI)this.Api).Tesselator.TesselateShape((CollectibleObject)block, Shape.TryGet(this.Api, "canmods:shapes/block/jewelgrinder-top.json"), out modeldata);
                ((ICoreClientAPI)this.Api).Tesselator.TesselateShape("jewelgrinder-top", Shape.TryGet(this.Api, "canjewelry:shapes/block/jewelgrinder-top.json"), out modeldata, (ITexPositionSource)this, new Vec3f(0.0f, block.Shape.rotateY, 0.0f));
               // ((ICoreClientAPI)this.Api).Tesselator.TesselateShape((CollectibleObject)block, Shape.TryGet(this.Api, "canmods:shapes/block/jewelgrinder-top.json"), out modeldata);
            }
            return modeldata;
        }
        public bool CanGrind()
        {
            if (this.InputStack == null)
            {
                return false;
            }
            return true;
        }
        //rough 0
        //medium 1
        //fine 2
        public int getGrindLayerType()
        {
            if(this.InputStack == null)
            {
                return -1;
            }
            if (this.InputStack.Item.Attributes.KeyExists("cangrindinfo"))
            {
                return this.InputStack.Item.Attributes["cangrindinfo"].AsInt();
            }
            return -1;
        }
        public void doGrind(IPlayer player, float secondsUsed)
        {
            if(canjewelry.sapi == null)
            {
                return;
            }
            if(!this.playersGrinding.ContainsKey(player.PlayerUID) || (double)this.GrindSpeed < 0.3)
            {
               return;
            }
            if (secondsUsed - this.playersGrinding[player.PlayerUID] >= 1)
            {
                this.playersGrinding[player.PlayerUID] = secondsUsed;

                if (player.InventoryManager.ActiveHotbarSlot.Itemstack == null)
                {
                    return;
                }
                ItemSlot activeSlot = player.InventoryManager.ActiveHotbarSlot;
                ItemStack activeItemStack = activeSlot.Itemstack;
                AssetLocation item_code = activeSlot.Itemstack.Collectible.Code;

                if(activeItemStack.Item is not CANCutGemItem)
                {
                    return;
                }

                if (!activeSlot.Itemstack.Attributes.HasAttribute("cangrindlayerinfo"))
                {
                    ITreeAttribute tree = new TreeAttribute();

                    tree.SetInt("grindtype", 1);
                    tree.SetInt("grindcounter", 10);
                    activeItemStack.Attributes["cangrindlayerinfo"] = tree;
                    return;
                }
                ITreeAttribute itree = activeSlot.Itemstack.Attributes.GetTreeAttribute("cangrindlayerinfo");
                if (itree.GetInt("grindtype") == this.getGrindLayerType())
                {
                    this.InputStack.Collectible.DamageItem(canjewelry.sapi.World, player.Entity, this.InputSlot);
                    float grindSpeed = this.GrindSpeed;
                    float num1 = 1f * grindSpeed;
                    float num2 = 5f * grindSpeed;
                    float num3 = 1f * grindSpeed;
                    float num4 = 20f * grindSpeed;
                    gemTypeToColor.TryGetValue(activeItemStack.Collectible.Variant["gemtype"], out int colorParticles);

                    BEJewelGrinder.FlourDustParticles.Color = -421266951;//colorParticles;
                    BEJewelGrinder.FlourDustParticles.MinQuantity = num1;
                    BEJewelGrinder.FlourDustParticles.AddQuantity = num2;
                    BEJewelGrinder.FlourDustParticles.MinPos.Set((double)this.Pos.X - 1.0 / 32.0, (double)this.Pos.Y + 11.0 / 16.0, (double)this.Pos.Z - 1.0 / 32.0);
                    BEJewelGrinder.FlourDustParticles.MinVelocity.Set(-0.1f, 0.0f, -0.1f);
                    BEJewelGrinder.FlourDustParticles.AddVelocity.Set(0.2f, 0.2f, 0.2f);
                    BEJewelGrinder.FlourParticles.MinPos.Set((double)this.Pos.X - 1.0 / 32.0, (double)this.Pos.Y + 11.0 / 16.0, (double)this.Pos.Z - 1.0 / 32.0);
                    BEJewelGrinder.FlourParticles.AddQuantity = num4;
                    BEJewelGrinder.FlourParticles.MinQuantity = num3;
                    this.Api.World.SpawnParticles((IParticlePropertiesProvider)BEJewelGrinder.FlourParticles);
                    this.Api.World.SpawnParticles((IParticlePropertiesProvider)BEJewelGrinder.FlourDustParticles);
                    if (itree.GetInt("grindcounter") <= 1)
                    {
                        ITreeAttribute cutGemTree = activeItemStack.Attributes.GetTreeAttribute(CANJWConstants.CUT_GEM_TREE);
                        canjewelry.config.CuttingAttributesDict.TryGetValue(cutGemTree.GetString(CANJWConstants.CUTTING_TYPE), out var cuttingAttributes);
                        var currentValues = (cutGemTree[CANJWConstants.ENCRUSTABLE_BUFFS_VALUES] as FloatArrayAttribute).value;
                        currentValues[0] = currentValues[0] * cuttingAttributes.GrindingBuffIncreaseMultipliers[itree.GetInt("grindtype")];
                        (cutGemTree[CANJWConstants.ENCRUSTABLE_BUFFS_VALUES] as FloatArrayAttribute).value = currentValues;
                        if (itree.GetInt("grindtype") == 2)
                        {
                            activeItemStack.Attributes.RemoveAttribute("cangrindlayerinfo");                          
                            return;
                        }
                        itree.SetInt("grindtype", itree.GetInt("grindtype") + 1);
                        itree.SetInt("grindcounter", 20);
                        player.InventoryManager.ActiveHotbarSlot.MarkDirty();
                    }
                    else
                    {
                        itree.SetInt("grindcounter", itree.GetInt("grindcounter") - 1);
                        player.InventoryManager.ActiveHotbarSlot.MarkDirty();
                        return;
                    }
                }
            }

        }
        ~BEJewelGrinder()
        {
            if (this.ambientSound == null)
                return;
            this.ambientSound.Dispose();
        }
        protected TextureAtlasPosition getOrCreateTexPos(AssetLocation texturePath)
        {
            TextureAtlasPosition texpos = (this.Api as ICoreClientAPI).BlockTextureAtlas[texturePath];
            if (texpos == null)
            {
                IAsset texAsset = this.Api.Assets.TryGet(texturePath.Clone().WithPathPrefixOnce("textures/").WithPathAppendixOnce(".png"), true);
                if (texAsset != null)
                {
                    int num;
                    (this.Api as ICoreClientAPI).BlockTextureAtlas.GetOrInsertTexture(texturePath, out num, out texpos, () => texAsset.ToBitmap(this.Api as ICoreClientAPI), 0.005f);
                }
                else
                {
                    /*this.Api.World.Logger.Warning("For render in shield {0}, require texture {1}, but no such texture found.", new object[]
                    {
                        this.Code,
                        texturePath
                    });*/
                }
            }
            return texpos;
        }    
        private void setRenderer()
        {
            ICoreAPI api = this.Api;
            if ((api != null ? (api.Side == EnumAppSide.Client ? 1 : 0) : 0) == 0)
                return;
            if (this.renderer != null)
            {
                this.renderer.meshref.Dispose();
                this.renderer.meshref = (api as ICoreClientAPI).Render.UploadMesh(this.GenMesh("top"));
                this.renderer.ShouldRender = true;
                return;
            }
            else
            {
                this.renderer = new JewelGrinderTopRenderer(this.Api as ICoreClientAPI, this, this.Pos, this.GenMesh("top"));
            }
           
            this.renderer.mechPowerPart = this.mpc;
            (this.Api as ICoreClientAPI).Event.RegisterRenderer((IRenderer)this.renderer, EnumRenderStage.Opaque, "jewelgrinder-top");
            (this.Api as ICoreClientAPI).Event.RegisterRenderer((IRenderer)this.renderer, EnumRenderStage.ShadowFar, "jewelgrinder-top");
            (this.Api as ICoreClientAPI).Event.RegisterRenderer((IRenderer)this.renderer, EnumRenderStage.ShadowNear, "jewelgrinder-top");
            this.renderer.ShouldRender = true;
        }       
        public string GetMeshCacheKey(ItemStack itemstack)
        {
            if (inventory[0].Itemstack != null)
            {
                return inventory[0].Itemstack?.Collectible.LastCodePart();
            }
            return "-";
        }
    }
}

