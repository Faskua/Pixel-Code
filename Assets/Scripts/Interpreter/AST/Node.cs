

public abstract class ASTNode{
    public IDType Type { get; set; }
    public CodeLocation Location { get; protected set; }
    public ASTNode(IDType type, CodeLocation location){
        Type = type;
        Location = location;
    }
    public abstract bool Validate(Global Global);
    public bool CheckType(IDType type) => Type == type;
}



