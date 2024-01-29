﻿using System.Numerics;

namespace DGD203_2
{
    public class Game
    {
        #region VARIABLES
        #region Game Constants

        private const int _defaultMapWidth = 5;
        private const int _defaultMapHeight = 5;

        #endregion

        #region Game Variables

        #region Player Variables

        public Player Player { get; set; }
        public Inventory Inventory { get; set; }
        public npc _npc { get; set; }

        private string _playerName;
        private List<Item> _loadedItems;
        public bool _haswon = false;

        #endregion

        #region World Variables

        private Location[] _locations;
        public Vector2 dagerLocation = new Vector2(1, -2);

        #endregion

        public bool _gameRunning;
        public Map _gameMap;
        private string _playerInput;
        public string _Answer;
        public bool canTalk = false;
        public bool canTake = true;

        #endregion

        #endregion

        #region METHODS

        #region Initialization

        public void StartGame(Game gameInstanceReference)
        {
            CreateNewMap();

            LoadGame();

            CreatePlayer();

            _npc = new npc(_Answer, new Vector2(1, -2), canTake);

            InitializeGameConditions();

            _gameRunning = true;

            StartGameLoop();
        }

        private void CreateNewMap()
        {
            _gameMap = new Map(this, _defaultMapWidth, _defaultMapHeight);
        }

        private void CreatePlayer()
        {
            if (_playerName == null)
            {
                GetPlayerName();
            }

            Player = new Player(_playerName, _loadedItems);
            Console.WriteLine("the player HP is:" + Player.Health.ToString());
        }

        private void GetPlayerName()
        {
            Console.WriteLine("Welcome Hero to this mighty quest !");
            Console.WriteLine("can you give me your name?");
            _playerName = Console.ReadLine();

            Console.WriteLine($"Pleased to meet you {_playerName}, we will have a great adventure together!,your mission is to enter the area and reach the demon and kill him with the obsidian dager, to find it you need to go to winterfell in the deep south from there you can continue by yourself good luck...\n type 'help' to see the available commands");
        }

        private void InitializeGameConditions()
        {
            _gameMap.CheckForLocation(_gameMap.GetCoordinates());
        }

        #endregion

        #region Game Loop

        private void StartGameLoop()
        {
            while (_gameRunning)
            {
                GetInput();
                ProcessInput();
                npcstart();

                if (Player.Health == 0)
                {
                    PlayerDied();
                }
            }
        }

        public void PlayerDied()
        {
            Console.WriteLine("dead??!!!,dont let that stop you pick up your dagger and try again soldir");
            exiting();
        }

        private void GetInput()
        {
            _playerInput = Console.ReadLine();
        }

        public void npcstart()
        {
            if (_gameMap.GetCoordinates() == new Vector2(1, -2))
            {
                canTalk = true;
            }
            else
            {
                canTalk = false;
            }
        }

        private void ProcessInput()
        {
            if (_playerInput == "" || _playerInput == null)
            {
                Console.WriteLine("Give me a command!");
                return;
            }

            switch (_playerInput)
            {
                case "N":
                    _gameMap.MovePlayer(0, 1);
                    break;
                case "S":
                    _gameMap.MovePlayer(0, -1);
                    break;
                case "E":
                    _gameMap.MovePlayer(1, 0);
                    break;
                case "W":
                    _gameMap.MovePlayer(-1, 0);
                    break;
                case "exit":
                    exiting();
                    break;
                case "save":
                    SaveGame();
                    Console.WriteLine("Game saved");
                    break;
                case "load":
                    LoadGame();
                    Console.WriteLine("Game loaded");
                    break;
                case "help":
                    Console.WriteLine(HelpMessage());
                    break;
                case "where":
                    _gameMap.CheckForLocation(_gameMap.GetCoordinates());
                    break;
                case "clear":
                    Console.Clear();
                    break;
                case "who":
                    Console.WriteLine($"You are {Player.Name}, the one and only");
                    break;
                case "take":
                    if (_gameMap.GetCoordinates() == _npc.npccoordinates)
                    {
                        if (!_npc.cantake)
                        {
                            _gameMap.TakeItem(Player, _gameMap.GetCoordinates());
                        }
                        else
                        {
                            Console.WriteLine("Cant take the bomb");
                        }
                    }
                    else
                        _gameMap.TakeItem(Player, _gameMap.GetCoordinates());
                    break;
                case "talk":
                    if (_gameMap.GetCoordinates() == _npc.npccoordinates)
                    {
                        _npc.talk();
                    }
                    break;
                case "dager":
                    if (_gameMap.GetCoordinates() == new Vector2(0, 2))
                    {
                        if (Player.playerHasdager(Item.dager))
                        {
                            Player.playerHasdager(Item.dager);
                            Console.WriteLine("good job, you killed the demon now these lands can have some peace");
                            exiting();
                        }
                        else
                        {
                            PlayerDied();
                        }
                    }
                    else
                    {
                        Console.WriteLine("Command not recognized. Please type 'help' for a list of available commands");
                    }
                    break;
                case "bag":
                    Player.CheckInventory();
                    break;
                default:
                    Console.WriteLine("Command not recognized. Please type 'help' for a list of available commands");
                    break;
            }
        }

        public void exiting()
        {
            Console.WriteLine("goodbay");
            _gameRunning = false;
        }

        #endregion

        #region Save Management

        private void LoadGame()
        {
            string path = SaveFilePath();

            if (!File.Exists(path)) return;

            // Reading the file contents
            string[] saveContent = File.ReadAllLines(path);

            // Set the player name
            _playerName = saveContent[0];

            // Set player coordinates
            List<int> coords = saveContent[1].Split(',').Select(int.Parse).ToList();
            Vector2 coordArray = new Vector2(coords[0], coords[1]);

            // Set player inventory
            _loadedItems = new List<Item>();

            List<string> itemStrings = saveContent[2].Split(',').ToList();

            for (int i = 0; i < itemStrings.Count; i++)
            {
                if (Enum.TryParse(itemStrings[i], out Item result))
                {
                    Item item = result;
                    _loadedItems.Add(item);
                    _gameMap.RemoveItemFromLocation(item);
                }
            }

            _gameMap.SetCoordinates(coordArray);

        }

        private void SaveGame()
        {
            // Player Coordinates
            string xCoord = _gameMap.GetCoordinates()[0].ToString();
            string yCoord = _gameMap.GetCoordinates()[1].ToString();
            string playerCoords = $"{xCoord},{yCoord}";

            // Player inventory
            List<Item> items = Player.Inventory.Items;
            string playerItems = "";
            for (int i = 0; i < items.Count; i++)
            {
                playerItems += items[i].ToString();

                if (i != items.Count - 1)
                {
                    playerItems += ",";
                }
            }

            string saveContent = $"{_playerName}{Environment.NewLine}{playerCoords}{Environment.NewLine}{playerItems}";

            string path = SaveFilePath();

            File.WriteAllText(path, saveContent);
        }

        private string SaveFilePath()
        {
            // Get the save file path
            string workingDirectory = Environment.CurrentDirectory;
            string projectDirectory = Directory.GetParent(workingDirectory).Parent.Parent.FullName;
            string path = projectDirectory + @"\save.txt";

            return path;
        }

        #endregion

        #region Miscellaneous

        private string HelpMessage()
        {
            return @"Here are the current commands:
N: go north
S: go south
W: go west
E: go east
where: find your current location
who: what is your name
Clear: clear the screen
dager: to throw the dager on the demon
take: take an item 
talk: talk with an NPC
bag: see what items you have
load: Load saved game
save: save current game
exit: exit the game";
        }
        #endregion

        #endregion
    }
}