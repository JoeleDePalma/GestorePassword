; ------------------------------------------------------------
;   Installer per Gestore Password - Versione 1.3.0
;   Compatibile con GitHub Actions
; ------------------------------------------------------------

[Setup]
AppName=Gestore Password
AppVersion=1.3.0
DefaultDirName={pf}\Gestore Password
DefaultGroupName=Gestore Password
DisableProgramGroupPage=no
OutputDir=Output
OutputBaseFilename=GestorePassword-Setup
Compression=lzma2
SolidCompression=yes
ArchitecturesInstallIn64BitMode=x64
PrivilegesRequired=admin
WizardStyle=modern
AllowNoIcons=yes

[Languages]
Name: "italian"; MessagesFile: "compiler:Languages\Italian.isl"

[Files]
; Copia i file generati dal publish single-file
Source: "output\*"; DestDir: "{app}"; Flags: recursesubdirs createallsubdirs

[Icons]
Name: "{group}\Gestore Password"; Filename: "{app}\GestorePassword.Desktop.exe"
Name: "{commondesktop}\Gestore Password"; Filename: "{app}\GestorePassword.Desktop.exe"; Tasks: desktopicon

[Tasks]
Name: "desktopicon"; Description: "Crea un collegamento sul Desktop"; GroupDescription: "Opzioni aggiuntive"

[Run]
Filename: "{app}\GestorePassword.Desktop.exe"; Description: "Avvia Gestore Password"; Flags: nowait postinstall skipifsilent
