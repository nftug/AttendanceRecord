#!/bin/bash
set -e

APP_NAME="AttendanceRecord"
PROJECT_NAME="AttendanceRecord.Presentation"

case "$1" in
  run)
    echo "🟢 Starting frontend and app..."
    cd frontend
    pnpm install
    pnpm run dev &
    FRONTEND_PID=$!
    cd ..

    dotnet watch --project "$PROJECT_NAME" run
    kill $FRONTEND_PID
    ;;
  publish)
    echo "🌱 Building frontend..."
    cd frontend
    pnpm install
    pnpm run build
    cd ..

    echo "🧹 Cleaning wwwroot..."
    rm -rf "$PROJECT_NAME/wwwroot"
    mkdir -p "$PROJECT_NAME/wwwroot"

    echo "📦 Copying frontend files to wwwroot..."
    cp -r frontend/dist/* "$PROJECT_NAME/wwwroot/"

    echo "🛠 Publishing .NET backend..."
    dotnet publish "$PROJECT_NAME" -c Release

    if [ "$(uname)" == 'Darwin' ]; then
      ./publish_mac_app.sh
    fi
    ;;
  *)
    echo "Usage: $0 {run|publish}"
    echo "  run     - run frontend (Vite) and app in dev mode"
    echo "  publish - build frontend and publish backend"
    exit 1
    ;;
esac