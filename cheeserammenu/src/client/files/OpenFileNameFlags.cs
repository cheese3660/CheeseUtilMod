namespace CheeseRamMenu.Client.Files
{
    public enum OpenFileNameFlags : uint
    {
        AllowMultiselect = 0x00000200U,
        CreatePrompt = 0x00002000U,
        DontAddToRecent = 0x02000000U,
        EnableHook = 0x00000020U,
        EnableIncludeNotify = 0x00400000U,
        EnableSizing = 0x00800000U,
        EnableTemplate = 0x00000040U,
        EnableTemplateHandle = 0x00000080U,
        Explorer = 0x00080000U,
        ExtensionDifferent = 0x00000400U,
        FileMustExist = 0x00001000U,
        ForceShowHidden = 0x10000000U,
        HideReadOnly = 0x00000004U,
        LongNames = 0x00200000U,
        NoChangeDir = 0x00000008U,
        NoDereferenceLinks = 0x00100000U,
        NoLongNames = 0x00040000U,
        NoNetworkButton = 0x00020000U,
        NoReadOnlyReturn = 0x00008000U,
        NoTestFileCreate = 0x00010000U,
        NoValidate = 0x00000100U,
        OverwritePrompt = 0x00000002U,
        PathMustExist = 0x00000800U,
        ReadOnly = 0x00000001U,
        ShareAware = 0x00004000U,
        ShowHelp = 0x00000010U
    }
}