
public abstract class Function
{
    public Wall Wall { get; private set; }
    public Function(Wall wall){
        Wall = wall;
    }
    public abstract int Evaluate();
}