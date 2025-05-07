

public abstract class Command
{
    public Wall Wall { get; private set; }
    public Command(Wall wall){
        Wall = wall;
    }
    public abstract void Order();
}