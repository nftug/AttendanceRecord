param (
    [string]$mode = ""
)

$APP_NAME = "AttendanceRecord"
$PROJECT_NAME = "AttendanceRecord.Presentation"

switch ($mode) {
    "run" {
        Write-Host "🟢 Starting frontend and app..."

        Push-Location ./frontend
        pnpm install
        $frontend = Start-Process -FilePath "cmd" -ArgumentList "/c pnpm run dev" -PassThru
        Pop-Location

        dotnet watch --project "$PROJECT_NAME" run

        if ($frontend -ne $null) {
            Stop-Process -Id $frontend.Id -Force
        }
    }

    "publish" {
        Write-Host "🌱 Building frontend..."

        Push-Location ./frontend
        pnpm install
        pnpm run build
        Pop-Location

        Write-Host "🧹 Cleaning wwwroot..."
        Remove-Item -Recurse -Force "$PROJECT_NAME/wwwroot"
        New-Item -ItemType Directory -Force -Path "$PROJECT_NAME/wwwroot" | Out-Null

        Write-Host "📦 Copying frontend files to wwwroot..."
        Copy-Item -Recurse -Force "frontend/dist/*" "$PROJECT_NAME/wwwroot/"

        Write-Host "🛠 Publishing .NET backend..."
        dotnet publish "$PROJECT_NAME" -c Release
    }

    Default {
        Write-Host "Usage: ./dev.ps1 [run|publish]"
        Write-Host "  run     - run frontend (Vite) and app in dev mode"
        Write-Host "  publish - build frontend and publish backend"
        exit 1
    }
}