# Stickman-Ragdoll-Tutorial
 
**This is the code for my YouTube video: *How to Make an Active 2D Stickman Ragdoll in Unity | Balance, Run, Jump***

https://youtu.be/q_enFap8Pr8
 
In this video, I go over from start to finish making an active 2D ragdoll in Unity. I teach you how to make the ragdoll character in Photoshop, import and rig the character in Unity, and script the run, jump, and balance. Enjoy!

This Unity project is licensed under the MIT License, read more about usability under in the LICENSE file: https://github.com/craftyclawboom/Stickman-Ragdoll-Tutorial/blob/82c8ff8f4606540adfeac0100f2eac17211e713f/LICENSE

## Database

The project stores progress in a small SQLite database created in
`Application.persistentDataPath` at runtime. The schema can be found in
[`Database/schema.sql`](Database/schema.sql).

### Schema overview

- **Players** – list of players. The demo uses a single player with ID `1`.
- **Skins** – all available skin keys.
- **PlayerProgress** – one-to-one relation with `Players`, holding current
  money and last unlocked level for each player.
- **PlayerSkins** – links a player with a skin. Stores whether the skin is
  opened and if it is currently chosen. References both `Players` and `Skins`
  via foreign keys.

These relations allow multiple players to share the same skin catalog while
keeping individual progress separate.
