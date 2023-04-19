using System.Reflection;
using ChatterPlugin.Windows;
using Dalamud.Interface.Windowing;
using Dalamud.Logging;
using Dalamud.Plugin;

namespace ChatterPlugin;

/*
11:55:44.213 | INF [IME] Enabled!
11:55:44.214 | DBG [SVC] Service<"DalamudIME">: Construction complete
11:55:44.259 | DBG [SVC] Service<"DalamudAtkTweaks">: Construction complete
11:55:44.260 | DBG [SVC] Service<"DalamudInterface">: Construction complete
11:55:44.260 | DBG [SVC] Service<"PluginImageCache">: Construction complete
11:55:44.310 | INF [PLUGINM] ============= LoadPluginsSync(DrawAvailableSync) START =============
11:55:44.311 | INF [PLUGINM] ============= LoadPluginsSync(DrawAvailableSync) END =============
11:55:44.311 | INF [PLUGINM] ============= LoadPluginsAsync(DrawAvailableAsync) START =============
11:55:44.315 | INF [PLUGINM] Loading plugin Globetrotter
11:55:44.315 | INF [PLUGINM] Loading plugin RezPls
11:55:44.315 | INF [PLUGINM] Loading plugin Accountant
11:55:44.315 | INF [PLUGINM] Loading plugin QoL Bar
11:55:44.315 | INF [PLUGINM] Loading dev plugin Chatter Plugin
11:55:44.315 | INF [PLUGINM] Loading plugin Pixel Perfect
11:55:44.315 | INF [PLUGINM] Loading plugin Macro Chain
11:55:44.315 | INF [PLUGINM] Loading plugin Sonar
11:55:44.315 | INF [PLUGINM] Loading plugin Kapture
11:55:44.315 | INF [PLUGINM] Loading plugin Penny Pincher
11:55:44.315 | INF [PLUGINM] Loading plugin XIV Combo
11:55:44.497 | INF [PLUGINM] Loading plugin Item Vendor Location
11:55:44.498 | INF [LOCALPLUGIN] Loading Globetrotter.dll
11:55:44.499 | INF [LOCALPLUGIN] Loading PennyPincher.dll
11:55:44.499 | INF [LOCALPLUGIN] Loading MacroChain.dll
11:55:44.499 | INF [LOCALPLUGIN] Loading Kapture.dll
11:55:44.542 | INF [PLUGINM] Loading plugin GatherBuddy
11:55:44.543 | INF [LOCALPLUGIN] Finished loading Globetrotter.dll
11:55:44.543 | INF [PLUGINM] Loading plugin Death Recap
11:55:44.585 | INF [PLUGINM] Loading plugin Burning Down the House
11:55:44.620 | INF [LOCALPLUGIN] Loading SonarPlugin.dll
11:55:44.625 | INF [LOCALPLUGIN] Finished loading MacroChain.dll
11:55:44.627 | DBG [SonarPlugin] Initializing Sonar [Stub]
11:55:44.656 | INF [LOCALPLUGIN] Finished loading SonarPlugin.dll
11:55:44.659 | INF [LOCALPLUGIN] Loading RezPls.dll
11:55:44.739 | INF [LOCALPLUGIN] Loading ItemSearchPlugin.dll
11:55:44.773 | INF [LOCALPLUGIN] Finished loading ItemSearchPlugin.dll
11:55:44.784 | INF [LOCALPLUGIN] Loading Glamaholic.dll
11:55:44.837 | INF [LOCALPLUGIN] Loading BDTHPlugin.dll
11:55:44.845 | DBG [SonarPlugin] Starting Sonar
11:55:44.872 | INF [LOCALPLUGIN] Loading ItemVendorLocation.dll
11:55:44.885 | INF [LOCALPLUGIN] Loading SimpleTweaksPlugin.dll
11:55:44.893 | INF [LOCALPLUGIN] Finished loading PennyPincher.dll
11:55:44.909 | INF [LOCALPLUGIN] Loading DeathRecap.dll
11:55:44.931 | INF [LOCALPLUGIN] Finished loading DeathRecap.dll
11:55:44.931 | INF [LOCALPLUGIN] Finished loading BDTHPlugin.dll
11:55:45.085 | DBG [RezPls] UpdateParty address 0x00007FF67257F5F0, baseOffset 0x0000000000BDF5F0.
11:55:45.157 | DBG [RezPls] Hooked onto UpdateParty at address 0x00007FF67257F5F0.
11:55:45.166 | INF [LOCALPLUGIN] Finished loading RezPls.dll
11:55:45.261 | INF [GameFontManager] NewFontRef: Queueing RebuildFonts because "GameFontStyle(Axis18, 18pt, skew=0, weight=0)" has been requested.
11:55:45.263 | INF [SimpleTweaksPlugin] Tweak Blacklist:
11:55:45.263 | INF [SimpleTweaksPlugin] StopCraftingButton
11:55:45.263 | INF [SimpleTweaksPlugin] FixedShadowDistance
11:55:45.263 | INF [SimpleTweaksPlugin] RefreshMarketPrices
11:55:45.263 | INF [SimpleTweaksPlugin] TooltipTweaks
11:55:45.268 | DBG [SimpleTweaksPlugin] Initalizing Tweak: AutoLockHotbar
11:55:45.269 | DBG [SimpleTweaksPlugin] Enable: AutoLockHotbar
11:55:45.275 | DBG [SimpleTweaksPlugin] Initalizing Tweak: AutoOpenCommendWindow
11:55:45.275 | DBG [SimpleTweaksPlugin] Enable: AutoOpenCommendWindow
11:55:45.276 | DBG [SimpleTweaksPlugin] Initalizing Tweak: AutoOpenLootWindow
11:55:45.276 | INF [LOCALPLUGIN] Loading GatherBuddy.dll
11:55:45.276 | DBG [SimpleTweaksPlugin] Initalizing Tweak: BaitCommand
11:55:45.277 | DBG [SimpleTweaksPlugin] Initalizing Tweak: CharaCardCommand
11:55:45.277 | DBG [SimpleTweaksPlugin] Initalizing Tweak: CharacterClassSwitcher
11:55:45.288 | DBG [SimpleTweaksPlugin] Initalizing Tweak: CharaViewIncreasedZoom
11:55:45.288 | DBG [SimpleTweaksPlugin] Enable: CharaViewIncreasedZoom
11:55:45.289 | DBG [SimpleTweaksPlugin] Initalizing Tweak: ChatTweaks
11:55:45.364 | DBG [SimpleTweaksPlugin] Enable: ChatTweaks
11:55:45.366 | INF [SimpleTweaksPlugin] Enable: Chat Name Colours @ Chat Tweaks
11:55:45.370 | INF [SimpleTweaksPlugin] Enable: Clickable Links in Chat @ Chat Tweaks
11:55:45.371 | INF [SimpleTweaksPlugin] Enable: Smart AutoScroll @ Chat Tweaks
11:55:45.387 | DBG [SimpleTweaksPlugin] Initalizing Tweak: ChrDirCommand
11:55:45.387 | DBG [SimpleTweaksPlugin] Initalizing Tweak: CombatMovementControl
11:55:45.388 | DBG [SimpleTweaksPlugin] Initalizing Tweak: CommandAlias
11:55:45.388 | DBG [SimpleTweaksPlugin] Initalizing Tweak: ContentsFinderConfirmClassSwitch
11:55:45.388 | DBG [SimpleTweaksPlugin] Enable: ContentsFinderConfirmClassSwitch
11:55:45.394 | DBG [SimpleTweaksPlugin] Initalizing Tweak: DataCentreOnTitleScreen
11:55:45.394 | DBG [SimpleTweaksPlugin] Initalizing Tweak: DisableClickTargeting
11:55:45.394 | DBG [SimpleTweaksPlugin] Initalizing Tweak: DisableMountVolumeChange
11:55:45.394 | DBG [SimpleTweaksPlugin] Initalizing Tweak: DisableMouseCameraControl
11:55:45.394 | DBG [SimpleTweaksPlugin] Initalizing Tweak: DisableTitleScreenMovie
11:55:45.394 | DBG [SimpleTweaksPlugin] Enable: DisableTitleScreenMovie
11:55:45.394 | DBG [SimpleTweaksPlugin] Initalizing Tweak: EmoteLogSubcommand
11:55:45.394 | DBG [SimpleTweaksPlugin] Initalizing Tweak: EstateListCommand
11:55:45.395 | DBG [SimpleTweaksPlugin] Enable: EstateListCommand
11:55:45.398 | DBG [SimpleTweaksPlugin] Initalizing Tweak: ExtendedMacroIcon
11:55:45.399 | DBG [SimpleTweaksPlugin] Initalizing Tweak: FixedShadowDistance
11:55:45.399 | DBG [SimpleTweaksPlugin] Initalizing Tweak: FixTarget
11:55:45.400 | DBG [SimpleTweaksPlugin] Enable: FixTarget
11:55:45.401 | DBG [SimpleTweaksPlugin] Initalizing Tweak: HideMouseAfterInactivity
11:55:45.401 | DBG [SimpleTweaksPlugin] Initalizing Tweak: HighResScreenshots
11:55:45.402 | DBG [SimpleTweaksPlugin] Initalizing Tweak: HouseLightCommand
11:55:45.403 | DBG [SimpleTweaksPlugin] Initalizing Tweak: JokeTweaks
11:55:45.404 | DBG [SimpleTweaksPlugin] Initalizing Tweak: KeyInterrupt
11:55:45.404 | DBG [SimpleTweaksPlugin] Initalizing Tweak: LongVeil
11:55:45.404 | DBG [SimpleTweaksPlugin] Initalizing Tweak: MainCommandCommand
11:55:45.404 | DBG [SimpleTweaksPlugin] Initalizing Tweak: MinionAway
11:55:45.405 | DBG [SimpleTweaksPlugin] Initalizing Tweak: MoreGearSets
11:55:45.405 | DBG [SimpleTweaksPlugin] Initalizing Tweak: NoSellList
11:55:45.405 | DBG [SimpleTweaksPlugin] Initalizing Tweak: PidCommand
11:55:45.405 | DBG [SimpleTweaksPlugin] Initalizing Tweak: QuickSellItems
11:55:45.406 | DBG [SimpleTweaksPlugin] Initalizing Tweak: RecommendEquipCommand
11:55:45.406 | DBG [SimpleTweaksPlugin] Initalizing Tweak: RefreshMarketPrices
11:55:45.406 | DBG [SimpleTweaksPlugin] Initalizing Tweak: RememberQuickGathering
11:55:45.407 | DBG [SimpleTweaksPlugin] Initalizing Tweak: SanctuarySprintReplacer
11:55:45.407 | DBG [SimpleTweaksPlugin] Initalizing Tweak: ScreenshotFileName
11:55:45.407 | DBG [SimpleTweaksPlugin] Initalizing Tweak: SetOptionCommand
11:55:45.422 | DBG [SimpleTweaksPlugin] Initalizing Tweak: SmartStrafe
11:55:45.422 | DBG [SimpleTweaksPlugin] Initalizing Tweak: SyncCrafterBars
11:55:45.422 | DBG [SimpleTweaksPlugin] Initalizing Tweak: SyncGathererBars
11:55:45.424 | DBG [SimpleTweaksPlugin] Initalizing Tweak: SystemConfigInGroupPose
11:55:45.425 | DBG [SimpleTweaksPlugin] Initalizing Tweak: TooltipTweaks
11:55:45.541 | INF [SonarPlugin.Dalamud] Initializing Sonar
11:55:47.057 | DBG [SonarPlugin.Dalamud] Creating WavePlayer
11:55:47.077 | DBG [SonarPlugin.Dalamud] WavePlayer type: NAudio.Wave.WasapiOut
11:55:47.207 | INF [DryIoc] PlayerTracker initialized
11:55:47.236 | INF [DryIoc] Hunt Notifier Initialized
11:55:47.236 | INF [DryIoc] Fate Notifier Initialized
11:55:47.237 | INF [DryIoc] Lumina Data initialized
11:55:47.239 | INF [DryIoc] Sonar Main Overlay Initialized
11:55:47.976 | WRN [HITCH] Long "UiBuilder(BDTHPlugin)" detected, 161.9238ms > 100ms - check in the plugin stats window.
11:55:48.111 | INF [DryIoc] Sonar Commands Initialized
11:55:48.112 | DBG [SonarPlugin.Dalamud] Starting SonarCommands
11:55:48.121 | DBG [SonarPlugin.Dalamud] Adding command handlers for "SonarCommands"
11:55:48.129 | DBG [SonarPlugin.Dalamud] Starting PlayerProvider
11:55:48.130 | INF [DryIoc] FateTracker Initialized
11:55:48.130 | DBG [SonarPlugin.Dalamud] Starting SonarFateProvider
11:55:48.131 | INF [DryIoc] MobTracker Initialized
11:55:48.131 | DBG [SonarPlugin.Dalamud] Starting SonarHuntProvider
11:55:48.131 | DBG [SonarPlugin.Dalamud] Mixer Inputs: 0
11:55:48.131 | DBG [SonarPlugin.Dalamud] Starting FateNotifier
11:55:48.132 | DBG [SonarPlugin.Dalamud] Starting HuntNotifier
11:55:48.133 | DBG [SonarPlugin.Dalamud] Starting SonarMainOverlay
11:55:48.144 | INF [LOCALPLUGIN] Finished loading Glamaholic.dll
11:55:48.165 | DBG [SimpleTweaksPlugin] Enable: TooltipTweaks
11:55:48.172 | INF [SimpleTweaksPlugin] Enable: Show Desynthesis Skill @ Tooltip Tweaks
11:55:48.200 | INF [SimpleTweaksPlugin] Enable: Show Painting Preview @ Tooltip Tweaks
11:55:48.204 | INF [SimpleTweaksPlugin] Enable: Track Faded Orchestrion Rolls @ Tooltip Tweaks
11:55:48.209 | DBG [SimpleTweaksPlugin] Initalizing Tweak: TreasureHuntTargets
11:55:48.210 | DBG [SimpleTweaksPlugin] Initalizing Tweak: TryOnCorrectItem
11:55:48.211 | DBG [SimpleTweaksPlugin] Enable: TryOnCorrectItem
11:55:48.222 | DBG [SonarPlugin.Dalamud] WavePlayer Disposed: NAudio.Wave.WasapiOut
11:55:48.237 | DBG [SimpleTweaksPlugin] Initalizing Tweak: UiAdjustments
11:55:48.285 | INF [Sonar] Sonar is ready
11:55:48.297 | INF [Sonar] Connected to Sonar
11:55:48.586 | DBG [SimpleTweaksPlugin] Enable: UiAdjustments
11:55:48.586 | INF [SimpleTweaksPlugin] Enable: Accurate Venture Times @ UI Tweaks
11:55:48.613 | INF [SimpleTweaksPlugin] Enable: Adjust Equipment Positions @ UI Tweaks
11:55:48.620 | INF [SimpleTweaksPlugin] Enable: Cleaner World Visit Menu @ UI Tweaks
11:55:48.620 | INF [SimpleTweaksPlugin] Enable: Combo Timer @ UI Tweaks
11:55:48.629 | INF [SimpleTweaksPlugin] Enable: Default to max when selling Triple Triad Cards @ UI Tweaks
11:55:48.630 | INF [SimpleTweaksPlugin] Enable: Extended Desynthesis Window @ UI Tweaks
11:55:48.681 | INF [SimpleTweaksPlugin] Enable: Hide Unwanted Banners @ UI Tweaks
11:55:48.701 | INF [SimpleTweaksPlugin] Enable: Improved Crafting Log @ UI Tweaks
11:55:48.710 | INF [SimpleTweaksPlugin] Enable: Item Level in Examine @ UI Tweaks
11:55:48.727 | INF [SimpleTweaksPlugin] Enable: Shield on HP Bar @ UI Tweaks
11:55:48.755 | INF [SimpleTweaksPlugin] Enable: Target HP @ UI Tweaks
11:55:48.798 | INF [SimpleTweaksPlugin] Enable: Time Until GP Max @ UI Tweaks
11:55:48.811 | INF [SimpleTweaksPlugin] Enable: Timer on Duty Waiting @ UI Tweaks
11:55:48.812 | DBG [SimpleTweaksPlugin] Initalizing Tweak: HideHotbarLock
11:55:48.813 | DBG [SimpleTweaksPlugin] Initalizing Tweak: KeepOpen
11:55:48.828 | INF [LOCALPLUGIN] Finished loading SimpleTweaksPlugin.dll
11:55:49.475 | INF [LOCALPLUGIN] Finished loading ItemVendorLocation.dll
11:55:50.076 | INF [LOCALPLUGIN] Finished loading Kapture.dll
11:55:54.977 | DBG [GatherBuddy] FishLogData address 0x00007FF673AA08D4, baseOffset 0x00000000021008D4.
11:55:54.977 | DBG [GatherBuddy] SpearFishLogData address 0x00007FF673AA0987, baseOffset 0x0000000002100987.
11:55:54.981 | DBG [GatherBuddy] EventFramework address 0x00007FF673A9A9D8, baseOffset 0x00000000020FA9D8.
11:55:54.981 | DBG [GatherBuddy] CurrentBait address 0x00007FF673AA097C, baseOffset 0x000000000210097C.
11:55:54.982 | DBG [GatherBuddy] CurrentWeather address 0x00007FF673A1C500, baseOffset 0x000000000207C500.
11:55:54.982 | DBG [GatherBuddy] SeTugType address 0x00007FF673A9A5C0, baseOffset 0x00000000020FA5C0.
11:55:54.989 | DBG [GatherBuddy] ProcessChatBox address 0x00007FF671FB3680, baseOffset 0x0000000000613680.
11:55:55.259 | DBG [GatherBuddy] PlaySound address 0x00007FF671A46180, baseOffset 0x00000000000A6180.
11:55:55.328 | DBG [GatherBuddy] UpdateFishCatch address 0x00007FF67268C7C0, baseOffset 0x0000000000CEC7C0.
11:55:55.332 | DBG [GatherBuddy] Hooked onto UpdateFishCatch at address 0x00007FF67268C7C0.
11:55:57.111 | INF [LOCALPLUGIN] Finished loading GatherBuddy.dll
11:55:57.112 | INF [PLUGINM] ============= LoadPluginsAsync(DrawAvailableAsync) END =============
11:55:57.112 | INF [PLUGINM] Loaded plugins on boot
11:55:57.132 | INF Saved cache to "C:\Users\james\AppData\Roaming\XIVLauncher\addon\Hooks\7.5.0.2\cachedSigs\2023.03.24.0000.0000.json"
11:56:38.974 | INF [SimpleTweaksPlugin] Create Custom BG Image Node#0
11:56:38.975 | INF [SimpleTweaksPlugin] Create Custom BG Image Node#1
11:56:41.211 | DBG TerritoryType changed: 339
11:56:41.538 | INF [PLUGINM] Starting plugin update
11:56:41.539 | INF [PLUGINM] Plugin update OK.
11:56:41.542 | WRN [HITCH] Long "GameNetworkDown" detected, 33.9199ms > 30ms - check in the plugin stats window.
11:56:41.561 | DBG Is login
11:56:54.022 | WRN [HITCH] Long "FrameworkUpdate" detected, 212.8924ms > 50ms - check in the plugin stats window.
11:56:55.718 | DBG TerritoryType changed: 130
11:57:06.763 | DBG TerritoryType changed: 131
11:58:22.655 | WRN [HITCH] Long "GameNetworkDown" detected, 33.91ms > 30ms - check in the plugin stats window.
12:01:14.994 | DBG TerritoryType changed: 130
12:03:15.585 | INF [SimpleTweaksPlugin] Try on from : GrandCompanySupplyList
12:04:59.121 | DBG TerritoryType changed: 535
12:06:36.065 | DBG TerritoryType changed: 130
12:07:15.669 | DBG TerritoryType changed: 141
12:09:14.604 | DBG TerritoryType changed: 129
12:09:44.579 | DBG HandlePrintMessageDetour Sender modified: System.Byte[](1046357002912) -> Dalamud.Game.Libc.StdString(3158158784992)
12:09:56.604 | DBG HandlePrintMessageDetour String modified: System.Byte[](3157113651928) -> Dalamud.Game.Libc.StdString(3157756242656)
12:10:50.106 | DBG TerritoryType changed: 129
12:10:50.107 | WRN [HITCH] Long "GameNetworkDown" detected, 33.1611ms > 30ms - check in the plugin stats window.
12:10:50.850 | DBG HandlePrintMessageDetour String modified: System.Byte[](1046357004128) -> Dalamud.Game.Libc.StdString(3157756255168)
12:11:41.504 | DBG Handled RMT ad: 5GOLD.COM--Buy FFXIV Gil Cheapest,100% Handwork Guaranteed,24/7 online service[5% OFF Code;GOLD]. $%^&*
12:12:03.712 | DBG TerritoryType changed: 130
12:12:09.942 | DBG HandlePrintMessageDetour Sender modified: System.Byte[](1046357002912) -> Dalamud.Game.Libc.StdString(3158945056656)
12:12:41.468 | DBG HandlePrintMessageDetour Sender modified: System.Byte[](1046357002912) -> Dalamud.Game.Libc.StdString(3157756285360)
12:12:48.097 | DBG HandlePrintMessageDetour Sender modified: System.Byte[](1046357002912) -> Dalamud.Game.Libc.StdString(3158158781728)
12:12:58.730 | DBG HandlePrintMessageDetour Sender modified: System.Byte[](1046357002912) -> Dalamud.Game.Libc.StdString(3158718099280)
12:12:59.106 | DBG HandlePrintMessageDetour Sender modified: System.Byte[](1046357002912) -> Dalamud.Game.Libc.StdString(3158718110432)
12:12:59.528 | DBG HandlePrintMessageDetour Sender modified: System.Byte[](1046357002912) -> Dalamud.Game.Libc.StdString(3158718097104)
12:13:03.066 | DBG HandlePrintMessageDetour Sender modified: System.Byte[](1046357002912) -> Dalamud.Game.Libc.StdString(3158718108528)
12:13:08.493 | DBG HandlePrintMessageDetour Sender modified: System.Byte[](1046357002912) -> Dalamud.Game.Libc.StdString(3158718103360)
12:13:13.558 | DBG HandlePrintMessageDetour Sender modified: System.Byte[](1046357002912) -> Dalamud.Game.Libc.StdString(3158718101184)
12:13:22.481 | DBG HandlePrintMessageDetour Sender modified: System.Byte[](1046357002912) -> Dalamud.Game.Libc.StdString(3157285062848)
12:13:22.481 | DBG HandlePrintMessageDetour Sender modified: System.Byte[](1046357002912) -> Dalamud.Game.Libc.StdString(3157285059856)
12:13:22.618 | DBG HandlePrintMessageDetour Sender modified: System.Byte[](1046357002912) -> Dalamud.Game.Libc.StdString(3157285066384)
12:13:28.910 | DBG HandlePrintMessageDetour Sender modified: System.Byte[](1046357002912) -> Dalamud.Game.Libc.StdString(3157285056592)
12:13:41.044 | DBG HandlePrintMessageDetour Sender modified: System.Byte[](1046357002912) -> Dalamud.Game.Libc.StdString(3157285057952)
12:13:45.676 | DBG HandlePrintMessageDetour Sender modified: System.Byte[](1046357002912) -> Dalamud.Game.Libc.StdString(3157392004864)
12:13:48.177 | DBG HandlePrintMessageDetour Sender modified: System.Byte[](1046357002912) -> Dalamud.Game.Libc.StdString(3157392016560)
12:13:48.178 | DBG HandlePrintMessageDetour Sender modified: System.Byte[](1046357002912) -> Dalamud.Game.Libc.StdString(3157392004864)
12:13:50.467 | DBG HandlePrintMessageDetour Sender modified: System.Byte[](1046357002912) -> Dalamud.Game.Libc.StdString(3157392010576)
12:13:53.356 | DBG HandlePrintMessageDetour Sender modified: System.Byte[](1046357002912) -> Dalamud.Game.Libc.StdString(3157392013296)
12:13:56.186 | DBG HandlePrintMessageDetour Sender modified: System.Byte[](1046357002912) -> Dalamud.Game.Libc.StdString(3157392007312)
12:14:00.708 | DBG HandlePrintMessageDetour Sender modified: System.Byte[](1046357002912) -> Dalamud.Game.Libc.StdString(3157392007312)
12:14:08.063 | DBG HandlePrintMessageDetour Sender modified: System.Byte[](1046357002912) -> Dalamud.Game.Libc.StdString(3157392012480)
12:14:09.029 | DBG HandlePrintMessageDetour Sender modified: System.Byte[](1046357002912) -> Dalamud.Game.Libc.StdString(3157392004864)
12:14:16.365 | DBG HandlePrintMessageDetour Sender modified: System.Byte[](1046357002912) -> Dalamud.Game.Libc.StdString(3157760352384)
12:14:17.587 | DBG HandlePrintMessageDetour Sender modified: System.Byte[](1046357002912) -> Dalamud.Game.Libc.StdString(3157760359184)
12:14:20.618 | DBG HandlePrintMessageDetour Sender modified: System.Byte[](1046357002912) -> Dalamud.Game.Libc.StdString(3157392004864)
12:14:23.449 | DBG HandlePrintMessageDetour Sender modified: System.Byte[](1046357002912) -> Dalamud.Game.Libc.StdString(3157392016560)
12:14:27.635 | DBG HandlePrintMessageDetour Sender modified: System.Byte[](1046357002912) -> Dalamud.Game.Libc.StdString(3157076251520)
12:14:29.772 | DBG HandlePrintMessageDetour Sender modified: System.Byte[](1046357002912) -> Dalamud.Game.Libc.StdString(3157076257232)
12:14:32.698 | DBG HandlePrintMessageDetour Sender modified: System.Byte[](1046357002912) -> Dalamud.Game.Libc.StdString(3158158785264)
12:14:35.105 | DBG HandlePrintMessageDetour Sender modified: System.Byte[](1046357002912) -> Dalamud.Game.Libc.StdString(3158158785264)
12:14:40.118 | DBG HandlePrintMessageDetour Sender modified: System.Byte[](1046357002912) -> Dalamud.Game.Libc.StdString(3158158785264)
12:14:40.244 | DBG HandlePrintMessageDetour Sender modified: System.Byte[](1046357002912) -> Dalamud.Game.Libc.StdString(3158158785264)
12:14:41.465 | DBG HandlePrintMessageDetour Sender modified: System.Byte[](1046357002912) -> Dalamud.Game.Libc.StdString(3158158785264)
12:14:43.305 | DBG HandlePrintMessageDetour Sender modified: System.Byte[](1046357002912) -> Dalamud.Game.Libc.StdString(3157285058224)
12:14:44.695 | DBG HandlePrintMessageDetour Sender modified: System.Byte[](1046357002912) -> Dalamud.Game.Libc.StdString(3158158785264)
12:14:51.658 | DBG HandlePrintMessageDetour Sender modified: System.Byte[](1046357002912) -> Dalamud.Game.Libc.StdString(3158158785264)
12:14:52.672 | DBG HandlePrintMessageDetour Sender modified: System.Byte[](1046357002912) -> Dalamud.Game.Libc.StdString(3157542162912)
12:15:05.083 | DBG HandlePrintMessageDetour Sender modified: System.Byte[](1046357002912) -> Dalamud.Game.Libc.StdString(3158158785264)
12:15:07.668 | DBG HandlePrintMessageDetour Sender modified: System.Byte[](1046357002912) -> Dalamud.Game.Libc.StdString(3158158785264)
12:15:11.393 | DBG HandlePrintMessageDetour Sender modified: System.Byte[](1046357002912) -> Dalamud.Game.Libc.StdString(3158158785264)
12:15:20.944 | DBG HandlePrintMessageDetour Sender modified: System.Byte[](1046357002912) -> Dalamud.Game.Libc.StdString(3157760360544)
12:15:26.029 | DBG HandlePrintMessageDetour Sender modified: System.Byte[](1046357002912) -> Dalamud.Game.Libc.StdString(3157756289712)
12:15:26.478 | DBG HandlePrintMessageDetour Sender modified: System.Byte[](1046357002912) -> Dalamud.Game.Libc.StdString(3157756278560)
12:15:28.459 | DBG HandlePrintMessageDetour Sender modified: System.Byte[](1046357002912) -> Dalamud.Game.Libc.StdString(3157756284544)
12:15:30.438 | DBG HandlePrintMessageDetour Sender modified: System.Byte[](1046357002912) -> Dalamud.Game.Libc.StdString(3157756287808)
12:15:31.315 | DBG HandlePrintMessageDetour Sender modified: System.Byte[](1046357002912) -> Dalamud.Game.Libc.StdString(3157756282912)
12:15:39.149 | DBG HandlePrintMessageDetour Sender modified: System.Byte[](1046357002912) -> Dalamud.Game.Libc.StdString(3157392010032)
12:15:40.810 | DBG HandlePrintMessageDetour Sender modified: System.Byte[](1046357002912) -> Dalamud.Game.Libc.StdString(3157392004864)
12:15:44.186 | DBG HandlePrintMessageDetour Sender modified: System.Byte[](1046357002912) -> Dalamud.Game.Libc.StdString(3157756287808)
12:15:48.892 | DBG HandlePrintMessageDetour Sender modified: System.Byte[](1046357002912) -> Dalamud.Game.Libc.StdString(3158718109072)
12:15:53.532 | DBG HandlePrintMessageDetour Sender modified: System.Byte[](1046357002912) -> Dalamud.Game.Libc.StdString(3158718109072)
12:15:56.370 | DBG HandlePrintMessageDetour Sender modified: System.Byte[](1046357002912) -> Dalamud.Game.Libc.StdString(3158158785264)
12:16:00.086 | DBG HandlePrintMessageDetour Sender modified: System.Byte[](1046357002912) -> Dalamud.Game.Libc.StdString(3158158785264)
12:16:02.482 | DBG HandlePrintMessageDetour Sender modified: System.Byte[](1046357002912) -> Dalamud.Game.Libc.StdString(3158158785264)
12:16:12.733 | DBG HandlePrintMessageDetour Sender modified: System.Byte[](1046357002912) -> Dalamud.Game.Libc.StdString(3158158785264)
12:16:14.873 | DBG HandlePrintMessageDetour Sender modified: System.Byte[](1046357002912) -> Dalamud.Game.Libc.StdString(3158158785264)
12:16:17.981 | DBG HandlePrintMessageDetour Sender modified: System.Byte[](1046357002912) -> Dalamud.Game.Libc.StdString(3158158785264)
12:16:19.268 | DBG HandlePrintMessageDetour Sender modified: System.Byte[](1046357002912) -> Dalamud.Game.Libc.StdString(3158158785264)
12:16:24.991 | DBG HandlePrintMessageDetour Sender modified: System.Byte[](1046357002912) -> Dalamud.Game.Libc.StdString(3157285056048)
12:16:34.892 | DBG HandlePrintMessageDetour Sender modified: System.Byte[](1046357002912) -> Dalamud.Game.Libc.StdString(3158158785264)
12:16:49.995 | DBG HandlePrintMessageDetour Sender modified: System.Byte[](1046357002912) -> Dalamud.Game.Libc.StdString(3158158785536)
12:17:31.217 | DBG HandlePrintMessageDetour Sender modified: System.Byte[](1046357002912) -> Dalamud.Game.Libc.StdString(3157285056592)
12:17:55.348 | DBG HandlePrintMessageDetour Sender modified: System.Byte[](1046357002912) -> Dalamud.Game.Libc.StdString(3157285061760)
12:17:55.946 | DBG HandlePrintMessageDetour Sender modified: System.Byte[](1046357002912) -> Dalamud.Game.Libc.StdString(3157285056048)
12:18:17.186 | DBG HandlePrintMessageDetour Sender modified: System.Byte[](1046357002912) -> Dalamud.Game.Libc.StdString(3157285056592)
12:18:24.294 | DBG HandlePrintMessageDetour Sender modified: System.Byte[](1046357002912) -> Dalamud.Game.Libc.StdString(3157285056048)
12:18:27.986 | DBG HandlePrintMessageDetour Sender modified: System.Byte[](1046357002912) -> Dalamud.Game.Libc.StdString(3157285060128)
12:18:34.209 | DBG HandlePrintMessageDetour Sender modified: System.Byte[](1046357002912) -> Dalamud.Game.Libc.StdString(3157285058224)
12:18:53.111 | DBG HandlePrintMessageDetour Sender modified: System.Byte[](1046357002912) -> Dalamud.Game.Libc.StdString(3157285058224)
12:18:56.222 | DBG HandlePrintMessageDetour Sender modified: System.Byte[](1046357002912) -> Dalamud.Game.Libc.StdString(3157285058224)
12:19:25.334 | DBG HandlePrintMessageDetour Sender modified: System.Byte[](1046357002912) -> Dalamud.Game.Libc.StdString(3158158785536)
12:19:31.408 | DBG HandlePrintMessageDetour Sender modified: System.Byte[](1046357002912) -> Dalamud.Game.Libc.StdString(3158158785264)
12:20:34.671 | DBG HandlePrintMessageDetour Sender modified: System.Byte[](1046357002912) -> Dalamud.Game.Libc.StdString(3157285056048)
12:20:49.939 | DBG HandlePrintMessageDetour Sender modified: System.Byte[](1046357002912) -> Dalamud.Game.Libc.StdString(3158158785264)
12:20:59.398 | DBG HandlePrintMessageDetour Sender modified: System.Byte[](1046357002912) -> Dalamud.Game.Libc.StdString(3158158785264)
12:21:08.125 | DBG HandlePrintMessageDetour Sender modified: System.Byte[](1046357002912) -> Dalamud.Game.Libc.StdString(3158158785264)
12:21:09.116 | DBG HandlePrintMessageDetour Sender modified: System.Byte[](1046357002912) -> Dalamud.Game.Libc.StdString(3158158785264)
12:21:15.282 | DBG HandlePrintMessageDetour Sender modified: System.Byte[](1046357002912) -> Dalamud.Game.Libc.StdString(3158158785264)
12:21:18.971 | DBG HandlePrintMessageDetour Sender modified: System.Byte[](1046357002912) -> Dalamud.Game.Libc.StdString(3158158785264)
12:21:33.424 | DBG [PLUGIN] Skipping reload of Chatter Plugin, file has changed again.
12:21:33.424 | DBG [PLUGIN] Skipping reload of Chatter Plugin, file has changed again.
12:21:33.434 | DBG [PLUGIN] Skipping reload of Chatter Plugin, state (Unloaded) is not Loaded, LoadError or UnloadError.
12:21:44.490 | DBG HandlePrintMessageDetour Sender modified: System.Byte[](1046357002912) -> Dalamud.Game.Libc.StdString(3157762892624)
12:21:52.132 | DBG HandlePrintMessageDetour Sender modified: System.Byte[](1046357002912) -> Dalamud.Game.Libc.StdString(3157762892624)
12:22:05.593 | DBG HandlePrintMessageDetour Sender modified: System.Byte[](1046357002912) -> Dalamud.Game.Libc.StdString(3158718099280)
12:22:24.251 | DBG HandlePrintMessageDetour Sender modified: System.Byte[](1046357002912) -> Dalamud.Game.Libc.StdString(3158158785264)
12:22:31.855 | DBG HandlePrintMessageDetour Sender modified: System.Byte[](1046357002912) -> Dalamud.Game.Libc.StdString(3158158785264)
12:22:35.980 | DBG HandlePrintMessageDetour Sender modified: System.Byte[](1046357002912) -> Dalamud.Game.Libc.StdString(3158158785264)
12:22:37.867 | DBG HandlePrintMessageDetour Sender modified: System.Byte[](1046357002912) -> Dalamud.Game.Libc.StdString(3158158785264)
12:22:56.816 | DBG HandlePrintMessageDetour Sender modified: System.Byte[](1046357002912) -> Dalamud.Game.Libc.StdString(3158158785264)
12:23:01.712 | DBG HandlePrintMessageDetour Sender modified: System.Byte[](1046357002912) -> Dalamud.Game.Libc.StdString(3158158785536)
12:23:02.413 | DBG HandlePrintMessageDetour Sender modified: System.Byte[](1046357002912) -> Dalamud.Game.Libc.StdString(3158158785536)
12:23:13.177 | DBG HandlePrintMessageDetour Sender modified: System.Byte[](1046357002912) -> Dalamud.Game.Libc.StdString(3158158785264)
12:23:17.586 | DBG HandlePrintMessageDetour Sender modified: System.Byte[](1046357002912) -> Dalamud.Game.Libc.StdString(3158158785264)
12:23:26.341 | DBG HandlePrintMessageDetour String modified: System.Byte[](3157113651928) -> Dalamud.Game.Libc.StdString(3158158785264)
12:23:27.022 | DBG HandlePrintMessageDetour Sender modified: System.Byte[](1046357002912) -> Dalamud.Game.Libc.StdString(3158158785536)
12:23:30.209 | DBG HandlePrintMessageDetour Sender modified: System.Byte[](1046357002912) -> Dalamud.Game.Libc.StdString(3158158785536)
12:23:34.025 | DBG HandlePrintMessageDetour String modified: System.Byte[](3157113651928) -> Dalamud.Game.Libc.StdString(3158158785536)
12:23:34.530 | DBG HandlePrintMessageDetour Sender modified: System.Byte[](1046357002912) -> Dalamud.Game.Libc.StdString(3158158785536)
12:23:36.598 | DBG HandlePrintMessageDetour Sender modified: System.Byte[](1046357002912) -> Dalamud.Game.Libc.StdString(3158158785264)
12:23:38.819 | DBG HandlePrintMessageDetour Sender modified: System.Byte[](1046357002912) -> Dalamud.Game.Libc.StdString(3158158785264)
12:23:39.599 | DBG HandlePrintMessageDetour String modified: System.Byte[](3157113651928) -> Dalamud.Game.Libc.StdString(3158158785264)
12:23:45.563 | DBG HandlePrintMessageDetour String modified: System.Byte[](3157113651928) -> Dalamud.Game.Libc.StdString(3158158785808)
12:23:48.598 | DBG HandlePrintMessageDetour Sender modified: System.Byte[](1046357002912) -> Dalamud.Game.Libc.StdString(3158158785264)
12:23:49.207 | DBG HandlePrintMessageDetour Sender modified: System.Byte[](1046357002912) -> Dalamud.Game.Libc.StdString(3158158785264)
12:23:49.573 | DBG HandlePrintMessageDetour String modified: System.Byte[](3157113651928) -> Dalamud.Game.Libc.StdString(3158158785264)
12:23:51.100 | DBG HandlePrintMessageDetour Sender modified: System.Byte[](1046357002912) -> Dalamud.Game.Libc.StdString(3158158785264)
12:23:52.043 | DBG HandlePrintMessageDetour Sender modified: System.Byte[](1046357002912) -> Dalamud.Game.Libc.StdString(3158158785808)
12:24:02.957 | DBG HandlePrintMessageDetour Sender modified: System.Byte[](1046357002912) -> Dalamud.Game.Libc.StdString(3157076250976)
12:24:08.854 | DBG HandlePrintMessageDetour Sender modified: System.Byte[](1046357002912) -> Dalamud.Game.Libc.StdString(3157076252608)
12:24:13.449 | DBG HandlePrintMessageDetour Sender modified: System.Byte[](1046357002912) -> Dalamud.Game.Libc.StdString(3157285100384)
12:24:22.770 | DBG HandlePrintMessageDetour Sender modified: System.Byte[](1046357002912) -> Dalamud.Game.Libc.StdString(3157285094128)
12:24:27.451 | DBG HandlePrintMessageDetour Sender modified: System.Byte[](1046357002912) -> Dalamud.Game.Libc.StdString(3157285100384)
12:24:30.843 | DBG HandlePrintMessageDetour Sender modified: System.Byte[](1046357002912) -> Dalamud.Game.Libc.StdString(3157285100384)
12:24:30.843 | DBG HandlePrintMessageDetour Sender modified: System.Byte[](1046357002912) -> Dalamud.Game.Libc.StdString(3157285100384)
12:24:40.718 | DBG HandlePrintMessageDetour Sender modified: System.Byte[](1046357002912) -> Dalamud.Game.Libc.StdString(3157285094128)
12:24:43.899 | DBG HandlePrintMessageDetour Sender modified: System.Byte[](1046357002912) -> Dalamud.Game.Libc.StdString(3157285100384)
12:24:47.470 | DBG HandlePrintMessageDetour Sender modified: System.Byte[](1046357002912) -> Dalamud.Game.Libc.StdString(3157285094128)
12:25:03.563 | DBG HandlePrintMessageDetour Sender modified: System.Byte[](1046357002912) -> Dalamud.Game.Libc.StdString(3157285100384)
12:25:09.187 | DBG HandlePrintMessageDetour Sender modified: System.Byte[](1046357002912) -> Dalamud.Game.Libc.StdString(3157285094128)
12:25:13.188 | DBG HandlePrintMessageDetour Sender modified: System.Byte[](1046357002912) -> Dalamud.Game.Libc.StdString(3158158785264)
12:25:14.706 | DBG HandlePrintMessageDetour Sender modified: System.Byte[](1046357002912) -> Dalamud.Game.Libc.StdString(3158158785264)
12:25:18.513 | DBG HandlePrintMessageDetour Sender modified: System.Byte[](1046357002912) -> Dalamud.Game.Libc.StdString(3158158785264)
12:25:22.673 | DBG HandlePrintMessageDetour Sender modified: System.Byte[](1046357002912) -> Dalamud.Game.Libc.StdString(3158158785264)
12:25:23.688 | DBG HandlePrintMessageDetour Sender modified: System.Byte[](1046357002912) -> Dalamud.Game.Libc.StdString(3158158785264)
12:25:25.148 | DBG HandlePrintMessageDetour Sender modified: System.Byte[](1046357002912) -> Dalamud.Game.Libc.StdString(3158158785264)
12:25:29.236 | DBG HandlePrintMessageDetour Sender modified: System.Byte[](1046357002912) -> Dalamud.Game.Libc.StdString(3158718108528)
12:25:31.386 | DBG HandlePrintMessageDetour Sender modified: System.Byte[](1046357002912) -> Dalamud.Game.Libc.StdString(3158718112064)
12:25:35.313 | DBG HandlePrintMessageDetour Sender modified: System.Byte[](1046357002912) -> Dalamud.Game.Libc.StdString(3157392034240)
12:25:38.198 | DBG HandlePrintMessageDetour Sender modified: System.Byte[](1046357002912) -> Dalamud.Game.Libc.StdString(3157392028528)
12:25:42.601 | DBG HandlePrintMessageDetour Sender modified: System.Byte[](1046357002912) -> Dalamud.Game.Libc.StdString(3157392023088)
12:25:45.380 | DBG HandlePrintMessageDetour Sender modified: System.Byte[](1046357002912) -> Dalamud.Game.Libc.StdString(3157392034240)
12:25:50.938 | DBG HandlePrintMessageDetour Sender modified: System.Byte[](1046357002912) -> Dalamud.Game.Libc.StdString(3157392031248)
12:25:56.312 | DBG HandlePrintMessageDetour Sender modified: System.Byte[](1046357002912) -> Dalamud.Game.Libc.StdString(3157392034240)
12:26:04.141 | DBG HandlePrintMessageDetour Sender modified: System.Byte[](1046357002912) -> Dalamud.Game.Libc.StdString(3157285100384)
12:26:08.103 | DBG HandlePrintMessageDetour Sender modified: System.Byte[](1046357002912) -> Dalamud.Game.Libc.StdString(3157285100384)
12:26:08.242 | DBG HandlePrintMessageDetour Sender modified: System.Byte[](1046357002912) -> Dalamud.Game.Libc.StdString(3157285094128)
12:26:13.058 | DBG HandlePrintMessageDetour Sender modified: System.Byte[](1046357002912) -> Dalamud.Game.Libc.StdString(3157285094128)
12:26:16.333 | DBG HandlePrintMessageDetour Sender modified: System.Byte[](1046357002912) -> Dalamud.Game.Libc.StdString(3157285094128)
12:26:26.528 | DBG HandlePrintMessageDetour Sender modified: System.Byte[](1046357002912) -> Dalamud.Game.Libc.StdString(3158158785264)
12:26:36.294 | DBG HandlePrintMessageDetour Sender modified: System.Byte[](1046357002912) -> Dalamud.Game.Libc.StdString(3158158785536)
12:26:36.295 | DBG HandlePrintMessageDetour Sender modified: System.Byte[](1046357002912) -> Dalamud.Game.Libc.StdString(3158158785264)
12:26:38.080 | DBG HandlePrintMessageDetour Sender modified: System.Byte[](1046357002912) -> Dalamud.Game.Libc.StdString(3157760378496)
12:26:44.392 | DBG HandlePrintMessageDetour Sender modified: System.Byte[](1046357002912) -> Dalamud.Game.Libc.StdString(3157760368704)
12:26:49.665 | DBG HandlePrintMessageDetour Sender modified: System.Byte[](1046357002912) -> Dalamud.Game.Libc.StdString(3157760379040)
12:26:52.044 | DBG HandlePrintMessageDetour Sender modified: System.Byte[](1046357002912) -> Dalamud.Game.Libc.StdString(3157760373872)
12:26:55.527 | DBG [PLUGIN] Skipping reload of Chatter Plugin, state (Unloaded) is not Loaded, LoadError or UnloadError.
12:26:55.527 | DBG [PLUGIN] Skipping reload of Chatter Plugin, file has changed again.
12:26:55.527 | DBG [PLUGIN] Skipping reload of Chatter Plugin, file has changed again.
12:27:10.638 | DBG HandlePrintMessageDetour Sender modified: System.Byte[](1046357002912) -> Dalamud.Game.Libc.StdString(3157391966240)
12:27:14.623 | INF [PLUGINM] Now reloading all PluginMasters...
12:27:14.624 | INF [PLUGINR] Fetching repo: https://kamori.goats.dev/Plugin/PluginMaster
12:27:15.644 | INF [PLUGINR] Successfully fetched repo: https://kamori.goats.dev/Plugin/PluginMaster
12:27:15.645 | INF [PLUGINR] Fetching repo: https://raw.githubusercontent.com/LeonBlade/DalamudPlugins/main/repo.json
12:27:15.775 | INF [PLUGINR] Successfully fetched repo: https://raw.githubusercontent.com/LeonBlade/DalamudPlugins/main/repo.json
12:27:15.775 | INF [PLUGINM] PluginMasters reloaded, now refiltering...
12:27:19.762 | DBG HandlePrintMessageDetour Sender modified: System.Byte[](1046357002912) -> Dalamud.Game.Libc.StdString(3158158766224)
12:27:20.532 | INF [LOCALPLUGIN] Loading ChatterPlugin.dll
12:27:23.870 | DBG HandlePrintMessageDetour Sender modified: System.Byte[](1046357002912) -> Dalamud.Game.Libc.StdString(3158158757520)
12:27:24.726 | INF [ChatterPlugin] @@@@ player: Fiora Greyback   world: Zalera
12:27:24.731 | INF [LOCALPLUGIN] Finished loading ChatterPlugin.dll
12:27:27.919 | DBG HandlePrintMessageDetour Sender modified: System.Byte[](1046357002912) -> Dalamud.Game.Libc.StdString(3158158768400)
12:27:36.212 | DBG HandlePrintMessageDetour Sender modified: System.Byte[](1046357002912) -> Dalamud.Game.Libc.StdString(3157076259136)
12:27:45.389 | DBG HandlePrintMessageDetour Sender modified: System.Byte[](1046357002912) -> Dalamud.Game.Libc.StdString(3158158757792)
12:27:51.888 | DBG HandlePrintMessageDetour Sender modified: System.Byte[](1046357002912) -> Dalamud.Game.Libc.StdString(3158158755616)
12:27:56.443 | DBG HandlePrintMessageDetour Sender modified: System.Byte[](1046357002912) -> Dalamud.Game.Libc.StdString(3157542167536)
12:27:59.416 | DBG HandlePrintMessageDetour Sender modified: System.Byte[](1046357002912) -> Dalamud.Game.Libc.StdString(3157542156656)
12:28:00.525 | DBG HandlePrintMessageDetour Sender modified: System.Byte[](1046357002912) -> Dalamud.Game.Libc.StdString(3157542169168)
12:28:19.255 | DBG HandlePrintMessageDetour Sender modified: System.Byte[](1046357002912) -> Dalamud.Game.Libc.StdString(3157392010032)
12:28:29.534 | DBG HandlePrintMessageDetour Sender modified: System.Byte[](1046357002912) -> Dalamud.Game.Libc.StdString(3158158767312)
12:28:40.754 | DBG HandlePrintMessageDetour Sender modified: System.Byte[](1046357002912) -> Dalamud.Game.Libc.StdString(3158158758336)
12:28:48.493 | DBG HandlePrintMessageDetour Sender modified: System.Byte[](1046357002912) -> Dalamud.Game.Libc.StdString(3158158760512)
12:28:50.300 | ERR Could not invoke registered OnMessageDelegate for "HandleChatMessage"
System.ArgumentOutOfRangeException: StartIndex cannot be less than zero. (Parameter 'startIndex')
   at System.String.Remove(Int32 startIndex)
   at ChatterPlugin.ChatLogManager.ChatLog.CleanUpSender(String sender) in C:\Users\james\source\repos\jlkeesey\ChatterPlugin\ChatterPlugin\ChatLogManager.cs:line 183
   at ChatterPlugin.ChatLogManager.ChatLog.LogInfo(XivChatType xivType, UInt32 senderId, String sender, String message) in C:\Users\james\source\repos\jlkeesey\ChatterPlugin\ChatterPlugin\ChatLogManager.cs:line 144
   at ChatterPlugin.ChatLogManager.LogInfo(XivChatType xivType, UInt32 senderId, String sender, String message) in C:\Users\james\source\repos\jlkeesey\ChatterPlugin\ChatterPlugin\ChatLogManager.cs:line 88
   at ChatterPlugin.ChatManager.HandleChatMessage(XivChatType xivType, UInt32 senderId, SeString& seSender, SeString& seMessage, Boolean& isHandled) in C:\Users\james\source\repos\jlkeesey\ChatterPlugin\ChatterPlugin\ChatManager.cs:line 27
   at Dalamud.Game.Gui.ChatGui.HandlePrintMessageDetour(IntPtr manager, XivChatType chattype, IntPtr pSenderName, IntPtr pMessage, UInt32 senderid, IntPtr parameter) in C:\goatsoft\companysecrets\dalamud\Game\Gui\ChatGui.cs:line 413
12:28:51.731 | DBG HandlePrintMessageDetour Sender modified: System.Byte[](1046357002912) -> Dalamud.Game.Libc.StdString(3157076252608)
12:28:55.481 | ERR Could not invoke registered OnMessageDelegate for "HandleChatMessage"
System.ArgumentOutOfRangeException: StartIndex cannot be less than zero. (Parameter 'startIndex')
   at System.String.Remove(Int32 startIndex)
   at ChatterPlugin.ChatLogManager.ChatLog.CleanUpSender(String sender) in C:\Users\james\source\repos\jlkeesey\ChatterPlugin\ChatterPlugin\ChatLogManager.cs:line 183
   at ChatterPlugin.ChatLogManager.ChatLog.LogInfo(XivChatType xivType, UInt32 senderId, String sender, String message) in C:\Users\james\source\repos\jlkeesey\ChatterPlugin\ChatterPlugin\ChatLogManager.cs:line 144
   at ChatterPlugin.ChatLogManager.LogInfo(XivChatType xivType, UInt32 senderId, String sender, String message) in C:\Users\james\source\repos\jlkeesey\ChatterPlugin\ChatterPlugin\ChatLogManager.cs:line 88
   at ChatterPlugin.ChatManager.HandleChatMessage(XivChatType xivType, UInt32 senderId, SeString& seSender, SeString& seMessage, Boolean& isHandled) in C:\Users\james\source\repos\jlkeesey\ChatterPlugin\ChatterPlugin\ChatManager.cs:line 27
   at Dalamud.Game.Gui.ChatGui.HandlePrintMessageDetour(IntPtr manager, XivChatType chattype, IntPtr pSenderName, IntPtr pMessage, UInt32 senderid, IntPtr parameter) in C:\goatsoft\companysecrets\dalamud\Game\Gui\ChatGui.cs:line 413
12:28:56.022 | DBG HandlePrintMessageDetour Sender modified: System.Byte[](1046357002912) -> Dalamud.Game.Libc.StdString(3157392010032)
12:28:56.023 | DBG HandlePrintMessageDetour Sender modified: System.Byte[](1046357002912) -> Dalamud.Game.Libc.StdString(3157392010032)
12:28:56.515 | DBG HandlePrintMessageDetour Sender modified: System.Byte[](1046357002912) -> Dalamud.Game.Libc.StdString(3157392010032)
12:28:57.504 | DBG HandlePrintMessageDetour Sender modified: System.Byte[](1046357002912) -> Dalamud.Game.Libc.StdString(3157392010032)
12:29:00.223 | DBG HandlePrintMessageDetour Sender modified: System.Byte[](1046357002912) -> Dalamud.Game.Libc.StdString(3157392010032)
{ } [ Send ]
 *
 */

public sealed partial class Chatter : IDalamudPlugin
{
    public static string Version = string.Empty;

    private readonly JlkWindowManager windowManager;

    public Chatter(DalamudPluginInterface pluginInterface)
    {
        try
        {
            Dalamud.Initialize(pluginInterface);
            Version = Assembly.GetExecutingAssembly().GetName().Version?.ToString() ?? "";

            Configuration = Configuration.Load();

            WindowSystem = new WindowSystem(Name);

            ChatLogManager = new ChatLogManager();
            ChatManager = new ChatManager(ChatLogManager);

            var name = Dalamud.ClientState.LocalPlayer?.Name ?? "Who?";
            var world = Dalamud.ClientState.LocalPlayer?.HomeWorld.GameData?.Name ?? "Where?";
            PluginLog.Log($"@@@@ player: {name}   world: {world}");

            // you might normally want to embed resources and load them from the manifest stream
            // var imagePath = Path.Combine(PluginInterface.AssemblyLocation.Directory?.FullName!, "goat.png");
            // var goatImage = this.PluginInterface.UiBuilder.LoadImage(imagePath);

            windowManager = new JlkWindowManager(this);
            RegisterCommands();
        }
        catch
        {
            Dispose();
            throw;
        }
    }

#pragma warning disable CS8618
    /// <summary>
    ///     The configuration of this plugin.
    /// </summary>
    public static Configuration Configuration { get; private set; }
#pragma warning restore CS8618

    public ChatManager ChatManager { get; private set; }
    public ChatLogManager ChatLogManager { get; private set; }
    public WindowSystem WindowSystem { get; private set; }
    public string Name => "ChatterPlugin";

    public void Dispose()
    {
        Configuration?.Save(); // Should be auto-saved but let's be sure

        UnregisterCommands();
        ChatLogManager?.Dispose();
        ChatManager?.Dispose();
        windowManager?.Dispose();
    }
}
