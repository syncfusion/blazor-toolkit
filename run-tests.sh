#!/bin/bash
# Local test runner that bypasses the root-owned /etc/dotnet apphost registration
# by invoking vstest.console.dll through the local muxer (~/.dotnet) which has the net8 runtime.
set -e
cd "$(dirname "$0")"
export DOTNET_ROOT="$HOME/.dotnet"
export DOTNET_MULTILEVEL_LOOKUP=0

TEST_PROJ="tests/Syncfusion.Blazor.Toolkit.BUnitTest/Syncfusion.Blazor.Toolkit.BUnitTest.csproj"
TEST_DLL="tests/Syncfusion.Blazor.Toolkit.BUnitTest/bin/Debug/net8.0/Syncfusion.Blazor.Toolkit.BUnitTest.dll"

echo "Building test project (net8.0)..."
"$HOME/.dotnet/dotnet" build "$TEST_PROJ" -c Debug -f net8.0 --nologo -v q

echo "Running tests..."
"$HOME/.dotnet/dotnet" exec "$HOME/.dotnet/sdk/10.0.401/vstest.console.dll" "$TEST_DLL"
