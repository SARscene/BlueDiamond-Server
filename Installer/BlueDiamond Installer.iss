#define MyAppName "Blue Diamond Incident Management"
#define MyAppBuild "Debug"
#define MyAppExeName "BlueDiamond.Desktop.exe"
#define MyAppPath "..\BlueDiamond.Desktop\bin\" + MyAppBuild
#define MyWebPath "..\BlueDiamond" 

#define MyApplicationFile MyAppPath + "\" + MyAppExeName
#define MyAppVersion GetFileProductVersion(MyApplicationFile)
#define MyAppPublisher "Blue Toque Software"
#define MyAppURL "http://www.blueToque.ca/"

[Setup]
; NOTE: The value of AppId uniquely identifies this application.
; Do not use the same AppId value in installers for other applications.
; (To generate a new GUID, click Tools | Generate GUID inside the IDE.)
AppId={{2093971F-C73A-4DF3-B0C1-67EBB6CC660C}
AppName={#MyAppName}
AppVersion={#MyAppVersion}
;AppVerName={#MyAppName} {#MyAppVersion}
AppPublisher={#MyAppPublisher}
AppPublisherURL={#MyAppURL}
AppSupportURL={#MyAppURL}
AppUpdatesURL={#MyAppURL}
DefaultDirName={pf}\BlueDiamond
DefaultGroupName=Blue Diamond Incident Management
OutputDir=..\Build
SetupIconFile=..\BlueDiamond.Desktop\favicon.ico
Compression=lzma
SolidCompression=yes
OutputBaseFilename="BlueDiamond.{#MyAppVersion}.Setup"
ArchitecturesInstallIn64BitMode=x64

[Languages]
Name: "english"; MessagesFile: "compiler:Default.isl"

[Tasks]
Name: "desktopicon"; Description: "{cm:CreateDesktopIcon}"; GroupDescription: "{cm:AdditionalIcons}"; Flags: unchecked
Name: "quicklaunchicon"; Description: "{cm:CreateQuickLaunchIcon}"; GroupDescription: "{cm:AdditionalIcons}"; Flags: unchecked; OnlyBelowVersion: 0,6.1

[Files]
; Desktop ============================================================================================================================
Source: "{#MyAppPath}\BlueDiamond.Desktop.exe"; DestDir: "{app}"; Flags: ignoreversion
;Source: "{#MyAppPath}\BlueDiamond.Desktop.exe.config"; DestDir: "{app}"; Flags: ignoreversion
Source: "..\BlueDiamond.Desktop\App.Deployed.config"; DestDir: "{app}"; DestName: "BlueDiamond.Desktop.exe.config"; Flags: ignoreversion
Source: "{#MyAppPath}\BlueDiamond.Desktop.pdb"; DestDir: "{app}"; Flags: ignoreversion
Source: "{#MyAppPath}\BlueDiamond.Utility.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "{#MyAppPath}\BlueDiamond.Utility.pdb"; DestDir: "{app}"; Flags: ignoreversion
Source: "{#MyAppPath}\BlueToque.Utility.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "{#MyAppPath}\BlueToque.Utility.pdb"; DestDir: "{app}"; Flags: ignoreversion
Source: "{#MyAppPath}\BlueToque.WebBrowser2.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "{#MyAppPath}\BlueToque.WebBrowser2.pdb"; DestDir: "{app}"; Flags: ignoreversion
Source: "{#MyAppPath}\Gma.QrCodeNet.Encoding.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "{#MyAppPath}\System.Reactive.Core.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "{#MyAppPath}\System.Reactive.Core.xml"; DestDir: "{app}"; Flags: ignoreversion
Source: "{#MyAppPath}\System.Reactive.Interfaces.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "{#MyAppPath}\System.Reactive.Interfaces.xml"; DestDir: "{app}"; Flags: ignoreversion
Source: "{#MyAppPath}\System.Reactive.Linq.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "{#MyAppPath}\System.Reactive.Linq.xml"; DestDir: "{app}"; Flags: ignoreversion
Source: "{#MyAppPath}\System.Reactive.PlatformServices.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "{#MyAppPath}\System.Reactive.PlatformServices.xml"; DestDir: "{app}"; Flags: ignoreversion
Source: "{#MyAppPath}\System.Reactive.Windows.Threading.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "{#MyAppPath}\System.Reactive.Windows.Threading.xml"; DestDir: "{app}"; Flags: ignoreversion

; Web ======================================================================================================================
Source: "{#MyWebPath}\App_Data\*"; DestDir: "{app}\Web\App_Data"; Flags: ignoreversion recursesubdirs createallsubdirs
Source: "{#MyWebPath}\bin\*"; DestDir: "{app}\Web\bin"; Flags: ignoreversion recursesubdirs createallsubdirs
Source: "{#MyWebPath}\Content\*"; DestDir: "{app}\Web\Content"; Flags: ignoreversion recursesubdirs createallsubdirs
Source: "{#MyWebPath}\Downloads\*"; DestDir: "{app}\Web\Downloads"; Flags: ignoreversion recursesubdirs createallsubdirs
Source: "{#MyWebPath}\fonts\*"; DestDir: "{app}\Web\fonts"; Flags: ignoreversion recursesubdirs createallsubdirs
Source: "{#MyWebPath}\Scripts\*"; DestDir: "{app}\Web\Scripts"; Flags: ignoreversion recursesubdirs createallsubdirs
Source: "{#MyWebPath}\Views\*"; DestDir: "{app}\Web\Views"; Flags: ignoreversion recursesubdirs createallsubdirs
Source: "{#MyWebPath}\favicon.ico"; DestDir: "{app}\Web"; Flags: ignoreversion
Source: "{#MyWebPath}\Global.asax"; DestDir: "{app}\Web"; Flags: ignoreversion
Source: "{#MyWebPath}\Packages.config"; DestDir: "{app}\Web"; Flags: ignoreversion
Source: "{#MyWebPath}\Web.config"; DestDir: "{app}\Web"; Flags: ignoreversion

; install IISExpress
;Source: "3rdParty\iisexpress_1_11_x86_en-US.msi"; DestDir: "{tmp}"; AfterInstall: RunOtherInstaller
Source: "3rdParty\iisexpress_x86_en-US.msi"; DestDir: "{tmp}"; 
Source: "3rdParty\iisexpress_amd64_en-US.msi"; DestDir: "{tmp}"; AfterInstall: RunOtherInstaller

[Icons]
Name: "{group}\{#MyAppName}"; Filename: "{app}\{#MyAppExeName}"
Name: "{group}\{cm:ProgramOnTheWeb,{#MyAppName}}"; Filename: "{#MyAppURL}"
Name: "{group}\{cm:UninstallProgram,{#MyAppName}}"; Filename: "{uninstallexe}"
Name: "{commondesktop}\{#MyAppName}"; Filename: "{app}\{#MyAppExeName}"; Tasks: desktopicon
Name: "{userappdata}\Microsoft\Internet Explorer\Quick Launch\{#MyAppName}"; Filename: "{app}\{#MyAppExeName}"; Tasks: quicklaunchicon

[Run]
;FileName: "{tmp}\iisexpress_1_11_x86_en-US.msi"; Flags: postinstall shellexec runascurrentuser
Filename: "{app}\{#MyAppExeName}"; Description: "{cm:LaunchProgram,{#StringChange(MyAppName, '&', '&&')}}"; Flags: nowait postinstall skipifsilent runascurrentuser


[Code]
procedure RunOtherInstaller;
var
  ResultCode: Integer;
  Inst: String;
begin
  // figure out if IIS is already installed
  if (FileExists(ExpandConstant('{pf64}\IIS Express\iisexpress.exe'))) OR (FileExists(ExpandConstant('{pf32}\IIS Express\iisexpress.exe'))) then
  begin
   Log('IIS Express already installed' );
   exit;
  end;

  // figure out which installer to use
  if IsWin64 then
    begin
      Inst:= ExpandConstant('{tmp}\iisexpress_amd64_en-US.msi');
    end
  else
    begin
      Inst:= ExpandConstant('{tmp}\iisexpress_x86_en-US.msi');
    end;

   Log('Executing installer "'+Inst+'"' );

  //if ShellExec('',ExpandConstant('{tmp}\iisexpress_x86_en-US.msi'), '' ,'', SW_HIDE, ewWaitUntilTerminated, ResultCode) then
  if ShellExec('',Inst, '' ,'', SW_HIDE, ewWaitUntilTerminated, ResultCode) then
    begin
      Log('Executed IIS Express installer with result code: "' + SysErrorMessage(ResultCode) +'" ('+ IntToStr(ResultCode) + ')' );
    end
  else
    begin
      MsgBox('Other installer failed to run!' + #13#10 +SysErrorMessage(ResultCode), mbError, MB_OK);
    end
 end;

