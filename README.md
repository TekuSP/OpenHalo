# OpenHalo
- Mellow Fly Halo replacement firmware, Open-Source.
- Current state of the firwmare is NOT DONE.
- If you have any ideas how to improve the screens and what to add to them, open an Issue.

[![Build Nanoframework](https://github.com/TekuSP/OpenHalo/actions/workflows/nanoframework_build.yml/badge.svg?branch=master)](https://github.com/TekuSP/OpenHalo/actions/workflows/nanoframework_build.yml)
[![Quality Gate Status](https://sonarcloud.io/api/project_badges/measure?project=TekuSP_OpenHalo&metric=alert_status)](https://sonarcloud.io/summary/new_code?id=TekuSP_OpenHalo)
---
 ![image](https://github.com/TekuSP/OpenHalo/assets/13198444/df67db7e-9a60-4000-8a0f-c8efe350ac0d)
 ![image](https://github.com/TekuSP/OpenHalo/assets/13198444/383a31a1-ae9c-4b33-806c-89166bdfe481)
 ![image](https://github.com/TekuSP/OpenHalo/assets/13198444/3da6add0-58e6-43df-a21e-c68618b3a21f)
 ![image](https://github.com/TekuSP/OpenHalo/assets/13198444/48b68e8d-33e9-4129-ad90-e2350c2a8049)
 ![image](https://github.com/TekuSP/OpenHalo/assets/13198444/6eb68406-c70a-4abb-bd43-35565a1cc5c2)
---
# Collaborating
- Project runs on nanoframework, get it here: https://docs.nanoframework.net/content/getting-started-guides/getting-started-managed.html
- Download latest release of firmware from here: https://github.com/TekuSP/nf-interpreter/releases/tag/halo_graphics_v1
- Flash the firmware using `nanoff --clrfile nanoCLR.bin --serialport COM7 --update --masserase`
- Flash current deployment code (What you get from build in `bin\Debug\`) using `nanoff --target ESP32_S3_BLE --serialport COM7 --deploy --image .\OpenHalo.bin`
- Run Visual Studio or Visual Studio Code with Extension and upload source code to deployment area.
- WARNING: There is 15 sec delay at startup which is for debugger, so it has time to attach. If you want quick starts, you can remove it.
- WARNING: Currently it cannot be setup via Web Portal, as HTML files are not finished. You can run `            ConfigHelper.SaveConfig(new MainConfig() { MoonrakerApiKey = "", MoonrakerUri = "", Wifis = new Wifi[] { new Wifi() { Hidden = true, Password = , SSID =} } });` on line 112 with your values once, then remove it and it should always read your config. To escape from Web Portal Setup with QR Code, Long touch the touchscreen
