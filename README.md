# Pressure Test Drop Calculator - 2 Input Windows Version

Inputs:
1. Test Pressure (psi)
2. Test Duration (minutes)

The application interpolates seconds per psi from the reference pressure table.

Reference:
500 psi = 240 sec/psi
1,000 psi = 120 sec/psi
3,000 psi = 40 sec/psi
5,000 psi = 24 sec/psi
10,000 psi = 12 sec/psi
15,000 psi = 8 sec/psi

The allowable-drop percentage is calculated at 0.05% per minute, consistent with the supplied table:
3 min = 0.15%
5 min = 0.25%
10 min = 0.50%
15 min = 0.75%
60 min = 3.00%

Build:
dotnet publish -c Release -r win-x64 --self-contained true /p:PublishSingleFile=true
