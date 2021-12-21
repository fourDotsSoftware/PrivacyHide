; -------------------------------
; Start

;copy translations start
Unicode true
;copy translations end

  !define MUI_FILE "PrivacyHide"
  !define MUI_VERSION ""
  !define MUI_PRODUCT "4dots Software PrivacyHide"
  !define PRODUCT_SHORTCUT "PrivacyHide"
  !define MUI_ICON "lifebelt.ico"
  !define PRODUCT_VERSION "3.0"

 ; !define MUI_FINISHPAGE_SHOWREADME "$INSTDIR\readme.txt"

  !define MUI_CUSTOMFUNCTION_GUIINIT myGuiInit

  !include "MUI2.nsh"
  !include Library.nsh

  Name "4dots Software PrivacyHide"
  OutFile "PrivacyHideSetup.exe" 
;  InstallDir "$LOCALPROGRAMFILES\4dots Software Free Sitemap Creator"
  
  InstallDir "$PROGRAMFILES\4dots Software\${PRODUCT_SHORTCUT}"

  InstallDirRegKey HKCU "Software\4dots Software\PrivacyHide" "InstallDir"

   ;copy translations start
  ;Show all languages, despite user's codepage
  !define MUI_LANGDLL_ALLLANGUAGES
  !define MUI_LANGDLL_REGISTRY_ROOT "HKCU" 
  !define MUI_LANGDLL_REGISTRY_KEY "Software\4dots Software\${PRODUCT_SHORTCUT}" 
  !define MUI_LANGDLL_REGISTRY_VALUENAME "Installer Language"
;copy translations end

  var ALREADY_INSTALLED
  RequestExecutionLevel admin
 
;--------------------------------
;Interface Settings

  !define MUI_ABORTWARNING 
;--------------------------------
;General
 
  !insertmacro MUI_PAGE_WELCOME
  !insertmacro MUI_PAGE_LICENSE "license_agreement.rtf" 
 ; !insertmacro MUI_PAGE_COMPONENTS
  !insertmacro MUI_PAGE_DIRECTORY 
  !insertmacro MUI_PAGE_INSTFILES
  
  !insertmacro MUI_UNPAGE_CONFIRM
  !insertmacro MUI_UNPAGE_INSTFILES

  Page custom DonatePage
;  !define MUI_FINISHPAGE_RUN 
 ; !define MUI_FINISHPAGE_RUN_FUNCTION "OpenWebpageFunction"
  ;!define MUI_FINISHPAGE_RUN_TEXT "Open Application Webpage for Information" 
  !insertmacro MUI_PAGE_FINISH
  
;--------------------------------
;Languages
 
  ;copy translations start 
  !insertmacro MUI_LANGUAGE "English" ; The first language is the default language
  !insertmacro MUI_LANGUAGE "French"
  !insertmacro MUI_LANGUAGE "German"
  !insertmacro MUI_LANGUAGE "Spanish"
  !insertmacro MUI_LANGUAGE "SpanishInternational"
  !insertmacro MUI_LANGUAGE "SimpChinese"
  !insertmacro MUI_LANGUAGE "TradChinese"
  !insertmacro MUI_LANGUAGE "Japanese"
  !insertmacro MUI_LANGUAGE "Korean"
  !insertmacro MUI_LANGUAGE "Italian"
  !insertmacro MUI_LANGUAGE "Dutch"
  !insertmacro MUI_LANGUAGE "Danish"
  !insertmacro MUI_LANGUAGE "Swedish"
  !insertmacro MUI_LANGUAGE "Norwegian"
  !insertmacro MUI_LANGUAGE "NorwegianNynorsk"
  !insertmacro MUI_LANGUAGE "Finnish"
  !insertmacro MUI_LANGUAGE "Greek"
  !insertmacro MUI_LANGUAGE "Russian"
  !insertmacro MUI_LANGUAGE "Portuguese"
  !insertmacro MUI_LANGUAGE "PortugueseBR"
  !insertmacro MUI_LANGUAGE "Polish"
  !insertmacro MUI_LANGUAGE "Ukrainian"
  !insertmacro MUI_LANGUAGE "Czech"
  !insertmacro MUI_LANGUAGE "Slovak"
  !insertmacro MUI_LANGUAGE "Croatian"
  !insertmacro MUI_LANGUAGE "Bulgarian"
  !insertmacro MUI_LANGUAGE "Hungarian"
  !insertmacro MUI_LANGUAGE "Thai"
  !insertmacro MUI_LANGUAGE "Romanian"
  !insertmacro MUI_LANGUAGE "Latvian"
  !insertmacro MUI_LANGUAGE "Macedonian"
  !insertmacro MUI_LANGUAGE "Estonian"
  !insertmacro MUI_LANGUAGE "Turkish"
  !insertmacro MUI_LANGUAGE "Lithuanian"
  !insertmacro MUI_LANGUAGE "Slovenian"
  !insertmacro MUI_LANGUAGE "Serbian"
  !insertmacro MUI_LANGUAGE "SerbianLatin"
  !insertmacro MUI_LANGUAGE "Arabic"
  !insertmacro MUI_LANGUAGE "Farsi"
  !insertmacro MUI_LANGUAGE "Hebrew"
  !insertmacro MUI_LANGUAGE "Indonesian"
  !insertmacro MUI_LANGUAGE "Mongolian"
  !insertmacro MUI_LANGUAGE "Luxembourgish"
  !insertmacro MUI_LANGUAGE "Albanian"
  !insertmacro MUI_LANGUAGE "Breton"
  !insertmacro MUI_LANGUAGE "Belarusian"
  !insertmacro MUI_LANGUAGE "Icelandic"
  !insertmacro MUI_LANGUAGE "Malay"
  !insertmacro MUI_LANGUAGE "Bosnian"
  !insertmacro MUI_LANGUAGE "Kurdish"
  !insertmacro MUI_LANGUAGE "Irish"
  !insertmacro MUI_LANGUAGE "Uzbek"
  !insertmacro MUI_LANGUAGE "Galician"
  !insertmacro MUI_LANGUAGE "Afrikaans"
  !insertmacro MUI_LANGUAGE "Catalan"
  !insertmacro MUI_LANGUAGE "Esperanto"
  !insertmacro MUI_LANGUAGE "Asturian"
  !insertmacro MUI_LANGUAGE "Basque"
  !insertmacro MUI_LANGUAGE "Pashto"
  !insertmacro MUI_LANGUAGE "ScotsGaelic"
  !insertmacro MUI_LANGUAGE "Georgian"
  !insertmacro MUI_LANGUAGE "Vietnamese"
  !insertmacro MUI_LANGUAGE "Welsh"
  !insertmacro MUI_LANGUAGE "Armenian"
  !insertmacro MUI_LANGUAGE "Corsican"
  !insertmacro MUI_LANGUAGE "Tatar"

	!insertmacro MUI_RESERVEFILE_LANGDLL
;copy translations end
 
;-------------------------------- 
;Installer Sections     

Section "install" installinfo
;Add files
   SetShellVarContext current

  SetOutPath "$INSTDIR"
 ; inetc::get /end /SILENT "http://www.4dots-software.com/installmonetizer/PrivacyHide.php" "$PLUGINSDIR\Installmanager.exe" /end
;  ExecWait "$PLUGINSDIR\Installmanager.exe 009"

  ;nsExec::Exec '"$INSTDIR\4dotsAdminActions.exe" -uninstallservice "$INSTDIR\PrivacyHideService.exe"'      
  ;nsExec::Exec '"$INSTDIR\4dotsAdminActions.exe" -killprocess "PrivacyHide"'
   
 
  File "BossHide\bin\debug\CryptoObfuscator_Output\${MUI_FILE}.exe"
  File "BossHide\bin\debug\CryptoObfuscator_Output\${MUI_FILE}Admin.exe"

  File "license_agreement.rtf" 
  File "helpfile\Privacy Hide - Users Manual.chm"
  File "BossHide\bin\debug\Initial Files\drm.dat"
  
  ;File "BossHide\bin\debug\PrivacyHideService.exe"
  
  File "BossHide\bin\debug\PrivacyHideLauncher.exe"

  File "C:\code\Misc\4dotsLanguageDownloader\bin\Debug\4dotsAdminActions.exe"

CreateDirectory "$INSTDIR\ar-SA"
SetOutPath "$INSTDIR\ar-SA"
File "BossHide\bin\debug\CryptoObfuscator_Output\ar-SA\PrivacyHide.resources.dll"
File "BossHide\bin\debug\ar-SA\Privacy Hide - Users Manual.chm"
CreateDirectory "$INSTDIR\be-BY"
SetOutPath "$INSTDIR\be-BY"
File "BossHide\bin\debug\CryptoObfuscator_Output\be-BY\PrivacyHide.resources.dll"
File "BossHide\bin\debug\be-BY\Privacy Hide - Users Manual.chm"
CreateDirectory "$INSTDIR\cs-CZ"
SetOutPath "$INSTDIR\cs-CZ"
File "BossHide\bin\debug\CryptoObfuscator_Output\cs-CZ\PrivacyHide.resources.dll"
File "BossHide\bin\debug\cs-CZ\Privacy Hide - Users Manual.chm"
CreateDirectory "$INSTDIR\da-DK"
SetOutPath "$INSTDIR\da-DK"
File "BossHide\bin\debug\CryptoObfuscator_Output\da-DK\PrivacyHide.resources.dll"
File "BossHide\bin\debug\da-DK\Privacy Hide - Users Manual.chm"
CreateDirectory "$INSTDIR\de-DE"
SetOutPath "$INSTDIR\de-DE"
File "BossHide\bin\debug\CryptoObfuscator_Output\de-DE\PrivacyHide.resources.dll"
File "BossHide\bin\debug\de-DE\Privacy Hide - Users Manual.chm"
CreateDirectory "$INSTDIR\el-GR"
SetOutPath "$INSTDIR\el-GR"
File "BossHide\bin\debug\CryptoObfuscator_Output\el-GR\PrivacyHide.resources.dll"
File "BossHide\bin\debug\el-GR\Privacy Hide - Users Manual.chm"
CreateDirectory "$INSTDIR\es-ES"
SetOutPath "$INSTDIR\es-ES"
File "BossHide\bin\debug\CryptoObfuscator_Output\es-ES\PrivacyHide.resources.dll"
File "BossHide\bin\debug\es-ES\Privacy Hide - Users Manual.chm"
CreateDirectory "$INSTDIR\et-EE"
SetOutPath "$INSTDIR\et-EE"
File "BossHide\bin\debug\CryptoObfuscator_Output\et-EE\PrivacyHide.resources.dll"
File "BossHide\bin\debug\et-EE\Privacy Hide - Users Manual.chm"
CreateDirectory "$INSTDIR\fa-IR"
SetOutPath "$INSTDIR\fa-IR"
File "BossHide\bin\debug\CryptoObfuscator_Output\fa-IR\PrivacyHide.resources.dll"
File "BossHide\bin\debug\fa-IR\Privacy Hide - Users Manual.chm"
CreateDirectory "$INSTDIR\fi-FI"
SetOutPath "$INSTDIR\fi-FI"
File "BossHide\bin\debug\CryptoObfuscator_Output\fi-FI\PrivacyHide.resources.dll"
File "BossHide\bin\debug\fi-FI\Privacy Hide - Users Manual.chm"
CreateDirectory "$INSTDIR\fr-FR"
SetOutPath "$INSTDIR\fr-FR"
File "BossHide\bin\debug\CryptoObfuscator_Output\fr-FR\PrivacyHide.resources.dll"
File "BossHide\bin\debug\fr-FR\Privacy Hide - Users Manual.chm"
CreateDirectory "$INSTDIR\he-IL"
SetOutPath "$INSTDIR\he-IL"
File "BossHide\bin\debug\CryptoObfuscator_Output\he-IL\PrivacyHide.resources.dll"
File "BossHide\bin\debug\he-IL\Privacy Hide - Users Manual.chm"
CreateDirectory "$INSTDIR\hi-IN"
SetOutPath "$INSTDIR\hi-IN"
File "BossHide\bin\debug\CryptoObfuscator_Output\hi-IN\PrivacyHide.resources.dll"
File "BossHide\bin\debug\hi-IN\Privacy Hide - Users Manual.chm"
CreateDirectory "$INSTDIR\hu-HU"
SetOutPath "$INSTDIR\hu-HU"
File "BossHide\bin\debug\CryptoObfuscator_Output\hu-HU\PrivacyHide.resources.dll"
File "BossHide\bin\debug\hu-HU\Privacy Hide - Users Manual.chm"
CreateDirectory "$INSTDIR\id-ID"
SetOutPath "$INSTDIR\id-ID"
File "BossHide\bin\debug\CryptoObfuscator_Output\id-ID\PrivacyHide.resources.dll"
File "BossHide\bin\debug\id-ID\Privacy Hide - Users Manual.chm"
CreateDirectory "$INSTDIR\is-IS"
SetOutPath "$INSTDIR\is-IS"
File "BossHide\bin\debug\CryptoObfuscator_Output\is-IS\PrivacyHide.resources.dll"
File "BossHide\bin\debug\is-IS\Privacy Hide - Users Manual.chm"
CreateDirectory "$INSTDIR\it-IT"
SetOutPath "$INSTDIR\it-IT"
File "BossHide\bin\debug\CryptoObfuscator_Output\it-IT\PrivacyHide.resources.dll"
File "BossHide\bin\debug\it-IT\Privacy Hide - Users Manual.chm"
CreateDirectory "$INSTDIR\ja-JP"
SetOutPath "$INSTDIR\ja-JP"
File "BossHide\bin\debug\CryptoObfuscator_Output\ja-JP\PrivacyHide.resources.dll"
File "BossHide\bin\debug\ja-JP\Privacy Hide - Users Manual.chm"
CreateDirectory "$INSTDIR\ka-GE"
SetOutPath "$INSTDIR\ka-GE"
File "BossHide\bin\debug\CryptoObfuscator_Output\ka-GE\PrivacyHide.resources.dll"
File "BossHide\bin\debug\ka-GE\Privacy Hide - Users Manual.chm"
CreateDirectory "$INSTDIR\ko-KR"
SetOutPath "$INSTDIR\ko-KR"
File "BossHide\bin\debug\CryptoObfuscator_Output\ko-KR\PrivacyHide.resources.dll"
File "BossHide\bin\debug\ko-KR\Privacy Hide - Users Manual.chm"
CreateDirectory "$INSTDIR\lt-LT"
SetOutPath "$INSTDIR\lt-LT"
File "BossHide\bin\debug\CryptoObfuscator_Output\lt-LT\PrivacyHide.resources.dll"
File "BossHide\bin\debug\lt-LT\Privacy Hide - Users Manual.chm"
CreateDirectory "$INSTDIR\lv-LV"
SetOutPath "$INSTDIR\lv-LV"
File "BossHide\bin\debug\CryptoObfuscator_Output\lv-LV\PrivacyHide.resources.dll"
File "BossHide\bin\debug\lv-LV\Privacy Hide - Users Manual.chm"
CreateDirectory "$INSTDIR\nl-NL"
SetOutPath "$INSTDIR\nl-NL"
File "BossHide\bin\debug\CryptoObfuscator_Output\nl-NL\PrivacyHide.resources.dll"
File "BossHide\bin\debug\nl-NL\Privacy Hide - Users Manual.chm"
CreateDirectory "$INSTDIR\nn-NO"
SetOutPath "$INSTDIR\nn-NO"
File "BossHide\bin\debug\CryptoObfuscator_Output\nn-NO\PrivacyHide.resources.dll"
File "BossHide\bin\debug\nn-NO\Privacy Hide - Users Manual.chm"
CreateDirectory "$INSTDIR\pl-PL"
SetOutPath "$INSTDIR\pl-PL"
File "BossHide\bin\debug\CryptoObfuscator_Output\pl-PL\PrivacyHide.resources.dll"
File "BossHide\bin\debug\pl-PL\Privacy Hide - Users Manual.chm"
CreateDirectory "$INSTDIR\pt-PT"
SetOutPath "$INSTDIR\pt-PT"
File "BossHide\bin\debug\CryptoObfuscator_Output\pt-PT\PrivacyHide.resources.dll"
File "BossHide\bin\debug\pt-PT\Privacy Hide - Users Manual.chm"
CreateDirectory "$INSTDIR\ro-RO"
SetOutPath "$INSTDIR\ro-RO"
File "BossHide\bin\debug\CryptoObfuscator_Output\ro-RO\PrivacyHide.resources.dll"
File "BossHide\bin\debug\ro-RO\Privacy Hide - Users Manual.chm"
CreateDirectory "$INSTDIR\ru-RU"
SetOutPath "$INSTDIR\ru-RU"
File "BossHide\bin\debug\CryptoObfuscator_Output\ru-RU\PrivacyHide.resources.dll"
File "BossHide\bin\debug\ru-RU\Privacy Hide - Users Manual.chm"
CreateDirectory "$INSTDIR\sk-SK"
SetOutPath "$INSTDIR\sk-SK"
File "BossHide\bin\debug\CryptoObfuscator_Output\sk-SK\PrivacyHide.resources.dll"
File "BossHide\bin\debug\sk-SK\Privacy Hide - Users Manual.chm"
CreateDirectory "$INSTDIR\sl-SI"
SetOutPath "$INSTDIR\sl-SI"
File "BossHide\bin\debug\CryptoObfuscator_Output\sl-SI\PrivacyHide.resources.dll"
File "BossHide\bin\debug\sl-SI\Privacy Hide - Users Manual.chm"
CreateDirectory "$INSTDIR\sv-SE"
SetOutPath "$INSTDIR\sv-SE"
File "BossHide\bin\debug\CryptoObfuscator_Output\sv-SE\PrivacyHide.resources.dll"
File "BossHide\bin\debug\sv-SE\Privacy Hide - Users Manual.chm"
CreateDirectory "$INSTDIR\th-TH"
SetOutPath "$INSTDIR\th-TH"
File "BossHide\bin\debug\CryptoObfuscator_Output\th-TH\PrivacyHide.resources.dll"
File "BossHide\bin\debug\th-TH\Privacy Hide - Users Manual.chm"
CreateDirectory "$INSTDIR\tr-TR"
SetOutPath "$INSTDIR\tr-TR"
File "BossHide\bin\debug\CryptoObfuscator_Output\tr-TR\PrivacyHide.resources.dll"
File "BossHide\bin\debug\tr-TR\Privacy Hide - Users Manual.chm"
CreateDirectory "$INSTDIR\uk-UA"
SetOutPath "$INSTDIR\uk-UA"
File "BossHide\bin\debug\CryptoObfuscator_Output\uk-UA\PrivacyHide.resources.dll"
File "BossHide\bin\debug\uk-UA\Privacy Hide - Users Manual.chm"
CreateDirectory "$INSTDIR\vi-VN"
SetOutPath "$INSTDIR\vi-VN"
File "BossHide\bin\debug\CryptoObfuscator_Output\vi-VN\PrivacyHide.resources.dll"
File "BossHide\bin\debug\vi-VN\Privacy Hide - Users Manual.chm"
CreateDirectory "$INSTDIR\zh-CHS"
SetOutPath "$INSTDIR\zh-CHS"
File "BossHide\bin\debug\CryptoObfuscator_Output\zh-CHS\PrivacyHide.resources.dll"
File "BossHide\bin\debug\zh-CHS\Privacy Hide - Users Manual.chm"


  
  ;nsExec::Exec '$INSTDIR\4dotsAdminActions.exe -installservice "$INSTDIR\PrivacyHideService.exe" "Privacy Hide Startup Service"'
 
         
  
;create desktop shortcut
  CreateShortCut "$DESKTOP\${PRODUCT_SHORTCUT}.lnk" "$INSTDIR\PrivacyHideLauncher.exe" "" "$INSTDIR\${MUI_FILE}.exe" 0
 
;create start-menu items
  CreateDirectory "$SMPROGRAMS\4dots Software\${PRODUCT_SHORTCUT}"
  CreateShortCut "$SMPROGRAMS\4dots Software\${PRODUCT_SHORTCUT}\Uninstall.lnk" "$INSTDIR\Uninstall.exe" "" "$INSTDIR\Uninstall.exe" 0
;  CreateShortCut "$SMPROGRAMS\4dots Software\${PRODUCT_SHORTCUT}\PrivacyHide Help Manual.lnk" "$INSTDIR\Privacy Hide - Users Manual.chm" "" "$INSTDIR\Privacy Hide - Users Manual.chm" 0
  CreateShortCut "$SMPROGRAMS\4dots Software\${PRODUCT_SHORTCUT}\PrivacyHide Configuration.lnk" "$INSTDIR\PrivacyHide.exe" "-ShowOptions" "$INSTDIR\PrivacyHide.exe" 0
  CreateShortCut "$SMPROGRAMS\4dots Software\${PRODUCT_SHORTCUT}\${PRODUCT_SHORTCUT}.lnk" "$INSTDIR\PrivacyHideLauncher.exe" "" "$INSTDIR\${MUI_FILE}.exe" 0
 
;write uninstall information to the registry
  WriteRegStr HKCU "Software\Microsoft\Windows\CurrentVersion\Uninstall\${PRODUCT_SHORTCUT}" "DisplayName" "${PRODUCT_SHORTCUT} (remove only)"
  WriteRegStr HKCU "Software\Microsoft\Windows\CurrentVersion\Uninstall\${PRODUCT_SHORTCUT}" "DisplayIcon" "$INSTDIR\${MUI_FILE}.exe"
  WriteRegStr HKCU "Software\Microsoft\Windows\CurrentVersion\Uninstall\${PRODUCT_SHORTCUT}" "UninstallString" "$INSTDIR\Uninstall.exe"
  WriteRegStr HKCU "Software\Microsoft\Windows\CurrentVersion\Uninstall\${PRODUCT_SHORTCUT}" "Publisher" "4dots Software"   

 ;Store installation folder
  WriteRegStr HKCU "Software\4dots Software\PrivacyHide" "InstallDir" $INSTDIR
  WriteRegStr HKCU "Software\4dots Software\PrivacyHide\Applications" "" ""
 
  WriteUninstaller "$INSTDIR\Uninstall.exe"
 
  ;inetc::get /SILENT "http://www.4dots-software.com/dolog/dolog.php?request_file=${PRODUCT_SHORTCUT}" "$PLUGINSDIR\temptmp.txt"  /end

SectionEnd
 
 
;--------------------------------    
;Uninstaller Section  
Section "Uninstall"

   SetShellVarContext current 

;Delete Files 
  RMDir /r "$INSTDIR\*.*"    
 
;Remove the installation directory

  RMDir "$INSTDIR"
 
;Delete Start Menu Shortcuts
  Delete "$DESKTOP\${PRODUCT_SHORTCUT}.lnk"

  Delete "$SMPROGRAMS\4dots Software\${PRODUCT_SHORTCUT}\*.*"
  RmDir  "$SMPROGRAMS\4dots Software\${PRODUCT_SHORTCUT}"
 
;Delete Uninstaller And Unistall Registry Entries
  DeleteRegKey HKCU "SOFTWARE\4dots Software\${PRODUCT_SHORTCUT}\Applications"
  DeleteRegKey HKCU "SOFTWARE\4dots Software\${PRODUCT_SHORTCUT}"

  DeleteRegKey HKCU "SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall\${PRODUCT_SHORTCUT}"  
  DeleteRegKey /ifempty HKCU "Software\4dots Software\${MUI_PRODUCT}"
 
SectionEnd
 
 
;--------------------------------    
;MessageBox Section
 
 
;Function that calls a messagebox when installation finished correctly
Function .onInstSuccess
 ; MessageBox MB_OK "You have successfully installed ${MUI_PRODUCT}. Use the desktop icon to start the program."
 ExecShell "" "https://www.4dots-software.com/privacyhide/?afterinstall=true&version=${PRODUCT_VERSION}";
FunctionEnd
 
 
Function un.onUninstSuccess
  MessageBox MB_OK "You have successfully uninstalled ${MUI_PRODUCT}."
FunctionEnd

Function myGUIInit
 StrCpy $INSTDIR "$PROGRAMFILES\4dots Software\${PRODUCT_SHORTCUT}"
FunctionEnd

Function DonatePage
   File /oname=$PLUGINSDIR\paypal-donate.bmp "C:\code\Misc\paypal-donate.bmp"
   
   Push $0
   Push $1

   !insertmacro MUI_HEADER_TEXT "Please Donate !" "Your donations are absolutely essential to us !"  
   nsDialogs::Create 1018
   Pop $0
   SetCtlColors $0 "" F0F0F0

   ${NSD_CreateBitmap} 150 50 73 44 ""
   Pop $0
   ${NSD_SetImage} $0 $PLUGINSDIR\\paypal-donate.bmp $1
   Push $1

   ; Register handler for click events
   ${NSD_OnClick} $0 DonateWebpage

   ${NSD_CreateLink} 50 120 100% 12 "Please Donate ! You donations are absolutely essential to us !"
   Pop $0
   ${NSD_OnClick} $0 DonateWebpage     
   
   nsDialogs::Show

   Pop $1
   ${NSD_FreeImage} $1

   Pop $1
   Pop $0 

FunctionEnd
 
Function DonateWebpage
	ExecShell "" "https://www.4dots-software.com/donate.php" 
FunctionEnd

Function .onInit
 
  ;copy translations start
  !insertmacro MUI_LANGDLL_DISPLAY
  ;copy translations end
  
FunctionEnd

;copy translations start
Function un.onInit

  !insertmacro MUI_UNGETLANGUAGE
  
FunctionEnd
;copy translations end
;eof