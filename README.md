# BeatSaberMapsToMinecraftTrack
A Program to convert Beat Saber Custom Levels to a minecart track in minecraft. 
_Beat SaberMap version 2.0.0 is supported natively_

# Usage
## Generate all Commands
1. Start the program
2. Drag and drop info.dat of your song in
3. Then any difficulty file from the song
5.  press `y` if you want destroyable blocks (a bit like in the [BeatCraft Videos](https://www.youtube.com/watch?v=Wm0wFAJr1Xo)) and it'll start generating and should finish within about a few seconds.
6.  Finally fill in where the finished file should be and hit enter to save it to disk.

## Place all blocks
1. [Start/Setup a minecraft 1.13 (and up) spigot server](https://www.spigotmc.org/wiki/spigot-installation/) with a flat world (in server.properties change `level-type` to `flat`).
2. Add the CommandExecutor plugin (which is under the [releases section](https://github.com/ComputerElite/BeatSaberMapsToMinecraftTrack/releases)) to your server. (You do this by copying the jar file to the plugins folder next to your server)
3. Go to the coordinates 0, 5, 0 (this is the origin of the world where the track will be placed)
4. Open the chat and type the command: `/bs [path]` where you replace `[path]` with the location of the file you generated earlier under [Generate all Commands](#generate-all-commands) and wait until the plugin says you it has finished (please note that this will spam your server console and chat if you are op a lot)
5. Enjoy your ride

## Install the ressource pack
**Based onthe PureBDCraft texture pack from [bdcraft.net](https://bdcraft.net/)**
1. Get the ressource pack from the [releases section](https://github.com/ComputerElite/BeatSaberMapsToMinecraftTrack/releases)
2. copy it to `%appdata%\.minecraft`
3. then select it in-game
_without the ressource pack it might look wierd_
