
public abstract class Instruction
{
    public Wall Wall { get; private set; }
    public Instruction(Wall wall){
        Wall = wall;
    }
    public abstract void Paint();
}
