using DGD203_2;
using System.Numerics;

public class npc
{
    public Player Player { get; set; }
    public Game Game { get; set; }
    public Map Map { get; set; }
    public Location Location { get; set; }
    public Inventory Inventory { get; set; }

    public string answer;
    public Vector2 npccoordinates;
    public string name;
    public bool cantake = true;
    public int heatlh;

    public npc(string answer, Vector2 npclocation, bool cantake)
    {
        this.answer = answer;
        npccoordinates = npclocation;
        this.cantake = cantake;
    }

    public void talk()
    {
        Player = new Player(name, null);
        Inventory = new Inventory();
        Game = new Game();
        Map = new Map(Game, 5, 5);
        heatlh = Player.Health;
        Console.WriteLine("huh, who is this,who dare to inter the witch cave,if you enter here you cant get out easly,you want the dager??,well i will give you this riddle solve it and its your but if you fail you will pay for interrupting my sleeping");
        question();
        while (cantake)
        {
            getInput();
            handleInput();
        }
    }

    public void question()
    {
        Console.WriteLine("so,my riddle is,\n\nI am easy to lift, but hard to throw. What am I\n1.a feather\n2.a Cotton ball\n3.a Bubble\n4.a Puff of smoke\n5.a Cloud");
    }

    public void getInput()
    {
        answer = Console.ReadLine();
    }

    public void handleInput()
    {
        if (answer != null)
        {
            if (heatlh > 0)
            {
                if (cantake)
                {
                    switch (answer)
                    {
                        case "1":
                            Console.WriteLine("hmmm,fine here is your dager now flip off here");
                            Player.playerHasdager(Item.dager);
                            cantake = false;
                            break;
                        case "2":
                            if (heatlh == 100)
                            {
                                Console.WriteLine("false you have one chance left");
                                Console.WriteLine("Hint:Its used by writers in the past");
                            }
                            else if (heatlh == 50)
                            {
                                Console.WriteLine("false you have a last chance left");
                            }
                            break;
                        case "3":
                            if (heatlh == 100)
                            {
                                Console.WriteLine("false you have one chance left");
                                Console.WriteLine("Hint:Its used by writers in the past");
                            }
                            else if (heatlh == 50)
                            {
                                Console.WriteLine("false you have a last chance left");
                            }
                            break;
                        case "4":
                            if (heatlh == 100)
                            {
                                Console.WriteLine("false you have one chance left");
                                Console.WriteLine("Hint:Its used by writers in the past");
                            }
                            else if(heatlh == 50)
                            {
                                Console.WriteLine("false you have a last chance left");
                            }
                            break;
                        case "5":
                            if (heatlh == 100)
                            {
                                Console.WriteLine("false you cant throw a cloud genius");
                                Console.WriteLine("Hint:Its used by writers in the past");
                            }
                            else if (heatlh == 50)
                            {
                                Console.WriteLine("false you have a last chance left");
                            }
                            break;
                        default:
                            Console.WriteLine("wrong input!!");
                            break;
                    }
                    heatlh = heatlh - 50;
                }
            }
            else
            {
                Game.PlayerDied();
                exit();
            }
        }
    }
    public void exit()
    {
        Environment.Exit(0);
    }


}
