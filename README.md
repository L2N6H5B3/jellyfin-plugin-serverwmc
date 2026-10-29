# Jellyfin Plugin for Windows MediaCenter with ServerWMC

[![](https://img.shields.io/github/languages/top/L2N6H5B3/jellyfin-plugin-serverwmc.svg)](https://github.com/L2N6H5B3/jellyfin-plugin-serverwmc)
[![](https://img.shields.io/github/contributors/L2N6H5B3/jellyfin-plugin-serverwmc.svg)](https://github.com/L2N6H5B3/jellyfin-plugin-serverwmc)
[![](https://img.shields.io/github/license/L2N6H5B3/jellyfin-plugin-serverwmc.svg)](https://github.com/L2N6H5B3/jellyfin-plugin-serverwmc)
[![](https://img.shields.io/github/v/release/L2N6H5B3/jellyfin-plugin-serverwmc.svg)](https://github.com/L2N6H5B3/jellyfin-plugin-serverwmc/releases/latest)

Live TV, the program guide, scheduled recordings and Recorded TV in Jellyfin, from a Windows Media Center machine running [ServerWMC](https://serverwmc.github.io/).

The original plugin was archived after Jellyfin 10.7. This fork ports it to **Jellyfin 12.3 and later** on .NET 10.

## Features

- Stream Live TV
- Stream recorded programs
- Program guide
- Manage recordings and series recordings

## Compatibility

| Plugin version | Jellyfin version |
|---|---|
| 8.0.0.0 | 12.3 and newer |
| 6.0.0.0 (original, archived) | 10.7 |

## Prerequisites

- **[Jellyfin](https://jellyfin.org/) Server 12.3 or newer.**
- **Windows Media Center**, installed and configured with your tuners.
- **[ServerWMC](https://serverwmc.github.io/) build 1123 or newer**, running on the WMC machine. WMC itself doesn't need to be open.
- If Jellyfin and ServerWMC are on different machines, Jellyfin must be able to reach the WMC **Recorded TV** folder over the network (see the "Folders" tab in ServerWMC).

## Installation

This plugin isn't in the official Jellyfin plugin catalog, so you install it manually.

1. **Download** `Jellyfin.Plugin.ServerWMC.dll` from the [latest release](https://github.com/L2N6H5B3/jellyfin-plugin-serverwmc/releases/latest).

2. **Stop** Jellyfin Server.

3. **Remove any older ServerWMC plugin.** In the plugins folder (see the table below), delete any existing `ServerWMC_*` folder.

4. **Create a folder** named `ServerWMC_<version>` (for example `ServerWMC_8.0.0.0`) in the plugins folder and copy the `.dll` into it:

   | Install type | Plugins folder |
   |---|---|
   | Windows (installer, runs as a service) | `C:\ProgramData\Jellyfin\Server\plugins` |
   | Windows (tray app or portable) | `%LocalAppData%\jellyfin\plugins` |
   | Linux (Debian/Ubuntu/Fedora packages) | `/var/lib/jellyfin/plugins` |
   | Docker: official `jellyfin/jellyfin` image | `/config/plugins` |
   | Docker: `binhex/arch-jellyfin` or `linuxserver/jellyfin` | `/config/data/plugins` |
   | macOS | `~/.local/share/jellyfin/plugins` |

   For Docker, the path is inside the container. Put the folder under the host directory you mapped to `/config`.

   If none of these match your setup, open **Dashboard → Logs**. The startup log prints `Program data path`, and the plugins folder is `plugins` inside it.

   On Linux and Docker, make sure the user Jellyfin runs as can read the folder. For Docker, use the container's `PUID:PGID`, for example `99:100` on Unraid:

   ```bash
   sudo chown -R jellyfin:jellyfin /var/lib/jellyfin/plugins/ServerWMC_8.0.0.0
   ```

5. **Start** Jellyfin Server. **ServerWMC** should be listed as *Active* under **Dashboard → Plugins**.

## Configuration

1. Go to **Dashboard → Plugins → ServerWMC**.
2. Fill in the settings:
   - **ServerWMC IP/Name:** the address of the WMC machine. Use `localhost` only if Jellyfin runs directly on that machine. **In Docker, always use the WMC machine's LAN IP**, because `localhost` means the container itself.
   - **ServerWMC Port:** `9080` unless you changed it in ServerWMC.
   - **Linux 'Recorded TV' path:** Linux and Docker only. Mount WMC's Recorded TV share and enter the local mount path, for example `/mnt/recordedtv`. For Docker, mount the share on the host, map it into the container (e.g. `-v /mnt/remotes/WMC_RecordedTV:/recordedtv`), and enter the path inside the container (`/recordedtv`).
3. Click **Save**. If saving fails, check that ServerWMC is running and that the Jellyfin server can reach that IP and port. Firewalls are a common cause.
4. Go to **Dashboard → Scheduled Tasks** and run **Refresh Guide** to load channels and the program guide right away.

Live TV channels, the guide and recording timers appear under **Live TV**. Existing recordings show up in the **ServerWMC Recordings** channel.

## Troubleshooting

- **No channels after Refresh Guide:** check **Dashboard → Logs** for lines starting with `[ServerWMC]`.
  - "ServerWMC is down" means Jellyfin can't reach ServerWMC.
  - "ServerWMC needs to be updated" means you need a newer ServerWMC build.
  - "Unable to resolve ServerWMC address" means the IP or name in the plugin settings is wrong.
- **Recordings won't play:** look for a `Recorded TV folder ... not accessible` warning in the log. On Windows, the Recorded TV folder must be a network share set in ServerWMC's "Folders" tab. On Linux and Docker, check the mount and the Linux path setting.
- **Plugin not listed:** confirm your Jellyfin version is supported (see [Compatibility](#compatibility)) and that the `.dll` is inside its own `ServerWMC_<version>` folder, not loose in `plugins`.

### Known limitations

- Jellyfin no longer shows tuner status or "update available" on the Live TV page for plugins, so that information isn't shown anymore.

## Build Instructions

Requires the [.NET 10 SDK](https://dotnet.microsoft.com/download/dotnet/10.0).

```
dotnet publish Jellyfin.Plugin.ServerWMC/Jellyfin.Plugin.ServerWMC.csproj --configuration Release --output bin
```

The plugin is `bin/Jellyfin.Plugin.ServerWMC.dll`. That's the only file you need to install.

Special thanks to **KrustyReturns** for initial development and permission to port this plugin to Jellyfin.
