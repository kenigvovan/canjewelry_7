﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vintagestory.API.Client;
using Vintagestory.API.Common;
using Vintagestory.API.Config;
using Vintagestory.API.MathTools;
using Vintagestory.API.Util;
using Vintagestory.ServerMods.NoObf;

namespace canjewelry.src.jewelry
{
    public class GuiDialogJewelerSet : GuiDialogBlockEntity
    {
        GuiElementHorizontalTabs groupOfInterests;
        public float Width { get; private set; }
        public float Height { get; private set; }
        public GuiDialogJewelerSet(string dialogTitle, InventoryBase inventory, BlockPos blockEntityPos, ICoreClientAPI capi) : base(dialogTitle, inventory, blockEntityPos, capi)
        {            
            if (IsDuplicate)
            {
                return;
            }
            this.Width = 300;
            this.Height = 400;
            capi.World.Player.InventoryManager.OpenInventory((IInventory)inventory);
            SetupDialog();
        }
        public void SetupDialog()
        {
            int chosenGroupTab = groupOfInterests == null ? 0 : groupOfInterests.activeElement;
            int fixedY1 = 60;
            ElementBounds elementBounds = ElementStdBounds.AutosizedMainDialog.WithAlignment(EnumDialogArea.CenterMiddle);
            ElementBounds backgroundBounds = ElementBounds.Fill.WithFixedPadding(GuiStyle.ElementToDialogPadding).WithFixedSize(Width, Height); ;
            backgroundBounds.BothSizing = ElementSizing.FitToChildren;

            elementBounds.BothSizing = ElementSizing.FitToChildren;
            elementBounds.WithChild(backgroundBounds);

            ElementBounds tabsBounds = ElementBounds.FixedPos(EnumDialogArea.LeftTop, 100, 40).WithFixedHeight(30.0).WithFixedWidth(140);


            int fixedY2 = fixedY1 + 28;
            ElementBounds leftInsetBounds = ElementBounds.FixedPos(EnumDialogArea.LeftMiddle,
                                                                0,
                                                                0)
                .WithFixedHeight(GuiElement.scaledi(70))
                .WithFixedWidth(GuiElement.scaledi(72));

            ElementBounds rightInsetBounds = ElementBounds.FixedPos(EnumDialogArea.LeftMiddle,
                                                                140,
                                                                0)
                .WithFixedHeight(GuiElement.scaledi(70))
                .WithFixedWidth(GuiElement.scaledi(72)); ;

            ElementBounds bounds4 = ElementBounds.FixedPos(EnumDialogArea.LeftTop, 160, 80).WithFixedHeight(150).WithFixedWidth(250);

            ElementBounds leftSlotBounds = ElementBounds.FixedPos(EnumDialogArea.LeftTop, GuiElement.scaledi(10), GuiElement.scaledi(10))
                .WithFixedWidth(48).WithFixedHeight(48);
            ElementBounds rightSlotsBounds = ElementBounds.FixedPos(EnumDialogArea.LeftTop, GuiElement.scaledi(10), GuiElement.scaledi(10))
                .WithFixedWidth(48).WithFixedHeight(48);

            ElementBounds buttonBounds = ElementBounds.FixedPos(EnumDialogArea.LeftBottom, 140, 0)
                .WithFixedWidth(100).WithFixedHeight(48);

            leftInsetBounds.WithChild(leftSlotBounds);
            rightInsetBounds.WithChild(rightSlotsBounds);
            //backgroundBounds.WithChildren(rightSlotsBounds);
            int fixedY3 = fixedY2 + 28;
            
           
            GuiTab[] tabs1 = new GuiTab[2];

            tabs1[0] = new GuiTab();
            tabs1[0].Name = "Sockets";
            tabs1[0].DataInt = 0;

            tabs1[1] = new GuiTab();
            tabs1[1].Name = "Gems";
            tabs1[1].DataInt = 1;

            this.SingleComposer = this.Composers["jewelersetgui" + this.BlockEntityPosition?.ToString()] = this.capi.Gui.CreateCompo("jewelersetgui" + this.BlockEntityPosition?.ToString(), elementBounds).
                  AddShadedDialogBG(backgroundBounds).
                  AddDialogTitleBar(Lang.Get("canjewelry:jewelset_gui_name"), new Action(this.OnTitleBarClose));
            this.Composers["jewelersetgui" + this.BlockEntityPosition?.ToString()].AddHorizontalTabs(tabs1, tabsBounds, new Action<int>(this.OnGroupTabClicked), CairoFont.WhiteSmallText(), CairoFont.WhiteSmallText(), "GroupTabs");
            this.groupOfInterests = this.Composers["jewelersetgui" + this.BlockEntityPosition?.ToString()].GetHorizontalTabs("GroupTabs");
            this.groupOfInterests.activeElement = chosenGroupTab;
            //SingleComposer.AddInset(backgroundBounds);
            //SingleComposer.AddInset(tabsBounds);
            backgroundBounds.WithChildren(tabsBounds, leftInsetBounds, bounds4, rightInsetBounds, buttonBounds);
            if (this.groupOfInterests.activeElement == 0)
            {
                //mainBounds.WithFixedWidth(GuiElement.scaledi(200));
                //rightSlotsBounds.WithFixedWidth(GuiElement.scaledi(40));

                double elementToDialogPadding = GuiStyle.ElementToDialogPadding;
                double unscaledSlotPadding = GuiElementItemSlotGridBase.unscaledSlotPadding;

                this.Composers["jewelersetgui" + this.BlockEntityPosition?.ToString()].AddItemSlotGrid((IInventory)this.Inventory, new Action<object>(((GuiDialogJewelerSet)this).DoSendPacket), 1, new int[1]
                {
                   0
                }, leftSlotBounds, "craftinggrid");

                rightInsetBounds.WithFixedHeight(70).WithFixedWidth(72);

                this.Composers["jewelersetgui" + this.BlockEntityPosition?.ToString()].AddItemSlotGrid((IInventory)this.Inventory, new Action<object>(((GuiDialogJewelerSet)this).DoSendPacket), 1, new int[1]
                {
                    1
                }, rightSlotsBounds, "craftinggrid2");
                this.Composers["jewelersetgui" + this.BlockEntityPosition?.ToString()].AddInset(leftInsetBounds);
                this.Composers["jewelersetgui" + this.BlockEntityPosition?.ToString()].AddInset(rightInsetBounds);
                this.Composers["jewelersetgui" + this.BlockEntityPosition?.ToString()].AddButton(Lang.Get("canjewelry:gui_add_socket"),
                    () => onClickBackButtonPutSocket(),
                    buttonBounds,
                    CairoFont.WhiteSmallText().WithOrientation(EnumTextOrientation.Center));
            }
            else
            {
                //gems input
                
                double elementToDialogPadding = GuiStyle.ElementToDialogPadding;
                double unscaledSlotPadding = GuiElementItemSlotGridBase.unscaledSlotPadding;
                ElementBounds elementBoundsbig = ElementStdBounds.SlotGrid(EnumDialogArea.None, 100.0, 40.0, 4, 3).FixedGrow(unscaledSlotPadding);



                this.Composers["jewelersetgui" + this.BlockEntityPosition?.ToString()].AddItemSlotGrid((IInventory)this.Inventory, new Action<object>(((GuiDialogJewelerSet)this).DoSendPacket), 1, new int[1]
                            {
                                0
                            }, leftSlotBounds, "encrusteditem");
                
                
                if (this.Inventory.Count > 1)
                {
                    if(this.Inventory[0].Itemstack != null)
                    {
                        if (this.Inventory[0].Itemstack.Attributes.HasAttribute("canencrusted"))
                        {

                            //tree
                            var canencrustedTree = this.Inventory[0].Itemstack.Attributes.GetTreeAttribute("canencrusted");
                            if (canencrustedTree.GetInt("socketsnumber") > 0)
                            {
                                int socketCount = canencrustedTree.GetInt("socketsnumber");
                                int[] intArr = new int[socketCount];
                                rightInsetBounds.WithFixedHeight(70).WithFixedWidth(48 * socketCount + (socketCount > 1 ? GuiElement.scaledi(12) * (socketCount) : GuiElement.scaledi(12) * 2));
                                rightSlotsBounds.WithFixedWidth(48 * socketCount + (socketCount > 1 ? GuiElement.scaledi(12) * (socketCount) : GuiElement.scaledi(12) * 2));
                                for (int i = 0; i < intArr.Length; i++)
                                {
                                    intArr[i] = i + 1;
                                }
                                this.Composers["jewelersetgui" + this.BlockEntityPosition?.ToString()].AddItemSlotGrid((IInventory)this.Inventory, new Action<object>(((GuiDialogJewelerSet)this).DoSendPacket), intArr.Length, intArr, rightSlotsBounds, "socketsslots");
                            }
                        }
                    }
                }
                this.Composers["jewelersetgui" + this.BlockEntityPosition?.ToString()].AddInset(leftInsetBounds);
                this.Composers["jewelersetgui" + this.BlockEntityPosition?.ToString()].AddInset(rightInsetBounds);
                this.Composers["jewelersetgui" + this.BlockEntityPosition?.ToString()].AddButton(Lang.Get("canjewelry:gui_add_gem"),
                   () => onClickBackButtonPutGem(),
                   buttonBounds,
                   CairoFont.WhiteSmallText().WithOrientation(EnumTextOrientation.Center));
            }
            this.Composers["jewelersetgui" + this.BlockEntityPosition?.ToString()].Compose();
            this.Composers["jewelersetgui" + this.BlockEntityPosition?.ToString()].UnfocusOwnElements();
            ComposeAvailableGemTypesGui();

            /*double elementToDialogPadding = GuiStyle.ElementToDialogPadding;
            double unscaledSlotPadding = GuiElementItemSlotGridBase.unscaledSlotPadding;
            ElementBounds bounds1 = ElementBounds.Fill.WithFixedPadding(GuiStyle.ElementToDialogPadding);
            ElementBounds elementBounds = ElementStdBounds.SlotGrid(EnumDialogArea.None, 0.0, 40.0, 3, 3).FixedGrow(unscaledSlotPadding);
            ElementBounds bounds2 = ElementStdBounds.SlotGrid(EnumDialogArea.None, 60.0, 90.0, 1, 1).RightOf(elementBounds, 50.0).FixedGrow(unscaledSlotPadding);
            ElementBounds bounds3 = ElementBounds.FixedOffseted(EnumDialogArea.None, 0.0, 40.0, 20.0, 20.0).RightOf(elementBounds, 20.0);
            bounds1.BothSizing = ElementSizing.FitToChildren;
            bounds1.WithChildren(elementBounds, bounds3, bounds2);
            ElementBounds bounds4 = ElementStdBounds.AutosizedMainDialog.WithAlignment(EnumDialogArea.RightMiddle).WithFixedAlignmentOffset(-GuiStyle.DialogToScreenPadding, 0.0);
           
            this.SingleComposer = GuiComposerHelpers.
                AddShadedDialogBG(this.capi.Gui.CreateCompo("fgtabledlg" + this.BlockEntityPosition?.ToString(), bounds4), bounds1, true, 5.0).
                AddDialogTitleBar("Crafting Table", new Action(this.OnTitleBarClose));
            //SingleComposer.AddStaticText(Lang.Get("claimsext:gui-type-name"), CairoFont.WhiteSmallishText(), ElementBounds.Fixed(60, (double)bounds3.fixedY + 20, 200, 20));
            SingleComposer.AddTextInput(ElementBounds.Fixed(40, (double)bounds3.fixedY + 60, 220, 34), (name) => collectedStringValue = name, null, "collectedValue");
            SingleComposer.AddButton(Lang.Get("canmods:gui-rename-ok"), () => onClickRenameCollectible(), ElementBounds.Fixed(80, (double)bounds3.fixedY + 120, 90, 40));
            SingleComposer.Compose();
            this.SingleComposer.UnfocusOwnElements();*/
        }
        private string[] GetAvailableGemTypes(ItemStack itemStack)
        {
            string itemCode = itemStack.Collectible.Code.Path;
            List<string> res = new List<string>();
            foreach(var gemTypeSetPair in canjewelry.config.buffNameToPossibleItem)
            {
                foreach(var it in gemTypeSetPair.Value)
                {                 
                    if (it.Contains("pick"))
                    {
                        var c = 3;
                    }
                    if (WildcardUtil.Match("*" + it + "*", itemCode))
                    {
                        res.Add(gemTypeSetPair.Key);
                    }
                }
            }
            return res.ToArray();
        }
        public void ComposeAvailableGemTypesGui()
        {
            if(this.Inventory[0].Itemstack == null || !this.Inventory[0].Itemstack.Collectible.Attributes.KeyExists("canhavesocketsnumber"))
            {
                this.Composers.Remove("jewelersetgui-types");
                //this.Composers["jewelersetgui-types"]?.Clear();
                //this.capi.Gui.CreateCompo("jewelersetgui-types", dialogBounds).Compose();
                //this.Composers["jewelersetgui-types"]?.Dispose();
                //var cc = this.capi.Gui.CreateCompo("jewelersetgui-types", new ElementBounds());
                //cc.Compose();
                return;
            }
            string[] availableGemTypes = GetAvailableGemTypes(this.Inventory[0].Itemstack);
            if(availableGemTypes.Length < 1)
            {
                return;
            }
            ElementBounds leftDlgBounds = this.Composers["jewelersetgui" + this.BlockEntityPosition?.ToString()].Bounds;
            double b = leftDlgBounds.InnerHeight / (double)RuntimeEnv.GUIScale + 10.0;

            //ElementBounds elementBounds = ElementStdBounds.AutosizedMainDialog.WithAlignment(EnumDialogArea.RightBottom);
            //ElementBounds backgroundBounds = ElementBounds.Fill.WithFixedPadding(GuiStyle.ElementToDialogPadding).WithFixedSize(Width, Height);
            ElementBounds bgBounds = ElementBounds.Fixed(0.0, 0.0,
                235, leftDlgBounds.InnerHeight / (double)RuntimeEnv.GUIScale - GuiStyle.ElementToDialogPadding - 20.0 + b).WithFixedPadding(GuiStyle.ElementToDialogPadding);
            ElementBounds dialogBounds = bgBounds.ForkBoundingParent(0.0, 0.0, 0.0, 0.0)
                .WithAlignment(EnumDialogArea.LeftMiddle)
                .WithFixedAlignmentOffset((leftDlgBounds.renderX + leftDlgBounds.OuterWidth + 10.0) / (double)RuntimeEnv.GUIScale,  0);
            bgBounds.BothSizing = ElementSizing.FitToChildren;

            dialogBounds.BothSizing = ElementSizing.FitToChildren;
            dialogBounds.WithChild(bgBounds);
            ElementBounds textBounds = ElementBounds.FixedPos(EnumDialogArea.LeftTop,
                                                               0,
                                                                0);
                //.WithFixedHeight(leftDlgBounds.InnerHeight)
                //.WithFixedWidth(leftDlgBounds.InnerWidth / 2);
            bgBounds.WithChildren(textBounds);

            //SingleComposer.AddStaticText("hello", CairoFont.WhiteDetailText(), bgBounds);

            this.Composers["jewelersetgui-types"] = this.capi.Gui.CreateCompo("jewelersetgui-types", dialogBounds).AddShadedDialogBG(bgBounds, false, 5.0, 0.75f);
           // this.Composers["jewelersetgui-types"].AddInset(dialogBounds);
            for(int i = 0; i < availableGemTypes.Length; i++)
            {
                ElementBounds el = textBounds.CopyOffsetedSibling().WithFixedHeight(20)
                    .WithFixedWidth(100)
                    .WithFixedPosition(0, i * 20);
                bgBounds.WithChildren(el);

                this.Composers["jewelersetgui-types"].AddStaticText(availableGemTypes[i], CairoFont.WhiteDetailText(), el);
            }
            //this.Composers["jewelersetgui-types"].AddStaticText("hello", CairoFont.WhiteDetailText(), ElementBounds.Fixed(0, 0, 200, 20));
            //this.Composers["jewelersetgui-types"].AddStaticText("hello1", CairoFont.WhiteDetailText(), bgBounds);
            /*ElementBounds leftDlgBounds = this.Composers["single"].Bounds;
            ElementBounds bounds = this.Composers["single"].Bounds;
            double b = bounds.InnerHeight / (double)RuntimeEnv.GUIScale + 10.0;
            ElementBounds bgBounds = ElementBounds.Fixed(0.0, 0.0, 235.0,
                leftDlgBounds.InnerHeight / (double)RuntimeEnv.GUIScale - GuiStyle.ElementToDialogPadding - 20.0 + b)
                .WithFixedPadding(GuiStyle.ElementToDialogPadding);
            ElementBounds dialogBounds = bgBounds.ForkBoundingParent(0.0, 0.0, 0.0, 0.0).WithAlignment(EnumDialogArea.LeftMiddle);
            SingleComposer.AddStaticText("hello", CairoFont.WhiteDetailText(), bgBounds);*/
            this.Composers["jewelersetgui-types"].Compose();
        }
        private void OnTitleBarClose() => this.TryClose();
        public override void OnGuiClosed()
        {
            this.capi.Network.SendPacketClient(this.capi.World.Player.InventoryManager.CloseInventory((IInventory)this.Inventory));
            base.Inventory.SlotModified -= this.OnInventorySlotModified;
            base.OnGuiClosed();
        }
        private void OnGroupTabClicked(int clicked)
        {
            this.groupOfInterests.activeElement = clicked;
            this.SetupDialog();
        }
        public override void OnGuiOpened()
        {
            base.OnGuiOpened();
            base.Inventory.SlotModified += this.OnInventorySlotModified;

        }
        private void OnInventorySlotModified(int slotid)
        {
            if (slotid == 0)
            {
                this.capi.Event.EnqueueMainThreadTask(new Action(this.SetupDialog), "setupjewelersetdlg");
                this.capi.Event.EnqueueMainThreadTask(new Action(this.ComposeAvailableGemTypesGui), "setupavailabletypesdlg");
                
            }
        }
        public bool onClickBackButtonPutSocket()
        {
            this.capi.Network.SendBlockEntityPacket(this.BlockEntityPosition, 1004);
            //this.chosenCommand = enumChosenCommand.NO_CHOSEN_COMMAND;
            // this.buildWindow();
            return true;
        }
        public bool onClickBackButtonPutGem()
        {
            this.capi.Network.SendBlockEntityPacket(this.BlockEntityPosition, 1005);
            //this.chosenCommand = enumChosenCommand.NO_CHOSEN_COMMAND;
            // this.buildWindow();
            return true;
        }
    }
}
