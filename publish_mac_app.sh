#!/bin/bash

APP_NAME="AttendanceRecord"
PROJECT_NAME="AttendanceRecord"
BIN_PATH="./$PROJECT_NAME/bin/Release/net9.0/osx-arm64/publish"
APP_BUNDLE="$APP_NAME.app"

mkdir -p "$APP_BUNDLE/Contents/MacOS"
mkdir -p "$APP_BUNDLE/Contents/Resources"

cp -r $BIN_PATH/* "$APP_BUNDLE/Contents/MacOS"
rm -r "$APP_BUNDLE/Contents/MacOS/$APP_NAME.dSYM"

cat > "$APP_BUNDLE/Contents/Info.plist" <<EOF
<?xml version="1.0" encoding="UTF-8"?>
<plist version="1.0">
<dict>
  <key>CFBundleName</key>
  <string>$APP_NAME</string>
  <key>CFBundleIdentifier</key>
  <string>com.example.$APP_NAME</string>
  <key>CFBundleVersion</key>
  <string>1.0</string>
  <key>CFBundleExecutable</key>
  <string>$APP_NAME</string>
  <key>CFBundlePackageType</key>
  <string>APPL</string>
  <key>LSMinimumSystemVersion</key>
  <string>10.13</string>
  <key>CFBundleInfoDictionaryVersion</key>
  <string>6.0</string>
</dict>
</plist>
EOF

chmod +x "$APP_BUNDLE/Contents/MacOS/$APP_NAME"
xattr -dr com.apple.quarantine "$APP_BUNDLE"
