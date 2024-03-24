using System;
using System.Collections.Generic;
using UnityEngine;
using LogicWorld.UI;
using LICC;
using LogicAPI.Data.BuildingRequests;
using TMPro;
using LogicUI.MenuParts;
using CheeseUtilMod.Client;
using System.IO;
using System.Windows.Forms;
using EccsGuiBuilder.Client.Layouts.Controller;
using EccsGuiBuilder.Client.Layouts.Elements;
using EccsGuiBuilder.Client.Wrappers;
using EccsGuiBuilder.Client.Wrappers.AutoAssign;
using LogicWorld.BuildingManagement;
using Application = UnityEngine.Application;

namespace CheeseRamMenu.Client
{
    public class RamMenu : EditComponentMenu, IAssignMyFields
    {
        public static void init()
        {
            WS.window("CheeseRamMenu")
                .configureContent(content => content
                    .vertical(20f, new RectOffset(20, 20, 20, 20), expandHorizontal: true)
                    .add(WS.inputField
                        .injectionKey(nameof(filePathInputField))
                        .fixedSize(1000, 80)
                        .setPlaceholderLocalizationKey("CheeseRamMenu.FileFieldHint")
                        .disableRichText()
                    )
                    .addContainer("Buttons", buttons => buttons
                        .injectionKey(nameof(buttonsSection))
                        .horizontal(anchor: TextAnchor.UpperCenter)
                        .add(WS.button.setLocalizationKey("CheeseRamMenu.FileSave")
                            .injectionKey(nameof(saveButton))
                            .add<ButtonLayout>())
                        .add(WS.button.setLocalizationKey("CheeseRamMenu.FileLoad")
                            .injectionKey(nameof(loadButton))
                            .add<ButtonLayout>()
                        ))
                    .addContainer("BottomBox", bottomBox => bottomBox
                        .injectionKey(nameof(bottomSection))
                        .vertical(anchor: TextAnchor.UpperCenter)
                        .addContainer("BottomInnerBox", innerBox => innerBox
                            .addAndConfigure<GapListLayout>(layout => {
                                layout.layoutAlignment = RectTransform.Axis.Vertical;
                                layout.childAlignment = TextAnchor.UpperCenter;
                                layout.elementsUntilGap = 0;
                                layout.countElementsFromFront = false;
                                layout.spacing = 20;
                                layout.expandChildThickness = true;
                            })
                            .addContainer("BottomBox1", container => container
                                .addAndConfigure<GapListLayout>(layout => {
                                    layout.layoutAlignment = RectTransform.Axis.Horizontal;
                                    layout.childAlignment = TextAnchor.MiddleCenter;
                                    layout.elementsUntilGap = 0;
                                    layout.spacing = 20;
                                })
                                .add(WS.textLine.setLocalizationKey("CheeseRamMenu.AddressLines"))
                                .add(WS.slider
                                    .injectionKey(nameof(addressPegSlider))
                                    .fixedSize(500, 45)
                                    .setInterval(1)
                                    .setMin(4)
                                    .setMax(24)
                                )
                            )
                            .addContainer("BottomBox2", container => container
                                .addAndConfigure<GapListLayout>(layout => {
                                    layout.layoutAlignment = RectTransform.Axis.Horizontal;
                                    layout.childAlignment = TextAnchor.MiddleCenter;
                                    layout.elementsUntilGap = 0;
                                    layout.spacing = 20;
                                })
                                .add(WS.textLine.setLocalizationKey("CheeseRamMenu.BitWidth"))
                                .add(WS.slider
                                    .injectionKey(nameof(widthPegSlider))
                                    .fixedSize(500, 45)
                                    .setInterval(1)
                                    .setMin(1)
                                    .setMax(64)
                                )
                            )
                        )
                    )
                )
                .add<RamMenu>()
                .build();
        }

        [AssignMe]
        public TMP_InputField filePathInputField;
        [AssignMe]
        public HoverButton loadButton;
        [AssignMe]
        public HoverButton saveButton;
        [AssignMe]
        public InputSlider addressPegSlider;
        [AssignMe]
        public InputSlider widthPegSlider;
        [AssignMe]
        public GameObject bottomSection;
        // [AssignMe]

        [AssignMe]
        public GameObject buttonsSection;

        private bool isComponentResizable;

        protected override void OnStartEditing()
        {
            // errorText.SetActive(false);
            if (FirstComponentBeingEdited.ClientCode is RamResizableClient)
            {
                var num_inputs = FirstComponentBeingEdited.Component.Data.InputCount;
                var num_outputs = FirstComponentBeingEdited.Component.Data.OutputCount;
                addressPegSlider.SetValueWithoutNotify(num_inputs - 3 - num_outputs);
                widthPegSlider.SetValueWithoutNotify(num_outputs);
                bottomSection.SetActive(true);
                isComponentResizable = true;
            }
            else
            {
                var num_inputs = FirstComponentBeingEdited.Component.Data.InputCount;
                var num_outputs = FirstComponentBeingEdited.Component.Data.OutputCount;
                addressPegSlider.SetValueWithoutNotify(num_inputs - 3 - num_outputs);
                widthPegSlider.SetValueWithoutNotify(num_outputs);
                bottomSection.SetActive(false);
                isComponentResizable = false;
            }
        }

        public override void Initialize()
        {
            base.Initialize();
            loadButton.OnClickEnd += LoadFile;
            saveButton.OnClickEnd += SaveFile;
            addressPegSlider.OnValueChangedInt += AddressCountChanged;
            widthPegSlider.OnValueChangedInt += BitwidthChanged;
            if (Application.platform == RuntimePlatform.WindowsPlayer)
            {
                filePathInputField.gameObject.SetActive(false);
            }
        }

        private void BitwidthChanged(int newBitwidth)
        {
            if(!isComponentResizable)
            {
                return;
            }
            BuildRequestManager.SendBuildRequest(new BuildRequest_ChangeDynamicComponentPegCounts(
                FirstComponentBeingEdited.Address,
                newBitwidth + 3 + addressPegSlider.ValueAsInt,
                newBitwidth
            ));
        }

        private void AddressCountChanged(int newAddressBitWidth)
        {
            if(!isComponentResizable)
            {
                return;
            }
            BuildRequestManager.SendBuildRequest(new BuildRequest_ChangeDynamicComponentPegCounts(
                FirstComponentBeingEdited.Address,
                newAddressBitWidth + 3 + widthPegSlider.ValueAsInt,
                widthPegSlider.ValueAsInt
            ));
        }

        private string GetFilePath(bool save)
        {
            if (Application.platform == RuntimePlatform.WindowsPlayer)
            {
                var dialog = save ? (FileDialog)new SaveFileDialog() : new OpenFileDialog();
                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    return dialog.FileName;
                }
                else
                {
                    return "";
                }
            }
            else
            {
                return filePathInputField.text;
            }
        }
        
        private void LoadFile()
        {
            var filePath = GetFilePath(true);
            if (string.IsNullOrEmpty(filePath)) return;
            var loadable = (FileLoadable) FirstComponentBeingEdited.ClientCode;
            if (File.Exists(filePath))
            {
                var bytes = File.ReadAllBytes(filePath);
                var lineWriter = LConsole.BeginLine();
                loadable.Load(bytes, lineWriter, true);
                lineWriter.End();
            }
            else
            {
                // errorText.SetActive(true);
                LConsole.WriteLine($"Unable to load file rich text <mspace=0.65em>'<noparse>{filePath}</noparse>'</mspace> as it does not exist");
            }
        }

        private void SaveFile()
        {
            var filePath = GetFilePath(true);
            if (string.IsNullOrEmpty(filePath)) return;
            var savable = (FileSavable)FirstComponentBeingEdited.ClientCode;
            savable.Save(true, Save);
            return;

            void Save(byte[] data)
            {
                try
                {
                    File.WriteAllBytes(filePath, data);
                    LConsole.WriteLine("Successfully opened file");
                }
                catch (Exception e)
                {
                    LConsole.PrintException(e);
                }
            }
        }

        protected override IEnumerable<string> GetTextIDsOfComponentTypesThatCanBeEdited()
        {
            return new string[] {
                "CheeseUtilMod.Ram4aX1b",
                "CheeseUtilMod.Ram8aX1b",
                "CheeseUtilMod.Ram16aX1b",
                "CheeseUtilMod.Ram4aX4b",
                "CheeseUtilMod.Ram8aX4b",
                "CheeseUtilMod.Ram16aX4b",
                "CheeseUtilMod.Ram4aX8b",
                "CheeseUtilMod.Ram8aX8b",
                "CheeseUtilMod.Ram16aX8b",
                "CheeseUtilMod.Ram4aX16b",
                "CheeseUtilMod.Ram8aX16b",
                "CheeseUtilMod.Ram16aX16b",
                "CheeseUtilMod.RamResizable",
            };
        }
    }
}
