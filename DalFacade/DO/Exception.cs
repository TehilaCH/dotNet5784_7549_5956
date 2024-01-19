
namespace DO;
/// <summary>
/// Exception for an entity with an ID number that does not exist in the list
/// </summary>
[Serializable]
public class DalDoesNotExistException : Exception
{
    public DalDoesNotExistException(string? message) : base(message) { }
}

/// <summary>
/// Exception for an entity with an ID number that already exists in the list
/// </summary>
[Serializable]
public class DalAlreadyExistsException : Exception
{
    public DalAlreadyExistsException(string? message) : base(message) { }
}

/// <summary>
/// Exception for deleting an entity that should not be deleted
/// </summary>
[Serializable]
public class DalDeletionImpossible : Exception
{
    public DalDeletionImpossible(string? message) : base(message) { }
}
/// <summary>
/// name of an exception used with XML files
/// </summary>
public class DalXMLFileLoadCreateException : Exception
{
    public DalXMLFileLoadCreateException(string? message) : base(message) { }
}
