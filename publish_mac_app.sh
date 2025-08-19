#!/bin/bash

APP_NAME="AttendanceRecord"
PROJECT_NAME="AttendanceRecord.Presentation"
BIN_PATH="./$PROJECT_NAME/bin/Release/net9.0/osx-arm64/publish"
APP_BUNDLE="$APP_NAME.app"

mkdir -p "$APP_BUNDLE/Contents/MacOS"
mkdir -p "$APP_BUNDLE/Contents/Resources"

cp -r $BIN_PATH/* "$APP_BUNDLE/Contents/MacOS"
rm -r "$APP_BUNDLE/Contents/MacOS/$APP_NAME.dSYM"

cp "./$PROJECT_NAME/Info.plist" "$APP_BUNDLE/Contents/"

chmod +x "$APP_BUNDLE/Contents/MacOS/$APP_NAME"
xattr -dr com.apple.quarantine "$APP_BUNDLE"
