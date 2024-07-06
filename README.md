# Unity Floor Tiling

This system will create floor tile at a certain radius keeping the player at the center. It will update the floor as the player moves creating new tiles or deleting them as needed.
I have used this system in the game [Boat Blitz](https://play.google.com/store/apps/details?id=com.novalabs.bb&pcampaignid=web_share).

<b>Remainder:</b> The code is not very efficient. Keeping the Spawn Radius below 100 is recommended. 
The less the better. 

Two more system were also created along with to work better. 

## Pooling System
Not something new, but effective with system to work. The pooling system will create a fix amount of tiles at
the start of game and will create more if need arise.

## Prefab Light baking System
<b>Not my creation.</b> I was looking for a way to bake the light for my game [Boat Blitz](https://play.google.com/store/apps/details?id=com.novalabs.bb&pcampaignid=web_share) on prefabs.
While searching on github. I found just what I needed for my game. Find more details here : [Prefab Light mapping](https://github.com/Ayfel/PrefabLightmapping).

https://github.com/FahimKamal/Unity_Floor_Tiling/assets/47342396/5ce34894-9dbd-43c1-93d2-1d88af06a8b9

