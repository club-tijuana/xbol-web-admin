namespace Odasoft.XBOL.AdminPortal.Helpers;

public class FieldBinding
{
    private readonly string _fieldName;
    private readonly FormContext _context;

    internal FieldBinding(string fieldName, FormContext context)
    {
        _fieldName = fieldName;
        _context = context;
    }

    public Func<T, IEnumerable<string>> ValidationFor<T>() => _context.ValidationFor<T>(_fieldName);

    public void OnChanged() => _context.ClearFieldError(_fieldName);
}
