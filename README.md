
# PokemonLike

PokemonLike is a turn based Poke-like game where we have a monster
and a random enemy



## Features

- Sql Server connection
    - Automatic DataBase Creation
    - DataBase is recreated at every db connection (comment line 35 (`_ = new Init();`) in SettingsViewVM.cs to avoid db recreation) 
- Auto Connect to test account by double clicking Enter on MainMenu (after connecting the db)
- Login
    - Can Be Done with two Enter clicks
    - Has the default test:test account
    - Can Check the password with the eye icon
    - Or clear the boxes with the x
- Monsters List
    - Work offline (= without login)
    - Have Pokemon details in the List
    - Can create a player with a popup dialog
    - Have multiples players = Have 1 Monster per Player
    - Choose a player to start a battle
    - If the player have a Monster Button "Go to Game" become visible
    - Left Arrow button : back to Main MainMenu
    - Sync Arrows Icon : Change Monster
- Spells List
    - A searchbar at the top right when writing suggestions popup
    - Details card at the left that can be flipped to see the Spell Description
- Game
    - turn by turn Game
    - Score that count the number of defeated Ennemies
    - Spell Damage apear if the mouse hover a spell (Tooltip)
    - After killing an enemy click any spell to generate another enemy
    - Note: Some Spells Have 0 Damage
    - Enemy Damage = Spell Damage x ( Score + 1 )
## Run Locally

Clone the project

```bash
  git clone git@github.com:yomarhub/Pokemon-Like.git
```

Go to the project directory

```bash
  cd Pokemon-Like
```

Open The Project file with VisualStudio

```bash
  PokeLike.sln
```

Run the program

## Note

It can take some time to fetch the pokemon list that's normal
## Authors

- [@yomarhub](https://www.github.com/yomarhub)

