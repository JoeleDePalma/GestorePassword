[Setup]
AppName=GestorePassword
AppVersion=1.3.0
DefaultDirName={pf}\GestorePassword
DefaultGroupName=GestorePassword
OutputDir=Output
OutputBaseFilename=installer
Compression=lzma
SolidCompression=yes
DisableProgramGroupPage=yes

[Files]
Source: "output\GestorePassword.Desktop.exe"; DestDir: "{app}"; Flags: ignoreversion

[Icons]
Name: "{group}\GestorePassword"; Filename: "{app}\GestorePassword.Desktop.exe"
Name: "{commondesktop}\GestorePassword"; Filename: "{app}\GestorePassword.Desktop.exe"; Tasks: desktopicon

[Tasks]
Name: "desktopicon"; Description: "Create a desktop icon"; GroupDescription: "Additional icons:"; Flags: unchecked

[Run]
Filename: "{app}\GestorePassword.Desktop.exe"; Description: "Launch GestorePassword"; Flags: nowait postinstall skipifsilent