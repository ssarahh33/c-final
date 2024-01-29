﻿using DGD203_2;
using System.Numerics;

public class Map
{
    private Game _theGame;

    private Vector2 _coordinates;

    private int[] _widthBoundaries;
    private int[] _heightBoundaries;

    private Location[] _locations;
    public npc _npc;
    public Game Game;

    public Map(Game game, int width, int height)
    {
        _theGame = game;

        int widthBoundary = (width - 1) / 2;

        _widthBoundaries = new int[2];
        _widthBoundaries[0] = -widthBoundary;
        _widthBoundaries[1] = widthBoundary;

        int heightBoundary = (height - 1) / 2;

        _heightBoundaries = new int[2];
        _heightBoundaries[0] = -heightBoundary;
        _heightBoundaries[1] = heightBoundary;

        _coordinates = new Vector2(0, 0);

        GenerateLocations();

        Console.WriteLine($"Created map with size {width}x{height}");
    }

    #region Coordinates

    public Vector2 GetCoordinates()
    {
        return _coordinates;
    }

    public void SetCoordinates(Vector2 newCoordinates)
    {
        _coordinates = newCoordinates;
    }

    #endregion

    #region Movement

    public void MovePlayer(int x, int y)
    {
        int newXCoordinate = (int)_coordinates[0] + x;
        int newYCoordinate = (int)_coordinates[1] + y;

        if (!CanMoveTo(newXCoordinate, newYCoordinate))
        {
            Console.WriteLine("You can't go that way Turn Back");
            return;
        }

        _coordinates[0] = newXCoordinate;
        _coordinates[1] = newYCoordinate;

        CheckForLocation(_coordinates);
    }

    private bool CanMoveTo(int x, int y)
    {
        return !(x < _widthBoundaries[0] || x > _widthBoundaries[1] || y < _heightBoundaries[0] || y > _heightBoundaries[1]);
    }

    #endregion

    #region Locations

    private void GenerateLocations()
    {
        _locations = new Location[5];

        Vector2 demonLocation = new Vector2(0, 2);
        Location demon = new Location("demon", "\nbig cave,a big demon shoud be inside", LocationType.Combat, demonLocation);
        _locations[0] = demon;

        Vector2 winterfellLocation = new Vector2(0, -2);
        Location winterfell = new Location("winterfell castle", "big castle but Not very welcoming people you ask about the dager many ignor you but one old man tell yo about a witch in the east who can help", LocationType.City, winterfellLocation);
        _locations[1] = winterfell;

        Vector2 witchLocation = new Vector2(1, -2);
        List<Item> witchItem = new List<Item>();
        witchItem.Add(Item.dager);
        Location witch = new Location("the old witch", "an old witch live in the woods with some items,maybe the dagger one of them?", LocationType.npc, witchLocation, witchItem);
        _locations[2] = witch;

        Vector2 casterlyrockLocation = new Vector2(2, 0);
        List<Item> casterlyrockItem = new List<Item>();
        casterlyrockItem.Add(Item.Coin);
        Location casterlyrock = new Location("casterlyrock", "big castle with a lot of gold,maybe i can have some who woukd know they are filthy rich", LocationType.City, casterlyrockLocation, casterlyrockItem);
        _locations[3] = casterlyrock;

        Vector2 kingslandingLocation = new Vector2(-2, 1);
        List<Item> kingslandingItem = new List<Item>();
        kingslandingItem.Add(Item.Rune);
        Location kingslanding = new Location("king's landing", "THE CAPITAL CITY,yet no one care", LocationType.City, kingslandingLocation, kingslandingItem);
        _locations[4] = kingslanding;
    }

    public void CheckForLocation(Vector2 coordinates)
    {
        Console.WriteLine($"You are now standing on {_coordinates[0]},{_coordinates[1]}");

        if (IsOnLocation(_coordinates, out Location location))
        {
            if (location.Type == LocationType.Combat)
            {
                Console.WriteLine("Prepare to fight!,throw the DAGER");
                Combat combat = new Combat(_theGame);
            }
            else if (location.Type == LocationType.City)
            {
                Console.WriteLine($"You are in {location.Name} {location.Type}");
                Console.WriteLine(location.Discription);

                if (HasItem(location))
                {
                    Console.WriteLine($"There is a {location.ItemsOnLocation[0]} here TAKE it");
                }
            }
            else if (location.Type == LocationType.npc)
            {
                Console.WriteLine("You are in the witch presence TALK to her.");
            }
        }
    }

    private bool IsOnLocation(Vector2 coords, out Location foundLocation)
    {
        for (int i = 0; i < _locations.Length; i++)
        {
            if (_locations[i].Coordinates == coords)
            {
                foundLocation = _locations[i];
                return true;
            }
        }
        foundLocation = null;
        return false;
    }

    private bool HasItem(Location location)
    {
        return location.ItemsOnLocation.Count != 0;
    }

    public void TakeItem(Player player, Vector2 coordinates)
    {
        if (IsOnLocation(coordinates, out Location location))
        {
            if (HasItem(location))
            {
                Item itemOnLocation = location.ItemsOnLocation[0];

                player.TakeItem(itemOnLocation);
                location.RemoveItem(itemOnLocation);

                Console.WriteLine($"You took the {itemOnLocation}");

                return;
            }
        }
        Console.WriteLine("There is nothing to take here!");
    }

    public void RemoveItemFromLocation(Item item)
    {
        for (int i = 0; i < _locations.Length; i++)
        {
            if (_locations[i].ItemsOnLocation.Contains(item))
            {
                _locations[i].RemoveItem(item);
            }
        }
    }

    #endregion
}