#define MyAppName "WheelBridge"
#define MyAppVersion "0.2.0"
#define MyAppPublisher "WheelBridge"
#define MyAppExeName "WheelBridge.App.exe"

[Setup]
AppId={{8D64E9B4-2C16-4C39-9E37-6D4C76C0A120}
AppName={#MyAppName}
AppVersion={#MyAppVersion}
AppPublisher={#MyAppPublisher}
DefaultDirName={autopf}\WheelBridge
DefaultGroupName=WheelBridge
UninstallDisplayIcon={app}\{#MyAppExeName}
OutputDir=output
OutputBaseFilename=WheelBridge-Setup-{#MyAppVersion}-x64
ArchitecturesAllowed=x64
ArchitecturesInstallIn64BitMode=x64
PrivilegesRequired=admin
WizardStyle=classic
DisableProgramGroupPage=yes
Compression=lzma2
SolidCompression=yes
WizardResizable=no
CloseApplications=yes
RestartApplications=no
Uninstallable=yes
LicenseFile=LICENSE-SETUP.txt

[Languages]
Name: "english"; MessagesFile: "compiler:Default.isl"

[Tasks]
Name: "desktopicon"; Description: "Create a desktop shortcut"; GroupDescription: "Additional shortcuts:"; Flags: unchecked

[Files]
Source: "publish\*"; DestDir: "{app}"; Flags: ignoreversion recursesubdirs createallsubdirs

[Icons]
Name: "{group}\WheelBridge"; Filename: "{app}\{#MyAppExeName}"
Name: "{autodesktop}\WheelBridge"; Filename: "{app}\{#MyAppExeName}"; Tasks: desktopicon

[Run]
Filename: "{app}\{#MyAppExeName}"; Description: "Launch WheelBridge"; Flags: postinstall nowait skipifsilent
