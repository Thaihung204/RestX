@echo off
setlocal enabledelayedexpansion

set TOKEN=MkWcWfoH04QmUFcfIdvmQtp-HRuOASpZY2H6tZjY
set ZONE_ID=26cedf45e311cdd387ef740b3c97c0b1

set RECORD1_ID=20be7900f743f7b527a3b76f7eb761bf
set RECORD2_ID=5b6da5de8c8458d3b6b0f8929f42e3df
set RECORD3_ID=c38bf5c693a8110de74984eb1e406ed1

set RECORD1_NAME=restx.food
set RECORD2_NAME=restx
set RECORD3_NAME=www


for /f "delims=" %%i in ('curl -s https://api.ipify.org') do set IP=%%i
echo IP public hiện tại: %IP%


call :UpdateRecord %RECORD1_ID% %RECORD1_NAME%
call :UpdateRecord %RECORD2_ID% %RECORD2_NAME%
call :UpdateRecord %RECORD3_ID% %RECORD3_NAME%

echo Done
exit /b

:UpdateRecord
echo.
echo Updating record: %2
curl -X PUT "https://api.cloudflare.com/client/v4/zones/%ZONE_ID%/dns_records/%1" ^
     -H "Authorization: Bearer %TOKEN%" ^
     -H "Content-Type: application/json" ^
     --data "{\"type\":\"A\",\"name\":\"%2\",\"content\":\"%IP%\",\"ttl\":120,\"proxied\":true}"
echo.
exit /b
